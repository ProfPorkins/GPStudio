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
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using GPStudio.Shared;

namespace GPStudio.Client
{
	public partial class fmDataManager : Form
	{
		private static bool m_bActive = false;
		private bool m_bLoadFile = true;

		public fmDataManager()
		{
			InitializeComponent();

			graphData.GraphPane.Fill = new ZedGraph.Fill(Color.AliceBlue, Color.WhiteSmoke, 0F);
			graphData.GraphPane.Chart.Fill = new ZedGraph.Fill(Color.Silver, Color.White, 45.0f);

			//
			// Load up the list of existing files from the database
			InitializeFileList();

			//
			// Set Active to true so only one copy of this window can be created
			Active = true;
		}

		//
		// Property for m_bActive
		public static bool Active
		{
			get { return m_bActive; }
			set { m_bActive = value; }
		}

		private void fmDataImport_FormClosed(object sender, FormClosedEventArgs e)
		{
			//
			// Set the Active flag to false so a new copy of this window 
			// can be created.
			Active = false;
		}

		//
		// These are methods which get passed in as delegates to the modeling
		// Training object so the UI can be updated as to what is going on.
		private void InitProgress(int MaxValue)
		{
			tsProgress.Maximum = MaxValue;
		}
		private void IncrementProgress()
		{
			tsProgress.Increment(1);
			Application.DoEvents();
		}

		/// <summary>
		/// Used to give Input/Target column headers for the Training just loaded
		/// </summary>
		private void NameColumns()
		{
			foreach (DataGridViewColumn dgColumn in dgView.Columns)
			{
				//
				// Only the last column is a target column
				if (dgColumn.Index == (dgView.Columns.Count-1))
				{
					dgColumn.HeaderText = m_gpData.Header[dgView.Columns.Count - 1];
				}
				else
				{
					dgColumn.HeaderText=m_gpData.Header[dgColumn.Index];
				}
			}

			//
			// Go through each row and give it a number
			foreach (DataGridViewRow dgRow in dgView.Rows)
			{
				int value = dgRow.Index + 1;
				dgRow.HeaderCell.Value = value.ToString();
			}
		}

		/// <summary>
		/// Loads the list of data files imported into the database into the
		/// left side listview display.
		/// </summary>
		private void InitializeFileList()
		{
			//
			// Get the DB connection
			using (OleDbConnection DBConnection = GPDatabaseUtils.Connect())
			{
				OleDbCommand cmd = new OleDbCommand("SELECT DBCode,Name,Description FROM tblModelingFile;", DBConnection);
				OleDbDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					ListViewItem lvItem = lvFiles.Items.Add(rdr["Name"].ToString());
					lvItem.Group = lvFiles.Groups[0];
					lvItem.ImageIndex = 0;
					lvItem.ToolTipText = rdr["Description"].ToString();
					lvItem.Tag = Convert.ToInt32(rdr["DBCode"]);
				}
			}
		}

		/// <summary>
		/// Fill the graph with the Training set
		/// </summary>
		/// <param name="Training">Reference to the Training object to be plotted</param>
		private void DisplayDataGraph(GPModelingData gpData)
		{
			//
			// If nothing is selected, get the hell out of here
			if (lvFiles.SelectedItems.Count == 0) return;

			//
			// Initialize the graph pane
			InitialzeDataGraph(gpData);

			//
			// The the X & Y Values
			double[] XValues = null;
			double[] YValues = null;
			if (gpData.TimeSeries)
			{
				//
				// If time series, use a count as the X-Axis
				XValues = new double[gpData.Rows];
				YValues = new double[gpData.Rows];
				for (int Value = 0; Value <gpData.Rows; Value++)
				{
					XValues[Value] = Value+1;
					YValues[Value] = gpData[Value , 0];
				}
			}
			else
			{
				XValues = gpData.InputColumn(cbXAxis.SelectedIndex);
				YValues = gpData.ObjectiveColumn(0);
			}
			
			if (rdChartTypeXY.Checked || rdChartTypeScatter.Checked)
			{
				ZedGraph.LineItem line=graphData.GraphPane.AddCurve("Series 1", XValues, YValues, Color.Blue, ZedGraph.SymbolType.None);
				if (rdChartTypeScatter.Checked)
				{
					line.Line.IsVisible = false;
					line.Symbol.Type = ZedGraph.SymbolType.XCross;
					line.Symbol.Size = 3;
				}
			}
			else
			if (rdChartTypeBar.Checked)
			{
				graphData.GraphPane.AddBar("Series 1", XValues, YValues, Color.Blue);
			}

			//
			// Force the graph to visually update
			graphData.GraphPane.AxisChange(this.CreateGraphics());
			graphData.Invalidate();
		}

