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
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using GPStudio.Shared;
using GPStudio.Interfaces;
using System.Runtime.Remoting.Lifetime;

namespace GPStudio.Client
{
	/// <summary>
	/// This class manages a modeling session.  It is responsible for the management
	/// of all the remote server objects as part of its effort to get a model created.
	/// </summary>
	public class GPModeler
	{
		/// <summary>
		/// Constructor that passes in the various properties the modeler needs from
		/// the UI to manage the remote objects and to report back to the UI as things
		/// take place.
		/// </summary>
		public GPModeler(GPProjectProfile profile,int TrainingID,DEL_ValidatedServer delValidatedServer,DEL_ReportStatus delStatus,DEL_ReportFitness delFitness,DEL_GenerationComplete delGenerationComplete)
		{
			m_Profile = profile;
			m_TrainingID = TrainingID;
			m_DELValidatedServer=delValidatedServer;
			m_DELReportStatus = delStatus;
			m_DELReportFitness = delFitness;
			m_DELGenerationComplete = delGenerationComplete;
		}

		/// <summary>
		/// Delegate that lets the modeler report back to the UI that a new
		/// server has been validated, so it can update accordingly.
		/// </summary>
		/// <param name="Test"></param>
		public delegate void DEL_ValidatedServer(String Test);
		/// <summary>
		/// Delegate that allows the status bar of the modeling page to
		/// be updated with a string.
		/// </summary>
		public delegate void DEL_ReportStatus(String Text);
		/// <summary>
		/// Delegate that reports the generation stats for a single server
		/// </summary>
		public delegate void DEL_ReportFitness(int Server, int Generation, ModelingResults.ServerData Stats);
		/// <summary>
		/// Delegate that indicates all reporting for a generation is complete; indicates
		/// it is time to update the UI.
		/// </summary>
		public delegate void DEL_GenerationComplete();

		protected GPProjectProfile m_Profile;
		protected int m_TrainingID;
		protected DEL_ValidatedServer m_DELValidatedServer;
		protected DEL_ReportStatus m_DELReportStatus;
		protected DEL_ReportFitness m_DELReportFitness;
		protected DEL_GenerationComplete m_DELGenerationComplete;

		/// <summary>
		/// Public method that goes about getting the program modeled.  This is
		/// intended to be called asynchronously...
		/// </summary>
		public void RequestProgram()
		{
			//
			// Load up the distributed servers
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Validating distributed server connections...");
			}
			fmMain.ServerManager.RegisterServersFromXml(Application.StartupPath + "\\Servers.xml");

			//
			// Get the distributed servers initialized...training data, function sets, etc.
			List<IGPModeler> Modelers = null;
			GPModelingData Training = null;
			InitializeServers(fmMain.ServerManager,ref Modelers, ref Training);
			m_PossibleHits = Training.Rows;

			//
			// Do the modeling
			ExecuteSimulation(Modelers, Training);
		}

		/// <summary>
		/// We are going to asynchronously call the 'EvaluateFitness' method
		/// of the IGPServer interface.  This allows us to call into the modeler
		/// to abort a session in process very quickly.
		/// </summary>
		/// <param name="Generation"></param>
		private delegate void EvaluateFitnessDEL(int Generation);


		/// <summary>
		/// Manages the modeling simulation.
		/// </summary>
		/// <param name="Modelers">Collection of distributed modeler interfaces</param>
		/// <param name="Training">Data set being used for the modeling</param>
		private void ExecuteSimulation(List<IGPModeler> Modelers, GPModelingData Training)
		{
			m_AbortSession = false;
			m_HaveBestProgram = false;
			m_BestProgram = "";

			//
			// Go through all the runs...but stop if a perfect program is found
			bool DoneSimulation = false;
			//
			// Create the initial population
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Initializing Population...");
			}
			InitializePopulations(Modelers, m_Profile.ModelingProfile.PopulationSize);

