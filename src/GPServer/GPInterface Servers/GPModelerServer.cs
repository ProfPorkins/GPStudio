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
using System.IO;
using System.Runtime.Remoting.Lifetime;
using GPStudio.Interfaces;
using GPStudio.Shared;
using System.IO.Compression;

namespace GPStudio.Server
{
	/// <summary>
	/// This class provides an implementation of the IGPModler interface.
	/// </summary>
	public class GPModelerServer : MarshalByRefObject, IGPModeler
	{
		/// <summary>
		/// Basic default constructor
		/// </summary>
		public GPModelerServer()
		{
			m_FunctionSet = null;
			m_Training = null;

			m_ADFSet = new List<byte>();
			m_ADLSet = new List<byte>();
			m_ADRSet = new List<byte>();

			m_Servers = new List<IGPModeler>();
		}

		/// <summary>
		/// Prepare the lease sponsorship settings
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			ILease LeaseInfo = (ILease)base.InitializeLifetimeService();

			LeaseInfo.InitialLeaseTime = TimeSpan.FromMinutes(GPEnums.REMOTING_RENEWAL_MINUTES);
			LeaseInfo.RenewOnCallTime = TimeSpan.FromMinutes(GPEnums.REMOTING_RENEWAL_MINUTES);
			LeaseInfo.SponsorshipTimeout = TimeSpan.FromMinutes(GPEnums.REMOTING_TIMEOUT_MINUTES);

			return LeaseInfo;
		}

		/// <summary>
		/// Interface to available functions for use in modeling
		/// </summary>
		public IGPFunctionSet FunctionSet
		{
			set { m_FunctionSet = value; }
			get { return m_FunctionSet; }
		}
		private IGPFunctionSet m_FunctionSet;

		/// <summary>
		/// Interface to the training data
		/// </summary>
		public GPTrainingData Training
		{
			set { m_Training = value; }
			get { return m_Training; }
		}
		private GPTrainingData m_Training;

		/// <summary>
		/// String representation of the fitness function is sent to the server
		/// and is compiled and prepared for use right here.
		/// </summary>
		public String FitnessFunction
		{
			set
			{
				//
				// Write the C# class for the function
				GPCompilerServer Compiler = new GPCompilerServer();
				String CSharpCode = Compiler.WriteFitnessClass(value);

				//
				// Compile the class
				String[] Errors=null;
				m_FitnessFunction = Compiler.CompileFitnessClass(CSharpCode,out Errors);
			}
		}
		private GPFitnessCustomBase m_FitnessFunction;

		/// <summary>
		/// Property that exposes the fitness function computation class
		/// </summary>
		public GPFitnessCustomBase Fitness
		{
			get { return m_FitnessFunction; }
		}
		private GPFitness m_Fitness;

		
		/// <summary>
		/// This object contains the parameters that describe how to manage the modeling
		/// </summary>
		public GPModelingProfile Profile
		{
			set 
			{ 
				m_Profile = value;
				//
				// The profile has a count of the number of input terminals, let's
				// create the collection the GP code needs.
				m_TerminalSet=new List<GPEnums.TerminalType>(m_Training.Columns);
				for (short Terminal=0; Terminal<m_Training.Columns; Terminal++)
				{
					this.TerminalSet.Add(GPEnums.TerminalType.UserDefined);
				}
				if (m_Profile.UseRndDouble)
				{
					this.TerminalSet.Add(GPEnums.TerminalType.RandomDouble);
				}
				if (m_Profile.UseRndInteger)
				{
					this.TerminalSet.Add(GPEnums.TerminalType.RandomInteger);
				}
			}

			get { return m_Profile; }
		}
		private GPModelingProfile m_Profile;

		/// <summary>
		/// Collection of the input terminals defined for this profile.  This 
		/// collection is created when the profile is assigned to this object.
		/// </summary>
		private List<GPEnums.TerminalType> m_TerminalSet;
		public List<GPEnums.TerminalType> TerminalSet
		{
			get { return m_TerminalSet; }
		}

		/// <summary>
		/// Set of ADFs to evolve
		/// </summary>
		/// <param name="ParamCount"></param>
		public List<byte> ADFSet
		{
			get { return m_ADFSet; }
		}
		private List<byte> m_ADFSet;

