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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using GPStudio.Interfaces;
using GPStudio.Shared;
using System.Xml;
using System.Runtime.Remoting.Lifetime;

namespace GPStudio.Client
{
	/// <summary>
	/// The purpose of this class is to manage the set of distributed
	/// servers in use by the application.  It is indended to be used
	/// as a singleton object.
	/// </summary>
	public class ServerManagerSingleton : IEnumerable
	{
		/// <summary>
		/// Your basic default constructor, get the comm channels registered
		/// </summary>
		public ServerManagerSingleton()
		{
			RegisterChannels();

			m_Servers = new SortedDictionary<string, IGPServer>();
			m_Descriptions = new SortedDictionary<string, string>();
			m_Sponsors = new SortedDictionary<string, GPServerClientSponsor>();
		}

		#region IEnumerable & Related

		/// <summary>
		/// Provide an enumerator over the registered IGPServer interfaces
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			foreach (IGPServer iGPServer in m_Servers.Values)
			{
				yield return iGPServer;
			}
		}

		/// <summary>
		/// A count of the number of registered servers
		/// </summary>
		public int Count
		{
			get { return m_Servers.Count; }
		}

		#endregion

		/// <summary>
		/// Indicator for the TCP channel
		/// </summary>
		private bool m_TcpRegistered = false;
		/// <summary>
		/// Indicator for the HTTP channel
		/// </summary>
		private bool m_HttpRegistered = false;

		/// <summary>
		/// Register both types of channels - They throw a RemotingException
		/// if something doesn't work.
		/// We have to do a full registration of these channels because the client
		/// registers an ISponsor interface with the remote objects, meaning that the
		/// remote servers must be able to communicate back to the client.
		/// </summary>
		public void RegisterChannels()
		{
			IDictionary props = new Hashtable();
			props["timeout"] = DISTRIBUTED_TIMEOUT;
			props["port"] = 0;	// Let .NET choose an available port
			try
			{
				BinaryServerFormatterSinkProvider prov = new BinaryServerFormatterSinkProvider();
				prov.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				TcpChannel Channel = new TcpChannel(props, null, prov);
				ChannelServices.RegisterChannel(Channel, false);

			}
			catch
			{
				m_TcpRegistered = false;
			}
			m_TcpRegistered = true;

			try
			{
				SoapServerFormatterSinkProvider prov = new SoapServerFormatterSinkProvider();
				prov.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				HttpChannel Channel = new HttpChannel(props, null, prov);
				ChannelServices.RegisterChannel(Channel, false);
			}
			catch
			{
				m_HttpRegistered = false;
			}
			m_HttpRegistered = true;
		}

		/// <summary>
		/// Define the XML strings used in the server configuration file
		/// </summary>
		public const String XML_DOCUMENTROOT = "Configuration";
		public const String XML_SERVER = "Server";
		public const String XML_INTERNETNAME = "InternetName";
		public const String XML_DESCRIPTION = "Description";
		public const String XML_PORT = "Port";
		public const String XML_PROTOCOL = "Protocol";
		public const String XML_ENABLED = "Enabled";

		/// <summary>
		/// Name and port for the local computer.
		/// </summary>
		public const String LOCALHOST = "localhost";
		public const String LOCALPORT = "1201";

		/// <summary>
		/// Timeout setting for remote methods to repond.  Of particular interest is
		/// the time it takes to validate a remote server, the default timeout setting
		/// is much too long, this makes the validation happen much more quickly.
		/// </summary>
		private const int DISTRIBUTED_TIMEOUT = 10000;	// 10000 milliseconds TODO: Make this a configuration setting

		/// <summary>
		/// Registers all servers from the configuration file
		/// </summary>
		/// <param name="XmlFile">The configuration file to read from</param>
		public void RegisterServersFromXml(String XmlFile)
		{
			//
			// Remove any existing servers
			m_Servers.Clear();
			m_Descriptions.Clear();
			m_Sponsors.Clear();

			//
			// Open the configuration file and load the servers
			String ServersFile = XmlFile;
			XmlDocument xmlProgram = new XmlDocument();
			xmlProgram.Load(ServersFile);

			XmlNode Root = xmlProgram.DocumentElement;
			//
			// Work through the list of Server nodes in the file
			XmlNodeList ServerList = Root.SelectNodes(XML_SERVER);
			foreach (XmlNode xmlServer in ServerList)
			{
				String Description = xmlServer.SelectSingleNode(XML_DESCRIPTION).InnerText;
				String HostName = xmlServer.SelectSingleNode(XML_INTERNETNAME).InnerText;
				String Port = xmlServer.SelectSingleNode(XML_PORT).InnerText;
				String Protocol = xmlServer.SelectSingleNode(XML_PROTOCOL).InnerText;
				bool Enabled = Convert.ToBoolean(xmlServer.SelectSingleNode(XML_ENABLED).InnerText);

				//
				// Only register those servers that have been selected by the user
				// and that pass validation.
				if (Enabled && ValidateServer(HostName,Port,Protocol,Description))
				{
					RegisterServer(HostName, Port, Protocol,Description);
				}
			}

			//
			// Remember to add the local server back in
			RegisterServer(LOCALHOST,LOCALPORT,GPEnums.CHANNEL_TCP,"Local Computer");
		}