			//
			// Go through all generations...but stop if a perfect program is found
			int Generation = 0;
			while (!DoneSimulation)
			{
#if GPLOG
				GPLog.ReportLine("Computing Fitness for Generation: "+Generation,true);
#endif
				if (m_DELReportStatus != null)
				{
					m_DELReportStatus("Computing Fitness for Generation " + (Generation + 1) + "...");
				}

				//
				// Let's block (for now) as each generation is computed
				EvaluateFitness(Modelers, Generation);

				List<String> BestPrograms = null;
				if (!m_AbortSession)
				{
					//
					// Grab the current best program and stats
					m_HaveBestProgram = true;
					UpdateGenerationResults(Generation,Modelers,ref BestPrograms);
				}

				//
				// find out if we are done
				DoneSimulation = IsModelRunDone(m_BestFitness, m_BestHits, Generation, Training.Training);

				if (!DoneSimulation)
				{
					//
					// Instruct the server to create the next population
					if (m_DELReportStatus != null)
					{
						m_DELReportStatus("Updating Population...");
					}
					UpdatePopulations(Modelers, Generation);
				}

				Generation++;
			}

#if GPLOG
			GPLog.Report("Simulation Complete, cleaning up modelers...",true);
#endif

			//
			// Instruct each modeler to clean up as much as possible
			foreach (IGPModeler iModeler in Modelers)
			{
				iModeler.ForceCleanup();

				//
				// Unregister it for the function set associated with the modeler
				ILease LeaseInfo = (ILease)((MarshalByRefObject)iModeler.FunctionSet).GetLifetimeService();
				LeaseInfo.Unregister(m_FunctionSetSponsor);

				//
				// Unregister the lease sponsor for the modeler
				LeaseInfo = (ILease)((MarshalByRefObject)iModeler).GetLifetimeService();
				LeaseInfo.Unregister(m_ModelerSponsor);
			}

#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif

			m_Modelers = null;
			m_ModelerSponsor = null;
			m_FunctionSetSponsor = null;
			System.GC.Collect();
		}

		/// <summary>
		/// Delegates used to support the async population update calls to the remote servers
		/// </summary>
		/// <param name="Generation"></param>
		private delegate void DEL_IComputeNextGeneration(int Generation);
		private delegate void DEL_IDistributePopulation();

		/// <summary>
		/// Instructs each modeler to update its population
		/// </summary>
		/// <param name="Modelers"></param>
		/// <param name="Generation"></param>
		private void UpdatePopulations(List<IGPModeler> Modelers, int Generation)
		{
#if GPLOG
			GPLog.Report("Distributing Programs...",true);
#endif
			//
			// Tell each server to distribute its programs, use async calls so everybody
			// works asynchronously.
			List<DEL_IDistributePopulation> DelegatesDistribute = new List<DEL_IDistributePopulation>(Modelers.Count);
			List<IAsyncResult> ResultsDistribute = new List<IAsyncResult>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				DEL_IDistributePopulation del = new DEL_IDistributePopulation(Modelers[ModelerIndex].DistributePopulation);
				IAsyncResult ar = del.BeginInvoke(null, null);

				DelegatesDistribute.Add(del);
				ResultsDistribute.Add(ar);
			}

			//
			// Go into a loop to wait for all the method calls to complete
			for (int Delegate = 0; Delegate < DelegatesDistribute.Count; Delegate++)
			{
				DelegatesDistribute[Delegate].EndInvoke(ResultsDistribute[Delegate]);
			}

#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif

#if GPLOG
			GPLog.Report("Computing next populations...",true);
#endif

			//
			// Tell each server to compute its next population generation
			// Make all calls asynchronously
			List<DEL_IComputeNextGeneration> DelegateNext = new List<DEL_IComputeNextGeneration>(Modelers.Count);
			List<IAsyncResult> ResultNext = new List<IAsyncResult>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				DEL_IComputeNextGeneration del = new DEL_IComputeNextGeneration(Modelers[ModelerIndex].ComputeNextGeneration);
				IAsyncResult ar = del.BeginInvoke(Generation, null, null);

				DelegateNext.Add(del);
				ResultNext.Add(ar);
			}

			//
			// Go into a loop to wait for all the method calls to complete
			for (int Delegate = 0; Delegate < DelegateNext.Count; Delegate++)
			{
				DelegateNext[Delegate].EndInvoke(ResultNext[Delegate]);
			}
