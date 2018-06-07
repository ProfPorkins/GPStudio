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
using System.IO;

namespace GPStudio.Server
{
	/// <summary>
	/// This is a class that accepts a GPProgramTree and converts that program
	/// tree into a "program."  This is an abstract class that is designed to
	/// be derived from, where the derived class creates the actual program
	/// from that is needed.
	///
	/// This class implements the logic that controls how a program gets written,
	/// but leaves the details to the derived classes.
	/// </summary>
	public abstract class GPLanguageWriter
	{
		public GPLanguageWriter(GPProgram Program,bool TimeSeries)
		{
			m_Program = Program;
			m_TimeSeries = TimeSeries;
		}
		protected GPProgram m_Program;
		protected bool m_TimeSeries;

		public struct tagUserDefinedFunction
		{
			public String Name;
			public short Arity;
			public String UserCode;
		}

		/// <summary>
		/// This method manages the task of getting the program written.  It relies
		/// upon the derived class having implemented the abstract methods that
		/// provide the specifics for each language.
		/// </summary>
		/// <param name="InputStream"></param>
		/// <returns></returns>
		public bool Write(Stream InputStream)
		{
			//
			// Go through and write out the various program sections
			Initialize(InputStream);

			//
			// Header
			WriteHeader(m_Program);

			//
			// RPB
			WriteRPB(m_Program.RPB);

			//
			// ADFs
			for (int ADF = 0; ADF < m_Program.ADF.Count; ADF++)
			{
				WriteADF(m_Program.ADF[ADF]);
			}

			//
			// ADLs
			for (int ADL = 0; ADL < m_Program.ADL.Count; ADL++)
			{
				WriteADL(m_Program.ADL[ADL]);
			}

			//
			// ADRs
			for (int ADR = 0; ADR < m_Program.ADR.Count; ADR++)
			{
				WriteADR(m_Program.ADR[ADR]);
			}

			//
			// Trailer
			WriteTrailer(m_Program);

			//
			// Derived classes should NOT close the stream they created
			Terminate();

			//
			// Flush any outstanding commands, ensures the file gets correctly
			// written when it is closed off or rewound as a memory stream.
			InputStream.Flush();

			return false;
		}

		//
		// Abstract methods any derived class must implement
		public abstract void Initialize(Stream InputStream);
		public abstract void Terminate();

		public abstract bool WriteHeader(GPProgram Program);
		public abstract bool WriteRPB(GPProgramBranchRPB rpb);
		public abstract bool WriteADF(GPProgramBranchADF adf);
		public abstract bool WriteADL(GPProgramBranchADL adl);
		public abstract bool WriteADR(GPProgramBranchADR adr);
		public abstract bool WriteTrailer(GPProgram Program);

	}
}
