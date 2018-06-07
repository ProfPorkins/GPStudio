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
using System.Text;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This class provides the base operations for any program fitness selection
	/// class.  A derived class will provide its own measure of fitness, single or 
	/// multi-objective and any variation within.  The result from the derived class
	/// is a single value for each program known as the RawFitness to this class.
	/// </summary>
	public class GPFitnessObjectiveBase
	{
		public GPFitnessObjectiveBase(int PopulationSize)
		{
			this.PopulationSize = PopulationSize;
		}

		/// <summary>
		/// How many individuals in the Population to work with
		/// </summary>
		protected int PopulationSize
		{
			set 
			{
				//
				// Only re-allocate if the size changes.
				if (m_PopulationSize != value)
				{
					m_PopulationSize = value;
					//
					// Allocate room for the fitness computations
					m_FitnessAdjusted = new double[PopulationSize];
					m_FitnessNormalized = new double[PopulationSize];
					m_CumulativeNormalized = new double[PopulationSize];
				}
			}
			get { return m_PopulationSize; }
		}
		private int m_PopulationSize;

		/// <summary>
		/// Reference to the raw fitness value as used by this class to compute
		/// the fitness selection stats.
		/// </summary>
		protected virtual double[] RawFitness
		{
			set
			{
				m_RawFitness = value;
				//
				// Now, get the program selection stats computed, so the different
				// program selection members can be used for the genetic operations.

				//
				// Normalized fitness
				ComputeNormalizedFitness();

				//
				// Create a ranked (sorted by fitness) array that indexes back
				// into the stored results.  This is needed for all of the program
				// reproduction/selection techniques.
				ConstructSortedIndex();

				//
				// Prepare the Greedy OverSelection stats
				ComputeOverSelectionPercent();
			}
		}
		protected double[] m_RawFitness;

		#region Fitness Selection Data Members

		private double[] m_FitnessAdjusted;
		private double[] m_FitnessNormalized;
		private double[] m_CumulativeNormalized;
		private double m_PopPercent;

		/// <summary>
		/// Collection that stores the programs, ranked by their fitness, from
		/// largest (worst) to best (smallest).  The double value is the raw fitness 
		/// and the int value is the index program index;
		/// </summary>
		public List<KeyValuePair<double, int>> FitnessRanked
		{
			get { return m_FitnessRanked; }
			set { m_FitnessRanked = value; }
		}
		List<KeyValuePair<double, int>> m_FitnessRanked;

		#endregion

		#region Public Program Selection Methods

		public virtual void PrepareFitness(double[] RawFitness,GPPopulation Population)
		{
			m_Population = Population;
			this.RawFitness = RawFitness;
		}
		private GPPopulation m_Population;

		/// <summary>
		/// Returns the indicated program for a genetic operation.
		/// </summary>
		/// <param name="Program"></param>
		/// <returns></returns>
		public virtual GPProgram Programs(int Program)
		{
			return m_Population.Programs[Program];
		}

		/// <summary>
		/// Tournament selection, with a tournament size of 2
		/// </summary>
		/// <returns>Index into the current Population of the winning program</returns>
		public int SelectProgramTournament()
		{
			int Program1 = GPUtilities.rngNextInt(m_PopulationSize);
			int Program2 = GPUtilities.rngNextInt(m_PopulationSize);

			//
			// Choose the one with the best fitness
			double Fitness1 = m_FitnessNormalized[Program1];
			double Fitness2 = m_FitnessNormalized[Program2];

			//
			// Choose the best and return the the program index
			if (Fitness1 > Fitness2)
			{
				return Program1;
			}

			return Program2;
		}

		/// <summary>
		/// Select a program according to Koza's Greedy Over-Selection
		/// technique:  Genetic Programming I Section 6.3.5
		///
		/// 80% of the time select from the most fit
		/// 20% select from the least fit
		/// </summary>
		/// <returns>Index into the current Population of the winning program</returns>
		public int SelectProgramOverSelection()
		{
			int Program1;
			int Program2;
			//
			// Decide which group to choose from
			double GroupRnd = GPUtilities.rngNextDouble();
			if (GroupRnd <= 0.80)
			{
				//
				// Use roulette wheel selection to select two programs from this group
				Program1 = SelectProgramRoulette(0.0, m_PopPercent);
				Program2 = SelectProgramRoulette(0.0, m_PopPercent);
			}
			else
			{
				//
				// Choose two programs from this group
				Program1 = SelectProgramRoulette(m_PopPercent, 1.0 - m_PopPercent);
				Program2 = SelectProgramRoulette(m_PopPercent, 1.0 - m_PopPercent);
			}
			//
			// Choose the one with the best fitness
			double Fitness1 = m_FitnessNormalized[Program1];
			double Fitness2 = m_FitnessNormalized[Program2];

			//
			// Choose the best and return the the program index
			if (Fitness1 > Fitness2)
			{
				return Program1;
			}

			return Program2;
		}

		#endregion

		#region Program Selection Support Methods

		/// <summary>
		/// Use roulette wheel selection to choose a program from the group.  The search
		/// for the program position is done against a cumulative index so that a binary
		/// search can be done, instead of a linear search (yea!).
		/// </summary>
		/// <param name="StartPercent">Where to begin the search</param>
		/// <param name="CumulativePercent">The percentage value to search for</param>
		/// <returns>Index of the program chosen</returns>
		private int SelectProgramRoulette(double StartPercent, double CumulativePercent)
		{
			//
			// Make a random number selection, no bigger than the CumulativePercent
			double Select = StartPercent + GPUtilities.rngNextDouble() * CumulativePercent;
			if (Select > 1.0)
			{
				Select = 1.0;
				Console.WriteLine("Big Problem!");	// TODO: Should use exception handling
			}
			//
			// Program number we get back is the program in the ranked array,
			// so it needs to be used to get the "real" program in the program array
			int Program = FindCumulative(0, PopulationSize, Select);

			//
			// The .Value property of the ranked fitness is the original program
			// location in the list of programs.
			return FitnessRanked[Program].Value;
		}

		/// <summary>
		/// Binary search to find the cumulative probability location in the ranked fitness array
		/// </summary>
		/// <param name="StartPos"></param>
		/// <param name="EndPos"></param>
		/// <param name="Select">Cumulative probability being searched for</param>
		/// <returns>Program index of the program that meets the cumulative prob criteria</returns>
		private int FindCumulative(int StartPos, int EndPos, double Select)
		{
			//
			// Compute the middle position
			int Middle = StartPos + (EndPos - StartPos) / 2;
			double ValMiddle = m_CumulativeNormalized[Middle];
			if (Middle == 0 || (StartPos == EndPos))
				return Middle;

			if (Select <= ValMiddle && m_CumulativeNormalized[Middle - 1] < Select)
			{
				return Middle;	// We found it
			}
			else if (ValMiddle > Select)
			{
				return FindCumulative(StartPos, Middle, Select);
			}
			return FindCumulative(Middle, EndPos, Select);
		}

		#endregion

		#region Raw Fitness to Program Selection Stats Methods

		/// <summary>
		/// This method computes the Normalized Fitness, this requires that the 
		/// Adjusted Fitness is first computed, then the Normalized Fitness can 
		/// be computed.
		/// </summary>
		private void ComputeNormalizedFitness()
		{
			//
			// Start with the Adjusted fitness
			double fAdjustedTotal = 0.0;
			for (int nItem = 0; nItem < PopulationSize; nItem++)
			{
				m_FitnessAdjusted[nItem] = 1.0 / (1.0 + m_RawFitness[nItem]);
				fAdjustedTotal += m_FitnessAdjusted[nItem];
			}

			//
			// Now, compute the normalized fitness
			for (int nItem = 0; nItem < PopulationSize; nItem++)
			{
				m_FitnessNormalized[nItem] = m_FitnessAdjusted[nItem] / fAdjustedTotal;
			}
		}

		/// <summary>
		/// Creates a sorted index back into the original results.  The sorting 
		/// is based upon the RawFitness values.
		/// </summary>
		private void ConstructSortedIndex()
		{
			//
			// Use the generic collections to help build the sorted array
			m_FitnessRanked = new List<KeyValuePair<double, int>>(PopulationSize);
			//
			// Add in the Raw Fitness values.
			int Program = 0;
			foreach (double Fitness in m_FitnessNormalized)
			{
				m_FitnessRanked.Add(new KeyValuePair<double, int>(Fitness, Program++));
			}

			//
			// Sort based upon the Key
			m_FitnessRanked.Sort(CompareFitnessKey);

			//
			// Now, given the sorted array, build the cumulative normalized array
			double PrevCumulative = 0.0;
			for (int item = 0; item < PopulationSize; item++)
			{
				m_CumulativeNormalized[item] = PrevCumulative + m_FitnessRanked[item].Key;
				PrevCumulative = m_CumulativeNormalized[item];
			}

			//
			// Make the last one to be larger than 1 to handle roundoff error
			// in building the cumulative values.
			m_CumulativeNormalized[PopulationSize - 1] = 1.01;
		}

		/// <summary>
		/// Builds up the stats needed by the rank and overselection techniques
		/// </summary>
		private void ComputeOverSelectionPercent()
		{
			//
			// Determine the % of the most fit individuals, based upon the Population size
			// 1000 = 32%, 2000 = 16% and so on, to a minimum of a 1% selection
			if (PopulationSize <= 1000)
				m_PopPercent = 0.32;
			else if (PopulationSize <= 2000)
				m_PopPercent = 0.16;
			else if (PopulationSize <= 4000)
				m_PopPercent = 0.08;
			else if (PopulationSize <= 8000)
				m_PopPercent = 0.04;
			else if (PopulationSize <= 16000)
				m_PopPercent = 0.02;
			else
				m_PopPercent = 0.01;
		}

		/// <summary>
		/// This is used to compare two program fitness values - for the previous method.
		/// We want largest to smallest, so the comparison is backwards of what you might expect.
		/// </summary>
		/// <param name="a">Left hand side</param>
		/// <param name="b">Right hand side</param>
		/// <returns>1 if a less than b, -1 if a greater than b, 0 if equal</returns>
		private int CompareFitnessKey(KeyValuePair<double, int> a, KeyValuePair<double, int> b)
		{
			if (a.Key < b.Key) return 1;
			if (a.Key > b.Key) return -1;

			return 0;
		}

		#endregion
	}
}
