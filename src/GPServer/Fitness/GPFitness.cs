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
using System.Threading;
using GPStudio.Shared;

namespace GPStudio.Server
{
    public class GPFitness
    {
		/// <summary>
		/// Default constructor - Prepare the memory for storing results.
		/// </summary>
		/// <param name="Config">Modeling configuration</param>
		/// <param name="TrainingData">Reference to the training data</param>
		/// <param name="Tolerance">Allowable tolerance around a resulting value for exact matching</param>
		/// <param name="UseInputHistory">True, if the InputHistory parameter is in use</param>
		public GPFitness(GPModelerServer Config,GPTrainingData TrainingData,double Tolerance,bool UseInputHistory)
		{
			m_Config = Config;
			m_TrainingData = TrainingData;
			m_Tolerance = Tolerance;
			m_UseInputHistory = UseInputHistory;

			//
			// Create the contained program fitness selection object
			if (Config.Profile.SPEA2MultiObjective)
			{
				m_FitnessSelection = new GPFitnessSPEA2(Config.Profile.PopulationSize);
			}
			else
			{
				m_FitnessSelection = new GPFitnessSingle(Config.Profile.PopulationSize);
			}

			//
			// Given the training data, compute the maximum possible error, we need
			// this for the adaptive parsimony pressure.
			m_MaximumError=ComputeMaximumError(TrainingData);
			ComputeTrainingStats(TrainingData);

			//
			// Create room for the fitness measures
			InitializeStorage(Config.Profile.PopulationSize);

			m_PrevPopulationSize = Config.Profile.PopulationSize;

			//
			// Have to convert the training data version of the historical inputs
			// into the form that programs utilize.
			TransformHistoricalInputs(TrainingData);

			//
			// The input history for the custom fitness functions is a little
			// different than UDFs because there is no time step during fitness computation,
			// so need to do a little dance to handle that.
			PrepareFitnessInputHistory(TrainingData);

			//
			// Create the processing threads, one for each processor
			InitializeProcessingThreads(Environment.ProcessorCount);
		}

		~GPFitness()
		{
			TerminateProcessingThreads();
		}

		protected GPModelerServer m_Config;
		protected GPTrainingData m_TrainingData;

		/// <summary>
		/// Object that provides the program selection service.
		/// </summary>
		public GPFitnessObjectiveBase FitnessSelection
		{
			get { return m_FitnessSelection; }
			set { m_FitnessSelection = value; }
		}
		protected GPFitnessObjectiveBase m_FitnessSelection;

		/// <summary>
		/// Prepares the storage for the fitness computation
		/// </summary>
		/// <param name="PopulationSize"></param>
		protected void InitializeStorage(int PopulationSize)
		{
			m_FitnessMeasure = new double[PopulationSize];
			m_FitnessHits = new int[PopulationSize];
		}

		/// <summary>
		/// Creates one fitness evaluation thread for each processor on the computer.
		/// </summary>
		/// <param name="ProcessorCount"></param>
		private void InitializeProcessingThreads(int ProcessorCount)
		{
			//
			// Create a suspend wait handle for managing the state of the threads.  This is
			// initially set to not-signaled, meaning that all threads immediately go into
			// a suspended state upon startup.
			wh_Suspend = new EventWaitHandle(false, EventResetMode.ManualReset);

			//
			// Now, create the processing threads
			m_ProcessingThreads = new List<Thread>(ProcessorCount);
			for (int Processor = 0; Processor < ProcessorCount; Processor++)
			{
				Thread NewThread = new Thread(new ThreadStart(EvaluatePopulationThread));
				m_ProcessingThreads.Add(NewThread);

				//
				// go ahead and start the thread...it will suspend itself right away
				NewThread.Start();
			}
		}
		private List<Thread> m_ProcessingThreads;
		private EventWaitHandle wh_Suspend;

		/// <summary>
		/// Clean up any processing thread still alive
		/// </summary>
		public void TerminateProcessingThreads()
		{
			//
			// Make sure we have something to do
			if (m_ProcessingThreads == null) return;

			//
			// This try catch block put here for when running in console mode to allow the
			// program to gracefully shut down.
			try
			{
				foreach (Thread t in m_ProcessingThreads)
				{
					t.Abort();
				}
				m_ProcessingThreads = null;
			}
			catch
			{
			}
		}

