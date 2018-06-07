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
using System.Data.OleDb;
using System.Data;
using GPStudio.Shared;

namespace GPStudio.Client
{
	partial class fmProject
	{
		/// <summary>
		/// Prepare the list of validated fitness functions that can be
		/// chosen from.
		/// </summary>
		private void InitializeFitnessFunctions()
		{
			OleDbConnection con = GPDatabaseUtils.Connect();
			{
				String SQL = "SELECT DBCode,Name FROM tblFitnessFunction ";
				SQL+="WHERE Validated = TRUE ";
				SQL+="ORDER BY Name";

				OleDbDataAdapter daFunctions = new OleDbDataAdapter(SQL, con);
				DataSet dSet = new DataSet();
				daFunctions.Fill(dSet);

				foreach (DataRow row in dSet.Tables[0].Rows)
				{
					RadioButton btnItem = new RadioButton();
					btnItem.Text = row.ItemArray.GetValue(1).ToString();
					btnItem.Tag = row.ItemArray.GetValue(0);
					btnItem.Click += FitnessRadioButton_Click;

					btnItem.Parent = flowFitnessFunctions;
				}
			}
			con.Close();
		}

		/// <summary>
		/// Prepare the list of validated functions that can be selected
		/// </summary>
		private void InitializeFunctionSet()
		{
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{

				//
				// Select only functions from the C# list
				String SQL = "SELECT tblFunctionSet.DBCode,tblFunctionSet.Name";
				SQL += ",tblFunctionSet.Arity,tblFunctionSet.Description";
				SQL += ",tblFunctionCategory.Name ";
				SQL += "FROM tblFunctionSet ";
				SQL += "INNER JOIN tblFunctionCategory ON tblFunctionSet.CategoryID = tblFunctionCategory.DBCode  ";
				SQL += "WHERE tblFunctionSet.FunctionLanguageID = 3 ";
				SQL += "AND tblFunctionSet.Validated = TRUE ";
				SQL += "ORDER BY tblFunctionCategory.Name,tblFunctionSet.Name";

				OleDbCommand cmd = new OleDbCommand(SQL, con);
				OleDbDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					ListViewItem lvItem = lvFunctions.Items.Add(rdr["tblFunctionSet.Name"].ToString());
					lvItem.SubItems.Add(((short)rdr["Arity"]).ToString());
					lvItem.SubItems.Add(rdr["Description"].ToString());
					lvItem.SubItems.Add(rdr["tblFunctionCategory.Name"].ToString());

					lvItem.ToolTipText = rdr["Description"].ToString();
					lvItem.Tag = Convert.ToInt32(rdr["DBCode"]);
				}
			}
		}

		/// <summary>
		/// Sets all the UI controls according to this modeling profile.
		/// </summary>
		/// <param name="Profile">Profile to load into the UI</param>
		private void UpdateProfileUI(GPProjectProfile Profile)
		{
			m_LoadingProfile = true;

			UpdateUI_ModelingType(Profile);
			UpdateUI_ProgramStructure(Profile);
			UpdateUI_PopulationStructure(Profile);
			UpdateUI_PopulationDistributed(Profile);
			UpdateUI_Generations(Profile);
			UpdateUI_Fitness(Profile);
			UpdateUI_FunctionSet(Profile);
			UpdateUI_TerminalSet(Profile);

			m_LoadingProfile = false;
		}

		//
		// Grabs the current state of the UI and updates the profile
		// Training structure accordingly.
		private void CollectProfileUI(GPProjectProfile Profile)
		{
			CollectUI_ModelingType(Profile);
			CollectUI_ProgramStructure(Profile);
			CollectUI_PopulationStructure(Profile);
			CollectUI_PopulationDistributed(Profile);
			CollectUI_Generations(Profile);
			CollectUI_Fitness(Profile);
			CollectUI_FunctionSet(Profile);
			CollectUI_TerminalSet(Profile);
		}

