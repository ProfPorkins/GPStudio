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
using System.Runtime.CompilerServices;

//
// This is put here to allow GPServer access to the interal data members of
// this class.
[assembly: InternalsVisibleTo("GPServer")]

namespace GPStudio.Server
{
	/// <summary>
	/// Container for an Automatically Defined Recursion within the program tree.
	/// </summary>
	public class GPProgramBranchADR : GPProgramBranchADRoot
	{

		/// <summary>
		/// Constructor where the settings for creating the ADR are specified.
		/// </summary>
		/// <param name="parent">Reference to the program this ADR belongs to</param>
		/// <param name="InitialDepth">Max depth an new tree can be constructed</param>
		/// <param name="WhichFunction">Numeric indicator of "which" ADR this is in the program</param>
		/// <param name="NumberArgs">Count of arguments this ADR will accept</param>
		public GPProgramBranchADR(GPProgram parent, int InitialDepth, short WhichADR, byte NumberArgs)
			: base(parent, InitialDepth, WhichADR, NumberArgs)
		{
			Unwind = false;
			m_RecursionCount = 0;
		}

		/// <summary>
		/// Recursion Condition Branch
		/// </summary>
		public GPNode RCB
		{
			get { return m_RCB; }
			set { m_RCB = value; }
		}
		internal GPNode m_RCB;

		/// <summary>
		/// Recursion Body Branch
		/// </summary>
		public GPNode RBB
		{
			get { return m_RBB; }
			set { m_RBB = value; }
		}
		internal GPNode m_RBB;

		/// <summary>
		/// Recursion Update Branch
		/// </summary>
		public GPNode RUB
		{
			get { return m_RUB; }
			set { m_RUB = value; }
		}
		internal GPNode m_RUB;

		/// <summary>
		/// Recursion Ground Branch
		/// </summary>
		public GPNode RGB
		{
			get { return m_RGB; }
			set { m_RGB = value; }
		}
		internal GPNode m_RGB;
		
		/// <summary>
		/// Number of nodes in the RCB
		/// </summary>
		public ushort NodeCountRCB
		{
			get { return m_NodeCountRCB; }
		}
		private ushort m_NodeCountRCB;

		/// <summary>
		/// Number of nodes in the RBB
		/// </summary>
		public ushort NodeCountRBB
		{
			get { return m_NodeCountRBB; }
		}
		private ushort m_NodeCountRBB;

		/// <summary>
		/// Number of nodes in the RUB
		/// </summary>
		public ushort NodeCountRUB
		{
			get { return m_NodeCountRUB; }
		}
		private ushort m_NodeCountRUB;

		/// <summary>
		/// Number of nodes in the RGB
		/// </summary>
		public ushort NodeCountRGB
		{
			get { return m_NodeCountRGB; }
		}
		private ushort m_NodeCountRGB;

		private const string STRING_ADR0 = "ADR0";
		private const string STRING_ADR1 = "ADR1";
		private const string STRING_ADR2 = "ADR2";
		private const string STRING_ADR3 = "ADR3";
		private const string STRING_ADR4 = "ADR4";
		public override string ToString()
		{
			//
			// Simple memory optimization to reduce the pressure on the memory allocation
			// from having to create new strings over and over.
			switch (this.WhichFunction)
			{
				case 0: return STRING_ADR0;
				case 1: return STRING_ADR1;
				case 2: return STRING_ADR2;
				case 3: return STRING_ADR3;
				case 4: return STRING_ADR4;
				default:
					return "ADR" + this.WhichFunction.ToString();

			}
		}

		/// <summary>
		/// Makes a clone of the ADR
		/// </summary>
		public new Object Clone()
		{
			GPProgramBranchADR obj = (GPProgramBranchADR)base.Clone();

			//
			// Clone each of the branches
			obj.RCB = (GPNode)this.RCB.Clone();
			obj.RBB = (GPNode)this.RBB.Clone();
			obj.RUB = (GPNode)this.RUB.Clone();
			obj.RGB = (GPNode)this.RGB.Clone();

			return obj;
		}

		/// <summary>
		/// Works through the program tree to update the various statistics associated
		/// with it.
		/// </summary>
		public override void UpdateStats()
		{
			m_NodeCountRCB = DoCountNodes(this.RCB);
			m_NodeCountRBB = DoCountNodes(this.RBB);
			m_NodeCountRUB = DoCountNodes(this.RUB);
			m_NodeCountRGB = DoCountNodes(this.RGB);

			CountNodes = (ushort)(m_NodeCountRCB + m_NodeCountRBB + m_NodeCountRUB + m_NodeCountRGB);

			CountLeafNodes = DoCountLeafNodes(this.RCB);
			CountLeafNodes += DoCountLeafNodes(this.RBB);
			CountLeafNodes += DoCountLeafNodes(this.RUB);
			CountLeafNodes += DoCountLeafNodes(this.RGB);

			Depth = ComputeDepth(this.RCB);
			int TempDepth = ComputeDepth(this.RBB);
			if (TempDepth > Depth) Depth = TempDepth;
			TempDepth = ComputeDepth(this.RUB);
			if (TempDepth > Depth) Depth = TempDepth;
			TempDepth = ComputeDepth(this.RGB);
			if (TempDepth > Depth) Depth = TempDepth;
		}

		/// <summary>
		/// Execute the recursion
		/// </summary>
		public double EvaluateAsDouble(GPProgram tree)
		{
			if (m_RecursionCount >= MAXRECURSIONS || Unwind)
			{
				Unwind = true;
				m_RecursionCount--;

				double Result=m_RGB.EvaluateAsDouble(tree, this);

				if (m_RecursionCount == 0)
					Unwind = false;
				return Result;
			}

			m_RecursionCount++;
			if (m_RCB.EvaluateAsDouble(tree, this) > 0.0)
			{
				m_RBB.EvaluateAsDouble(tree, this);
				m_RUB.EvaluateAsDouble(tree, this);
			}

			m_RecursionCount--;

			return m_RGB.EvaluateAsDouble(tree,this);
		}

		private bool Unwind = false;
		private int m_RecursionCount = 0;
		private const int MAXRECURSIONS = 25;	// TODO: Think about parameterizing this

		/// <summary>
		/// Perform the genetic operation of editing on this program branch
		/// </summary>
		public override void Edit(GPProgram tree)
		{
			this.RCB.Edit(tree,this);
			this.RBB.Edit(tree,this);
			this.RUB.Edit(tree,this);
			this.RGB.Edit(tree, this);
		}
	}
}
