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
using GPStudio.Shared;

namespace GPStudio.Server
{
	public abstract class GPProgramBranchFactory
	{
		public GPProgramBranchFactory(GPProgramBranch Branch, GPModelerServer Config)
		{
			m_Branch=Branch;
			m_Config = Config;
		}
		protected GPProgramBranch m_Branch;
		protected GPModelerServer m_Config;

		//
		// All derived classes must implement the following methods
		public abstract bool Build(GPProgramBranch Branch,GPEnums.TreeBuild TreeBuild);
		public abstract void Mutate();
		public abstract void Crossover(GPProgramBranch sibling);

		/// <summary>
		/// Utilities to locate a node in a program branch
		/// </summary>
		protected class FindResult
		{
			public GPNodeFunction Parent = null;
			public int ChildNumber = 0;
			public GPNode Node = null;
		}

		/// <summary>
		/// Unfortunately, this is essentially a linear search, I should really label each
		/// node so we have a BST tree to speed up the search.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="node"></param>
		/// <param name="nFindLabel"></param>
		/// <returns></returns>
		protected FindResult FindNode(GPNode parent, GPNode node, int nFindLabel)
		{
			//
			// Termination Criteria: If FindLabel and the node label match
			if (node.Label == nFindLabel)
			{
				FindResult find = new FindResult();

				find.Parent = (GPNodeFunction)parent;
				find.Node = node;
				//
				// Figure out which child of the parent this is
				if (parent != null)
				{
					GPNodeFunction nodeFunc = (GPNodeFunction)parent;
					for (int nChild = 0; nChild < nodeFunc.Children.Count; nChild++)
					{
						if (node == nodeFunc.Children[nChild])
						{
							find.ChildNumber = nChild;
						}
					}
				}
				return find;
			}

			//
			// Search the children
			if (node is GPNodeFunction)
			{
				GPNodeFunction nodeFunc = (GPNodeFunction)node;
				for (int nChild = 0; nChild < nodeFunc.Children.Count; nChild++)
				{
					FindResult find = FindNode(nodeFunc, nodeFunc.Children[nChild], nFindLabel);
					if (find != null)
					{
						return find;
					}

				}
			}

			return null;
		}

		/// <summary>
		/// A simple factory function that creates a terminal node and gets
		/// it's value initialized.
		/// </summary>
		/// <returns>new terminal node</returns>
		protected GPNode CreateNodeTerminal()
		{
			GPNodeTerminal node=GPNodeTerminal.Initialize(
				false, (short)0,
				false, (short)0,
				false, (short)0,
				m_Config.TerminalSet,m_Config.Profile.IntegerMin,m_Config.Profile.IntegerMax);

			return node;
		}

		/// <summary>
		/// This is a simple factory function that selects from the available
		/// list of functions and creates a class of that type.
		/// </summary>
		/// <param name="UseADF">Select from ADFs</param>
		/// <param name="StartADF">Which ADF to start choosing from</param>
		/// <param name="UseADL">Select from ADLs</param>
		/// <param name="StartADL">Which ADL to start choosing from</param>
		/// <param name="UseADR">Select from ADRs</param>
		/// <param name="StartADR">Which ADR to start choosing from</param>
		protected GPNode CreateNodeFunction(bool UseADF, int StartADF, bool UseADL, int StartADL,bool UseADR,int StartADR)
		{
			//
			// Add up all the different function types that we can choose from
			//		*"Regular" function types sent over from the client
			//		*ADFs
			//		*ADLs
			//		*ADRs
			int Count = m_Config.FunctionSet.Count;
			if (UseADF)
			{
				Count += m_Branch.Parent.ADF.Count - StartADF;
			}
			if (UseADL)
			{
				Count += m_Branch.Parent.ADL.Count - StartADL;
			}
			if (UseADR)
			{
				Count += m_Branch.Parent.ADR.Count - StartADR;
			}

			//
			// Randomly select from the function node choices.
			int Function = GPUtilities.rngNextInt(Count);

			//
			// See if we chose a "regular" function
			if (Function < m_Config.FunctionSet.Count)
			{
				GPNodeFunction Type = (GPNodeFunction)m_Config.FunctionSet[Function];
				return (GPNodeFunction)Type.Clone();
			}

			//
			// See if we chose an ADF
			Function -= m_Config.FunctionSet.Count;
			if (Function < (m_Branch.Parent.ADF.Count - StartADF))
			{
				int WhichADF = StartADF + Function;
				byte NumberArgs = m_Branch.Parent.ADF[WhichADF].NumberArgs;
				GPNodeFunctionADF adfFunc = new GPNodeFunctionADF(WhichADF, NumberArgs);

				return adfFunc;
			}

			//
			// See if we chose an ADL
			Function -= m_Config.ADFSet.Count;
			if (Function < (m_Branch.Parent.ADL.Count - StartADL))
			{
				int WhichADL = StartADL + Function;
				byte NumberArgs = m_Branch.Parent.ADL[WhichADL].NumberArgs;
				GPNodeFunctionADL adlFunc = new GPNodeFunctionADL(WhichADL, NumberArgs);

				return adlFunc;
			}

			//
			// See if we chose an ADR
			Function -= m_Config.ADLSet.Count;
			if (Function < (m_Branch.Parent.ADR.Count - StartADR))
			{
				int WhichADR = StartADR + Function;
				byte NumberArgs = m_Branch.Parent.ADR[WhichADR].NumberArgs;
				GPNodeFunctionADR adrFunc = new GPNodeFunctionADR(WhichADR, NumberArgs);

				return adrFunc;
			}

			return null;
		}
	}
}
