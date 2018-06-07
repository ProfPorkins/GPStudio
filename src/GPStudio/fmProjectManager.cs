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
using System.Windows.Forms;
using System.Data.OleDb;

namespace GPStudio.Client
{
	public partial class fmProjectManager : Form
	{
		public fmProjectManager()
		{
			InitializeComponent();

			InitializeProjectList();

			lvProjects.Focus();
		}

		//
		// Loads the list of Training files already imported into the database
		private void InitializeProjectList()
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			OleDbDataAdapter daFiles = new OleDbDataAdapter("SELECT DBCode,Name,Description FROM tblProject", con);
			DataSet dSet = new DataSet();
			daFiles.Fill(dSet);

			foreach (DataRow row in dSet.Tables[0].Rows)
			{
				ListViewItem lvItem = lvProjects.Items.Add(row.ItemArray.GetValue(1).ToString());
				lvItem.ToolTipText = row.ItemArray.GetValue(2).ToString();
				lvItem.Tag = row.ItemArray.GetValue(0);
				lvItem.ImageIndex = 0;
			}

			con.Close();
		}

		private void lvProjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool Status = true;

			if (lvProjects.SelectedItems.Count == 0)
			{
				Status = false;
			}
			else
			{
				//
				// Check to see if the project is already open
				if (((fmMain)this.Owner).ProjectOpen(Convert.ToInt32(lvProjects.SelectedItems[0].Tag.ToString())))
				{
					Status = false;
				}
			}

			btnOK.Enabled = Status;
			btnDelete.Enabled = Status;
		}

		private void lvProjects_DoubleClick(object sender, EventArgs e)
		{

			if (lvProjects.SelectedItems.Count > 0)
			{
				this.DialogResult = DialogResult.OK;
			}
		}

		private void lvProjects_KeyDown(object sender, KeyEventArgs e)
		{

			if (lvProjects.SelectedItems.Count == 0) return;

			if (e.KeyCode == Keys.Delete)
			{
				btnDelete_Click_1(sender, null);
			}
		}

		private void btnDelete_Click_1(object sender, EventArgs e)
		{
			if (MessageBox.Show("Confirm Project Removal?", "Project Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				ListViewItem lvSelected = lvProjects.Items[lvProjects.SelectedItems[0].Index];
				int ProjectID = Convert.ToInt32(lvSelected.Tag.ToString());

				if (GPProject.DeleteProject(ProjectID))
				{
					lvSelected.Remove();
				}
				else
				{
					MessageBox.Show("Unable to delete the project from the database.", "Project Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}