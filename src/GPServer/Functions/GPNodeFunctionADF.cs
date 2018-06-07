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
using System.Collections;
using System.Text;

namespace GPStudio.Server
{

	public class GPNodeFunctionADF : GPNodeFunctionADRoot
	{
		public GPNodeFunctionADF(int WhichADF,byte NumberArgs):base(WhichADF,NumberArgs)
		{
		}

		private const string STRING_ADF0 = "ADF0";
		private const string STRING_ADF1 = "ADF1";
		private const string STRING_ADF2 = "ADF2";
		private const string STRING_ADF3 = "ADF3";
		private const string STRING_ADF4 = "ADF4";
		public override string ToString()
		{
			//
			// Simple memory optimization to reduce the pressure on the memory allocation
			// from having to create new strings over and over.
			switch (this.WhichFunction)
			{
				case 0: return STRING_ADF0;
				case 1: return STRING_ADF1;
				case 2: return STRING_ADF2;
				case 3: return STRING_ADF3;
				case 4: return STRING_ADF4;
				default:
					return "ADF" + this.WhichFunction.ToString();

			}
		}

		/// <summary>
		/// A Function node really acts as a reference to make a call into
		/// a Function program branch.  What this method does is to first crawl
		/// through each of the Function parameter subtrees and get their values
		/// computed up.  These values are stored internal to the function
		/// to act as a sort of stack, because each of the subtrees could
		/// make calls to the same Function and we don't want the results to get
		/// overwritten.  Once all the subtrees have been evalutated, the
		/// results are written into the Function parameters and the Function program
		/// branch is called.
		/// </summary>
		/// <param name="tree">Program tree this Function belongs to</param>
		/// <param name="execBranch">Program Branch this Function is executing within</param>
		public override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch)
		{
			GPProgramBranchADF adf = tree.ADF[this.WhichFunction];

			base.PrepareFunctionParameters(tree, execBranch,adf);

			//
			// Evalute the Function program branch
			return adf.EvaluateAsDouble(tree);
		}
	}
}
