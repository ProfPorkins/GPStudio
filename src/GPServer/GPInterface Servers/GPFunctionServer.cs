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
using System.Runtime.Remoting.Lifetime;
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This class provides an implementation of the IGPFunctionSet interface,
	/// allowing a client to manage compiled GPNodeFunction user defined functions.
	/// </summary>
	public class GPFunctionServer : MarshalByRefObject, IGPFunctionSet
	{
		public GPFunctionServer()
		{
			Clear();

			//
			// Get an interface to the compiler we'll eventually need
			m_Compiler = new GPCompilerServer();
		}

		/// <summary>
		/// Need a compiler interface so we can compile functions as they come in
		/// </summary>
		GPCompilerServer m_Compiler;

		/// <summary>
		/// Container that holds the compiled function nodes
		/// </summary>
		private SortedDictionary<String, GPNodeFunction> m_FunctionSet;
		private List<String> m_FunctionSetKeys;

		/// <summary>
		/// Reset the current function set
		/// </summary>
		public void Clear()
		{
			m_FunctionSet = new SortedDictionary<string, GPNodeFunction>();
			m_FunctionSetKeys = new List<string>();

			//
			// Indicate that no time series functions are currently defined
			m_UseInputHistory = false;
		}

		/// <summary>
		/// Add a new function to the set.  This accepts the C# code of the function,
		/// gets it compiled and added to the set.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Arity"></param>
		/// <param name="UserCode"></param>
		/// <returns></returns>
		public bool AddFunction(String Name, short Arity, bool TerminalParameters, String UserCode)
		{
			//
			// Make sure we don't already have the function
			if (m_FunctionSet.ContainsKey(Name.ToUpper()))
			{
				//
				// Just return true, it's not really an error, but we don't need
				// a duplicate either.
				return true;
			}

			//
			// Write the function
			String FunctionCode = m_Compiler.WriteUserFunction(Name, Arity, UserCode);
			//
			// Get it compiled
			String[] Errors=null;
			GPNodeFunction node = m_Compiler.CompileUserFunction(FunctionCode, Name, out Errors);
			node.TerminalParameters = TerminalParameters;
			if (node == null)
			{
				return false;
			}

			//
			// Add it to our set!
			m_FunctionSet.Add(Name.ToUpper(), node);
			//
			// We also keep a collection of the keys in a List<> collection
			// so we can have an 'int' indexer for the function set
			m_FunctionSetKeys.Add(Name.ToUpper());

			//
			// Check to see if the code uses "InputHistory", if it does, indicate
			// this function set requires the input history of data to be built.
			if (UserCode.Contains("InputHistory"))
			{
				m_UseInputHistory = true;
			}

			return true;
		}

		/// <summary>
		/// Adds the built-in set/get memory functions
		/// </summary>
		public bool UseMemory
		{
			set
			{
				if (value == true)
				{
					m_FunctionSet.Add(SETMEM, new GPNodeFunctionSetMem());
					m_FunctionSet.Add(GETMEM, new GPNodeFunctionGetMem());

					m_FunctionSetKeys.Add(SETMEM);
					m_FunctionSetKeys.Add(GETMEM);
				}
			}
		}

		private const string SETMEM = "SETMEM";
		private const string GETMEM = "GETMEM";

		/// <summary>
		/// Property that indicates whether or not time series functions
		/// are in use.  This property can only be set to true by the AddFunction
		/// method, it scans the code to see if "InputHistory" is used.
		/// </summary>
		public bool UseInputHistory
		{
			get { return m_UseInputHistory; }
		}
		private bool m_UseInputHistory;

		/// <summary>
		/// Indexer that returns a reference to the compiled GPNodeFuncton
		/// </summary>
		/// <param name="Function"></param>
		/// <returns></returns>
		public Object this[string Function]
		{
			get { return m_FunctionSet[Function]; }
		}

		/// <summary>
		/// Array style Indexer that returns a reference to the compiled GPNodeFunction
		/// </summary>
		/// <param name="Function"></param>
		/// <returns></returns>
		public Object this[int Function]
		{
			get 
			{ 
				return m_FunctionSet[m_FunctionSetKeys[Function]];
			}
		}

		/// <summary>
		/// Count of current functions being managed
		/// </summary>
		public int Count
		{
			get { return m_FunctionSet.Count; }
		}

		/// <summary>
		/// Returns the linear index of the function from the list of function keys
		/// </summary>
		/// <param name="Key">String name of the function</param>
		/// <returns>Index of the key</returns>
		public int IndexOfKey(string Key)
		{
			return m_FunctionSetKeys.IndexOf(Key);
		}

		#region Distributed Computing

		/// <summary>
		/// Prepare the lease sponsorship settings
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			ILease LeaseInfo = (ILease)base.InitializeLifetimeService();

			LeaseInfo.InitialLeaseTime = TimeSpan.FromMinutes(GPEnums.REMOTING_RENEWAL_MINUTES);
			LeaseInfo.RenewOnCallTime = TimeSpan.FromMinutes(GPEnums.REMOTING_RENEWAL_MINUTES);
			LeaseInfo.SponsorshipTimeout = TimeSpan.FromMinutes(GPEnums.REMOTING_TIMEOUT_MINUTES);

			return LeaseInfo;
		}

		#endregion
	}
}
