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
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This class implements the IGPProgram interface, which allows remote
	/// clients to interact with a single GPProgram, through that interface.
	/// </summary>
	public class GPProgramServer: MarshalByRefObject,IGPProgram
	{
		/// <summary>
		/// Prepare the lease sponsorship settings
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;	// Set an infinite lifetime
		}

		/// <summary>
		/// Private GPProgram data member
		/// </summary>
		public GPProgram Program
		{
			get { return m_Program; }
		}
		private GPProgram m_Program;

		/// <summary>
		/// Function set object, must contain the exact same functions as when
		/// the program was modeled.
		/// </summary>
		public IGPFunctionSet FunctionSet
		{
			set { m_FunctionSet = value; }
		}
		private IGPFunctionSet m_FunctionSet;
		
		/// <summary>
		/// Data set to use for computing results against.
		/// </summary>
		public GPTrainingData Training
		{
			set { m_Training = value; }
		}
		private GPTrainingData m_Training;

		/// <summary>
		/// Current set of user input values for evaluating the program
		/// </summary>
		public double[] UserInputs
		{
			set 
			{
				m_UserInputs = value;
				m_Program.UserTerminals = m_UserInputs; 
			}
		}
		double[] m_UserInputs;

		/// <summary>
		/// The program must receive the input history as a generic list, so the
		/// array representation must be converted into a generic list...that is what
		/// is being done here.
		/// Why does the program need a generic list?  Because the source code versions
		/// of the programs do not know in advance how long the history will be, so
		/// a growable container is required...if everything was self-contained and no
		/// source representation was needed, the program could use a regular array.
		/// </summary>
		public double[][] UserInputHistory
		{
			set 
			{
				m_UserInputHistory = value;
				//
				// Have to convert the Array into a Generic array for
				// server side use.  Remember, generics are not serializable,
				// that is why this is being done.
				m_Program.InputHistory = new List<List<double>>(m_UserInputHistory.Length);
				foreach (double[] row in m_UserInputHistory)
				{
					if (row != null)	// The last row might be a null
					{
						List<double> GRow = new List<double>(row.Length);
						foreach (double item in row)
						{
							GRow.Add(item);
						}
						m_Program.InputHistory.Add(GRow);
					}
				}
			}
		}
		double[][] m_UserInputHistory;

		/// <summary>
		/// Construct a GPProgram object from an XML program description
		/// </summary>
		/// <param name="ProgramXML">Program XML string</param>
		/// <returns>True/False depending upon success or failure</returns>
		public bool ProgramFromXML(String ProgramXML)
		{
			GPProgramReaderXML xmlReader = new GPProgramReaderXML(ProgramXML, m_FunctionSet);
			m_Program = xmlReader.Construct();

			if (m_Program == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Run the program!
		/// TODO: Not too good here, should throw an exception if m_Program is null
		/// </summary>
		/// <returns></returns>
		private double Compute()
		{
			if (m_Program == null)
			{
				return 0.0;
			}

			return m_Program.EvaluateAsDouble();
		}

		/// <summary>
		/// Computes the whole series of fitness cases over the input history.  This
		/// provides the "chunky" interface needed to keep the number of transports
		/// over the network down to one, for all computations...saves a TON of time!
		/// </summary>
		/// <returns></returns>
		public double[] ComputeBatch()
		{
			//
			// Have the GPTrainingData object create the input history for
			// batch computation.
			m_UserInputHistory = m_Training.InputData;
			double[] Results = new double[m_UserInputHistory.Length];

			for (int Row = 0; Row < Results.Length; Row++)
			{
				this.UserInputs=m_UserInputHistory[Row];
				Results[Row] = this.Compute();
			}

			return Results;
		}

		/// <summary>
		/// Performs a history of computations for a time series.  This method expects
		/// the Training data set to be assigned, so it can build the input history array.
		/// </summary>
		/// <returns></returns>
		public double[] ComputeBatchTS(int InputDimension,int PredictionDistance)
		{
			double[] Results=new double[m_Training.Rows];
			double[] UserInputs = new double[InputDimension];

			for (int Row = InputDimension; Row < (m_Training.Rows - PredictionDistance) + 1; Row++)
			{
				//
				// Set the correct input history
				this.UserInputHistory = m_Training.HistoricalDataSets[Row];
				//
				// Prepare the input terminals
				for (int Value = 0; Value < InputDimension; Value++)
				{
					int InputLocation = Row - InputDimension + Value;
					UserInputs[Value] = m_Training[InputLocation, 0];
				}
				this.UserInputs = UserInputs;

				Results[Row + PredictionDistance - 1] = this.Compute();
			}

			//
			// Let's be easy on Time Series and say that the first InputDimension plus
			// the prediction distance values are the same as the training...this keeps 
			// the error from being messed up.
			for (int Row = 0; Row < InputDimension + PredictionDistance - 1; Row++)
			{
				Results[Row] = m_Training[Row, 0];
			}

			return Results;
		}
	}
}
