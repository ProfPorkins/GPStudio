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
	public class GPProgramBranchFactoryRPB : GPProgramBranchFactory
	{
		public GPProgramBranchFactoryRPB(GPProgramBranchRPB RPB, GPModelerServer Config)
			: base(RPB,Config)
		{

		}

		/// <summary>
		/// This method directs the construction of an RPB
		/// </summary>
		/// <param name="Branch"></param>
		/// <param name="TreeBuild"></param>
		/// <returns></returns>
		public override bool Build(GPProgramBranch Branch, GPEnums.TreeBuild TreeBuild)
		{
			m_Branch = Branch;
			//
			// Call the recursive method to create the tree
			m_Branch.Root = BuildInternal(TreeBuild, m_Branch.DepthInitial,0,false);

			//
			// Update the tree stats
			m_Branch.UpdateStats();

			//
			// Convert the program into array representation
			m_Branch.ConvertToArray(m_Config.FunctionSet);

			return true;
		}

		/// <summary>
		/// Handles mutation of the RPB branch.  A node is randomly selected
		/// for mutation and a new branch is built from that location.
		/// </summary>
		public override void Mutate()
		{
			//
			// Step 1: Select a node randomly
			int NodeLabel = GPUtilities.rngNextInt(m_Branch.CountNodes);
			//
			// Step 2: Find this node, its parent and which child of the parent it is
			GPProgramBranchFactory.FindResult find = FindNode(null, m_Branch.Root, NodeLabel);
			//
			// Step 3: Build a new subtree that will replace the selected node
			// Assign a current depth of 2, to allow a terminal to mutate into the location
			GPNode newSubtree = null;
			if (find.Node is GPNodeFunction)
			{
				newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, m_Branch.DepthInitial, 2, ((GPNodeFunction)find.Node).TerminalParameters);
			}
			else
			{
				//
				// Now, even if this node is a terminal, it's parent might be a function that can only accept
				// terminal parameters.  If that is the case, can only mutate by create a new terminal node
				if (find.Parent is GPNodeFunction)
				{
					newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, m_Branch.DepthInitial, 2, ((GPNodeFunction)find.Parent).TerminalParameters);
				}
				else
				{
					newSubtree = BuildInternal(GPEnums.TreeBuild.Grow, m_Branch.DepthInitial, 2, false);
				}
			}
			//
			// If the parent of the result is null, then we are replacing the root node,
			// otherwise we are replacing a child of the parent.
			if (find.Parent == null)
			{
				m_Branch.Root = newSubtree;
			}
			else
			{
				find.Parent.Children[find.ChildNumber]=newSubtree;
			}

			//
			// Update the stats for this tree
			m_Branch.UpdateStats();
		}

		/// <summary>
		/// This method performs crossover on the result producing branch.
		/// 90% of the time internal nodes are selected and 10% of the time
		/// leaf nodes are selected.
		/// TODO: Parameterize those percentages
		/// 
		/// If either one of the selected nodes is a function that accepts only
		/// terminals as parameters, no crossover is performed.
		/// 
		/// </summary>
		/// <param name="sibling"></param>
		public override void Crossover(GPProgramBranch sibling)
		{
			GPProgramBranchRPB rightRPB = (GPProgramBranchRPB)sibling;

			//
			// Step 1: Find a node in the left tree
			double TypeLeft = GPUtilities.rngNextDouble();
			GPProgramBranchFactory.FindResult findLeft = null;
			bool DoneLeft = false;
			while (!DoneLeft)
			{
				int NodeLeft = GPUtilities.rngNextInt(m_Branch.CountNodes);
				findLeft = FindNode(null, m_Branch.Root, NodeLeft);
				if (TypeLeft < 0.90 && (findLeft.Node is GPNodeFunction))
				{
					DoneLeft = true;
				}
				else if (TypeLeft >= 0.90 && (findLeft.Node is GPNodeTerminal))
				{
					DoneLeft = true;
				}
				else if (TypeLeft >= 0.90 && (findLeft.Node is GPNodeFunction) && ((GPNodeFunction)findLeft.Node).Children.Count == 0)
				{
					//
					// This if statement accounts for the possibility of functions that
					// are constant values...because I didn't use a terminal type for them
					DoneLeft = true;
				}
				else if (m_Branch.CountNodes == 1)
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
				int NodeRight = GPUtilities.rngNextInt(rightRPB.CountNodes);
				findRight = FindNode(null, rightRPB.Root, NodeRight);
				if (TypeRight < 0.90 && (findRight.Node is GPNodeFunction))
				{
					DoneRight = true;
				}
				else if (TypeRight >= 0.90 && (findRight.Node is GPNodeTerminal))
				{
					DoneRight = true;
				}
				else if (TypeRight >= 0.90 && (findRight.Node is GPNodeFunction) && ((GPNodeFunction)findRight.Node).Children.Count == 0)
				{
					//
					// This if statement accounts for the possibility of functions that
					// are constant values...because I didn't use a terminal type for them
					DoneRight = true;
				}
				else if (rightRPB.CountNodes == 1)
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
				m_Branch.Root = findRight.Node;
			}
			else
			{
				findLeft.Parent.Children[findLeft.ChildNumber]=findRight.Node;
			}

			if (findRight.Parent == null)
			{
				rightRPB.Root = findLeft.Node;
			}
			else
			{
				findRight.Parent.Children[findRight.ChildNumber]=findLeft.Node;
			}

			//
			// Update the stats for these trees
			m_Branch.UpdateStats();
			rightRPB.UpdateStats();
		}

		/// <summary>
		/// Custom build function that creates the result producing branch
		/// of the genetic program.
		/// </summary>
		/// <param name="TreeBuild"></param>
		/// <param name="MaxDepth"></param>
		/// <param name="CurrentDepth"></param>
		/// <param name="TerminalParameters"></param>
		/// <returns></returns>
		private GPNode BuildInternal(GPEnums.TreeBuild TreeBuild, int MaxDepth,int CurrentDepth,bool TerminalParameters)
		{
			//
			// Randomly create this node
			GPNode newNode = CreateNode(TreeBuild,MaxDepth,CurrentDepth,TerminalParameters);
			//
			// If we just created a function, build its children
			if (newNode is GPNodeFunction)
			{
				GPNodeFunction node = (GPNodeFunction)newNode;
				//
				// Need to generate the appropriate number of nodes for the operation
				for (int Child = 0; Child < node.NumberArgs; Child++)
				{
					node.Children.Add(BuildInternal(TreeBuild, MaxDepth - 1, CurrentDepth + 1, node.TerminalParameters));
				}
			}

			return newNode;
		}

		/// <summary>
		/// This method randomly creates a new GPNode appropriate for an
		/// RPB branch.
		/// </summary>
		/// <param name="TreeBuild">Tree building technique</param>
		/// <param name="Depth">Initial Max depth of the tree</param>
		/// <param name="TerminalOnly">If true, create only a terminal node</param>
		/// <returns>Newly created node</returns>
		private GPNode CreateNode(GPEnums.TreeBuild TreeBuild, int Depth,int CurrentDepth,bool TerminalOnly)
		{
			//
			// If required, create a terminal node only
			if (TerminalOnly)
			{
				return CreateNodeTerminal();
			}

			//
			// if the depth is the Initial Max Depth (i.e. the first node in the tree)
			// automaticaly choose a function, don't want to waste our time with a 
			// terminal.  Also, for the 'full' type growth, we want to select a function 
			// at every node except for the final leaf nodes, which will be chosen 
			// from the list of terminals.
			if ((Depth == m_Branch.Depth || (TreeBuild == GPEnums.TreeBuild.Full && Depth > 0)))
			{
				return CreateNodeFunction(true, 0, true,0,true,0);
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
				return CreateNodeFunction(true, 0, true,0, true,0);
			}

			//
			// Throw an exception that something really, really bad happened!

			return null;
		}
	}
}
