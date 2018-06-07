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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using GPStudio.Interfaces;
using GPStudio.Shared;
using System.Runtime.Remoting.Lifetime;

namespace GPStudio.Client
{
	/// <summary>
	/// The results tab implements the IBatchClient interface so it can update when a
	/// batch model has been completed.
	/// </summary>
	partial class fmProject : IBatchClient
	{
		/// <summary>
		/// Get the results presented
		/// </summary>
		private void InitializeResultsDisplay()
		{
			//
			// Load the modeling Training for visualization -- Look at the
			// Training combo box to find out what to use.
			m_TrainingData = new GPModelingData();

			//
			// A new project may not have a training file yet defined, so be
			// sure to check for this condition.  TODO: This sure seems like a hack,
			// need to find a more elegant solution (when I'm not so tired).
			if (cbResultsDataSet.SelectedIndex == 0 && m_Project.DataTrainingID != 0)
			{
				m_TrainingData.LoadFromDB(m_Project.DataTrainingID, null, null);
			}
			else if (cbResultsDataSet.SelectedIndex > 0)
			{
				int ModelingFileID = ((ComboDataItem)cbResultsDataSet.SelectedItem).ModelingFileID;
				m_TrainingData.LoadFromDB(ModelingFileID, null, null);
			}
			else
			{
				return;
			}

			//
			// Give this data to the IGPCustomFitness object so it can be used for fitness computations
			if (m_ICustomFitness != null)
			{
				m_ICustomFitness.Training = m_TrainingData.TrainingForModeling(
												m_Profile.ModelType.InputDimension,
												m_Profile.ModelType.PredictionDistance);
			}

			//
			// Setup the X-Axis selection combo box
			cbXAxis.Items.Clear();
			for (int nItem = 0; nItem < m_TrainingData.Columns; nItem++)
			{
				cbXAxis.Items.Add(m_TrainingData.Header[nItem]);
			}
			cbXAxis.SelectedItem = cbXAxis.Items[0];

			//
			// Display the training Training
			InitializeResultsGraph(m_TrainingData, null, null, null);
			InitializeTabularGrid(m_TrainingData);
		}

		//
		// Queries and displays the list of programs created for the indicated
		// profile and project
		private void DisplayPrograms(int ProfileID, int ProjectID)
		{
			//
			// Clear any current items
			lvResultsPrograms.Items.Clear();

			//
			// Query for the any programs
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{

				string sSQL = "SELECT DBCode,Date,Fitness,Hits,MaxHits,Complexity,Program FROM tblProgram ";
				sSQL += "WHERE ProjectID = " + ProjectID;
				sSQL += " AND ModelProfileID = " + ProfileID;
				sSQL += " ORDER BY Fitness,Hits,Complexity";

				OleDbCommand cmd = new OleDbCommand(sSQL, con);
				OleDbDataReader rdr = cmd.ExecuteReader();

				System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
				while (rdr.Read())
				{
					ListViewItem lvItem = lvResultsPrograms.Items.Add("");
					//
					// Place the DBCode in the tooltip
					lvItem.ToolTipText = rdr["DBCode"].ToString();
					lvItem.Text = rdr["Date"].ToString();
					if (((Double)rdr["Fitness"]) < GPEnums.RESULTS_TOLERANCE)
					{
						lvItem.SubItems.Add("0.0000");
					}
					else
					{
						double FitnessError = (double)rdr["Fitness"];
						lvItem.SubItems.Add(String.Format("{0:#,0.####}", FitnessError));
					}
					lvItem.SubItems.Add(rdr["Hits"].ToString());
					lvItem.SubItems.Add(rdr["Complexity"].ToString());
					//
					// Place the program text in the tag - Have to convert the bytes into a String
					String ProgramXML = encoding.GetString((byte[])rdr["Program"]);
					lvItem.Tag = ProgramXML;
				}
			}
		}

		/// <summary>
		/// Add the modeling results to the tabular pane
		/// </summary>
		/// <param name="InputDimension"></param>
		/// <param name="Name"></param>
		/// <param name="XValues"></param>
		/// <param name="YValues"></param>
		private void AddResultsTabular(int InputDimension, string Name, double[] XValues, double[] YValues)
		{
			//
			// Need some place to store error, so we can compute up some error stats
			List<double> PointError = new List<double>(dgResults.Rows.Count);
			double ErrorTotal = 0.0;
			//
			// If this is time series, handle it separately
			if (m_TrainingData.TimeSeries)
			{
				dgResults.Rows.Clear();
				dgResults.Rows.Add(XValues.Length);
				for (int Row = 0; Row < dgResults.Rows.Count; Row++)
				{
					dgResults.Rows[Row].Cells[0].Value = XValues[Row];
					dgResults.Rows[Row].Cells[1].Value = XValues[Row];
					dgResults.Rows[Row].Cells[2].Value = YValues[Row];

					double ModelError = Math.Abs(YValues[Row] - XValues[Row]);
					PointError.Add(ModelError);
					dgResults.Rows[Row].Cells[3].Value = ModelError;
					ErrorTotal += ModelError;

					//
					// Give the row a label
					int RowLabel = Row + 1;
					dgResults.Rows[Row].HeaderCell.Value = RowLabel.ToString();
				}
			}
			else
			{
				//
				// Place the modeling result into the second to last (and existing) column
				// While doing this, compute up the average, median and std. dev. of the error
				for (int Row = 0; Row < dgResults.Rows.Count; Row++)
				{
					dgResults.Rows[Row].Cells[dgResults.Columns.Count - 2].Value = YValues[Row];
					//
					// Compute the absolute error and add it
					double ModelError = Math.Abs(YValues[Row] - (double)dgResults.Rows[Row].Cells[dgResults.Columns.Count - 3].Value);
					//
					// TODO: Replace this with a user defined tolerance value
					if (ModelError < GPEnums.RESULTS_TOLERANCE) ModelError = 0.0;
					PointError.Add(ModelError);
					dgResults.Rows[Row].Cells[dgResults.Columns.Count - 1].Value = ModelError;

					ErrorTotal += ModelError;
				}
			}
			//
			// Compute the average error
			double ErrorAverage = ErrorTotal / dgResults.Rows.Count;

			//
			// compute the std. dev. of the error
			double DiffTotal = 0.0;
			foreach (double Error in PointError)
			{
				DiffTotal += ((Error - ErrorAverage) * (Error - ErrorAverage));
			}
			double ErrorStdDev = Math.Sqrt(DiffTotal / dgResults.Rows.Count);

			//
			// Compute the median of the error
			PointError.Sort();
			double ErrorMedian = PointError[PointError.Count / 2];

			//
			// Compute the custom fitness function results
			m_ICustomFitness.Predictions = PrepareFitnessPredictions(YValues);
			double ErrorFitness = m_ICustomFitness.Compute();

			//
			// Display the results
			tsTabularErrorAverage.Text = String.Format("Raw Average: {0:#,0.####}", ErrorAverage);
			tsTabularErrorStdDev.Text = String.Format("Raw Std Dev: {0:#,0.####}", ErrorStdDev);
			tsTabularErrorMedian.Text = String.Format("Raw Median: {0:#,0.####}", ErrorMedian);
			tsTabularFitnessFunction.Text = String.Format("Fitness Error: {0:#,0.####}", ErrorFitness);
		}

