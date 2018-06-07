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

namespace GPStudio.Client
{
	public partial class fmBatchProcessing : Form, IBatchClient
	{
		private static bool m_bActive = false;
		private const int SUBITEM_PROJECT = 0;
		private const int SUBITEM_PROFILE = 1;
		private const int SUBITEM_ADDED = 2;
		private const int SUBITEM_STARTED = 3;
		private const int SUBITEM_FINISHED = 4;
		private const int SUBITEM_FITNESS = 5;
		private const int SUBITEM_HITS = 6;
		private const int SUBITEM_COMPLEXITY = 7;
		private const string CANCEL_STRING = "Cancelled!";

		/// <summary>
		/// Property that indicates whether or not this form is already up
		/// and running
		/// </summary>
		public static bool Active
		{
			get { return fmBatchProcessing.m_bActive; }
			set { fmBatchProcessing.m_bActive = value; }
		}

		/// <summary>
		/// Default constructor, get the components created and the active flag
		/// set to true.
		/// </summary>
		public fmBatchProcessing()
		{
			InitializeComponent();

			Active = true;

			//
			// Register with the batch processing manager
			fmMain.BatchManager.RegisterClient(this);
		}

		private void fmBatchProcessing_FormClosed(object sender, FormClosedEventArgs e)
		{
			//
			// Unregister from the batch processing manager events
			fmMain.BatchManager.UnregisterClient(this);

			//
			// Set the Active flag to false so a new copy of this window 
			// can be created.
			Active = false;
		}

		#region IBatchClient Implementation

		//
		// List of delegates used for ensuring the UI gets updated by the creating thread
		private delegate void DEL_AddProcess(object ProcessID, String ProjectName, String ProfileName, DateTime TimeAdded, DateTime TimeStarted,bool IsStarted);
		private delegate void DEL_ProcessStarted(object ProcessID, DateTime TimeStarted,int MaxGenerations);
		private delegate void DEL_ProcessUpdate(int Generation);
		private delegate void DEL_ProcessStatus(string Message);
		private delegate void DEL_ProcessComplete(object ProcessID, DateTime TimeComplete, bool Canceled, double Fitness, int Hits, int Complexity);

		public void AddProcess(object ProcessID, String ProjectName, String ProfileName,DateTime TimeAdded, DateTime TimeStarted, bool IsStarted)
		{
			if (lvProcesses.InvokeRequired)
			{
				DEL_AddProcess d = new DEL_AddProcess(AddProcess);
				this.Invoke(d, new object[] { ProcessID, ProjectName, ProfileName, TimeAdded, TimeStarted, IsStarted});
			}
			else
			{
				//
				// Add this process to the list view
				ListViewItem lviNew = lvProcesses.Items.Add(ProjectName);
				lviNew.Tag = ProcessID;
				lviNew.Name = ProcessID.GetHashCode().ToString();
				lviNew.SubItems.Add(ProfileName);
				lviNew.SubItems.Add(TimeAdded.ToLongTimeString());
				if (IsStarted)
				{
					//
					// This means it is already being modeled, so put the starting
					// time on the display.  Also, show the project/profile name in
					// the status bar
					lviNew.SubItems.Add(TimeStarted.ToLongTimeString());
					tsModel.Text = ProjectName + " : " + ProfileName;
				}
				else
				{
					lviNew.SubItems.Add("");	// Empty started string
				}
				lviNew.SubItems.Add("");	// Empty completed string
				lviNew.SubItems.Add("");	// Empty Fitness Error
				lviNew.SubItems.Add("");	// Empty Hits
				lviNew.SubItems.Add("");	// Empty Complexity
			}
		}

		/// <summary>
		/// Indicates a new batch modeling process has started
		/// </summary>
		/// <param name="ProcessID"></param>
		/// <param name="TimeStarted"></param>
		/// <param name="MaxGenerations"></param>
		public void ProcessStarted(object ProcessID, DateTime TimeStarted,int MaxGenerations)
		{
			if (this.lvProcesses.InvokeRequired)
			{
				DEL_ProcessStarted d = new DEL_ProcessStarted(ProcessStarted);
				this.Invoke(d, new object[] { ProcessID, TimeStarted, MaxGenerations });
			}
			else
			{
				//
				// Find this process in the list view
				ListViewItem[] lviProcess = lvProcesses.Items.Find(ProcessID.GetHashCode().ToString(), false);
				if (lviProcess != null)
				{
					//
					// "Ass"uming the program is coded correctly, should have a single entry in the array
					lviProcess[0].SubItems[SUBITEM_STARTED].Text = TimeStarted.ToLongTimeString();
					//
					// Update the status bar with the project/profile name
					tsModel.Text = lviProcess[0].SubItems[SUBITEM_PROJECT].Text + " : " + lviProcess[0].SubItems[SUBITEM_PROFILE].Text;
					//
					// Prepare the progress bar
					pgModel.Maximum = MaxGenerations;
					pgModel.Minimum = 1;
					pgModel.Value = 1;
				}
			}
		}

