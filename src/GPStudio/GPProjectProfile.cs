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
using System.Data.OleDb;
using System.Data;
using GPStudio.Shared;

namespace GPStudio.Client
{
	/// <summary>
	/// This class represents the settings for some particular modeling
	/// profile.  It contains the settings to drive the generation of
	/// a set of models.
	/// TODO: This class is a bit of a mess.  It started life with the original
	/// client server model, but I've really rearranged things since then.  I'd
	/// like to clean this up in a major way, at some point.
	/// </summary>
	public class GPProjectProfile
	{
		/// <summary>
		/// Default constructor - Get the default profile settings prepared, along with
		/// creating any objects we need to.
		/// </summary>
		public GPProjectProfile()
		{
			m_ModelingProfile = new GPModelingProfile();

			ModelType = new GPModelType();
			Dirty = false;

			//
			// Setup the Function Set
			m_FunctionSet = new List<string>();
			//
			// Provide some default function selections
			m_FunctionSet.Add("Add");
			m_FunctionSet.Add("Subtract");
			m_FunctionSet.Add("Multiply");
			m_FunctionSet.Add("Divide");
			m_FunctionSet.Add("Average");
			m_FunctionSet.Add("Min");
			m_FunctionSet.Add("Max");
			m_FunctionSet.Add("Sqrt");
			m_FunctionSet.Add("Exp");
			m_FunctionSet.Add("Pow");

			m_ADFSet = new List<byte>();
			m_ADLSet = new List<byte>();
			m_ADRSet = new List<byte>();

			//
			// Set a default fitness functin ID = Raw Fitness
			// TODO: Yes, this is dangerous, the value "should" be read
			// from the database, along with some additional UI protection.
			FitnessFunctionID = 1;
		}

		/// <summary>
		/// Contained GPModelingProfile object
		/// </summary>
		private GPModelingProfile m_ModelingProfile;
		/// <summary>
		/// A containment model is used, instead of an inheritance model because
		/// we need to send the modeling profile to a remote object and ONLY
		/// a GPModelingProfile object, because GPProjectProfile can not be
		/// serialized.
		/// </summary>
		public GPModelingProfile ModelingProfile
		{
			get { return m_ModelingProfile; }
		}

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}
		private string m_Name;

		private bool m_Dirty;
		public bool Dirty
		{
			get { return m_Dirty; }
			set { m_Dirty = value; }
		}

		private int m_ProfileID;
		public int ProfileID
		{
			get { return m_ProfileID; }
			set { m_ProfileID = value; }
		}
		
		private int m_FitnessFunctionID;
		public int FitnessFunctionID
		{
			get { return m_FitnessFunctionID; }
			set { m_FitnessFunctionID = value; }
		}

		//
		// Modeling Type
		public class GPModelType
		{
			private GPEnums.ModelType m_Type;
			private short m_InputDimension;
			private short m_PredictionDistance;

			public GPModelType()
			{
				m_Type = GPEnums.ModelType.Regression;
				m_InputDimension = 1;
				m_PredictionDistance = 1;
			}

			public GPEnums.ModelType Type
			{
				get { return m_Type; }
				set { m_Type = value; }
			}

			public short InputDimension
			{
				get { return m_InputDimension; }
				set { m_InputDimension = value; }
			}

			public short PredictionDistance
			{
				get { return m_PredictionDistance; }
				set { m_PredictionDistance = value; }
			}
		}
		public GPModelType ModelType;

		#region Structures

		public List<byte> m_ADFSet;
		public List<byte> m_ADLSet;
		public List<byte> m_ADRSet;

		#endregion

		#region Generation Options

		public bool m_useMaxNumber = true;
		public int m_maxNumber = 50;
		public bool m_useRawFitness0 = true;
		public bool m_useHitsMaxed = true;

		#endregion

		/// <summary>
		/// Container that lists the user defined functions for this profile
		/// </summary>
		public List<string> FunctionSet
		{
		  get { return m_FunctionSet; }
		  set { m_FunctionSet = value; }
		}
		private List<string> m_FunctionSet;
		
