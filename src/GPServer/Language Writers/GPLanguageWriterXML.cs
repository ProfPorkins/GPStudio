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
using System.Xml;

namespace GPStudio.Server
{
	/// <summary>
	/// The cannonical serialized form of a program is an XML format.  The
	/// use of the XML format is so a program can be persisted to storage and
	/// then read back from storage and the corresponding in-memory representation
	/// can be used.
	///
	/// Having this allows the program to be stored in a language independent
	/// format so that it can be re-loaded and serialized to a specific language
	/// format by any program writer.  Additionally, it allows a client UI to
	/// load up a program from storage and perform computations without having
	/// to write to (and potentially compile and load) a specific language for use.
	///
	///
	/// Nothing fancy out this, just a hierarchical representation of the tree
	/// data structure.
	/// </summary>
	public class GPLanguageWriterXML : GPLanguageWriter
	{
		private XmlTextWriter m_xmlWriter;

		public GPLanguageWriterXML(GPProgram Program, bool TimeSeries)
			: base(Program,TimeSeries)
		{
		}

		/// <summary>
		// Create an XML stream writer to write the program as an xml file
		/// </summary>
		/// <param name="InputStream">Stream the xml writer to utilize</param>
		public override void Initialize(System.IO.Stream InputStream)
		{
			m_xmlWriter=new XmlTextWriter(InputStream,System.Text.Encoding.UTF8);
			m_xmlWriter.Formatting = Formatting.Indented;
			m_xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");

			m_xmlWriter.WriteStartElement("GeneticProgram");
		}

		/// <summary>
		/// Close up the XML document
		/// </summary>
		public override void Terminate()
		{
			//
			// Close off the document
			m_xmlWriter.WriteEndElement();
			m_xmlWriter.Flush();
		}

		//
		// Write out the size of memory in the header
		public override bool WriteHeader(GPProgram Program) 
		{
			m_xmlWriter.WriteStartElement("Header");

			//
			// Record the size of memory to use
			m_xmlWriter.WriteStartElement("MemoryCount");
			m_xmlWriter.WriteValue(m_Program.CountMemory);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteEndElement();
			return true; 
		}

		//
		// Nothing needed in the trailer for an XML representation
		public override bool WriteTrailer(GPProgram Program) { return true; }


		public override bool WriteRPB(GPProgramBranchRPB rpb)
		{
			m_xmlWriter.WriteStartElement("RPB");

			//
			// Record the number of parameters
			m_xmlWriter.WriteStartElement("ParameterCount");
			m_xmlWriter.WriteValue(m_Program.InputDimension);
			m_xmlWriter.WriteEndElement();

			WriteProgramNode(rpb.Root);

			m_xmlWriter.WriteEndElement();

			return true;
		}

		public override bool WriteADF(GPProgramBranchADF adf)
		{
			m_xmlWriter.WriteStartElement("ADF");

			//
			// Record the number of parameters
			m_xmlWriter.WriteStartElement("ParameterCount");
			m_xmlWriter.WriteValue(adf.NumberArgs);
			m_xmlWriter.WriteEndElement();

			WriteProgramNode(adf.Root);

			m_xmlWriter.WriteEndElement();

			return true;
		}

