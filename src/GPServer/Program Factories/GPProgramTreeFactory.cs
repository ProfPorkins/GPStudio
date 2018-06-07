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
using System.Text;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// The purpose of this class is to perform the various genetic operators
	/// on a program tree.  I wanted to have this code separated from a program
	/// tree so that it is unique to the server code and the program tree code
	/// can be shared between the server and any clients that may want to use the
	/// runtime representation of a program tree.
	/// </summary>
	public class GPProgramTreeFactory
	{
		public GPProgramTreeFactory(GPModelerServer Config, int InputDimension)
		{
			m_Config = Config;
			m_InputDimension = InputDimension;

			m_OperatorADF = new GPProgramBranchFactoryADF(null, Config);
			m_OperatorADL = new GPProgramBranchFactoryADL(null, Config);
			m_OperatorADR = new GPProgramBranchFactoryADR(null, Config);
			m_OperatorRPB = new GPProgramBranchFactoryRPB(null, Config);
		}
		protected GPProgram m_Program;
		protected GPModelerServer m_Config;
		protected int m_InputDimension;
		protected GPProgramBranchFactoryADF m_OperatorADF;
		protected GPProgramBranchFactoryADL m_OperatorADL;
		protected GPProgramBranchFactoryADR m_OperatorADR;
		protected GPProgramBranchFactoryRPB m_OperatorRPB;

		/// <summary>
		/// Create a brand spanking new baby program tree!
		/// </summary>
		/// <param name="Depth">Max depth the tree can be</param>
		/// <param name="treeBuild">Tree building technique</param>
		/// <returns>Newly constructed program</returns>
		public GPProgram Construct(int nDepth, GPEnums.TreeBuild treeBuild)
		{
			//
			// Create an empty program tree
			m_Program = new GPProgram(null);

			//
			// We start by creating the ADFs first, because we want them available
			// to the RPB when it is created.

			//
			// Create some ADF branches
			for (short ADF = 0; ADF < m_Config.ADFSet.Count; ADF++)
			{
				GPProgramBranchADF adfBranch = new GPProgramBranchADF(m_Program, nDepth, ADF, m_Config.ADFSet[ADF]);
				m_OperatorADF.Build(adfBranch,treeBuild);
				m_Program.ADF.Add(adfBranch);
			}

			//
			// Create some ADL branches
			for (short ADL = 0; ADL < m_Config.ADLSet.Count; ADL++)
			{
				GPProgramBranchADL adlBranch = new GPProgramBranchADL(m_Program, nDepth, ADL, m_Config.ADLSet[ADL]);
				m_OperatorADL.Build(adlBranch, treeBuild);
				m_Program.ADL.Add(adlBranch);
			}

			//
			// Create some ADR branches
			for (short ADR = 0; ADR < m_Config.ADRSet.Count; ADR++)
			{
				GPProgramBranchADR adrBranch = new GPProgramBranchADR(m_Program, nDepth, ADR, m_Config.ADRSet[ADR]);
				m_OperatorADR.Build(adrBranch, treeBuild);
				m_Program.ADR.Add(adrBranch);
			}

			//
			// Build the RPB branch
			m_Program.RPB = new GPProgramBranchRPB(m_Program, m_Program.ADF.Count, nDepth, treeBuild);
			m_OperatorRPB.Build(m_Program.RPB,treeBuild);

			return m_Program;
		}

		/// <summary>
		/// Some operations create an empty program tree factory object to be used
		/// on many different programs.  This function allows a program tree to be
		/// attached to this factory and its operations performed on that tree
		/// </summary>
		/// <param name="gpTree">Program tree to attach</param>
		public void Attach(GPProgram gpTree)
		{
			m_Program = gpTree;
		}

		/// <summary>
		/// Performs a random mutation somewhere within the program tree.  All of
		/// the various program branches are equally weighted for selection.
		/// </summary>
		public void Mutate()
		{
			//
			// Make a random selection about which program branch to mutate.  We check
			// to see if the RPB uses an ADF, ADL or ADR, if not, they are not included as
			// an option for mutation.  This is an evoluationary optimization, don't waste
			// time mutating something that isn't being used.
			int Count = 1; // 1 is for the RPB
			if (m_Program.CountRPBADFNodes > 0)
				Count += m_Program.ADF.Count;
			if (m_Program.CountRPBADLNodes > 0)
				Count += m_Program.ADL.Count;
			if (m_Program.CountRPBADRNodes > 0)
				Count += m_Program.ADR.Count;

			int Selection = GPUtilities.rngNextInt(Count);

			//
			// Let's find out what we hit
			if (Selection == 0)	// RPB
			{
				GPProgramBranchFactoryRPB rpbOperator=new GPProgramBranchFactoryRPB(m_Program.RPB,m_Config);
				rpbOperator.Mutate();
				return;
			}
			Selection--;
			//
			// See if we hit an ADF - and there are ADFs in use
			if ((Selection < m_Program.ADF.Count) && (m_Program.CountRPBADFNodes > 0))
			{
				GPProgramBranchFactoryADF adfOperator = new GPProgramBranchFactoryADF(m_Program.ADF[Selection],m_Config);
				adfOperator.Mutate();
				return;
			}
			if (m_Program.CountRPBADFNodes > 0)	// only subtract if ADFs are in use
				Selection -= m_Program.ADF.Count;

			//
			// See if we hit an ADL - and ADLs are in use
			if ((Selection < m_Program.ADL.Count) && (m_Program.CountRPBADLNodes > 0))
			{
				GPProgramBranchFactoryADL adlOperator = new GPProgramBranchFactoryADL(m_Program.ADL[Selection], m_Config);
				adlOperator.Mutate();
				return;
			}
			if (m_Program.CountRPBADLNodes > 0) // only subtract if ADLs are in use
				Selection -= m_Program.ADL.Count;

			//
			// See if we hit an ADR - and ADRs are in use
			if ((Selection < m_Program.ADR.Count) && (m_Program.CountRPBADRNodes > 0))
			{
				GPProgramBranchFactoryADR adrOperator = new GPProgramBranchFactoryADR(m_Program.ADR[Selection], m_Config);
				adrOperator.Mutate();
				return;
			}
			if (m_Program.CountRPBADRNodes > 0) // only subtract if ADRs are in use
				Selection -= m_Program.ADR.Count;

		}

		/// <summary>
		/// This directs a crossover to be made with a sibling.
		/// A random selection is made from each of the program branches
		/// and then that branch is requested to perform mutation with
		/// the sibling.
		/// </summary>
		/// <param name="sibling"></param>
		/// <returns></returns>
		public GPProgram Crossover(GPProgram sibling)
		{
			//
			// Make a random selection about which program branch to select
			int Count = 1; // 1 is for the RPB
			if (m_Program.CountRPBADFNodes > 0)
				Count += m_Program.ADF.Count;
			if (m_Program.CountRPBADLNodes > 0)
				Count += m_Program.ADL.Count;
			if (m_Program.CountRPBADRNodes > 0)
				Count += m_Program.ADR.Count;

			int Selection=GPUtilities.rngNextInt(Count);
			
			if (Selection == 0)	// RPB
			{
				GPProgramBranchFactoryRPB rpbOperator = new GPProgramBranchFactoryRPB(m_Program.RPB, m_Config);
				rpbOperator.Crossover(sibling.RPB);

				return sibling;
			}
			Selection--;
			//
			// See if we hit an ADF
			if ((Selection < m_Program.ADF.Count) && (m_Program.CountRPBADFNodes > 0))
			{
				GPProgramBranchFactoryADF adfOperator = new GPProgramBranchFactoryADF(m_Program.ADF[Selection], m_Config);
				adfOperator.Crossover(sibling.ADF[Selection]);

				return sibling;
			}
			if (m_Program.CountRPBADFNodes > 0)
				Selection-=m_Program.ADF.Count;
			
			//
			// See if we hit an ADL
			if ((Selection < m_Program.ADL.Count) && (m_Program.CountRPBADLNodes > 0))
			{
				GPProgramBranchFactoryADL adlOperator = new GPProgramBranchFactoryADL(m_Program.ADL[Selection], m_Config);
				adlOperator.Crossover(sibling.ADL[Selection]);

				return sibling;
			}
			if (m_Program.CountRPBADLNodes > 0)
				Selection -= m_Program.ADL.Count;

			//
			// See if we hit an ADR
			if ((Selection < m_Program.ADR.Count) && (m_Program.CountRPBADRNodes > 0))
			{
				GPProgramBranchFactoryADR adrOperator = new GPProgramBranchFactoryADR(m_Program.ADR[Selection], m_Config);
				adrOperator.Crossover(sibling.ADR[Selection]);

				return sibling;
			}
			if (m_Program.CountRPBADRNodes > 0)
				Selection -= m_Program.ADR.Count;
			
		return null;
		}
	}
}
