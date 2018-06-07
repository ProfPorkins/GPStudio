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
	/// C language program writer
	/// </summary>
	public class GPLanguageWriterC : GPLanguageWriter
	{
		public GPLanguageWriterC(GPProgram Program, bool TimeSeries, SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
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
		/// TODO:
		/// </summary>
		/// <param name="Program">The program object we are translating</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteHeader(GPProgram Program)
		{
			//
			// Write prototypes for everything
			m_Writer.WriteLine("//");
			m_Writer.WriteLine("// Function Prototypes");
			m_Writer.WriteLine("double SetMem(double p0,double p1);");
			m_Writer.WriteLine("double GetMem(double p0);");

			//
			// Write the user defined method prototypes
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				StringBuilder Prototype = new StringBuilder();
				WriteUserFunctionPrototype(kvp.Value, Prototype);
				m_Writer.WriteLine(Prototype.ToString()+";");
			}
			m_Writer.WriteLine();

			//
			// Declare the indexed memory
			m_Writer.WriteLine();
			m_Writer.WriteLine("//");
			m_Writer.WriteLine("// Indexed Memory");
			m_Writer.WriteLine("double m_Memory["+m_Program.CountMemory+"];");
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
			m_Writer.Write("double Compute(");
			//
			// Write the input parameters
			WriteProgramParameters();
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("{");

			//
			// Reset the memory cells
			m_Writer.WriteLine("\n\tint Cell;");
			m_Writer.WriteLine("\n\tfor (Cell=0; Cell<" + m_Program.CountMemory + "; Cell++)");
			m_Writer.WriteLine("\t\tm_Memory[Cell]=0;");
			m_Writer.WriteLine();

			m_Writer.Write("return ");

			WriteProgramBody(rpb.Root);

			//
			// Close off the function
			m_Writer.WriteLine(";");
			m_Writer.WriteLine("}");
			m_Writer.WriteLine();

			return true;
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
					m_Writer.Write("\n\tADF" + adf.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADF are its children
					for (int Param = 0; Param < adf.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}

					m_Writer.Write(")\n\t");
				}
				else if (node is GPNodeFunctionADL)
				{
					GPNodeFunctionADL adl = (GPNodeFunctionADL)node;
					m_Writer.Write("\n\tADL" + adl.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADL are its children
					for (int Param = 0; Param < adl.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(")\n\t");
				}
				else if (node is GPNodeFunctionSetMem)
				{
					m_Writer.Write("\n\tSetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(",");
					WriteProgramBody(((GPNodeFunction)node).Children[1]);
					m_Writer.Write(")");
				}
				else if (node is GPNodeFunctionGetMem)
				{
					m_Writer.Write("\n\tGetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(")");
				}
				else
				{
					m_Writer.Write("\n\t" + node.ToString() + "(");
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
			m_Writer.WriteLine("int countADF" + adf.WhichFunction + "=0;");
			m_Writer.Write("double ADF" + adf.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADFParameters(adf);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("\t{");

			m_Writer.WriteLine("\tif (countADF" + adf.WhichFunction + " >= 1) return 1.0;");
			m_Writer.WriteLine("\tcountADF" + adf.WhichFunction + "++;");
			m_Writer.WriteLine();

			m_Writer.Write("\tdouble Result=");

			WriteProgramBody(adf.Root);

			//
			// Close off the function
			m_Writer.WriteLine(";");
			m_Writer.WriteLine();
			m_Writer.WriteLine("\tcountADF" + adf.WhichFunction + "--;");
			m_Writer.WriteLine();
			m_Writer.WriteLine("return Result;");
			m_Writer.WriteLine("}");
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
			m_Writer.Write("double ADL" + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("{");

			//
			// Call the LIB
			m_Writer.Write("\tLIB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");
			m_Writer.WriteLine();

			//
			// Initialize the loop
			m_Writer.WriteLine("\tint LoopIndex = 0;");
			m_Writer.WriteLine("\tdouble Result = 0.0;");

			//
			// Prepare the condition
			m_Writer.Write("\twhile ((LoopIndex < 25) && (LCB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(") > 0.0))");

			//
			// Write the loop body
			m_Writer.WriteLine("\t{");
			m_Writer.Write("\t\tResult = LBB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");

			m_Writer.Write("\t\tLUB" + adl.WhichFunction + "(");
			WriteADLParameters(adl, false);
			m_Writer.WriteLine(");");

			m_Writer.WriteLine("\t\tLoopIndex++;");

			m_Writer.WriteLine("\t}");
			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("return Result;");
			m_Writer.WriteLine("}");
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
			m_Writer.Write("double " + BranchName + adl.WhichFunction + "(");
			//
			// Write the function parameters
			WriteADLParameters(adl, true);
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("{");

			m_Writer.Write("\tdouble Result=");

			WriteProgramBody(BranchRoot);
			m_Writer.WriteLine(";");

			//
			// Close the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("return Result;");
			m_Writer.WriteLine("}");
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
			WriteUserFunctionPrototype(Function, FunctionCode);
			FunctionCode.AppendLine();
			//
			// Opening brace
			FunctionCode.AppendLine("{");
			//
			// Place the user code but first, do a little diddy that makes
			// sure the code in indented in a pretty way.
			StringBuilder UserCode = new StringBuilder(Function.UserCode);
			UserCode.Replace("\n", "\n\t");
			FunctionCode.AppendLine(UserCode.ToString());

			//
			// Closing brace
			FunctionCode.AppendLine("}");

			return FunctionCode.ToString();
		}

		/// <summary>
		/// My first refactored method!  This writes the prototype for a function, needed
		/// by the function writer and up in the header declaration.
		/// </summary>
		/// <param name="Function"></param>
		/// <param name="FunctionCode"></param>
		private void WriteUserFunctionPrototype(GPLanguageWriter.tagUserDefinedFunction Function, StringBuilder FunctionCode)
		{
			FunctionCode.Append("double " + Function.Name + "(");
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
			FunctionCode.Append(")");
		}

		/// <summary>
		/// Output the SetMem function
		/// </summary>
		protected void WriteSetMem()
		{
			m_Writer.WriteLine("double SetMem(double p0,double p1)");
			m_Writer.WriteLine("{");
			m_Writer.WriteLine("\tm_Memory[(int)fabs(p0) % " + m_Program.CountMemory + "]=p1;");
			m_Writer.WriteLine();
			m_Writer.WriteLine("return p1;");
			m_Writer.WriteLine("}");
			m_Writer.WriteLine();
		}

		/// <summary>
		/// Output the GetMem function
		/// </summary>
		protected void WriteGetMem()
		{
			m_Writer.WriteLine("double GetMem(double p0)");
			m_Writer.WriteLine("{");
			m_Writer.WriteLine("\treturn m_Memory[(int)fabs(p0) % " + m_Program.CountMemory + "];");
			m_Writer.WriteLine("}");
			m_Writer.WriteLine();
		}
	}
}
