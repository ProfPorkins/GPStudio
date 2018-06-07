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

namespace GPStudio.Client
{
	/// <summary>
	/// This class is used to hold the results data created during a
	/// modeling session.  It holds the results for each server and has
	/// the ability to aggregate these data into combined stats.
	/// </summary>
	public class ModelingResults
	{
		/// <summary>
		/// Constructor that creates the results collection
		/// </summary>
		public ModelingResults()
		{
			m_Results = new List<List<ServerData>>();
			m_BestProgram = new List<BestProgram>();
		}

		/// <summary>
		/// Represents each of the stats recorded from each server for a
		/// single generation.
		/// </summary>
		public class ServerData
		{
			public double BestFitness;
			public int BestHits;
			public int BestComplexity;

			public int ComplexityMinimum;
			public int ComplexityAverage;
			public int ComplexityMaximum;

			public double FitnessMinimum;
			public double FitnessAverage;
			public double FitnessMaximum;
		}

		/// <summary>
		/// Structure used to track the best program in the collection
		/// </summary>
		private class BestProgram
		{
			public int Server=0;
			public int Generation=0;
		}

		/// <summary>
		/// Data member use to hold the server data.  The first dimension
		/// is the server, the second is the results by generation.
		/// </summary>
		private List<List<ServerData>> m_Results;

		/// <summary>
		/// Data member used to track the best program in the collection
		/// </summary>
		//private BestProgram m_BestProgram;
		private List<BestProgram> m_BestProgram;

		/// <summary>
		/// Allows a new server to be added
		/// </summary>
		public void AddServer()
		{
			m_Results.Add(new List<ServerData>());
		}

		/// <summary>
		/// Public method which allows data to be recorded
		/// </summary>
		/// <param name="WhichServer"></param>
		/// <param name="Stats"></param>
		public void ReportServerData(int Generation,int WhichServer,ServerData Stats)
		{
			m_Results[WhichServer].Add(Stats);

			//
			// Track the best program
			if (Generation >= m_BestProgram.Count)
			{
				BestProgram obj = new BestProgram();
				m_BestProgram.Add(obj);
				//
				// Beginning with the second generation, set this equal to the
				// previous best program.
				if (Generation >= 1)
				{
					obj.Server = m_BestProgram[m_BestProgram.Count - 1].Server;
					obj.Generation = m_BestProgram[m_BestProgram.Count - 1].Generation;
				}
				else
				{
					obj.Server = WhichServer;
					obj.Generation = Generation;
				}
			}

			if (Stats.BestFitness < 
				this[m_BestProgram[m_BestProgram.Count-1].Server,
					 m_BestProgram[m_BestProgram.Count-1].Generation].BestFitness)
			{
				m_BestProgram[m_BestProgram.Count - 1].Server = WhichServer;
				m_BestProgram[m_BestProgram.Count - 1].Generation = Generation;
			}
		}

		/// <summary>
		/// Indexer which allows access to a single server's results
		/// </summary>
		/// <param name="Server"></param>
		/// <returns></returns>
		public List<ServerData> this[int Server]
		{
			get
			{
				return m_Results[Server];
			}
		}

		/// <summary>
		/// Indexer which allows access to a specific server and generation's results
		/// </summary>
		/// <param name="Server"></param>
		/// <param name="Generation"></param>
		/// <returns></returns>
		public ServerData this[int Server, int Generation]
		{
			get
			{
				return m_Results[Server][Generation];
			}
		}

		/// <summary>
		/// Returns the server index containing the best program at the indicated generation
		/// </summary>
		/// <param name="Generation"></param>
		/// <returns></returns>
		public int BestServer(int Generation)
		{
			return m_BestProgram[Generation].Server;
		}

		/// <summary>
		/// Returns the number of generations of data stored
		/// </summary>
		public int Generations
		{
			get { return m_BestProgram.Count; }
		}

		/// <summary>
		/// Returns the maximum fitness value from the entire population
		/// </summary>
		/// <param name="Generation"></param>
		/// <returns></returns>
		public double FitnessMaximum(int Generation)
		{
			double Max = m_Results[0][Generation].FitnessMaximum;
			for (int Server = 0; Server < m_Results.Count; Server++)
			{
				if (Max < m_Results[Server][Generation].FitnessMaximum)
				{
					Max = m_Results[Server][Generation].FitnessMaximum;
				}
			}

			return Max;
		}

		/// <summary>
		/// Returns the average fitness from the entire population
		/// </summary>
		/// <param name="Generation"></param>
		/// <returns></returns>
		public double FitnessAverage(int Generation)
		{
			double Total = 0.0;
			for (int Server = 0; Server < m_Results.Count; Server++)
			{
				Total += m_Results[Server][Generation].FitnessAverage;
			}

			return Total / m_Results.Count;
		}
	}
}
