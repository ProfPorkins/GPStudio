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
	/// This is the base class from which all GP function set items are
	/// build.  It implements the Cloneable interface so that deep copies
	/// are made of nodes when a program tree is copied.
	/// </summary>
	public abstract class GPNode : ICloneable
	{
		/// <summary>
		/// A unique identifier for this node, used for finding a node during
		/// the crossover and mutation operations.
		/// </summary>
		public ushort Label
		{
			get { return m_Label; }
			set { m_Label = value; }
		}
		protected ushort m_Label = 0;

		/// <summary>
		/// These are the base "evaluate" methods that provide the implicit interpreter
		/// </summary>
		public abstract double EvaluateAsDouble(GPProgram tree, GPProgramBranch execBranch);

		/// <summary>
		/// All derived classes must implement the editing genetic operator
		/// </summary>
		public abstract GPNode Edit(GPProgram tree, GPProgramBranch execBranch);

		/// <summary>
		/// Cloneable interface implementation
		/// </summary>
		public abstract Object Clone();
	}
}