		/// <summary>
		/// The GPProgram object needs to work with a generic list of historical
		/// inputs...the reason is discussed elsewhere, but it basically has to do
		/// because of the final source code generation and how that required custom
		/// user defined functions to be written.
		/// 
		/// The first row contains the inputs for the current prediction.  In other words,
		/// the first prediction has a history of the inputs for itself.
		/// 
		/// TODO: This is kind of an ugly wart because we are keeping around two copies
		/// of the same data.  Once in the Training data (where it should be) and one
		/// time here.  At some point, this needs to be dealt with and only one copy
		/// of the data should be kept around.
		/// </summary>
		/// <param name="Training"></param>
		private void TransformHistoricalInputs(GPTrainingData Training)
		{
			m_UserInputHistory = new List<List<List<double>>>(Training.HistoricalDataSets.Length);

			//
			// Start by creating a reference to each full historical set
			foreach (double[][] Set in Training.HistoricalDataSets)
			{
				List<List<double>> TSet = new List<List<double>>(Set.Length);
				m_UserInputHistory.Add(TSet);
			}

			//
			// Next, create the rows for the sets.  We reuse the rows from previous
			// sets into the later sets to save on memory space...in a huge way.
			for (int Row = 0; Row < Training.HistoricalDataSets[Training.HistoricalDataSets.Length-1].Length; Row++)
			{
				//
				// Create each row and add it to all the historical sets as needed
				List<double> TRow = new List<double>(Training.HistoricalDataSets[Training.HistoricalDataSets.Length - 1][Row].Length);
				foreach (double Item in Training.HistoricalDataSets[Training.HistoricalDataSets.Length-1][Row])
				{
					TRow.Add(Item);
				}

				//
				// Go through the sets and add this row accordingly
				for (int Set = Row; Set < m_UserInputHistory.Count; Set++)
				{
					m_UserInputHistory[Set].Add(TRow);
				}
			}
		}
		private List<List<List<double>>> m_UserInputHistory;

		/// <summary>
		/// Construct the input history for the custom fitness functions.  For
		/// regression models, pass the entire input data set it, for time series
		/// models, have to reconstruct the original input series.
		/// </summary>
		/// <param name="TrainingData"></param>
		private void PrepareFitnessInputHistory(GPTrainingData TrainingData)
		{
			if (TrainingData.TimeSeriesSource)
			{
				//
				// Have to create a single column of data, an exact copy of the
				// time series data itself.  This turns out to be the number of rows
				// PLUS the remaining data in the last row.
				m_UserInputHistoryCustomFitness = new List<List<double>>(TrainingData.Rows + TrainingData.Columns - 1);
				for (int Row = 0; Row < TrainingData.Rows; Row++)
				{
					m_UserInputHistoryCustomFitness.Add(new List<double>(1));
					m_UserInputHistoryCustomFitness[Row].Add(TrainingData[Row, 0]);
				}

				//
				// Now, grab the remaing data items in the last row
				for (int Column = 1; Column < TrainingData.Columns; Column++)
				{
					m_UserInputHistoryCustomFitness.Add(new List<double>(1));
					m_UserInputHistoryCustomFitness[TrainingData.Rows + Column - 1].Add(TrainingData[TrainingData.Rows - 1, Column]);
				}
			}
			else
			{
				m_UserInputHistoryCustomFitness = m_UserInputHistory[m_UserInputHistory.Count - 1];
			}
		}
		private List<List<double>> m_UserInputHistoryCustomFitness;

		/// <summary>
		/// Size of the Population from the previous generation.  This is tracked
		/// because memory conditions may require a reduction in Population size
		/// </summary>
		protected int m_PrevPopulationSize;
		/// <summary>
		/// Allowable error tolerance on program computatonal results
		/// </summary>
		protected double m_Tolerance;
		/// <summary>
		/// Maximum error possible for the training set
		/// </summary>
		protected double m_MaximumError;
		/// <summary>
		/// Indicates if time series functions are in use.  If not, then historical
		/// data sets are not created, saving a lot of memory.
		/// </summary>
		protected bool m_UseInputHistory;
		/// <summary>
		/// Average value of the training samples
		/// </summary>
		protected double m_TrainingAverage;
		/// <summary>
		/// Minimum value of the training samples
		/// </summary>
		protected double m_TrainingMin;
		/// <summary>
		/// Maximum value of the training samples
		/// </summary>
		protected double m_TrainingMax;

