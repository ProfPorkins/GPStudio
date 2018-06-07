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
	/// Container for an Automatically Defined Loop within the program tree.
	/// </summary>
	public class GPProgramBranchADL : GPProgramBranchADRoot
	{

		/// <summary>
		/// Constructor where the settings for creating the ADL are specified.
		/// </summary>
		/// <param name="parent">Reference to the program this ADL belongs to</param>
		/// <param name="InitialDepth">Max depth an new tree can be constructed</param>
		/// <param name="WhichFunction">Numeric indicator of "which" ADL this is in the program</param>
		/// <param name="NumberArgs">Count of arguments this ADL will accept</param>
		public GPProgramBranchADL(GPProgram parent, int InitialDepth, short WhichADL, byte NumberArgs)
			: base(parent, InitialDepth,WhichADL,NumberArgs)
		{
		}

		/// <summary>
		/// Loop Initialization Branch
		/// </summary>
		public GPNode LIB
		{
			get { return m_LIB; }
			set { m_LIB = value; }
		}
		internal GPNode m_LIB;

		/// <summary>
		/// Loop Condition Branch
		/// </summary>
		public GPNode LCB
		{
			get { return m_LCB; }
			set { m_LCB = value; }
		}
		internal GPNode m_LCB;

		/// <summary>
		/// Loop Body Branch
		/// </summary>
		public GPNode LBB
		{
			get { return m_LBB; }
			set { m_LBB = value; }
		}
		internal GPNode m_LBB;

		/// <summary>
		/// Loop Update Branch
		/// </summary>
		public GPNode LUB
		{
			get { return m_LUB; }
			set { m_LUB = value; }
		}
		internal GPNode m_LUB;

		/// <summary>
		/// Number of nodes in the LIB
		/// </summary>
		public ushort NodeCountLIB
		{
			get { return m_NodeCountLIB; }
		}
		private ushort m_NodeCountLIB;
		
		/// <summary>
		/// Number of nodes in the LCB
		/// </summary>
		public ushort NodeCountLCB
		{
			get { return m_NodeCountLCB; }
		}
		private ushort m_NodeCountLCB;

		/// <summary>
		/// Number of nodes in the LBB
		/// </summary>
		public ushort NodeCountLBB
		{
			get { return m_NodeCountLBB; }
		}
		private ushort m_NodeCountLBB;

		/// <summary>
		/// Number of nodes in the LUB
		/// </summary>
		public ushort NodeCountLUB
		{
			get { return m_NodeCountLUB; }
		}
		private ushort m_NodeCountLUB;

		private const string STRING_ADL0 = "ADL0";
		private const string STRING_ADL1 = "ADL1";
		private const string STRING_ADL2 = "ADL2";
		private const string STRING_ADL3 = "ADL3";
		private const string STRING_ADL4 = "ADL4";
		public override string ToString()
		{
			//
			// Simple memory optimization to reduce the pressure on the memory allocation
			// from having to create new strings over and over.
			switch (this.WhichFunction)
			{
				case 0: return STRING_ADL0;
				case 1: return STRING_ADL1;
				case 2: return STRING_ADL2;
				case 3: return STRING_ADL3;
				case 4: return STRING_ADL4;
				default:
					return "ADL" + this.WhichFunction.ToString();

			}
		}

		/// <summary>
		/// Makes a clone of the ADL
		/// </summary>
		public new Object Clone()
		{
			GPProgramBranchADL obj = (GPProgramBranchADL)base.Clone();

			//
			// Clone each of the branches
			obj.LIB = (GPNode)this.LIB.Clone();
			obj.LCB = (GPNode)this.LCB.Clone();
			obj.LBB = (GPNode)this.LBB.Clone();
			obj.LUB = (GPNode)this.LUB.Clone();

			return obj;
		}

		/// <summary>
		/// Works through the program tree to update the various statistics associated
		/// with it.
		/// </summary>
		public override void UpdateStats()
		{
			m_NodeCountLIB = DoCountNodes(this.LIB);
			m_NodeCountLCB = DoCountNodes(this.LCB);
			m_NodeCountLBB = DoCountNodes(this.LBB);
			m_NodeCountLUB = DoCountNodes(this.LUB);

			CountNodes = (ushort)(m_NodeCountLIB + m_NodeCountLCB + m_NodeCountLBB + m_NodeCountLUB);

			CountLeafNodes = DoCountLeafNodes(this.LIB);
			CountLeafNodes += DoCountLeafNodes(this.LCB);
			CountLeafNodes += DoCountLeafNodes(this.LBB);
			CountLeafNodes += DoCountLeafNodes(this.LUB);

			Depth = ComputeDepth(this.LIB);
			int TempDepth = ComputeDepth(this.LCB);
			if (TempDepth > Depth) Depth = TempDepth;
			TempDepth = ComputeDepth(this.LBB);
			if (TempDepth > Depth) Depth = TempDepth;
			TempDepth = ComputeDepth(this.LUB);
			if (TempDepth > Depth) Depth = TempDepth;
		}

		private const int MAXITERATIONS = 25;	// TODO: Think about parameterizing this
		/// <summary>
		//// Run this loop
		/// </summary>
		public double EvaluateAsDouble(GPProgram tree)
		{
			//
			// Loop initialization
			m_LIB.EvaluateAsDouble(tree, this);
			int LoopIndex = 0;

			//
			// Go into the loop
			double Result = 0.0;
			while ((LoopIndex < MAXITERATIONS) && (m_LCB.EvaluateAsDouble(tree, this) > 0.0))
			{
				Result=m_LBB.EvaluateAsDouble(tree, this);

				m_LUB.EvaluateAsDouble(tree,this);
				LoopIndex++;
			}

			return Result;
		}

		/// <summary>
		/// Perform the genetic operation of editing on this program branch
		/// </summary>
		public override void Edit(GPProgram tree)
		{
			this.LIB.Edit(tree,this);
			this.LCB.Edit(tree,this);
			this.LBB.Edit(tree,this);
			this.LUB.Edit(tree, this);
		}
	}
}