		//
		// Update the UI controls Program Structure section
		private void UpdateUI_ProgramStructure(GPProjectProfile Profile)
		{
			udStructure_Reproduction.Value = Profile.ModelingProfile.ProbabilityReproduction;
			udStructure_Mutation.Value = Profile.ModelingProfile.ProbabilityMutation;
			udStructure_Crossover.Value = Profile.ModelingProfile.ProbabilityCrossover;

			//
			// ADFs
			lvADF.Items.Clear();
			foreach (byte adf in Profile.m_ADFSet)
			{
				ListViewItem lviADF = lvADF.Items.Add("ADF" + (lvADF.Items.Count));
				lviADF.SubItems.Add(adf.ToString());
			}

			//
			// ADLs
			lvADL.Items.Clear();
			foreach (byte adl in Profile.m_ADLSet)
			{
				ListViewItem lviADL = lvADL.Items.Add("ADL" + (lvADL.Items.Count));
				lviADL.SubItems.Add(adl.ToString());
			}

			//
			// ADRs
			lvADR.Items.Clear();
			foreach (byte adr in Profile.m_ADRSet)
			{
				ListViewItem lviADR = lvADR.Items.Add("ADR" + (lvADR.Items.Count));
				lviADR.SubItems.Add(adr.ToString());
			}

			chkStructure_UseMemory.Checked = Profile.ModelingProfile.UseMemory;
			udStructure_MemoryCount.Value = Profile.ModelingProfile.CountMemory;
		}

		//
		// Save the UI controls Modeling Type section
		private void CollectUI_ModelingType(GPProjectProfile Profile)
		{
			if (rdModeling_Regression.Checked)
			{
				Profile.ModelType.Type = GPEnums.ModelType.Regression;
			}
			else
				if (rdModeling_TimeSeries.Checked)
				{
					Profile.ModelType.Type = GPEnums.ModelType.TimeSeries;
					Profile.ModelType.InputDimension = (short)udModeling_InputDimension.Value;
					Profile.ModelType.PredictionDistance = (short)udModeling_PredictionDistance.Value;
				}
		}

		//
		// Update the UI controls Modeling Type section
		private void UpdateUI_ModelingType(GPProjectProfile Profile)
		{
			switch (Profile.ModelType.Type)
			{
				case GPEnums.ModelType.Regression:
					rdModeling_Regression.Checked = true;
					break;
				case GPEnums.ModelType.TimeSeries:
					rdModeling_TimeSeries.Checked = true;
					udModeling_InputDimension.Value = Profile.ModelType.InputDimension;
					udModeling_PredictionDistance.Value = Profile.ModelType.PredictionDistance;
					break;
			}
		}

		//
		// Save the UI controls Program Structure section
		private void CollectUI_ProgramStructure(GPProjectProfile Profile)
		{
			Profile.ModelingProfile.ProbabilityReproduction = (short)udStructure_Reproduction.Value;
			Profile.ModelingProfile.ProbabilityMutation = (short)udStructure_Mutation.Value;
			Profile.ModelingProfile.ProbabilityCrossover = (short)udStructure_Crossover.Value;

			Profile.ModelingProfile.UseMemory = chkStructure_UseMemory.Checked;
			Profile.ModelingProfile.CountMemory = (short)udStructure_MemoryCount.Value;
		}