#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif
		}

		/// <summary>
		/// Delegate used to asynchronously obtain population stats from each modeler
		/// </summary>
		private delegate void DEL_IGetPopulationStats(out double PopulationFitnessMax, out double PopulationFitnessAve, out int PopulationComplexityMin, out int PopulationComplexityMax, out int PopulationComplexityAve);

		/// <summary>
		/// Works through the distributed servers to obtain the population statistics
		/// </summary>
		/// <param name="Modelers"></param>
		/// <param name="PopulationFitnessMax"></param>
		/// <param name="PopulationFitnessAve"></param>
		/// <param name="PopulationComplexityMin"></param>
		/// <param name="PopulationComplexityMax"></param>
		/// <param name="PopulationComplexityAve"></param>
		private void GetPopulationStats(List<IGPModeler> Modelers,ref double PopulationFitnessMax, ref double PopulationFitnessAve, ref int PopulationComplexityMin, ref int PopulationComplexityMax, ref int PopulationComplexityAve)
		{
			double tPopulationFitnessMax=0;
			double tPopulationFitnessAve=0;
			int tPopulationComplexityMin=100000;	// Some big huge number
			int tPopulationComplexityMax=0;
			int tPopulationComplexityAve=0;

			//
			// Make all calls asynchronously - This, "in theory", should be faster
			// than make each call, one after the other, and waiting for each result
			// before getting the next.
			List<DEL_IGetPopulationStats> DelegateList = new List<DEL_IGetPopulationStats>(Modelers.Count);
			List<IAsyncResult> ResultList = new List<IAsyncResult>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				DEL_IGetPopulationStats del = new DEL_IGetPopulationStats(Modelers[ModelerIndex].GetPopulationStats);
				IAsyncResult ar = del.BeginInvoke(
					out tPopulationFitnessMax,
					out tPopulationFitnessAve,
					out tPopulationComplexityMin,
					out tPopulationComplexityMax,
					out tPopulationComplexityAve, null, null);

				DelegateList.Add(del);
				ResultList.Add(ar);
			}

			double FitnessTotal = 0.0;
			int ComplexityTotal = 0;
			//
			// Go into a loop to wait for all the method calls to complete.  As
			// they complete, collect the results and update as needed.
			for (int Delegate = 0; Delegate < DelegateList.Count; Delegate++)
			{
				DelegateList[Delegate].EndInvoke(
					out tPopulationFitnessMax,
					out tPopulationFitnessAve,
					out tPopulationComplexityMin,
					out tPopulationComplexityMax,
					out tPopulationComplexityAve,
					ResultList[Delegate]);

				if (tPopulationFitnessMax > PopulationFitnessMax)
					PopulationFitnessMax = tPopulationFitnessMax;

				if (tPopulationComplexityMin < PopulationComplexityMin)
					PopulationComplexityMin = tPopulationComplexityMin;

				if (tPopulationComplexityMax > PopulationComplexityMax)
					PopulationComplexityMax = tPopulationComplexityMax;

				FitnessTotal += tPopulationFitnessAve;
				ComplexityTotal += tPopulationComplexityAve;
			}

			PopulationFitnessAve = FitnessTotal / Modelers.Count;
			PopulationComplexityAve = ComplexityTotal / Modelers.Count;
		}

		/// <summary>
		/// String of the current best program of the modeling run
		/// </summary>
		private String m_BestProgram;
		private bool m_HaveBestProgram;	// Flag to indicate if a best program has been returned
		private double m_BestFitness;


		private int m_BestHits;
		private int m_BestComplexity;
		private int m_PossibleHits;

		/// <summary>
		/// Works through the set of distributed servers, gets the best program
		/// and records the statistics generated at each server.
		/// </summary>
		private void UpdateGenerationResults(int Generation,List<IGPModeler> Modelers, ref List<String> BestPrograms)
		{
			//
			// Collect best program from each modeler
			BestPrograms = new List<string>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				BestPrograms.Add(Modelers[ModelerIndex].BestProgram);
			}

#if GPLOG
			GPLog.ReportLine("Number of best programs returned: " + BestPrograms.Count,true);
#endif

			m_BestFitness = 1000000000.0; // Some crazy big value
			m_BestComplexity = 10000000; // Some crazy big value
			m_BestHits = 0;	// Worst possible value

#if GPLOG
			GPLog.ReportLine("Finding Best program among servers...",true);
