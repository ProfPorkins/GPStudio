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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GPStudio.Interfaces;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public partial class fmFunctionEditor : Form
	{
		/// <summary>
		/// Property that indicates whether or not this form is already up
		/// and running
		/// </summary>
		public static bool Active
		{
			get { return fmFunctionEditor.m_bActive; }
			set { fmFunctionEditor.m_bActive = value; }
		}
		private static bool m_bActive = false;

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
		private bool m_FunctionDirty = false;

		private bool ProgramUpdating
		{
			get { return m_ProgramUpdating; }
			set { m_ProgramUpdating = value; }
		}
		private bool m_ProgramUpdating = false;

		private SortedDictionary<String, int> m_FunctionCategory=new SortedDictionary<string,int>();

		public fmFunctionEditor()
		{
			InitializeComponent();

			Active = true;
			this.FunctionDirty = false;
			this.ProgramUpdating = false;

			//
			// Prepare the list of function categories
			InitializeCategories();

			//
			// Default: Select the C# language
			tsMainLanguage.DropDownItems[2].PerformClick();
		}

		/// <summary>
		/// Loads the list of function categories available for selection
		/// </summary>
		private void InitializeCategories()
		{
			//
			// Clear any previous categories
			cbCategory.Items.Clear();

			//
			// Build the query
			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{

				String SQL = "SELECT DBCode,Name FROM tblFunctionCategory ORDER BY Name";
				OleDbDataAdapter daCategories = new OleDbDataAdapter(SQL, con);
				DataSet dSet = new DataSet();
				daCategories.Fill(dSet);

				foreach (DataRow row in dSet.Tables[0].Rows)
				{
					String Name = row.ItemArray.GetValue(1).ToString();
					int CategoryID = Convert.ToInt32(row.ItemArray.GetValue(0).ToString());

					//
					// Add it to the UI
					cbCategory.Items.Add(row.ItemArray.GetValue(1).ToString());
					//
					// Maintain a list of categories and their associated DBCodes
					m_FunctionCategory[Name] = CategoryID;
				}
			}
		}

		private void fmFunctionSet_FormClosed(object sender, FormClosedEventArgs e)
		{
			//
			// Set the Active flag to false so a new copy of this window 
			// can be created.
			Active = false;
		}

		/// <summary>
		/// Load the list of functions into the listview, select from the
		/// indicated language.
		/// </summary>
		/// <param name="FunctionLanguageID">Which language to load from</param>
		private void InitializeFunctionSet(String FunctionLanguageID)
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

			String sSQL="SELECT DBCode,Name FROM tblFunctionSet WHERE FunctionLanguageID = "+FunctionLanguageID+" ORDER BY Name";
			OleDbDataAdapter daFiles = new OleDbDataAdapter(sSQL, con);
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
				lvFunctions.Items[0].Selected=true;
			}

			con.Close();

			this.ProgramUpdating = false;
		}

		private void lvFunctions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			//
			// If the previous selection is dirty, ask the user if they want to save first
			if (FunctionDirty && !this.ProgramUpdating && !e.IsSelected)
			{
				if (MessageBox.Show("Current function has been modified, would you like to save it first?", "User Defined Functions", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					SaveFunction(e.ItemIndex);
				}
			}

			FunctionDirty = false;

			this.ProgramUpdating = true;
			//
			// Enable the controls as appropriate
			tsMainDelete.Enabled = e.IsSelected;
			tsMainSave.Enabled = false;
			tsMainValidate.Enabled = e.IsSelected;
			//
			// Only enable validate if the C# language is selected
			if (tsMainLanguage.Text != tsMainLanguageItem_CSharp.Text)
			{
				tsMainValidate.Enabled = false;
			}

			txtFunctionName.ReadOnly = !e.IsSelected;
			txtSource.ReadOnly = !e.IsSelected;
			txtDescription.ReadOnly = !e.IsSelected;
			udParamterCount.Enabled = e.IsSelected;
			cbCategory.Enabled = e.IsSelected;
			chkTerminalParameters.Enabled = e.IsSelected;

			//
			// If a selection is going away, reset the view, otherwise
			// get the selected function loaded up and stuff
			if (e.IsSelected)
			{
				//
				// Grab the DBCode and use it to get the function loaded up
				int DBCode=(int)e.Item.Tag;
				bool IsMasterFunction=DisplayFunction(DBCode);
				//
				// Generate a prototype
				DisplayPrototype();

				//
				// If this is a master function, don't allow any changes
				if (IsMasterFunction)
				{
					txtFunctionName.ReadOnly = true;
					txtSource.ReadOnly=true;
					txtDescription.ReadOnly=true;
					udParamterCount.Enabled = false;
					cbCategory.Enabled = false;
					chkTerminalParameters.Enabled = false;

					txtSource.BackColor = SystemColors.ControlLight;
					txtDescription.BackColor = SystemColors.ControlLight;
				}
			}
			else
			{
				txtSource.Text = "";
				txtDescription.Text = "";
				txtFunctionName.Text = "";
				lvErrors.Items.Clear();

				txtSource.BackColor = SystemColors.Window;
				txtDescription.BackColor = SystemColors.Window;
			}

			this.ProgramUpdating = false;
		}

		/// <summary>
		/// Given the DBCode, Load and display that function
		/// </summary>
		/// <param name="FunctionID">DBCode of the function to load</param>
		/// <returns>True if this is a master function, False otherwise</returns>
		private bool DisplayFunction(long FunctionID)
		{
			bool IsMasterFunction = false;

			using (OleDbConnection con = GPDatabaseUtils.Connect())
			{
				String SQL = "SELECT tblFunctionSet.Name,tblFunctionSet.Description";
				SQL += ",tblFunctionSet.Arity,tblFunctionSet.Code,tblFunctionSet.MasterLock,";
				SQL += "tblFunctionSet.Validated,tblFunctionCategory.Name,tblFunctionSet.TerminalParameters ";
				SQL += "FROM tblFunctionSet ";
				SQL += "INNER JOIN tblFunctionCategory ON tblFunctionSet.CategoryID = tblFunctionCategory.DBCode  ";
				SQL += "WHERE tblFunctionSet.DBCode = " + FunctionID;

				OleDbDataAdapter daFiles = new OleDbDataAdapter(SQL, con);
				DataSet dSet = new DataSet();
				daFiles.Fill(dSet);

				//
				// There better be a single row! :)
				DataRow row = dSet.Tables[0].Rows[0];
				txtFunctionName.Text = row.ItemArray.GetValue(0).ToString();
				udParamterCount.Value = Convert.ToDecimal(row.ItemArray.GetValue(2).ToString());
				txtSource.Text = row.ItemArray.GetValue(3).ToString();
				txtDescription.Text = row.ItemArray.GetValue(1).ToString();
				IsMasterFunction = Convert.ToBoolean(row.ItemArray.GetValue(4).ToString());
				chkValidated.Checked = Convert.ToBoolean(row.ItemArray.GetValue(5).ToString());
				cbCategory.SelectedIndex=cbCategory.Items.IndexOf(row.ItemArray.GetValue(6).ToString());
				chkTerminalParameters.Checked = Convert.ToBoolean(row.ItemArray.GetValue(7).ToString());
			}

			return IsMasterFunction;
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
			string sSQL = "UPDATE tblFunctionSet SET ";
			sSQL += "Name = '" + txtFunctionName.Text + "'";
			sSQL += ", Description = '" + txtDescription.Text + "'";
			sSQL += ", Arity = " + (int)udParamterCount.Value;
			sSQL += ", TerminalParameters = " + chkTerminalParameters.Checked;
			sSQL += ", Code = '" + txtSource.Text + "'";
			sSQL += ", CategoryID = "+m_FunctionCategory[cbCategory.Text];
			sSQL += ", Validated = False";

			sSQL+=" WHERE DBCode = "+FunctionID;

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

		//
		// Takes the settings by the user and creates a function prototype
		// to help the user think about the code they are writing.
		//
		// TODO: The prototype writing functions should be static members of
		// the language writer classes.  This should also prolly be a delegate
		// to make things more efficient and elegant
		private void DisplayPrototype()
		{

			//
			// Build a language appropriate prototype
			switch (Convert.ToInt32(tsMainLanguage.Tag))
			{
				case GPEnums.LANGUAGEID_C:
					lbPrototype.Text = WritePrototype_C(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
				case GPEnums.LANGUAGEID_CPP:
					lbPrototype.Text = WritePrototype_CPP(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
				case GPEnums.LANGUAGEID_CSHARP:
					lbPrototype.Text = WritePrototype_CSharp(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
				case GPEnums.LANGUAGEID_VB:
					lbPrototype.Text = "Not Yet Implemented";
					break;
				case GPEnums.LANGUAGEID_VBNET:
					lbPrototype.Text = WritePrototype_VBNet(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
				case GPEnums.LANGUAGEID_JAVA:
					lbPrototype.Text = WritePrototype_Java(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
				case GPEnums.LANGUAGEID_FORTRAN:
					lbPrototype.Text = WritePrototype_Fortran(txtFunctionName.Text, (int)udParamterCount.Value);
					break;
			}
		}

		/// <summary>
		/// Writes a C style function prototype
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_C(String FunctionName, int Arity)
		{
			//
			// All functions return a double, by definition
			String Prototype = "double " + FunctionName + "(";

			//
			// Fill out the parameters
			for (int Param = 0; Param < Arity; Param++)
			{
				//
				// See if a comma should be added
				if (Param > 0)
				{
					Prototype += ", ";
				}

				Prototype += "double p" + Param;
			}

			//
			// I just about forgot the closing paren
			Prototype += ")";

			return Prototype;
		}

		/// <summary>
		/// Writes a C++ style function prototype
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_CPP(String FunctionName, int Arity)
		{
			//
			// All functions return a double, by definition
			String Prototype = "double " + FunctionName + "(vector<vector<double>> InputHistory";

			//
			// Fill out the parameters
			for (int Param = 0; Param < Arity; Param++)
			{
				Prototype += ", double p" + Param;
			}

			//
			// I just about forgot the closing paren
			Prototype += ")";

			return Prototype;
		}

		/// <summary>
		/// Writes a C# style function prototype
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_CSharp(String FunctionName,int Arity)
		{
			//
			// All functions return a double, by definition
			String Prototype = "public double " + FunctionName + "(List<List<double>> InputHistory";

			//
			// Fill out the parameters
			for (int Param = 0; Param < Arity; Param++)
			{
				Prototype += ",double p" + Param;
			}

			//
			// I just about forgot the closing paren
			Prototype += ")";

			return Prototype;
		}

		/// <summary>
		/// Writes a VB.NET style function prototype
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_VBNet(String FunctionName, int Arity)
		{
			//
			// All functions return a double, by definition
			String Prototype = "Public Function " + FunctionName + "(";
			Prototype += "ByVal InputHistory As List(Of List(Of Double))";

			//
			// Fill out the parameters
			for (int Param = 0; Param < Arity; Param++)
			{
				Prototype += ",ByVal p" + Param+" As Double";
			}

			//
			// I just about forgot the closing paren
			Prototype += ") As Double";

			return Prototype;
		}

		/// <summary>
		/// Writes a Java style function prototype.  Reusues the C# code because
		/// they are exactly the same.
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_Java(String FunctionName, int Arity)
		{
			//
			// All functions return a double, by definition
			String Prototype = "public double " + FunctionName + "(ArrayList<ArrayList<Double>> InputHistory";

			//
			// Fill out the parameters
			for (int Param = 0; Param < Arity; Param++)
			{
				Prototype += ",double p" + Param;
			}

			//
			// I just about forgot the closing paren
			Prototype += ")";

			return Prototype;
		}

		/// <summary>
		/// Writes a Fortran style function prototype
		/// </summary>
		/// <param name="FunctionName">Text name of the function</param>
		/// <param name="Arity"># of function parameters</param>
		/// <returns>String representation of the prototype</returns>
		private String WritePrototype_Fortran(String FunctionName, int Arity)
		{
			StringBuilder Prototype = new StringBuilder();
			Prototype.Append("REAL FUNCTION " + FunctionName + "(");
			for (short Param = 0; Param < Arity; Param++)
			{
				//
				// See if we should add a comma
				if (Param > 0)
				{
					Prototype.Append(", ");
				}
				Prototype.Append("p" + Param);
			}
			Prototype.Append(")");

			return Prototype.ToString();
		}

		private void udParamterCount_ValueChanged(object sender, EventArgs e)
		{
			//
			// Update the prototype
			DisplayPrototype();

			if (!this.ProgramUpdating)	FunctionDirty = true;
		}

		private void txtFunctionName_TextChanged(object sender, EventArgs e)
		{
			//
			// Update the prototype
			DisplayPrototype();

			if (!this.ProgramUpdating) FunctionDirty = true;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveFunction(lvFunctions.SelectedItems[0].Index);
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

			FunctionDirty = false;
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			//
			// Create a blank entry in the database for a new function
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to insert a new record in the DB
			String sSQL = "INSERT INTO tblFunctionSet(Name,Description,Arity,TerminalParameters,Code,FunctionLanguageID,CategoryID) ";
			//
			// Get the language ID
			String FunctionLanguageID = (String)tsMainLanguage.Tag;
			sSQL = sSQL + "VALUES ('New_Function','Function Description',1,false,'',"+FunctionLanguageID+","+m_FunctionCategory["Other"]+")";

			OleDbCommand cmd = new OleDbCommand(sSQL, con);

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
				ListViewItem item=lvFunctions.Items.Add("New_Function");
				item.Tag=FunctionID;
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

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvFunctions.SelectedIndices.Count <= 0) return;

			//
			// Confirm from the user this item should be deleted
			if (MessageBox.Show("Confirm Function Removal?", GPEnums.APPLICATON_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
			OleDbCommand cmd = new OleDbCommand("DELETE FROM tblFunctionSet WHERE DBCode = " + FunctionID, con);

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

		private void btnValidate_Click(object sender, EventArgs e)
		{
			//
			// Reset any previous errors
			lvErrors.Items.Clear();

			//
			// Obtain the compiler interface from the local object
			IGPCompiler iCompiler = fmMain.ServerManager.LocalServer.Compiler;

			//
			// Get the function written
			String FunctionCode = iCompiler.WriteUserFunction(
				txtFunctionName.Text,
				(short)udParamterCount.Value,
				txtSource.Text);

			//
			// Compile and check for errors
			String[] Errors;
			bool Validated = iCompiler.ValidateUserFunction(
				txtFunctionName.Text,
				FunctionCode,
				out Errors);

			if (!Validated)
			{
				//
				// Fill the listview with the errors
				foreach (String Item in Errors)
				{
					ListViewItem lviError = lvErrors.Items.Add(Item);
					lviError.ToolTipText = Item;
				}

				tabFooter.SelectedTab = pageErrors;
				//
				// Update the database to reflect this function is validated
				SetValidation(Convert.ToInt32(lvFunctions.SelectedItems[0].Tag), false);
				chkValidated.Checked = false;

				//
				// Let the user know there were errors
				MessageBox.Show("Function failed validation!", "Function Set Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				//
				// Update the database to reflect this function is validated
				SetValidation(Convert.ToInt32(lvFunctions.SelectedItems[0].Tag), true);
				chkValidated.Checked = true;

				MessageBox.Show("Function correctly validated!", "Function Set Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			string sSQL = "UPDATE tblFunctionSet SET ";
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

		private void tsMainLanguage_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

			//
			// change the text to the selected item
			tsMainLanguage.Tag = e.ClickedItem.Tag;
			tsMainLanguage.Text = e.ClickedItem.Text;
		}

		private void tsMainLanguage_TextChanged(object sender, EventArgs e)
		{
			//
			// Update the list of functions
			InitializeFunctionSet((String)tsMainLanguage.Tag);
		}

		private void txtSource_TextChanged(object sender, EventArgs e)
		{
			if (!this.ProgramUpdating) FunctionDirty = true;
		}

		private void cbCategory_TextChanged(object sender, EventArgs e)
		{
			if (!this.ProgramUpdating) FunctionDirty = true;
		}

		private void chkTerminalParameters_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.ProgramUpdating) FunctionDirty = true;
		}
	}
}