		/// <summary>
		/// A new generation was completed, update the progress bar to indicate the progress.
		/// </summary>
		/// <param name="Generation"></param>
		public void ProcessUpdate(int Generation)
		{
			if (this.lvProcesses.InvokeRequired)
			{
				DEL_ProcessUpdate d = new DEL_ProcessUpdate(ProcessUpdate);
				this.Invoke(d, new object[] { Generation } );
			}
			else
			{
				//
				// Increment the progress bar
				pgModel.Increment(1);
			}
		}

		/// <summary>
		/// Received a modeling message, show it in the status bar
		/// </summary>
		/// <param name="Message"></param>
		public void ProcessStatus(string Message)
		{
			if (this.lvProcesses.InvokeRequired)
			{
				DEL_ProcessStatus d = new DEL_ProcessStatus(ProcessStatus);
				this.Invoke(d, new object[] { Message });
			}
			else
			{
				tsStatus.Text = "Status: "+Message;
			}
		}

		/// <summary>
		/// A batch modeling process completed, update the UI with the results.
		/// </summary>
		/// <param name="ProcessID"></param>
		/// <param name="TimeComplete"></param>
		/// <param name="Canceled"></param>
		public void ProcessComplete(object ProcessID, DateTime TimeComplete, bool Canceled, double Fitness, int Hits, int Complexity)
		{
			if (this.lvProcesses.InvokeRequired)
			{
				DEL_ProcessComplete d = new DEL_ProcessComplete(ProcessComplete);
				this.Invoke(d, new object[] { ProcessID, TimeComplete, Canceled, Fitness, Hits, Complexity });
			}
			else
			{
				//
				// Find this process in the list view
				ListViewItem[] lviProcess = lvProcesses.Items.Find(ProcessID.GetHashCode().ToString(), false);
				if (lviProcess != null)
				{
					if (!Canceled)
					{
						//
						// "Ass"uming the program is coded correctly, should have a single entry in the array
						lviProcess[0].SubItems[SUBITEM_FINISHED].Text = TimeComplete.ToLongTimeString();
						lviProcess[0].SubItems[SUBITEM_FITNESS].Text = Fitness.ToString();
						lviProcess[0].SubItems[SUBITEM_HITS].Text = Hits.ToString();
						lviProcess[0].SubItems[SUBITEM_COMPLEXITY].Text = Complexity.ToString();
					}
				}

				tsModel.Text = "Projct/Profile";
				tsStatus.Text = "Status: ";
				pgModel.Value = 1;
			}
		}

		public int CompareTo(IBatchClient other)
		{
			if (other == this)
			{
				return 0;
			}

			return 1;
		}

		#endregion

		private void lvProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			// If nothing selected, disable the cancel & move buttons
			if (lvProcesses.SelectedItems.Count == 0)
			{
				tsMainCancel.Enabled = false;
				tsMainMoveUp.Enabled = false;
				tsMainMoveDown.Enabled = false;
			} // If selected, but finished or canceled, disable the cancel & move buttons
			else if (lvProcesses.SelectedItems[0].SubItems[SUBITEM_FINISHED].Text.Length > 0)
			{
				tsMainCancel.Enabled = false;
				tsMainMoveUp.Enabled = false;
				tsMainMoveDown.Enabled = false;
			}
			else // Otherwise, enable the cancel & move buttons
			{
				tsMainCancel.Enabled = true;
				//
				// If this one is currently being modeled, then no move buttons
				if (lvProcesses.SelectedItems[0].SubItems[SUBITEM_STARTED].Text.Length == 0)
				{
					tsMainMoveUp.Enabled = true;
					//
					// As long as there is another item in the list view
					if ((lvProcesses.Items.Count-1) > lvProcesses.SelectedItems[0].Index)
					{
						tsMainMoveDown.Enabled = true;
					}
				}
			}
		}

