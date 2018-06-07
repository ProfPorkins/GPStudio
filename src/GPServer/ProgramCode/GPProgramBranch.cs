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
using GPStudio.Interfaces;

namespace GPStudio.Server
{
	/// <summary>
	/// Represents the container for a branch of a program...
	///		Main Program Branch (RPB)
	///		Automatically Defined Function (ADF)
	///		Automatically Defined Loop (ADL)
	///		Automatically Defined Recursion (ADR)
	/// </summary>
	public abstract class GPProgramBranch : ICloneable
	{
		/// <summary>
		/// Default constructor, right now we don't do anything with it
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="nInitialDepth"></param>
		public GPProgramBranch(GPProgram parent, int nInitialDepth)
		{
			m_Parent=parent;
			m_DepthInitial=m_Depth=nInitialDepth;
		}
		
		/// <summary>
		/// Root node of this program branch
		/// </summary>
		public GPNode Root
		{
			get { return m_Root; }
			set { m_Root=value; }
		}
		protected GPNode m_Root = null;

		/// <summary>
		/// Reference to the GPProgram this program branch belongs to
		/// </summary>
		public GPProgram Parent
		{
			get { return m_Parent; }
			set { m_Parent = value; }
		}
		protected GPProgram m_Parent;

		/// <summary>
		/// Current depth of the program tree
		/// </summary>
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}
		protected int m_Depth = 0;

		/// <summary>
		/// Initial allowed depth for when the program was originally created
		/// </summary>
		public int DepthInitial
		{
			get { return m_DepthInitial; }
		}
		protected int m_DepthInitial = 0;

		/// <summary>
		/// Number of nodes in this program branch
		/// </summary>
		public ushort CountNodes
		{
			get { return m_CountNodes; }
			set { m_CountNodes = value; }
		}
		protected ushort m_CountNodes = 0;

		/// <summary>
		/// Number of leaf nodes in the branch
		/// </summary>
		public ushort CountLeafNodes
		{
			get { return m_CountLeafNodes; }
			set { m_CountLeafNodes = value; }
		}
		protected ushort m_CountLeafNodes;

		/// <summary>
		/// Number of ADF nodes in the tree.  This is not the number of ADF
		/// branches, but the number of times ADFs are called by this branch.
		/// </summary>
		public ushort CountADFNodes
		{
			get { return m_CountADFNodes; }
		}
		protected ushort m_CountADFNodes;

		/// <summary>
		/// Number of ADL nodes in the tree...same note as the ADFs above
		/// </summary>
		public ushort CountADLNodes
		{
			get { return m_CountADLNodes; }
		}
		protected ushort m_CountADLNodes;

		/// <summary>
		/// Number of ADR nodes in the tree...same note as the ADFs above
		/// </summary>
		public ushort CountADRNodes
		{
			get { return m_CountADRNodes; }
		}
		protected ushort m_CountADRNodes;

		/// <summary>
		/// Perform the genetic operation of editing on the program branch.  If
		/// anything custom needs to be done, derived classes can override.
		/// </summary>
		public virtual void Edit(GPProgram tree)
		{
			m_Root = m_Root.Edit(tree, this);
		}

		/// <summary>
		/// Cloneable interface
		/// </summary>
		/// <returns>Clone of the program branch</returns>
		public Object Clone()
		{
			GPProgramBranch obj = (GPProgramBranch)this.MemberwiseClone();

			if (m_Root != null)
			{
				obj.m_Root = (GPNode)m_Root.Clone();
			}
			obj.m_DepthInitial=m_DepthInitial;
			obj.m_Depth=m_Depth;
			obj.m_CountNodes=m_CountNodes;
			obj.m_CountLeafNodes=m_CountLeafNodes;
			obj.m_CountADFNodes = m_CountADFNodes;
			obj.m_CountADLNodes = m_CountADLNodes;
			obj.m_CountADRNodes = m_CountADRNodes;

			if (m_TreeArrayNew != null)
			{
				obj.m_TreeArrayNew = new List<byte>(m_TreeArrayNew.Count);
				foreach (byte Value in m_TreeArrayNew)
				{
					obj.m_TreeArrayNew.Add(Value);
				}
			}
			
			return obj;
		}

		/// <summary>
		/// Works through the program tree to update the various statistics associated
		/// with it.
		/// </summary>
		public virtual void UpdateStats()
		{
			Depth = ComputeDepth(Root);
			CountLeafNodes = DoCountLeafNodes(Root);
			CountNodes = DoCountNodes(Root);
		}

