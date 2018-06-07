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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public partial class fmFitnessEditor : Form
	{
		private static bool m_bActive = false;
		/// <summary>
		/// Property that indicates whether or not this form is already up
		/// and running
		/// </summary>
		public static bool Active
		{
			get { return fmFitnessEditor.m_bActive; }
			set { fmFitnessEditor.m_bActive = value; }
		}

		private bool m_FunctionDirty = false;
		/// <summary>
		/// Property that indicates if any changes have been made to the
		/// function properties.  If it is dirty, it needs to be saved.
		/// </summary>
		private bool FunctionDirty
		{
			get { return m_FunctionDirty; }
			set
			{
				m_FunctionDirty = value;
				tsMainSave.Enabled = value;	// Update the corresponding UI save button
			}
		}

		private bool m_ProgramUpdating = false;
		private bool ProgramUpdating
		{
			get { return m_ProgramUpdating; }
			set { m_ProgramUpdating = value; }
		}

		public fmFitnessEditor()
		{
			InitializeComponent();

			//
			// Set the prototype description (C# only)
			lbPrototype.Text="double ComputeFitness(List<List<double>> InputHistory,double[] Predictions, double[] Training, double TrainingAverage,double Tolerance)";

			Active = true;
			this.FunctionDirty = false;
			this.ProgramUpdating = false;

			InitializeFunctionList();
		}

		private void fmFitnessEditor_FormClosed(object sender, FormClosedEventArgs e)
		{
			//
			// Set the Active flag to false so a new copy of this window 
			// can be created.
			Active = false;
		}

		/// <summary>
		/// Load the list of fitness functions into the list view
		/// </summary>
		private void InitializeFunctionList()
		{
			this.ProgramUpdating = true;

			//
			// If an item is selected, unselect it.  This causes the form
			// to revert to an initial disabled state
			if (lvFunctions.SelectedItems.Count > 0)
			{
				lvFunctions.SelectedItems[0].Selected = false;
			}

			//
			// Clear any previous functions
			lvFunctions.Items.Clear();

			//
			// Get the next set queried up!
			OleDbConnection con = GPDatabaseUtils.Connect();

			String SQL = "SELECT DBCode,Name FROM tblFitnessFunction ORDER BY Name";
			OleDbDataAdapter daFiles = new OleDbDataAdapter(SQL, con);
			DataSet dSet = new DataSet();
			daFiles.Fill(dSet);

			foreach (DataRow row in dSet.Tables[0].Rows)
			{
				ListViewItem lvItem = lvFunctions.Items.Add(row.ItemArray.GetValue(1).ToString());
				lvItem.Group = lvFunctions.Groups[0];
				lvItem.Tag = row.ItemArray.GetValue(0);
			}

			//
			// Autoselect the first one (if any)
			if (lvFunctions.Items.Count > 0)
			{
				lvFunctions.Items[0].Selected = true;
			}

			con.Close();

			this.ProgramUpdating = false;
		}

		private void tsMainAdd_Click(object sender, EventArgs e)
		{
			//
			// Create a blank entry in the database for a new function
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to insert a new record in the DB
			String SQL = "INSERT INTO tblFitnessFunction(Name,Code) ";
			SQL += "VALUES ('New_Function','-- Source Code Goes Here ---')";

			OleDbCommand cmd = new OleDbCommand(SQL, con);

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
				int FunctionID = Convert.ToInt32(reader[0].ToString()); ;
				reader.Close();

				//
				// Add the new item to the list of functions and get it
				// displayed
				ListViewItem item = lvFunctions.Items.Add("New_Function");
				item.Tag = FunctionID;
				item.Group = lvFunctions.Groups[0];
				item.Selected = true;
			}
			catch (OleDbException ex)
			{
				string sError = ex.Message.ToString();
			}

			con.Close();

			if (!this.ProgramUpdating) FunctionDirty = true;

			//
			// Place the focus on the function name text box so the user
			// can start typing the name in without doing anything else.
			txtFunctionName.Focus();
		}

		private void lvFunctions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			//
			// If the previous selection is dirty, ask the user if they want to save first
			if (FunctionDirty && !this.ProgramUpdating && !e.IsSelected)
			{
				if (MessageBox.Show("Current function has been modified, would you like to save it first?", "Fitness Functions", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					SaveFunction(e.ItemIndex);
				}
			}

			this.FunctionDirty = false;
			this.ProgramUpdating = true;
			//
			// Enable the controls as appropriate
			tsMainDelete.Enabled = e.IsSelected;
			tsMainSave.Enabled = false;
			tsMainValidate.Enabled = e.IsSelected;

			txtFunctionName.Enabled = e.IsSelected;
			txtSource.ReadOnly = !e.IsSelected;
			txtSource.BackColor = SystemColors.Window;

			//
			// If a selection is going away, reset the view, otherwise
			// get the selected function loaded up and stuff
			if (e.IsSelected)
			{
				//
				// Grab the DBCode and use it to get the function loaded up
				int DBCode = (int)e.Item.Tag;
				bool IsMasterFunction = DisplayFunction(DBCode);

				//
				// If this is a master function, don't allow any changes.
				// If this function is in use, don't allow any changes.
				if (IsMasterFunction || IsFunctionInUse(DBCode))
				{
					txtFunctionName.Enabled = false;
					txtSource.ReadOnly = true;
					tsMainDelete.Enabled = false;

					txtSource.BackColor = SystemColors.ControlLight;
				}
			}
			else
			{
				txtSource.Text = "";
				txtFunctionName.Text = "";
				lvErrors.Items.Clear();
			}

			this.ProgramUpdating = false;
		}

		/// <summary>
		/// Given the DBCode, Load and display that function
		/// </summary>
		/// <param name="FunctionID">DBCode of the function to load</param>
		/// <returns>True if this is a master function, False otherwise</returns>
		private bool DisplayFunction(int FunctionID)
		{
			bool IsMasterFunction = false;

			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				String SQL = "SELECT Name,Code,MasterLock,Validated FROM tblFitnessFunction ";
				SQL += "WHERE DBCode = " + FunctionID;

				OleDbDataAdapter daFiles = new OleDbDataAdapter(SQL, con);
				DataSet dSet = new DataSet();
				daFiles.Fill(dSet);

				//
				// There better be a single row! :)
				DataRow row = dSet.Tables[0].Rows[0];
				txtFunctionName.Text = row.ItemArray.GetValue(0).ToString();
				txtSource.Text = row.ItemArray.GetValue(1).ToString();
				IsMasterFunction = Convert.ToBoolean(row.ItemArray.GetValue(2).ToString());
				chkValidated.Checked = Convert.ToBoolean(row.ItemArray.GetValue(3).ToString());
				//
				// Also need to see if it is in use
				chkInUse.Checked = IsFunctionInUse(FunctionID);
			}

			return IsMasterFunction;
		}

		/// <summary>
		/// Checks to see if this fitness function is in use.
		/// </summary>
		/// <param name="FunctionID">DBCode of the function to test</param>
		/// <returns>True if it is in use, False otherwise</returns>
		private bool IsFunctionInUse(int FunctionID)
		{
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				string sSQL = "SELECT DBCode FROM tblModelProfile ";
				sSQL += "WHERE FitnessFunctionID = " + FunctionID;
				OleDbDataAdapter daFiles = new OleDbDataAdapter(sSQL, con);
				DataSet dSet = new DataSet();
				daFiles.Fill(dSet);

				if (dSet.Tables[0].Rows.Count > 0)
				{
					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Get the function saved and the UI updated accordingly
		/// </summary>
		/// <param name="ListViewPosition">Position of the function in the listview</param>
		private void SaveFunction(int ListViewPosition)
		{
			//
			// Take the current UI settings and Save it to the selected function
			SaveFunctionToDB(ListViewPosition);
			//
			// Update the listview caption
			lvFunctions.Items[ListViewPosition].Text = txtFunctionName.Text;

			//
			// By definition, the function is not validated
			chkValidated.Checked = false;

			this.FunctionDirty = false;
		}

		/// <summary>
		/// Save the current function back out to the DB, this is an UPDATE
		/// function, not an add a new one to the database type function.
		/// </summary>
		/// <param name="ListViewPosition">Position of the function in the listview</param>
		private void SaveFunctionToDB(int ListViewPosition)
		{
			//
			// Get the DBCode first
			int FunctionID = (int)lvFunctions.Items[ListViewPosition].Tag;
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to update the various values
			string sSQL = "UPDATE tblFitnessFunction SET ";
			sSQL += "Name = '" + txtFunctionName.Text + "'";
			sSQL += ", Code = '" + txtSource.Text + "'";
			sSQL += ", Validated = False";

			sSQL += " WHERE DBCode = " + FunctionID;

			OleDbCommand cmd = new OleDbCommand(sSQL, con);

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

		private void txtFunctionName_TextChanged(object sender, EventArgs e)
		{
			if (!this.ProgramUpdating) FunctionDirty = true;
		}

		private void txtSource_TextChanged(object sender, EventArgs e)
		{
			if (!this.ProgramUpdating) FunctionDirty = true;
		}

		private void tsMainSave_Click(object sender, EventArgs e)
		{
			SaveFunction(lvFunctions.SelectedItems[0].Index);
		}

		private void tsMainDelete_Click(object sender, EventArgs e)
		{
			if (lvFunctions.SelectedIndices.Count <= 0) return;

			//
			// Confirm from the user this item should be deleted
			if (MessageBox.Show("Confirm Fitness Function Removal?", GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				int FunctionID = Convert.ToInt32(lvFunctions.Items[lvFunctions.SelectedIndices[0]].Tag.ToString());
				if (DeleteFunctionFromDB(FunctionID))
				{
					lvFunctions.Items[lvFunctions.SelectedIndices[0]].Remove();
				}
				else
				{
					MessageBox.Show("Error in removing the Function.  Check to see that it is not in use by a Modeling Profile", GPEnums.APPLICATON_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			if (!this.ProgramUpdating) FunctionDirty = false;
		}

		/// <summary>
		/// Remove the selected function ID from the database
		/// </summary>
		/// <param name="FunctionID">Which function to remove</param>
		/// <returns>True/False upon success or failure</returns>
		private bool DeleteFunctionFromDB(int FunctionID)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to add the blob to the database
			OleDbCommand cmd = new OleDbCommand("DELETE FROM tblFitnessFunction WHERE DBCode = " + FunctionID, con);

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

		private void tsMainValidate_Click(object sender, EventArgs e)
		{
			//
			// Reset any previous errors
			lvErrors.Items.Clear();

			//
			// Obtain the compiler interface from the local object
			IGPCompiler iCompiler = (IGPCompiler)fmMain.ServerManager.LocalServer.Compiler;

			//
			// Get the Fitness class written
			String FunctionCode = iCompiler.WriteFitnessClass(txtSource.Text);

			String[] Errors = null;
			bool Validated = iCompiler.ValidateFitnessClass(FunctionCode, out Errors);
			//
			// Update the validation status
			SetValidation(Convert.ToInt32(lvFunctions.SelectedItems[0].Tag), Validated);
			chkValidated.Checked = Validated;
			//
			// Let the user know what happened
			if (!Validated)
			{
				//
				// Fill the listview with the errors
				foreach (String Item in Errors)
				{
					ListViewItem lviError = lvErrors.Items.Add(Item);
					lviError.ToolTipText = Item;
				}

				//
				// Let the user know there were errors
				MessageBox.Show("Function failed validation!", "Fitness Functions", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				MessageBox.Show("Function correctly validated!", "Fitness Functions", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// Updates the database entry of this function to be validated or not
		/// validated according to the Validated parameter.
		/// </summary>
		/// <param name="FunctionID">DBCode of the function</param>
		/// <param name="Validated">Validation state of the function</param>
		private void SetValidation(int FunctionID, bool Validated)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to update the various values
			string sSQL = "UPDATE tblFitnessFunction SET ";
			sSQL += "Validated = " + Validated.ToString();
			sSQL += " WHERE DBCode = " + FunctionID;

			OleDbCommand cmd = new OleDbCommand(sSQL, con);

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
	}
}