		/// <summary>
		/// Specify a new ADF to be evolved
		/// </summary>
		/// <param name="ParamCount"></param>
		public void AddADF(byte ParamCount)
		{
			m_ADFSet.Add(ParamCount);
		}

		/// <summary>
		/// Set of ADLs to evolve
		/// </summary>
		/// <param name="ParamCount"></param>		
		public List<byte> ADLSet
		{
			get { return m_ADLSet; }
		}
		private List<byte> m_ADLSet;

		/// <summary>
		/// Specify a new ADL to be evolved
		/// </summary>
		/// <param name="ParamCount"></param>
		public void AddADL(byte ParamCount)
		{
			m_ADLSet.Add(ParamCount);
		}

		/// <summary>
		/// Set of ADRs to evolve
		/// </summary>
		/// <param name="ParamCount"></param>
		public List<byte> ADRSet
		{
			get { return m_ADRSet; }
		}
		private List<byte> m_ADRSet;
		
		/// <summary>
		/// Specify a new ADR to be evolved
		/// </summary>
		/// <param name="ParamCount"></param>
		public void AddADR(byte ParamCount)
		{
			m_ADRSet.Add(ParamCount);
		}

		/// <summary>
		/// Creates the initial Population on this server.  The Population size
		/// is sent as a parameter, rather than relying upon the profile, because
		/// this might be one of many remote objects and it is only working with
		/// some subset of the Population.
		/// </summary>
		/// <param name="PopulationSize">How many individuals to create initially</param>
		public void InitializePopulation(int PopulationSize)
		{
			//
			// Go ahead and replace, there is no need to have any knowledge of the
			// original Population size;
			m_Profile.PopulationSize = PopulationSize;
			m_Population = new GPPopulation(this);
			m_Population.Build(this.Profile.PopulationBuild, (short)Training.Columns);

			//
			// Need to get this initialized somewhere, so this seems like a good time
			ResetSeedPrograms();
		}

		GPPopulation m_Population;
		private GPProgram m_BestProgram;
		private int m_PopulationComplexityMin;
		private int m_PopulationComplexityMax;
		private int m_PopulationComplexityAve;

		/// <summary>
		/// Compute the fitness of the entire Population and return the value of
		/// the best program in the Population.
		/// </summary>
		public void EvaluateFitness(int Generation)
		{
			//
			// First time through the fitness object doesn't exist, so have to create it
			if (m_Fitness == null)
			{
				m_Fitness = new GPFitness(
					this, 
					this.Training,
					GPEnums.RESULTS_TOLERANCE,
					m_FunctionSet.UseInputHistory);
			}

			m_BestProgram=m_Fitness.Compute(Generation, m_Population);

			//
			// Always simplify a program before it is transmitted.
			m_BestProgram.ConvertToTree(this.FunctionSet, false);
			m_BestProgram.Edit();
			m_BestProgram.ConvertToArray(this.FunctionSet);

			//
			// Obtain the Population stats
			m_Population.ComputeComplexity(
				out m_PopulationComplexityMin, 
				out m_PopulationComplexityMax, 
				out m_PopulationComplexityAve);
		}

		/// <summary>
		/// Add a new distributed node to this server
		/// </summary>
		/// <param name="iServer"></param>
		public void AddModeler(IGPModeler iServer)
		{
			m_Servers.Add(iServer);
		}
		List<IGPModeler> m_Servers;

		/// <summary>
		/// Selects programs from the current Population and transmits them
		/// to connected servers in the topology.
		/// </summary>
		public void DistributePopulation()
		{
			//
			// Don't waste our time if we are connected to nothing.
			if (m_Servers.Count == 0) return;

			//
			// Select some programs to transmit
			List<byte[]> Programs = new List<byte[]>(m_Profile.DistributedTransferCount);
			for (int ProgramCount = 0; ProgramCount < m_Profile.DistributedTransferCount; ProgramCount++)
			{
				Programs.Add(this.SelectProgramTournament);
			}

			switch (m_Profile.DistributedTopology)
			{
				case GPEnums.DistributedTopology.Ring:
					DistributePopulationRing(Programs);
					break;
				case GPEnums.DistributedTopology.Star:
					DistributePopulationStar(Programs);
					break;
			}
		}