		//
		// Update the UI controls Population Structure section
		private void UpdateUI_PopulationStructure(GPProjectProfile Profile)
		{
			switch (Profile.ModelingProfile.PopulationBuild)
			{
				case GPEnums.PopulationInit.Full:
					cmbStructure_PopInit.SelectedIndex = cmbStructure_PopInit.Items.IndexOf("Full");
					break;
				case GPEnums.PopulationInit.Grow:
					cmbStructure_PopInit.SelectedIndex = cmbStructure_PopInit.Items.IndexOf("Grow");
					break;
				case GPEnums.PopulationInit.Ramped:
					cmbStructure_PopInit.SelectedIndex = cmbStructure_PopInit.Items.IndexOf("Ramped Half-Half");
					break;
			}

			udPopulation_TreeDepth.Value = Profile.ModelingProfile.PopulationInitialDepth;

			switch (Profile.ModelingProfile.Reproduction)
			{
				case GPEnums.Reproduction.Tournament:
					cmbStructure_PopReproduction.SelectedIndex = cmbStructure_PopReproduction.Items.IndexOf("Tournament");
					break;
				case GPEnums.Reproduction.OverSelection:
					cmbStructure_PopReproduction.SelectedIndex = cmbStructure_PopReproduction.Items.IndexOf("Overselection");
					break;
			}

			udStructure_PopSize.Value = Profile.ModelingProfile.PopulationSize;
		}

		//
		// Save the UI controls Population Structure section
		private void CollectUI_PopulationStructure(GPProjectProfile Profile)
		{
			//
			// TODO: need to not use text, need to use an ID of some kinds
			switch (cmbStructure_PopInit.Text)
			{
				case "Grow":
					Profile.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Grow;
					break;
				case "Full":
					Profile.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Full;
					break;
				case "Ramped Half-Half":
					Profile.ModelingProfile.PopulationBuild = GPEnums.PopulationInit.Ramped;
					break;
			}
			Profile.ModelingProfile.PopulationInitialDepth = (int)udPopulation_TreeDepth.Value;

			switch (cmbStructure_PopReproduction.Text)
			{
				case "Tournament":
					Profile.ModelingProfile.Reproduction = GPEnums.Reproduction.Tournament;
					break;
				case "Overselection":
					Profile.ModelingProfile.Reproduction = GPEnums.Reproduction.OverSelection;
					break;
			}
			Profile.ModelingProfile.PopulationSize = (int)udStructure_PopSize.Value;
		}

		//
		// Update the UI controls for the Distributed Population Options
		private void UpdateUI_PopulationDistributed(GPProjectProfile Profile)
		{
			udDistributed_TransferCount.Value = Profile.ModelingProfile.DistributedTransferCount;

			switch (Profile.ModelingProfile.DistributedTopology)
			{
				case GPEnums.DistributedTopology.Ring:
					rdDistributed_Topology_Ring.Checked = true;
					break;
				case GPEnums.DistributedTopology.Star:
					rdDistributed_Topology_Star.Checked = true;
					break;
			}
			udDistributed_Topology_StarPercent.Value = Profile.ModelingProfile.DistributedTopologyStarPercent;
		}

		//
		// Save the UI status for the Distributed Population Options
		private void CollectUI_PopulationDistributed(GPProjectProfile Profile)
		{
			Profile.ModelingProfile.DistributedTransferCount = (int)udDistributed_TransferCount.Value;

			if (rdDistributed_Topology_Ring.Checked)
			{
				Profile.ModelingProfile.DistributedTopology = GPEnums.DistributedTopology.Ring;
			}
			else if (rdDistributed_Topology_Star.Checked)
			{
				Profile.ModelingProfile.DistributedTopology = GPEnums.DistributedTopology.Star;
			}
			Profile.ModelingProfile.DistributedTopologyStarPercent = (int)udDistributed_Topology_StarPercent.Value;
		}

		//
		// Update the UI controls Generations tab page
		private void UpdateUI_Generations(GPProjectProfile Profile)
		{
			chkGens_MaxNumber.Checked = Profile.m_useMaxNumber;
			udGens_MaxNumber.Value = Profile.m_maxNumber;

			chkGens_RawFitness0.Checked = Profile.m_useRawFitness0;
			chkGens_HitsMaxed.Checked = Profile.m_useHitsMaxed;
		}

