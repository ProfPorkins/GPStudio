/*
Copyright (c) 2018 James Dean Mathias

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using GPStudio.Shared;

namespace GPStudio.Client
{
	partial class fmProject
	{

		private void lvProfilesModeling_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// Update the UI controls based upon item selection status
			if (lvProfilesModeling.SelectedIndices.Count > 0)
			{
				//
				// Only enable the button if no modeling is in process---that can be
				// detected if the cancel button is enabled
				btnModel.Enabled = !btnCancel.Enabled;
			}
			else
			{
				btnModel.Enabled = false;
			}
		}

		/// <summary>
		/// The current profile that is being modeled
		/// </summary>
		GPProjectProfile m_ModelProfile;
		private void btnModel_Click(object sender, EventArgs e)
		{
			//
			// Check to see if any batch processes are active, if so, let the user know this isn't
			// a spectacularly great idea.
			if (fmMain.BatchManager.ActiveSimulation)
			{
				String Msg = "Batch processing is active, it is not recommended to run interactive simulations at the same time, ";
				Msg += "because all simulations share the same CPU resources, making the interactive simulation run much slower.  ";
				Msg += "Would you like to continue anyway?";
				if (MessageBox.Show(Msg, GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}

			//
			// Disable the modeling button right away!
			btnModel.Enabled = false;

			if (!CanModel(ref m_ModelProfile))
			{
				btnModel.Enabled = true;
				return;
			}

			//
			// Reset the list of distributed servers
			cmbPopulationFitnessServers.Items.Clear();

			//
			// Prepare various UI controls for the start of modeling
			btnProfileDelete.Enabled = false;
			pgModeling.Enabled = true;
			pgModeling.Maximum = (m_ModelProfile.m_maxNumber + 2);
			pgModeling.Value = 1;

			//
			// Reset the fitness graph
			InitializeFitnessGraph();
			InitializePopComplexityGraph();
			InitializePopFitnessGraph();

			//
			// Thread the modeling
			m_ThreadModeling = new Thread(new ParameterizedThreadStart(ModelThread));
			m_ThreadModeling.Start(m_ModelProfile);
		}

		private void tsModelBatch_Click(object sender, EventArgs e)
		{
			GPProjectProfile Profile=null;
			//
			// Validate we can move forward with processing
			if (!CanModel(ref Profile))
			{
				return;
			}
			//
			// Add this to the Batch Processing Manager.  This is a synchronous
			// function call to add the simulation.  The batch processor will
			// place it into a queue and run the simulation asynchronously...in short,
			// this does not block.
			fmMain.BatchManager.AddSimulation(Profile, m_Project.DataTrainingID,m_Project.DBCode);
		}

		/// <summary>
		/// Validates whether or not the modeling should proceed.
		/// </summary>
		/// <param name="Profile">If the profile is valid, reference to the modeling profile</param>
		/// <returns>true/false depending on whether the model is validated for simulation</returns>
		private bool CanModel(ref GPProjectProfile Profile)
		{
			//
			// Ensure a profile is selected
			if (lvProfilesModeling.SelectedIndices.Count == 0)
			{
				MessageBox.Show("Please select a modeling profile", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			//
			// Download the profile from the database
			Profile = new GPProjectProfile();
			Profile.LoadFromDB((int)lvProfilesModeling.SelectedItems[0].Tag);

			//
			// If the project doesn't have training data, don't start the modeling
			if (!(m_Project.DataTrainingID > 0))
			{
				MessageBox.Show("Select a training data set before modeling", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
				return false;
			}

			switch (Profile.ValidateForModeling())
			{
				case GPProjectProfile.Validation.FunctionSet:
					MessageBox.Show("Please select functions for modeling", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
					return false;
				case GPProjectProfile.Validation.Probability:
					MessageBox.Show("Ensure the Population Operators total 100", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
					return false;
			}

			return true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			btnCancel.Enabled = false;
			UpdateModelStatus("Canceling...");
			Application.DoEvents();
			//
			// Stop the Modeling
			m_Modeler.Abort();
		}

		Thread m_ThreadModeling;
		GPModeler m_Modeler;
		/// <summary>
		/// The entry point for a modeling thread.  This connects to the server,
		/// sends the modeling profile, training Training and then requests the model
		/// to be written.
		/// </summary>
		/// <param name="arg">Reference to the modeling profile</param>
		private void ModelThread(Object arg)
		{
#if GPLOG
			GPLog.ReportLine("Requesting a program.",true);
#endif
			//
			// Create the modeler object - the parameter to this thread is
			// the model profile.
			m_Modeler = new GPModeler(
				(GPProjectProfile)arg,
				m_Project.DataTrainingID,
				new GPModeler.DEL_ValidatedServer(AddValidatedServer),
				new GPModeler.DEL_ReportStatus(UpdateModelStatus),
				new GPModeler.DEL_ReportFitness(ReportFitness),
				new GPModeler.DEL_GenerationComplete(GenerationComplete));

			//
			// Make an asynchronous call that gets the modeling started
			MethodInvoker miModeler = new MethodInvoker(m_Modeler.RequestProgram);
			IAsyncResult ar=miModeler.BeginInvoke(null, null);

			//
			// Now we can go ahead and enable the cancel button
			EnableCancel();

			//
			// Wait for the modeling to complete
			miModeler.EndInvoke(ar);

#if GPLOG
			GPLog.ReportLine("Program request complete.",true);
#endif

			//
			// Save the best program to the database
			int ProgramID = 0;
			if (m_Modeler.SaveBestToDB(m_Project.DBCode, ref ProgramID))
			{
				//
				// Update the results display and select this program
				UpdateModelStatus("Saving best program...");
				UpdateModelResults(ProgramID);
			}

			//
			// Do some cleanup, now that the modeling is complete
			EnableModelButton();
			ProgressReset();
			UpdateModelStatus("");
			EnableStatusMarquee(false);
		}

		/// <summary>
		/// Private member used to hold the statistics generated by the modeling
		/// </summary>
		private ModelingResults m_ServerData;

		/// <summary>
		/// Used to report the last generation stats for each server
		/// </summary>
		/// <param name="Generation">Generation of thedata</param>
		/// <param name="Server">Index of the server being reported</param>
		/// <param name="Stats">The actual data</param>
		public void ReportFitness(int Generation, int Server, ModelingResults.ServerData Stats)
		{
			//
			// Record this data in the results object
			m_ServerData.ReportServerData(Generation,Server, Stats);
		}

		/// <summary>
		/// Update the UI with the current modeling status
		/// </summary>
		public void GenerationComplete()
		{
			//
			// Obtain the most recent fitness values
			int BestServer = m_ServerData.BestServer(m_ServerData.Generations-1);
			double Fitness = m_ServerData[BestServer, m_ServerData.Generations - 1].BestFitness;
			int Hits = m_ServerData[BestServer, m_ServerData.Generations - 1].BestHits;

			m_ptListFitness.Add(m_ServerData.Generations, Fitness);
			m_ptListHits.Add(m_ServerData.Generations, (double)Hits);
			graphFitness.GraphPane.AxisChange(this.CreateGraphics());

			//
			// Display fitness for the currently selected server
			// Don't allow the ave/max fitness to be more than 5/3 times as big
			// as the min fitness, because it really screws up the graph.
			double FitnessMin = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].FitnessMinimum;
			double FitnessMax = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].FitnessMaximum;
			double FitnessAve = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].FitnessAverage;
			FitnessMax = Math.Min(FitnessMax,FitnessMin*5);
			FitnessAve = Math.Min(FitnessAve,FitnessMin*3);

			m_ptListPopFitnessMin.Add(m_ServerData.Generations, FitnessMin);
			m_ptListPopFitnessMax.Add(m_ServerData.Generations, FitnessMax);
			m_ptListPopFitnessAve.Add(m_ServerData.Generations, FitnessAve);
			graphPopulationFitness.GraphPane.AxisChange(this.CreateGraphics());

			//
			// Obtain the most recent population complexity values- use data from the 
			// selected server
			int ComplexityMin = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].ComplexityMinimum;
			int ComplexityMax = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].ComplexityMaximum;
			int ComplexityAve = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].ComplexityAverage;
			int ComplexityBest = m_ServerData[m_ModelingSelectedServer, m_ServerData.Generations - 1].BestComplexity;

			m_ptListComplexityMin.Add(m_ServerData.Generations, ComplexityMin);
			m_ptListComplexityMax.Add(m_ServerData.Generations, ComplexityMax);
			m_ptListComplexityAve.Add(m_ServerData.Generations, ComplexityAve);
			m_ptListComplexityBestOf.Add(m_ServerData.Generations, ComplexityBest);
			graphPopulationComplexity.GraphPane.AxisChange(this.CreateGraphics());

			graphFitness.Invalidate();
			graphPopulationComplexity.Invalidate();
			graphPopulationFitness.Invalidate(); 

			//
			// Update the generation progress bar
			ProgressIncrement();
		}

		private int m_ModelingSelectedServer;
		/// <summary>
		/// Thread safe technique to add a new validated server to the modeling UI
		/// </summary>
		/// <param name="Text">Name of the server</param>
		private void AddValidatedServer(String Text)
		{
			if (this.cmbPopulationFitnessServers.InvokeRequired)
			{
				GPModeler.DEL_ValidatedServer d = new GPModeler.DEL_ValidatedServer(AddValidatedServer);
				this.Invoke(d,new object[] { Text });
			}
			else
			{
				this.cmbPopulationFitnessServers.Items.Add(Text);
				if (this.cmbPopulationFitnessServers.Items.Count == 1)
				{
					//
					// For the first validated server, create the server results collection
					m_ServerData = new ModelingResults();

					this.cmbPopulationFitnessServers.SelectedIndex = 0;
					m_ModelingSelectedServer=0;
				}
				//
				// Add the new server to the results collection
				m_ServerData.AddServer();
			}
		}

		private void cmbPopulationFitnessServers_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_ModelingSelectedServer = cmbPopulationFitnessServers.SelectedIndex;
			//
			// Check to see if there is data to add - We might be trying to initialize
			// the modeling, if so, no need to do anything.
			if (m_ServerData != null && m_ServerData.Generations > 0)
			{
				//
				// Rebuild the population fitness graph
				InitializePopFitnessGraph();
				LoadDataForPopFitnessGraph();

				InitializePopComplexityGraph();
				LoadDataForPopComplexityGraph();
			}
		}

		/// <summary>
		/// Thread safe technique to update the model status.
		/// </summary>
		/// <param name="Text">Text to display</param>
		private void UpdateModelStatus(String Text)
		{
			if (this.statusModeling.InvokeRequired)
			{
				GPModeler.DEL_ReportStatus d = new GPModeler.DEL_ReportStatus(UpdateModelStatus);
				this.Invoke(d, new Object[] { Text });
			}
			else
			{
				this.lbStatus.Text = Text;
			}
		}

		delegate void EnableCancelCallback();
		private void EnableCancel()
		{
			if (this.InvokeRequired)
			{
				EnableCancelCallback d = new EnableCancelCallback(EnableCancel);
				this.Invoke(d, null);
			}
			else
			{
				this.btnCancel.Enabled = true;
			}
		}

		delegate void EnableStatusMarqueeCallback(bool Status);
		/// <summary>
		/// Thread safe technique to update the model status marquee.
		/// </summary>
		private void EnableStatusMarquee(bool Status)
		{
			if (this.statusModeling.InvokeRequired)
			{
				EnableStatusMarqueeCallback d = new EnableStatusMarqueeCallback(EnableStatusMarquee);
				this.Invoke(d, new Object[] { Status });
			}
			else
			{
				this.pgStatus.Visible = Status;
			}
		}

		delegate void ProgressIncrementCallback();
		/// <summary>
		/// Thread safe technique to increment the progress bar
		/// </summary>
		private void ProgressIncrement()
		{
			if (this.InvokeRequired)
			{
				ProgressIncrementCallback d = new ProgressIncrementCallback(ProgressIncrement);
				this.Invoke(d);
			}
			else
			{
				this.pgModeling.PerformStep();
			}
		}

		//
		// Thread safe technique to reset the progress bar
		delegate void ProgressResetCallback();
		private void ProgressReset()
		{
			if (this.InvokeRequired)
			{
				ProgressResetCallback d = new ProgressResetCallback(ProgressReset);
				this.Invoke(d);
			}
			else
			{
				this.pgModeling.Value = 1;
			}
		}

		delegate void EnableModelButtonCallback();
		/// <summary>
		/// Thread safe technique to re-enable the modeling button (and disable the cancel button)
		/// </summary>
		private void EnableModelButton()
		{
			if (this.InvokeRequired)
			{
				EnableModelButtonCallback d = new EnableModelButtonCallback(EnableModelButton);
				this.Invoke(d);
			}
			else
			{
				this.btnModel.Enabled = true;
				this.btnCancel.Enabled = false;
				this.btnProfileDelete.Enabled = true;
			}
		}

		delegate void UpdateModelResultsCallback(int ProgramID);
		/// <summary>
		/// Thread safe technique to update the model results.  This reloads
		/// the list of models for the currently selected program.  It also goes
		/// through and selects the program with ID of ProgramID.
		/// </summary>
		/// <param name="ProgramID"></param>
		private void UpdateModelResults(int ProgramID)
		{
			if (this.lvProfilesResults.InvokeRequired)
			{
				UpdateModelResultsCallback d = new UpdateModelResultsCallback(UpdateModelResults);
				this.Invoke(d, new Object[] { ProgramID });
			}
			else
			{
				this.lvProfilesResults_SelectedIndexChanged(this, null);
				//
				// Select the most recently created model
				foreach (ListViewItem lviProgram in lvResultsPrograms.Items)
				{
					if (ProgramID == Convert.ToInt32(lviProgram.ToolTipText))
					{
						lviProgram.Selected = true;
						lviProgram.EnsureVisible();
					}
				}
			}
		}

		//
		// These are a sloppy way of holding the graph results, but I can't seem
		// to think of a much better way to do this.  Or, I should say better, I'm
		// feeling a little too lazy to figure out a better way, ahem :)
		private ZedGraph.PointPairList m_ptListFitness;
		private ZedGraph.PointPairList m_ptListHits;
		private ZedGraph.PointPairList m_ptListComplexityMin;
		private ZedGraph.PointPairList m_ptListComplexityMax;
		private ZedGraph.PointPairList m_ptListComplexityAve;
		private ZedGraph.PointPairList m_ptListComplexityBestOf;
		private ZedGraph.PointPairList m_ptListPopFitnessMin;
		private ZedGraph.PointPairList m_ptListPopFitnessMax;
		private ZedGraph.PointPairList m_ptListPopFitnessAve;

		/// <summary>
		/// Prepares the fitness graph for use
		/// </summary>
		private void InitializeFitnessGraph()
		{
			//
			// Decide on a title
			String Y1Title="Fitness";
			//
			// The try-catch block is to handle the initialization case with no modeling
			// profile yet defined.
			if (m_ModelProfile != null)
			{
				Y1Title = "Fitness Error : "+GPDatabaseUtils.FieldValue(m_ModelProfile.FitnessFunctionID, "tblFitnessFunction", "Name");
			}

			ZedGraph.GraphPane pane = new ZedGraph.GraphPane(
				new Rectangle(0, 0, graphFitness.Width, graphFitness.Height),
				"Best Program Fitness",
				"Generation",
				Y1Title);

			graphFitness.GraphPane = pane;

			//
			// Need a second Y-Axis for the hit counts
			graphFitness.GraphPane.AddY2Axis("# Hits");
			graphFitness.GraphPane.Y2Axis.Title.Text = "# Hits";
			graphFitness.GraphPane.Y2Axis.IsVisible = true;

			graphFitness.GraphPane.YAxis.Title.FontSpec.FontColor = Color.Blue;
			graphFitness.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.Blue;
			graphFitness.GraphPane.Y2Axis.Title.FontSpec.FontColor = Color.Maroon;
			graphFitness.GraphPane.Y2Axis.Scale.FontSpec.FontColor = Color.Maroon;

			graphFitness.GraphPane.Fill = new ZedGraph.Fill(Color.AliceBlue, Color.WhiteSmoke, 0F);
			graphFitness.GraphPane.Chart.Fill = new ZedGraph.Fill(Color.Silver, Color.White, 45.0f);

			//
			// Prepare the plotting curve
			m_ptListFitness = new ZedGraph.PointPairList();
			m_ptListHits = new ZedGraph.PointPairList();
			ZedGraph.CurveItem CurveFitness = graphFitness.GraphPane.AddCurve("Fitness", m_ptListFitness, Color.Blue);
			ZedGraph.CurveItem CurveHits = graphFitness.GraphPane.AddCurve("Hits", m_ptListHits, Color.Maroon);
			CurveHits.IsY2Axis = true;

			((ZedGraph.LineItem)CurveFitness).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveHits).Symbol.Size = 4;
		}

		/// <summary>
		/// Prepares the population complexity graph for use
		/// </summary>
		private void InitializePopComplexityGraph()
		{
			ZedGraph.GraphPane pane = new ZedGraph.GraphPane(
				new Rectangle(0, 0, graphPopulationComplexity.Width, graphPopulationComplexity.Height),
				"Population Complexity",
				"Generation",
				"Complexity (Node Count)");

			graphPopulationComplexity.GraphPane = pane;
			graphPopulationComplexity.GraphPane.Fill = graphFitness.GraphPane.Fill;
			graphPopulationComplexity.GraphPane.Chart.Fill = graphFitness.GraphPane.Chart.Fill;

			m_ptListComplexityMin = new ZedGraph.PointPairList();
			m_ptListComplexityMax = new ZedGraph.PointPairList();
			m_ptListComplexityAve = new ZedGraph.PointPairList();
			m_ptListComplexityBestOf = new ZedGraph.PointPairList();
			ZedGraph.CurveItem CurveMin = graphPopulationComplexity.GraphPane.AddCurve("Min", m_ptListComplexityMin, Color.Red);
			ZedGraph.CurveItem CurveMax = graphPopulationComplexity.GraphPane.AddCurve("Max", m_ptListComplexityMax, Color.Green);
			ZedGraph.CurveItem CurveAve = graphPopulationComplexity.GraphPane.AddCurve("Ave", m_ptListComplexityAve, Color.Blue);
			ZedGraph.CurveItem CurveBest = graphPopulationComplexity.GraphPane.AddCurve("Best Of", m_ptListComplexityBestOf, Color.Black);

			((ZedGraph.LineItem)CurveMin).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveMax).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveAve).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveBest).Symbol.Size = 4;

		}

		/// <summary>
		/// Prepares the population fitness graph for use
		/// </summary>
		private void InitializePopFitnessGraph()
		{
			ZedGraph.GraphPane pane = new ZedGraph.GraphPane(
				new Rectangle(0, 0, graphPopulationFitness.Width, graphPopulationFitness.Height),
				"Population Fitness",
				"Generation",
				graphFitness.GraphPane.YAxis.Title.Text);

			graphPopulationFitness.GraphPane = pane;
			graphPopulationFitness.GraphPane.Fill = graphFitness.GraphPane.Fill;
			graphPopulationFitness.GraphPane.Chart.Fill = graphFitness.GraphPane.Chart.Fill;

			//
			// Prepare the plotting curves
			m_ptListPopFitnessMin = new ZedGraph.PointPairList();
			m_ptListPopFitnessMax = new ZedGraph.PointPairList();
			m_ptListPopFitnessAve = new ZedGraph.PointPairList();
			ZedGraph.CurveItem CurveMin = graphPopulationFitness.GraphPane.AddCurve("Min", m_ptListPopFitnessMin, Color.Red);
			ZedGraph.CurveItem CurveMax = graphPopulationFitness.GraphPane.AddCurve("Max", m_ptListPopFitnessMax, Color.Green);
			ZedGraph.CurveItem CurveAve = graphPopulationFitness.GraphPane.AddCurve("Ave", m_ptListPopFitnessAve, Color.Blue);

			((ZedGraph.LineItem)CurveMin).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveMax).Symbol.Size = 4;
			((ZedGraph.LineItem)CurveAve).Symbol.Size = 4;
		}

		/// <summary>
		/// Loads the historical modeling data for the currently selected
		/// distributed server.  Typically called in reponse to the user changing
		/// the combo box selection for which server to view.
		/// </summary>
		private void LoadDataForPopFitnessGraph()
		{

			for (int Generation = 0; Generation < m_ServerData.Generations; Generation++)
			{
				double FitnessMin = m_ServerData[m_ModelingSelectedServer, Generation].FitnessMinimum;
				double FitnessMax = m_ServerData[m_ModelingSelectedServer, Generation].FitnessMaximum;
				double FitnessAve = m_ServerData[m_ModelingSelectedServer, Generation].FitnessAverage;
				FitnessMax = Math.Min(FitnessMax, FitnessMin * 5);
				FitnessAve = Math.Min(FitnessAve, FitnessMin * 3);

				m_ptListPopFitnessMin.Add(Generation+1, FitnessMin);
				m_ptListPopFitnessMax.Add(Generation+1, FitnessMax);
				m_ptListPopFitnessAve.Add(Generation+1, FitnessAve);
			}

			//
			// Tell the graph to update itself
			graphPopulationFitness.GraphPane.AxisChange(this.CreateGraphics());
			graphPopulationFitness.Invalidate(); 
		}

		/// <summary>
		/// Loads the historical modeling data for the currently selected
		/// distributed server.  Typically called in reponse to the user changing
		/// the combo box selection for which server to view.
		/// </summary>
		private void LoadDataForPopComplexityGraph()
		{

			for (int Generation = 0; Generation < m_ServerData.Generations; Generation++)
			{
				int ComplexityMin = m_ServerData[m_ModelingSelectedServer, Generation].ComplexityMinimum;
				int ComplexityMax = m_ServerData[m_ModelingSelectedServer, Generation].ComplexityMaximum;
				int ComplexityAve = m_ServerData[m_ModelingSelectedServer, Generation].ComplexityAverage;
				int ComplexityBest = m_ServerData[m_ModelingSelectedServer, Generation].BestComplexity;

				m_ptListComplexityMin.Add(Generation + 1, ComplexityMin);
				m_ptListComplexityMax.Add(Generation + 1, ComplexityMax);
				m_ptListComplexityAve.Add(Generation + 1, ComplexityAve);
				m_ptListComplexityBestOf.Add(Generation+1,ComplexityBest);
			}
			//
			// Tell the graph to update itself
			graphPopulationComplexity.GraphPane.AxisChange(this.CreateGraphics());
			graphPopulationComplexity.Invalidate();
		}
	}
}
