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
	public class GPLanguageWriterVBNet : GPLanguageWriter
	{
		public GPLanguageWriterVBNet(GPProgram Program, bool TimeSeries, SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
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
		/// VB.NET represents the program as a class that can be incorporated into
		/// another program.  This method writes the class header for the "Program".
		/// </summary>
		/// <param name="Program">The program object we are translating</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteHeader(GPProgram Program)
		{
			m_Writer.WriteLine("Public Class GeneticProgram");
			m_Writer.WriteLine();

			//
			// Write the indexed memory
			m_Writer.WriteLine("\tPrivate m_Memory("+m_Program.CountMemory +") As Double");
			m_Writer.WriteLine();

			//
			// Write the input history
			m_Writer.WriteLine("\tPrivate InputHistory As List(Of List(Of Double))");
			m_Writer.WriteLine();

			//
			// Write the constructor - Have it initialize the time series array
			m_Writer.WriteLine("\tPublic Sub New()");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tInputHistory = New List(Of List(Of Double))()");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\tEnd Sub");
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
			m_Writer.Write("\tPublic Function Compute(");
			//
			// Write the input parameters
			WriteProgramParameters();
			m_Writer.WriteLine(") As Double");
			m_Writer.WriteLine();

			//
			// Time series programs initialize data before the computation
			if (m_TimeSeries)
			{
				WriteTimeSeriesInit(m_Program.InputDimension);
			}

			//
			// Reset the memory cells
			m_Writer.WriteLine("\t\tDim Cell As Integer");
			m_Writer.WriteLine("\t\tFor Cell = 0 To " + m_Program.CountMemory);
			m_Writer.WriteLine("\t\t\tm_Memory(Cell) = 0");
			m_Writer.WriteLine("\t\tNext");
			m_Writer.WriteLine();

			m_Writer.WriteLine("\t\tDim Result As Double");
			m_Writer.Write("\t\tResult = ");

			WriteProgramBody(rpb.Root);
			m_Writer.WriteLine();
			m_Writer.WriteLine();

			//
			// Update the input history
			if (!m_TimeSeries)
			{
				WriteInputHistoryUpdate(m_Program.InputDimension);
			}

			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("\tReturn Result");
			m_Writer.WriteLine("\tEnd Function");
			m_Writer.WriteLine();

			return true;
		}

		/// <summary>
		/// A time series program needs to be initialized with the first set
		/// of historical values.
		/// </summary>
		/// <param name="InputDimension">Number of historical parameters</param>
		private void WriteTimeSeriesInit(int InputDimension)
		{
			m_Writer.WriteLine("\t\t'");
			m_Writer.WriteLine("\t\t' First time in, prepare the time series history");
			m_Writer.WriteLine("\t\t' All other times update with the newest value");
			m_Writer.WriteLine("\t\tIf InputHistory.Count = 0 Then");
			for (int Parameter = 0; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\t\t\tInputHistory.Add(New List(Of Double)(1))");
				m_Writer.WriteLine("\t\t\tInputHistory(" + Parameter + ").Add(t" + Parameter + ")");
			}

			//
			// All subsequent "Compute" calls add the newest value
			m_Writer.WriteLine("\t\tElse");

			m_Writer.WriteLine("\t\t\tDim TestRow As New List(Of Double)(1)");
			m_Writer.WriteLine("\t\t\tTestRow.Add(t" + (InputDimension - 1) + ")");
			m_Writer.WriteLine("\t\t\tInputHistory.Add(TestRow)");

			m_Writer.WriteLine("\t\tEnd If");
			m_Writer.WriteLine();
		}

		/// <summary>
		/// Create the code section that updates the input history for each call
		/// to the "Compute" function.
		/// </summary>
		/// /// <param name="InputDimension">Number of historical parameters</param>
		private void WriteInputHistoryUpdate(int InputDimension)
		{
			m_Writer.WriteLine("\t\tDim InputRow As New List(Of Double)("+InputDimension+")");
			for (int Parameter = 0; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\t\tInputRow.Add(t" + Parameter + ")");
			}
			m_Writer.WriteLine("\t\tInputHistory.Add(InputRow)");

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
				m_Writer.Write("ByVal t" + Convert.ToString(nTerminal)+" As Double");
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
					m_Writer.Write(" _\n\t\tADF" + adf.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADF are its children
					for (int Param = 0; Param < adf.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}

					m_Writer.Write(") _\n\t\t");
				}
				else if (node is GPNodeFunctionADL)
				{
					GPNodeFunctionADL adl = (GPNodeFunctionADL)node;
					m_Writer.Write(" _\n\t\tADL" + adl.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADL are its children
					for (int Param = 0; Param < adl.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(") _\n\t\t");
				}
				else if (node is GPNodeFunctionSetMem)
				{
					m_Writer.Write(" _\n\t\tSetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(",");
					WriteProgramBody(((GPNodeFunction)node).Children[1]);
					m_Writer.Write(")");
				}
				else if (node is GPNodeFunctionGetMem)
				{
					m_Writer.Write(" _\n\t\tGetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(")");
				}
				else
				{
					m_Writer.Write(" _\n\t\t" + node.ToString() + "(");
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
			m_Writer.WriteLine("\tPrivate countADF" + adf.WhichFunction + " As Integer=0");
			m_Writer.Write("\tPrivate Function ADF" + adf.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADFParameters(adf);
			m_Writer.WriteLine(") As Double");
			m_Writer.WriteLine();

			m_Writer.WriteLine("\t\tIf countADF" + adf.WhichFunction + " >= 1");
			m_Writer.WriteLine("\t\t\tReturn 1.0");
			m_Writer.WriteLine("\t\tEnd If");
			m_Writer.WriteLine("\t\tcountADF" + adf.WhichFunction + " = countADF" + adf.WhichFunction + " + 1");
			m_Writer.WriteLine();

			m_Writer.WriteLine("\t\tDim Result As Double");
			m_Writer.Write("\t\tResult= ");

			WriteProgramBody(adf.Root);

			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tcountADF" + adf.WhichFunction + " = countADF" + adf.WhichFunction + " - 1");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\treturn Result");
			m_Writer.WriteLine("\tEnd Function");
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
				m_Writer.Write("ByVal p" + Convert.ToString(nParam)+" As Double");
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
			m_Writer.Write("\tPrivate Function ADL" + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(") As Double");
			m_Writer.WriteLine();

			//
			// Call the LIB
			m_Writer.Write("\t\tLIB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine();

			//
			// Initialize the loop
			m_Writer.WriteLine("\t\tDim LoopIndex As Integer = 0");
			m_Writer.WriteLine("\t\tDim Result As Double = 0.0");

			//
			// Prepare the condition
			m_Writer.Write("\t\tWhile ((LoopIndex < 25) And (LCB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(") > 0.0))");

			//
			// Write the loop body
			m_Writer.WriteLine();
			m_Writer.Write("\t\t\tResult = LBB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(")");

			m_Writer.Write("\t\t\tLUB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(")");

			m_Writer.WriteLine("\t\t\tLoopIndex = LoopIndex + 1");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tEnd While");

			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\treturn Result");
			m_Writer.WriteLine("\tEnd Function");
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
		/// <param name="BranchName">Root name for the function</param>
		/// <param name="BranchRoot">Root GPNode of the loop branch</param>
		private void WriteADLBranch(GPProgramBranchADL adl, String BranchName, GPNode BranchRoot)
		{
			//
			// Write the function declaration
			m_Writer.Write("\tPrivate Function " + BranchName + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(") As Double");
			m_Writer.WriteLine();

			m_Writer.WriteLine("\t\tDim Result As Double");
			m_Writer.Write("\t\tResult=");

			WriteProgramBody(BranchRoot);
			m_Writer.WriteLine();

			//
			// Close the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tReturn Result");
			m_Writer.WriteLine("\tEnd Function");
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
					m_Writer.Write("ByVal ");
				}
				m_Writer.Write("p" + Convert.ToString(nParam));
				if (WriteType)
				{
					m_Writer.Write(" As Double");
				}
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
			m_Writer.WriteLine("End Class");

			return true;
		}

		/// <summary>
		/// Convert the user code into a VB.NET language version that can
		/// be used by the program.
		/// </summary>
		/// <param name="Function">User Defined components</param>
		/// <returns>True/False upon success failure</returns>
		private String WriteUserFunction(GPLanguageWriter.tagUserDefinedFunction Function)
		{
			StringBuilder FunctionCode = new StringBuilder();

			//
			// Build the function name and parameter list
			FunctionCode.Append("\tPrivate Function " + Function.Name + "(");
			for (short Param = 0; Param < Function.Arity; Param++)
			{
				//
				// See if we should add a comma
				if (Param > 0)
				{
					FunctionCode.Append(", ");
				}
				FunctionCode.Append("ByVal p" + Param+" As Double");
			}
			FunctionCode.AppendLine(") As Double");

			//
			// Space between declaration and body
			FunctionCode.AppendLine();
			//
			// Place the user code but first, do a little diddy that makes
			// sure the code in indented in a pretty way.
			StringBuilder UserCode = new StringBuilder(Function.UserCode);
			UserCode.Replace("\n", "\n\t\t");
			FunctionCode.AppendLine("\t\t"+UserCode.ToString());

			//
			// Close the function
			FunctionCode.AppendLine("\tEnd Function");
			FunctionCode.AppendLine();

			return FunctionCode.ToString();
		}

		/// <summary>
		/// Output the SetMem function
		/// </summary>
		protected void WriteSetMem()
		{
			m_Writer.WriteLine("\tPrivate Function SetMem(ByVal p0 As Double, ByVal p1 As Double) As Double");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tm_Memory(Math.Abs(CInt(p0) Mod " + m_Program.CountMemory + ")) = p1");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tReturn p1");
			m_Writer.WriteLine("\tEnd Function");
			m_Writer.WriteLine();
		}

		/// <summary>
		/// Output the GetMem function
		/// </summary>
		protected void WriteGetMem()
		{
			m_Writer.WriteLine("\tPrivate Function GetMem(ByVal p0 As Double) As Double");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tReturn m_Memory(Math.Abs(CInt(p0) Mod " + m_Program.CountMemory + "))");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\t\tEnd Function");
			m_Writer.WriteLine();
		}
	}
}
