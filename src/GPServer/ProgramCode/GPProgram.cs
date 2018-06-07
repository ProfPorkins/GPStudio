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
	/// This is the tree data structure used to represent a Genetic Program.
	/// </summary>
	public class GPProgram : ICloneable
	{
		/// <summary>
		/// This constructor is only used when a clone() of the object
		/// is being made.  This keeps a new program from being generated.
		/// </summary>
		/// <param name="root"></param>
		/// <param name="InputDimension"></param>
		public GPProgram(GPProgramBranchRPB root)
		{
			m_RPB = root;
			//
			// Allocate the ADF List structure
			m_listADF = new List<GPProgramBranchADF>();

			//
			// Allocate the ADL List structure
			m_listADL = new List<GPProgramBranchADL>();

			//
			// Allocate the ADR List structure
			m_listADR = new List<GPProgramBranchADR>();
		}

		/// <summary>
		/// Cloneable interface
		/// </summary>
		/// <returns>Clone of the object</returns>
		public Object Clone()
		{
			//
			// Clone the RPB
			GPProgramBranchRPB rpb = (GPProgramBranchRPB)m_RPB.Clone();
			rpb.Parent = this;

			//
			// copy the main tree
			GPProgram tree = new GPProgram(rpb);
			m_Memory = new double[this.CountMemory];

			//
			// Handle the ADFs
			tree.m_listADF = new List<GPProgramBranchADF>();
			foreach (GPProgramBranchADF adf in m_listADF)
			{
				GPProgramBranchADF copy = (GPProgramBranchADF)adf.Clone();
				copy.Parent = this;
				tree.m_listADF.Add(copy);
			}

			//
			// Handle the ADLs
			tree.m_listADL = new List<GPProgramBranchADL>();
			foreach (GPProgramBranchADL adl in m_listADL)
			{
				GPProgramBranchADL copy = (GPProgramBranchADL)adl.Clone();
				copy.Parent = this;
				tree.m_listADL.Add(copy);
			}

			//
			// Handle the ADRs
			tree.m_listADR = new List<GPProgramBranchADR>();
			foreach (GPProgramBranchADR adr in m_listADR)
			{
				GPProgramBranchADR copy = (GPProgramBranchADR)adr.Clone();
				copy.Parent = this;
				tree.m_listADR.Add(copy);
			}

			return tree;
		}

		/// <summary>
		/// Number of Indexed Memory entries 
		/// </summary>
		public short CountMemory
		{
			get { return (short)m_Memory.Length; }
			set
			{
				//
				// Allocate the memory to use
				m_Memory = new double[value];
			}
		}
		public static double[] m_Memory;

		/// <summary>
		/// Result Producing Branch of the program
		/// </summary>
		public GPProgramBranchRPB RPB
		{
			get { return m_RPB; }
			set { m_RPB = value; }
		}
		protected GPProgramBranchRPB m_RPB;

		/// <summary>
		/// Set of Automatically Defined Functions
		/// </summary>
		public List<GPProgramBranchADF> ADF
		{
			get { return m_listADF; }
			set { m_listADF = value; }
		}
		protected List<GPProgramBranchADF> m_listADF;

		/// <summary>
		/// Set of Automatically Defined Loops
		/// </summary>
		public List<GPProgramBranchADL> ADL
		{
			get { return m_listADL; }
			set { m_listADL = value; }
		}
		protected List<GPProgramBranchADL> m_listADL;

		/// <summary>
		/// Set of Automatically Defined Recursions
		/// </summary>
		public List<GPProgramBranchADR> ADR
		{
			get { return m_listADR; }
			set { m_listADR = value; }
		}
		protected List<GPProgramBranchADR> m_listADR;

		public short InputDimension
		{
			get 
			{
				if (m_UserTerminals != null)
				{
					return (short)m_UserTerminals.Length;
				}
				else
				{
					return m_UserTerminalCount;
				}
			}
			set { m_UserTerminalCount=value; }
		}
		private short m_UserTerminalCount = 0;

		/// <summary>
		/// These are the user defined values that come from the training data.  These
		/// are used during the program evaluation.
		/// </summary>
		public double[] UserTerminals
		{
			get { return m_UserTerminals; }
			set { m_UserTerminals = value; }
		}
		private double[] m_UserTerminals;

		/// <summary>
		/// Reference to the input history series of data
		/// </summary>
		public List<List<double>> InputHistory
		{
			get { return m_InputHistory; }
			set { m_InputHistory = value; }
		}
		private List<List<double>> m_InputHistory;	
		
		/// <summary>
		/// Count up the number of nodes in the whole program.
		/// </summary>
		public int CountNodes
		{
			get
			{
				int nCount = m_RPB.CountNodes;

				for (int nADF = 0; nADF < this.ADF.Count; nADF++)
				{
					nCount += this.ADF[nADF].CountNodes;
				}

				for (int nADL = 0; nADL < this.ADL.Count; nADL++)
				{
					nCount += this.ADL[nADL].CountNodes;
				}

				for (int nADR = 0; nADR < this.ADR.Count; nADR++)
				{
					nCount += this.ADR[nADR].CountNodes;
				}

				return nCount;
			}
		}

		/// <summary>
		/// Count of ADF nodes in the RPB
		/// </summary>
		public int CountRPBADFNodes
		{
			get { return m_RPB.CountADFNodes; }
		}

		/// <summary>
		/// Count of ADL nodes in the RPB
		/// </summary>
		public int CountRPBADLNodes
		{
			get { return m_RPB.CountADLNodes; }
		}

		/// <summary>
		/// Count of ADR nodes in the RPB
		/// </summary>
		public int CountRPBADRNodes
		{
			get { return m_RPB.CountADRNodes; }
		}

		/// <summary>
		/// Perform the genetic operation of editing upon this program
		/// </summary>
		public void Edit()
		{
			//
			// Go through each of the program structures and perform editing
			m_RPB.Edit(this);
			m_RPB.UpdateStats();

			foreach (GPProgramBranchADF adf in m_listADF)
			{
				adf.Edit(this);
				adf.UpdateStats();
			}

			foreach (GPProgramBranchADL adl in m_listADL)
			{
				adl.Edit(this);
				adl.UpdateStats();
			}

			foreach (GPProgramBranchADR adr in m_listADR)
			{
				adr.Edit(this);
				adr.UpdateStats();
			}
		}
		
		/// <summary>
		/// This method evaluates the program and provides the return result
		/// as a double data type.
		/// </summary>
		public double EvaluateAsDouble()
		{
			//
			// reset memory
			for (int nPos = 0; nPos < this.CountMemory; nPos++)
			{
				m_Memory[nPos] = 0.0;
			}
			//
			// Run the beast!
			return m_RPB.EvaluateAsDouble(this);
		}

		/// <summary>
		/// Kickoff method for converting the program tree representation into
		/// a nice compact array representation.
		/// </summary>
		/// <param name="FunctionSet">List of user defined functions, the index
		/// of the function is used for as the array representation</param>
		public void ConvertToArray(IGPFunctionSet FunctionSet)
		{
			m_RPB.ConvertToArray(FunctionSet);
			foreach (GPProgramBranchADF adf in m_listADF)
			{
				adf.ConvertToArray(FunctionSet);
			}

			//
			// TODO: Should do this for ADLs and ADRs also, haven't yet done it
			// because use of those is very low right now.
		}

		/// <summary>
		/// Kickoff method for converting the program from an array based representation
		/// back into the nice easy to use tree representation.
		/// </summary>
		/// <param name="FunctionSet"></param>
		/// <param name="ForFitness">If this is for fitness computations, don't remove the array, because
		/// we can save time on the ConvertToArray method later on.</param>
		public void ConvertToTree(IGPFunctionSet FunctionSet, bool ForFitness)
		{
			m_RPB.ConvertToTree(FunctionSet,ForFitness);
			foreach (GPProgramBranchADF adf in m_listADF)
			{
				adf.ConvertToTree(FunctionSet,ForFitness);
			}
		}
	}
}
