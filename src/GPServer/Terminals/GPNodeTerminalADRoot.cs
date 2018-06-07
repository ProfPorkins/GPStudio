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
	/// Base class from which all function type parameters are derived
	/// </summary>
	public abstract class GPNodeTerminalADRoot : GPNodeTerminal, ICloneable
	{

		public GPNodeTerminalADRoot(short WhichParameter)
		{
			m_WhichParameter = WhichParameter;
		}

		private short m_WhichParameter;
		/// <summary>
		/// Position of this parameter in the argument list
		/// </summary>
		public short WhichParameter
		{
			get { return m_WhichParameter; }
			set { WhichParameter = value; }
		}

		/// <summary>
		/// Needed when converting the terminal for program writing
		/// </summary>
		/// <returns>String representation of the terminal</returns>
		public override String ToString()
		{
			return "p" + Convert.ToString(this.WhichParameter);
		}

		/// <summary>
		/// ICloneable interface
		/// </summary>
		/// <returns>Clone of the object</returns>
		public override Object Clone()
		{
			GPNodeTerminalADRoot obj = (GPNodeTerminalADRoot)this.MemberwiseClone();

			return obj;
		}

		/// <summary>
		/// Override from GPNode for implicit evaluation
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="execBranch"></param>
		/// <returns>value of the terminal</returns>
		public abstract override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch);
	}
}