		/// <summary>
		/// The results display is front padded with extra values that we don't need
		/// for the fitness computation, so have to eliminate them before giving the
		/// results to the fitness computation object.
		/// </summary>
		/// <param name="YValues">Full column of predctions from the model results</param>
		/// <returns>Set of predictions with the front padding removed</returns>
		private double[] PrepareFitnessPredictions(double[] YValues)
		{
			//
			// The actual number of predictions is the number of input values minus the input dimension,
			// if this is a time series data set.
			if (m_Profile.ModelType.Type == GPEnums.ModelType.TimeSeries)
			{
				double[] Predictions = new double[YValues.Length - m_Profile.ModelType.InputDimension-m_Profile.ModelType.PredictionDistance+1];
				Array.Copy(
					YValues, m_Profile.ModelType.InputDimension+m_Profile.ModelType.PredictionDistance-1, 
					Predictions, 0, 
					Predictions.Length);

				return Predictions;
			}

			//
			// For regression, return the whole set of predictions.
			return YValues;
		}

		/// <summary>
		/// Prepare the tabular grid for Training
		/// </summary>
		/// <param name="Training">Data set to display</param>
		private void InitializeTabularGrid(GPModelingData Training)
		{
			//
			// Reset anything that might be there already
			dgResults.Columns.Clear();

			//
			// Creating the columns
			for (int Input = 0; Input < Training.Columns; Input++)
			{
				dgResults.Columns.Add(Training.Header[Input],Training.Header[Input]);
				dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Format = GPEnums.NUMERIC_DISPLAY_FORMAT;
				dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}
			dgResults.Columns.Add(cbResultsDataSet.Text, cbResultsDataSet.Text);
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Format = GPEnums.NUMERIC_DISPLAY_FORMAT;
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgResults.Columns.Add("Model", "Model");
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Format = GPEnums.NUMERIC_DISPLAY_FORMAT;
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgResults.Columns.Add("Raw Error", "Raw Error");
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Format = GPEnums.NUMERIC_DISPLAY_FORMAT;
			dgResults.Columns[dgResults.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dgResults.Rows.Add(Training.Rows);
			for (int Row = 0; Row < Training.Rows; Row++)
			{
				//
				// Display the inputs
				for (int nInput = 0; nInput < Training.Columns; nInput++)
				{
					dgResults.Rows[Row].Cells[nInput].Value = Training[Row, nInput];
				}
				//
				// Display the objective
				if (Training.TimeSeries)
				{
					dgResults.Rows[Row].Cells[Training.Columns].Value = Training[Row, 0];
				}
				else
				{
					dgResults.Rows[Row].Cells[Training.Columns].Value = Training.ObjectiveRow(Row)[0];
				}
				//
				// Give the row a label
				int value = Row + 1;
				dgResults.Rows[Row].HeaderCell.Value = value.ToString();
			}
		}

		/// <summary>
		/// Prepare the Results graph for plotting.  This is still sloppy, but it
		/// is better than what I had before! :)
		/// </summary>
		/// <param name="gpData"></param>
		/// <param name="Program"></param>
		/// <param name="XValues"></param>
		/// <param name="YValues"></param>
		private void InitializeResultsGraph(GPModelingData gpData, String Program, double[] XValues, double[] YValues)
		{
			ZedGraph.GraphPane pane = new ZedGraph.GraphPane(
				new Rectangle(0, 0, graphResults.Width, graphResults.Height),
				"Modeling Comparison",
				cbXAxis.Items[cbXAxis.SelectedIndex].ToString(),
				gpData.Header[gpData.Header.Length - 1]);

			graphResults.GraphPane = pane;
			graphResults.GraphPane.Fill = new ZedGraph.Fill(Color.AliceBlue, Color.WhiteSmoke, 0F);
			graphResults.GraphPane.Chart.Fill = new ZedGraph.Fill(Color.Silver, Color.White, 45.0f);

			//
			// If the user has selected "Count" to be the X-Axis, or we are in a time series
			// project, use a numberic count as the X-Axis label.
			string[] XLabels = new string[gpData.Rows];
			double[] XTemp = null;
			if (chkCount.Checked || gpData.TimeSeries)
			{
				XTemp = new double[gpData.Rows];
				//
				// Build a set of labels using a row count
				for (int nRow = 0; nRow < gpData.Rows; nRow++)
				{
					XLabels[nRow] = ((int)(nRow + 1)).ToString();
					XTemp[nRow] = nRow + 1.0;
				}
				pane.XAxis.Title.Text = "Count";
			}
			else
			{
				//
				// Build a set of labels from the common X-Axis
				for (int nRow = 0; nRow < gpData.Rows; nRow++)
				{
					XLabels[nRow] = gpData[nRow, 0].ToString();
				}
			}

			//
			// If we are in Bar plot mode, set up accordingly
			if (rdChartTypeBar.Checked)
			{
				pane.XAxis.Type = ZedGraph.AxisType.Text;
				pane.XAxis.Scale.TextLabels = XLabels;
				pane.XAxis.MajorTic.IsBetweenLabels = true;
			}

			//
			// If we have XY or scatter (great code comment, I know)
			if (rdChartTypeXY.Checked || rdChartTypeScatter.Checked)
			{
				//
				// Add the training Training
				if (chkModelOnly.Checked == false)
				{
					ZedGraph.LineItem line = null;
					if (gpData.TimeSeries)
					{
						line = graphResults.GraphPane.AddCurve(cbXAxis.Items[cbXAxis.SelectedIndex].ToString(), XTemp, gpData.InputColumn(0), Color.Blue, ZedGraph.SymbolType.None);
					}
					else
					{
						line = graphResults.GraphPane.AddCurve(cbXAxis.Items[cbXAxis.SelectedIndex].ToString(), gpData.InputColumn(cbXAxis.SelectedIndex), gpData.ObjectiveColumn(0), Color.Blue, ZedGraph.SymbolType.None);
					}
					if (rdChartTypeScatter.Checked)
					{
						line.Line.IsVisible = false;
						line.Symbol.Type = ZedGraph.SymbolType.XCross;
						line.Symbol.Size = 4;
					}
				}
				//
				// Add the modeling Training
				if (XValues != null)
				{
					ZedGraph.LineItem lineModel = null;
					if (gpData.TimeSeries)
					{
						lineModel = graphResults.GraphPane.AddCurve(Program, XTemp, YValues, Color.Red, ZedGraph.SymbolType.None);
					}
					else
					{
						lineModel = graphResults.GraphPane.AddCurve(Program, gpData.InputColumn(cbXAxis.SelectedIndex), YValues, Color.Red, ZedGraph.SymbolType.None);
					}
					if (rdChartTypeScatter.Checked)
					{
						lineModel.Line.IsVisible = false;
						lineModel.Symbol.Type = ZedGraph.SymbolType.Plus;
						lineModel.Symbol.Size = 4;
					}
				}
			}
			else if (rdChartTypeBar.Checked)
			{
				if (chkModelOnly.Checked == false)
				{
					graphResults.GraphPane.AddBar(cbXAxis.Items[cbXAxis.SelectedIndex].ToString(), gpData.InputColumn(cbXAxis.SelectedIndex), gpData.ObjectiveColumn(0), Color.Blue);
				}
				//
				// Add the modeling Training
				if (XValues != null)
				{
					graphResults.GraphPane.AddBar(Program, gpData.InputColumn(cbXAxis.SelectedIndex), YValues, Color.Red);
				}
			}

			//
			// Force the graph to visually update
			graphResults.GraphPane.AxisChange(this.CreateGraphics());
			graphResults.Invalidate();
		}

		GPModelingData m_TrainingData;
		double[] m_ModelXValues = null;
		double[] m_ModelYValues = null;
		private void lvResultsPrograms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			//
			// Reset the program diagram
			diagProgram.NewDiagram("<Empty>");
			lvProgramHist.Items.Clear();

			//
			// Update results if the item is selected, otherwise clear things out
			if (e.IsSelected)
			{
				//
				// Compute the program results
				PrepareProgramResults(e.Item);
				//
				// Compute the program structure stats
				DisplayProgramHist((String)e.Item.Tag);

				//
				// Enable the context menu
				lvResultsPrograms.ContextMenuStrip = contextMenuPrograms;
			}
			else
			{
				//
				// Reset the modeled results
				m_ModelXValues = null;
				m_ModelYValues = null;

				//
				// Reset the view
				InitializeResultsGraph(m_TrainingData, null, null, null);
				InitializeTabularGrid(m_TrainingData);

				//
				// Disable the context menu
				lvResultsPrograms.ContextMenuStrip = null;
			}
		}