		//
		// Save the UI controls Generations tab page
		private void CollectUI_Generations(GPProjectProfile Profile)
		{
			Profile.m_useMaxNumber = chkGens_MaxNumber.Checked;
			Profile.m_maxNumber = (int)udGens_MaxNumber.Value;

			Profile.m_useRawFitness0 = chkGens_RawFitness0.Checked;
			Profile.m_useHitsMaxed = chkGens_HitsMaxed.Checked;
		}

		/// <summary>
		/// Update the UI controls on the Fitness tab page
		/// </summary>
		/// <param name="Profile">Reference to the object containing the profile settings</param>
		private void UpdateUI_Fitness(GPProjectProfile Profile)
		{
			//
			// Measurement of fitness - Check the item with the correct
			// FitnessFunctionID value
			foreach (RadioButton btn in flowFitnessFunctions.Controls)
			{
				if (btn.Tag.ToString() == Profile.FitnessFunctionID.ToString())
				{
					btn.Checked = true;
				}
			}

			//
			// Adaptive parsimiony setting
			chkFitness_Parsimony.Checked = Profile.ModelingProfile.AdaptiveParsimony;
			chkFitness_MultiObjective.Checked = Profile.ModelingProfile.SPEA2MultiObjective;
		}

		//
		// Save the UI controls Fitness tab page
		/// <summary>
		/// Grab the UI control settings from the Fitness tab page
		/// </summary>
		/// <param name="Profile">Profile object to place the settings</param>
		private void CollectUI_Fitness(GPProjectProfile Profile)
		{
			//
			// Measurement of fitness - Find the checked item, there
			// will only be one.
			foreach (RadioButton btn in flowFitnessFunctions.Controls)
			{
				if (btn.Checked)
				{
					Profile.FitnessFunctionID = Convert.ToInt32(btn.Tag.ToString());
				}
			}

			//
			// Adaptive parsimony
			Profile.ModelingProfile.AdaptiveParsimony = chkFitness_Parsimony.Checked;
			Profile.ModelingProfile.SPEA2MultiObjective = chkFitness_MultiObjective.Checked;
		}

		//
		// Update the UI controls Function Set tab page
		private void UpdateUI_FunctionSet(GPProjectProfile Profile)
		{
			//
			// Reset all the items to be not checked
			foreach (ListViewItem item in lvFunctions.Items)
			{
				item.Checked = false;
			}

			//
			// Set which functions belong to this profile
			foreach (string Function in Profile.FunctionSet)
			{
				//
				// Check the item of the same name
				ListViewItem item = lvFunctions.FindItemWithText(Function);
				if (item != null)
				{
					item.Checked = true;
				}
			}
		}

		//
		// Save the UI controls Function Set tab page
		private void CollectUI_FunctionSet(GPProjectProfile Profile)
		{
			//
			// Reset the function set
			Profile.FunctionSet.Clear();

			//
			// Go through the list view and add back in the selected items
			foreach (ListViewItem item in lvFunctions.Items)
			{
				if (item.Checked)
				{
					Profile.FunctionSet.Add(item.Text);
				}
			}
		}

		//
		// Update the UI controls Terminal Set tab page
		private void UpdateUI_TerminalSet(GPProjectProfile Profile)
		{
			chkTS_RndInteger.Checked = Profile.ModelingProfile.UseRndInteger;
			txtTS_IntegerMin.Text = Profile.ModelingProfile.IntegerMin.ToString();
			txtTS_IntegerMax.Text = Profile.ModelingProfile.IntegerMax.ToString();

			chkTS_RndDouble.Checked = Profile.ModelingProfile.UseRndDouble;
		}

		//
		// Save the UI controls Terminal Set tab page
		private void CollectUI_TerminalSet(GPProjectProfile Profile)
		{
			Profile.ModelingProfile.UseRndInteger = chkTS_RndInteger.Checked;
			Profile.ModelingProfile.IntegerMin = Convert.ToInt32(txtTS_IntegerMin.Text);
			Profile.ModelingProfile.IntegerMax = Convert.ToInt32(txtTS_IntegerMax.Text);

			Profile.ModelingProfile.UseRndDouble = chkTS_RndDouble.Checked;
		}

