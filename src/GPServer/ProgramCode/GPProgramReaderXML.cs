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
using System.Xml;
using System.IO;
using GPStudio.Shared;
using GPStudio.Interfaces;

namespace GPStudio.Server
{
	public class GPProgramReaderXML
	{
		XmlDocument m_DocProgram;
		IGPFunctionSet m_FunctionSet;

		/// <summary>
		/// Constructor that accepts a stream to work from
		/// </summary>
		/// <param name="XMLStream"></param>
		/// <param name="FunctionSet"></param>
		public GPProgramReaderXML(Stream XMLStream,IGPFunctionSet FunctionSet)
		{
			//
			// Save the function set
			m_FunctionSet = FunctionSet;

			//
			// Create a DOM document for parsing
			m_DocProgram = new XmlDocument();
			m_DocProgram.Load(XMLStream);
			m_DocProgram.Normalize();
		}

		/// <summary>
		/// Constructor that accepts an XML string to work from
		/// </summary>
		/// <param name="ProgramXML"></param>
		/// <param name="FunctionSet"></param>
		public GPProgramReaderXML(String ProgramXML, IGPFunctionSet FunctionSet)
		{
			//
			// Save the function set
			m_FunctionSet = FunctionSet;

			//
			// Create a DOM document for parsing
			m_DocProgram = new XmlDocument();
			m_DocProgram.LoadXml(ProgramXML);
			m_DocProgram.Normalize();
		}

		/// <summary>
		/// Build a program based upon the XML description that is provided.
		/// </summary>
		/// <returns></returns>
		public GPProgram Construct()
		{
			//
			// Create a program tree
			GPProgram Program=new GPProgram(null);

			//
			// Start off with the root node
			XmlElement xmlRoot = m_DocProgram.DocumentElement;

			//
			// Read the indexed memory size
			XmlNode xmlHeader = xmlRoot.SelectSingleNode("Header");
			if (xmlHeader != null)
			{
				XmlNode xmlMemory = xmlHeader.SelectSingleNode("MemoryCount");
				Program.CountMemory = Convert.ToInt16(xmlMemory.InnerText);
			}
			else
			{
				Program.CountMemory = 1;	// Some stupid default
			}

			//
			// Load the ADRs
			List<GPProgramBranchADR> ADRList = new List<GPProgramBranchADR>();
			XmlNodeList xmlListADR = xmlRoot.SelectNodes("ADR");
			short WhichADR = 0;
			foreach (XmlNode adrNode in xmlListADR)
			{
				GPProgramBranchADR adrBranch = LoadBranchADR(adrNode, WhichADR, Program);
				ADRList.Add(adrBranch);

				WhichADR++;
			}

			//
			// Add this as the list of ADRs for the program
			Program.ADR = ADRList;

			//
			// Load the ADLs
			List<GPProgramBranchADL> ADLList = new List<GPProgramBranchADL>();
			XmlNodeList xmlListADL = xmlRoot.SelectNodes("ADL");
			short WhichADL = 0;
			foreach (XmlNode adlNode in xmlListADL)
			{
				GPProgramBranchADL adlBranch = LoadBranchADL(adlNode, WhichADL, Program);
				ADLList.Add(adlBranch);

				WhichADL++;
			}

			//
			// Add this as the list of ADLs for the program
			Program.ADL = ADLList;

			//
			// Load the ADFs
			List<GPProgramBranchADF> ADFList = new List<GPProgramBranchADF>();
			XmlNodeList xmlListADF=xmlRoot.SelectNodes("ADF");
			short WhichADF = 0;
			foreach (XmlNode adfNode in xmlListADF)
			{
				GPProgramBranchADF adfBranch = LoadBranchADF(adfNode,WhichADF,Program);
				ADFList.Add(adfBranch);

				WhichADF++;
			}

			//
			// Don't forget to assign this list of ADFs to the program tree
			Program.ADF = ADFList;

			//
			// Get the RPB node
			XmlNode xmlRPB = xmlRoot.SelectSingleNode("RPB");

			//
			// Create the RPB
			Program.RPB = LoadBranchRPB(xmlRPB, ADFList,Program);

			return Program;
		}