		/// <summary>
		/// Computes the maximum possible error for the training data.  A factor
		/// of 10 is used on the max error, because the reality is that programs can
		/// actually create error larger than the "max error" because they might return
		/// funky constants.
		/// </summary>
		/// <param name="Training">Training data</param>
		/// <returns>Maximum error</returns>
		private double ComputeMaximumError(GPTrainingData Training)
		{
			//
			// Compute the maximum possible error
			double MaximumError = 0.0;
			for (int Value = 0; Value < Training.Rows; Value++)
			{
				MaximumError += Math.Abs(Training.ObjectiveRow(Value)[0]);
			}

		return MaximumError*10.0;
		}

		/// <summary>
		/// Compute the average, min and max value of the training data
		/// </summary>
		/// <param name="Training">Training data</param>
		private void ComputeTrainingStats(GPTrainingData Training)
		{
			m_TrainingMax = Training.ObjectiveRow(0)[0];
			m_TrainingMin = Training.ObjectiveRow(0)[0];

			double Total = 0.0;
			for (int Value = 0; Value < Training.Rows; Value++)
			{
				Total += Training.ObjectiveRow(Value)[0];
				m_TrainingMax = Math.Max(m_TrainingMax, Training.ObjectiveRow(Value)[0]);
				m_TrainingMin = Math.Min(m_TrainingMin, Training.ObjectiveRow(Value)[0]);
			}

			m_TrainingAverage = Total / Training.Rows;
		}

		/// <summary>
		/// Measure of raw fitness
		/// </summary>
		public double[] FitnessMeasure
		{
			get { return m_FitnessMeasure; }
		}
		protected double[] m_FitnessMeasure;	// All fitness computations place results here


		/// <summary>
		/// Number of exact matches versus the training data
		/// </summary>
		protected int[] m_FitnessHits;	// All fitness computations have to track this

		/// <summary>
		/// Index into the Population of the best program
		/// </summary>
		public int BestProgram
		{
			get { return m_BestProgram; }
			set { m_BestProgram = value; }
		}
		private int m_BestProgram;

		/// <summary>
		/// Object reference to the best program in the Population
		/// </summary>
		public GPProgram BestProgramRef
		{
			get { return m_BestProgramRef; }
			set { m_BestProgramRef = value; }
		}
		private GPProgram m_BestProgramRef;

		/// <summary>
		/// Fitness of the best program
		/// </summary>
		public double BestFitness
		{
			get { return m_FitnessMeasure[BestProgram]; }
		}

		/// <summary>
		/// Property that indicates if the fitness has gone to 0
		/// </summary>
		public bool FitnessMinimized
		{
			get
			{
				if (BestFitness <= m_Tolerance && BestFitness >= 0-m_Tolerance)
				{
					return true;
				}

			return false;
			}
		}

		/// <summary>
		/// Number of hits the best program produced
		/// </summary>
		public int BestFitnessHits
		{
			get { return m_FitnessHits[BestProgram]; }
		}

