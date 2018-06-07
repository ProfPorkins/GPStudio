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
using System.Diagnostics;
using GPStudio.Shared;

namespace GPStudio.Client
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
			this.Text = GPEnums.APPLICATON_NAME;

			//
			// Display the current DB & Server address
			DisplayPrefs();
		}

		private void fmMain_Load(object sender, EventArgs e)
		{
			//
			// Check to see if we have a valid database
			while (!GPDatabaseUtils.IsDatabaseValid)
			{
				String msg = "The currently configured database is invalid.  Please use the ";
				msg += "Preferences dialog to select a valid GP Studio database.";
				MessageBox.Show(msg, GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);

				MenuPreferences_Click(sender, e);
			}

			//
			// Create the batch processing manager
			fmMain.BatchManager = new BatchProcessingManager();

			//
			// Create remote client sponsor server
			fmMain.SponsorServer = new SponsorThread();

			//
			// Create the server manager and go ahead and add a reference
			// to the local service.
			ServerManager = new ServerManagerSingleton();
			if (!ServerManager.RegisterServer(
				ServerManagerSingleton.LOCALHOST,
				ServerManagerSingleton.LOCALPORT,
				GPEnums.CHANNEL_TCP,"Local Computer"))
			{
				String msg="The local GP Server service is not running.  ";
				msg += "This service is required for GP Studio to correctly run.  ";
				msg += "Please refer to the User Guide or the online knowledge base for ";
				msg += "details on how to troubleshoot this problem.  After you press OK, ";
				msg += "GP Studio will close.";
				MessageBox.Show(msg, GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);

				Application.Exit();
			}
		}

		/// <summary>
		/// Object that manages batch processing of models
		/// </summary>
		public static BatchProcessingManager BatchManager;

		/// <summary>
		/// Places the remote object sponsors in a separate thread, trying to ensure
		/// the communication happens even while modeling.
		/// </summary>
		public static SponsorThread SponsorServer;

		/// <summary>
		/// This is the object that manages all the distributed servers this
		/// client can utilize.
		/// </summary>
		public static ServerManagerSingleton ServerManager;

		private void DisplayPrefs()
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

			tsDatabase.Text=config.AppSettings.Settings["GPDatabase"].Value;
		}

		private void cascadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.Cascade);
		}

		private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileVertical);
		}

		private void menuProjectNew_Click(object sender, EventArgs e)
		{
			//
			// Create the new project entry
			GPProject proj = new GPProject("New Project");

			//
			// Bring up the project form
			fmProject project = new fmProject(proj);
			project.MdiParent = this;
			project.Show();
		}

		private void menuExit_Click(object sender, EventArgs e)
		{
			//
			// TODO: In future, be sure to make sure all modeling pages
			// have been saved and nothing is currently running.

			Application.Exit();
		}

		private void menuDataManager_Click(object sender, EventArgs e)
		{
			//
			// If already active, bring it to the front
			if (fmDataManager.Active)
			{
				//
				// Go through the list of MDI windows
				foreach (Form fmChild in this.MdiChildren)
				{
					if (fmChild is fmDataManager)
					{
						fmChild.BringToFront();
					}
				}				
				return;
			}

			fmDataManager import = new fmDataManager();
			import.MdiParent = this;
			import.Show();
		}

		private void menuProjectOpen_Click(object sender, EventArgs e)
		{
			//
			// Bring up the list of project
			fmProjectManager dlg = new fmProjectManager();
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				//
				// Load up this project and bring up the Project form
				ListViewItem lvSelected = dlg.lvProjects.Items[dlg.lvProjects.SelectedItems[0].Index];
				int ProjectID=Convert.ToInt32(lvSelected.Tag.ToString());
				//
				// Load up the project
				GPProject proj = new GPProject("New Project");
				proj.Load(ProjectID);
				//
				// Bring up the project form
				fmProject project = new fmProject(proj);
				project.MdiParent = this;
				project.Show();
			}

		}

		private void btnFunctionSet_Click(object sender, EventArgs e)
		{
			if (fmFunctionEditor.Active)
			{
				//
				// Go through the list of MDI windows
				foreach (Form fmChild in this.MdiChildren)
				{
					if (fmChild is fmFunctionEditor)
					{
						fmChild.BringToFront();
					}
				}
				return;
			}

			fmFunctionEditor form = new fmFunctionEditor();
			form.MdiParent = this;
			form.Show();
		}

		private void btnFitnessFunctions_Click(object sender, EventArgs e)
		{
			if (fmFitnessEditor.Active)
			{
				//
				// Go through the list of MDI windows
				foreach (Form fmChild in this.MdiChildren)
				{
					if (fmChild is fmFitnessEditor)
					{
						fmChild.BringToFront();
					}
				}
				return;
			}

			fmFitnessEditor form = new fmFitnessEditor();
			form.MdiParent = this;
			form.Show();
		}


		private void MenuPreferences_Click(object sender, EventArgs e)
		{
			//
			// Bring up the preferences edit
			fmPreferences dlg = new fmPreferences();
			dlg.ShowDialog();
			//
			// Update the status bar
			DisplayPrefs();
		}

		private void fileToolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
		{
			//
			// Enable/disable the preferences menu as appropriate
			if (this.MdiChildren.Length > 0)
			{
				MenuPreferences.Enabled = false;
			}
			else
			{
				MenuPreferences.Enabled = true;
			}
		}

		/// <summary>
		/// Determines if a project is already opened
		/// </summary>
		/// <param name="ProjectID">DBCode of the project in question</param>
		/// <returns>True if the project is opened, false otherwise</returns>
		public bool ProjectOpen(int ProjectID)
		{
			//
			// Go through the list of MDI windows
			foreach (Form fmChild in this.MdiChildren)
			{
				if (fmChild is fmProject)
				{
					fmProject fmProject = (fmProject)fmChild;
					if (fmProject.ProjectID == ProjectID)
					{
						return true;
					}
				}
			}

			return false;
		}

		private void indexToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			//
			// Load up the user guide
			Process.Start(Application.StartupPath + "\\Docs\\User Guide.pdf");
		}

		private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			fmAboutBox dlg = new fmAboutBox();
			dlg.ShowDialog();
		}

		private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			//
			// Terminate the sponsor server
			fmMain.SponsorServer.Terminate();

			//
			// Terminate the batch processing manager
			fmMain.BatchManager.Terminate();
		}

		private void batchProcessingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (fmBatchProcessing.Active)
			{
				//
				// Go through the list of MDI windows
				foreach (Form fmChild in this.MdiChildren)
				{
					if (fmChild is fmBatchProcessing)
					{
						fmChild.BringToFront();
					}
				}
				return;
			}

			fmBatchProcessing form = new fmBatchProcessing();
			form.MdiParent = this;
			form.Show();
		}
    }
}