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
namespace GPStudio.Client
{
	partial class fmBatchProcessing
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmBatchProcessing));
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsMainCancel = new System.Windows.Forms.ToolStripButton();
			this.tsMainMoveUp = new System.Windows.Forms.ToolStripButton();
			this.tsMainMoveDown = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainCancelAll = new System.Windows.Forms.ToolStripButton();
			this.lvProcesses = new System.Windows.Forms.ListView();
			this.colProject = new System.Windows.Forms.ColumnHeader();
			this.colProfile = new System.Windows.Forms.ColumnHeader();
			this.colAdded = new System.Windows.Forms.ColumnHeader();
			this.colStarted = new System.Windows.Forms.ColumnHeader();
			this.colFinished = new System.Windows.Forms.ColumnHeader();
			this.colFitnessError = new System.Windows.Forms.ColumnHeader();
			this.colFitnessHits = new System.Windows.Forms.ColumnHeader();
			this.colComplexity = new System.Windows.Forms.ColumnHeader();
			this.sbMain = new System.Windows.Forms.StatusStrip();
			this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.pgModel = new System.Windows.Forms.ToolStripProgressBar();
			this.tsModel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsMain.SuspendLayout();
			this.sbMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainCancel,
            this.tsMainMoveUp,
            this.tsMainMoveDown,
            this.toolStripSeparator1,
            this.tsMainCancelAll});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(737, 36);
			this.tsMain.TabIndex = 3;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsMainCancel
			// 
			this.tsMainCancel.Enabled = false;
			this.tsMainCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsMainCancel.Image")));
			this.tsMainCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainCancel.Name = "tsMainCancel";
			this.tsMainCancel.Size = new System.Drawing.Size(43, 33);
			this.tsMainCancel.Text = "Cancel";
			this.tsMainCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainCancel.Click += new System.EventHandler(this.tsMainCancel_Click);
			// 
			// tsMainMoveUp
			// 
			this.tsMainMoveUp.Enabled = false;
			this.tsMainMoveUp.Image = global::GPStudio.Client.Properties.Resources.nav_up;
			this.tsMainMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainMoveUp.Name = "tsMainMoveUp";
			this.tsMainMoveUp.Size = new System.Drawing.Size(53, 33);
			this.tsMainMoveUp.Text = "Move Up";
			this.tsMainMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainMoveUp.Click += new System.EventHandler(this.tsMainMoveUp_Click);
			// 
			// tsMainMoveDown
			// 
			this.tsMainMoveDown.Enabled = false;
			this.tsMainMoveDown.Image = global::GPStudio.Client.Properties.Resources.nav_down;
			this.tsMainMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainMoveDown.Name = "tsMainMoveDown";
			this.tsMainMoveDown.Size = new System.Drawing.Size(67, 33);
			this.tsMainMoveDown.Text = "Move Down";
			this.tsMainMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainMoveDown.Click += new System.EventHandler(this.tsMainMoveDown_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 36);
			// 
			// tsMainCancelAll
			// 
			this.tsMainCancelAll.Image = ((System.Drawing.Image)(resources.GetObject("tsMainCancelAll.Image")));
			this.tsMainCancelAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainCancelAll.Name = "tsMainCancelAll";
			this.tsMainCancelAll.Size = new System.Drawing.Size(57, 33);
			this.tsMainCancelAll.Text = "Cancel All";
			this.tsMainCancelAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainCancelAll.Click += new System.EventHandler(this.tsMainCancelAll_Click);
			// 
			// lvProcesses
			// 
			this.lvProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colProject,
            this.colProfile,
            this.colAdded,
            this.colStarted,
            this.colFinished,
            this.colFitnessError,
            this.colFitnessHits,
            this.colComplexity});
			this.lvProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvProcesses.FullRowSelect = true;
			this.lvProcesses.GridLines = true;
			this.lvProcesses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvProcesses.Location = new System.Drawing.Point(0, 36);
			this.lvProcesses.MultiSelect = false;
			this.lvProcesses.Name = "lvProcesses";
			this.lvProcesses.Size = new System.Drawing.Size(737, 381);
			this.lvProcesses.TabIndex = 4;
			this.lvProcesses.UseCompatibleStateImageBehavior = false;
			this.lvProcesses.View = System.Windows.Forms.View.Details;
			this.lvProcesses.SelectedIndexChanged += new System.EventHandler(this.lvProcesses_SelectedIndexChanged);
			// 
			// colProject
			// 
			this.colProject.Text = "Project";
			this.colProject.Width = 113;
			// 
			// colProfile
			// 
			this.colProfile.Text = "Profile";
			this.colProfile.Width = 111;
			// 
			// colAdded
			// 
			this.colAdded.Text = "Added";
			this.colAdded.Width = 80;
			// 
			// colStarted
			// 
			this.colStarted.Text = "Started";
			this.colStarted.Width = 80;
			// 
			// colFinished
			// 
			this.colFinished.Text = "Finished";
			this.colFinished.Width = 80;
			// 
			// colFitnessError
			// 
			this.colFitnessError.Text = "Fitness Error";
			this.colFitnessError.Width = 76;
			// 
			// colFitnessHits
			// 
			this.colFitnessHits.Text = "Hits";
			this.colFitnessHits.Width = 51;
			// 
			// colComplexity
			// 
			this.colComplexity.Text = "Complexity";
			this.colComplexity.Width = 73;
			// 
			// sbMain
			// 
			this.sbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsModel,
            this.pgModel});
			this.sbMain.Location = new System.Drawing.Point(0, 395);
			this.sbMain.Name = "sbMain";
			this.sbMain.Size = new System.Drawing.Size(737, 22);
			this.sbMain.TabIndex = 5;
			this.sbMain.Text = "statusStrip1";
			// 
			// tsStatus
			// 
			this.tsStatus.Name = "tsStatus";
			this.tsStatus.Size = new System.Drawing.Size(83, 17);
			this.tsStatus.Text = "Modeling Status";
			// 
			// pgModel
			// 
			this.pgModel.Name = "pgModel";
			this.pgModel.Size = new System.Drawing.Size(200, 16);
			// 
			// tsModel
			// 
			this.tsModel.Name = "tsModel";
			this.tsModel.Size = new System.Drawing.Size(75, 17);
			this.tsModel.Text = "Project/Profile";
			// 
			// fmBatchProcessing
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(737, 417);
			this.Controls.Add(this.sbMain);
			this.Controls.Add(this.lvProcesses);
			this.Controls.Add(this.tsMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmBatchProcessing";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Batch Processing Status";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmBatchProcessing_FormClosed);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.sbMain.ResumeLayout(false);
			this.sbMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ListView lvProcesses;
		private System.Windows.Forms.StatusStrip sbMain;
		private System.Windows.Forms.ColumnHeader colProject;
		private System.Windows.Forms.ColumnHeader colProfile;
		private System.Windows.Forms.ColumnHeader colStarted;
		private System.Windows.Forms.ColumnHeader colFinished;
		private System.Windows.Forms.ColumnHeader colFitnessError;
		private System.Windows.Forms.ColumnHeader colFitnessHits;
		private System.Windows.Forms.ColumnHeader colComplexity;
		private System.Windows.Forms.ColumnHeader colAdded;
		private System.Windows.Forms.ToolStripStatusLabel tsStatus;
		private System.Windows.Forms.ToolStripButton tsMainCancel;
		private System.Windows.Forms.ToolStripButton tsMainCancelAll;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsMainMoveUp;
		private System.Windows.Forms.ToolStripButton tsMainMoveDown;
		private System.Windows.Forms.ToolStripProgressBar pgModel;
		private System.Windows.Forms.ToolStripStatusLabel tsModel;

	}
}