		// -------------------------------------------------------------- //
		//
		// Profile Methods Below
		//
		// -------------------------------------------------------------- //	
		

		/// <summary>
		/// Saves the profile to the specified stream.  The profile is saved
		/// into an XML format, just using an XmlWriter on top of the stream.
		/// </summary>
		/// <param name="ProfileStream">Stream to Save the profile with</param>
		/// <returns>True/False upon success/failure</returns>
		public bool Save(Stream ProfileStream)
		{
			//
			// Create an XML document
			XmlTextWriter xmlWriter = new XmlTextWriter(ProfileStream, System.Text.Encoding.UTF8);
			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
			xmlWriter.WriteStartElement("GPConfiguration");

			SaveModelType(xmlWriter);

			SaveFitnessOptions(xmlWriter);
			SaveReproduction(xmlWriter);

			SavePopulationInit(xmlWriter);
			SaveGPStructure(xmlWriter);
			SaveGenerationOptions(xmlWriter);
			SaveFunctionSet(xmlWriter);
			SaveTerminalSet(xmlWriter);
			
			//
			// Close off the document
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();

			Dirty = false;
			
		return true;
		}
		
		/// <summary>
		/// Loads a model profile into memory.  The profile is stored in
		/// an XML format, so we use XML parsing to pull out the sections
		/// from the file.
		///
		/// The reading of the file is done by section.  Each section is responsible
		/// for opening, finding, reading and closing the file.  This makes it so the
		/// settings in the file can be organized in any order...human friendly.
		/// </summary>
		/// <param name="ProfileStream">The stream to read the profile from</param>
		/// <returns>True/False upon success or failure</returns>
		public bool Load(Stream ProfileStream)
		{

			//
			// Using a DOM model, so open accordingly
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(ProfileStream);
			xmlDocument.Normalize();
			XmlElement xmlRoot = xmlDocument.DocumentElement;

			LoadModelType(xmlRoot);
			LoadFitnessOptions(xmlRoot);
			LoadReproduction(xmlRoot);

			LoadPopulationInit(xmlRoot);
			LoadGPStructure(xmlRoot);
			LoadGenerationOptions(xmlRoot);
			LoadFunctionSet(xmlRoot);
			LoadTerminalSet(xmlRoot);

			Dirty = false;

		return true;
		}

		/// <summary>
		/// Creates an entry for this profile in the database
		/// </summary>
		/// <param name="ModelProfileID">DBCode of the newly created profile</param>
		/// <returns>True/False upon success or failure</returns>
		public bool CreateProfileInDB(ref int ModelProfileID)
		{
			//
			// Save the profile to a memory stream and then pull out the bytes
			byte[] blobData = null;
			using (MemoryStream ms = new MemoryStream())
			{
				this.Save(ms);

				blobData = new byte[ms.Length];
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(blobData, 0, (int)ms.Length);
			}

			OleDbParameter param = new OleDbParameter(
				"@Profile",
				OleDbType.VarBinary,
				blobData.Length,
				ParameterDirection.Input,
				false,
				0, 0, null,
				DataRowVersion.Current, blobData);

			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to add the blob to the database
			OleDbCommand cmd = new OleDbCommand("INSERT INTO tblModelProfile(Name,Profile) VALUES ('New Profile',?)", con);
			cmd.Parameters.Add(param);

			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
				//
				// Grab the DBCode generated for this file
				cmd.CommandText = "SELECT @@IDENTITY";
				OleDbDataReader reader = cmd.ExecuteReader();
				reader.Read();
				this.ProfileID = Convert.ToInt32(reader[0].ToString()); ;
				ModelProfileID = this.ProfileID;
				reader.Close();
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
				return false;
			}

			con.Close();

