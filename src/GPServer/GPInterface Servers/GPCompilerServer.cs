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
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using GPStudio.Interfaces;

namespace GPStudio.Server
{
	public class GPCompilerServer : MarshalByRefObject, IGPCompiler
	{

		/// <summary>
		/// Writes a full GPNodeFunctionXXX class that can be compiled and used
		/// as a node in a program tree.
		/// </summary>
		/// <param name="Name">User Defined name of the function</param>
		/// <param name="Arity">Number of input parameters</param>
		/// <param name="UserCode">C# body of the function</param>
		/// <returns> C# class that can be compiled and used</returns>
		public String WriteUserFunction(string Name, short Arity, string UserCode)
		{
			StringBuilder UserFunction = new StringBuilder();

			//
			// Write the class header
			UserFunction.AppendLine("using System;");
			UserFunction.AppendLine("using System.Collections.Generic;");
			UserFunction.AppendLine("");
			UserFunction.AppendLine("namespace GPStudio.Server");
			UserFunction.AppendLine("{");
			UserFunction.AppendLine("\tpublic class GPNodeFunction" + Name + " : GPNodeFunction");
			UserFunction.AppendLine("\t{");
			UserFunction.AppendLine("");
			UserFunction.AppendLine("\t\tpublic GPNodeFunction" + Name + "() {}");
			UserFunction.AppendLine("");
			UserFunction.AppendLine("\t\tpublic override byte NumberArgs");
			UserFunction.AppendLine("\t\t{");
			UserFunction.AppendLine("\t\t\tget { return " + Arity + "; }");
			UserFunction.AppendLine("\t\t}");
			UserFunction.AppendLine("");

			//
			// Build the EvaluateAsDouble prototype
			UserFunction.AppendLine("\t\tpublic override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch)");
			UserFunction.AppendLine("\t\t{");
			//
			// Here is the trick, get each of the parameters evaluated!
			for (short Param = 0; Param < Arity; Param++)
			{
				//
				// Build the param evaluation string
				String EvalParameter = "\t\t\tdouble p" + Param + " = this.Children[" + Param + "].EvaluateAsDouble(tree, execBranch);";
				UserFunction.AppendLine(EvalParameter);
			}

			//
			// Create a local reference to the historical data
			UserFunction.AppendLine("\t\tList<List<double>> InputHistory=tree.InputHistory;");

			//
			// Place the user written code
			UserFunction.Append("\t\t");
			UserFunction.AppendLine(UserCode);
			UserFunction.AppendLine("\t\t}");

			//
			// Close off the class and then the namespace
			UserFunction.AppendLine("\t}");
			UserFunction.AppendLine("}");

			return UserFunction.ToString();
		}

		/// <summary>
		/// Writes a named GPFitnessCustom that implements the fitness code 
		/// passed in.  The class is derived from GPFitnessCustomBase, which contains
		/// a single abstract method that must be implemented...that does the
		/// fitness computation.
		/// </summary>
		/// <param name="SourceCode">C# body of the fitness function</param>
		/// <returns>C# class representing the custom fitness class</returns>
		public String WriteFitnessClass(String UserCode)
		{
			StringBuilder FitnessClass = new StringBuilder();

			//
			// Build the class header
			FitnessClass.AppendLine("using System;");
			FitnessClass.AppendLine("using System.Collections.Generic;");
			FitnessClass.AppendLine("");
			FitnessClass.AppendLine("namespace GPStudio.Server");
			FitnessClass.AppendLine("{");

			//
			// Write the class and a default constructor
			FitnessClass.AppendLine("\tpublic class GPFitnessCustom:GPFitnessCustomBase");
			FitnessClass.AppendLine("\t{");
			FitnessClass.AppendLine("\t\tpublic GPFitnessCustom() {}");

			//
			// Write the custom fitness function
			FitnessClass.AppendLine("\t\tpublic override double ComputeFitness(List<List<double>> InputHistory,double[] Predictions, double[] Training, double TrainingAverage,double Tolerance)");
			FitnessClass.AppendLine("\t\t{");
			FitnessClass.AppendLine("\t\t");

			FitnessClass.AppendLine(UserCode);

			//
			// Close the fitness function
			FitnessClass.AppendLine("\t\t}");

			//
			// Close the class
			FitnessClass.AppendLine("\t}");

			//
			// Close the namespace
			FitnessClass.AppendLine("}");

			return FitnessClass.ToString();
		}

