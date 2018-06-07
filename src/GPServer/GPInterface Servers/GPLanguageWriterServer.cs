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
using System.IO;
using GPStudio.Interfaces;

namespace GPStudio.Server
{
	/// <summary>
	/// Provides an implementation of the IGPLanguageWriter interface that
	/// provides services to a client to write programs in various languages.
	/// </summary>
	public class GPLanguageWriterServer : MarshalByRefObject, IGPLanguageWriter
	{
		public GPLanguageWriterServer()
		{
			Clear();
		}

		/// <summary>
		/// Create a new collection to store the user defined code
		/// </summary>
		public void Clear()
		{
			m_FunctionSetCode = new SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction>();
		}

		/// <summary>
		/// Collection that holds the source code forms of the user defined functions
		/// </summary>
		private SortedDictionary<String, GPLanguageWriter.tagUserDefinedFunction> m_FunctionSetCode;

		/// <summary>
		/// Add the source code form of a language specific function to the object
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Arity"></param>
		/// <param name="UserCode"></param>
		/// <returns></returns>
		public bool AddFunction(String Name, short Arity, String UserCode)
		{
			GPLanguageWriter.tagUserDefinedFunction func = new GPLanguageWriter.tagUserDefinedFunction();
			func.Name = Name;
			func.Arity = Arity;
			func.UserCode = UserCode;
			m_FunctionSetCode.Add(Name.ToUpper(), func);

			return true;
		}

		/// <summary>
		/// Write a program in C
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteC(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterC(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in C++
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteCPP(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterCPP(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in C#
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteCSharp(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterCSharp(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in Java
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteJava(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterJava(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in VB.NET
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteVBNet(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterVBNet(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in Fortran
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteFortran(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterFortran(((GPProgramServer)Program).Program, TimeSeries, m_FunctionSetCode));
		}

		/// <summary>
		/// Write a program in XML
		/// </summary>
		/// <param name="Program"></param>
		/// <param name="TimeSeries"></param>
		/// <returns></returns>
		public String WriteXML(IGPProgram Program, bool TimeSeries)
		{
			return Write(new GPLanguageWriterXML(((GPProgramServer)Program).Program, TimeSeries));
		}

		/// <summary>
		/// Private member that accepts a language writer and uses it to
		/// get the source code written to a string.
		/// </summary>
		/// <param name="lw"></param>
		/// <returns></returns>
		private String Write(GPLanguageWriter lw)
		{
			//
			// Create a memory stream to write to, and then, take that
			// memory stream, read it and return it as a string
			using (MemoryStream ms = new MemoryStream())
			{
				lw.Write(ms);
				ms.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(ms))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