		/// <summary>
		/// Select programs and send them to every connected node.
		/// </summary>
		private void DistributePopulationRing(List<byte[]> Programs)
		{
			foreach (IGPModeler iModeler in m_Servers)
			{
				foreach (byte[] Program in Programs)
				{
					iModeler.AddSeedProgram(Program);
				}
			}
		}

		/// <summary>
		/// Select programs and send them, according to the star probablity,
		/// to every connected node.
		/// </summary>
		private void DistributePopulationStar(List<byte[]> Programs)
		{
			//
			// Convert to a double percent for use in random selection
			double Percent = m_Profile.DistributedTopologyStarPercent / 100.0;
			Random rnd = new Random();

			foreach (IGPModeler iModeler in m_Servers)
			{
				//
				// Roll the dice and see if we should transmit to this server or not
				if (Percent >= rnd.NextDouble())
				{
					foreach (byte[] Program in Programs)
					{
						iModeler.AddSeedProgram(Program);
					}
				}
			}
		}

		/// <summary>
		/// Accept a new seed program that should be inserted into the next
		/// Population generation.
		/// </summary>
		/// <param name="xmlBytes"></param>
		public void AddSeedProgram(byte[] xmlBytes)
		{
			String ProgramXML = DecompressProgram(xmlBytes);
			GPProgramReaderXML xmlReader = new GPProgramReaderXML(ProgramXML,m_FunctionSet);
			GPProgram Program = xmlReader.Construct();
			if (Program != null)
			{
				Program.ConvertToArray(m_FunctionSet);
				m_SeedPrograms.Add(Program);
			}
		}

		/// <summary>
		/// Clear any seed programs that have been received
		/// </summary>
		private void ResetSeedPrograms()
		{
			m_SeedPrograms = new List<GPProgram>();
		}

		private List<GPProgram> m_SeedPrograms;
		private GPPopulationFactory m_PopulatonFactory;
		/// <summary>
		/// Create the next generation of programs.
		/// </summary>
		public void ComputeNextGeneration(int Generation)
		{
			if (m_PopulatonFactory == null)
			{
				m_PopulatonFactory = new GPPopulationFactory(this, this.Training.Columns);
			}
			//
			// Create the next generation of programs
			GPPopulation popNew = m_PopulatonFactory.ComputeNext(Generation, m_Population, m_Fitness, m_SeedPrograms);

			//
			// If the Population size changed, update the configuration
			if (popNew.Count != m_Population.Count)
			{
				this.Profile.PopulationSize = popNew.Count;
				Console.WriteLine("GPServer: Memory is low, Population size reduced to ({0})", popNew.Count);

				//
				// Reset the Fitness object
				m_Fitness.TerminateProcessingThreads();
				m_Fitness = null;
			}

			m_Population = popNew;

			//
			// Clear out any seed programs we just used
			ResetSeedPrograms();
		}

		/// <summary>
		/// Instruct the fitness object to immediately terminate the computation
		/// </summary>
		public void Abort()
		{
			m_Fitness.Abort=true;
		}

		/// <summary>
		/// This method instructs the server object to clean up as much memory
		/// as possible.
		/// </summary>
		public void ForceCleanup()
		{
			m_Population = null;
			m_Fitness.TerminateProcessingThreads();
			m_Fitness = null;
			m_Training = null;
			System.GC.Collect();
		}

		/// <summary>
		/// Returns a string representation of the best program
		/// </summary>
		public String BestProgram 
		{
			get
			{
				return CreateProgramString(m_BestProgram);
			}
		}

		/// <summary>
		/// Converts the program to the XML String representation
		/// </summary>
		/// <param name="Program"></param>
		/// <returns>XML String representation of the program</returns>
		private String CreateProgramString(GPProgram Program)
		{
			String ProgramString = "";
			//
			// Serialize the program to an XML string
			Program.ConvertToTree(this.FunctionSet, false);

			GPLanguageWriterXML ProgramWriterXML = new GPLanguageWriterXML(Program, this.Training.TimeSeries);
			using (MemoryStream ms = new MemoryStream())
			{
				ProgramWriterXML.Write(ms);
				ms.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(ms))
				{
					ProgramString = reader.ReadToEnd();
				}
			}

