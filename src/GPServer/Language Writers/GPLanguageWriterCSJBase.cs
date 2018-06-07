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
using System.IO;

namespace GPStudio.Server
{
	/// <summary>
	/// A base class used by both C# and Java writers.  After originally writing those
	/// two classes I realized how much code they really do have in common.  So, this
	/// class was refactored out to remove/reduce the duplicated code.
	/// </summary>
	public abstract class GPLanguageWriterCSJBase : GPLanguageWriter
	{
		public GPLanguageWriterCSJBase(GPProgram Program, bool TimeSeries,SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
			: base(Program,TimeSeries)
		{
			m_FunctionSet = FunctionSet;
		}
		protected SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> m_FunctionSet;

		/// <summary>
		/// Create a stream writer on top of hte stream object passed in
		/// </summary>
		/// <param name="InputStream">Source stream</param>
		public override void Initialize(Stream InputStream)
		{
			m_Writer = new StreamWriter(InputStream);
		}
		protected StreamWriter m_Writer;

		/// <summary>
		/// Close the stream writer
		/// </summary>
		public override void Terminate()
		{
			m_Writer.Flush();
		}

		/// <summary>
		/// C# and Java handle some of the generic types differently, but fundamentally,
		/// the code is the same.  So, in order to write the code once, but allow
		/// for the unique parts to be customized elegantly.
		/// </summary>
		/// <returns>String representation of the genetic item</returns>
		protected abstract string ListType { get; }
		protected abstract string ListAdd { get; }
		protected abstract string DoubleType { get; }
		protected abstract string ListCount { get; }

		/// <summary>
		/// A time series program needs to be initialized with the first set
		/// of historical values.
		/// </summary>
		/// <param name="InputDimension">Number of historical parameters</param>
		protected abstract void WriteTimeSeriesInit(int InputDimension);


		/// <summary>
		/// C#/Java represent the program as a class that can be incorporated into
		/// another program.  This method writes the class header for the "Program".
		/// </summary>
		/// <param name="Program">The program object we are translating</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteHeader(GPProgram Program)
		{
			m_Writer.WriteLine("public class GeneticProgram");
			m_Writer.WriteLine("{");
			m_Writer.WriteLine();

			//
			// Write the indexed memory
			m_Writer.WriteLine("\tpublic double[] m_Memory = null;");
			//
			// Write the input history
			m_Writer.WriteLine("\tprivate " + ListType + "<" + ListType + "<" + DoubleType + ">> InputHistory;");
			m_Writer.WriteLine();

			//
			// Write the constructor - have it initialize the indexed memory and Input History
			m_Writer.WriteLine("\tpublic GeneticProgram()");
			m_Writer.WriteLine("\t{");
			m_Writer.WriteLine("\t\tm_Memory=new double[" + m_Program.CountMemory + "];");
			m_Writer.WriteLine("\t\tInputHistory = new " + ListType + "<" + ListType + "<" + DoubleType + ">>();");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();

			return true;
		}

		/// <summary>
		/// Writes the specified RPB to the stream
		/// </summary>
		/// <param name="rpb">RPB tree to translate</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteRPB(GPProgramBranchRPB rpb)
		{
			//
			// Write the function declaration
			m_Writer.Write("\tpublic double Compute(");
			//
			// Write the input parameters
			WriteProgramParameters();
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("\t{");

			//
			// Time series programs initialize data before the computation
			if (m_TimeSeries)
			{
				WriteTimeSeriesInit(m_Program.InputDimension);
			}

			//
			// Reset the memory cells
			m_Writer.WriteLine("\n\t\tfor (int Cell=0; Cell<" + m_Program.CountMemory + "; Cell++)");
			m_Writer.WriteLine("\t\t\tm_Memory[Cell]=0;");
			m_Writer.WriteLine();

			//
			// Compute the result
			m_Writer.Write("\t\tdouble Result=");

			WriteProgramBody(rpb.Root);

			//
			// Close off the result computation
			m_Writer.WriteLine(";");
			m_Writer.WriteLine();

			//
			// Update the input history
			if (!m_TimeSeries)
			{
				WriteInputHistoryUpdate(m_Program.InputDimension);
			}

			//
			// Close off the function and get the result returned
			m_Writer.WriteLine();
			m_Writer.WriteLine("\treturn Result;");

			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();

			return true;
		}

		/// <summary>
		/// Create the code section that updates the input history for each call
		/// to the "Compute" function.
		/// </summary>
		/// /// <param name="InputDimension">Number of historical parameters</param>
		private void WriteInputHistoryUpdate(int InputDimension)
		{
			m_Writer.WriteLine("\t\t" + ListType + "<" + DoubleType + "> InputRow = new " + ListType + "<" + DoubleType + ">(" + InputDimension + ");");
			for (int Parameter = 0; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\t\tInputRow." + ListAdd + "(t" + Parameter + ");");
			}
			m_Writer.WriteLine("\t\tInputHistory." + ListAdd + "(InputRow);");

			m_Writer.WriteLine();
		}

		/// <summary>
		/// Declare each of the User Defined terminals
		/// </summary>
		private void WriteProgramParameters()
		{
			for (short nTerminal = 0; nTerminal < m_Program.InputDimension; nTerminal++)
			{
				if (nTerminal != 0)
				{
					m_Writer.Write(",");
				}
				m_Writer.Write("double t" + Convert.ToString(nTerminal));
			}
		}

		/// <summary>
		/// Given a tree node, the node is translated into code, this method is
		/// recursively called for each of the node's children.
		/// </summary>
		/// <param name="node">The node to translate</param>
		protected void WriteProgramBody(GPNode node)
		{
			//
			// Recursively work through the nodes
			if (node is GPNodeTerminal)
			{
				m_Writer.Write(((GPNodeTerminal)node).ToString());
				//
				// There are no children, so don't need to make a recursive call
			}
			else if (node is GPNodeFunction)
			{
				if (node is GPNodeFunctionADF)
				{
					GPNodeFunctionADF adf = (GPNodeFunctionADF)node;
					m_Writer.Write("\n\t\tADF" + adf.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADF are its children
					for (int Param = 0; Param < adf.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}

					m_Writer.Write(")\n\t\t");
				}
				else if (node is GPNodeFunctionADL)
				{
					GPNodeFunctionADL adl = (GPNodeFunctionADL)node;
					m_Writer.Write("\n\t\tADL" + adl.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADL are its children
					for (int Param = 0; Param < adl.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(")\n\t\t");
				}
				else if (node is GPNodeFunctionADR)
				{
					GPNodeFunctionADR adr = (GPNodeFunctionADR)node;
					m_Writer.Write("\n\t\tADR" + adr.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADR are its children
					for (int Param = 0; Param < adr.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(")\n\t\t");
				}
				else if (node is GPNodeFunctionSetMem)
				{
					m_Writer.Write("\n\t\tSetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(",");
					WriteProgramBody(((GPNodeFunction)node).Children[1]);
					m_Writer.Write(")");
				}
				else if (node is GPNodeFunctionGetMem)
				{
					m_Writer.Write("\n\t\tGetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(")");
				}
				else
				{
					m_Writer.Write("\n\t\t" + node.ToString() + "(");
					//
					// The parameters to the function are its children
					for (short Param = 0; Param < m_FunctionSet[node.ToString().ToUpper()].Arity; Param++)
					{
						if (Param > 0)
							m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(")");
				}
			}
			else
			{
				Console.WriteLine("Have an undefined node we are tying to write");
			}
		}

		/// <summary>
		/// Writes the indicated ADF to the file
		/// </summary>
		/// <param name="ADBranch">The ADF to write</param>
		public override bool WriteADF(GPProgramBranchADF adf)
		{
			//
			// Write the function declaration
			m_Writer.WriteLine("\tprivate static int countADF" + adf.WhichFunction + "=0;");
			m_Writer.Write("\tprivate double ADF" + adf.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADFParameters(adf);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("\t{");

			m_Writer.WriteLine("\t\tif (countADF" + adf.WhichFunction + " >= 1) return 1.0;");
			m_Writer.WriteLine("\t\tcountADF" + adf.WhichFunction + "++;");
			m_Writer.WriteLine();

			m_Writer.Write("\t\tdouble fResult=");

			WriteProgramBody(adf.Root);

			//
			// Close off the function
			m_Writer.WriteLine(";");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tcountADF" + adf.WhichFunction + "--;");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\treturn fResult;");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();

			return true;
		}

		/// <summary>
		/// Declare each of the ADF parameters
		/// </summary>
		/// <param name="ADBranch">Which ADF to work with</param>
		private void WriteADFParameters(GPProgramBranchADF adf)
		{
			for (short nParam = 0; nParam < adf.NumberArgs; nParam++)
			{
				if (nParam != 0)
				{
					m_Writer.Write(",");
				}
				m_Writer.Write("double p" + Convert.ToString(nParam));
			}
		}

		/// <summary>
		/// Writes the indicated ADL to the file
		/// </summary>
		/// <param name="adl">The ADL to write</param>
		/// <returns></returns>
		public override bool WriteADL(GPProgramBranchADL adl)
		{
			//
			// Write the function declaration
			m_Writer.Write("\tprivate double ADL" + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("\t{");

			//
			// Call the LIB
			m_Writer.Write("\t\tLIB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");
			m_Writer.WriteLine();

			//
			// Initialize the loop
			m_Writer.WriteLine("\t\tint LoopIndex = 0;");
			m_Writer.WriteLine("\t\tdouble Result = 0.0;");

			//
			// Prepare the condition
			m_Writer.Write("\t\twhile ((LoopIndex < 25) && (LCB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(") > 0.0))");

			//
			// Write the loop body
			m_Writer.WriteLine("\t\t{");
			m_Writer.Write("\t\t\tResult = LBB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");

			m_Writer.Write("\t\t\tLUB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");

			m_Writer.WriteLine("\t\t\tLoopIndex++;");

			m_Writer.WriteLine("\t\t}");
			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("\treturn Result;");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();

			//
			// Write each of the four supporting methods for the loop
			WriteADLBranch(adl, "LIB", adl.LIB);
			WriteADLBranch(adl, "LCB", adl.LCB);
			WriteADLBranch(adl, "LBB", adl.LBB);
			WriteADLBranch(adl, "LUB", adl.LUB);

			return true;
		}

		/// <summary>
		/// TODO: Not implemented, because I haven't yet got the ADR code working
		/// </summary>
		/// <param name="adr"></param>
		/// <returns></returns>
		public override bool WriteADR(GPProgramBranchADR adr) { return true; }

		/// <summary>
		/// Writes a function for the passed in loop branch
		/// </summary>
		/// <param name="adl">Parent ADL tree</param>
		/// <param name="ADLBranch">Root name for the function</param>
		/// <param name="BranchRoot">Root GPNode of the loop branch</param>
		private void WriteADLBranch(GPProgramBranchADL adl, String BranchName, GPNode BranchRoot)
		{
			//
			// Write the function declaration
			m_Writer.Write("\tprivate double " + BranchName + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("\t{");

			m_Writer.Write("\t\tdouble fResult=");

			WriteProgramBody(BranchRoot);
			m_Writer.WriteLine(";");

			//
			// Close the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("\treturn Result;");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();
		}

		/// <summary>
		/// Declare each of the ADL parameters
		/// </summary>
		/// <param name="adl"></param>
		/// <param name="WriteType"></param>
		private void WriteADLParameters(GPProgramBranchADL adl, bool WriteType)
		{
			for (short nParam = 0; nParam < adl.NumberArgs; nParam++)
			{
				if (nParam != 0)
				{
					m_Writer.Write(",");
				}
				if (WriteType)
				{
					m_Writer.Write("double ");
				}
				m_Writer.Write("p" + Convert.ToString(nParam));
			}
		}

		/// <summary>
		/// Write the needed support methods and then close off the main program class
		/// </summary>
		/// <param name="Program">Program being translated</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteTrailer(GPProgram Program)
		{
			//
			// Write the built=in support methods
			WriteSetMem();
			WriteGetMem();

			//
			// Write the user defined methods
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				String FunctionCode = WriteUserFunction((GPLanguageWriter.tagUserDefinedFunction)kvp.Value);
				m_Writer.Write(FunctionCode);
			}

			//
			// Close off the main program class
			m_Writer.WriteLine("}");

			return true;
		}

		/// <summary>
		/// Convert the user code into a C#/Java language version that can
		/// be used byt he program.
		/// </summary>
		/// <param name="Function">User Defined components</param>
		/// <returns>True/False upon success failure</returns>
		private String WriteUserFunction(GPLanguageWriter.tagUserDefinedFunction Function)
		{
			StringBuilder FunctionCode = new StringBuilder();

			//
			// Build the function name and parameter list
			FunctionCode.Append("\tprivate double " + Function.Name + "(");
			for (short Param = 0; Param < Function.Arity; Param++)
			{
				//
				// See if we should add a comma
				if (Param > 0)
				{
					FunctionCode.Append(", ");
				}
				FunctionCode.Append("double p" + Param);
			}
			FunctionCode.AppendLine(")");

			//
			// Opening brace & initial tab indent
			FunctionCode.AppendLine("\t{");
			FunctionCode.Append("\t\t");
			//
			// Place the user code but first, do a little diddy that makes
			// sure the code in indented in a pretty way.
			StringBuilder UserCode = new StringBuilder(Function.UserCode);
			UserCode.Replace("\n", "\n\t\t");
			FunctionCode.AppendLine(UserCode.ToString());

			//
			// Closing brace
			FunctionCode.AppendLine("\t}");

			return FunctionCode.ToString();
		}

		protected abstract void WriteSetMem();
		protected abstract void WriteGetMem();

		/// <summary>
		/// Only difference between C#/Java is the capitalization of the abs function
		/// </summary>
		/// <param name="FunctionAbs"></param>
		protected void WriteSetMem(String FunctionAbs)
		{
			m_Writer.WriteLine("\tprivate double SetMem(double p0,double p1)");
			m_Writer.WriteLine("\t{");
			m_Writer.WriteLine("\t\tm_Memory[" + FunctionAbs + "(((int)p0) % " + m_Program.CountMemory + ")]=p1;");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\treturn p1;");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();
		}

		/// <summary>
		/// Only difference between C#/Java is the capitalization of the abs function
		/// </summary>
		/// <param name="FunctionAbs"></param>
		protected void WriteGetMem(String FunctionAbs)
		{
			m_Writer.WriteLine("\tprivate double GetMem(double p0)");
			m_Writer.WriteLine("\t{");
			m_Writer.WriteLine("\t\treturn m_Memory[" + FunctionAbs + "(((int)p0) % " + m_Program.CountMemory + ")];");
			m_Writer.WriteLine("\t}");
			m_Writer.WriteLine();
		}
	}
}
