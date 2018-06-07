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

namespace GPStudio.Server
{
	/// <summary>
	/// Provides the ability to set a value within the indexed memory.
	/// </summary>
	public class GPNodeFunctionSetMem : GPNodeFunction
	{
		/// <summary>
		/// By definition, 2 parameters.  The first is the location in memory, the second
		/// is the value to store.
		/// </summary>
		public override byte NumberArgs
		{
			get { return 2; }
		}
		
		/// <summary>
		/// Stores the value in memory.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="execBranch"></param>
		/// <returns>The value just stored in memory</returns>
		public override double EvaluateAsDouble(GPProgram tree,GPProgramBranch execBranch)
		{
			//
			// Set the memory value
			double p1=Math.Abs(this.Children[0].EvaluateAsDouble(tree,execBranch));
			double p2=this.Children[1].EvaluateAsDouble(tree,execBranch);
			
			GPProgram.m_Memory[Math.Abs(((int)p1) % tree.CountMemory)]=p2;
			
		return p2;
		}
	}
}