		/// <summary>
		/// Computes the max depth of the tree
		/// </summary>
		/// <param name="node">Root of the tree</param>
		/// <returns>Depth of the tree</returns>
		protected int ComputeDepth(GPNode node)
		{
			int nMaxChild = 0;

			if (node == null) return 0;

			//
			// Only need to probe further if we have a function node with children
			if (node is GPNodeFunction)
			{
				GPNodeFunction nodeFunc = (GPNodeFunction)node;

				//
				// Determine the child with the maximum depth
				for (int nChild = 0; nChild < nodeFunc.Children.Count; nChild++)
				{
					int nCurrentChild = ComputeDepth(nodeFunc.Children[nChild]);
					if (nCurrentChild > nMaxChild)
					{
						nMaxChild = nCurrentChild;
					}
				}
			}

			return nMaxChild + 1;	// +1 is the current node depth
		}

		private ushort m_CountLabel;
		/// <summary>
		/// Counts the number of nodes in the branch.  While the nodes are
		/// being counted, give each one a label so it can be later used for
		/// searching.  This method also takes care of getting the count of ADF
		/// and ADL nodes updated.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected ushort DoCountNodes(GPNode node)
		{
			//
			// Reset all the stats to zip
			m_CountADFNodes = 0;
			m_CountADLNodes = 0;
			m_CountADRNodes = 0;
			m_CountLabel = 0;
			DoCountNodesInternal(node);

			return m_CountLabel;
		}

		/// <summary>
		/// Internal recursive method that does the actual work of counting the
		/// nodes in the tree.
		/// </summary>
		/// <param name="node"></param>
		private void DoCountNodesInternal(GPNode node)
		{
			if (node == null) return;
			//
			// If we made it this far, count the node
			node.Label = m_CountLabel++;

			//
			// Update the ADF and ADL counts
			if (node is GPNodeFunctionADF) m_CountADFNodes++;
			if (node is GPNodeFunctionADL) m_CountADLNodes++;
			if (node is GPNodeFunctionADR) m_CountADRNodes++;

			//
			// If a terminal node, that's it, just count the node
			if (node is GPNodeTerminal)
			{
				return;
			}

			//
			// Otherwise, we have a functional node, so count up the children
			GPNodeFunction nodeFunc = (GPNodeFunction)node;
			for (int nChild = 0; nChild < nodeFunc.Children.Count; nChild++)
			{
				DoCountNodesInternal(nodeFunc.Children[nChild]);
			}
		}

		/// <summary>
		/// Counts the number of leaf nodes in the branch
		/// </summary>
		/// <param name="node">Root of the tree</param>
		/// <returns>Number of leaf nodes in the branch</returns>
		protected ushort DoCountLeafNodes(GPNode node)
		{
			if (node == null) return 0;
			//
			// If a terminal node, that's it, plus, it's also a leaf node by definition
			if (node is GPNodeTerminal)
			{
				return 1;
			}

			//
			// Otherwise, we have a functional node, so count up leafs in the children
			GPNodeFunction nodeFunc = (GPNodeFunction)node;
			ushort countChild = 0;
			for (int nChild = 0; nChild < nodeFunc.Children.Count; nChild++)
			{
				countChild += DoCountLeafNodes(nodeFunc.Children[nChild]);
			}
			//
			// If this is a functional node with no children, it is a terminal, so count it.
			if (nodeFunc.Children.Count == 0)
			{
				return (ushort)(countChild + 1);
			}
			return countChild;
		}

		private int m_ArrayPos;
		List<byte> m_TreeArrayNew;
		/// <summary>
		/// Converts the tree into an array based representation.  The purpose
		/// of the array representation is that it takes up FAR LESS memory and
		/// allows for a larger popuplation.  This comes at the expense of some
		/// compute time, however.
		/// </summary>
		/// <param name="FunctionSet"></param>
		public void ConvertToArray(IGPFunctionSet FunctionSet)
		{
			//
			// If the array already exists, reuse it.  The reason it would already exist
			// is if the conversion to a tree was don't for fitness computations and not
			// for altering the tree sturcture.  A simple optimization technique
			if (m_TreeArrayNew != null)
			{
				m_Root = null;
				return;
			}

			//
			// Allocate the nodes we need
			try
			{
				m_TreeArrayNew = new List<byte>(m_CountNodes);
			}
			catch (Exception)
			{
				System.Console.WriteLine("ConvertToArray: " + m_CountNodes);
				return;
			}

			//
			// Use a recursive technique to work through the nodes and get
			// them stored into the array
			m_ArrayPos = 0;
			StoreNode(m_Root, FunctionSet);

			//
			// Get rid of the tree
			m_Root = null;
		}