		/// <summary>
		/// Prepare the graph to accept the plotted data
		/// </summary>
		/// <param name="gpData"></param>
		private void InitialzeDataGraph(GPModelingData gpData)
		{
			ZedGraph.GraphPane pane = new ZedGraph.GraphPane(
				new Rectangle(0, 0, graphData.Width, graphData.Height),
				"Modeling Data",
				cbXAxis.Items[cbXAxis.SelectedIndex].ToString(),
				gpData.Header[m_gpData.Header.Length-1]);

			pane.Fill = new ZedGraph.Fill(Color.AliceBlue, Color.WhiteSmoke, 0F);
			pane.Chart.Fill = new ZedGraph.Fill(Color.Silver, Color.White, 45.0f);


			//
			// IF this is a time series, the x-axis is NOT whatever the combo box is
			// it is just a count
			if (gpData.TimeSeries)
			{
				pane.XAxis.Title.Text = "Count";
			}

			//
			// No need for a legend here
			pane.Legend.IsVisible = false;

			//
			// If we are in Bar plot mode, set up accordingly
			if (rdChartTypeBar.Checked)
			{
				//
				// Use a value or count bar axis
				string[] XLabels = new string[gpData.Rows];
				if (chkCount.Checked)
				{
					//
					// Build a set of lables using a row count
					for (int nRow = 0; nRow < gpData.Rows; nRow++)
					{
						XLabels[nRow] = ((int)(nRow + 1)).ToString();
					}
					pane.XAxis.Title.Text = "Count";
				}
				else
				{
					//
					// Build a set of labels from the common X-Axis
					for (int nRow = 0; nRow < gpData.Rows; nRow++)
					{
						XLabels[nRow] = gpData[nRow, 0].ToString();
					}
				}

				pane.XAxis.Type = ZedGraph.AxisType.Text;
				pane.XAxis.Scale.TextLabels = XLabels;
				pane.XAxis.MajorTic.IsBetweenLabels = true;
			}

			graphData.GraphPane = pane;
		}

		/// <summary>
		/// Setup and fill the grid with the data
		/// </summary>
		/// <param name="gpData"></param>
		private void DisplayDataGrid(GPModelingData gpData)
		{
			//
			// Set a busy cursor
			Cursor PrevCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			tsLabel.Text = "Filling Grid";
			Application.DoEvents();

			//
			// Reset the grid
			dgView.Rows.Clear();

			//
			// Get the number of columns, this is the input + objective count
			dgView.ColumnCount = gpData.Columns + gpData.Objectives;
			//
			// Set the view style for the cells
			for (int Column = 0; Column < dgView.ColumnCount; Column++)
			{
				dgView.Columns[Column].DefaultCellStyle.Format = GPEnums.NUMERIC_DISPLAY_FORMAT;
				dgView.Columns[Column].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			dgView.Rows.Add(gpData.Rows);

			for (int Row = 0; Row < gpData.Rows; Row++)
			{
				for (int Input = 0; Input < gpData.Columns; Input++)
				{
					dgView.Rows[Row].Cells[Input].Value = gpData[Row, Input];
				}

				for (int Objective = 0; Objective < gpData.Objectives; Objective++)
				{
					dgView.Rows[Row].Cells[gpData.Columns + Objective].Value = gpData.ObjectiveRow(Row)[Objective];
				}
			}

			//
			// Replace the cursor
			this.Cursor = PrevCursor;
			tsLabel.Text = "";
		}

		private bool m_DeletingItem = false;
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvFiles.SelectedIndices.Count <= 0) return;

			//
			// Ask the user if they are really sure about this
			if (MessageBox.Show("Confirm File Delete?","GP Studio Data Manager",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				//
				// Delete the current selection
				int ModelingFileID = Convert.ToInt32(lvFiles.Items[lvFiles.SelectedIndices[0]].Tag.ToString());
				GPModelingData.DeleteData(ModelingFileID);

				//
				// Remove the entry from the list of files
				m_DeletingItem = true;
				lvFiles.SelectedItems[0].Remove();
				m_DeletingItem = false;
				//
				// Select a new data file, if'n one exists
				if (lvFiles.Items.Count > 0)
				{
					lvFiles.Items[0].Selected = true;
				}
			}
		}

