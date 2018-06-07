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
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public partial class fmPreferences : Form
	{
		public fmPreferences()
		{
			InitializeComponent();

			//
			// Obtain the application settings
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			lbDatabase.Text = config.AppSettings.Settings["GPDatabase"].Value;

			//
			// Get the distributed servers displayed
			DisplayServers();
		}

		/// <summary>
		/// Identify which columns belong to which field
		/// </summary>
		private const int COLUMN_DESCRIPTION = 1;
		private const int COLUMN_INTERNETNAME = 2;
		private const int COLUMN_PORT = 3;
		private const int COLUMN_PROTOCOL = 4;

		private const int SUBITEM_INTERNETNAME = 2;
		private const int SUBITEM_PORT = 3;
		private const int SUBITEM_PROTOCOL = 4;

		private const String FILENAME_SERVER = "\\Servers.xml";

		/// <summary>
		/// Use the Servers.xml file to obtain the list of distributed
		/// servers.
		/// </summary>
		private void DisplayServers()
		{
			String ServersFile = Application.StartupPath + FILENAME_SERVER;
			XmlDocument xmlProgram = new XmlDocument();
			xmlProgram.Load(ServersFile);

			XmlNode Root = xmlProgram.DocumentElement;
			//
			// Work through the list of Server nodes in the file
			XmlNodeList ServerList = Root.SelectNodes(ServerManagerSingleton.XML_SERVER);
			foreach (XmlNode xmlServer in ServerList)
			{
				String FriendlyName = xmlServer.SelectSingleNode(ServerManagerSingleton.XML_DESCRIPTION).InnerText;
				String InternetName = xmlServer.SelectSingleNode(ServerManagerSingleton.XML_INTERNETNAME).InnerText;
				String Port = xmlServer.SelectSingleNode(ServerManagerSingleton.XML_PORT).InnerText;
				String Protocol = xmlServer.SelectSingleNode(ServerManagerSingleton.XML_PROTOCOL).InnerText;
				bool Enabled = Convert.ToBoolean(xmlServer.SelectSingleNode(ServerManagerSingleton.XML_ENABLED).InnerText);

				ListViewItem lviServer = lvServers.Items.Add("");
				lviServer.SubItems.Add(FriendlyName);
				lviServer.SubItems.Add(InternetName);
				lviServer.SubItems.Add(Port);
				lviServer.SubItems.Add(Protocol);
				lviServer.Checked = Enabled;
			}

		}

		/// <summary>
		/// Collect the server configuration from the UI and write the
		/// server configuraton XML file.
		/// </summary>
		private void SaveServerConfiguration()
		{
			//
			// Create the xml file
			String ServersFile = Application.StartupPath + FILENAME_SERVER;
			using (XmlTextWriter xmlWriter = new XmlTextWriter(ServersFile, System.Text.Encoding.UTF8))
			{
				xmlWriter.Formatting = Formatting.Indented;
				xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
				xmlWriter.WriteStartElement(ServerManagerSingleton.XML_DOCUMENTROOT);

				foreach (ListViewItem lviServer in lvServers.Items)
				{
					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_SERVER);

					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_INTERNETNAME);
					xmlWriter.WriteValue(lviServer.SubItems[COLUMN_INTERNETNAME].Text);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_PORT);
					xmlWriter.WriteValue(lviServer.SubItems[COLUMN_PORT].Text);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_PROTOCOL);
					xmlWriter.WriteValue(lviServer.SubItems[COLUMN_PROTOCOL].Text);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_DESCRIPTION);
					xmlWriter.WriteValue(lviServer.SubItems[COLUMN_DESCRIPTION].Text);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteStartElement(ServerManagerSingleton.XML_ENABLED);
					xmlWriter.WriteValue(lviServer.Checked.ToString());
					xmlWriter.WriteEndElement();

					xmlWriter.WriteEndElement();

				}

				//
				// Close off the document
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//
			// Update the database configuration
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			config.AppSettings.Settings["GPDatabase"].Value = lbDatabase.Text;
			config.Save();

			//
			// Save the server configuration
			SaveServerConfiguration();

			this.DialogResult=DialogResult.OK;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			odDatabase.FileName = lbDatabase.Text;
			if (odDatabase.ShowDialog() == DialogResult.OK)
			{
				//
				// Check to see if the database is valid, if not, then don't
				// accept the selection.
				if (GPDatabaseUtils.ValidateDatabase(odDatabase.FileName))
				{
					lbDatabase.Text = odDatabase.FileName;
				}
				else
				{
					String msg = "The selected database is not compatible with this version of ";
					msg += "the GP Studio software.  Please select another database.";
					MessageBox.Show(msg, GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
				}
			}
		}

		private void lvServers_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
		{
			//
			// Depending upon which item selected, use a different edit control
			switch (e.SubItem)
			{
				case COLUMN_DESCRIPTION:
					txtFriendlyName.Text = e.Item.Text;
					lvServers.StartEditing(txtFriendlyName, e.Item, e.SubItem);
					break;
				case COLUMN_INTERNETNAME:
					txtInternetName.Text = e.Item.SubItems[0].Text;
					lvServers.StartEditing(txtInternetName, e.Item, e.SubItem);
					break;
				case COLUMN_PORT:
					txtPort.Text = e.Item.SubItems[1].Text;
					lvServers.StartEditing(txtPort, e.Item, e.SubItem);
					break;
				case COLUMN_PROTOCOL:
					cmbProtocol.Text = e.Item.SubItems[SUBITEM_PROTOCOL].Text;
					lvServers.StartEditing(cmbProtocol, e.Item, e.SubItem);
					break;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//
			// Get a new, default server, added
			ListViewItem lviNew = lvServers.Items.Add("");
			lviNew.SubItems.Add("New Server");
			lviNew.SubItems.Add("New Host");
			lviNew.SubItems.Add("1201");
			lviNew.SubItems.Add("Tcp");
			lviNew.Checked = false;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			//
			// Ask the user to confirm first
			if (MessageBox.Show("Confirm removal of distributed server?", GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				//
				// Remove the selected server
				lvServers.SelectedItems[0].Remove();
			}
		}

		private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvServers.SelectedItems.Count == 0)
			{
				btnRemove.Enabled = false;
				btnTest.Enabled = false;
			}
			else
			{
				btnRemove.Enabled = true;
				btnTest.Enabled = true;
			}
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			//
			// Create a busy cursor
			Cursor PrevCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;

			//
			// Try to invoke the test interface on the remote server
			String HostName = lvServers.SelectedItems[0].SubItems[SUBITEM_INTERNETNAME].Text;
			String Port = lvServers.SelectedItems[0].SubItems[SUBITEM_PORT].Text;
			String Protocol = lvServers.SelectedItems[0].SubItems[SUBITEM_PROTOCOL].Text;
			bool Validate = fmMain.ServerManager.ValidateServer(HostName, Port, Protocol,"");

			//
			// Restore the cursor
			this.Cursor = PrevCursor;

			if (Validate)
			{
				MessageBox.Show("Remote service is running", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
			}
			else
			{
				MessageBox.Show("Remote service is not reachable or running the incorrect GP Server version.  Ensure the settings are correct, the service is running and that it is the correction version and try again.", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
			}
		}
	}
}