		/// <summary>
		/// Converts a tree node into an array based representation.  For functions
		/// an byte lookup is used into the FunctionSet, for terminals, the type
		/// is recorded along with whatever extra data may be needed.
		/// TODO: Get rid of the unsafe code.  I know there is a bit conversion
		/// function, I just couldn't find it quickly.
		/// </summary>
		/// <param name="Node">The node to convert</param>
		/// <param name="FunctionSet"></param>
		private void StoreNode(GPNode Node, IGPFunctionSet FunctionSet)
		{
			//
			// Figure out what kind of node we have and store it accordingly.
			if (Node is GPNodeFunction)
			{
				GPNodeFunction NodeFunction = (GPNodeFunction)Node;
				if (NodeFunction is GPNodeFunctionADF)
				{
					GPNodeFunctionADF adf=(GPNodeFunctionADF)NodeFunction;
					//
					// For an ADF node,record it as value 255 and then store the which
					// ADF it refers to and the number of arguments.
					m_TreeArrayNew.Add((byte)255);
					m_TreeArrayNew.Add((byte)adf.WhichFunction);
					m_TreeArrayNew.Add(adf.NumberArgs);
				}
				else if (NodeFunction is GPNodeFunctionADL)
				{
					GPNodeFunctionADL adl = (GPNodeFunctionADL)NodeFunction;
					//
					// For an ADL node,record it as value 254 and then store the which
					// ADL it refers to and the number of arguments.
					m_TreeArrayNew.Add((byte)254);
					m_TreeArrayNew.Add((byte)adl.WhichFunction);
					m_TreeArrayNew.Add(adl.NumberArgs);
				}
				else if (NodeFunction is GPNodeFunctionADR)
				{
					GPNodeFunctionADR adr = (GPNodeFunctionADR)NodeFunction;
					//
					// For an ADR node,record it as value 253 and then store the which
					// ADR it refers to and the number of arguments.
					m_TreeArrayNew.Add((byte)253);
					m_TreeArrayNew.Add((byte)adr.WhichFunction);
					m_TreeArrayNew.Add(adr.NumberArgs);
				}
				else
				{
					//
					// Store the index of the function and then the number of children
					// for this function
					m_TreeArrayNew.Add((byte)FunctionSet.IndexOfKey(NodeFunction.ToStringUpper()));
					m_TreeArrayNew.Add((byte)((GPNodeFunction)Node).Children.Count);
				}
				
				//
				// Store each of the children
				foreach (GPNode NodeChild in NodeFunction.Children)
				{
					StoreNode(NodeChild, FunctionSet);
				}
			}
			else // Terminal Node
			{
				//
				// Depending upon the type of the terminal, set the appropriate value
				if (Node is GPNodeTerminalADFParam)
				{
					m_TreeArrayNew.Add(200);
					m_TreeArrayNew.Add((byte)((GPNodeTerminalADFParam)Node).WhichParameter);
				}
				else if (Node is GPNodeTerminalUserDefined)
				{
					m_TreeArrayNew.Add(201);
					m_TreeArrayNew.Add((byte)((GPNodeTerminalUserDefined)Node).WhichUserDefined);
				}
				else if (Node is GPNodeTerminalDouble)
				{
					m_TreeArrayNew.Add(202);
					double dValue=((GPNodeTerminalDouble)Node).Value;
					unsafe
					{
						byte* ptr = (byte*)&dValue;
						for (int pos = 0; pos < sizeof(double); pos++)
						{
							m_TreeArrayNew.Add(ptr[pos]);
						}
					}
				}
				else if (Node is GPNodeTerminalInteger)
				{
					m_TreeArrayNew.Add(203);
					int dValue = ((GPNodeTerminalInteger)Node).Value;
					unsafe
					{
						byte* ptr = (byte*)&dValue;
						for (int pos = 0; pos < sizeof(int); pos++)
						{
							m_TreeArrayNew.Add(ptr[pos]);
						}
					}
				}
			}
		}

		/// <summary>
		/// Restores the program from the array representation back into a tree structure.
		/// </summary>
		/// <param name="FunctionSet"></param>
		/// <param name="ForFitness"></param>
		public void ConvertToTree(IGPFunctionSet FunctionSet, bool ForFitness)
		{
			//
			// Recursively build the tree out of the array
			m_ArrayPos = 0;
			GPNode root = ConvertNode(FunctionSet);
			m_Root = root;

			//
			// Remove the array - unless this is for a fitness computation
			if (!ForFitness)
			{
				m_TreeArrayNew = null;
			}

			this.UpdateStats();
		}

