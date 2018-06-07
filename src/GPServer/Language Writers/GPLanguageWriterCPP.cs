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
	/// C++ language program writer
	/// </summary>
	public class GPLanguageWriterCPP : GPLanguageWriter
	{
		public GPLanguageWriterCPP(GPProgram Program, bool TimeSeries, SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
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
		/// Write the C++ class header file
		/// </summary>
		/// <param name="Program">The program object we are translating</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteHeader(GPProgram Program)
		{
			//
			// Get the class declaration started
			m_Writer.WriteLine("#pragma once");
			m_Writer.WriteLine("#include <math.h>");
			m_Writer.WriteLine("#include <vector>");
			m_Writer.WriteLine("using namespace std;");
			m_Writer.WriteLine();
			m_Writer.WriteLine("class GeneticProgram");
			m_Writer.WriteLine("{");
			m_Writer.WriteLine("public:");
			m_Writer.WriteLine("\tGeneticProgram(void) {}");
			m_Writer.WriteLine();

			//
			// Write the GPCompute prototype
			m_Writer.Write("\tdouble Compute(");
			//
			// Write the input parameters
			WriteProgramParameters();
			m_Writer.WriteLine(");");

			//
			// Create the private section
			m_Writer.WriteLine();
			m_Writer.WriteLine("private:");

			//
			// Declare the indexed memory & the input history
			m_Writer.WriteLine("\t//");
			m_Writer.WriteLine("\t// Indexed Memory");
			m_Writer.WriteLine("\tdouble m_Memory[" + m_Program.CountMemory + "];");
			m_Writer.WriteLine("\tvector<vector<double>> InputHistory;");
			m_Writer.WriteLine();

			//
			// Write prototypes for everything
			m_Writer.WriteLine("\t//");
			m_Writer.WriteLine("\t// Function Prototypes");
			m_Writer.WriteLine("\tdouble SetMem(double p0,double p1);");
			m_Writer.WriteLine("\tdouble GetMem(double p0);");

			//
			// Write the user defined method prototypes
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				StringBuilder Prototype = new StringBuilder();
				WriteUserFunctionPrototype(kvp.Value, Prototype,"");
				m_Writer.WriteLine("\t"+Prototype.ToString() + ";");
			}
			m_Writer.WriteLine();

			//
			// ADF Prototypes
			foreach (GPProgramBranchADF adf in Program.ADF)
			{
				String Prototype=BuildADFPrototype(adf, "");
				m_Writer.WriteLine("\t" + Prototype + ";");
			}

			//
			// ADL Prototypes
			foreach (GPProgramBranchADL adl in Program.ADL)
			{
				m_Writer.WriteLine("\t" + BuildADLPrototype(adl, "ADL", "") + ";");
				m_Writer.WriteLine("\t" + BuildADLPrototype(adl, "LIB", "") + ";");
				m_Writer.WriteLine("\t" + BuildADLPrototype(adl, "LCB", "") + ";");
				m_Writer.WriteLine("\t" + BuildADLPrototype(adl, "LBB", "") + ";");
				m_Writer.WriteLine("\t" + BuildADLPrototype(adl, "LUB", "") + ";");
			}

			//
			// Close the class declaration
			m_Writer.WriteLine("};");

			//
			// Tell the user that everything else is the .cpp file
			m_Writer.WriteLine();
			m_Writer.WriteLine("// --- The section below is the .cpp class implementation ---");
			m_Writer.WriteLine("#include \"GeneticProgram.h\"");
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
			m_Writer.Write("double GeneticProgram::Compute(");
			//
			// Write the input parameters
			WriteProgramParameters();
			m_Writer.WriteLine(")");
			m_Writer.WriteLine("{");

			//
			// Time series programs initialize data before the computation
			if (m_TimeSeries)
			{
				WriteTimeSeriesInit(m_Program.InputDimension);
			}

			//
			// Reset the memory cells
			m_Writer.WriteLine("\n\tfor (int Cell=0; Cell<" + m_Program.CountMemory + "; Cell++)");
			m_Writer.WriteLine("\t\tm_Memory[Cell]=0;");
			m_Writer.WriteLine();

			//
			// Perform the computation
			m_Writer.Write("\tdouble Result= ");
			WriteProgramBody(rpb.Root);
			m_Writer.WriteLine(";\n\n");

			//
			// Update the input history
			if (!m_TimeSeries)
			{
				WriteInputHistoryUpdate(m_Program.InputDimension);
			}

			//
			// Close off the function
			m_Writer.WriteLine();
			m_Writer.WriteLine("return Result;");
			m_Writer.WriteLine("}");
			m_Writer.WriteLine();

			return true;
		}

		/// <summary>
		/// A time series program needs to be initialized with the first set
		/// of historical values.
		/// </summary>
		/// <param name="InputDimension">Number of historical parameters</param>
		protected void WriteTimeSeriesInit(int InputDimension)
		{
			m_Writer.WriteLine("\t//");
			m_Writer.WriteLine("\t// First time in, prepare the time series history");
			m_Writer.WriteLine("\t// All other times update with the newest value");
			m_Writer.WriteLine("\tif (InputHistory.size() == 0)");
			m_Writer.WriteLine("\t{");
			m_Writer.WriteLine("\t\tvector<double> TestRow;");
			m_Writer.WriteLine("\t\tTestRow.push_back(t0);");
			m_Writer.WriteLine("\t\tInputHistory.push_back(TestRow);");
			for (int Parameter = 1; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\t\tTestRow[0] = t"+Parameter+";");
				m_Writer.WriteLine("\t\tInputHistory.push_back(TestRow);");
			}

			m_Writer.WriteLine("\t}");

			//
			// All subsequent "Compute" calls add the newest value
			m_Writer.WriteLine("\telse");
			m_Writer.WriteLine("\t{");

			m_Writer.WriteLine("\t\tvector<double> TestRow;");
			m_Writer.WriteLine("\t\tTestRow.push_back(t" + (InputDimension - 1) + ");");
			m_Writer.WriteLine("\t\tInputHistory.push_back(TestRow);");

			m_Writer.WriteLine("\t}");
		}

		/// <summary>
		/// Create the code section that updates the input history for each call
		/// to the "Compute" function.
		/// </summary>
		/// /// <param name="InputDimension">Number of historical parameters</param>
		private void WriteInputHistoryUpdate(int InputDimension)
		{
			m_Writer.WriteLine("\tvector<double> InputRow;");
			for (int Parameter = 0; Parameter < InputDimension; Parameter++)
			{
				m_Writer.WriteLine("\tInputRow.push_back(t" + Parameter + ");");
			}
			m_Writer.WriteLine("\tInputHistory.push_back(InputRow);");

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
			m_Writer.WriteLine(BuildADFPrototype(adf, "GeneticProgram::"));
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
		/// Write an ADF prototype
		/// </summary>
		/// <param name="ADBranch"></param>
		/// <param name="ClassRoot"></param>
		/// <returns>String representation of the ADF prototype</returns>
		private String BuildADFPrototype(GPProgramBranchADF adf,String ClassRoot)
		{
			String Prototype = "double " + ClassRoot + "ADF" + adf.WhichFunction + "(";
			Prototype += BuildADFParameters(adf)+")";

			return Prototype;
		}

		/// <summary>
		/// Declare each of the ADF parameters
		/// </summary>
		/// <param name="ADBranch">Which ADF to work with</param>
		/// <returns>String representation of the ADF parameters</returns>
		private String BuildADFParameters(GPProgramBranchADF adf)
		{
			String Parameters = "";
			for (short nParam = 0; nParam < adf.NumberArgs; nParam++)
			{
				if (nParam != 0)
				{
					Parameters += ",";
				}
				Parameters+="double p" + Convert.ToString(nParam);
			}

			return Parameters;
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
			m_Writer.WriteLine(BuildADLPrototype(adl, "ADL", "GeneticProgram::"));
			m_Writer.WriteLine("{");

			//
			// Call the LIB
			m_Writer.Write("\tLIB" + adl.WhichFunction + "(");
			m_Writer.Write(BuildADLParameters(adl, false));
			m_Writer.WriteLine(");");
			m_Writer.WriteLine();

			//
			// Initialize the loop
			m_Writer.WriteLine("\tint LoopIndex = 0;");
			m_Writer.WriteLine("\tdouble Result = 0.0;");

			//
			// Prepare the condition
			m_Writer.Write("\twhile ((LoopIndex < 25) && (LCB" + adl.WhichFunction + "(");
			m_Writer.Write(BuildADLParameters(adl, false));
			m_Writer.WriteLine(") > 0.0))");

			//
			// Write the loop body
			m_Writer.WriteLine("\t{");
			m_Writer.Write("\t\tResult = LBB" + adl.WhichFunction + "(");
			m_Writer.Write(BuildADLParameters(adl, false));
			m_Writer.WriteLine(");");

			m_Writer.Write("\t\tLUB" + adl.WhichFunction + "(");
			m_Writer.Write(BuildADLParameters(adl, false));
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
		/// Write an ADL prototype
		/// </summary>
		/// <param name="adl"></param>
		/// <param name="ADLBranch"></param>
		/// <param name="ClassRoot"></param>
		/// <returns>String representation of the ADL prototype</returns>
		private String BuildADLPrototype(GPProgramBranchADL adl,String ADLBranch,String ClassRoot)
		{
			String Prototype = "double " + ClassRoot + ADLBranch + adl.WhichFunction + "(";
			Prototype += BuildADLParameters(adl, true) + ")";

			return Prototype;
		}

		/// <summary>
		/// Writes a function for the passed in loop branch
		/// </summary>
		/// <param name="adl">Parent ADL tree</param>
		/// <param name="ADLBranch">Root name for the function</param>
		/// <param name="BranchRoot">Root GPNode of the loop branch</param>
		private void WriteADLBranch(GPProgramBranchADL adl, String ADLBranch, GPNode BranchRoot)
		{
			//
			// Write the function declaration
			m_Writer.WriteLine(BuildADLPrototype(adl, ADLBranch, "GeneticProgram::"));
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
		/// <returns>String representation of the ADL parameters</returns>
		private String BuildADLParameters(GPProgramBranchADL adl, bool WriteType)
		{
			String Parameters = "";
			for (short nParam = 0; nParam < adl.NumberArgs; nParam++)
			{
				if (nParam != 0)
				{
					Parameters += ",";
				}
				if (WriteType)
				{
					Parameters+="double ";
				}
				Parameters+="p" + Convert.ToString(nParam);
			}

			return Parameters;
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
			WriteUserFunctionPrototype(Function, FunctionCode, "GeneticProgram::");
			FunctionCode.AppendLine();
			//
			// Opening brace
			FunctionCode.AppendLine("{");
			//
			// Place the user code but first, do a little diddy that makes
			// sure the code in indented in a pretty way.
			StringBuilder UserCode = new StringBuilder(Function.UserCode);
			UserCode.Replace("\n", "\n\t");
			FunctionCode.AppendLine("\t"+UserCode.ToString());

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
		private void WriteUserFunctionPrototype(GPLanguageWriter.tagUserDefinedFunction Function, StringBuilder FunctionCode,String ClassRoot)
		{
			FunctionCode.Append("double " +ClassRoot+ Function.Name + "(");
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
			m_Writer.WriteLine("double GeneticProgram::SetMem(double p0,double p1)");
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
			m_Writer.WriteLine("double GeneticProgram::GetMem(double p0)");
			m_Writer.WriteLine("{");
			m_Writer.WriteLine("\treturn m_Memory[(int)fabs(p0) % " + m_Program.CountMemory + "];");
			m_Writer.WriteLine("}");
			m_Writer.WriteLine();
		}
	}
}
