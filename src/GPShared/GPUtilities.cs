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

namespace GPStudio.Shared
{
	/////////////////////////////////////////////////////////////////////
	//
	// This class contains various misc utilities that are used in support
	// of the GP development system.
	//

	public class GPUtilities 
	{
		private static Random m_RNG=new Random();

		//
		// Public Constructor
		public GPUtilities()
		{
		}
		
		public static double rngNextDouble()		{ return m_RNG.NextDouble(); }
		public static int rngNextInt()				{ return m_RNG.Next(); }
		public static int rngNextInt(int nMax)		{ return m_RNG.Next(nMax); }
		public static int rngNextInt(int nMin, int nMax) { return m_RNG.Next(nMin, nMax); }

		/// <summary>
		/// Forces the software to use US style formatting when writing numbers.  The reason for this
		/// is that the program expects "." to be used and not ","...commas are used to separate parameters
		/// in SQL statments and can wreak havoc when dynamically construction SQL.  I UNDERSTAND this is
		/// a little nationalistic, but it is really done because I have limited time to provide comprehensive
		/// localization of the application.
		/// </summary>
		public static System.Globalization.CultureInfo NumericFormat
		{
			get { return m_NumericFormat; }
		}
		private static System.Globalization.CultureInfo m_NumericFormat = new System.Globalization.CultureInfo("en-US", false);
	}
}