		private void btnProfileDelete_Click(object sender, EventArgs e)
		{
			//
			// Let the user know if this has programs associated with it.
			String msg="Confirm Profile Removal?";
			if (m_Profile.ProfileInUse)
			{
				msg += " This profile has programs associated with it!";
			}

			if (MessageBox.Show(msg, GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				//
				// It no longer matters at this point, it's gone!
				m_Profile.Dirty = false;

				//
				// Remove the selected profile from the project
				int ProfileID = Convert.ToInt32(lvProfiles.SelectedItems[0].Tag.ToString());
				if (m_Project.DeleteProfile(ProfileID))
				{
					//
					// Remove this same profile from the modeling & Results tab
					ListViewItem lvDelete = lvProfiles.SelectedItems[0];
					lvProfilesModeling.FindItemWithText(lvDelete.Text).Remove();
					lvProfilesResults.FindItemWithText(lvDelete.Text).Remove();

					//
					// Remove the item from the project page
					lvProfiles.SelectedItems[0].Remove();

					//
					// Remove the profile from the database
					DeleteProfileFromDB(ProfileID);
				}
			}

			//
			// If no profiles, enable the training data button
			if (lvProfiles.Items.Count == 0)
			{
				btnDataTraining.Enabled = true;
			}
		}

		//
		// Remove the selected profile ID from the database
		private bool DeleteProfileFromDB(int ProfileID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to add the blob to the database
			OleDbCommand cmd = new OleDbCommand("DELETE FROM tblModelProfile WHERE DBCode = " + ProfileID, con);

			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
			}

			con.Close();

			return true;
		}

		private void rdModeling_TimeSeries_CheckedChanged(object sender, EventArgs e)
		{
			gbModeling_Regression.Enabled = rdModeling_TimeSeries.Checked;
		}

		GPProjectProfile m_Profile;
		private void lvProfiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			//
			// If the current profile is dirty, ask the user if they want to save it
			if (m_Profile != null && m_Profile.Dirty)
			{
				if (MessageBox.Show("Current profile has been modified, would you like to save it first?", GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					btnProfileSave_Click(sender,e);
				}
				//
				// Set the dirty flag to false even if the user didn't save, it prevents
				// the code from asking them again.
				m_Profile.Dirty = false;
			}

			//
			// If a modeling session is active, don't enable the Delete button
			if (!btnCancel.Enabled)
			{
				btnProfileDelete.Enabled = e.IsSelected;
			}

			btnProfileSave.Enabled = false;	// Always disable upon new selection
			btnProfileCopy.Enabled = e.IsSelected;
			if (e.IsSelected)
			{
				//
				// Get the selected profile displayed
				int ProfileID = (int)e.Item.Tag;
				m_Profile = new GPProjectProfile();
				m_Profile.LoadFromDB(ProfileID);

				UpdateProfileUI(m_Profile);

				//
				// If this profile is in use, then disable all the tab pages
				// so it can't be modified
				bool InUse = m_Profile.ProfileInUse;
				foreach (TabPage Page in tabProfile.TabPages)
				{
					foreach (Control Child in Page.Controls)
					{
						Child.Enabled = !InUse;
					}
				}

				//
				// Disable the delete buttons for the ADF, ADL and ADRs
				if (lvADF.SelectedItems.Count == 0) btnADFDelete.Enabled = false;
				if (lvADL.SelectedItems.Count == 0) btnADLDelete.Enabled = false;
				if (lvADR.SelectedItems.Count == 0) btnADRDelete.Enabled = false;
			}
			else
			{
				//
				// Disable the ability to edit the profile
				foreach (TabPage Page in tabProfile.TabPages)
				{
					foreach (Control Child in Page.Controls)
					{
						Child.Enabled = false;
					}
				}
			}
		}

