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
	public class GPFitnessSPEA2:GPFitnessObjectiveBase
	{
		/// <summary>
		/// How much to divide the Population by for determining the archive size.
		/// TODO: This should be parameterized
		/// </summary>
		private const int ARCHIVESCALE = 8;	

		public GPFitnessSPEA2(int PopulationSize)
			: base(PopulationSize / ARCHIVESCALE)
		{
			InitializeStorage(PopulationSize);
		}

		/// <summary>
		/// Prepares the storage for the fitness computation
		/// </summary>
		/// <param name="PopulationSize"></param>
		protected void InitializeStorage(int PopulationSize)
		{
			m_Archive = new List<SPEA2ProgramStats>();
		}

		/// <summary>
		/// Use the SPEA2 algorithm to compute fitness, with RawFitness and Program
		/// Complexity the two objectives to minimize.
		/// </summary>
		/// <param name="RawFitness"></param>
		/// <param name="Population"></param>
		public override void PrepareFitness(double[] RawFitness, GPPopulation Population)
		{
			m_RawFitness = RawFitness;

			//
			// --- Run The SPEA2 Algorithm ---
			m_Archive = UpdateArchive(Population);

			//
			// Create a new RawFitness based upon the Archive SPEA2 fitness values
			double[] SPEA2Fitness = new double[m_Archive.Count];
			for (int Program = 0; Program < m_Archive.Count; Program++)
			{
				SPEA2Fitness[Program]=m_Archive[Program].SPEA2Fitness;
			}

			//
			// In case the archive size changes, let the base class know so it can resize
			// its storage to match the new size.
			base.PopulationSize = m_Archive.Count;

			//
			// Use the base class to get the final program selection stats computed.
			base.PrepareFitness(SPEA2Fitness, Population);
		}

		/// <summary>
		/// Returns the indicated program for a genetic operation, we return from the archive
		/// and not from the general Population.
		/// </summary>
		/// <param name="Program"></param>
		/// <returns></returns>
		public override GPProgram Programs(int Program)
		{
			return m_Archive[Program].Program;
		}

		/// <summary>
		/// Assign the Population complexity values for use in the MOO domination calculation
		/// </summary>
		/// <param name="Population"></param>
		private int[] PrepareComplexity(GPPopulation Population)
		{
			int[] Complexity = new int[Population.Count];
			for (int Program = 0; Program < Population.Count; Program++)
			{
				Complexity[Program] = Population.Programs[Program].CountNodes;
			}

			return Complexity;
		}

		#region SPEA2 Archive Construction

		/// <summary>
		/// Copys all nondominated programs from ArchivePopulation and PopCurrent into
		/// the next archive...which is returned from the method
		/// </summary>
		/// <param name="ArchivePopulation"></param>
		/// <param name="Population"></param>
		private List<SPEA2ProgramStats> UpdateArchive(GPPopulation Population)
		{
			SPEA2ProgramStats[] SPEA2Population = new SPEA2ProgramStats[Population.Count];

			//
			// Grab the complexity of each program.  We do this here, rather than getting it
			// from the Population object because it saves some computation time due to the
			// complexity values being reused several times.
			int[] Complexity = PrepareComplexity(Population);

			//
			// Init the min/max objective values - arbitrarily choose the first program's complexity value
			m_MinObjectiveFitness = m_MaxObjectiveFitness = m_RawFitness[0];
			m_MinObjectiveComplexity = m_MaxObjectiveComplexity = Complexity[0];

			//
			// Compute how many solutions each program in the current Population is dominated by
			for (int ProgramA = 0; ProgramA < Population.Count; ProgramA++)
			{
				SPEA2Population[ProgramA] = new SPEA2ProgramStats();
				SPEA2Population[ProgramA].Program = Population.Programs[ProgramA];
				SPEA2Population[ProgramA].RawFitness = m_RawFitness[ProgramA];
				SPEA2Population[ProgramA].Complexity = Complexity[ProgramA];
				SPEA2Population[ProgramA].Strength = 0;

				//
				// Test ProgramA against all programs in the current Population
				for (int ProgramB = 0; ProgramB < Population.Count; ProgramB++)
				{
					if ((m_RawFitness[ProgramB] > SPEA2Population[ProgramA].RawFitness) &&
						(Complexity[ProgramB] > SPEA2Population[ProgramA].Complexity))
					{
						SPEA2Population[ProgramA].Strength++;
					}
				}

				//
				// Test ProgramA against all programs in the current archive
				for (int ProgramB = 0; ProgramB < Archive.Count; ProgramB++)
				{
					if (Archive[ProgramB].RawFitness > SPEA2Population[ProgramA].RawFitness &&
						Archive[ProgramB].Complexity > SPEA2Population[ProgramA].Complexity)
					{
						SPEA2Population[ProgramA].Strength++;
					}
				}

				//
				// Maintain the min/max objective values from the current Population
				m_MinObjectiveFitness = Math.Min(SPEA2Population[ProgramA].RawFitness, m_MinObjectiveFitness);
				m_MinObjectiveComplexity = Math.Min(SPEA2Population[ProgramA].Complexity, m_MinObjectiveComplexity);

				m_MaxObjectiveFitness = Math.Max(SPEA2Population[ProgramA].RawFitness, m_MaxObjectiveFitness);
				m_MaxObjectiveComplexity = Math.Max(SPEA2Population[ProgramA].Complexity, m_MaxObjectiveComplexity);
			}

			//
			// Count how many solutions each program in the archive is dominated by
			for (int ProgramA = 0; ProgramA < Archive.Count; ProgramA++)
			{
				SPEA2ProgramStats ProgramAStats = Archive[ProgramA];
				ProgramAStats.Strength = 0;
				//
				// Test ProgramA against all programs in the current Population
				for (int ProgramB = 0; ProgramB < Population.Count; ProgramB++)
				{
					if (m_RawFitness[ProgramB] > ProgramAStats.RawFitness &&
						Complexity[ProgramB] > ProgramAStats.Complexity)
					{
						ProgramAStats.Strength++;
					}
				}

				//
				// Test ProgramA against all programs in the current archive
				for (int ProgramB = 0; ProgramB < Archive.Count; ProgramB++)
				{
					if (Archive[ProgramB].RawFitness > ProgramAStats.RawFitness &&
						Archive[ProgramB].Complexity > ProgramAStats.Complexity)
					{
						ProgramAStats.Strength++;
					}
				}
				Archive[ProgramA] = ProgramAStats;

				//
				// Maintain the min/max objective values from the current archive
				m_MinObjectiveFitness = Math.Min(ProgramAStats.RawFitness, m_MinObjectiveFitness);
				m_MinObjectiveComplexity = Math.Min(ProgramAStats.Complexity, m_MinObjectiveComplexity);

				m_MaxObjectiveFitness = Math.Max(ProgramAStats.RawFitness, m_MaxObjectiveFitness);
				m_MaxObjectiveComplexity = Math.Max(ProgramAStats.Complexity, m_MaxObjectiveComplexity);
			}

			if (m_MinObjectiveComplexity == m_MaxObjectiveComplexity)
			{
				m_MaxObjectiveComplexity += 1.0;
			}

			//
			// Now, we can compute the SPEA2 fitness for each individual by summing
			// the strenth values for each program it is dominated by.
			for (int ProgramA = 0; ProgramA < Population.Count; ProgramA++)
			{
				SPEA2Population[ProgramA].SPEA2Fitness = 0.0;
				//
				// Test ProgramA against all programs in the current Population
				for (int ProgramB = 0; ProgramB < Population.Count; ProgramB++)
				{
					if (m_RawFitness[ProgramB] < SPEA2Population[ProgramA].RawFitness &&
						Complexity[ProgramB] < SPEA2Population[ProgramA].Complexity)
					{
						SPEA2Population[ProgramA].SPEA2Fitness += SPEA2Population[ProgramB].Strength;
					}
				}

				//
				// Test ProgramA against all programs in the current archive
				for (int ProgramB = 0; ProgramB < Archive.Count; ProgramB++)
				{
					if (Archive[ProgramB].RawFitness < SPEA2Population[ProgramA].RawFitness &&
						Archive[ProgramB].Complexity < SPEA2Population[ProgramA].Complexity)
					{
						SPEA2Population[ProgramA].SPEA2Fitness += Archive[ProgramB].Strength;
					}
				}
			}

			//
			// Now, do it for the current archive
			for (int ProgramA = 0; ProgramA < Archive.Count; ProgramA++)
			{
				SPEA2ProgramStats ProgramAStats = Archive[ProgramA];
				ProgramAStats.SPEA2Fitness = 0.0;
				//
				// Test ProgramA against all programs in the current Population
				for (int ProgramB = 0; ProgramB < Population.Count; ProgramB++)
				{
					if (SPEA2Population[ProgramB].RawFitness < ProgramAStats.RawFitness &&
						SPEA2Population[ProgramB].Complexity < ProgramAStats.Complexity)
					{
						ProgramAStats.SPEA2Fitness += SPEA2Population[ProgramB].Strength;
					}
				}

				//
				// Test ProgramA against all programs in the current archive
				for (int ProgramB = 0; ProgramB < Archive.Count; ProgramB++)
				{
					if (Archive[ProgramB].RawFitness < ProgramAStats.RawFitness &&
						Archive[ProgramB].Complexity < ProgramAStats.Complexity)
					{
						ProgramAStats.SPEA2Fitness += Archive[ProgramB].Strength;
					}
				}

				Archive[ProgramA] = ProgramAStats;
			}

			//
			// Compute the Density estimate for each program in the current Population
			for (int ProgramA = 0; ProgramA < Population.Count; ProgramA++)
			{
				SPEA2Population[ProgramA].Density = ComputeDensity(ref SPEA2Population[ProgramA], SPEA2Population, Archive);
			}

			//
			// Compute the Density estimate for each program in the current archive
			for (int ProgramA = 0; ProgramA < Archive.Count; ProgramA++)
			{
				SPEA2ProgramStats ProgramAStats = Archive[ProgramA];
				ProgramAStats.Density = ComputeDensity(ref ProgramAStats, SPEA2Population, Archive);
				Archive[ProgramA] = ProgramAStats;
			}

			//
			// Final SPEA2 fitness calculation: F(i)=R(i)+D(i)
			for (int ProgramA = 0; ProgramA < Population.Count; ProgramA++)
			{
				SPEA2Population[ProgramA].SPEA2Fitness = SPEA2Population[ProgramA].SPEA2Fitness + SPEA2Population[ProgramA].Density;
			}

			for (int ProgramA = 0; ProgramA < Archive.Count; ProgramA++)
			{
				SPEA2ProgramStats ProgramAStats = Archive[ProgramA];
				ProgramAStats.SPEA2Fitness = ProgramAStats.SPEA2Fitness + ProgramAStats.Density;
				Archive[ProgramA] = ProgramAStats;				
			}

			//
			// Add all nondominated programs from the current Population into the new archive
			// The max complexity program allowed in the archive is 10,000.  There are two reasons for this...
			//   *The List container has an "short" sized counter, so programs bigger than 16 bits can't be stored
			//    in the compressed array representation.  Setting a value of 10,000 keeps programs from
			//    getting too out of hand.
			//   *A program size of 10,000 is ridiculous in size, so don't allow it.
			List<SPEA2ProgramStats> New_Archive = new List<SPEA2ProgramStats>();
			SortedDictionary<int, int> New_ArchivePrograms = new SortedDictionary<int, int>();
			for (int Program = 0; Program < Population.Programs.Count; Program++)
			{
				if (SPEA2Population[Program].SPEA2Fitness < 1.0 && SPEA2Population[Program].Complexity > 2
					&& SPEA2Population[Program].Complexity < 10000)
				{
					New_Archive.Add(SPEA2Population[Program]);
					New_ArchivePrograms.Add(Program, Program);
				}
			}

			//
			// Add all nondominated programs for the current archive into the new archive
			for (int Program = 0; Program < Archive.Count; Program++)
			{
				if (Archive[Program].SPEA2Fitness < 1.0 && Archive[Program].Complexity > 2)
				{
					New_Archive.Add(Archive[Program]);
				}
			}

#if GPLOG
			GPLog.ReportLine("Archive Size: " + New_Archive.Count,true);
#endif

			//
			// Get the archive to the right size
			int ArchiveSize = Population.Count / ARCHIVESCALE;
			if (New_Archive.Count < ArchiveSize)
			{
				ExpandArchive(New_Archive, ArchiveSize, SPEA2Population, New_ArchivePrograms);
			}
			else if (New_Archive.Count > ArchiveSize)
			{
				ShrinkArchive(New_Archive, ArchiveSize);
			}

			return New_Archive;
		}

		/// <summary>
		/// Increase the archive to the indicated size by selecting the next best programs from the
		/// Population not already in the archive.
		/// </summary>
		/// <param name="New_Archive"></param>
		/// <param name="ArchiveSize"></param>
		/// <param name="SPEA2Population"></param>
		private void ExpandArchive(List<SPEA2ProgramStats> New_Archive, int ArchiveSize, SPEA2ProgramStats[] SPEA2Population, SortedDictionary<int, int> New_ArchivePrograms)
		{
			while (New_Archive.Count < ArchiveSize)
			{
				//
				// Make a random selection
				int Program = GPUtilities.rngNextInt(SPEA2Population.Length);
				if (!New_ArchivePrograms.ContainsKey(Program))
				{
					New_Archive.Add(SPEA2Population[Program]);
					New_ArchivePrograms.Add(Program, Program);
				}
			}
		}

		/// <summary>
		/// Shrink the archive to the indicated size by selecting the program with the with the smallest
		/// distance until the desired archive size is achieved.
		/// </summary>
		/// <param name="New_Archive"></param>
		/// <param name="ArchiveSize"></param>
		private void ShrinkArchive(List<SPEA2ProgramStats> New_Archive, int ArchiveSize)
		{
			//
			// Place the archive in distance sorted order
			New_Archive.Sort(CompareSPEA2Distance);
			while (New_Archive.Count > ArchiveSize)
			{
				New_Archive.RemoveAt(0);
			}
		}

		/// <summary>
		/// Helper function used to sort the archive programs by density
		/// </summary>
		/// <param name="ProgramA"></param>
		/// <param name="ProgramB"></param>
		/// <returns></returns>
		public static int CompareSPEA2Distance(SPEA2ProgramStats ProgramA, SPEA2ProgramStats ProgramB)
		{
			if (ProgramA.Distance < ProgramB.Distance) return -1;
			if (ProgramA.Distance > ProgramB.Distance) return 1;

			return 0;
		}

		/// <summary>
		/// Computes the SPEA2 density estimate for the specified program in the main Population
		/// </summary>
		/// <param name="SPEA2Population"></param>
		/// <param name="Archive"></param>
		/// <returns>Density estimate</returns>
		private double ComputeDensity(ref SPEA2ProgramStats ProgramStats, SPEA2ProgramStats[] SPEA2Population, List<SPEA2ProgramStats> Archive)
		{
			List<double> Distance = new List<double>();

			//
			// Compute the from program values once.
			// Normalize the objective values before computing the distance
			double RawFitness = (ProgramStats.RawFitness - m_MinObjectiveFitness) / (m_MaxObjectiveFitness - m_MinObjectiveFitness);
			double Complexity = (ProgramStats.Complexity - m_MinObjectiveComplexity) / (m_MaxObjectiveComplexity - m_MinObjectiveComplexity);

			//
			// Compute the distance to every other program in the Population (in objective space)
			for (int ToProgram = 0; ToProgram < SPEA2Population.Length; ToProgram++)
			{
				//
				// Normalize the objective values before computing the distance
				double RawFitnessTo = (SPEA2Population[ToProgram].RawFitness - m_MinObjectiveFitness) / (m_MaxObjectiveFitness - m_MinObjectiveFitness);
				double SqFitness = Math.Pow(RawFitness - RawFitnessTo, 2.0);

				double ComplexityTo = (SPEA2Population[ToProgram].Complexity - m_MinObjectiveComplexity) / (m_MaxObjectiveComplexity - m_MinObjectiveComplexity);
				double SqComplexity = Math.Pow((Complexity - ComplexityTo), 2.0);

				Distance.Add(Math.Sqrt(SqFitness+SqComplexity));
			}

			//
			// Now do it for the archive
			for (int ToProgram = 0; ToProgram < Archive.Count; ToProgram++)
			{
				//
				// Normalize the objective values before computing the distance
				double RawFitnessTo = (Archive[ToProgram].RawFitness - m_MinObjectiveFitness) / (m_MaxObjectiveFitness - m_MinObjectiveFitness);
				double SqFitness = Math.Pow(RawFitness - RawFitnessTo, 2.0);

				double ComplexityTo = (Archive[ToProgram].Complexity - m_MinObjectiveComplexity) / (m_MaxObjectiveComplexity - m_MinObjectiveComplexity);
				double SqComplexity = Math.Pow((Complexity - ComplexityTo), 2.0);

				Distance.Add(Math.Sqrt(SqFitness + SqComplexity));				
			}

			//
			// Sort these distances
			Distance.Sort();

			//
			// Save the minimum distance, which is the 2nd element, because the 1st will be the distance
			// to itself, which is 0.
			ProgramStats.Distance = Distance[1];

			//
			// Take the sqrt(N+N(Archive)) item as the k estimate
			int Item = (int)Math.Sqrt(SPEA2Population.Length+Archive.Count);
			double Density=1.0/(Distance[Item]+2.0);

			return Density;
		}

		//
		// These are used to normalize the fitness objectives so they are on equal footing
		// when it comes to the distance calculation.
		private double m_MinObjectiveFitness;
		private double m_MaxObjectiveFitness;
		private double m_MinObjectiveComplexity;
		private double m_MaxObjectiveComplexity;


		#endregion

		/// <summary>
		/// Structure used to hold the SPEA2 algorithm computations
		/// </summary>
		public struct SPEA2ProgramStats
		{
			public GPProgram Program;
			public int Strength;		// Number of programs that dominate this program
			public double Distance;		// SPEA2 distance
			public double Density;		// SPEA2 density estimate
			public double SPEA2Fitness;	// Summation of the strengths of all programs that dominate this program

			public double RawFitness;	// Raw fitness from the Fitness object
			public int Complexity;		// Number of nodes for this program
		}

		/// <summary>
		/// Current SPEA2 archive, all mating operations are performed against this Population of programs
		/// </summary>
		public List<SPEA2ProgramStats> Archive
		{
			get { return m_Archive; }
		}
		private List<SPEA2ProgramStats> m_Archive;
	}
}