		/// <summary>
		/// Used to determine if a UDF correctly compiles.  If the function doesn't
		/// successfully compile, the errors are returned through the Errors parameter.
		/// </summary>
		/// <param name="Name">Name of the function</param>
		/// <param name="FunctionCode">function code to test</param>
		/// <param name="Errors">Array of errors, if any</param>
		/// <returns>True/False upon success/failure</returns>
		public bool ValidateUserFunction(string Name, string FunctionCode, out string[] Errors)
		{
			GPNodeFunction func = CompileUserFunction(FunctionCode, Name, out Errors);

			if (func == null) return false;

			return true;
		}

		/// <summary>
		/// Used to determine if a fitness function will correctly compile.  If it
		/// doesn't, errors are returned through the Errors parameters.
		/// </summary>
		/// <param name="FitnessCode">Fitness class to test</param>
		/// <param name="Errors">Array of errors, if any</param>
		/// <returns>True/False upon success/failure</returns>
		public bool ValidateFitnessClass(String FitnessCode, out String[] Errors)
		{
			GPFitnessCustomBase func = CompileFitnessClass(FitnessCode, out Errors);

			if (func == null) return false;

			return true;
		}

		///
		/// <summary>
		/// Take the CSharp code, compile it to an assembly and then return an instance 
		/// of the class (Classname).  This method absolutely assumes it is compiling 
		/// a GPNodeFunction derived object.
		/// </summary>
		/// <param name="CSharpCode">The C# code to compile</param>
		/// <param name="FunctionName">Name of the user defined function</param>
		/// <param name="Errors">List of compilation errors</param>
		/// <returns>in-memory instance of the code ready for execution</returns>
		public GPNodeFunction CompileUserFunction(String CSharpCode, String FunctionName, out String[] Errors)
		{
			CSharpCodeProvider csProvider = new CSharpCodeProvider();
			CompilerParameters csParams = new CompilerParameters();

			csParams.GenerateExecutable = false;
			csParams.GenerateInMemory = true;
			csParams.TreatWarningsAsErrors = false;

			//
			// We derive from GPNodeFunction, have to reference the GPServer.exe assembly
			csParams.ReferencedAssemblies.Add(Application.StartupPath + "\\GPServer.exe");

			//
			// This does the actual compilation of the source code
			CompilerResults results = csProvider.CompileAssemblyFromSource(csParams, CSharpCode);

			//
			// See if there are any errors
			Errors = null;
			if (results.Errors.Count > 0)
			{
				//
				// Convert the errors into a string array that is send back
				Errors = new String[results.Errors.Count];
				for (int Item = 0; Item < results.Errors.Count; Item++)
				{
					Errors[Item] = (Item + 1).ToString() + ": " + results.Errors[Item].ErrorText;
				}
				return null;
			}

			//
			// Given the assembly, create an instance of the object from it
			GPNodeFunction codeInstance = (GPNodeFunction)results.CompiledAssembly.CreateInstance("GPStudio.Server.GPNodeFunction" + FunctionName);
			return codeInstance;
		}

		/// <summary>
		/// Take the CSharp code, compile it to an assembly and then return any
		/// errors that came up during compilation.
		/// </summary>
		/// <param name="CSharpCode">The C# code to compile</param>
		/// <param name="Errors">List of compilation errors</param>
		/// <returns>in-memory instance of the code ready for execution</returns>
		public GPFitnessCustomBase CompileFitnessClass(String CSharpCode, out String[] Errors)
		{
			CSharpCodeProvider csProvider = new CSharpCodeProvider();
			CompilerParameters csParams = new CompilerParameters();

			csParams.GenerateExecutable = false;
			csParams.GenerateInMemory = true;
			csParams.TreatWarningsAsErrors = false;

			//
			// We derive from GPFitnessCustomBase, so need to reference the assembly
			csParams.ReferencedAssemblies.Add(Application.StartupPath + "\\GPServer.exe");

			//
			// Compile
			CompilerResults results = csProvider.CompileAssemblyFromSource(csParams, CSharpCode);

			//
			// See if there are any errors
			Errors = null;
			if (results.Errors.Count > 0)
			{
				//
				// Convert the errors into a string array that is send back
				Errors = new String[results.Errors.Count];
				for (int Item = 0; Item < results.Errors.Count; Item++)
				{
					Errors[Item] = (Item + 1).ToString() + ": " + results.Errors[Item].ErrorText;
				}
				return null;
			}
			//
			// Given the assembly, create an instance of the object from it
			GPFitnessCustomBase codeInstance = (GPFitnessCustomBase)results.CompiledAssembly.CreateInstance("GPStudio.Server.GPFitnessCustom");

			return codeInstance;
		}
	}
}
