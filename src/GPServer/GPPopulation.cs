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
using System.Collections;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This class represents a Population of Genetic Programs.  The Population
	/// is responsible for computing its own fitness, creating the new
	/// generation and whatever else I feel like putting in here.
	/// </summary>
    public class GPPopulation : IEnumerable
    {
		/// <summary>
		/// Basic constructor, accepts the configuration and prepares the
		/// container for the programs.
		/// </summary>
		/// <param name="Config"></param>
		public GPPopulation(GPModelerServer Config)
		{
			m_Config = Config;
			m_Programs = new List<GPProgram>();
		}

        private GPModelerServer m_Config;
		private List<GPProgram> m_Programs;

		/// <summary>
		/// Provides an enumerator over the programs
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			foreach (GPProgram Program in m_Programs)
			{
				yield return Program;
			}
		}

		/// <summary>
		/// Number of programs in the Population
		/// </summary>
		public int Count
		{
			get { return m_Programs.Count; }
		}

		/// <summary>
		/// Allow read access to the list of programs
		/// </summary>
		public List<GPProgram> Programs
		{
			get { return m_Programs; }
		}

		/// <summary>
		/// Constructs an initial Population for the given parameters.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="InputDimension"></param>
		/// <returns></returns>
        public bool Build(GPEnums.PopulationInit BuildMethod, short InputDimension)
        {
            GPGeneratePopulation buildPopulation = null;

            if (BuildMethod == GPEnums.PopulationInit.Full)
            {
				buildPopulation = new GPGeneratePopulationFull(m_Config,InputDimension);
            }
            else
            if (BuildMethod == GPEnums.PopulationInit.Grow)
            {
				buildPopulation = new GPGeneratePopulationGrow(m_Config, InputDimension);
            }
            else
            if (BuildMethod == GPEnums.PopulationInit.Ramped)
            {
				buildPopulation = new GPGeneratePopulationRamped(m_Config, InputDimension);
            }

			m_Programs = buildPopulation.Generate(m_Config.Profile.PopulationSize);

            return true;
        }

		/// <summary>
		/// Computes the min/max/ave complexity of the Population
		/// </summary>
		/// <param name="Minimum"></param>
		/// <param name="Maximum"></param>
		/// <param name="Average"></param>
		public void ComputeComplexity(out int Minimum, out int Maximum, out int Average)
		{
			Minimum =Maximum= m_Programs[0].CountNodes;

			long TotalComplexity = 0;
			foreach (GPProgram Program in m_Programs)
			{
				TotalComplexity += Program.CountNodes;
				Minimum = Math.Min(Minimum, Program.CountNodes);
				Maximum = Math.Max(Maximum, Program.CountNodes);
			}

			Average =(int)(TotalComplexity / (long)m_Programs.Count);
		}
	}
}