		/// <summary>
		/// Parse the RPB branch
		/// </summary>
		/// <param name="rpbNode"></param>
		/// <param name="ADFList"></param>
		/// <param name="Program"></param>
		/// <returns></returns>
		private GPProgramBranchRPB LoadBranchRPB(XmlNode rpbNode,List<GPProgramBranchADF> ADFList,GPProgram Program)
		{
			//
			// Start out with the input dimension
			XmlNode xmlNode = rpbNode.SelectSingleNode("ParameterCount");
			Program.InputDimension = Convert.ToInt16(xmlNode.InnerText);

			//
			// Create the Branch tree
			GPProgramBranchRPB rpb = new GPProgramBranchRPB(Program, ADFList.Count, 0, GPEnums.TreeBuild.Undef);

			//
			// Get the root of the branch
			xmlNode = rpbNode.SelectSingleNode("GPNode");
			rpb.Root = ReadGPNode(xmlNode);

			rpb.UpdateStats();

			return rpb;
		}

		/// <summary>
		/// Parse an ADR branch
		/// </summary>
		/// <param name="adrNode"></param>
		/// <param name="WhichADR"></param>
		/// <param name="Program"></param>
		/// <returns>ADR program branch</returns>
		private GPProgramBranchADR LoadBranchADR(XmlNode adrNode, short WhichADR, GPProgram Program)
		{
			//
			// First step, parse out the number of arguments
			XmlNode xmlNode = adrNode.SelectSingleNode("ParameterCount");
			byte ParameterCount = Convert.ToByte(xmlNode.InnerText);

			//
			// Create the Branch tree
			GPProgramBranchADR adr = new GPProgramBranchADR(Program, 0, WhichADR, ParameterCount);

			//
			// There are four branches to read, do each of them separately
			xmlNode = adrNode.SelectSingleNode("RCB");
			adr.RCB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adrNode.SelectSingleNode("RBB");
			adr.RBB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adrNode.SelectSingleNode("RUB");
			adr.RUB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adrNode.SelectSingleNode("RGB");
			adr.RGB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			adr.UpdateStats();

			return adr;
		}

		/// <summary>
		/// Parse an ADL branch
		/// </summary>
		/// <param name="adlNode"></param>
		/// <param name="WhichADL"></param>
		/// <param name="Program"></param>
		/// <returns></returns>
		private GPProgramBranchADL LoadBranchADL(XmlNode adlNode, short WhichADL, GPProgram Program)
		{
			//
			// First step, parse out the number of arguments
			XmlNode xmlNode = adlNode.SelectSingleNode("ParameterCount");
			byte ParameterCount = Convert.ToByte(xmlNode.InnerText);

			//
			// Create the Branch tree
			GPProgramBranchADL adl = new GPProgramBranchADL(Program, 0, WhichADL, ParameterCount);

			//
			// There are four branches to read, do each of them separately
			xmlNode = adlNode.SelectSingleNode("LIB");
			adl.LIB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adlNode.SelectSingleNode("LCB");
			adl.LCB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adlNode.SelectSingleNode("LBB");
			adl.LBB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			xmlNode = adlNode.SelectSingleNode("LUB");
			adl.LUB = ReadGPNode(xmlNode.SelectSingleNode("GPNode"));

			adl.UpdateStats();

			return adl;
		}

		/// <summary>
		/// Parse an ADF branch
		/// </summary>
		/// <param name="adfNode"></param>
		/// <param name="WhichADF"></param>
		/// <param name="Program"></param>
		/// <returns></returns>
		private GPProgramBranchADF LoadBranchADF(XmlNode adfNode,short WhichADF,GPProgram Program)
		{
			//
			// First step, parse out the number of arguments
			XmlNode xmlNode = adfNode.SelectSingleNode("ParameterCount");
			byte ParameterCount = Convert.ToByte(xmlNode.InnerText);

			//
			// Create the Branch tree
			GPProgramBranchADF adf=new GPProgramBranchADF(Program, 0, WhichADF, ParameterCount);

			//
			// Get the root ADF node
			xmlNode = adfNode.SelectSingleNode("GPNode");
			adf.Root = ReadGPNode(xmlNode);

			adf.UpdateStats();

			return adf;
		}

