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
	/// FORTRAN language program writer
	/// </summary>
	public class GPLanguageWriterFortran : GPLanguageWriter
	{
		public GPLanguageWriterFortran(GPProgram Program, bool TimeSeries, SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> FunctionSet)
			: base(Program,TimeSeries)
		{
			m_FunctionSet = FunctionSet;

			//
			// Prepare a list of all possible Fortran intrinsics.  This way, if the user
			// ever defines a function that is the same as an intrinsic, the intrinsic is
			// used instead of making a mess of things by declaring a user defined function
			// of the same name.
			FunctionIntrinsics = new SortedList<string, string>();
			FunctionIntrinsics.Add("SIN", "SIN");
			FunctionIntrinsics.Add("ASIN", "ASIN");
			FunctionIntrinsics.Add("SINH", "SINH");
			FunctionIntrinsics.Add("COS", "COS");
			FunctionIntrinsics.Add("ACOS", "ACOS");
			FunctionIntrinsics.Add("COSH", "COSH");
			FunctionIntrinsics.Add("TAN", "TAN");
			FunctionIntrinsics.Add("ATAN", "ATAN");
			FunctionIntrinsics.Add("TANH", "TANH");
			FunctionIntrinsics.Add("ATAN2", "ATAN2");
			FunctionIntrinsics.Add("EXP", "EXP");
			FunctionIntrinsics.Add("LOG", "LOG");
			FunctionIntrinsics.Add("LOG10", "LOG10");
			FunctionIntrinsics.Add("SQRT", "SQRT");
			FunctionIntrinsics.Add("ABS", "ABS");
			FunctionIntrinsics.Add("IABS", "IABS");
			FunctionIntrinsics.Add("NINT", "NINT");
			FunctionIntrinsics.Add("ANINT", "ANINT");
			FunctionIntrinsics.Add("MIN", "MIN");
			FunctionIntrinsics.Add("MAX", "MAX");
			FunctionIntrinsics.Add("FLOOR", "FLOOR");
			FunctionIntrinsics.Add("CEILING", "CEILING");
		}
		protected SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> m_FunctionSet;
		SortedList<String, String> FunctionIntrinsics;

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
			// Program declaration
			WriteFortranString("      PROGRAM GeneticProgram");

			//
			// Declare the indexed memory - damn common block!
			WriteFortranString("");
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			//
			// Declare the ADF counters
			if (Program.ADF.Count > 0)
			{
				m_Writer.Write("      INTEGER ");
				foreach (GPProgramBranchADF adf in Program.ADF)
				{
					if (adf.WhichFunction > 0)
					{
						m_Writer.Write(",");
					}
					m_Writer.Write("CountADF" + adf.WhichFunction);
				}
				WriteFortranString("");

				m_Writer.Write("      COMMON /ADF/ ");
				foreach (GPProgramBranchADF adf in Program.ADF)
				{
					if (adf.WhichFunction > 0)
					{
						m_Writer.Write(",");
					}
					m_Writer.Write("CountADF" + adf.WhichFunction);
				}
				WriteFortranString("");
			}



			WriteFortranString("");
			WriteFortranString("      STOP");
			WriteFortranString("      END");
			WriteFortranString("");

			return true;
		}

		/// <summary>
		/// Writes the specified RPB to the stream
		/// </summary>
		/// <param name="rpb">RPB tree to translate</param>
		/// <returns>True/False upon success or failure</returns>
		public override bool WriteRPB(GPProgramBranchRPB rpb)
		{
			WriteFunctionPrototype("Compute", m_Program.InputDimension,"t");

			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			//
			// Declare the function types
			WriteFortranString("      REAL SetMem");
			WriteFortranString("      REAL GetMem");
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				if (!IsFortranIntrinsic(kvp.Key))
				{
					WriteFortranString("      REAL " + ((GPLanguageWriter.tagUserDefinedFunction)kvp.Value).Name);
				}
			}
			//
			// Declare any ADFs
			foreach (GPProgramBranchADF adf in m_Program.ADF)
			{
				WriteFortranString("      REAL ADF" + adf.WhichFunction);
			}
			//
			// Declare any ADLs
			foreach (GPProgramBranchADL adl in m_Program.ADL)
			{
				WriteFortranString("      REAL ADL" + adl.WhichFunction);
			}

			WriteFortranString("      INTEGER Cell");
			WriteFortranString("");

			//
			// Reset the memory cells
			WriteFortranString("      DO 10 Cell=1," + m_Program.CountMemory);
			WriteFortranString("           Memory(Cell)=0");
			WriteFortranString("10    CONTINUE");
			WriteFortranString("");

			m_Writer.Write("      Compute= ");

			WriteProgramBody(rpb.Root);

			//
			// Close off the function
			WriteFortranString("");
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");

			return true;
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
				if (node is GPNodeTerminalDouble || node is GPNodeTerminalInteger)
				{
					//
					// Put doubles on their own line, helps prevent the lines
					// from getting too long
					m_Writer.WriteLine();
					m_Writer.Write("     . ");
				}
				m_Writer.Write(((GPNodeTerminal)node).ToString());
				//
				// There are no children, so don't need to make a recursive call
			}
			else if (node is GPNodeFunction)
			{
				if (node is GPNodeFunctionADF)
				{
					GPNodeFunctionADF adf = (GPNodeFunctionADF)node;
					m_Writer.Write("\n     . ADF" + adf.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADF are its children
					for (int Param = 0; Param < adf.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}

					m_Writer.Write(")\n     . ");
				}
				else if (node is GPNodeFunctionADL)
				{
					GPNodeFunctionADL adl = (GPNodeFunctionADL)node;
					m_Writer.Write("\n     . ADL" + adl.WhichFunction);
					m_Writer.Write("(");
					//
					// The parameters to the ADL are its children
					for (int Param = 0; Param < adl.NumberArgs; Param++)
					{
						if (Param > 0) m_Writer.Write(",");
						WriteProgramBody(((GPNodeFunction)node).Children[Param]);
					}
					m_Writer.Write(")\n     . ");
				}
				else if (node is GPNodeFunctionSetMem)
				{
					m_Writer.Write("\n     . SetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(",");
					WriteProgramBody(((GPNodeFunction)node).Children[1]);
					m_Writer.Write(")");
				}
				else if (node is GPNodeFunctionGetMem)
				{
					m_Writer.Write("\n     . GetMem(");
					WriteProgramBody(((GPNodeFunction)node).Children[0]);
					m_Writer.Write(")");
				}
				else
				{
					m_Writer.Write("\n     . " + node.ToString() + "(");
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
		/// <param name="adf">The ADF to write</param>
		public override bool WriteADF(GPProgramBranchADF adf)
		{
			WriteFunctionPrototype("ADF" + adf.WhichFunction, adf.NumberArgs, "p");
			//
			// Add the common blocks
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			WriteFortranString("      INTEGER CountADF" + adf.WhichFunction);
			WriteFortranString("      COMMON /ADF/ CountADF" + adf.WhichFunction);
			//
			// Declare the function types
			WriteFortranString("      REAL SetMem");
			WriteFortranString("      REAL GetMem");
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				if (!IsFortranIntrinsic(kvp.Key))
				{
					WriteFortranString("      REAL " + ((GPLanguageWriter.tagUserDefinedFunction)kvp.Value).Name);
				}
			}

			WriteFortranString("");
			WriteFortranString("      IF (CountADF" + adf.WhichFunction + " .GE. 1) THEN");
			WriteFortranString("          CountADF" + adf.WhichFunction + "=1");
			WriteFortranString("          RETURN");
			WriteFortranString("      ENDIF");
			WriteFortranString("");
			WriteFortranString("      CountADF" + adf.WhichFunction + "=CountADF" + adf.WhichFunction + "+1");
			WriteFortranString("");

			m_Writer.Write("      ADF"+adf.WhichFunction+"=");

			WriteProgramBody(adf.Root);

			//
			// Close off the function
			WriteFortranString("");
			WriteFortranString("");
			WriteFortranString("      CountADF" + adf.WhichFunction + "=CountADF" + adf.WhichFunction + "-1");
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");

			return true;
		}

		/// <summary>
		/// Declare each of the AD program branch parameters
		/// </summary>
		/// <param name="ADBranch">Which ADF to work with</param>
		private String ConstructADParameters(GPProgramBranchADRoot ADBranch)
		{
			String Parameters = "";
			for (short nParam = 0; nParam < ADBranch.NumberArgs; nParam++)
			{
				if (nParam != 0)
				{
					Parameters += ",";
				}
				Parameters += ("p" + nParam);
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
			WriteFunctionPrototype("ADL" + adl.WhichFunction, adl.NumberArgs, "p");

			//
			// Delcare the loop branch functions
			WriteFortranString("      REAL LIB" + adl.WhichFunction);
			WriteFortranString("      REAL LCB" + adl.WhichFunction);
			WriteFortranString("      REAL LBB" + adl.WhichFunction);
			WriteFortranString("      REAL LUB" + adl.WhichFunction);

			//
			// Declare the loop variables
			WriteFortranString("      INTEGER LoopIndex");
			WriteFortranString("      REAL Result");
			WriteFortranString("      REAL Dummy");
			WriteFortranString("");

			//
			// Call the LIB
			WriteFortranString("      Dummy = LIB" + adl.WhichFunction + "(" + ConstructADParameters(adl) + ")");
			WriteFortranString("");

			//
			// Initialize the loop
			WriteFortranString("      LoopIndex = 0");
			WriteFortranString("      Result = 0.0");

			//
			// Prepare the condition
			WriteFortranString("      DO WHILE ((LoopIndex < 25) .AND. (LCB" + adl.WhichFunction + "("+ConstructADParameters(adl)+") .GT. 0.0))");

			//
			// Write the loop body
			WriteFortranString("          Result = LBB" + adl.WhichFunction + "("+ConstructADParameters(adl)+")");
			WriteFortranString("          Dummy = LUB" + adl.WhichFunction + "("+ConstructADParameters(adl)+")");
			WriteFortranString("          LoopIndex=LoopIndex+1");
			WriteFortranString("      ENDDO");

			//
			// Close off the function
			WriteFortranString("");
			WriteFortranString("      ADL" + adl.WhichFunction + "=Result");
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");

			//
			// Write each of the four supporting methods for the loop
			WriteADLBranch(adl, "LIB", adl.LIB);
			WriteADLBranch(adl, "LCB", adl.LCB);
			WriteADLBranch(adl, "LBB", adl.LBB);
			WriteADLBranch(adl, "LUB", adl.LUB);

			return true;
		}

		/// <summary>
		/// Writes a function for the passed in loop branch
		/// </summary>
		/// <param name="adl">Parent ADL tree</param>
		/// <param name="BranchName">Root name for the function</param>
		/// <param name="BranchRoot">Root GPNode of the loop branch</param>
		private void WriteADLBranch(GPProgramBranchADL adl, String BranchName, GPNode BranchRoot)
		{
			WriteFunctionPrototype(BranchName + adl.WhichFunction, adl.NumberArgs, "p");

			//
			// Add the common blocks
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			//
			// Declare the function types
			WriteFortranString("      REAL SetMem");
			WriteFortranString("      REAL GetMem");
			foreach (KeyValuePair<String, GPLanguageWriter.tagUserDefinedFunction> kvp in m_FunctionSet)
			{
				if (!IsFortranIntrinsic(kvp.Key))
				{
					WriteFortranString("      REAL " + ((GPLanguageWriter.tagUserDefinedFunction)kvp.Value).Name);
				}
			}
			//
			// Declare any ADFs
			foreach (GPProgramBranchADF adf in m_Program.ADF)
			{
				WriteFortranString("      REAL ADF" + adf.WhichFunction);
			}

			m_Writer.Write("      " + BranchName + adl.WhichFunction+"=");
			WriteFortranString("");

			WriteProgramBody(BranchRoot);
			WriteFortranString("");
			WriteFortranString("");

			//
			// Close the function
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");
		}

		/// <summary>
		/// TODO: Not implemented, because I haven't yet got the ADR code working
		/// </summary>
		/// <param name="adr"></param>
		/// <returns></returns>
		public override bool WriteADR(GPProgramBranchADR adr) { return true; }

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
				//
				// Keep an eye out for FORTRAN intrisics, don't write them
				if (!IsFortranIntrinsic(kvp.Key))
				{
					WriteUserFunction((GPLanguageWriter.tagUserDefinedFunction)kvp.Value);
					WriteFortranString("");
				}
			}

			return true;
		}

		/// <summary>
		/// Performs a test to see if the function in question is a FORTRAN instrinsic
		/// </summary>
		/// <param name="Function">Function name to test</param>
		/// <returns>True if it is an intrinsic, False otherwise</returns>
		private bool IsFortranIntrinsic(String Function)
		{
			return FunctionIntrinsics.ContainsKey(Function.ToUpper());
		}

		/// <summary>
		/// Convert the user code into a C#/Java language version that can
		/// be used byt he program.
		/// </summary>
		/// <param name="Function">User Defined components</param>
		private void WriteUserFunction(GPLanguageWriter.tagUserDefinedFunction Function)
		{
			WriteFunctionPrototype(Function.Name, Function.Arity, "p");
			//
			// Place the indexed memory Common block here
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			WriteFortranString("");

			//
			// Place the user code but first, do a little diddy that makes
			// sure the code in indented in a pretty way.
			StringBuilder UserCode = new StringBuilder(Function.UserCode);
			UserCode.Replace("\n", "\n      ");
			m_Writer.Write("      ");	// Need to indent the first line
			WriteFortranString(UserCode.ToString());

			//
			// Close off the function
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
		}

		/// <summary>
		/// Write a function prototype
		/// </summary>
		/// <param name="FunctionName">Name of the function</param>
		/// <param name="Arity">Number of parameters to the function</param>
		private void WriteFunctionPrototype(String FunctionName,short Arity,String Root)
		{
			String Prototype = "      REAL FUNCTION " + FunctionName + "(";
			String ParamTypes = "      REAL ";

			for (short Param = 0; Param < Arity; Param++)
			{
				//
				// See if we should add a comma
				if (Param > 0)
				{
					Prototype += ",";
					ParamTypes += ",";
				}
				Prototype += (Root + Param);

				//
				// While doing this, build up the type declarations
				ParamTypes+=(Root + Param);
			}

			Prototype += ")";
			WriteFortranString(Prototype);
			WriteFortranString("      IMPLICIT NONE");
			if (Arity > 0)
			{
				WriteFortranString(ParamTypes);
			}
		}

		/// <summary>
		/// Output the SetMem function
		/// </summary>
		protected void WriteSetMem()
		{
			WriteFortranString("      REAL FUNCTION SetMem(p0,p1)");
			WriteFortranString("      REAL p0,p1");
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			WriteFortranString("");
			WriteFortranString("          Memory(Mod(int(Abs(p0))," + m_Program.CountMemory + "))=p1");
			WriteFortranString("          SetMem=p1");
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");
		}

		/// <summary>
		/// Output the GetMem function
		/// </summary>
		protected void WriteGetMem()
		{
			WriteFortranString("      REAL FUNCTION GetMem(p0)");
			WriteFortranString("      REAL p0");
			WriteFortranString("      REAL Memory(" + m_Program.CountMemory + ")");
			WriteFortranString("      COMMON /idxmemory/ Memory");
			WriteFortranString("");
			WriteFortranString("          GetMem=Memory(Mod(int(Abs(p0))," + m_Program.CountMemory + "))");
			WriteFortranString("");
			WriteFortranString("      RETURN");
			WriteFortranString("      END");
			WriteFortranString("");
		}

		/// <summary>
		/// Writes out a potentially long string to the writer.  It writes it
		/// out in chunks of size 69 characters
		/// </summary>
		/// <param name="Line">The string to write</param>
		private void WriteFortranString(String Line)
		{
			while (Line.Length > 69)
			{
				String Section = Line.Substring(0, 69);
				m_Writer.WriteLine(Section);
				Line = "     . " + Line.Substring(69, Line.Length - 69);
			}
			m_Writer.WriteLine(Line);
		}
	}
}