#endif

			//
			// Collect the best program and population stats from each server
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				ModelingResults.ServerData Stats=new ModelingResults.ServerData();

				//
				// Best program
				Modelers[ModelerIndex].GetBestProgramStats(
					out Stats.BestFitness,
					out Stats.BestHits,
					out Stats.BestComplexity);

				//
				// Population stats
				Modelers[ModelerIndex].GetPopulationStats(
					out Stats.FitnessMaximum,
					out Stats.FitnessAverage,
					out Stats.ComplexityMinimum,
					out Stats.ComplexityMaximum,
					out Stats.ComplexityAverage);
				Stats.FitnessMinimum = Stats.BestFitness;

				//
				// Reports this back to the UI
				if (m_DELReportFitness != null)
				{
					m_DELReportFitness(Generation, ModelerIndex, Stats);
				}

#if GPLOG
				GPLog.ReportLine("     Testing Program - Fitness: " + Stats.BestFitness + " Hits: " + Stats.BestHits + " Complexity: " + Stats.BestComplexity, false);
#endif

				//
				// Track the best program string
				if (Stats.BestFitness < m_BestFitness)
				{
					m_BestProgram = BestPrograms[ModelerIndex];
					m_BestFitness = Stats.BestFitness;
					m_BestHits = Stats.BestHits;
					m_BestComplexity = Stats.BestComplexity;
				}
				else if (Stats.BestFitness == m_BestFitness && Stats.BestComplexity < m_BestComplexity)
				{
					m_BestProgram = BestPrograms[ModelerIndex];
					m_BestFitness = Stats.BestFitness;
					m_BestHits = Stats.BestHits;
					m_BestComplexity = Stats.BestComplexity;
				}
			}

#if GPLOG
			if (BestUpdated)
			{
				GPLog.ReportLine("     A best program was found", false);
			}
			else
			{
				GPLog.ReportLine("     No best program was updated!!",false);
				GPLog.ReportLine("     Modeler Count: " + Modelers.Count, false);
			}