		/// <summary>
		/// Takes the currently selected program and gets the results
		/// for it computed up.
		/// </summary>
		/// <param name="lviProgram">List View Item of the selected program</param>
		private void PrepareProgramResults(ListViewItem lviProgram)
		{
			//
			// Obtain the a program interface from the local object
			IGPProgram iProgram = fmMain.ServerManager.LocalServer.Program;

			//
			// Check to see if the IFunctionSet interface is still alive, if not, we'll
			// have to create it again.  For some reason, it doesn't appear the client
			// side sponsor is interacting with the server to keep it alive.  When
			// I do debugging, it seems the messages are being passed around correctly,
			// but when I set it for release, it seems to fail???
			try
			{
				int Count = m_IFunctionSet.Count;
			}
			catch
			{
				//
				// Let the user know we are busy for a second
				Cursor prev = this.Cursor;
				this.Cursor = Cursors.WaitCursor;

				int ProfileID = (int)lvProfilesResults.SelectedItems[0].Tag;
				PrepareFunctionSet(ProfileID);

				this.Cursor = prev;
			}

			//
			// Set the function set and training data properties
			iProgram.FunctionSet = m_IFunctionSet;
			iProgram.Training = m_TrainingData.Training;

			//
			// Construct the program
			iProgram.ProgramFromXML((String)lviProgram.Tag);

			//
			// Execute the program on the training Training
			ComputeProgram(iProgram, m_TrainingData, ref m_ModelXValues, ref m_ModelYValues);

			//
			// Update the graphical view
			InitializeResultsGraph(m_TrainingData, lviProgram.SubItems[0].Text, m_ModelXValues, m_ModelYValues);

			//
			// Update the tabular view
			AddResultsTabular(m_TrainingData.Columns, lviProgram.SubItems[0].Text, m_ModelXValues, m_ModelYValues);

			//
			// Get the program displayed
			DisplayProgramAsSource(iProgram);
		}

