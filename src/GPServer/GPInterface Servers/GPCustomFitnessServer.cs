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
	/// This class provides an implementation of the IGPCustomFitness interface which
	/// allows clients to compute the fitness of a set of results against a custom
	/// fitness function.
	/// </summary>
	public class GPCustomFitnessServer : MarshalByRefObject, IGPCustomFitness
	{
		/// <summary>
		/// Default constructor: Get an IGPCompiler created, because we need it to
		/// write and compile our custom fitness function.
		/// </summary>
		public GPCustomFitnessServer()
		{
			//
			// Get an interface to the compiler we'll eventually need
			m_Compiler = new GPCompilerServer();
		}

		/// <summary>
		/// Need a compiler interface so we can compile fitness functions as they come in
		/// </summary>
		GPCompilerServer m_Compiler;

		#region IGPCustomFitness Implementation

		/// <summary>
		/// Receives the fitness function from the client and then gets it compiled
		/// and prepared for use in computing the fitness of any results passed in.
		/// </summary>
		public string FitnessFunction
		{
			set
			{
				//
				// Get the class written
				String FitnessClass = m_Compiler.WriteFitnessClass(value);
				//
				// Get it compiled and save the object reference
				String[] Errors=null;
				m_FitnessCompiled = m_Compiler.CompileFitnessClass(FitnessClass, out Errors);
			}
		}
		/// <summary>
		/// Reference to the compiled fitness function
		/// </summary>
		private GPFitnessCustomBase m_FitnessCompiled;

		/// <summary>
		/// Receives the training data set from the user, this is where the "Input History" and
		/// ObjectiveAverage values are computed, which are used later in the .Compute method.
		/// </summary>
		public GPTrainingData Training
		{
			set
			{
				m_Training = value;
				//
				// Pre-compute the training data stats
				ComputeTrainingStats(m_Training,ref m_TrainingAverage,ref m_TrainingMin, ref m_TrainingMax);
				//
				// Construct the Input History data set
				PrepareInputHistory(m_Training,ref m_InputHistory);
			}
		}
		private GPTrainingData m_Training;
		private double m_TrainingAverage;
		private double m_TrainingMin;
		private double m_TrainingMax;

		/// <summary>
		/// Assigns the predictions made by the candidate program, these are given to the fitness
		/// computation function for comparison.
		/// </summary>
		public double[] Predictions
		{
			set
			{
				m_Predictions = value;
			}
		}
		private double[] m_Predictions;

		/// <summary>
		/// Computes the fitness of the program, based upon the current set of predictions given to it.
		/// </summary>
		/// <returns></returns>
		public double Compute()
		{
			return m_FitnessCompiled.ComputeFitness(m_InputHistory, m_Predictions, m_Training.ObjectiveColumn(0), m_TrainingAverage, GPEnums.RESULTS_TOLERANCE);
		}

		#endregion

		#region Support Methods

		/// <summary>
		/// Compute the average, min and max value of the training data
		/// </summary>
		/// <param name="Training">Training data</param>
		private void ComputeTrainingStats(GPTrainingData Training, ref double Average, ref double Min, ref double Max)
		{
			Max = Training.ObjectiveRow(0)[0];
			Min = Training.ObjectiveRow(0)[0];

			double Total = 0.0;
			for (int Value = 0; Value < Training.Rows; Value++)
			{
				Total += Training.ObjectiveRow(Value)[0];
				Max = Math.Max(Max, Training.ObjectiveRow(Value)[0]);
				Min = Math.Min(Min, Training.ObjectiveRow(Value)[0]);
			}

			Average = Total / Training.Rows;
		}

		/// <summary>
		/// Construct the input history for the custom fitness functions.  For
		/// regression models, pass the entire input data set it, for time series
		/// models, have to reconstruct the original input series.
		/// </summary>
		/// <param name="Training"></param>
		private void PrepareInputHistory(GPTrainingData Training, ref List<List<double>> InputHistory)
		{
			if (Training.TimeSeriesSource)
			{
				//
				// Have to create a single column of data, an exact copy of the
				// time series data itself.  This turns out to be the number of rows
				// PLUS the remaining data in the last row.
				InputHistory = new List<List<double>>(Training.Rows + Training.Columns - 1);
				for (int Row = 0; Row < Training.Rows; Row++)
				{
					InputHistory.Add(new List<double>(1));
					InputHistory[Row].Add(Training[Row, 0]);
				}

				//
				// Now, grab the remaing data items in the last row
				for (int Column = 1; Column < Training.Columns; Column++)
				{
					InputHistory.Add(new List<double>(1));
					InputHistory[Training.Rows + Column - 1].Add(Training[Training.Rows - 1, Column]);
				}
			}
			else
			{
				//
				// Create a data set of just the inputs, leaving out the prediction column
				InputHistory = new List<List<double>>(Training.Rows);
				for (int Row = 0; Row < Training.Rows; Row++)
				{
					InputHistory.Add(new List<double>(Training.Columns));
					for (int Column = 0; Column < Training.Columns; Column++)
					{
						InputHistory[Row].Add(Training.InputData[Row][Column]);
					}
				}
			}
		}
		private List<List<double>> m_InputHistory;

		#endregion
	}
}
