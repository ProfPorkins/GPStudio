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
	/// An integer terminal whose initial value is randomly selected
	/// </summary>
	public class GPNodeTerminalInteger : GPNodeTerminal, ICloneable
	{
		public GPNodeTerminalInteger(int Value)
		{
			m_Value = Value;
		}

		/// <summary>
		/// Property that exposes the value (obviously) of the terminal
		/// </summary>
		public int Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}
		private int m_Value;

		/// <summary>
		/// Property that exposes the data type of the terminal
		/// </summary>
		/// <returns>String description of the data type</returns>
		public override String ToValueTypeString()
		{
			return GPNodeTerminalInteger.VALUETYPE;
		}
		public const String VALUETYPE = "RandomInteger";

		/// <summary>
		///  Needed when converting the terminal for program writing
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			return Convert.ToString(this.Value);
		}

		/// <summary>
		/// ICloneable interface implementation
		/// </summary>
		/// <returns></returns>
		public override Object Clone()
		{
			GPNodeTerminalInteger obj = (GPNodeTerminalInteger)this.MemberwiseClone();

			return obj;
		}

		/// <summary>
		/// Override from GPNode for implicit evaluation
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="execBranch"></param>
		/// <returns></returns>
		public override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch)
		{
			return (double)this.Value;
		}

	}
}