		/// <summary>
		/// Property that indicates whether or not the hits have been maxed out
		/// </summary>
		public bool HitsMaximized
		{
			get
			{
				if (BestFitnessHits == m_TrainingData.Rows)
				{
					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Current worst fitness value in the Population.
		/// </summary>
		public double FitnessMaximum
		{
			get { return m_FitnessMaximum; }
			set { m_FitnessMaximum = value; }
		}
		private double m_FitnessMaximum;

		/// <summary>
		/// Current average fitness value in the Population.
		/// </summary>
		public double FitnessAverage
		{
			get { return m_FitnessAverage; }
			set { m_FitnessAverage = value; }
		}
		private double m_FitnessAverage;

		/// <summary>
		/// Performs the fitness computation over the Population
		/// </summary>
		/// <param name="Generation"></param>
		/// <param name="Population"></param>
		/// <returns></returns>
		public GPProgram Compute(int Generation, GPPopulation Population)
		{
			m_Abort = false;
			//
			// Go through the Population and compute the fitness of each
			// program, returning the best program index.
			BestProgram = EvaluatePopulation(Generation, Population);
			m_BestProgramRef = Population.Programs[BestProgram];
			if (!m_Abort)
			{
				m_FitnessSelection.PrepareFitness(this.FitnessMeasure, Population);
			}

			//
			// return the best program
			return m_BestProgramRef;
		}

		/// <summary>
		/// Works through the Population to compute the fitness for each program, keeping
		/// track of a few statistics, such as best program, worst fitness and
		/// average fitness.  Makes a call to the abstract method "ComputeFitness" that
		/// must be implemented by all GPFitness derived classes.
		/// </summary>
		/// <param name="Generation">Current modeling generation</param>
		/// <param name="Population">Current generation Population of programs</param>
		/// <returns></returns>
		private int EvaluatePopulation(int Generation, GPPopulation Population)
		{
			//
			// Check to see if the Population size changed, if so, reallocate the array
			// that holds the fitness values
			if (Population.Count != m_PrevPopulationSize)
			{
				if (m_Config.Profile.SPEA2MultiObjective)
				{
					m_FitnessSelection = new GPFitnessSPEA2(Population.Count);
				}
				else
				{
					m_FitnessSelection = new GPFitnessSingle(Population.Count);
				}
			}

			FitnessMaximum = 0.0;
			FitnessAverage = 0.0;

			//
			// Go through each program in the Population.  First, compute the raw
			// fitness result of the program.  Once that is done, a call to the
			// abstract ComputeFitness function is made where the User Defined Fitness
			// function is called.
			m_BestProgramGeneration = 0;
			m_WorstProgramGeneration = 0;
			m_CurrentProgram = 0;
			m_CompletedPrograms = 0;
			m_Population = Population;

			//
			// Create the event to be signed when the last program is done processing
			wh_LastProgramDone = new EventWaitHandle(false, EventResetMode.ManualReset);

			//
			// Signal the suspend event
			wh_Suspend.Set();

			//
			// Wait for the last program to be computed
			wh_LastProgramDone.WaitOne();

			//
			// Finish up the average Population fitness
			FitnessAverage /= Population.Count;

			return m_BestProgramGeneration;
		}

		/// <summary>
		/// Shared members for the multi-threaded fitness evaluation
		/// </summary>
		private EventWaitHandle wh_LastProgramDone;
		private GPPopulation m_Population;
		private int m_CurrentProgram;
		private int m_CompletedPrograms;
		private int m_BestProgramGeneration;
		private int m_WorstProgramGeneration;
		private object m_LockCurrentProgram = new object();
		private object m_LockFitnessStat = new object();
		private object m_LockCompletedPrograms = new object();

		/// <summary>
		/// All processing threads use this function to coordinate their activity in computing
		/// the fitness for each program in the Population.
		/// Each thread grabs (up to) 4 programs to evaluate
		/// TODO: Obviously, this needs to be a parameterized value, or at least,
		/// it needs to be based upon the number of processors detected, along
		/// with some research that suggests the optimum batch size for each thread
		/// to compute.
		/// </summary>
		private void EvaluatePopulationThread()
		{
			const int BATCHCOUNT = 5;
			//
			// Need an array to store fitness values for each thread.  This is done
			// once for each thread to keep from re-allocating this for each set
			// of programs evaluated.
			double[] Fitness = new double[BATCHCOUNT];
			//
			// Create an array to hold the intermediate test case results
			double[] Predictions = new double[m_TrainingData.Rows];

			try
			{
				//
				// First time in, suspend the thead
				wh_Suspend.WaitOne();

				//
				// Go into an infinite processing loop.  The thread, when no longer needed, will be
				// terminated with an .Abort() call.
				while (true)
				{
					//
					// Check to see if there is another set of programs to process, 
					// if not, suspend the thread
					bool Suspend = false;
					lock (m_LockCurrentProgram)
					{
						if (m_CurrentProgram == m_Population.Count || m_Abort)
						{
							Suspend = true;
						}
					}
					if (Suspend)
					{
						wh_LastProgramDone.WaitOne();
						wh_Suspend.WaitOne();
					}

					//
					// Evaluate (up to) the next BATCHCOUNT programs
					// Be sure to synchronize with the other threads
					int ThreadProgram;
					int ThreadProgramsLeft = 0;
					lock (m_LockCurrentProgram)
					{
						//
						// Set the current program for the thread to work with
						ThreadProgram = m_CurrentProgram;
						//
						// Set the number of programs to compute
						if ((m_CurrentProgram + BATCHCOUNT) <= m_Population.Count)
						{
							ThreadProgramsLeft = BATCHCOUNT;
						}
						else
						{
							ThreadProgramsLeft = m_Population.Count - m_CurrentProgram;
						}
						//
						// Update the program reference for the next thread to continue with
						m_CurrentProgram += ThreadProgramsLeft;
					}
					int ThreadProgramsCount = ThreadProgramsLeft;

					//
					// Evaluate the set of programs
					// Maintain a set of local batch results, to reduce the amount
					// of time spent in the lock that updates the global Population stats.
					double LocalMax = FitnessMaximum;
					double LocalTotal = 0;
					int LocalBestFitnessID = ThreadProgram;
					int LocalWorstFitnessID = ThreadProgram;

					ComputeThreadBatch(BATCHCOUNT, Fitness, Predictions, ref ThreadProgram, ref ThreadProgramsLeft, ref LocalMax, ref LocalTotal, ref LocalBestFitnessID, ref LocalWorstFitnessID);

					//
					// Update the Max/Ave fitness.
					lock (m_LockFitnessStat)
					{
						FitnessMaximum = Math.Max(FitnessMaximum, LocalMax);
						FitnessAverage += LocalTotal;

						//
						// Update the best & worst program of the generation
						if (m_FitnessMeasure[LocalBestFitnessID] < m_FitnessMeasure[m_BestProgramGeneration])
						{
							m_BestProgramGeneration = LocalBestFitnessID;
						}
						if (m_FitnessMeasure[LocalWorstFitnessID] > m_FitnessMeasure[m_WorstProgramGeneration])
						{
							m_WorstProgramGeneration = LocalWorstFitnessID;
						}
					}

					lock (m_LockCompletedPrograms)
					{
						m_CompletedPrograms += ThreadProgramsCount;

						//
						// If this thread just processed the last program, signal the event that all
						// programs are now done processing
						//if (ThreadProgram == m_Population.Count)
						if (m_CompletedPrograms == m_Population.Count)
						{
							//
							// Ensure that all threads synchronize into their waiting state first
							wh_Suspend.Reset();
							//
							// Now, indicate the last program is done.
							wh_LastProgramDone.Set();
						}
					}
				}
			}
			catch
			{	
				//
				// Purpose of this try-catch block is that it seems like some extra threads get left hanging around
				// between modeling sessions and think they should help out next time, sometimes resulting in an
				// exception becaue the objects they reference no longer exist.  This little exception handling
				// takes care of that little problem for us.
				// Console.WriteLine("Current Program: " + m_CurrentProgram.ToString());
			}
		}

		/// <summary>
		/// Evaluates the fitness of a block (batch) of programs.
		/// </summary>
		private void ComputeThreadBatch(int BATCHCOUNT, double[] Fitness, double[] Predictions, ref int ThreadProgram, ref int ThreadProgramsLeft, ref double LocalMax, ref double LocalTotal, ref int LocalBestFitnessID, ref int LocalWorstFitnessID)
		{
			while (ThreadProgramsLeft > 0)
			{
				//
				// Compute the fitness of each program
				double ProgramFitness = ComputeProgramFitness(ThreadProgram, Predictions);
				Fitness[BATCHCOUNT - ThreadProgramsLeft] = ProgramFitness;
				m_FitnessMeasure[ThreadProgram] = ProgramFitness;

				//
				// Update the batch local stats
				LocalMax = Math.Max(LocalMax, ProgramFitness);
				LocalTotal += ProgramFitness;
				if (ProgramFitness < m_FitnessMeasure[LocalBestFitnessID])
				{
					LocalBestFitnessID = ThreadProgram;
				}
				if (ProgramFitness > m_FitnessMeasure[LocalWorstFitnessID])
				{
					LocalWorstFitnessID = ThreadProgram;
				}

				ThreadProgram++;
				ThreadProgramsLeft--;
			}
		}

		/// <summary>
		/// Evaluates the program versus each set of training inputs and
		/// computes the custom fitness of the program.
		/// </summary>
		/// <param name="ProgramID">Index of the program in the Population</param>
		/// <param name="Predictions">Set of predicted values by the program</param>
		/// <returns>Custom fitness of the program</returns>
		private double ComputeProgramFitness(int ProgramID, double[] Predictions)
		{
			GPProgram Program = m_Population.Programs[ProgramID];
			//
			// Start by converting the program into a tree representation
			Program.ConvertToTree(m_Config.FunctionSet, true);
			//
			// Set the memory size
			Program.CountMemory = m_Config.Profile.CountMemory;

			//
			// Test each item in the training data set
			m_FitnessHits[ProgramID] = 0;
			for (int FitnessTest = 0; FitnessTest < m_TrainingData.Rows && !m_Abort; FitnessTest++)
			{
				//
				// Assign the input values to the program
				Program.UserTerminals = m_TrainingData.InputRow(FitnessTest);

				//
				// Assign a reference to the historical set of data for this fitness test
				if (m_UseInputHistory)
				{
					Program.InputHistory = m_UserInputHistory[FitnessTest];
				}

				//
				// This is the money shot, execute the genetic program!
				double Result = Program.EvaluateAsDouble();

				//
				// Deal with problems that might have come up during the fitness computation
				if (double.IsInfinity(Result) ||
					double.IsNaN(Result) ||
					double.IsNegativeInfinity(Result) ||
					double.IsPositiveInfinity(Result))
				{
					Result = m_MaximumError;
				}
				//
				// If the program size is above our threshold size, give it the
				// worst possible result to prevent it from being used in any of the
				// genetic operations.  There is a problem if the number of nodes in
				// the tree gets about the size of a 'short', we loose the ability to
				// count and label them.  Question...why not use an 'int'?  Two reasons...
				//	1.  A program bigger than 16bits of nodes is ridiculous in the first place
				//	2.  Memory, each node would require 32bits for a lable, instead of 16bits, 
				//		it doubles that bit of storage, which we don't need.
				if (Program.CountNodes >= GPEnums.PROGRAMSIZE_THRESHOLD)
				{
					Result = m_MaximumError;
				}
				//
				// Store the Program result
				Predictions[FitnessTest] = Result;

				//
				// Determine if we have a "hit" against the input data
				double Error = Math.Abs(Result - m_TrainingData.ObjectiveRow(FitnessTest)[0]);
				if (Error <= m_Tolerance && Error >= -m_Tolerance)
				{
					m_FitnessHits[ProgramID]++;
				}
			}

			//
			// Restore it back to an array. 
			Program.ConvertToArray(m_Config.FunctionSet);

			//
			// Make a call into the custom fitness object to evaluate the
			// fitness of the program.
			double ProgramFitness = m_Config.Fitness.ComputeFitness(
				m_UserInputHistoryCustomFitness,
				Predictions,
				m_TrainingData.ObjectiveColumn(0),
				m_TrainingAverage,
				m_Tolerance);

			//
			// Still have to check the result for problems
			if (double.IsNaN(ProgramFitness) || double.IsInfinity(ProgramFitness) || double.IsPositiveInfinity(ProgramFitness) || double.IsNegativeInfinity(ProgramFitness))
			{
				ProgramFitness = m_FitnessMeasure[m_WorstProgramGeneration];
			}

			return ProgramFitness;
		}

		/// <summary>
		/// Immediately terminate any current modeling
		/// </summary>
		public bool Abort
		{
			set
			{
				wh_LastProgramDone.Set();
				m_Abort = true;
			}
		}
		private bool m_Abort;
	}
}
