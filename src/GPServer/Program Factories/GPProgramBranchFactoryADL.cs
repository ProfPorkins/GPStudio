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
	public class GPProgramBranchFactoryADL : GPProgramBranchFactory
	{
		public GPProgramBranchFactoryADL(GPProgramBranchADL ADL, GPModelerServer Config)
			: base(ADL,Config)
		{
			m_BranchADL = ADL;
		}
		GPProgramBranchADL m_BranchADL;

		/// <summary>
		/// Construct the four branching structures of an ADL
		/// </summary>
		/// <param name="Branch">Reference to the ADL branch</param>
		/// <param name="TreeBuild">Tree building technqiue</param>
		/// <returns>True if the branch was correctly constructed</returns>
		public override bool Build(GPProgramBranch Branch, GPEnums.TreeBuild TreeBuild)
		{
			m_Branch = m_BranchADL = (GPProgramBranchADL)Branch;
			
			//
			// We create 4 different branching structures: LIB, LCB, LBB, LUB
			m_BranchADL.LIB = BuildInternal(TreeBuild, m_Branch.DepthInitial, false);
			m_BranchADL.LCB = BuildInternal(TreeBuild, m_Branch.DepthInitial, false);
			m_BranchADL.LBB = BuildInternal(TreeBuild, m_Branch.DepthInitial, false);
			m_BranchADL.LUB = BuildInternal(TreeBuild, m_Branch.DepthInitial, false);

			//
			// Update the tree stats
			m_Branch.UpdateStats();

			//
			// Convert the program into array representation
			m_Branch.ConvertToArray(m_Config.FunctionSet);

			return true;
		}

		/// <summary>
		/// Custom build function that creates the Automatically Defined Loop branch
		/// of the genetic program.
		/// </summary>
		/// <param name="TreeBuild">Tree building technique</param>
		/// <param name="MaxDepth">Maximum depth to allow the tree to grow</param>
		private GPNode BuildInternal(GPEnums.TreeBuild TreeBuild, int MaxDepth, bool TerminalParameters)
		{
			//
			// Randomly create this node
			GPNode newNode = CreateNode(TreeBuild, MaxDepth,TerminalParameters);
			//
			// If we just created a function, build its children
			if (newNode is GPNodeFunction)
			{
				GPNodeFunction node = (GPNodeFunction)newNode;
				//
				// Need to generate the appropriate number of nodes for the operation
				for (int Child = 0; Child < node.NumberArgs; Child++)
				{
					node.Children.Add(BuildInternal(TreeBuild, MaxDepth - 1,node.TerminalParameters));
				}
			}

			return newNode;
		}

		/// <summary>
		/// Handles mutation of the ADL branch.  One of the internal loop branches
		/// is randomly chosen and then a node in that branch is randomly selected
		/// for mutation and a new branch is built from that location.
		/// </summary>
		public override void Mutate()
		{
			//
			// Select a branch
			int WhichBranch = GPUtilities.rngNextInt(4);
			switch (WhichBranch)
			{
				case 0: 
					Mutate(ref m_BranchADL.m_LIB, m_BranchADL.NodeCountLIB, m_BranchADL.DepthInitial);
					break;
				case 1:
					Mutate(ref m_BranchADL.m_LCB, m_BranchADL.NodeCountLCB, m_BranchADL.DepthInitial);
					break;
				case 2:
					Mutate(ref m_BranchADL.m_LBB, m_BranchADL.NodeCountLBB, m_BranchADL.DepthInitial);
					break;
				case 3:
					Mutate(ref m_BranchADL.m_LUB, m_BranchADL.NodeCountLUB, m_BranchADL.DepthInitial);
					break;
			}

			m_Branch.UpdateStats();
		}

		/// <summary>
		/// Internal method to get the mutation done.  This method returns a reference
		/// to the root node of the branch.  If the first node is selected for mutation,
		/// that node will change, therefore, it needs to be updated in the calling method.
		/// </summary>
		/// <param name="BranchRoot">Root node of the loop branch we are mutating</param>
		/// <param name="CountNodes">Number of nodes in the branch</param>
		/// <param name="DepthInitial">Initial depth the branch was created with</param>
		private void Mutate(ref GPNode BranchRoot,int NodeCount,int DepthInitial)
		{
			//
			// Step 1: Select a node randomly
			int NodeLabel = GPUtilities.rngNextInt(NodeCount);
			//
			// Step 2: Find this node, its parent and which child of the parent it is
			GPProgramBranchFactory.FindResult find = FindNode(null, BranchRoot, NodeLabel);
			//
			// Step 3: Build a new subtree that will replace the selected node
			GPNode newSubtree = null;
			if (find.Node is GPNodeFunction)
			{
				newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, DepthInitial, ((GPNodeFunction)find.Node).TerminalParameters);
			}
			else
			{
				//
				// Now, even if this node is a terminal, it's parent might be a function that can only accept
				// terminal parameters.  If that is the case, can only mutate by creating a new terminal node
				if (find.Parent is GPNodeFunction)
				{
					newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, DepthInitial, ((GPNodeFunction)find.Parent).TerminalParameters);
				}
				else
				{
					newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, DepthInitial, false);
				}
			}

			//
			// If the parent of the result is null, then we are replacing the root node,
			// otherwise we are replacing a child of the parent.
			if (find.Parent == null)
			{
				BranchRoot = newSubtree;
			}
			else
			{
				find.Parent.Children[find.ChildNumber] = newSubtree;
			}
		}

		/// <summary>
		/// Performs a crossover operation with this program and the sibling program.
		/// Crossover is performed on like branches, i.e. LIBs only crossover with sibling
		/// LIBs.
		/// </summary>
		/// <param name="sibling">Which sibling to crossover with</param>
		public override void Crossover(GPProgramBranch sibling)
		{
			GPProgramBranchADL rightADL = (GPProgramBranchADL)sibling;

			//
			// Select a branch
			byte WhichBranch = (byte)GPUtilities.rngNextInt(4);
			switch (WhichBranch)
			{
				case 0:
					Crossover(ref m_BranchADL.m_LIB, m_BranchADL.NodeCountLIB,ref rightADL.m_LIB,rightADL.NodeCountLIB);
					break;
				case 1:
					Crossover(ref m_BranchADL.m_LCB, m_BranchADL.NodeCountLCB, ref rightADL.m_LCB, rightADL.NodeCountLCB);
					break;
				case 2:
					Crossover(ref m_BranchADL.m_LBB, m_BranchADL.NodeCountLBB, ref rightADL.m_LBB, rightADL.NodeCountLBB);
					break;
				case 3:
					Crossover(ref m_BranchADL.m_LUB, m_BranchADL.NodeCountLUB, ref rightADL.m_LUB, rightADL.NodeCountLUB);
					break;
			}

			//
			// Make sure the program stats get updated for both programs
			m_Branch.UpdateStats();
			sibling.UpdateStats();
		}

		/// <summary>
		/// This method performs crossover on the ADL branch.
		/// 90% of the time internal nodes are selected and 10% of the time
		/// leaf nodes are selected.
		/// TODO: Parameterize those percentages
		/// 
		/// If either one of the selected nodes is a function that accepts only
		/// terminals as parameters, no crossover is performed.
		/// 
		/// </summary>
		/// <param name="LeftBranchRoot">Root node of the left sibling</param>
		/// <param name="LeftNodeCount">Number of nodes in the left sibling</param>
		/// <param name="RightBranchRoot">Root node of the right sibling</param>
		/// <param name="RightNodeCount">Number of nodes in the right sibling</param>
		private void Crossover(ref GPNode LeftBranchRoot,int LeftNodeCount,ref GPNode RightBranchRoot,int RightNodeCount)
		{
			//
			// Step 1: Find a node in the left tree
			double TypeLeft = GPUtilities.rngNextDouble();
			GPProgramBranchFactory.FindResult findLeft = null;
			bool DoneLeft = false;
			while (!DoneLeft)
			{
				int NodeLeft = GPUtilities.rngNextInt(LeftNodeCount);
				findLeft = FindNode(null, LeftBranchRoot, NodeLeft);
				if (TypeLeft < 0.90 && (findLeft.Node is GPNodeFunction))
				{
					DoneLeft = true;
				}
				else if (TypeLeft >= 0.90 && (findLeft.Node is GPNodeTerminal))
				{
					DoneLeft = true;
				}
				else if (LeftNodeCount == 1)
				{
					DoneLeft = true;
				}
			}

			//
			// If the node is a function that only accepts terminal inputs, then crossover is not allowed
			if (findLeft.Node is GPNodeFunction && ((GPNodeFunction)findLeft.Node).TerminalParameters)
				return;
			if (findLeft.Parent != null && findLeft.Parent is GPNodeFunction && ((GPNodeFunction)findLeft.Parent).TerminalParameters)
				return;

			//
			// Step 2: Find a node in the right tree
			double TypeRight = GPUtilities.rngNextDouble();
			GPProgramBranchFactory.FindResult findRight = null;
			bool DoneRight = false;
			while (!DoneRight)
			{
				int NodeRight = GPUtilities.rngNextInt(RightNodeCount);
				findRight = FindNode(null, RightBranchRoot, NodeRight);
				if (TypeRight < 0.90 && (findRight.Node is GPNodeFunction))
				{
					DoneRight = true;
				}
				else if (TypeRight >= 0.90 && (findRight.Node is GPNodeTerminal))
				{
					DoneRight = true;
				}
				else if (RightNodeCount == 1)
				{
					DoneRight = true;
				}
			}

			//
			// If the node is a function that only accepts terminal inputs, then crossover is not allowed
			if (findRight.Node is GPNodeFunction && ((GPNodeFunction)findRight.Node).TerminalParameters)
				return;
			if (findRight.Parent != null && findRight.Parent is GPNodeFunction && ((GPNodeFunction)findRight.Parent).TerminalParameters)
				return;

			//
			// Step 3: Swap the references
			if (findLeft.Parent == null)
			{
				LeftBranchRoot = findRight.Node;
			}
			else
			{
				findLeft.Parent.Children[findLeft.ChildNumber] = findRight.Node;
			}

			if (findRight.Parent == null)
			{
				RightBranchRoot = findLeft.Node;
			}
			else
			{
				findRight.Parent.Children[findRight.ChildNumber] = findLeft.Node;
			}
		}

		/// <summary>
		/// This method randomly creates a new GPNode appropriate for an ADL branch.
		/// </summary>
		private GPNode CreateNode(GPEnums.TreeBuild TreeBuild, int Depth, bool TerminalOnly)
		{
			//
			// If required, create a terminal node only
			if (TerminalOnly)
			{
				return CreateNodeTerminal();
			}
			//
			// For the 'full' type growth, we want to select a function 
			// at every node except for the final leaf nodes, which will be chosen 
			// from the list of terminals.
			if (TreeBuild == GPEnums.TreeBuild.Full && Depth > 0)
			{
				//
				// Allow the loop to call ADFs
				// TODO: Add ability to call other ADLs (low priority)
				return CreateNodeFunction(true, 0, false, 0, false, 0);
			}
			//
			// If the depth is 0, then this MUST be a terminal node, no choice
			if (Depth == 0)
			{
				return CreateNodeTerminal();
			}
			//
			// We should only get here for the 'grow' type approach, here we make a uniform
			// random selection from among the combined list of operations and terminals
			// to decide the node type.
			double rnd = GPUtilities.rngNextDouble();
			double cumulative = m_Config.Profile.ProbabilityTerminalD;

			//
			// Start with terminal probability
			if (rnd <= cumulative)
			{
				return CreateNodeTerminal();
			}
			//
			// Next, probability of it being a function
			cumulative += m_Config.Profile.ProbabilityFunctionD;
			if (rnd <= cumulative)
			{
				//
				// Same note as above - can allow other ADFs to be called from here
				return CreateNodeFunction(true, 0, false, 0, false, 0);
			}

			//
			// TODO: Throw an exception that something really, really bad happened!

			return null;
		}

		/// <summary>
		/// A simple factory function that creates a terminal node and gets
		/// it's value initialized.
		/// </summary>
		protected new GPNode CreateNodeTerminal()
		{
			//
			// TODO: Need to modify the terminal initialization to allow the arguments
			// to be selected from ADL parameters.
			GPNodeTerminal node = GPNodeTerminal.Initialize(
				false, 0,
				true, m_BranchADL.NumberArgs,
				false, 0,
				m_Config.TerminalSet, m_Config.Profile.IntegerMin, m_Config.Profile.IntegerMax);

			return node;
		}
	}
}