			//
			// Convert it back to an array. I forgot to do this and it kept messing
			// things up as the modeling continued.
			Program.ConvertToArray(this.FunctionSet);

			return ProgramString;
		}

		/// <summary>
		/// Uses tournament selection to return a program from the Population.
		/// The returned program is an array of bytes that are compressed
		/// from the original program XML string.
		/// </summary>
		public byte[] SelectProgramTournament
		{
			get
			{
				int Program=m_Fitness.FitnessSelection.SelectProgramTournament();
				String ProgramString=CreateProgramString(m_Population.Programs[Program]);
				//
				// compress it, saves a lot of time in data transfer
				return CompressString(ProgramString);
			}
		}

		/// <summary>
		/// Compresses the string
		/// </summary>
		/// <param name="ProgramString"></param>
		/// <returns>Compressed array of bytes representing the original string</returns>
		private byte[] CompressString(String ProgramString)
		{
			//
			// Compress the string
			byte[] Compressed = null;
			using (MemoryStream ms = new MemoryStream())
			{
				//
				// Start by writing the string into the compressed string
				GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true);
				byte[] buffer = Encoding.ASCII.GetBytes(ProgramString);
				zs.Write(buffer, 0, buffer.Length);
				zs.Close();

				//
				// We new read the memory string to get back the compressed bytes
				Compressed = new byte[ms.Length];
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(Compressed, 0, Compressed.Length);
			}

			return Compressed;
		}

		/// <summary>
		/// Decompresses a compressed byte string back into an XML program
		/// </summary>
		/// <param name="xmlBytes"></param>
		/// <returns>XML String representation of the program</returns>
		private String DecompressProgram(byte[] xmlBytes)
		{
			//
			// Initialize to some starting size, while we figure out the actual
			// size.  Then, we'll reallocate when we know the final size.
			byte[] buffer = new byte[1024];
			using (MemoryStream ms = new MemoryStream(xmlBytes))
			{
				GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true);

				// Find out how big the decompressed string will be
				int DecompressSize = 0;
				int ReadSize = zs.Read(buffer, 0, 1024);
				while (ReadSize > 0)
				{
					DecompressSize += ReadSize;
					ReadSize = zs.Read(buffer, 0, 1024);
				}
				buffer = new byte[DecompressSize];
				zs.Close();
			}

			//
			// Now we can decompress for real
			using (MemoryStream ms = new MemoryStream(xmlBytes))
			{
				GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true);
				zs.Read(buffer, 0, buffer.Length);
				zs.Close();
			}

			return Encoding.ASCII.GetString(buffer);
		}

		/// <summary>
		/// Chunky interfaces to provide reporting back to the client
		/// </summary>
		#region Program & Population Fitness and Complexity Stats

		/// <summary>
		/// Return some basic stats about the current best program
		/// </summary>
		public void GetBestProgramStats(out double Fitness, out int Hits, out int Complexity)
		{
			Fitness = m_Fitness.BestFitness;
			Hits = m_Fitness.BestFitnessHits;
			Complexity = m_Fitness.BestProgramRef.CountNodes;
		}

		/// <summary>
		/// Return some basic stats about the current generation Population
		/// </summary>
		/// <param name="FitnessMax">Largest fitness error</param>
		/// <param name="FitnessAve">Average fitness error</param>
		/// <param name="ComplexityMin">Smallest program size</param>
		/// <param name="ComplexityMax">Largest program size</param>
		/// <param name="ComplexityAve">Average program size</param>
		public void GetPopulationStats(out double FitnessMax, out double FitnessAve, out int ComplexityMin, out int ComplexityMax, out int ComplexityAve)
		{
			FitnessMax = m_Fitness.FitnessMaximum;
			FitnessAve = m_Fitness.FitnessAverage;

			ComplexityMax = m_PopulationComplexityMax;
			ComplexityMin = m_PopulationComplexityMin;
			ComplexityAve = m_PopulationComplexityAve;
		}

		#endregion
	}
}