#endif

			if (m_DELGenerationComplete != null)
			{
				m_DELGenerationComplete();
			}
		}

		/// <summary>
		/// Delegate used to support the async calls to the remote servers
		/// </summary>
		/// <param name="Generation"></param>
		private delegate void DEL_IEvaluateFitness(int Generation);
		/// <summary>
		/// Instructs each server to evaluate the fitness of their populations.  Each
		/// call is async, but this method blocks until all have completed.
		/// </summary>
		/// <param name="Modelers"></param>
		/// <param name="Generation"></param>
		private void EvaluateFitness(List<IGPModeler> Modelers, int Generation)
		{
			//
			// Make all calls asynchronously
			List<DEL_IEvaluateFitness> DelegateList = new List<DEL_IEvaluateFitness>(Modelers.Count);
			List<IAsyncResult> ResultList = new List<IAsyncResult>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				DEL_IEvaluateFitness del = new DEL_IEvaluateFitness(Modelers[ModelerIndex].EvaluateFitness);
				IAsyncResult ar = del.BeginInvoke(Generation, null, null);

				DelegateList.Add(del);
				ResultList.Add(ar);
			}

			//
			// Go into a loop to wait for all the method calls to complete
			for (int Delegate = 0; Delegate < DelegateList.Count; Delegate++)
			{
				DelegateList[Delegate].EndInvoke(ResultList[Delegate]);
			}
		}
		private List<IGPModeler> m_Modelers;

		/// <summary>
		/// Delegate used to support the async calls to the remote servers
		/// </summary>
		/// <param name="PopulationSize"></param>
		private delegate void DEL_IInitializePopulation(int PopulationSize);
		/// <summary>
		/// Instructs each server to build their initial populations.  Each call to
		/// the server is async, but this method blocks until all populations are
		/// constructed.
		/// </summary>
		/// <param name="Modelers"></param>
		/// <param name="TotalPopulation"></param>
		private void InitializePopulations(List<IGPModeler> Modelers, int TotalPopulation)
		{
			//
			// Evenly split up the population, according to the following rules
			// 1.  All populations must be even numbered in size
			// 3.  Granularity of 100
			// 3.  Minimum of 100
			int PopulationEach = TotalPopulation / Modelers.Count;
			PopulationEach = (PopulationEach / 100) * 100;
			PopulationEach = Math.Max(PopulationEach, 100);

#if GPLOG
			GPLog.ReportLine("Initializing Populations.  Size @ each server: "+PopulationEach,true);
#endif

			//
			// Make all calls asynchronously
			List<DEL_IInitializePopulation> DelegateList = new List<DEL_IInitializePopulation>(Modelers.Count);
			List<IAsyncResult> ResultList = new List<IAsyncResult>(Modelers.Count);
			for (int ModelerIndex = 0; ModelerIndex < Modelers.Count; ModelerIndex++)
			{
				DEL_IInitializePopulation delInit = new DEL_IInitializePopulation(Modelers[ModelerIndex].InitializePopulation);
				IAsyncResult ar = delInit.BeginInvoke(PopulationEach, null, null);

				DelegateList.Add(delInit);
				ResultList.Add(ar);
			}

			//
			// Go into a loop to wait for all the method calls to complete
			for (int Delegate = 0; Delegate < DelegateList.Count; Delegate++)
			{
				DelegateList[Delegate].EndInvoke(ResultList[Delegate]);
			}

#if GPLOG
			GPLog.ReportLine("Population Initialization Complete",true);
#endif
		}

		/// <summary>
		/// Prepares the configuration settings at each server.
		///		*Training Data
		///		*UDFs
		///		*Fitness Function
		///		*Everything else
		/// This method builds a list of modeler interfaces at each server.  Remember,
		/// each time the Modeler property is used from IGPServer, a NEW modeler is
		/// created...so only want to do this once for each modeling session; hence,
		/// have to access it and keep track of it on the client side.
		/// </summary>
		/// <param name="ServerManager"></param>
		/// <param name="Modelers"></param>
		/// <param name="Training"></param>
		/// <returns>True/False upon success or failure</returns>
		private bool InitializeServers(ServerManagerSingleton ServerManager,ref List<IGPModeler> Modelers, ref GPModelingData Training)
		{
#if GPLOG
			GPLog.ReportLine("Initializing Distributed Servers...",true);
#endif
			//
			// Create a modeler sponsor
			m_FunctionSetSponsor = fmMain.SponsorServer.Create("FunctionSet");
			m_ModelerSponsor = fmMain.SponsorServer.Create("Modeler");

			//
			// Build a list of modeler interfaces
#if GPLOG
			GPLog.ReportLine("Number of GPServers: "+ServerManager.Count,true);
#endif
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Enumerating distributed servers...");
			}
			List<IGPServer> Servers = new List<IGPServer>(ServerManager.Count);
			Modelers = new List<IGPModeler>(ServerManager.Count);
			foreach (IGPServer iServer in ServerManager)
			{
				Modelers.Add(iServer.Modeler);

				//
				// We also keep track of the servers so we set the right function set
				// with the right server.
				Servers.Add(iServer);
			}
			m_Modelers = Modelers;

			//
			// Based upon the selected topology, send interfaces to the connected
			// servers.
			switch (m_Profile.ModelingProfile.DistributedTopology)
			{
				case GPEnums.DistributedTopology.Ring:
					InitializeServersTopologyRing(Modelers);
					break;
				case GPEnums.DistributedTopology.Star:
					InitializeServersTopologyStar(Modelers);
					break;
			}

			//
			// Report the list of servers to the UI - they're ordered the same as the interfaces,
			// so the foreach is a safe way to do it.
			foreach (String Description in ServerManager.Descriptions.Values)
			{
#if GPLOG
				GPLog.ReportLine("Server Description: "+Description,true);
#endif
				//
				// Report back to the UI we have a new valid server to record data for
				if (m_DELValidatedServer != null)
				{
					m_DELValidatedServer(Description);
				}
			}

			//
			// Transimit the training data
#if GPLOG
			GPLog.Report("Loading training data...",true);
#endif
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Loading training data...");
			}
			Training = new GPModelingData();
			Training.LoadFromDB(m_TrainingID, null, null);

#if GPLOG
			GPLog.ReportLine("...Complete", false);
