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
	public partial class fmSelectModelingFile : Form
	{
		public fmSelectModelingFile(int InputDimension)
		{
			InitializeComponent();
			InitializeFileList(InputDimension);
		}

		//
		// Loads the list of Training files already imported into the database.  Loads
		// only the list of files with the matching input dimension.
		private void InitializeFileList(int InputDimension)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			String sSQL = "SELECT DBCode,Name,Description FROM tblModelingFile";
			if (InputDimension > 0)
			{
				sSQL += " WHERE InputCount = " + InputDimension;
			}
			
			OleDbDataAdapter daFiles = new OleDbDataAdapter(sSQL, con);
			DataSet dSet = new DataSet();
			daFiles.Fill(dSet);

			foreach (DataRow row in dSet.Tables[0].Rows)
			{
				ListViewItem lvItem = lvFiles.Items.Add(row.ItemArray.GetValue(1).ToString());
				lvItem.ToolTipText = row.ItemArray.GetValue(2).ToString();
				lvItem.Tag = row.ItemArray.GetValue(0);
				lvItem.Group = lvFiles.Groups[0];
			}

			con.Close();
		}

		private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// Update the UI controls based upon item selection status
			if (lvFiles.SelectedIndices.Count > 0)
			{
				btnOK.Enabled = true;
			}
			else
			{
				btnOK.Enabled = false;
			}
		}
	}
}