		/// <summary>
		/// Builds the graphical diagram of the program.
		/// </summary>
		/// <param name="xmlProgram">XML representation of the program</param>
		private void DisplayDiagram(XmlNode xmlProgram)
		{
			//
			// Prepare the progress bar - The number of nodes in the program
			// is stored in the third item in the list view item.  
			// TODO: Yes, I know, this should be defined as a constant.
			statusResults.Visible = true;
			tsProgressResults.Value = 1;
			ListViewItem lviProgram=lvResultsPrograms.SelectedItems[0];
			tsProgressResults.Maximum=Convert.ToInt32(lviProgram.SubItems[3].Text);

			diagProgram.LayoutEnabled = false;
			diagProgram.NewDiagram();
			diagProgram.Root.Text = "Program";

			//
			// Add the RPB branch
			Netron.Lithium.ShapeBase ltRPB = diagProgram.Root.AddChild("Main");
			XmlNode xmlRPB = xmlProgram.SelectSingleNode("RPB");
			XmlNode xmlNode = xmlRPB.SelectSingleNode("GPNode");
			BuildDiagram(ltRPB.AddChild(""), xmlNode);

			//
			// Add the ADF branches
			XmlNodeList xmlListADF = xmlProgram.SelectNodes("ADF");
			for (int WhichADF=0; WhichADF<xmlListADF.Count; WhichADF++)
			{
				XmlNode adfNode=xmlListADF[WhichADF];
				Netron.Lithium.ShapeBase ltADF = diagProgram.Root.AddChild("ADF"+WhichADF);
				BuildDiagram(ltADF.AddChild(""), adfNode.SelectSingleNode("GPNode"));
			}

			//
			// Add the ADL branches
			XmlNodeList xmlListADL = xmlProgram.SelectNodes("ADL");
			for (int WhichADL=0; WhichADL<xmlListADL.Count; WhichADL++)
			{
				XmlNode adlNode = xmlListADL[WhichADL];
				Netron.Lithium.ShapeBase ltADL = diagProgram.Root.AddChild("ADL"+WhichADL);
				//
				// Add each of the loop branches
				BuildDiagram(ltADL.AddChild("LIB").AddChild(""), adlNode.SelectSingleNode("LIB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADL.AddChild("LCB").AddChild(""), adlNode.SelectSingleNode("LCB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADL.AddChild("LBB").AddChild(""), adlNode.SelectSingleNode("LBB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADL.AddChild("LUB").AddChild(""), adlNode.SelectSingleNode("LUB").SelectSingleNode("GPNode"));
			}

			//
			// Add the ADR branches
			XmlNodeList xmlListADR = xmlProgram.SelectNodes("ADR");
			for (int WhichADR=0; WhichADR<xmlListADR.Count; WhichADR++)
			{
				XmlNode adrNode = xmlListADR[WhichADR];
				Netron.Lithium.ShapeBase ltADR = diagProgram.Root.AddChild("ADR"+WhichADR);
				//
				// Add each of the loop branches
				BuildDiagram(ltADR.AddChild("RCB").AddChild(""), adrNode.SelectSingleNode("RCB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADR.AddChild("RBB").AddChild(""), adrNode.SelectSingleNode("RBB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADR.AddChild("RUB").AddChild(""), adrNode.SelectSingleNode("RUB").SelectSingleNode("GPNode"));
				BuildDiagram(ltADR.AddChild("RGB").AddChild(""), adrNode.SelectSingleNode("RGB").SelectSingleNode("GPNode"));
			}

			//
			// Hide the progress bar
			statusResults.Visible = false;
			diagProgram.LayoutEnabled = true;
		}

		/// <summary>
		/// A recursive method that constructs the diagram.  Nothing special to note,
		/// just a basic recursive technique.
		/// </summary>
		private void BuildDiagram(Netron.Lithium.ShapeBase ltNode, XmlNode xmlNode)
		{
			tsProgressResults.Increment(1);

			switch (xmlNode.Attributes[0].InnerText)
			{
				case "Function":
					{
						XmlNode xmlType = xmlNode.SelectSingleNode("Function");
						if (xmlType.InnerText == "ADF" ||
							xmlType.InnerText == "ADL" ||
							xmlType.InnerText == "ADR")
						{
							XmlNode xmlWhichADF = xmlNode.SelectSingleNode("WhichFunction");
							ltNode.Text = xmlType.InnerText + Convert.ToInt32(xmlWhichADF.InnerText);
						}
						else
						{
							ltNode.Text = xmlType.InnerText;
						}
						XmlNodeList listParams = xmlNode.SelectNodes("GPNode");
						foreach (XmlNode ParamNode in listParams)
						{
							Netron.Lithium.ShapeBase ltChild = ltNode.AddChild("child");
							//
							// recursively build from the child
							BuildDiagram(ltChild, ParamNode);
						}
					}
					break;
				case "Terminal":
					{
						XmlNode xmlType = xmlNode.SelectSingleNode("Terminal");
						XmlNode xmlValue = xmlNode.SelectSingleNode("Value");
						if (xmlType.InnerText == GPEnums.TerminalType.UserDefined.ToString())
						{
							if (m_TrainingData.TimeSeries)
							{
								ltNode.Text = dgResults.Columns[0].Name;
							}
							else
							{
								int WhichUserDefined = Convert.ToInt16(xmlValue.InnerText);
								ltNode.Text = dgResults.Columns[WhichUserDefined].Name;
							}
						}
						else if (xmlType.InnerText == GPEnums.TerminalType.ADFParameter.ToString() ||
								 xmlType.InnerText == GPEnums.TerminalType.ADLParameter.ToString() ||
								 xmlType.InnerText == GPEnums.TerminalType.ADRParameter.ToString())
						{
							ltNode.Text = "p" + xmlValue.InnerText;
						}
						else
						{
							ltNode.Text = xmlValue.InnerText;
						}
					}
					break;
			}
		}

		/// <summary>
		/// Gets the language version of the program written and displayed 
		/// </summary>
		/// <param name="iProgram">Interface to the program to translate into source code</param>
		private void DisplayProgramAsSource(IGPProgram iProgram)
		{
			//
			// See if we have a valid set of functions first
			if (m_ILanguageWriter == null)
			{
				txtProgram.Text = "Not all user defined functions have been\n";
				txtProgram.Text += "defined for this language.\n\n";
				txtProgram.Text += "Use the User Defined Functions editor\n";
				txtProgram.Text += "to complete the missing functions.";
				txtProgram.Text += "\n\n";
				txtProgram.Text += "The InputHistory time series of parameter inputs is\n";
				txtProgram.Text += "not supported for C and Fortran languages.  Any programs that\n";
				txtProgram.Text += "utilize the TS UDFs can not be written in these languages.";
				return;
			}

			//
			// Write the program - Using the correct language
			bool TimeSeries = m_Profile.ModelType.Type == GPEnums.ModelType.TimeSeries;
			switch (tsmenuLanguage.Text)
			{
				case GPEnums.LANGUAGE_C:
					txtProgram.Text = m_ILanguageWriter.WriteC(iProgram, TimeSeries);
					break;
				case GPEnums.LANGUAGE_CPP:
					txtProgram.Text = m_ILanguageWriter.WriteCPP(iProgram, TimeSeries);
					break;
				case GPEnums.LANGUAGE_CSHARP:
					txtProgram.Text=m_ILanguageWriter.WriteCSharp(iProgram, TimeSeries);
					break;
				case GPEnums.LANGUAGE_VBNET:
					txtProgram.Text = m_ILanguageWriter.WriteVBNet(iProgram, TimeSeries);
					break;
				case GPEnums.LANGUAGE_JAVA:
					txtProgram.Text = m_ILanguageWriter.WriteJava(iProgram, TimeSeries);
					break;
				case GPEnums.LANGUAGE_FORTRAN:
					txtProgram.Text = m_ILanguageWriter.WriteFortran(iProgram, TimeSeries);
					break;
				default:
					txtProgram.Text = "Application Error...";
					break;
			}
		}

		private void rdChartOption_Click(object sender, EventArgs e)
		{
			//
			// Switch the chart to a X-Y plot
			if (lvResultsPrograms.SelectedItems.Count > 0)
			{
				InitializeResultsGraph(m_TrainingData, lvResultsPrograms.SelectedItems[0].Text, m_ModelXValues, m_ModelYValues);
			}
			else
			{
				InitializeResultsGraph(m_TrainingData, null, null, null);
			}
		}

		/// <summary>
		/// Using the referenced modeling Training, compute the program results
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="Training"></param>
		/// <param name="XValues"></param>
		/// <param name="YValues"></param>
		private void ComputeProgram(IGPProgram Program, GPModelingData Training, ref double[] XValues, ref double[] YValues)
		{
			//
			// If this is a time series, have to handle the Training accordingly
			if (Training.TimeSeries)
			{
				XValues = new double[Training.Rows];
				YValues = new double[Training.Rows];

				//
				// Prepare the XValues
				for (int Row = 0; Row < m_ProfileResults.ModelType.InputDimension + m_ProfileResults.ModelType.PredictionDistance - 1; Row++)
				{
					XValues[Row] = Training[Row, 0];
				}
				for (int Row = m_ProfileResults.ModelType.InputDimension; Row < (Training.Rows - m_ProfileResults.ModelType.PredictionDistance) + 1; Row++)
				{
					XValues[Row + m_ProfileResults.ModelType.PredictionDistance - 1] = Training[Row + m_ProfileResults.ModelType.PredictionDistance - 1, 0];
				}

				//
				// Do an asynch call of this function.  Large data sets were causing
				// a timeout problem, so moved it to an asynch call to eliminate that
				// problem.

				//
				// Do a bulk computation on all the results
				DEL_ComputeBatchTS delComputeBatchTS = new DEL_ComputeBatchTS(Program.ComputeBatchTS);
				IAsyncResult ar = delComputeBatchTS.BeginInvoke(
					m_ProfileResults.ModelType.InputDimension,
					m_ProfileResults.ModelType.PredictionDistance,
					null, null);

				YValues = delComputeBatchTS.EndInvoke(ar);
			}
			else
			{
				//
				// Prepare the XValues
				XValues = new double[Training.Rows];
				for (int Row = 0; Row < Training.Rows; Row++)
				{
					XValues[Row] = Training[Row, 0];
				}

				//
				// Perform a batch computation
				YValues = Program.ComputeBatch();
			}
		}

		private delegate double[] DEL_ComputeBatchTS(int InputDimension, int PredictionDistance);

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			//
			// Delete the selected program
			if (MessageBox.Show("Confirm Program Delete?", "GP Studio Results", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				//
				// Grab the DBCode
				int ProgramID = Convert.ToInt32(lvResultsPrograms.SelectedItems[0].ToolTipText);
				//
				// Build the SQL command to delete
				String sSQL = "DELETE FROM tblProgram WHERE DBCode = " + ProgramID;

				OleDbConnection con = GPDatabaseUtils.Connect();

				//
				// Build the command to add the blob to the database
				OleDbCommand cmd = new OleDbCommand(sSQL, con);

				//
				// Execute the command
				try
				{
					cmd.ExecuteNonQuery();
					//
					// Remove the listview item
					lvResultsPrograms.SelectedItems[0].Remove();
				}
				catch (OleDbException ex)
				{
					string sError = ex.Message.ToString();
				}

				con.Close();

				//
				// Remove the diagram
				diagProgram.NewDiagram("<Empty>");
				lvProgramHist.Items.Clear();
			}
		}

		private void btnDiagramConstruct_Click(object sender, EventArgs e)
		{
			if (lvResultsPrograms.SelectedItems.Count > 0)
			{
				XmlDocument xmlProgram = new XmlDocument();
				xmlProgram.LoadXml((String)lvResultsPrograms.SelectedItems[0].Tag);
				XmlNode Root = xmlProgram.DocumentElement;
				DisplayDiagram(Root);
			}
		}

		/// <summary>
		/// Creates a histogram of the times each node occurs in the program.  this
		/// helps the user evaluate the structure of the program, in particular, which
		/// inputs are used and how frequently.
		/// </summary>
		private void DisplayProgramHist(String ProgramXML)
		{
			SortedDictionary<String, int> Hist = new SortedDictionary<string, int>();
			//
			// Construct an XmlDocument from the xml bytes
			XmlDocument xmlProgram = new XmlDocument();
			xmlProgram.LoadXml(ProgramXML);
			XmlNode Root = xmlProgram.DocumentElement;
			SearchChildNodes(Root, Hist);

			//
			// Add this results to the list view
			lvProgramHist.Items.Clear();
			foreach (KeyValuePair<String, int> Item in Hist)
			{
				ListViewItem lviHist = lvProgramHist.Items.Add(Item.Key);
				lviHist.SubItems.Add(Item.Value.ToString());
			}
		}

		/// <summary>
		/// Creates a frequency histogram of the nodes used in a program sub-tree
		/// </summary>
		/// <param name="DocNode">XML representation of the program</param>
		/// <param name="Hist">Structure to record the histogram results</param>
		private void SearchChildNodes(XmlNode DocNode, SortedDictionary<String, int> Hist)
		{
			//
			// If this is a GPNode, add it
			switch (DocNode.Name)
			{
				case "Function":
					if (Hist.ContainsKey(DocNode.InnerText))
					{
						Hist[DocNode.InnerText]++;
					}
					else
					{
						Hist.Add(DocNode.InnerText, 1);
					}
					//
					// Search the children
					foreach (XmlNode Child in DocNode.ChildNodes)
					{
						SearchChildNodes(Child, Hist);
					}
					break;
				case "Terminal":
					XmlNode Sibling = DocNode.NextSibling;
					String Type = "undef";
					switch (DocNode.InnerText)
					{
						case "UserDefined":
							//
							// Use the column header from the tabular grid as the name - for
							// time series, just use the first column
							if (m_TrainingData.TimeSeries)
							{
								Type = dgResults.Columns[0].Name;
							}
							else
							{
								int WhichUserDefined = Convert.ToInt32(Sibling.InnerText);
								Type = dgResults.Columns[WhichUserDefined].Name;
							}
							break;
						case "RandomDouble":
							Type = Sibling.InnerText;
							break;
						case "RandomInteger":
							Type = Sibling.InnerText;
							break;
					}
					if (Type != "undef")
					{
						if (Hist.ContainsKey(Type))
							Hist[Type]++;
						else
							Hist.Add(Type, 1);
					}
					break;
				default:
					//
					// Search the children
					foreach (XmlNode Child in DocNode.ChildNodes)
					{
						SearchChildNodes(Child, Hist);
					}
					break;
			}

		}

		private GPProjectProfile m_ProfileResults;
		private void lvProfilesResults_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// Reset the function set, so it will get created the next time a program is computed
			m_IFunctionSet = null;
			//
			// Update the UI controls based upon item selection status
			if (lvProfilesResults.SelectedIndices.Count > 0)
			{
				//
				// Show a busy cursor
				Cursor Prev = this.Cursor;
				this.Cursor = Cursors.WaitCursor;

				int ProfileID = (int)lvProfilesResults.SelectedItems[0].Tag;
				//
				// Load the profile for the selected Result Profile
				m_ProfileResults = new GPProjectProfile();
				m_ProfileResults.LoadFromDB(ProfileID);

				//
				// Display the programs related to this profile
				DisplayPrograms(ProfileID, m_Project.DBCode);

				//
				// Load and prepare the IGPCustomFitness object so that custom fitness
				// function results can be generated.
				PrepareCustomFitness(ProfileID);

				//
				// Replace the cursor
				this.Cursor = Prev;
			}
			else
			{
				lvResultsPrograms.Items.Clear();
				cbXAxis.Items.Clear();

				//
				// Disable the context menu
				lvResultsPrograms.ContextMenuStrip = null;
			}

			InitializeResultsDisplay();
		}

		private void cbResultsDataSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// Reset the modeled results, if any
			m_ModelXValues = null;
			m_ModelYValues = null;

			InitializeResultsDisplay();
			if (lvResultsPrograms.SelectedItems.Count > 0)
			{
				PrepareProgramResults(lvResultsPrograms.SelectedItems[0]);
			}
		}

		private void rdLanguage_Click(object sender, EventArgs e)
		{
			//
			// Update the language string
			tsmenuLanguage.Text = sender.ToString();

			//
			// If a program is selected get it written in java
			if (lvResultsPrograms.SelectedItems.Count > 0)
			{
				//
				// Reload the code functions for the newly selected language
				LoadUserDefinedFunctions((int)lvProfilesResults.SelectedItems[0].Tag);

				//
				// Obtain the a program interface from the local object
				IGPProgram iProgram = fmMain.ServerManager.LocalServer.Program;
				iProgram.FunctionSet = m_IFunctionSet;

				//
				// Construct the program
				iProgram.ProgramFromXML((String)lvResultsPrograms.SelectedItems[0].Tag);
				DisplayProgramAsSource(iProgram);
			}
		}

		GPServerClientSponsor m_FunctionSetSponsor;
		IGPFunctionSet m_IFunctionSet;
		IGPCustomFitness m_ICustomFitness;
		IGPLanguageWriter m_ILanguageWriter;

		/// <summary>
		/// Obtains an IGPCustomFitness object, from the local server, so that custom
		/// fitness functions can be used to compute results for display in the tabular
		/// results pane.
		/// </summary>
		/// <param name="ProfileID"></param>
		private void PrepareCustomFitness(int ProfileID)
		{
			//
			// Get the interface from the local server
			m_ICustomFitness = fmMain.ServerManager.LocalServer.CustomFitness;

			//
			// Grab the profile, it contains the fitness function ID we need
			GPProjectProfile Profile = new GPProjectProfile();
			Profile.LoadFromDB(ProfileID);
			//
			// Load the custom fitness function from the database
			String FitnessFunction = GPDatabaseUtils.FieldValue(Profile.FitnessFunctionID, "tblFitnessFunction", "Code");
			m_ICustomFitness.FitnessFunction = FitnessFunction;
		}

		/// <summary>
		/// Pulls the function set from the database and gets them compiled up
		/// and available for use when a program is selected from the results
		/// view.
		///		m_FunctionSet is ALWAYS the C# code
		///		m_FunctionSetCode is the language specific code that is used 
		///			by the language writers to create the language specific programs.
		/// </summary>
		/// <param name="ProfileID">Profile that contains the functions to prepare</param>
		private void PrepareFunctionSet(int ProfileID)
		{
			//
			// Obtain an IGPFunctionSet interface from the local server
			m_IFunctionSet = fmMain.ServerManager.LocalServer.FunctionSet;
			//
			// Assign a sponsor for the remote object
			m_FunctionSetSponsor = fmMain.SponsorServer.Create("Function Set");
			ILease LeaseInfo = (ILease)((MarshalByRefObject)m_IFunctionSet).GetLifetimeService();
			LeaseInfo.Register(m_FunctionSetSponsor);

			//
			// Reset anything that might have already been there
			m_IFunctionSet.Clear();

			//
			// Get the user defined functions loaded for the language writers
			if (!LoadUserDefinedFunctions(ProfileID))
			{
				return;
			}

			//
			// Grab the profile, it contains the list of functions we want
			GPProjectProfile Profile = new GPProjectProfile();
			Profile.LoadFromDB(ProfileID);

			//
			// Get each function from the database
			foreach (String FunctionName in Profile.FunctionSet)
			{
				short Arity = 0;
				bool TerminalParameters = false;
				String UserCode = "";
				//
				// Prepare the C# compiled code
				if (GPDatabaseUtils.LoadFunctionFromDB(FunctionName, ref Arity, ref TerminalParameters,ref UserCode, GPEnums.LANGUAGEID_CSHARP))
				{
					//
					// Add it to the function set interface
					m_IFunctionSet.AddFunction(FunctionName, Arity, TerminalParameters, UserCode);
				}
			}

			//
			// Check to see if indexed memory is used
			m_IFunctionSet.UseMemory = Profile.ModelingProfile.UseMemory;
		}

		/// <summary>
		/// Loads the user defined function from the database so they are ready
		/// for use by the language writer.
		/// </summary>
		/// <param name="ProfileID">Profile that contains the functions to prepare</param>
		/// <returns>True if all functions were found, False otherwise</returns>
		private bool LoadUserDefinedFunctions(int ProfileID)
		{
			//
			// Decide which language to Load for a program writer
			int LanguageID = 0;
			switch (tsmenuLanguage.Text)
			{
				case GPEnums.LANGUAGE_C:
					LanguageID = GPEnums.LANGUAGEID_C;
					break;
				case GPEnums.LANGUAGE_CPP:
					LanguageID = GPEnums.LANGUAGEID_CPP;
					break;
				case GPEnums.LANGUAGE_CSHARP:
					LanguageID = GPEnums.LANGUAGEID_CSHARP;
					break;
				case GPEnums.LANGUAGE_VBNET:
					LanguageID = GPEnums.LANGUAGEID_VBNET;
					break;
				case GPEnums.LANGUAGE_JAVA:
					LanguageID = GPEnums.LANGUAGEID_JAVA;
					break;
				case GPEnums.LANGUAGE_FORTRAN:
					LanguageID = GPEnums.LANGUAGEID_FORTRAN;
					break;
			}

			//
			// Obtain a language writer interface
			m_ILanguageWriter = fmMain.ServerManager.LocalServer.LanguageWriter;

			//
			// Grab the profile, it contains the list of functions we want
			GPProjectProfile Profile = new GPProjectProfile();
			Profile.LoadFromDB(ProfileID);

			//
			// Get each function from the database
			foreach (String FunctionName in Profile.FunctionSet)
			{
				short Arity = 0;
				bool TerminalParameters = false;
				String UserCode = "";
				//
				// Load the language specific code
				if (GPDatabaseUtils.LoadFunctionFromDB(FunctionName, ref Arity, ref TerminalParameters,ref UserCode, LanguageID))
				{
					//
					// Save the user code for the program writer
					m_ILanguageWriter.AddFunction(FunctionName, Arity, UserCode);
				}
				else
				{
					m_ILanguageWriter = null;
					return false;	// Let the UI know, this isn't going to work
				}
			}

			return true;
		}

		private void btnProgramExport_Click(object sender, EventArgs e)
		{
			//
			// Prepare the file extension, based upon the language radio button currently selected.
			string FileExtension = "";
			string Filter = "";
			switch (tsmenuLanguage.Text)
			{
				case GPEnums.LANGUAGE_C:
					FileExtension = ".c";
					Filter = "C Files|*.c";
					break;
				case GPEnums.LANGUAGE_CPP:
					FileExtension = ".cpp";
					Filter = "C++ Files|*.cpp";
					break;
				case GPEnums.LANGUAGE_CSHARP:
					FileExtension = ".cs";
					Filter = "C# Files|*.cs";
					break;
				case GPEnums.LANGUAGE_VBNET:
					FileExtension = ".vb";
					Filter = "Visual Basic.NET Files|*.vb";
					break;
				case GPEnums.LANGUAGE_JAVA:
					FileExtension = ".java";
					Filter = "Java Files|*.java";
					break;
				case GPEnums.LANGUAGE_FORTRAN:
					FileExtension = ".f";
					Filter = "Fortran Files|*.f";
					break;
			}

			dlgFileSaveSource.DefaultExt = FileExtension;
			dlgFileSaveSource.Filter = Filter;
			dlgFileSaveSource.FileName = "GeneticProgram" + FileExtension;
			if (dlgFileSaveSource.ShowDialog() == DialogResult.OK)
			{
				//
				// Save it!  Write it out one line at a time, because the text control doesn't
				// use the carriage return-line feed in the same way a file expects.  Therefore,
				// writing it line by line makes the file "look" right to all text editors.

				//
				// If this is a C++ program, save to separate .h and .cpp files
				if (tsmenuLanguage.Text == GPEnums.LANGUAGE_CPP)
				{
					//
					// Write the header section first
					String HeaderFilename = dlgFileSaveSource.FileName;
					HeaderFilename=HeaderFilename.Replace(FileExtension, ".h");
					int SourceLine = 0;
					bool WriteHeader = true;
					using (StreamWriter sw = new StreamWriter(HeaderFilename))
					{
						while (WriteHeader)
						{
							if (txtProgram.Lines[SourceLine].StartsWith("// --- The section"))
							{
								SourceLine++;
								WriteHeader = false;
							}
							else
							{
								sw.WriteLine(txtProgram.Lines[SourceLine++]);
							}
						}
					}
					//
					// Write the .cpp file
					using (StreamWriter sw=new StreamWriter(dlgFileSaveSource.FileName))
					{
						while (SourceLine < txtProgram.Lines.Length)
						{
							sw.WriteLine(txtProgram.Lines[SourceLine++]);
						}
					}
				}
				else
				{
					using (StreamWriter sw = new StreamWriter(dlgFileSaveSource.FileName))
					{
						foreach (string Line in txtProgram.Lines)
						{
							sw.WriteLine(Line);
						}
					}
				}
			}
		}

		private void tsExportCSV_Click(object sender, EventArgs e)
		{
			ExportGridToFile(",", ".csv","Comma Separated Files|*.csv");
		}

		private void tsExportSSV_Click(object sender, EventArgs e)
		{
			ExportGridToFile(";", ".ssv","Semi-Colon Separated Files|*.ssv");
		}

		private void tsExportTab_Click(object sender, EventArgs e)
		{
			ExportGridToFile("\t", ".txt","Tab Separated Files|*.txt");
		}

		/// <summary>
		/// Exports the grid to a file.  This function obtains the root filename
		/// from the user, and uses the delimeter and file extension given as parameters
		/// </summary>
		/// <param name="Delimeter">Value separator</param>
		/// <param name="FileExtension">Filename extension</param>
		private void ExportGridToFile(string Delimeter,string FileExtension,string Filter)
		{
			dlgFileSaveGrid.DefaultExt=FileExtension;
			dlgFileSaveGrid.FileName = "Results" + FileExtension;
			dlgFileSaveGrid.Filter = Filter;
			if (dlgFileSaveGrid.ShowDialog() == DialogResult.OK)
			{
				using (TextWriter tw = new StreamWriter(dlgFileSaveGrid.FileName))
				{
					//
					// Write the column headers
					foreach (DataGridViewColumn Column in dgResults.Columns)
					{
						tw.Write(Column.Name + Delimeter);
					}
					tw.WriteLine();

					foreach (DataGridViewRow Row in dgResults.Rows)
					{
						foreach (DataGridViewCell Cell in Row.Cells)
						{
							tw.Write(Cell.Value + Delimeter);
						}
						tw.WriteLine();
					}
				}
			}
		}


		#region IBatchClient Implementation

		private List<object> m_BatchProcessID = new List<object>();
		private List<string> m_BatchProcessProject = new List<string>();
		private List<string> m_BatchProcessProfile = new List<string>();

		/// <summary>
		/// Grab the ProcessID and associate it with the ProjectName and ProfileName, we need these to
		/// figure out when a process completes whether or not to refresh the results view.
		/// </summary>
		public void AddProcess(object ProcessID, string ProjectName, string ProfileName, DateTime TimeAdded, DateTime TimeStarted, bool IsStarted)
		{
			m_BatchProcessID.Add(ProcessID);
			m_BatchProcessProject.Add(ProjectName);
			m_BatchProcessProfile.Add(ProfileName);
		}

		public void ProcessStarted(object ProcessID, DateTime TimeStarted, int MaxGenerations)
		{	// Don't care
		}

		public void ProcessUpdate(int Generation)
		{	// Don't care
		}

		public void ProcessStatus(string Message)
		{	// Don't care
		}

		private delegate void DEL_ProcessComplete(object ProcessID, DateTime TimeComplete, bool Canceled, double Fitness, int Hits, int Complexity);
		/// <summary>
		/// Have to make this baby thread safe for the UI access
		/// </summary>
		public void ProcessComplete(object ProcessID, DateTime TimeComplete, bool Canceled, double Fitness, int Hits, int Complexity)
		{
			if (this.lvProfilesResults.InvokeRequired)
			{
				DEL_ProcessComplete d = new DEL_ProcessComplete(ProcessComplete);
				this.Invoke(d, new object[] { ProcessID, TimeComplete, Canceled, Fitness, Hits, Complexity });
			}
			else
			{
				//
				// Decide if this is a project/profile combination this page is currently viewing, if so,
				// refresh the results.
				m_FindProcessID=ProcessID;
				int BatchIndex = m_BatchProcessID.FindIndex(FindIndex);
				if (BatchIndex >= 0 && m_BatchProcessProject[BatchIndex] == m_Project.Name)
				{
					if (lvProfilesResults.SelectedItems.Count > 0)
					{
						if (lvProfilesResults.SelectedItems[0].Text == m_BatchProcessProfile[BatchIndex])
						{
							lvProfilesResults_SelectedIndexChanged(this, null);
						}
					}
				}
			}
		}

		private static object m_FindProcessID;
		private static bool FindIndex(object obj)
		{
			if (obj == m_FindProcessID)
				return true;
			
			return false;
		}

		public int CompareTo(IBatchClient other)
		{
			if (other == this)
			{
				return 0;
			}

			return 1;
		}

		#endregion
	}
}
