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

namespace GPStudio.Server
{
	/// <summary>
	/// This class defines an operation of some kind within the genetic
	/// program.  For example, mathematical expressions or other hard-
	/// coded or user defined functions.
	/// </summary>
	public abstract class GPNodeFunction : GPNode, ICloneable
	{
		/// <summary>
		/// Default constructor, get the string name created so it doesn't have
		/// to be remade over and over.
		/// </summary>
		public GPNodeFunction()
		{
			m_Children=new List<GPNode>();

			//
			// Memory optimization technique...create the name of the node
			// once and reuse it over and over.  Save a lot on having to
			// create strings.  Could be further optimized through the use of
			// some kind of singleton object that keeps the names around for all
			// instances of the same function name.
			m_NodeName = base.ToString().Substring(
				FunctionRoot.Length,
				base.ToString().Length - FunctionRoot.Length);
			m_NodeNameUpper = m_NodeName.ToUpper();
		}
		private const String FunctionRoot = "GPStudio.Shared.GPNodeFunction";

		#region Public Properties

		/// <summary>
		/// Collection that contains the children to this function; each child
		/// is a parameter to the function.
		/// </summary>
		public List<GPNode> Children
		{
			get { return m_Children; }
		}
		protected List<GPNode> m_Children;

		/// <summary>
		/// Specifies whether or not the parameters to this function must be terminal inputs only.
		/// True: If only terminal inputs
		/// False: If they can be anything
		/// </summary>
		public bool TerminalParameters
		{
			get { return m_TerminalParameters; }
			set { m_TerminalParameters = value; }
		}
		protected bool m_TerminalParameters = false;

		#endregion

		///
		/// <summary>
		/// override ToString() to return the root (user defined) part of the class name.
		/// TODO: After doing some profiling I've found this is called a LOT, I need to find
		/// some way to reduce the pressure on the memory allocation system by reducing
		/// how often this string is created.  Could create a singleton object that
		/// maintains a list of of already created names and reuse those strings each
		/// time this method is called.
		/// </summary>
		public override string ToString()
		{
			return m_NodeName;
		}
		protected String m_NodeName;

		/// <summary>
		/// Returns an uppercase version of the string.
		/// TODO: Refer to the note in ToString() for a memory optimization thought
		/// </summary>
		/// <returns></returns>
		public string ToStringUpper()
		{
			return m_NodeNameUpper;
		}
		protected String m_NodeNameUpper;
				
		/// <summary>
		/// ICloneable interface
		/// </summary>
		/// <returns>Clone of the object</returns>
		public override Object Clone()
		{
			GPNodeFunction obj = (GPNodeFunction)this.MemberwiseClone();

			obj.m_TerminalParameters = this.TerminalParameters;
			obj.m_Children=new List<GPNode>(); 
			//
			// Go through the children and start copying them
			foreach (GPNode child in m_Children)
			{
				obj.m_Children.Add((GPNode)child.Clone());
			}
			
		return obj;
		}

		/// <summary>
		/// Report the number of arguments from function node
		/// </summary>
		public abstract byte NumberArgs { get; }

		/// <summary>
		/// Recursively work through the nodes in the tree, checking the children
		/// first and then checking the current node.  If a node is able to be combined,
		/// then a new terminal node is created and it is returned, instead of the
		/// original subtree.
		/// </summary>
		public override GPNode Edit(GPProgram tree, GPProgramBranch execBranch)
		{
			//
			// Can not use a 'foreach' structure here because we need to replace
			// the child with whatever it returns of itself.
			for (int Child = 0; Child < m_Children.Count; Child++)
			{
				m_Children[Child] = m_Children[Child].Edit(tree, execBranch);
			}

			//
			// If all the children to this node are numbers, a simplification
			// can be performed.
			bool AllNumbers = true;
			foreach (GPNode child in m_Children)
			{
				if (!(child is GPNodeTerminalDouble || child is GPNodeTerminalInteger))
				{
					AllNumbers=false;
					break;
				}
			}
			//
			// The test to see if there are more than 0 arguments was added because
			// the time series functions may not have any parameters and they shouldn't
			// be converted to numeric terminals.
			if (AllNumbers && this.NumberArgs > 0)
			{
				//
				// Exec the function and replace with a new terminal node
				double result=this.EvaluateAsDouble(tree, execBranch);
				//
				// Create a new terminal node
				GPNodeTerminalDouble NodeNew = new GPNodeTerminalDouble(result);

				return NodeNew;
			}
			return this;
		}
	}
}