		private void lvADF_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			btnADFDelete.Enabled = e.IsSelected;
		}

		private void lvADL_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			btnADLDelete.Enabled = e.IsSelected;
		}

		private void lvADR_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			btnADRDelete.Enabled = e.IsSelected;
		}

		private void lvADF_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
		{
			if (e.SubItem == 1)
			{
				udADF.Value = m_Profile.m_ADFSet[e.Item.Index];
				lvADF.StartEditing(udADF, e.Item, e.SubItem);
			}
		}

		private void lvADL_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
		{
			if (e.SubItem == 1)
			{
				udADL.Value = m_Profile.m_ADLSet[e.Item.Index];
				lvADL.StartEditing(udADL, e.Item, e.SubItem);
			}
		}

		private void lvADR_SubItemClicked(object sender, ListViewEx.SubItemEventArgs e)
		{
			if (e.SubItem == 1)
			{
				udADR.Value = m_Profile.m_ADRSet[e.Item.Index];
				lvADR.StartEditing(udADR, e.Item, e.SubItem);
			}
		}

		private void btnADFAdd_Click(object sender, EventArgs e)
		{
			//
			// Add it to the UI
			ListViewItem lviADF = lvADF.Items.Add("ADF" + (lvADF.Items.Count));
			lviADF.SubItems.Add("1");

			//
			// Add it to the profile
			m_Profile.m_ADFSet.Add(1);

			Event_ProfileDirty(sender, e);
		}

		private void btnADLAdd_Click(object sender, EventArgs e)
		{
			//
			// Add it to the UI
			ListViewItem lviADL = lvADL.Items.Add("ADL" + (lvADL.Items.Count));
			lviADL.SubItems.Add("1");

			//
			// Add it to the profile
			m_Profile.m_ADLSet.Add(1);

			Event_ProfileDirty(sender, e);
		}

		private void btnADRAdd_Click(object sender, EventArgs e)
		{
			//
			// Add it to the UI
			ListViewItem lviADR = lvADR.Items.Add("ADR" + (lvADR.Items.Count));
			lviADR.SubItems.Add("1");

			//
			// Add it to the profile
			m_Profile.m_ADRSet.Add(1);

			Event_ProfileDirty(sender, e);
		}

		private void btnADFDelete_Click(object sender, EventArgs e)
		{
			//
			// Delete the current ADF
			ListViewItem lviADF = lvADF.SelectedItems[0];
			m_Profile.m_ADFSet.RemoveAt(lviADF.Index);
			lviADF.Remove();

			Event_ProfileDirty(sender, e);
		}

		private void btnADLDelete_Click(object sender, EventArgs e)
		{
			//
			// Delete the current ADL
			ListViewItem lviADL = lvADL.SelectedItems[0];
			m_Profile.m_ADLSet.RemoveAt(lviADL.Index);
			lviADL.Remove();

			Event_ProfileDirty(sender, e);
		}

		private void btnADRDelete_Click(object sender, EventArgs e)
		{
			//
			// Delete the current ADR
			ListViewItem lviADR = lvADR.SelectedItems[0];
			m_Profile.m_ADRSet.RemoveAt(lviADR.Index);
			lviADR.Remove();

			Event_ProfileDirty(sender, e);
		}

		private void udADF_ValueChanged(object sender, EventArgs e)
		{
			//
			// Update the parameter count in the profile
			ListViewItem lviUpdate = lvADF.SelectedItems[0];
			m_Profile.m_ADFSet[lviUpdate.Index] = Convert.ToByte(udADF.Value);

			Event_ProfileDirty(sender, e);
		}

		private void udADL_ValueChanged(object sender, EventArgs e)
		{
			//
			// Update the parameter count in the profile
			ListViewItem lviUpdate = lvADL.SelectedItems[0];
			m_Profile.m_ADLSet[lviUpdate.Index] = Convert.ToByte(udADL.Value);

			Event_ProfileDirty(sender, e);
		}

		private void udADR_ValueChanged(object sender, EventArgs e)
		{
			//
			// Update the parameter count in the profile
			ListViewItem lviUpdate = lvADR.SelectedItems[0];
			m_Profile.m_ADRSet[lviUpdate.Index] = Convert.ToByte(udADR.Value);

			Event_ProfileDirty(sender, e);
		}

		private void lvProfiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			//
			// Update the name in the database
			// There seems to be a strange bug in this event.  If the user leaves the
			// label highlighted and doesn't make a change, the label comes in as null,
			// so, in that case, don't do anything.
			if (e.Label != null)
			{
				int ProfileID = Convert.ToInt32(lvProfiles.Items[e.Item].Tag.ToString());
				UpdateProfileLabel(ProfileID, e.Label);

				//
				// Update this item in the other panes
				String PrevText = lvProfiles.SelectedItems[0].Text;
				lvProfilesModeling.FindItemWithText(PrevText).Text = e.Label;
				lvProfilesResults.FindItemWithText(PrevText).Text = e.Label;
			}
		}

		/// <summary>
		/// Update the label of the indicated profile ID
		/// </summary>
		/// <param name="ProfileID">Profile DBCode to give a new name</param>
		/// <param name="Name">Name to give the profile</param>
		private void UpdateProfileLabel(int ProfileID, String Name)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to update the profile name in the database
			OleDbCommand cmd = new OleDbCommand("UPDATE tblModelProfile SET Name = '" + Name + "' WHERE DBCode = " + ProfileID, con);

			//
			// Execute the command
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
			}

			con.Close();
		}

		private void btnProfileSave_Click(object sender, EventArgs e)
		{
			//
			// Grab the UI settings 
			CollectProfileUI(m_Profile);

			m_Profile.UpdateProfileInDB();

			btnProfileSave.Enabled = false;
		}


		private void btnProfileAdd_Click(object sender, EventArgs e)
		{
			//
			// Ensure a training data set has been added
			if (m_Project.DataTrainingID == 0)
			{
				MessageBox.Show("Please select a training data set before adding a modeling profile", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK);
				return;
			}

			//
			// Disable the training data selection button
			btnDataTraining.Enabled = false;

			//
			// Create a new, default, modeling profile for the user
			m_Profile = new GPProjectProfile();
			//
			// Update the controls with this profile
			UpdateProfileUI(m_Profile);

			int ModelProfileID = 0;
			m_Profile.CreateProfileInDB(ref ModelProfileID);

			//
			// Add the entry to the list of profiles
			ListViewItem lvNew = lvProfiles.Items.Add("New Profile");
			lvNew.Tag = ModelProfileID;
			lvNew.ImageIndex = 0;

			lvProfilesModeling.Items.Add((ListViewItem)lvNew.Clone());
			lvProfilesResults.Items.Add((ListViewItem)lvNew.Clone());

			m_Project.ModelProfiles.Add(ModelProfileID);

			//
			// Save the project to the database
			m_Project.Save();

			//
			// Set the profile label into edit mode - Before editing, select it
			// so that the after edit will end up working in the case this is
			// the first item
			lvNew.Selected = true;
			lvNew.BeginEdit();
		}

		private void btnProfileCopy_Click(object sender, EventArgs e)
		{

			//
			// Create a new profile in the database
			int NewProfileID = 0;
			m_Profile.CreateProfileInDB(ref NewProfileID);
			//
			// Collect the current settings
			CollectProfileUI(m_Profile);

			//
			// Save this one
			m_Profile.UpdateProfileInDB();
			UpdateProfileLabel(NewProfileID, "Copy of: " + lvProfiles.SelectedItems[0].Text);

			//
			// Create a new listview item and select it
			ListViewItem lviNew = lvProfiles.Items.Add(GPDatabaseUtils.FieldValue(NewProfileID, "tblModelProfile", "Name"));
			lviNew.Tag = NewProfileID;
			lviNew.ImageIndex = 0;

			lvProfilesModeling.Items.Add((ListViewItem)lviNew.Clone());
			lvProfilesResults.Items.Add((ListViewItem)lviNew.Clone());

			//
			// Add this profile to the current project
			m_Project.ModelProfiles.Add(NewProfileID);

			//
			// Save the project to the database
			m_Project.Save();

			lviNew.Selected = true;
		}

		private void btnDataValidationAdd_Click(object sender, EventArgs e)
		{
			//
			// Bring up the modeling Training selection dialog box; only allow files
			// that match the training file input count.
			int InputDimension = Convert.ToInt32(GPDatabaseUtils.FieldValue(m_Project.DataTrainingID, "tblModelingFile", "InputCount"));
			fmSelectModelingFile dlg = new fmSelectModelingFile(InputDimension);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				//
				// Add the selection to the project
				if (dlg.lvFiles.SelectedItems.Count > 0)
				{
					ListViewItem lvSelected = dlg.lvFiles.Items[dlg.lvFiles.SelectedItems[0].Index];
					ComboDataItem cbItem=new ComboDataItem(lvSelected.Text,Convert.ToInt32(lvSelected.Tag.ToString()));
					cbDataValidation.Items.Add(cbItem);
					//
					// Add it to the results page also
					cbResultsDataSet.Items.Add(cbItem);

					//
					// Select this item
					cbDataValidation.SelectedIndex = cbDataValidation.Items.Count - 1;

					//
					// Add it to the project and Save
					m_Project.DataValidation.Add(Convert.ToInt32(lvSelected.Tag.ToString()));
					m_Project.Save();
				}
			}
		}

		private void btnDataValidationDelete_Click(object sender, EventArgs e)
		{
			//
			// Remove it from the results page too
			cbResultsDataSet.Items.RemoveAt(cbDataValidation.SelectedIndex + 1);
			//
			// Delete the selected validation file
			m_Project.DataValidation.RemoveAt(cbDataValidation.SelectedIndex);
			cbDataValidation.Items.RemoveAt(cbDataValidation.SelectedIndex);

			//
			// Reselect a new one
			if (cbDataValidation.Items.Count > 0)
			{
				cbDataValidation.SelectedIndex = 0;
				cbResultsDataSet.SelectedIndex = 0;
			}

			//
			// Enable/Disable the delete button based upon count
			btnDataValidationDelete.Enabled=!(cbDataValidation.Items.Count == 0);

			m_Project.Save();
		}

		private void cbValidation_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbDataValidation.SelectedIndex >= 0)
			{
				btnDataValidationDelete.Enabled = true;
			}
			else
			{
				btnDataValidationDelete.Enabled = false;
			}
		}

		private void lvFunctions_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			Event_ProfileDirty(sender, e);
		}

		private void udStructure_PopSize_Validated(object sender, EventArgs e)
		{
			Event_ProfileDirty(sender, e);
		}

		private void FitnessRadioButton_Click(object sender, EventArgs e)
		{
			Event_ProfileDirty(sender, e);
		}
		private void chkFitness_MultiObjective_CheckedChanged(object sender, EventArgs e)
		{
			Event_ProfileDirty(sender, e);
		}

		private void chkFitness_MultiObjective_Click(object sender, EventArgs e)
		{
			//
			// Provide recommendations to the user
			if (chkFitness_MultiObjective.Checked)
			{
				String Recommendation = "When using Multi Objective Program Reduction, it is recommended ";
				Recommendation += "the Population Size is set to 1000, Reproduction set to 5%, Mutation set to 50%, ";
				Recommendation += "Crossover set to 45% and the Max Generations set in the range of 100 to 200.";

				MessageBox.Show(Recommendation,GPEnums.APPLICATON_NAME,MessageBoxButtons.OK);
			}
		}

		private void chkFitness_Parsimony_CheckedChanged(object sender, EventArgs e)
		{
			Event_ProfileDirty(sender, e);
		}
	}
}