#endif

			//
			// Asynchronously call initialization on each modeler
			List<DEL_InitializeModeler> DelegateList = new List<DEL_InitializeModeler>(Servers.Count);
			List<IAsyncResult> ResultList = new List<IAsyncResult>(Servers.Count);
			for (int ServerIndex = 0; ServerIndex < Servers.Count; ServerIndex++)
			{
				//
				// Make an asynchronous call to initialize
				DEL_InitializeModeler delInit = new DEL_InitializeModeler(InitializeModeler);
				IAsyncResult ar = delInit.BeginInvoke(Servers[ServerIndex], Modelers[ServerIndex], Training, null, null);

				DelegateList.Add(delInit);
				ResultList.Add(ar);
			}

			//
			// Go into a loop to wait for all the method calls to complete
			for (int Delegate = 0; Delegate < DelegateList.Count; Delegate++)
			{
				DelegateList[Delegate].EndInvoke(ResultList[Delegate]);
			}

			return true;
		}

		/// <summary>
		/// This is a client side sponsor to manage the lifetime of the function sets created for
		/// the modeling session.  It is emptied when the modeler is destroyed.
		/// </summary>
		private GPServerClientSponsor m_ModelerSponsor;
		private GPServerClientSponsor m_FunctionSetSponsor;

		/// <summary>
		/// Ring Topology: Sends interfaces to the two neighboring servers.  These
		/// are the only two servers programs are sent two between generations.
		/// </summary>
		/// <param name="Modelers"></param>
		private void InitializeServersTopologyRing(List<IGPModeler> Modelers)
		{
			//
			// If fewer than 2 (0 or 1) modelers, nothing to do
			if (Modelers.Count < 2) return;

			//
			// If only 2, handle as a special case.  Give each one a reference
			// to the other.
			if (Modelers.Count == 2)
			{
				Modelers[0].AddModeler(Modelers[1]);
				Modelers[1].AddModeler(Modelers[0]);

				return;
			}

			//
			// First server gets the last one.
			Modelers[0].AddModeler(Modelers[Modelers.Count - 1]);
			Modelers[0].AddModeler(Modelers[1]);

			//
			// Last server gets the first one
			Modelers[Modelers.Count - 1].AddModeler(Modelers[0]);
			Modelers[Modelers.Count - 1].AddModeler(Modelers[Modelers.Count - 2]);

			//
			// Now, handle the rest of the servers and form them into a ring configuration
			for (int Position = 1; Position < Modelers.Count-1; Position++)
			{
				Modelers[Position].AddModeler(Modelers[Position - 1]);
				Modelers[Position].AddModeler(Modelers[Position + 1]);
			}
		}

		/// <summary>
		/// Star Topology: Sends interfaces to all servers.  Then, when distribution is to take place,
		/// the server itself computes which % servers to send to.
		/// </summary>
		/// <param name="Modelers"></param>
		private void InitializeServersTopologyStar(List<IGPModeler> Modelers)
		{
			//
			// Send to each server the interfaces to all the other servers.  This is so
			// they can asynchronously contact any other server to transmit programs between
			// generations.
			foreach (IGPModeler iModeler in m_Modelers)
			{
				foreach (IGPModeler iClient in m_Modelers)
				{
					//
					// Don't send an interface to yourself
					if (iModeler != iClient)
					{
						iModeler.AddModeler(iClient);
					}
				}
			}
		}

		/// <summary>
		/// Delegate used to facilitate making an async call to initialize the modelers
		/// </summary>
		private delegate bool DEL_InitializeModeler(IGPServer iServer, IGPModeler iModeler, GPModelingData Training);

		/// <summary>
		/// Gets the settings for an individual modeler prepared.  This function is
		/// intended to be called asynchronously.
		/// </summary>
		/// <param name="iServer"></param>
		/// <param name="iModeler"></param>
		/// <param name="Training"></param>
		/// <returns></returns>
		private bool InitializeModeler(IGPServer iServer, IGPModeler iModeler, GPModelingData Training)
		{
			//
			// The data we want to send is data appropriate for modeling.  Time Series
			// data needs to be transformed for the modeler.
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Loading training data...");
			}
			iModeler.Training = Training.TrainingForModeling(
				m_Profile.ModelType.InputDimension,
				m_Profile.ModelType.PredictionDistance);

			//
			// Prepare the function set for each server
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Transmitting user defined functions...");
			}
			IGPFunctionSet iFunctionSet = iServer.FunctionSet;
			//
			// Use a client side sponsor for this function set
			// TODO: This sponsor should really come from the server we obtained
			// the function set interface from.  The reason it currently isn't, is
			// because the whole "lease lifetime" thing isn't fully thought out yet
			// in this application.
			ILease LeaseInfo = (ILease)((MarshalByRefObject)iFunctionSet).GetLifetimeService();
			LeaseInfo.Register(m_FunctionSetSponsor);

			//
			// Let's go ahead and assign the same lease sponsor to the modeler
			LeaseInfo = (ILease)((MarshalByRefObject)iModeler).GetLifetimeService();
			LeaseInfo.Register(m_ModelerSponsor);