		/// <summary>
		/// Add a new server to the list of available servers.
		/// </summary>
		/// <param name="HostName"></param>
		/// <param name="Port"></param>
		/// <param name="Protocol"></param>
		/// <returns></returns>
		public bool RegisterServer(String HostName,String Port,String Protocol,String Description)
		{
			//
			// Check to see if we already have this server registered
			if (m_Servers.ContainsKey(HostName + Port))
			{
				return true;
			}

			//
			// Make sure the Protocol channel has been successfully registered
			if (Protocol.ToUpper() == GPEnums.CHANNEL_TCP && !m_TcpRegistered)
			{
				return false;
			}
			if (Protocol.ToUpper() == GPEnums.CHANNEL_HTTP && !m_HttpRegistered)
			{
				return false;
			}

			//
			// Obtain the IGPServer interface 
			String CompleteHost = Protocol.ToLower() + "://" + HostName + ":" + Port + "/"+GPEnums.SERVER_SOAPNAME;
			object obj=Activator.GetObject(
				typeof(IGPServer),
				CompleteHost);

			IGPServer iGPServer = (IGPServer)obj;

			//
			// Validate this server is available
			if (!ValidateServer(iGPServer))
			{
				return false;
			}

			//
			// Assign a sponsor for the remote object
			GPServerClientSponsor Sponsor = fmMain.SponsorServer.Create("GPServer Singleton");
			ILease LeaseInfo=(ILease)((MarshalByRefObject)iGPServer).GetLifetimeService();
			LeaseInfo.Register(Sponsor);
			m_Sponsors.Add(HostName + Port, Sponsor);

			//
			// Add this server interface to our singleton
			m_Servers.Add(HostName + Port, iGPServer);
			m_Descriptions.Add(HostName+Port, Description);

			return true;
		}

		/// <summary>
		/// Collection of registered servers.  The key is HostName+Port,
		/// the value is a reference to the server interface.
		/// </summary>
		private SortedDictionary<String, IGPServer> m_Servers;
		private SortedDictionary<String, String> m_Descriptions;
		private SortedDictionary<String, GPServerClientSponsor> m_Sponsors;

		/// <summary>
		/// Remove the indicated server from the set
		/// </summary>
		/// <param name="HostName"></param>
		/// <param name="Port"></param>
		/// <returns></returns>
		public bool RemoveServer(String HostName, String Port)
		{
			//
			// Don't ever allow the local server to be removed
			if (HostName.ToUpper() == LOCALHOST.ToUpper())
			{
				return false;
			}

			//
			// Make sure the key exists before trying to remove
			if (m_Servers.ContainsKey(HostName + Port))
			{
				m_Sponsors.Remove(HostName + Port);
				m_Descriptions.Remove(HostName + Port);
				return m_Servers.Remove(HostName + Port);
			}
			return true;	// It's okay if it didn't exist
		}

		/// <summary>
		/// Public method that allows other parts of the program to see if a
		/// particular distributed server validates (is running).
		/// </summary>
		/// <param name="HostName"></param>
		/// <param name="Port"></param>
		/// <param name="Protocol"></param>
		/// <returns>True/False upon success or failure</returns>
		public bool ValidateServer(String HostName, String Port, String Protocol,String Description)
		{
			//
			// Check to see if we already have a reference to this server
			if (m_Servers.ContainsKey(HostName + Port))
			{
				return true;
			}

			//
			// Attempt to register, it contains all the details of getting the
			// server interface created and validated, so just reuse.
			if (!RegisterServer(HostName, Port, Protocol,Description))
			{
				return false;
			}

			//
			// Unregister, we are only temporarily testing for validation
			RemoveServer(HostName, Port);

			return true;
		}

		/// <summary>
		/// Call the Validate() method for IGPServer to find out if this server
		/// is actually up and running.
		/// </summary>
		/// <param name="iGPServer">Interface to the server to test</param>
		/// <returns>True/False upon success or failure</returns>
		private bool ValidateServer(IGPServer iGPServer)
		{

			try
			{
				if (iGPServer.Validate() != GPEnums.SERVER_VERSION)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Indexer that allows access to the distributed server interfaces
		/// </summary>
		/// <param name="HostName"></param>
		/// <param name="Port"></param>
		/// <returns>Reference to the server IGPServer interface</returns>
		public IGPServer this[String HostName,String Port]
		{
			get
			{
				return m_Servers[HostName + Port];
			}
		}

		/// <summary>
		/// Property that allows quick access to the localhost server
		/// </summary>
		public IGPServer LocalServer
		{
			get { return this[LOCALHOST,LOCALPORT]; }
		}

		/// <summary>
		/// Property that allows access to the list of registered servers.
		/// </summary>
		public SortedDictionary<string, IGPServer> Servers
		{
			get { return m_Servers; }
		}

		/// <summary>
		/// Property that allows access to the server descriptions.  The
		/// key is "Hostname+Port".
		/// </summary>
		public SortedDictionary<String, String> Descriptions
		{
			get { return m_Descriptions; }
		}

		/// <summary>
		/// Property that allows access to the client side sponsors for each of the servers.
		/// These sponsors can be used to sponsor other interfaces created at each server.
		/// </summary>
		public SortedDictionary<string, GPServerClientSponsor> Sponsors
		{
			get { return m_Sponsors; }
		}

	}
}
