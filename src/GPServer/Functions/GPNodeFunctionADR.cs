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

	public class GPNodeFunctionADR : GPNodeFunctionADRoot
	{
		public GPNodeFunctionADR(int WhichADR, byte NumberArgs)
			: base(WhichADR, NumberArgs)
		{
		}

		private const string STRING_ADR0 = "ADR0";
		private const string STRING_ADR1 = "ADR1";
		private const string STRING_ADR2 = "ADR2";
		private const string STRING_ADR3 = "ADR3";
		private const string STRING_ADR4 = "ADR4";
		public override string ToString()
		{
			//
			// Simple memory optimization to reduce the pressure on the memory allocation
			// from having to create new strings over and over.
			switch (this.WhichFunction)
			{
				case 0: return STRING_ADR0;
				case 1: return STRING_ADR1;
				case 2: return STRING_ADR2;
				case 3: return STRING_ADR3;
				case 4: return STRING_ADR4;
				default:
					return "ADR" + this.WhichFunction.ToString();

			}
		}

		/// <summary>
		/// An ADR node really acts as a reference to make a call into
		/// an ADR program branch.  What this method does is to first crawl
		/// through each of the ADR parameter subtrees and get their values
		/// computed up.  These values are stored internal to the function
		/// to act as a sort of stack, because each of the subtrees could
		/// make calls to the same ADR and we don't want the results to get
		/// overwritten.  Once all the subtrees have been evalutated, the
		/// results are written into the ADR parameters and the ADR program
		/// branch is called.
		/// </summary>
		/// <param name="tree">Program tree this ADR belongs to</param>
		/// <param name="execBranch">Program Branch this ADR is executing within</param>
		public override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch)
		{
			GPProgramBranchADR adr = tree.ADR[this.WhichFunction];
			base.PrepareFunctionParameters(tree, execBranch, adr);

			//
			// Evalute the Function program branch
			return adr.EvaluateAsDouble(tree);
		}
	}
}
