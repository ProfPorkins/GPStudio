using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Runtime.Remoting.Lifetime;
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// This is a factory class that generates all the various interfaces/objects
	/// a client may want to use.
	/// </summary>
	public class GPServer: MarshalByRefObject,IGPServer
	{
		/// <summary>
		/// Public constructor, no longer does anything.
		/// </summary>
		public GPServer()
		{
		}

		/// <summary>
		/// This is used by a client to validate it has access to the GPServer
		/// remoting object.
		/// </summary>
		/// <returns>Number indiating the version of the server</returns>
		public int Validate()
		{
			return GPEnums.SERVER_VERSION;
		}

		public IGPModeler Modeler
		{
			get
			{
				return new GPModelerServer();
			}
		}
		public IGPCompiler Compiler
		{
			get
			{
				return new GPCompilerServer();
			}
		}

		public IGPCustomFitness CustomFitness
		{
			get
			{
				return new GPCustomFitnessServer();
			}
		}

		public IGPFunctionSet FunctionSet
		{
			get
			{
				return new GPFunctionServer();
			}
		}

		public IGPProgram Program
		{
			get
			{
				return new GPProgramServer();
			}
		}

		public IGPLanguageWriter LanguageWriter
		{
			get
			{
				return new GPLanguageWriterServer();
			}
		}

		#region Distributed Computing

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

		#endregion

	}
}