		/// <summary>
		/// Restores a node from the array representation back into a tree representation
		/// </summary>
		/// <param name="FunctionSet"></param>
		/// <returns>Reference to the converted tree</returns>
		public GPNode ConvertNode(IGPFunctionSet FunctionSet)
		{
			//
			// Determine the type of the node
			// < 200 : User Defined function
			// 200 to 210 : Terminal
			// 253 : ADR
			// 254 : ADL
			// 255 : ADF
			if (m_TreeArrayNew[m_ArrayPos] == 255)
			{
				m_ArrayPos++;
				byte WhichADF = m_TreeArrayNew[m_ArrayPos++];
				byte NumberArgs=m_TreeArrayNew[m_ArrayPos++];
				GPNodeFunctionADF NodeNew = new GPNodeFunctionADF(WhichADF,NumberArgs);
				//
				// Build up the children before returning
				for (byte Child = 0; Child < NumberArgs; Child++)
				{
					NodeNew.Children.Add(ConvertNode(FunctionSet));
				}
				return NodeNew;
			}
			else if (m_TreeArrayNew[m_ArrayPos] == 254)
			{
				m_ArrayPos++;
				byte WhichADL = m_TreeArrayNew[m_ArrayPos++];
				byte NumberArgs = m_TreeArrayNew[m_ArrayPos++];
				GPNodeFunctionADL NodeNew = new GPNodeFunctionADL(WhichADL, NumberArgs);
				//
				// Build up the children before returning
				for (byte Child = 0; Child < NumberArgs; Child++)
				{
					NodeNew.Children.Add(ConvertNode(FunctionSet));
				}
				return NodeNew;
			}
			else if (m_TreeArrayNew[m_ArrayPos] == 253)
			{
				m_ArrayPos++;
				byte WhichADR = m_TreeArrayNew[m_ArrayPos++];
				byte NumberArgs = m_TreeArrayNew[m_ArrayPos++];
				GPNodeFunctionADR NodeNew = new GPNodeFunctionADR(WhichADR, NumberArgs);
				//
				// Build up the children before returning
				for (byte Child = 0; Child < NumberArgs; Child++)
				{
					NodeNew.Children.Add(ConvertNode(FunctionSet));
				}
				return NodeNew;
			}
			else if (m_TreeArrayNew[m_ArrayPos] < 200)
			{
				GPNodeFunction NodeNew = (GPNodeFunction)((GPNodeFunction)FunctionSet[m_TreeArrayNew[m_ArrayPos++]]).Clone();
				//
				// Build up this node's children first
				byte ChildCount = m_TreeArrayNew[m_ArrayPos++];
				for (byte Child = 0; Child < ChildCount; Child++)
				{
					NodeNew.Children.Add(ConvertNode(FunctionSet));
				}
				return NodeNew;
			}
			else // Terminal
			{
				switch (m_TreeArrayNew[m_ArrayPos++])
				{
					case 200:
						byte WhichADFParam = m_TreeArrayNew[m_ArrayPos++];
						GPNodeTerminalADFParam NodeADF = new GPNodeTerminalADFParam(WhichADFParam);
						return NodeADF;
					case 201:
						byte WhichUserDefined = m_TreeArrayNew[m_ArrayPos++];
						GPNodeTerminalUserDefined NodeUser = new GPNodeTerminalUserDefined(WhichUserDefined);
						return NodeUser;
					case 202:
						double dValue;
						unsafe
						{
							byte* ptr = (byte*)&dValue;
							for (int pos = 0; pos < sizeof(double); pos++)
							{
								ptr[pos] = m_TreeArrayNew[m_ArrayPos++];
							}
						}
						GPNodeTerminalDouble NodeDouble = new GPNodeTerminalDouble(dValue);
						return NodeDouble;
					case 203:
						int nValue;
						unsafe
						{
							byte* ptr = (byte*)&nValue;
							for (int pos = 0; pos < sizeof(int); pos++)
							{
								ptr[pos] = m_TreeArrayNew[m_ArrayPos++];
							}
						}
						GPNodeTerminalInteger NodeInteger = new GPNodeTerminalInteger(nValue);
						return NodeInteger;
				}
			}

			//
			// TODO: Throw an exception, instead of this lazy crap! :)
			return null;
		}

	}
}
