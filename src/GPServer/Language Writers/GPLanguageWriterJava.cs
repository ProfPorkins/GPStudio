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

namespace GPStudio.Server
{
	/// <summary>
	/// Creates a Java representation of the program tree
	/// </summary>
	public class GPLanguageWriterJava : GPLanguageWriterCSJBase
	{
		/// <summary>
		/// Constructor that just passes arguments down to the base class
		/// </summary>
		/// <param name="Program">Program to be turned into Java</param>
		/// <param name="FunctionSet">User Defined Functions</param>
		public GPLanguageWriterJava(GPProgram Program, bool TimeSeries, SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
			: base(Program,TimeSeries,FunctionSet)
		{
		}

		/// <summary>
		/// This method creates the initial class header and declaration
		/// that is required before any methods can be written.
		/// </summary>
		/// <param name="Program">Source program</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteHeader(GPProgram Program)
		{
			m_Writer.WriteLine("import java.util.*;");
			m_Writer.WriteLine();

			return base.WriteHeader(Program);
		}

		protected override string ListType { get { return "ArrayList"; } }
		protected override string ListAdd { get { return "add"; } }
		protected override string DoubleType { get { return "Double"; } }
		protected override string ListCount { get { return "size()"; } }

		protected override void WriteTimeSeriesInit(int InputDimension)
		{
			m_Writer.WriteLine("\t\t//");
			m_Writer.WriteLine("\t\t// First time in, prepare the time series history");
			m_Writer.WriteLine("\t\t// All other times update with the newest value");
			m_Writer.WriteLine("\t\tif (InputHistory." + ListCount + " == 0)");
			m_Writer.WriteLine("\t\t{");
			for (int Parameter = 0; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\t\t\tInputHistory.add(new ArrayList<Double>(1));");
				m_Writer.WriteLine("\t\t\tInputHistory.get(" + Parameter + ").add(t" + Parameter + ");");
			}

			m_Writer.WriteLine("\t\t}");

			//
			// All subsequent "Compute" calls add the newest value
			m_Writer.WriteLine("\t\telse");
			m_Writer.WriteLine("\t\t{");

			m_Writer.WriteLine("\t\t\tArrayList<Double> TestRow = new ArrayList<Double>(1);");
			m_Writer.WriteLine("\t\t\tTestRow.add(t" + (InputDimension - 1) + ");");
			m_Writer.WriteLine("\t\t\tInputHistory.add(TestRow);");

			m_Writer.WriteLine("\t\t}");
		}

		protected override void WriteSetMem()
		{
			base.WriteSetMem("Math.abs");
		}

		protected override void WriteGetMem()
		{
			base.WriteGetMem("Math.abs");
		}
	}
}
