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

namespace GPStudio.Shared
{
	/// <summary>
	/// This class contains all the parameters used to control a modeling
	/// session on the server.
	/// </summary>
	[Serializable]
	public class GPModelingProfile
	{

		#region Program Code Reduction Techniques

		/// <summary>
		/// Usage of SPEA2 multi-objective to reduce code bloat
		/// </summary>
		public bool SPEA2MultiObjective
		{
			set { m_SPEA2MultiObjective = value; }
			get { return m_SPEA2MultiObjective; }
		}
		private bool m_SPEA2MultiObjective = false;

		/// <summary>
		/// Usage of Adaptive Parsimony Pressure to reduce code bloat
		/// </summary>
		public bool AdaptiveParsimony 
		{ 
			set { m_AdaptiveParsimony = value; }
			get { return m_AdaptiveParsimony; }
		}
		private bool m_AdaptiveParsimony = false;


		#endregion

		#region Population Parameters

		/// <summary>
		/// Number of programs in the modeling population
		/// </summary>
		public int PopulationSize 
		{ 
			set { m_PopulationSize = value; }
			get { return m_PopulationSize; }
		}
		private int m_PopulationSize = 500;

		/// <summary>
		/// Population build initialization technique
		/// </summary>
		public GPEnums.PopulationInit PopulationBuild 
		{ 
			set { m_PopulationBuild = value; }
			get { return m_PopulationBuild; }
		}
		private GPEnums.PopulationInit m_PopulationBuild = GPEnums.PopulationInit.Ramped;

		/// <summary>
		/// Maximum depth to create trees when generating a new program for the population
		/// </summary>
		public int PopulationInitialDepth 
		{ 
			set { m_PopulationInitialDepth = value; }
			get { return m_PopulationInitialDepth; }
		}
		private int m_PopulationInitialDepth = 6;

		#endregion

		#region Distributed Parameters

		/// <summary>
		/// Number of programs to transmit between servers, according to the
		/// topological organization.
		/// </summary>
		public int DistributedTransferCount
		{
			set { m_DistributedTransferCount = value; }
			get { return m_DistributedTransferCount; }
		}
		private int m_DistributedTransferCount = 5;

		/// <summary>
		/// Topological organization for distributed servers
		/// </summary>
		public GPEnums.DistributedTopology DistributedTopology
		{
			set { m_DistributedTopology = value; }
			get { return m_DistributedTopology; }
		}
		private GPEnums.DistributedTopology m_DistributedTopology = GPEnums.DistributedTopology.Star;

		/// <summary>
		/// Percent of servers to distribute programs to from each server
		/// in the star organization.
		/// </summary>
		public int DistributedTopologyStarPercent
		{
			set
			{
				if (value < 0) m_DistributedTopologyStarPercent = 0;
				else if (value > 100) m_DistributedTopologyStarPercent = 100;
				else m_DistributedTopologyStarPercent = value;
			}
			get { return m_DistributedTopologyStarPercent; }
		}
		private int m_DistributedTopologyStarPercent = 75;

		#endregion

		#region Operator Probabilities

		private int m_ProbabilityReproduction=5;
		public int ProbabilityReproduction 
		{ 
			set { m_ProbabilityReproduction = value; }
			get { return m_ProbabilityReproduction; }
		}
		public double ProbabilityReproductionD
		{
			get { return m_ProbabilityReproduction/100.0; }
		}

		private int m_ProbabilityMutation=10;
		public int ProbabilityMutation 
		{ 
			set { m_ProbabilityMutation = value; }
			get { return m_ProbabilityMutation; }
		}
		public double ProbabilityMutationD
		{
			get { return m_ProbabilityMutation / 100.0; }
		}

		private int m_ProbabilityCrossover=85;
		public int ProbabilityCrossover 
		{ 
			set { m_ProbabilityCrossover = value; }
			get { return m_ProbabilityCrossover; }
		}
		public double ProbabilityCrossoverD
		{
			get { return m_ProbabilityCrossover / 100.0; }
		}

		private int m_ProbabilityTerminal = 50;
		public int ProbabilityTerminal
		{
			get { return m_ProbabilityTerminal; }
			set { m_ProbabilityTerminal = value; }
		}
		public double ProbabilityTerminalD
		{
			get { return m_ProbabilityTerminal / 100.0; }
		}

		private int m_ProbabilityFunction = 50;
		public int ProbabilityFunction
		{
			get { return m_ProbabilityFunction; }
			set { m_ProbabilityFunction = value; }
		}
		public double ProbabilityFunctionD
		{
			get { return m_ProbabilityFunction / 100.0; }
		}

	#endregion

		private GPEnums.Reproduction m_Reproduction = GPEnums.Reproduction.OverSelection;
		public GPEnums.Reproduction Reproduction
		{
			get { return m_Reproduction; }
			set { m_Reproduction = value; }
		}

		#region Memory

		private bool m_UseMemory = false;
		public bool UseMemory
		{
			get { return m_UseMemory; }
			set { m_UseMemory = value; }
		}

		private short m_CountMemory = 20;
		public short CountMemory
		{
			get { return m_CountMemory; }
			set { m_CountMemory = value; }
		}

		#endregion

		#region Terminals

		private bool m_UseRndInteger = false;
		public bool UseRndInteger
		{
			get { return m_UseRndInteger; }
			set { m_UseRndInteger = value; }
		}

		private int m_IntegerMax = 100;
		public int IntegerMax
		{
			get { return m_IntegerMax; }
			set { m_IntegerMax = value; }
		}

		private int m_IntegerMin = 0;
		public int IntegerMin
		{
			get { return m_IntegerMin; }
			set { m_IntegerMin = value; }
		}

		private bool m_UseRndDouble = true;
		public bool UseRndDouble
		{
			get { return m_UseRndDouble; }
			set { m_UseRndDouble = value; }
		}

		#endregion

	}
}