			return true;
		}

		/// <summary>
		/// Loads the indicated profile into the class
		/// </summary>
		/// <param name="ProfileID">DBCode of the profile to load</param>
		/// <returns>True/False upon success or failure</returns>
		public bool LoadFromDB(int ProfileID)
		{
			this.ProfileID = ProfileID;

			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				//
				// Build the command to read the blob to the database
				OleDbCommand cmd = new OleDbCommand("SELECT Profile FROM tblModelProfile WHERE DBCode = " + ProfileID, con);

				//
				// Execute the command
				try
				{
					OleDbDataReader reader = cmd.ExecuteReader();
					reader.Read();
					//
					// Pull the Training out
					Byte[] blob = new Byte[(reader.GetBytes(0, 0, null, 0, int.MaxValue))];
					reader.GetBytes(0, 0, blob, 0, blob.Length);
					reader.Close();

					using (MemoryStream ms = new MemoryStream())
					{
						ms.Write(blob, 0, blob.Length);
						ms.Seek(0, SeekOrigin.Begin);
						this.Load(ms);
					}
				}
				catch (OleDbException)
				{
					return false;
				}
			}

			this.Name = GPDatabaseUtils.FieldValue(ProfileID, "tblModelProfile", "Name");

			return true;
		}

		/// <summary>
		/// Updates an existing profile in the database with the current specifications
		/// </summary>
		/// <returns>True/False upon success or failure</returns>
		public bool UpdateProfileInDB()
		{
			//
			// Save the profile to a memory stream and then pull out the bytes
			byte[] blobData = null;
			using (MemoryStream ms = new MemoryStream())
			{
				this.Save(ms);

				blobData = new byte[ms.Length];
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(blobData, 0, (int)ms.Length);
			}

			OleDbParameter param = new OleDbParameter(
				"@Profile",
				OleDbType.VarBinary,
				blobData.Length,
				ParameterDirection.Input,
				false,
				0, 0, null,
				DataRowVersion.Current, blobData);

			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Execute the command
			try
			{
				//
				// Build the command to clear the blob in the database
				OleDbCommand cmd = new OleDbCommand("UPDATE tblModelProfile SET Profile = NULL WHERE DBCode = " + this.ProfileID, con);
				cmd.ExecuteNonQuery();

				//
				// Build the command to replace the blob in the database
				cmd = new OleDbCommand("UPDATE tblModelProfile SET Profile = ? WHERE DBCode = " + this.ProfileID, con);
				cmd.Parameters.Add(param);
				cmd.ExecuteNonQuery();
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
				return false;
			}

			con.Close();

			return true;
		}

		#region Profile Load/Save Methods

		private const String PROFILE_FILE_MODELTYPE				= "ModelType";
		private const String PROFILE_FILE_MODELTYPE_SETTING		= "Setting";
		private const String PROFILE_FILE_MODELTYPE_INPUTDIM	= "InputDimension";
		private const String PROFILE_FILE_MODELTYPE_PREDICTDIST	= "PredictionDistance";
		//
		// <ModelType>
		private bool LoadModelType(XmlElement xmlRoot)
		{
			try
			{
				XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_MODELTYPE);

				//
				// Grab the actual type
				XmlNode xmlNodeSetting = xmlNode.SelectSingleNode(PROFILE_FILE_MODELTYPE_SETTING);
				String sSetting = xmlNodeSetting.InnerText;

				if (sSetting == GPEnums.ModelType.Regression.ToString())
				{
					ModelType.Type = GPEnums.ModelType.Regression;
				}
				else
					if (sSetting == GPEnums.ModelType.TimeSeries.ToString())
				{
					ModelType.Type = GPEnums.ModelType.TimeSeries;
				}

				//
				// Grab the Input Dimension
				XmlNode xmlNodeInputDim = xmlNode.SelectSingleNode(PROFILE_FILE_MODELTYPE_INPUTDIM);
				ModelType.InputDimension = Convert.ToInt16(xmlNodeInputDim.InnerText);

				XmlNode xmlNodePredictDist = xmlNode.SelectSingleNode(PROFILE_FILE_MODELTYPE_PREDICTDIST);
				//
				// TODO: remove this test once we are ready for production relase
				if (xmlNodePredictDist != null)
				{
					ModelType.PredictionDistance = Convert.ToInt16(xmlNodePredictDist.InnerText);
				}
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		//
		// <ModelType>
		private bool SaveModelType(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_MODELTYPE);

			xmlWriter.WriteStartElement(PROFILE_FILE_MODELTYPE_SETTING);
			xmlWriter.WriteValue(ModelType.Type.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_MODELTYPE_INPUTDIM);
			xmlWriter.WriteValue(ModelType.InputDimension);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_MODELTYPE_PREDICTDIST);
			xmlWriter.WriteValue(ModelType.PredictionDistance);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();

			return true;
		}

		private const String PROFILE_FILE_SPEA2MULTIOBJECTIVE		= "SPEA2MultiObjective";
		private const String PROFILE_FILE_ADAPTIVEPARSIMONY			= "AdaptiveParsimony";
		/// <summary>
		/// Load the fitness settings from the XML document
		/// </summary>
		/// <param name="xmlRoot">Element the the settings are contained within</param>
		/// <returns>True/False upon success or failure</returns>
		private bool LoadFitnessOptions(XmlElement xmlRoot)
		{
			try
			{
				//
				// Fitness function is actually stored in the database
				FitnessFunctionID=Convert.ToInt32(GPDatabaseUtils.FieldValue(this.ProfileID, "tblModelProfile", "FitnessFunctionID"));

				//
				// SPEA2 Multi-Objective setting.  Placed in a try-catch block because legacy profiles will
				// not have this setting, so we need a default setting of false to be set.
				XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_SPEA2MULTIOBJECTIVE);
				try
				{
					this.ModelingProfile.SPEA2MultiObjective = Convert.ToBoolean(xmlNode.InnerText);
				}
				catch
				{
					this.ModelingProfile.SPEA2MultiObjective = false;
				}

				//
				// Adaptive parsimony setting
				xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_ADAPTIVEPARSIMONY);
				this.ModelingProfile.AdaptiveParsimony = Convert.ToBoolean(xmlNode.InnerText);
			}
			catch (Exception)
			{
				return false;
			}
			
		return true;
		}
		
		/// <summary>
		/// Save the fitness settings
		/// </summary>
		/// <param name="xmlWriter">XML stream to write to</param>
		/// <returns>True/False upon success or failure</returns>
		private bool SaveFitnessOptions(XmlTextWriter xmlWriter)
		{
			//
			// Save the fitness type - It is stored in the database
			GPDatabaseUtils.UpdateField(this.ProfileID, "tblModelProfile", "FitnessFunctionID", this.FitnessFunctionID);

			//
			// Save the SPEA2 Multi-Objective setting
			xmlWriter.WriteStartElement(PROFILE_FILE_SPEA2MULTIOBJECTIVE);
			xmlWriter.WriteValue(this.ModelingProfile.SPEA2MultiObjective.ToString());
			xmlWriter.WriteEndElement();

			//
			// Save the adaptive parsimony setting
			xmlWriter.WriteStartElement(PROFILE_FILE_ADAPTIVEPARSIMONY);
			xmlWriter.WriteValue(this.ModelingProfile.AdaptiveParsimony.ToString());
			xmlWriter.WriteEndElement();
			
		return true;
		}
		
		private const String PROFILE_FILE_REPRODUCTION					="Reproduction";
		
		/// <summary>
		/// Load the program reproduction settings
		/// </summary>
		/// <param name="xmlRoot"></param>
		/// <returns></returns>
		private bool LoadReproduction(XmlElement xmlRoot)
		{
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_REPRODUCTION);
			String sValue = xmlNode.InnerText;

			if (sValue == GPEnums.Reproduction.Tournament.ToString())
			{
				ModelingProfile.Reproduction = GPEnums.Reproduction.Tournament;
			}
			else if (sValue == GPEnums.Reproduction.OverSelection.ToString())
			{
				ModelingProfile.Reproduction = GPEnums.Reproduction.OverSelection;
			}
			else
			{
				return false;
			}
			
		return true;
		}
		
		/// <summary>
		/// Save the program reproduction settings
		/// </summary>
		/// <param name="xmlWriter"></param>
		/// <returns></returns>
		private bool SaveReproduction(XmlTextWriter xmlWriter)
		{

			xmlWriter.WriteStartElement(PROFILE_FILE_REPRODUCTION);
			xmlWriter.WriteValue(ModelingProfile.Reproduction.ToString());
			xmlWriter.WriteEndElement();

		return true;
		}	
		
		private const String PROFILE_FILE_POPULATIONINIT			="PopulationInit";
		private const String PROFILE_FILE_POPULATIONINIT_METHOD		="Method";
		private const String PROFILE_FILE_POPULATIONINIT_MAXDEPTH	="MaxDepth";
		
		//
		// <Population Init>
		private bool LoadPopulationInit(XmlElement xmlRoot)
		{
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_POPULATIONINIT);

			//
			// Grab the Method
			XmlNode xmlNodeMethod = xmlNode.SelectSingleNode(PROFILE_FILE_POPULATIONINIT_METHOD);
			String sMethod = xmlNodeMethod.InnerText;

			if (sMethod == GPEnums.PopulationInit.Full.ToString())
			{
				this.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Full;
			}
			else if (sMethod == GPEnums.PopulationInit.Grow.ToString())
			{
				this.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Grow;
			}
			else if (sMethod == GPEnums.PopulationInit.Ramped.ToString())
			{
				this.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Ramped;
			}

			//
			// Grab the Max Depth
			XmlNode xmlNodeMaxDepth = xmlNode.SelectSingleNode(PROFILE_FILE_POPULATIONINIT_MAXDEPTH);
			String sMaxDepth = xmlNodeMaxDepth.InnerText;
			this.ModelingProfile.PopulationInitialDepth = Convert.ToInt32(sMaxDepth);

		return true;
		}
		
		//
		// <Population Init>
		private bool SavePopulationInit(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_POPULATIONINIT);

			xmlWriter.WriteStartElement(PROFILE_FILE_POPULATIONINIT_METHOD);
			xmlWriter.WriteValue(this.ModelingProfile.PopulationBuild.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_POPULATIONINIT_MAXDEPTH);
			xmlWriter.WriteValue(this.ModelingProfile.PopulationInitialDepth);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();

			return true;
		}
		
		private const String PROFILE_FILE_GPSTRUCTURE				= "GPStructure";
		private const String PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TRANSFERCOUNT			= "DistributedTransferCount";
		private const String PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY				= "Topology";
		private const String PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY_STARPERCENT	= "StarPercent";
		private const String PROFILE_FILE_GPSTRUCTURE_REPRODUCTION	= "Reproduction";
		private const String PROFILE_FILE_GPSTRUCTURE_MUTATION		= "Mutation";
		private const String PROFILE_FILE_GPSTRUCTURE_CROSSOVER		= "Crossover";
		private const String PROFILE_FILE_GPSTRUCTURE_POPULATION	= "Population";
		private const String PROFILE_FILE_GPSTRUCTURE_ADF			= "ADF";
		private const String PROFILE_FILE_GPSTRUCTURE_ADL			= "ADL";
		private const String PROFILE_FILE_GPSTRUCTURE_ADR			= "ADR";
		private const String PROFILE_FILE_GPSTRUCTURE_USEMEMORY		= "UseMemory";
		private const String PROFILE_FILE_GPSTRUCTURE_MEMORYCOUNT	= "MemoryCount";
		//
		// <GP Structure>
		private bool LoadGPStructure(XmlElement xmlRoot)
		{
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_GPSTRUCTURE);

			XmlNodeList xmlNodeList = xmlNode.ChildNodes;
			foreach (XmlNode xmlChildNode in xmlNodeList)
			{
				//
				// Extract the node type and the value stored in the element
				String sValueType = xmlChildNode.Name;
				String sValue = xmlChildNode.InnerText;

				if (sValueType == PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TRANSFERCOUNT)
				{
					this.ModelingProfile.DistributedTransferCount = Convert.ToInt32(sValue);
				}
				else if (sValueType == PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY)
				{
					if (sValue == GPEnums.DistributedTopology.Ring.ToString())
					{
						this.ModelingProfile.DistributedTopology = GPEnums.DistributedTopology.Ring;
					}
					else if (sValue == GPEnums.DistributedTopology.Star.ToString())
					{
						this.ModelingProfile.DistributedTopology = GPEnums.DistributedTopology.Star;
					}
				}
				else if (sValueType == PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY_STARPERCENT)
				{
					this.ModelingProfile.DistributedTopologyStarPercent = Convert.ToInt32(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_REPRODUCTION))
				{
					this.ModelingProfile.ProbabilityReproduction = Convert.ToInt16(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_MUTATION))
				{
					this.ModelingProfile.ProbabilityMutation = Convert.ToInt16(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_CROSSOVER))
				{
					this.ModelingProfile.ProbabilityCrossover = Convert.ToInt16(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_POPULATION))
				{
					this.ModelingProfile.PopulationSize = Convert.ToInt32(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_ADF))
				{
					m_ADFSet.Add(Convert.ToByte(sValue));
				}
				else if (sValueType.EndsWith(PROFILE_FILE_GPSTRUCTURE_ADL))
				{
					m_ADLSet.Add(Convert.ToByte(sValue));
				}
				else if (sValueType.EndsWith(PROFILE_FILE_GPSTRUCTURE_ADR))
				{
					m_ADRSet.Add(Convert.ToByte(sValue));
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_USEMEMORY))
				{
					ModelingProfile.UseMemory = Convert.ToBoolean(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GPSTRUCTURE_MEMORYCOUNT))
				{
					ModelingProfile.CountMemory = Convert.ToInt16(sValue);
				}
			}
			
		return true;
		}
		
		//
		// <GP Structure>
		private bool SaveGPStructure(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE);

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TRANSFERCOUNT);
			xmlWriter.WriteValue(this.ModelingProfile.DistributedTransferCount);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY);
			xmlWriter.WriteValue(this.ModelingProfile.DistributedTopology.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_DISTRIBUTED_TOPOLOGY_STARPERCENT);
			xmlWriter.WriteValue(this.ModelingProfile.DistributedTopologyStarPercent);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_REPRODUCTION);
			xmlWriter.WriteValue(this.ModelingProfile.ProbabilityReproduction);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_MUTATION);
			xmlWriter.WriteValue(this.ModelingProfile.ProbabilityMutation);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_CROSSOVER);
			xmlWriter.WriteValue(this.ModelingProfile.ProbabilityCrossover);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_POPULATION);
			xmlWriter.WriteValue(this.ModelingProfile.PopulationSize);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_USEMEMORY);
			xmlWriter.WriteValue(ModelingProfile.UseMemory);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_MEMORYCOUNT);
			xmlWriter.WriteValue(ModelingProfile.CountMemory);
			xmlWriter.WriteEndElement();

			//
			// Automatically Defined Functions
			foreach (int adf in m_ADFSet)
			{
				xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_ADF);
				xmlWriter.WriteValue(adf);
				xmlWriter.WriteEndElement();
			}

			//
			// Automatically Defined Loops
			foreach (int adl in m_ADLSet)
			{
				xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_ADL);
				xmlWriter.WriteValue(adl);
				xmlWriter.WriteEndElement();
			}

			//
			// Automatically Defined Recursions
			foreach (int adr in m_ADRSet)
			{
				xmlWriter.WriteStartElement(PROFILE_FILE_GPSTRUCTURE_ADR);
				xmlWriter.WriteValue(adr);
				xmlWriter.WriteEndElement();
			}

			xmlWriter.WriteEndElement();
			
		return true;
		}	
		
		private const String PROFILE_FILE_GENERATIONOPT							="GenerationOptions";
		private const String PROFILE_FILE_GENERATIONOPT_USEMAXNUMBER			="UseMaxNumber";
		private const String PROFILE_FILE_GENERATIONOPT_MAXNUMBER				="MaxNumber";
		private const String PROFILE_FILE_GENERATIONOPT_USERAWFITNESS0			="UseRawFitness0";
		private const String PROFILE_FILE_GENERATIONOPT_USEHITSMAXED			="UseHitsMaxed";

		//
		// <Generation Options>
		private bool LoadGenerationOptions(XmlElement xmlRoot)
		{
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_GENERATIONOPT);

			XmlNodeList xmlNodeList = xmlNode.ChildNodes;
			foreach (XmlNode xmlChildNode in xmlNodeList)
			{
				//
				// Extract the node type and the value stored in the element
				String sValueType = xmlChildNode.Name;
				String sValue = xmlChildNode.InnerText;

				if (sValueType.Equals(PROFILE_FILE_GENERATIONOPT_USEMAXNUMBER))
				{
					m_useMaxNumber=Convert.ToBoolean(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GENERATIONOPT_MAXNUMBER))
				{
					m_maxNumber=Convert.ToInt32(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GENERATIONOPT_USERAWFITNESS0))
				{
					m_useRawFitness0=Convert.ToBoolean(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_GENERATIONOPT_USEHITSMAXED))
				{
					m_useHitsMaxed=Convert.ToBoolean(sValue);
				}
			}
			
		return true;
		}
		
		//
		// <Generation Options>
		private bool SaveGenerationOptions(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_GENERATIONOPT);

			xmlWriter.WriteStartElement(PROFILE_FILE_GENERATIONOPT_USEMAXNUMBER);
			xmlWriter.WriteValue(m_useMaxNumber);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GENERATIONOPT_MAXNUMBER);
			xmlWriter.WriteValue(m_maxNumber);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GENERATIONOPT_USERAWFITNESS0);
			xmlWriter.WriteValue(m_useRawFitness0);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_GENERATIONOPT_USEHITSMAXED);
			xmlWriter.WriteValue(m_useHitsMaxed);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
			
		return true;
		}	
		
		private const String PROFILE_FILE_FUNCTIONSET				="FunctionSet";
		//
		// <Function Set>
		private bool LoadFunctionSet(XmlElement xmlRoot)
		{
			//
			// Empty any current functions from the set
			m_FunctionSet.Clear();
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_FUNCTIONSET);

			XmlNodeList xmlNodeList = xmlNode.ChildNodes;
			foreach (XmlNode xmlChildNode in xmlNodeList)
			{
				//
				// Extract the node type and the value stored in the element
				String sValueType = xmlChildNode.Name;
				String sValue = xmlChildNode.InnerText;

				this.FunctionSet.Add(sValue);
			}

			return true;
		}
		
		//
		// <Function Set>
		private bool SaveFunctionSet(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_FUNCTIONSET);
			
			foreach (string Function in this.FunctionSet)
			{
				xmlWriter.WriteStartElement("Value");
				xmlWriter.WriteValue(Function);
				xmlWriter.WriteEndElement();
			}

			xmlWriter.WriteEndElement();
			
		return true;
		}	
		
		private const String PROFILE_FILE_TERMINALSET				="TerminalSet";
		private const String PROFILE_FILE_FUNCTIONSET_RNDINTEGER	="RandomInteger";
		private const String PROFILE_FILE_FUNCTIONSET_RNDINTEGERMIN = "IntegerMin";
		private const String PROFILE_FILE_FUNCTIONSET_RNDINTEGERMAX = "IntegerMax";
		private const String PROFILE_FILE_FUNCTIONSET_RNDDOUBLE		="RandomDouble";
		
		//
		// <Terminal Set>
		private bool LoadTerminalSet(XmlElement xmlRoot)
		{
			XmlNode xmlNode = xmlRoot.SelectSingleNode(PROFILE_FILE_TERMINALSET);

			XmlNodeList xmlNodeList = xmlNode.ChildNodes;
			foreach (XmlNode xmlChildNode in xmlNodeList)
			{
				//
				// Extract the node type and the value stored in the element
				String sValueType = xmlChildNode.Name;
				String sValue = xmlChildNode.InnerText;

				if (sValueType.Equals(PROFILE_FILE_FUNCTIONSET_RNDINTEGER))
				{
					ModelingProfile.UseRndInteger = Convert.ToBoolean(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_FUNCTIONSET_RNDINTEGERMIN))
				{
					ModelingProfile.IntegerMin = Convert.ToInt32(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_FUNCTIONSET_RNDINTEGERMAX))
				{
					ModelingProfile.IntegerMax = Convert.ToInt32(sValue);
				}
				else if (sValueType.Equals(PROFILE_FILE_FUNCTIONSET_RNDDOUBLE))
				{
					ModelingProfile.UseRndDouble = Convert.ToBoolean(sValue);
				}
			}
			
		return true;
		}
		
		//
		// <Terminal Set>
		private bool SaveTerminalSet(XmlTextWriter xmlWriter)
		{
			xmlWriter.WriteStartElement(PROFILE_FILE_TERMINALSET);

			xmlWriter.WriteStartElement(PROFILE_FILE_FUNCTIONSET_RNDINTEGER);
			xmlWriter.WriteValue(ModelingProfile.UseRndInteger);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_FUNCTIONSET_RNDINTEGERMIN);
			xmlWriter.WriteValue(ModelingProfile.IntegerMin);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_FUNCTIONSET_RNDINTEGERMAX);
			xmlWriter.WriteValue(ModelingProfile.IntegerMax);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement(PROFILE_FILE_FUNCTIONSET_RNDDOUBLE);
			xmlWriter.WriteValue(ModelingProfile.UseRndDouble);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
			
		return true;
		}	
		
		#endregion

		/// <summary>
		/// Checks to see if this profile has any generated programs that
		/// are associated with it.
		/// </summary>
		public bool ProfileInUse
		{
			get
			{
				OleDbConnection con = GPDatabaseUtils.Connect();

				string sSQL = "SELECT DBCode FROM tblProgram ";
				sSQL += "WHERE ModelProfileID = " + this.ProfileID;
				OleDbDataAdapter daFiles = new OleDbDataAdapter(sSQL, con);
				DataSet dSet = new DataSet();
				daFiles.Fill(dSet);

				con.Close();

				if (dSet.Tables[0].Rows.Count > 0)
					return true;

				return false;
			}
		}

		internal enum Validation
		{
			Valid,
			FunctionSet,
			Probability
		}
		/// <summary>
		/// Validates whether or not this model is complete and can be used for 
		/// modeling.
		/// </summary>
		/// <returns>True if it is value, False otherwise</returns>
		internal Validation ValidateForModeling()
		{
			//
			// Check the number of functions
			if (this.FunctionSet.Count == 0)
			{
				return Validation.FunctionSet;
			}

			//
			// Ensure the operators add up to 1.0 exactly, more is actually okay
			int ProbabilityTotal = 0;
			ProbabilityTotal += this.ModelingProfile.ProbabilityCrossover;
			ProbabilityTotal += this.ModelingProfile.ProbabilityMutation;
			ProbabilityTotal += this.ModelingProfile.ProbabilityReproduction;
			if (ProbabilityTotal != 100)
			{
				return Validation.Probability;
			}

			//
			// Ensure the fitness function exists


			return Validation.Valid;
		}
	}
}
