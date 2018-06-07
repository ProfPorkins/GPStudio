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
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// An abstract base class that defines how to generate an initial
	/// Population of genetic programs.
	/// </summary>
	public abstract class GPGeneratePopulation
	{
		/// <summary>
		/// Constructor: Create the program factory that will be used by derived classes
		/// to create the individual programs.
		/// </summary>
		/// <param name="Config"></param>
		/// <param name="InputDimension"></param>
		public GPGeneratePopulation(GPModelerServer Config, short InputDimension)
		{
			m_Config = Config;
			//
			// Create a program tree factor that will be used for creating new
			// program trees.
			m_TreeFactory = new GPProgramTreeFactory(Config,InputDimension);
		}
		protected GPModelerServer m_Config;
		protected GPProgramTreeFactory m_TreeFactory;

		/// <summary>
		/// Create an initial Population of programs
		/// </summary>
		/// <param name="Size"></param>
		/// <returns></returns>
		public abstract List<GPProgram> Generate(long Size);
	}
}
