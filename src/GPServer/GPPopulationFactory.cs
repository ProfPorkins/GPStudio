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
using System.Collections.Generic;
using GPStudio.Shared;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;

namespace GPStudio.Server
{
	class GPPopulationFactory
	{
		/// <summary>
		/// Standard constructor, accepts the GP configuration along with how
		/// many user input parameters are specified.
		/// </summary>
		/// <param name="ModelerConfig"></param>
		/// <param name="InputDimension"></param>
		public GPPopulationFactory(GPModelerServer ModelerConfig, int InputDimension)
		{
			m_ModelerConfig = ModelerConfig;
			m_InputDimension = InputDimension;
		}

		GPModelerServer m_ModelerConfig;
		private int m_InputDimension;
		GPPopulation m_PopCurrent;
		GPProgramTreeFactory m_TreeFactory;
		GPFitness m_Fitness;

		//
		// Define a delegate type for the program selection
		private delegate int DELProgramSelection();
		private DELProgramSelection ExecProgramSelection;

		/// <summary>
		/// Creates the next generation of programs.  This method also keeps tabs on memory
		/// as it creates the new Population.  If we run out of physical memory then the
		/// new Population size is changed to wherever we ran out of memory.
		/// </summary>
		/// <param name="nGeneration">Which generation is currently being processed</param>
		/// <param name="Population">Reference to the Population just computed for fitness</param>
		/// <param name="Fitness">Fitness object with the fitness computation results</param>
		/// <param name="AutoReproduce">List of programs to automatically reproduce into the next generation</param>
		/// <returns>Reference to the new Population to use</returns>
		public GPPopulation ComputeNext(int nGeneration, GPPopulation Population, GPFitness Fitness, List<GPProgram> AutoReproduce)
		{
			m_PopCurrent = Population;
			m_Fitness = Fitness;

			//
			// Setup the program selection delegate
			switch (m_ModelerConfig.Profile.Reproduction)
			{
				case GPEnums.Reproduction.Tournament:
					ExecProgramSelection = new DELProgramSelection(Fitness.FitnessSelection.SelectProgramTournament);
					break;
				case GPEnums.Reproduction.OverSelection:
					ExecProgramSelection = new DELProgramSelection(Fitness.FitnessSelection.SelectProgramOverSelection);
					break;
			}

			//
			// Create a Tree Factory object - It helps in support of the mutation and
			// crossover operations
			m_TreeFactory = new GPProgramTreeFactory(m_ModelerConfig, m_InputDimension);

			return ComputeNext(Population, Fitness, AutoReproduce);
		}

		public GPPopulation ComputeNext(GPPopulation PopCurrent, GPFitness Fitness, List<GPProgram> AutoReproduce)
		{
			GPPopulation PopNew = new GPPopulation(m_ModelerConfig);

			//
			// Add the new programs into the next generation automatically
			foreach (GPProgram Seed in AutoReproduce)
			{
				PopNew.Programs.Add(Seed);
			}
			//
			// Automatically reproduce the best program into the next generation
			PopNew.Programs.Add((GPProgram)Fitness.BestProgramRef.Clone());

			//
			// Now, go ahead and fill out the rest of the Population
			while (PopNew.Count < m_ModelerConfig.Profile.PopulationSize)
			{
				//
				// Make a probabilistic selection between...
				//	Reproduction
				//	Mutation
				//	Crossover
				double Choice = GPUtilities.rngNextDouble();
				double Cumulative = m_ModelerConfig.Profile.ProbabilityReproductionD;
				bool bSelected = false;

				if (Choice <= Cumulative)
				{
					Reproduce(PopNew,Fitness.FitnessSelection);
					bSelected = true;

				}
				Cumulative += m_ModelerConfig.Profile.ProbabilityMutationD;
				if (!bSelected && (Choice <= Cumulative))
				{
					Mutate(PopNew, Fitness.FitnessSelection);
					bSelected = true;
				}
				Cumulative += m_ModelerConfig.Profile.ProbabilityCrossoverD;
				if (!bSelected && (Choice <= Cumulative))
				{
					Crossover(PopNew, Fitness.FitnessSelection);
					bSelected = true;
				}

				//
				// Only check every 100 times to save a little time on doing this
				// check.
				if (PopNew.Count % 100 == 0)
				{
					if (!CheckMemory())
					{
						//
						// Do a garbage collect just to be sure and check again
						System.GC.Collect();
						if (!CheckMemory())
							break;
					}
				}
			}

			return PopNew;
		}

