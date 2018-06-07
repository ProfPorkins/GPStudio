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
using GPStudio.Shared;

namespace GPStudio.Server
{
	public class GPNodeTerminalDouble : GPNodeTerminal, ICloneable
	{
		public GPNodeTerminalDouble(double Value)
		{
			m_Value = Value;
		}

		private double m_Value;
		public double Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		public const String VALUETYPE = "RandomDouble";
		public override String ToValueTypeString()
		{
			return GPNodeTerminalDouble.VALUETYPE;
		}

		//
		// Needed when converting the terminal for program writing
		public override String ToString()
		{
			return Convert.ToString(this.Value, GPUtilities.NumericFormat);
		}

		//
		// Cloneable interface
		public override Object Clone()
		{
			GPNodeTerminalDouble obj = (GPNodeTerminalDouble)this.MemberwiseClone();

			return obj;
		}

		//
		// Override from GPNode for implicit evaluation
		public override double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch)
		{
			return this.Value;
		}

	}
}