		private void rdChartType_OptionChanged(object sender, EventArgs e)
		{
			DisplayDataGraph(m_gpData);
		}

		private void lvFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			//
			// Update the name in the database
			// There seems to be a strange bug in this event.  If the user leaves the
			// label highlighted and doesn't make a change, the label comes in as null,
			// so, in that case, don't do anything.
			if (e.Label != null)
			{
				int ModelingFileID = Convert.ToInt32(lvFiles.Items[e.Item].Tag.ToString());
				UpdateFileLabel(ModelingFileID, e.Label);
			}
		}

		//
		// Update the label of the indicated File ID
		private void UpdateFileLabel(int ModelingFileID, String Name)
		{
			OleDbConnection con = GPDatabaseUtils.Connect();

			//
			// Build the command to update the profile name in the database
			OleDbCommand cmd = new OleDbCommand("UPDATE tblModelingFile SET Name = '" + Name + "' WHERE DBCode = " + ModelingFileID, con);

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

		private void lvFiles_Resize(object sender, EventArgs e)
		{
			//
			// Update the width of the column
			colName.Width = lvFiles.Width - 20;
		}

		//
		// Load the Training associated with the current selection
		GPModelingData m_gpData;
		private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
		{

			if (m_DeletingItem) return;

			if ((lvFiles.SelectedIndices.Count <= 0) ||
				(m_bLoadFile == false))
				return;

			//
			// Grab the DBCode
			int ModelingFileID = Convert.ToInt32(lvFiles.Items[lvFiles.SelectedIndices[0]].Tag.ToString());

			//
			// Ge the Training loaded from the database
			m_gpData = new GPModelingData();
			tsLabel.Text = "Loading From Database";
			Cursor PrevCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			Application.DoEvents();

			m_gpData.LoadFromDB(
				ModelingFileID,
				new GPModelingData.DELInitProgress(InitProgress),
				new GPModelingData.DELIncrementProgress(IncrementProgress));

			this.Cursor = PrevCursor;
			tsLabel.Text = "";

			//
			// Setup the X-Axis selection combo box
			cbXAxis.Items.Clear();
			//
			// If we have time series data, handle it separately
			for (int Column = 0; Column < m_gpData.Columns; Column++)
			{
				cbXAxis.Items.Add(m_gpData.Header[Column]);
			}
			cbXAxis.SelectedItem = cbXAxis.Items[0];

			//
			// If this is a time series, autocheck the count box
			chkCount.Checked = m_gpData.TimeSeries;
			chkCount.Enabled = !m_gpData.TimeSeries;

			//
			// Place this Training into the grid
			DisplayDataGrid(m_gpData);
			NameColumns();

			//
			// Place this Training into the graph
			DisplayDataGraph(m_gpData);

			//
			// If this file is in use, disable the Delete Button
			btnDelete.Enabled = !GPModelingData.FileInUse(ModelingFileID);

			tsProgress.Value = 0;
		}

		private void menuCSV_Click(object sender, EventArgs e)
		{
			if (dlgFileOpen.ShowDialog() == DialogResult.OK)
			{
				//
				// Set a busy cursor
				tsLabel.Text = "Importing To Database...";
				Cursor PrevCursor = this.Cursor;
				this.Cursor = Cursors.WaitCursor;

				GPModelingData gpData = new GPModelingData();
				bool ValidFile = false;
				if (sender == menuCSV)
				{
					ValidFile = gpData.LoadCSV(
						dlgFileOpen.FileName,
						new GPModelingData.DELInitProgress(InitProgress),
						new GPModelingData.DELIncrementProgress(IncrementProgress));
				}
				else if (sender == menuSSV)
				{
					ValidFile = gpData.LoadSSV(
						dlgFileOpen.FileName,
						new GPModelingData.DELInitProgress(InitProgress),
						new GPModelingData.DELIncrementProgress(IncrementProgress));
				}

				//
				// Replace the busy cursor
				tsLabel.Text = "Done";
				this.Cursor = PrevCursor;
				tsProgress.Value = 0;

				if (ValidFile)
				{
					//
					// Add the item to the list and select it.
					ListViewItem lviNew = lvFiles.Items.Add(gpData.Name);
					lviNew.Tag = gpData.ModelingFileID;
					lviNew.Group = lvFiles.Groups[0];
					lviNew.ImageIndex = 0;
					lviNew.ToolTipText = gpData.Description;

					lviNew.Selected = true;
				}
				else
				{
					MessageBox.Show("Invalid File Format", "GP Studio Data Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}
	}
}
