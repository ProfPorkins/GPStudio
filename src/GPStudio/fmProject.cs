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
using System.Windows.Forms;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public partial class fmProject : Form
	{

		public fmProject(GPProject Project)
		{
			InitializeComponent();

			m_Project = Project;
			Title = m_Project.Name;

			//
			// Set the UI controls
			InitializeUISettings();

			//
			// Disable the ability to edit the profile
			foreach (TabPage Page in tabProfile.TabPages)
			{
				foreach (Control Child in Page.Controls)
				{
					Child.Enabled = false;
				}
			}

			//
			// Modeling Profile - Load the list of available functions
			InitializeFunctionSet();

			//
			// Modeling Profile - Load the list of available fitness functions
			InitializeFitnessFunctions();

			//
			// Prepare the visual appearance of the graphs
			InitializeFitnessGraph();
			InitializePopComplexityGraph();
			InitializePopFitnessGraph();

			//
			// Register with the batch processing manager
			fmMain.BatchManager.RegisterClient(this);
		}

		/// <summary>
		/// Database code of the current project
		/// </summary>
		public int ProjectID
		{
			get { return m_Project.DBCode; }
		}
		private GPProject m_Project;


		SortedList<int, int> m_UserGuideProjects = new SortedList<int, int>();

		/// <summary>
		/// Title to display on the form
		/// </summary>
		private String Title
		{
			set { Text = TitleRoot + value;	}
		}
		public const String TitleRoot = "Modeling Project - ";
		
		/// <summary>
		/// This is a private class used to represent an item in the list
		/// of modeling Training sets.  Kind of glad I did this, it made things
		/// a lot easier to deal with, need to remember to do this kind of thing
		/// more in the future.
		/// </summary>
		private class ComboDataItem
		{
			public String Title;
			public int ModelingFileID;

			public ComboDataItem(String Title, int ModelingFileID)
			{
				this.Title = Title;
				this.ModelingFileID = ModelingFileID;
			}

			public override String ToString()
			{
				return Title;
			}
		}

		/// <summary>
		/// Set all the controls according to the current project values
		/// </summary>
		private void InitializeUISettings()
		{
			txtProjectName.Text = m_Project.Name;
			txtDataTraining.Text = GPDatabaseUtils.FieldValue(m_Project.DataTrainingID, "tblModelingFile", "Name");

			//
			// Load the training file into the results set
			cbResultsDataSet.Items.Add(new ComboDataItem("Training", m_Project.DataTrainingID));
			//
			// Display the validation files
			foreach (int FileID in m_Project.DataValidation)
			{
				String Name = GPDatabaseUtils.FieldValue(FileID, "tblModelingFile", "Name");
				cbDataValidation.Items.Add(new ComboDataItem(Name, FileID));
				//
				// Add it to the results page
				cbResultsDataSet.Items.Add(new ComboDataItem(Name, FileID));
			}
			if (cbDataValidation.Items.Count > 0)
			{
				cbDataValidation.SelectedIndex = 0;
			}
			if (cbResultsDataSet.Items.Count > 0)
			{
				cbResultsDataSet.SelectedIndex = 0;
			}

			//
			// Display the profiles
			foreach (int ProfileID in m_Project.ModelProfiles)
			{
				ListViewItem item = lvProfiles.Items.Add(GPDatabaseUtils.FieldValue(ProfileID, "tblModelProfile", "Name"));
				item.Tag = ProfileID;
				item.ImageIndex = 0;

				lvProfilesModeling.Items.Add((ListViewItem)item.Clone());
				lvProfilesResults.Items.Add((ListViewItem)item.Clone());
			}

			//
			// Auto select the first item in the modeling profiles (if one exists),
			// except for the results page (because it takes time to Load and compile
			// the function set).
			if (lvProfiles.Items.Count > 0)
			{
				lvProfiles.Items[0].Selected = true;
				lvProfilesModeling.Items[0].Selected = true;

				//
				// Disable the training data selection
				btnDataTraining.Enabled = false;
			}
			else
			{
				btnDataTraining.Enabled = true;
			}

			//
			// Set the initial display language to C#
			tsmenuLanguage.Text = GPEnums.LANGUAGE_CSHARP;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			//
			// Bring up the modeling Training selection dialog box
			fmSelectModelingFile dlg = new fmSelectModelingFile(0);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				//
				// Add the selection to the project
				if (dlg.lvFiles.SelectedItems.Count > 0)
				{
					ListViewItem lvSelected = dlg.lvFiles.Items[dlg.lvFiles.SelectedItems[0].Index];
					txtDataTraining.Text = lvSelected.Text;
					txtDataTraining.Tag = lvSelected.Tag;

					m_Project.DataTrainingID = Convert.ToInt32(lvSelected.Tag.ToString());
					m_Project.Save();
				}
			}
		}

		private void txtProjectName_TextChanged(object sender, EventArgs e)
		{
			//
			// Update the titles
			m_Project.Name = txtProjectName.Text;
			Title = txtProjectName.Text;

			m_Project.Save();
		}

		private void txtDataTraining_TextChanged(object sender, EventArgs e)
		{
			if (txtDataTraining.Text == "<None>")
			{
				btnDataValidationAdd.Enabled = false;
			}
			else
			{
				btnDataValidationAdd.Enabled = true;
			}
		}

		private bool m_LoadingProfile = false;
		/// <summary>
		/// Indicate the profile is now dirty
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Event_ProfileDirty(object sender, EventArgs e)
		{
			if (!m_LoadingProfile && m_Profile != null)
			{
				//
				// The profile is now dirty, so enable the save button and indicate
				// a dirty flag on the profile structure itself.
				m_Profile.Dirty = true;
				btnProfileSave.Enabled = true;
			}
		}

		private void tabModeling_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbPopulationFitnessServers.Focus();
		}

		private void fmProject_FormClosing(object sender, FormClosingEventArgs e)
		{
			//
			// Release any sponsors we have set up
			m_FunctionSetSponsor = null;
			System.GC.Collect();

			//
			// Unregister from the Batch Processing Manager
			fmMain.BatchManager.UnregisterClient(this);
		}
	}
}