		private void tsMainCancel_Click(object sender, EventArgs e)
		{
			fmMain.BatchManager.CancelSimulation(lvProcesses.SelectedItems[0].Tag);
			lvProcesses.SelectedItems[0].SubItems[SUBITEM_STARTED].Text = CANCEL_STRING;
			lvProcesses.SelectedItems[0].SubItems[SUBITEM_FINISHED].Text = CANCEL_STRING;
			lvProcesses.SelectedItems[0].SubItems[SUBITEM_FITNESS].Text = CANCEL_STRING;
			lvProcesses.SelectedItems[0].SubItems[SUBITEM_HITS].Text = CANCEL_STRING;
			lvProcesses.SelectedItems[0].SubItems[SUBITEM_COMPLEXITY].Text = CANCEL_STRING;
			//
			// Go ahead and disable the cancel button, since we just cancelled something
			tsMainCancel.Enabled = false;
		}

		private void tsMainCancelAll_Click(object sender, EventArgs e)
		{
			//
			// Instruct the batch processing manager to cancel all pending
			// processes.
			fmMain.BatchManager.CancelAllSimulations();
			//
			// Mark them as cancelled.
			foreach (ListViewItem lvi in lvProcesses.Items)
			{
				if (lvi.SubItems[SUBITEM_FINISHED].Text.Length == 0)
				{
					fmMain.BatchManager.CancelSimulation(lvi.Tag);
					lvi.SubItems[SUBITEM_STARTED].Text = CANCEL_STRING;
					lvi.SubItems[SUBITEM_FINISHED].Text = CANCEL_STRING;
					lvi.SubItems[SUBITEM_FITNESS].Text = CANCEL_STRING;
					lvi.SubItems[SUBITEM_HITS].Text = CANCEL_STRING;
					lvi.SubItems[SUBITEM_COMPLEXITY].Text = CANCEL_STRING;
				}
			}

			//
			// Disable the regular cancel button
			tsMainCancel.Enabled = false;
			pgModel.Value = 1;

			tsModel.Text = "";
			tsStatus.Text = "Status: ";
		}

		private void tsMainMoveUp_Click(object sender, EventArgs e)
		{
			//
			// Ensure no new models are started while this function runs
			fmMain.BatchManager.SuspendProcessing();
			//
			// Only allow a move up, if the previous one in the list
			// is currently not being modeled
			ListViewItem lviPrevious=lvProcesses.Items[lvProcesses.SelectedItems[0].Index-1];
			if (lviPrevious.SubItems[SUBITEM_STARTED].Text.Length == 0)
			{
				//
				// Swap these two processes in the batch processing manager
				fmMain.BatchManager.SwapProcesses(lviPrevious.Tag,lvProcesses.SelectedItems[0].Tag);
				//
				// Swap them in the list view control
				ListViewItem lviSelected = lvProcesses.SelectedItems[0];
				lvProcesses.Items.Remove(lviSelected);
				lvProcesses.Items.Insert(lviPrevious.Index, lviSelected);
			}

			//
			// Resume batch model processing
			fmMain.BatchManager.ResumeProcessing();
		}

		private void tsMainMoveDown_Click(object sender, EventArgs e)
		{
			//
			// Ensure no new models are started while this function runs
			fmMain.BatchManager.SuspendProcessing();

			//
			// Ensure there is a next item before trying to move down
			if ((lvProcesses.Items.Count - 1) > lvProcesses.SelectedItems[0].Index)
			{
				ListViewItem lviNext = lvProcesses.Items[lvProcesses.SelectedItems[0].Index + 1];
				//
				// Swap these two processes in the batch processing manager
				fmMain.BatchManager.SwapProcesses(lvProcesses.SelectedItems[0].Tag, lviNext.Tag);
				//
				// Swap them in the list view control
				ListViewItem lviSelected = lvProcesses.SelectedItems[0];
				lvProcesses.Items.Remove(lviNext);
				lvProcesses.Items.Insert(lviSelected.Index, lviNext);
				//
				// If the selected one is now the last one, disable the move down button
				if (lvProcesses.SelectedIndices[0] == lvProcesses.Items.Count - 1)
				{
					tsMainMoveDown.Enabled = false;
				}
			}

			//
			// Resume batch model processing
			fmMain.BatchManager.ResumeProcessing();
		}
	}
}