#if GPLOG
			GPLog.ReportLine("Loading UDFs...",true);
#endif

			foreach (string FunctionName in m_Profile.FunctionSet)
			{
#if GPLOG
				GPLog.ReportLine("   UDF: "+FunctionName,false);
#endif
				//
				// Load the Function from the database
				short FunctionArity = 0;
				bool TerminalParameters = false;
				String FunctionCode = "";
				if (GPDatabaseUtils.LoadFunctionFromDB(FunctionName, ref FunctionArity, ref TerminalParameters, ref FunctionCode, GPEnums.LANGUAGEID_CSHARP))
				{
					//
					// Add it to the function set
					iFunctionSet.AddFunction(FunctionName, FunctionArity, TerminalParameters, FunctionCode);
				}
			}
			iFunctionSet.UseMemory = m_Profile.ModelingProfile.UseMemory;

			//
			// Assign it to the modeler
			iModeler.FunctionSet = iFunctionSet;

#if GPLOG
			GPLog.Report("Transmitting fitness function...",true);
#endif

			//
			// Pass the fitness function to each modeler
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Transmitting fitness function...");
			}
			String FitnessFunction = GPDatabaseUtils.FieldValue(m_Profile.FitnessFunctionID, "tblFitnessFunction", "Code");
			iModeler.FitnessFunction = FitnessFunction;

