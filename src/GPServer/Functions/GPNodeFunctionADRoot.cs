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
using System.IO;

namespace GPStudio.Server
{
	/// <summary>
	/// All "Automatically Defined" branches are derived from this class.  It
	/// provides some simple common features that help define what these evolved
	/// branches are like.
	/// </summary>
	public abstract class GPNodeFunctionADRoot : GPNodeFunction, ICloneable
	{
		/// <summary>
		/// Constructor that defines the structure of the function branch
		/// </summary>
		/// <param name="WhichADF"></param>
		/// <param name="NumberArgs"></param>
		public GPNodeFunctionADRoot(int WhichADF, byte NumberArgs)
		{
			m_WhichFunction = WhichADF;
			m_NumberArgs = NumberArgs;
		}

		/// <summary>
		/// Simple number that identifies this branch from the others, i.e. ADF0, ADF1,...
		/// TODO: Could change this to a byte to save some memory
		/// </summary>
		public int WhichFunction
		{
			get { return m_WhichFunction; }
		}
		private int m_WhichFunction;

		/// <summary>
		/// Number of parameters to this function branch
		/// </summary>
		public override byte NumberArgs
		{
			get { return m_NumberArgs; }
		}
		private byte m_NumberArgs;

		/// <summary>
		/// Evaluates the parameters to a function before the function is called.
		/// </summary>
		/// <param name="tree">Program tree this Function belongs to</param>
		/// <param name="execBranch">Program Branch this Function is executing within</param>
		public void PrepareFunctionParameters(GPProgram tree, GPProgramBranch execBranch,GPProgramBranchADRoot ADFunction)
		{
			//
			// Compute the parameters to the Function
			short nNumberArgs = ADFunction.NumberArgs;
			double[] argResult = new double[nNumberArgs];
			for (int nParam = 0; nParam < nNumberArgs; nParam++)
			{
				argResult[nParam] = ((GPNode)m_Children[nParam]).EvaluateAsDouble(tree, execBranch);
			}
			//
			// Place the results into the Function program branch
			for (int nParam = 0; nParam < nNumberArgs; nParam++)
			{
				ADFunction.ParamResults[nParam] = argResult[nParam];
			}
		}

		/// <summary>
		/// ICloneable interface
		/// </summary>
		/// <returns>Clone of the object</returns>
		public override Object Clone()
		{
			GPNodeFunctionADRoot obj = (GPNodeFunctionADRoot)base.Clone();

			obj.m_WhichFunction = m_WhichFunction;
			obj.m_NumberArgs = m_NumberArgs;

			return obj;
		}
	}
}