		public override bool WriteADL(GPProgramBranchADL adl)
		{
			m_xmlWriter.WriteStartElement("ADL");

			//
			// Record the number of parameters
			m_xmlWriter.WriteStartElement("ParameterCount");
			m_xmlWriter.WriteValue(adl.NumberArgs);
			m_xmlWriter.WriteEndElement();

			//
			// Write each of the loop branches
			m_xmlWriter.WriteStartElement("LIB");
			WriteProgramNode(adl.LIB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("LCB");
			WriteProgramNode(adl.LCB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("LBB");
			WriteProgramNode(adl.LBB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("LUB");
			WriteProgramNode(adl.LUB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteEndElement();

			return true;
		}

		public override bool WriteADR(GPProgramBranchADR adr)
		{
			m_xmlWriter.WriteStartElement("ADR");

			//
			// Record the number of parameters
			m_xmlWriter.WriteStartElement("ParameterCount");
			m_xmlWriter.WriteValue(adr.NumberArgs);
			m_xmlWriter.WriteEndElement();

			//
			// Write each of the loop branches
			m_xmlWriter.WriteStartElement("RCB");
			WriteProgramNode(adr.RCB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("RBB");
			WriteProgramNode(adr.RBB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("RUB");
			WriteProgramNode(adr.RUB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteStartElement("RGB");
			WriteProgramNode(adr.RGB);
			m_xmlWriter.WriteEndElement();

			m_xmlWriter.WriteEndElement();

			return true;
		}

		//
		// Create the Element entry for a single node in the progam
		private void WriteProgramNode(GPNode node)
		{
			m_xmlWriter.WriteStartElement("GPNode");

			//
			// Terminal or function node?
			if (node is GPNodeTerminal)
			{
				GPNodeTerminal nodeTerminal=(GPNodeTerminal)node;

				m_xmlWriter.WriteAttributeString("Type", "Terminal");
				//
				// Write the type
				m_xmlWriter.WriteStartElement("Terminal");
				m_xmlWriter.WriteValue(nodeTerminal.ToValueTypeString());
				m_xmlWriter.WriteEndElement();
				//
				// Write the value
				m_xmlWriter.WriteStartElement("Value");
				if (node is GPNodeTerminalADRoot)
				{
					m_xmlWriter.WriteValue(((GPNodeTerminalADRoot)node).WhichParameter);
				}
				else if (node is GPNodeTerminalUserDefined)
				{
					m_xmlWriter.WriteValue(((GPNodeTerminalUserDefined)node).WhichUserDefined);
				}
				else if (node is GPNodeTerminalInteger)
				{
					m_xmlWriter.WriteValue(((GPNodeTerminalInteger)node).Value);
				}
				else if (node is GPNodeTerminalDouble)
				{
					m_xmlWriter.WriteValue(((GPNodeTerminalDouble)node).ToString());
				}
				m_xmlWriter.WriteEndElement();
			}
			else
			if (node is GPNodeFunction)
			{
				m_xmlWriter.WriteAttributeString("Type","Function");

				WriteFunctionNodeBody(node);
			}

			m_xmlWriter.WriteEndElement();
		}

		//
		// Gets the function nodes written out
		private void WriteFunctionNodeBody(GPNode node)
		{
		GPNodeFunction nodeFunc=(GPNodeFunction)node;

            String sType = node.GetType().ToString();

			if (node is GPNodeFunctionADF)
			{
				GPNodeFunctionADF nodeADF=(GPNodeFunctionADF)nodeFunc;

				m_xmlWriter.WriteStartElement("Function");
				m_xmlWriter.WriteValue("ADF");
				m_xmlWriter.WriteEndElement();

				//
				// Write out "which" ADF function this is
				m_xmlWriter.WriteStartElement("WhichFunction");
				m_xmlWriter.WriteValue(nodeADF.WhichFunction);
				m_xmlWriter.WriteEndElement();

				for (int nParam = 0; nParam < nodeADF.NumberArgs; nParam++)
				{
					WriteProgramNode(nodeADF.Children[nParam]);
				}
			}
			else if (node is GPNodeFunctionADL)
			{
				GPNodeFunctionADL nodeADL = (GPNodeFunctionADL)nodeFunc;

				m_xmlWriter.WriteStartElement("Function");
				m_xmlWriter.WriteValue("ADL");
				m_xmlWriter.WriteEndElement();

				//
				// Write out "which" ADL function this is
				m_xmlWriter.WriteStartElement("WhichFunction");
				m_xmlWriter.WriteValue(nodeADL.WhichFunction);
				m_xmlWriter.WriteEndElement();

				for (int nParam = 0; nParam < nodeADL.NumberArgs; nParam++)
				{
					WriteProgramNode(nodeADL.Children[nParam]);
				}
			}
			else if (node is GPNodeFunctionADR)
			{
				GPNodeFunctionADR nodeADR = (GPNodeFunctionADR)nodeFunc;

				m_xmlWriter.WriteStartElement("Function");
				m_xmlWriter.WriteValue("ADR");
				m_xmlWriter.WriteEndElement();

				//
				// Write out "which" ADR function this is
				m_xmlWriter.WriteStartElement("WhichFunction");
				m_xmlWriter.WriteValue(nodeADR.WhichFunction);
				m_xmlWriter.WriteEndElement();

				for (int nParam = 0; nParam < nodeADR.NumberArgs; nParam++)
				{
					WriteProgramNode(nodeADR.Children[nParam]);
				}
			}
			else
			{
				m_xmlWriter.WriteStartElement("Function");
				m_xmlWriter.WriteValue(nodeFunc.ToString());
				m_xmlWriter.WriteEndElement();

				//
				// Write out its children
				for (int nParam = 0; nParam < nodeFunc.NumberArgs; nParam++)
				{
					WriteProgramNode(nodeFunc.Children[nParam]);
				}
			}
		}
	}
}
