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
	partial class fmFitnessEditor
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Functions", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmFitnessEditor));
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvFunctions = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.txtSource = new System.Windows.Forms.RichTextBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.tabFooter = new System.Windows.Forms.TabControl();
			this.pageErrors = new System.Windows.Forms.TabPage();
			this.lvErrors = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.chkInUse = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.chkValidated = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbPrototype = new System.Windows.Forms.Label();
			this.txtFunctionName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsMainAdd = new System.Windows.Forms.ToolStripButton();
			this.tsMainDelete = new System.Windows.Forms.ToolStripButton();
			this.tsMainSave = new System.Windows.Forms.ToolStripButton();
			this.tsMainSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainValidate = new System.Windows.Forms.ToolStripButton();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.panel4.SuspendLayout();
			this.tabFooter.SuspendLayout();
			this.pageErrors.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.Location = new System.Drawing.Point(0, 36);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvFunctions);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.txtSource);
			this.splitMain.Panel2.Controls.Add(this.panel4);
			this.splitMain.Panel2.Controls.Add(this.panel3);
			this.splitMain.Size = new System.Drawing.Size(705, 420);
			this.splitMain.SplitterDistance = 196;
			this.splitMain.TabIndex = 4;
			// 
			// lvFunctions
			// 
			this.lvFunctions.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvFunctions.FullRowSelect = true;
			this.lvFunctions.GridLines = true;
			listViewGroup1.Header = "Functions";
			listViewGroup1.Name = "lvFunctions";
			this.lvFunctions.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			this.lvFunctions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFunctions.HideSelection = false;
			this.lvFunctions.Location = new System.Drawing.Point(0, 0);
			this.lvFunctions.MultiSelect = false;
			this.lvFunctions.Name = "lvFunctions";
			this.lvFunctions.ShowItemToolTips = true;
			this.lvFunctions.Size = new System.Drawing.Size(196, 420);
			this.lvFunctions.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvFunctions.TabIndex = 5;
			this.lvFunctions.UseCompatibleStateImageBehavior = false;
			this.lvFunctions.View = System.Windows.Forms.View.Details;
			this.lvFunctions.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvFunctions_ItemSelectionChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 175;
			// 
			// txtSource
			// 
			this.txtSource.AcceptsTab = true;
			this.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSource.DetectUrls = false;
			this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSource.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSource.Location = new System.Drawing.Point(0, 113);
			this.txtSource.Name = "txtSource";
			this.txtSource.Size = new System.Drawing.Size(505, 200);
			this.txtSource.TabIndex = 2;
			this.txtSource.Text = "";
			this.txtSource.WordWrap = false;
			this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.tabFooter);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 313);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(505, 107);
			this.panel4.TabIndex = 1;
			// 
			// tabFooter
			// 
			this.tabFooter.Controls.Add(this.pageErrors);
			this.tabFooter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabFooter.Location = new System.Drawing.Point(0, 0);
			this.tabFooter.Name = "tabFooter";
			this.tabFooter.SelectedIndex = 0;
			this.tabFooter.Size = new System.Drawing.Size(505, 107);
			this.tabFooter.TabIndex = 2;
			// 
			// pageErrors
			// 
			this.pageErrors.Controls.Add(this.lvErrors);
			this.pageErrors.Location = new System.Drawing.Point(4, 22);
			this.pageErrors.Name = "pageErrors";
			this.pageErrors.Padding = new System.Windows.Forms.Padding(3);
			this.pageErrors.Size = new System.Drawing.Size(497, 81);
			this.pageErrors.TabIndex = 1;
			this.pageErrors.Text = "Errors";
			this.pageErrors.UseVisualStyleBackColor = true;
			// 
			// lvErrors
			// 
			this.lvErrors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
			this.lvErrors.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvErrors.FullRowSelect = true;
			this.lvErrors.GridLines = true;
			this.lvErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvErrors.Location = new System.Drawing.Point(3, 3);
			this.lvErrors.MultiSelect = false;
			this.lvErrors.Name = "lvErrors";
			this.lvErrors.ShowItemToolTips = true;
			this.lvErrors.Size = new System.Drawing.Size(491, 75);
			this.lvErrors.TabIndex = 0;
			this.lvErrors.UseCompatibleStateImageBehavior = false;
			this.lvErrors.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Width = 350;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.chkInUse);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.chkValidated);
			this.panel3.Controls.Add(this.groupBox1);
			this.panel3.Controls.Add(this.txtFunctionName);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(505, 113);
			this.panel3.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(109, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Used In Profile";
			// 
			// chkInUse
			// 
			this.chkInUse.AutoSize = true;
			this.chkInUse.Enabled = false;
			this.chkInUse.Location = new System.Drawing.Point(88, 31);
			this.chkInUse.Name = "chkInUse";
			this.chkInUse.Size = new System.Drawing.Size(15, 14);
			this.chkInUse.TabIndex = 9;
			this.chkInUse.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(420, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Validated";
			// 
			// chkValidated
			// 
			this.chkValidated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkValidated.AutoSize = true;
			this.chkValidated.Enabled = false;
			this.chkValidated.Location = new System.Drawing.Point(475, 32);
			this.chkValidated.Name = "chkValidated";
			this.chkValidated.Size = new System.Drawing.Size(15, 14);
			this.chkValidated.TabIndex = 6;
			this.chkValidated.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lbPrototype);
			this.groupBox1.Location = new System.Drawing.Point(7, 52);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(487, 54);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Prototype";
			// 
			// lbPrototype
			// 
			this.lbPrototype.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbPrototype.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbPrototype.Location = new System.Drawing.Point(3, 16);
			this.lbPrototype.Name = "lbPrototype";
			this.lbPrototype.Size = new System.Drawing.Size(481, 35);
			this.lbPrototype.TabIndex = 0;
			this.lbPrototype.Text = "C# Prototype";
			// 
			// txtFunctionName
			// 
			this.txtFunctionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFunctionName.Enabled = false;
			this.txtFunctionName.Location = new System.Drawing.Point(88, 6);
			this.txtFunctionName.Name = "txtFunctionName";
			this.txtFunctionName.Size = new System.Drawing.Size(402, 20);
			this.txtFunctionName.TabIndex = 1;
			this.txtFunctionName.TextChanged += new System.EventHandler(this.txtFunctionName_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Function Title";
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainAdd,
            this.tsMainDelete,
            this.tsMainSave,
            this.tsMainSeparator,
            this.tsMainValidate});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(705, 36);
			this.tsMain.TabIndex = 5;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsMainAdd
			// 
			this.tsMainAdd.Image = global::GPStudio.Client.Properties.Resources.newfolder;
			this.tsMainAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainAdd.Name = "tsMainAdd";
			this.tsMainAdd.Size = new System.Drawing.Size(30, 33);
			this.tsMainAdd.Text = "Add";
			this.tsMainAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainAdd.Click += new System.EventHandler(this.tsMainAdd_Click);
			// 
			// tsMainDelete
			// 
			this.tsMainDelete.Enabled = false;
			this.tsMainDelete.Image = global::GPStudio.Client.Properties.Resources.error;
			this.tsMainDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainDelete.Name = "tsMainDelete";
			this.tsMainDelete.Size = new System.Drawing.Size(42, 33);
			this.tsMainDelete.Text = "Delete";
			this.tsMainDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainDelete.Click += new System.EventHandler(this.tsMainDelete_Click);
			// 
			// tsMainSave
			// 
			this.tsMainSave.Enabled = false;
			this.tsMainSave.Image = global::GPStudio.Client.Properties.Resources.Save;
			this.tsMainSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainSave.Name = "tsMainSave";
			this.tsMainSave.Size = new System.Drawing.Size(35, 33);
			this.tsMainSave.Text = "Save";
			this.tsMainSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainSave.Click += new System.EventHandler(this.tsMainSave_Click);
			// 
			// tsMainSeparator
			// 
			this.tsMainSeparator.Name = "tsMainSeparator";
			this.tsMainSeparator.Size = new System.Drawing.Size(6, 36);
			// 
			// tsMainValidate
			// 
			this.tsMainValidate.Enabled = false;
			this.tsMainValidate.Image = global::GPStudio.Client.Properties.Resources.FormRun;
			this.tsMainValidate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainValidate.Name = "tsMainValidate";
			this.tsMainValidate.Size = new System.Drawing.Size(49, 33);
			this.tsMainValidate.Text = "Validate";
			this.tsMainValidate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsMainValidate.Click += new System.EventHandler(this.tsMainValidate_Click);
			// 
			// fmFitnessEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(705, 456);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.tsMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmFitnessEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Fitness Functions";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmFitnessEditor_FormClosed);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.tabFooter.ResumeLayout(false);
			this.pageErrors.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ListView lvFunctions;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.RichTextBox txtSource;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TabControl tabFooter;
		private System.Windows.Forms.TabPage pageErrors;
		private System.Windows.Forms.ListView lvErrors;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkValidated;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lbPrototype;
		private System.Windows.Forms.TextBox txtFunctionName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkInUse;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton tsMainAdd;
		private System.Windows.Forms.ToolStripButton tsMainDelete;
		private System.Windows.Forms.ToolStripButton tsMainSave;
		private System.Windows.Forms.ToolStripSeparator tsMainSeparator;
		private System.Windows.Forms.ToolStripButton tsMainValidate;
	}
}