		/// <summary>
		/// Checks the system memory to see if the run should continue.  There
		/// is enough memory to continue if there is still 1MB left.  The idea
		/// is that the next 100 new programs will take up less than 1MB of memory.
		///
		///	False: If there isn't enough memory to continue
		///	True: If there is enough memory to continue
		/// </summary>
		/// <returns></returns>
		private bool CheckMemory()
		{
			//
			// Work through all the processes and get their memory
			long UsedMemory = 0;
			foreach (Process aProcess in Process.GetProcesses())
			{
				UsedMemory += aProcess.PagedMemorySize64;
			}

			//
			// Grab the physical memory on the system
			ComputerInfo info = new ComputerInfo();
			long PhysicalMemoryTotal = (long)info.TotalPhysicalMemory;

			//
			// Hey, who put that magic number in there! :)
			if ((PhysicalMemoryTotal - UsedMemory) > 1048576)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Select a program based upon fitness to reproduce into the next generation.
		/// </summary>
		/// <param name="PopNew">Population to add the reproduced program into</param>
		private void Reproduce(GPPopulation PopNew,GPFitnessObjectiveBase FitnessSelection)
		{
			GPProgram Copy = (GPProgram)FitnessSelection.Programs(ExecProgramSelection()).Clone();
			//
			// Add this copied child into the new Population
			PopNew.Programs.Add(Copy);
		}

		/// <summary>
		/// Select a program, based on fitness, for mutation.  Place
		// the mutated program into the new Population.
		/// </summary>
		/// <param name="PopNew">Population to add the newly created program to</param>
		private void Mutate(GPPopulation PopNew,GPFitnessObjectiveBase FitnessSelection)
		{
			GPProgram Copy = (GPProgram)FitnessSelection.Programs(ExecProgramSelection()).Clone();

			//
			// Convert it to a tree
			Copy.ConvertToTree(m_ModelerConfig.FunctionSet, false);

			//
			// Perform the mutation
			m_TreeFactory.Attach(Copy);
			m_TreeFactory.Mutate();

			//
			// Return it back to an array
			Copy.ConvertToArray(m_ModelerConfig.FunctionSet);

			//
			// Add this mutated individual to the new Population
			PopNew.Programs.Add(Copy);
		}

		/// <summary>
		/// Select two parents, based upon fitness, for a crossover operation.
		/// Place the two children into the new Population.
		/// </summary>
		/// <param name="PopNew">Population to add the newly created program to</param>
		private void Crossover(GPPopulation PopNew, GPFitnessObjectiveBase FitnessSelection)
		{
			GPProgram Child1 = (GPProgram)FitnessSelection.Programs(ExecProgramSelection()).Clone();
			GPProgram Child2 = (GPProgram)FitnessSelection.Programs(ExecProgramSelection()).Clone();

			//
			// Convert to trees
			Child1.ConvertToTree(m_ModelerConfig.FunctionSet, false);
			Child2.ConvertToTree(m_ModelerConfig.FunctionSet, false);

			//
			// Perform the crossover
			m_TreeFactory.Attach(Child1);
			Child2 = m_TreeFactory.Crossover(Child2);

			//
			// Convert to arrays
			Child1.ConvertToArray(m_ModelerConfig.FunctionSet);
			Child2.ConvertToArray(m_ModelerConfig.FunctionSet);

			//
			// Add them to the new Population
			PopNew.Programs.Add(Child1);
			if (PopNew.Count < m_PopCurrent.Count)
			{
				PopNew.Programs.Add(Child2);
			}
		}

	}
}
