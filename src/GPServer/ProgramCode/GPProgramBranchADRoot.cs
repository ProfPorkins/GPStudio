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

namespace GPStudio.Server
{
	/// <summary>
	/// Base class from which all ADF, ADL and ADR branches are derived.  Turns out
	/// there are a lot of common properties and functions in these.
	/// </summary>
	public abstract class GPProgramBranchADRoot : GPProgramBranch, ICloneable
	{
		/// <summary>
		/// Numeric value indicating "which" Function this is in the program
		/// </summary>
		private short m_WhichFunction;
		public short WhichFunction
		{
			get { return m_WhichFunction; }
			set { m_WhichFunction = value; }
		}

		/// <summary>
		/// Number of arguments this Function accepts
		/// </summary>
		private byte m_NumberArgs;
		public byte NumberArgs
		{
			get { return m_NumberArgs; }
			set { m_NumberArgs = value; }
		}

		/// <summary>
		/// The parameters to the Function are computed before the Function is called.  This is
		/// where those parameters are stored.
		/// </summary>
		private double[] m_ParamResults;
		public double[] ParamResults
		{
			get { return m_ParamResults; }
			set { m_ParamResults = value; }
		}

		/// <summary>
		/// Constructor where the settings for creating the Function are specified.
		/// </summary>
		/// <param name="parent">Reference to the program this Function belongs to</param>
		/// <param name="InitialDepth">Max depth an new tree can be constructed</param>
		/// <param name="WhichFunction">Numeric indicator of "which" Function this is in the program</param>
		/// <param name="NumberArgs">Count of arguments this Function will accept</param>
		public GPProgramBranchADRoot(GPProgram parent, int InitialDepth, short WhichFunction, byte NumberArgs)
			: base(parent, InitialDepth)
		{
			this.WhichFunction=WhichFunction;
			this.NumberArgs=NumberArgs;
			m_ParamResults=new double[NumberArgs];
		}

		/// <summary>
		/// Makes a clone of the branch
		/// </summary>
		public new Object Clone()
		{
			GPProgramBranchADRoot obj = (GPProgramBranchADRoot)base.Clone();

			//
			// TODO: I don't believe these first two are be necessary, the base.Clone()
			// should take care of them for me...because it has a .MemberwiseClone() call.
			obj.m_WhichFunction = m_WhichFunction;
			obj.m_NumberArgs = m_NumberArgs;
			obj.m_ParamResults = new double[m_NumberArgs];

			return obj;
		}
	}
}
