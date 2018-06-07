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

namespace GPStudio.Server
{
	/// <summary>
	/// Container for an Automatically Defined Function within the program tree.
	/// </summary>
	public class GPProgramBranchADF : GPProgramBranchADRoot
	{
		/// <summary>
		/// Constructor where the settings for creating the ADF are specified.
		/// </summary>
		/// <param name="parent">Reference to the program this ADF belongs to</param>
		/// <param name="InitialDepth">Max depth an new tree can be constructed</param>
		/// <param name="WhichFunction">Numeric indicator of "which" ADF this is in the program</param>
		/// <param name="NumberArgs">Count of arguments this ADF will accept</param>
		public GPProgramBranchADF(GPProgram parent, int InitialDepth, short WhichADF, byte NumberArgs)
			: base(parent, InitialDepth,WhichADF,NumberArgs)
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

		private int m_CallCount;
		/// <summary>
		//// Compute the result for this ADF
		/// </summary>
		public double EvaluateAsDouble(GPProgram tree)
		{
			//
			// Only allow limited calling depth, to help speed up fitness
			// evaluation
			if (m_CallCount >= 1)	return 1.0;
			m_CallCount++;
			
			double fResult=m_Root.EvaluateAsDouble(tree,this);
			
			m_CallCount--;
			
		return fResult;
		}
	}

}