#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif

			//
			// Transmit the profile/modeling settings
			if (m_DELReportStatus != null)
			{
				m_DELReportStatus("Transmitting modeling parameters...");
			}
			iModeler.Profile = m_Profile.ModelingProfile;
			//
			// Send over which ADF and ADLs to evolve
			foreach (byte adf in m_Profile.m_ADFSet)
			{
				iModeler.AddADF(adf);
			}
			foreach (byte adl in m_Profile.m_ADLSet)
			{
				iModeler.AddADL(adl);
			}
			foreach (byte adr in m_Profile.m_ADRSet)
			{
				iModeler.AddADR(adr);
			}

			return true;
		}

		/// <summary>
		/// Checks to see if the conditions for terminating the modeling run are met
		/// </summary>
		/// <returns></returns>
		private bool IsModelRunDone(double BestFitness, int BestHits, int Generation, GPTrainingData Training)
		{
			//
			// See if number of generations has been exhausted
			if ((Generation+1) >= m_Profile.m_maxNumber &&
				m_Profile.m_useMaxNumber)
			{
				return true;
			}

			//
			// See if the number of hits has been maximized
			if (m_Profile.m_useHitsMaxed && BestHits == Training.Rows)
			{
				return true;
			}

			//
			// See if the fitness has geen minimized - Remember to use
			// tolerance to test this.
			if (m_Profile.m_useRawFitness0)
			{
				//
				// Test with tolerance
				if (BestFitness < GPEnums.RESULTS_TOLERANCE && BestFitness > -GPEnums.RESULTS_TOLERANCE)
				{
					return true;
				}
			}

			//
			// Return true if the user has aborted, otherwise, let's keep going
			return m_AbortSession;
		}

		private bool m_AbortSession = false;
		private delegate void DEL_AbortModeler();
		/// <summary>
		/// Allows client code to abort the modeling session.
		/// </summary>
		public void Abort()
		{
			if (m_Modelers == null) return;

			//
			// Asynchronously call the abort method on each modeler
			List<DEL_AbortModeler> DelegateList = new List<DEL_AbortModeler>(m_Modelers.Count);
			List<IAsyncResult> ResultList = new List<IAsyncResult>(m_Modelers.Count);
			for (int Modeler=0; Modeler<m_Modelers.Count; Modeler++)
			{
				//
				// Make an asynchronous call to abort
				DEL_AbortModeler delAbort = new DEL_AbortModeler(m_Modelers[Modeler].Abort);
				IAsyncResult ar = delAbort.BeginInvoke(null, null);

				DelegateList.Add(delAbort);
				ResultList.Add(ar);
			}

			//
			// There are some synchronization issues with the batch processing manager.  By placing
			// this into a try-catch block, those problems are easily resolved.  Is this a great design
			// decision, I'm not sure.
			try
			{
				//
				// Go into a loop to wait for all the method calls to complete
				for (int Delegate = 0; Delegate < DelegateList.Count; Delegate++)
				{
					DelegateList[Delegate].EndInvoke(ResultList[Delegate]);
				}
			}
			catch
			{
			}

			m_AbortSession = true;
		}

		/// <summary>
		/// Save the best program to the database
		/// </summary>
		/// <returns>True/False depending upon success or failure</returns>
		public bool SaveBestToDB(int ProjectID, ref int ProgramID)
		{
#if GPLOG
			GPLog.ReportLine("Saving best program to DB...",true);
#endif
			//
			// Make sure we have something to save
			if (!m_HaveBestProgram)
			{
#if GPLOG
				GPLog.ReportLine("No best program exists, nothing to save",true);
#endif
				return false;
			}

			//
			// Save the program text to a memory stream and then read back from
			// it to get the bytes...pretty slick!
#if GPLOG
				GPLog.Report("Converting to a memory stream...",true);
#endif
			byte[] blobData = null;
			using (MemoryStream msProgram = new MemoryStream())
			{
				StreamWriter strmProgram = new StreamWriter(msProgram);
				strmProgram.Write(m_BestProgram);
				strmProgram.Flush();

				//
				// Reposition the memory stream to the beginning and now read back out of it :)	
				msProgram.Seek(0, SeekOrigin.Begin);
				blobData = new byte[msProgram.Length];
				msProgram.Read(blobData, 0, System.Convert.ToInt32(msProgram.Length));
			}
#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif
#if GPLOG
			GPLog.Report("Creating the insert parameter...",true);
#endif

			OleDbParameter param = new OleDbParameter(
				"@Profile",
				OleDbType.VarBinary,
				blobData.Length,
				ParameterDirection.Input,
				false,
				0, 0, null,
				DataRowVersion.Current, blobData);
#if GPLOG
			GPLog.ReportLine("...Complete",false);
#endif

			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{

				//
				// Build the command to add the blob to the database
				String sSQL = "INSERT INTO tblProgram(ProjectID,Fitness,Hits,MaxHits,Complexity,ModelProfileID,[Date],Program) VALUES ";
				sSQL += "(" + ProjectID;
				sSQL += "," + Convert.ToString(m_BestFitness,GPUtilities.NumericFormat);
				sSQL += "," + m_BestHits;
				sSQL += "," + m_PossibleHits;
				sSQL += "," + m_BestComplexity;
				sSQL += "," + m_Profile.ProfileID;
				sSQL += "," + DateTime.Now.ToString("#yyyy-MM-dd HH:mm:ss#");
				sSQL += ",?)";
				OleDbCommand cmd = new OleDbCommand(sSQL, con);

#if GPLOG
				GPLog.ReportLine("SQL Write Command: "+sSQL,true);
#endif

				cmd.Parameters.Add(param);

				//
				// Execute the command
				ProgramID = 0;
				try
				{
#if GPLOG
					GPLog.Report("Attempting to write to the database...",true);
#endif

					cmd.ExecuteNonQuery();

#if GPLOG
					GPLog.ReportLine("Successful DB write!",false);
#endif
					//
					// Obtain back the DBCode for this model
					cmd.CommandText = "SELECT @@IDENTITY";
					OleDbDataReader reader = cmd.ExecuteReader();
					reader.Read();
					ProgramID = Convert.ToInt32(reader[0].ToString()); ;
					reader.Close();
#if GPLOG
					GPLog.ReportLine("Successfully obtained the project ID",true);
#endif
				}
				catch (OleDbException ex)
				{
#if GPLOG
					GPLog.ReportLine("...Failed!", false);
					GPLog.ReportLine("Exception during database write: "+ex.Message.ToString(),true);
#endif
					string sError = ex.Message.ToString();
					return false;
				}
			}

			return true;
		}
	}
}