		/// <summary>
		/// This is where most of the work is done, a GPNode is read
		/// </summary>
		/// <param name="TreeNode"></param>
		/// <returns></returns>
		private GPNode ReadGPNode(XmlNode TreeNode)
		{
			//
			// Find out which kind of node this is
			switch (TreeNode.Attributes[0].InnerText)
			{
				case "Function":
					return CreateFunction(TreeNode);
				case "Terminal":
					return CreateTerminal(TreeNode);
			}

			return null;
		}

		/// <summary>
		/// Constructs a Function node based upon the XML specification
		/// </summary>
		/// <param name="FunctionNode"></param>
		/// <returns></returns>
		private GPNode CreateFunction(XmlNode FunctionNode)
		{
			XmlNode xmlType = FunctionNode.SelectSingleNode("Function");
			XmlNodeList listParams = FunctionNode.SelectNodes("GPNode");

			GPNodeFunction gpFunction = null;

			//
			// See if we have an ADF
			if (xmlType.InnerText == "ADF")
			{
				XmlNode xmlWhichADF = FunctionNode.SelectSingleNode("WhichFunction");
				int WhichADF = Convert.ToInt32(xmlWhichADF.InnerText);
				gpFunction = new GPNodeFunctionADF(WhichADF, (byte)listParams.Count);
			}
			else if (xmlType.InnerText == "ADL")	// Check for an ADL
			{
				XmlNode xmlWhichADL = FunctionNode.SelectSingleNode("WhichFunction");
				int WhichADL = Convert.ToInt32(xmlWhichADL.InnerText);
				gpFunction = new GPNodeFunctionADL(WhichADL, (byte)listParams.Count);
			}
			else if (xmlType.InnerText == "ADR")	// Check for an ADR
			{
				XmlNode xmlWhichADR = FunctionNode.SelectSingleNode("WhichFunction");
				int WhichADR = Convert.ToInt32(xmlWhichADR.InnerText);
				gpFunction = new GPNodeFunctionADR(WhichADR, (byte)listParams.Count);
			}
			else
			{
				//
				// Get the correct function node type created
				GPNodeFunction func=(GPNodeFunction)m_FunctionSet[xmlType.InnerText.ToUpper()];
				gpFunction = (GPNodeFunction)func.Clone();
			}

			//
			// Build the list of parameters to this node
			foreach (XmlNode ParamNode in listParams)
			{
				gpFunction.Children.Add(ReadGPNode(ParamNode));
			}

			return gpFunction;
		}

		/// <summary>
		/// Constructs a Terminal node based upon the XML specification
		/// </summary>
		/// <param name="TerminalNode"></param>
		/// <returns></returns>
		private GPNode CreateTerminal(XmlNode TerminalNode)
		{
			XmlNode xmlType = TerminalNode.SelectSingleNode("Terminal");
			XmlNode xmlValue = TerminalNode.SelectSingleNode("Value");

			//
			// Set the terminal type
			switch (xmlType.InnerText)
			{
				case GPNodeTerminalADFParam.VALUETYPE:
					return new GPNodeTerminalADFParam(Convert.ToInt16(xmlValue.InnerText));
				case GPNodeTerminalADLParam.VALUETYPE:
					return new GPNodeTerminalADLParam(Convert.ToInt16(xmlValue.InnerText));
				case GPNodeTerminalADRParam.VALUETYPE:
					return new GPNodeTerminalADRParam(Convert.ToInt16(xmlValue.InnerText));
				case GPNodeTerminalDouble.VALUETYPE:
					return new GPNodeTerminalDouble(Convert.ToDouble(xmlValue.InnerText, GPUtilities.NumericFormat));
				case GPNodeTerminalInteger.VALUETYPE:
					return new GPNodeTerminalInteger(Convert.ToInt16(xmlValue.InnerText));
				case GPNodeTerminalUserDefined.VALUETYPE:
					return new GPNodeTerminalUserDefined(Convert.ToInt16(xmlValue.InnerText));
			}

			return null;
		}
	}
}
