using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;	// RunInstallerAttribute
using System.Diagnostics;		// EventLog
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Server
{
	/// <summary>
	/// Derive GPServer from ServiceBase, because GPServer is to be run as
	/// a Windows Service.
	/// </summary>
	class GPServerMain  : ServiceBase
	{
		public const String SERVICENAME = "GPServer";
		private const String SETTINGS_GPSERVERPROTOCOL_STR = "GPServerProtocol";
		private const String SETTINGS_GPSERVERPORT_STR = "GPServerPort";

		static void Main(string[] args)
		{
			//
			// Commented out code is only when run as a console program
			InitializeProtocol();

			Console.WriteLine("Press <Enter> to terminate GPServer...");
			Console.ReadLine();

			//
			// This line is for running as a Windows Service
			//ServiceBase.Run(new GPServerMain());
		}

		/// <summary>
		/// Default constructor, just set the service name
		/// </summary>
		public GPServerMain()
		{
			this.ServiceName = SERVICENAME;
		}

		/// <summary>
		/// ServiceBase OnStart implementation
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStart(string[] args)
		{
			//
			// Prepare the service protocol
			// TODO: Had one user have a problem with the service starting because the event log recording
			// failed.  So have commented out the code for now, while I think about how else to possibly handle this.
			if (!InitializeProtocol())
			{
				//
				// Report a problem getting started
				//EventLog evt = new EventLog("Application");
				//evt.Source = SERVICENAME;
				//evt.WriteEntry("GPServer failed to initialize");
			}
			else
			{
				//EventLog evt = new EventLog("Application");
				//evt.Source = SERVICENAME;
				//evt.WriteEntry("GPServer correctly initialized");
			}
		}

		/// <summary>
		/// Sets the transport protocol, according to the server configuration file
		/// </summary>
		private static bool InitializeProtocol()
		{
			try
			{
				Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

				//
				// Prepare the listening port
				int ServerPort = Convert.ToInt32(config.AppSettings.Settings[SETTINGS_GPSERVERPORT_STR].Value);
				IDictionary props = new Hashtable();
				props["port"] = ServerPort;

				//
				// Register the transport channel, either TCP or HTTP
				if (config.AppSettings.Settings[SETTINGS_GPSERVERPROTOCOL_STR].Value.ToUpper() == GPEnums.CHANNEL_TCP)
				{
#if GPLOG
					GPLog.Report("Attempting to register the TCP channel...",true);
#endif
					BinaryServerFormatterSinkProvider prov = new BinaryServerFormatterSinkProvider();
					prov.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
					TcpChannel Channel = new TcpChannel(props, null, prov);
					ChannelServices.RegisterChannel(Channel, false);
#if GPLOG
					GPLog.ReportLine("succeeded!",false);
#endif
				}
				else if (config.AppSettings.Settings[SETTINGS_GPSERVERPROTOCOL_STR].Value.ToUpper() == GPEnums.CHANNEL_HTTP)
				{
#if GPLOG
					GPLog.Report("Attempting to register the HTTP channel...",true);
#endif
					SoapServerFormatterSinkProvider prov = new SoapServerFormatterSinkProvider();
					prov.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
					HttpChannel Channel = new HttpChannel(props, null, prov);
					ChannelServices.RegisterChannel(Channel, false);
#if GPLOG
					GPLog.ReportLine("succeeded!",false);
#endif
				}
				else
				{
					return false;
				}
#if GPLOG
				GPLog.Report("Attempting to register well known service type...",true);
#endif
				//
				// Activate the listening object!
				RemotingConfiguration.RegisterWellKnownServiceType(
					typeof(GPServer),
					GPEnums.SERVER_SOAPNAME,
					WellKnownObjectMode.Singleton);
#if GPLOG
				GPLog.ReportLine("succeeded!",false);
#endif
			}
			catch (Exception)
			{
#if GPLOG
				GPLog.ReportLine("failure!",false);
#endif
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Windows Service Installer class for the GPServer service
	/// </summary>
	[RunInstallerAttribute(true)]
	public class GPServerInstaller : Installer
	{
		private ServiceInstaller serviceInstaller;
		private ServiceProcessInstaller processInstaller;

		public GPServerInstaller()
		{
			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();

			processInstaller.Account = ServiceAccount.LocalSystem;

			serviceInstaller.DisplayName = GPServerMain.SERVICENAME;
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			serviceInstaller.ServiceName = GPServerMain.SERVICENAME;

			Installers.Add(serviceInstaller);
			Installers.Add(processInstaller);
		}
	}
}
