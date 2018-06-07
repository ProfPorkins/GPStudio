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
	[Serializable]
	public class GPTrainingData
	{
		/// <summary>
		/// Default constructor, creates an empty data set
		/// </summary>
		public GPTrainingData()
		{
			m_TimeSeries = false;
			m_TimeSeriesSource = false;
		}

		/// <summary>
		/// Creates the data storage arrays
		/// </summary>
		/// <param name="Rows"></param>
		/// <param name="Columns"></param>
		/// <param name="Objectives"></param>
		public void ConstructStorage(int Rows, int Columns, int Objectives)
		{
			//
			// We can do both arrays at the same time because they have the
			// same number of rows.
			m_Input = new double[Rows][];
			m_Objective = new double[Rows][];
			for (int Row = 0; Row < Rows; Row++)
			{
				m_Input[Row] = new double[Columns];
				m_Objective[Row] = new double[Objectives];
			}
		}

		/// <summary>
		/// Indexer which allows access to the input data
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public double this[int Row, int Column]
		{
			get { return m_Input[Row][Column]; }
			set { m_Input[Row][Column] = value; }
		}

		/// <summary>
		/// Array of the objective values
		/// </summary>
		protected double[][] m_Objective;

		/// <summary>
		/// Is this a time series data set.  i.e. The Input and Objectives are the same
		/// </summary>
		public bool TimeSeries
		{
			get { return m_TimeSeries; }
			set { m_TimeSeries=value; }
		}
		private bool m_TimeSeries;

		/// <summary>
		/// Was the original data a time series data set?  When data is transmitted
		/// from the client to the server, it is converted into a "Standard" looking
		/// data set, but the Input History for custom fitness functions needs to
		/// know this little nuggest of information so it can create that data correctly.
		/// </summary>
		public bool TimeSeriesSource
		{
			set { m_TimeSeriesSource = value; }
			get { return m_TimeSeriesSource; }
		}
		private bool m_TimeSeriesSource;

		/// <summary>
		/// Number of input and objective rows
		/// </summary>
		public int Rows
		{
			get
			{
				if (m_Input != null)
					return m_Input.Length;
				else
					return 0;
			}
		}

		/// <summary>
		/// Number of input parameters in each row
		/// </summary>
		public int Columns
		{
			get
			{
				if (m_Input != null)
					return m_Input[0].Length;
				else
					return 0;
			}
		}

		/// <summary>
		/// Number of objectives
		/// </summary>
		public int Objectives
		{
			get
			{
				if (m_Objective != null)
					return m_Objective[0].Length;
				else
					return 0;
			}
		}

		/// <summary>
		/// Returns a reference to a single row of training inputs
		/// </summary>
		/// <param name="Row"></param>
		/// <returns></returns>
		public double[] InputRow(int Row)
		{
			return m_Input[Row];
		}

		/// <summary>
		/// Returns a reference to the full set of input data
		/// </summary>
		public double[][] InputData
		{
			get
			{
				return m_Input;
			}
		}

		/// <summary>
		/// Array of the training inputs
		/// </summary>
		protected double[][] m_Input;

		/// <summary>
		/// Returns a reference to a single row of objectives
		/// </summary>
		/// <param name="Row"></param>
		/// <returns></returns>
		public double[] ObjectiveRow(int Row)
		{
			return m_Objective[Row];
		}

		/// <summary>
		/// Return a column of training inputs
		/// </summary>
		/// <param name="Column">Which column to return</param>
		/// <returns>Array of values for the column</returns>
		public double[] InputColumn(int Column)
		{
			//
			// Use exception handling to validate the Column
			try
			{
				double[] data = new double[this.Rows];
				for (int Row = 0; Row < this.Rows; Row++)
				{
					data[Row] = m_Input[Row][Column];
				}

				return data;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Return a column of the training objectives
		/// </summary>
		/// <param name="Column">Which column to return</param>
		/// <returns>Array of values for the column</returns>
		public double[] ObjectiveColumn(int Column)
		{
			//
			// Use exception handling to validate the Column
			try
			{
				double[] data = new double[this.Rows];
				for (int Row = 0; Row < this.Rows; Row++)
				{
					data[Row] = m_Objective[Row][Column];
				}

				return data;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Public property that allows access to the historical data sets.  First
		/// time the property is accessed the data is created, thereafter, it is reused.
		/// </summary>
		public double[][][] HistoricalDataSets
		{
			get
			{
				if (m_HistoricalDataSets == null)
				{
					if (this.TimeSeries)
					{
						m_HistoricalDataSets = ConstructHistoricalDataTimeSeries();
					}
					else
					{
						m_HistoricalDataSets = ConstructHistoricalDataStandard();
					}

				}

				return m_HistoricalDataSets;
			}
		}

		/// <summary>
		/// Reference to a series of data sets that represent a historical time
		/// series of data as it is presented to a program, set by step.  Each
		/// "set" of data represents the complete history of data up to that
		/// point in the time series of data.  One set is created for each row
		/// of input data.
		/// </summary>
		private double[][][] m_HistoricalDataSets = null;

		/// <summary>
		/// Create the historical data sets for time series modeling types.
		/// </summary>
		/// <returns>Three dimensional array containing all the possible historical data sets</returns>
		private double[][][] ConstructHistoricalDataTimeSeries()
		{
			double[][][] Historical = new double[this.Rows][][];
			//
			// Create a unique test set reference
			for (int TestSet = 0; TestSet < this.Rows; TestSet++)
			{
				Historical[TestSet] = new double[TestSet+1][];
			}

			//
			// Now, we go through the rest of the records, creating new 1 element
			// arrays that get progressively added to each of the data sets.
			for (int FitnessTest = 0; FitnessTest < this.Rows; FitnessTest++)
			{
				double[] TestRow = new double[1];
				TestRow[0] = this[FitnessTest, 0];

				//
				// Do the progressive add
				for (int TestSet = FitnessTest+1; TestSet < this.Rows; TestSet++)
				{
					Historical[TestSet][FitnessTest] = TestRow;
				}
			}

			return Historical;
		}

		/// <summary>
		/// Create the historical data sets for regression modeling types.  Each set has a history
		/// of the previous inputs AND the current set of inputs.
		/// </summary>
		/// <returns>Three dimensional array containing all the possible historical data sets</returns>
		private double[][][] ConstructHistoricalDataStandard()
		{
			double[][][] Historical = new double[this.Rows][][];
			//
			// Create a unique test set reference
			for (int TestSet = 0; TestSet < this.Rows; TestSet++)
			{
				Historical[TestSet] = new double[TestSet+1][];
			}

			//
			// Go through each row of fitness tests and and reference back into the
			// original data for all sets.  This creates both a memory and
			// computational optimization.
			for (int FitnessTest = 0; FitnessTest < this.Rows; FitnessTest++)
			{
				//
				// Assign this row to each of the test sets as appropriate.  In other words,
				// all sets (after the first) get the first row, then beginning with the second set, they all
				// get the second row, the third set onward, they all get the third row and
				// so on and so forth.
				for (int TestSet = FitnessTest; TestSet < this.Rows; TestSet++)
				{
					Historical[TestSet][FitnessTest] = m_Input[FitnessTest];
				}
			}
			return Historical;
		}
	}
}
