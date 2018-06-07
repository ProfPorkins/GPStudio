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
	partial class fmDataManager
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Files", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmDataManager));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lvFiles = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.ilDataManager = new System.Windows.Forms.ImageList(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.tsModeling = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.menuCSV = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSSV = new System.Windows.Forms.ToolStripMenuItem();
			this.btnDelete = new System.Windows.Forms.ToolStripButton();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dgView = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.graphData = new ZedGraph.ZedGraphControl();
			this.panel4 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkCount = new System.Windows.Forms.CheckBox();
			this.cbXAxis = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rdChartTypeScatter = new System.Windows.Forms.RadioButton();
			this.rdChartTypeBar = new System.Windows.Forms.RadioButton();
			this.rdChartTypeXY = new System.Windows.Forms.RadioButton();
			this.statusData = new System.Windows.Forms.StatusStrip();
			this.tsLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tsModeling.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.statusData.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.panel3);
			this.splitContainer1.Panel1.Controls.Add(this.panel2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Panel2.Controls.Add(this.statusData);
			this.splitContainer1.Size = new System.Drawing.Size(660, 466);
			this.splitContainer1.SplitterDistance = 214;
			this.splitContainer1.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.lvFiles);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(214, 426);
			this.panel3.TabIndex = 3;
			// 
			// lvFiles
			// 
			this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
			this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvFiles.FullRowSelect = true;
			this.lvFiles.GridLines = true;
			listViewGroup1.Header = "Files";
			listViewGroup1.Name = "listViewGroup1";
			this.lvFiles.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			this.lvFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFiles.HideSelection = false;
			this.lvFiles.LabelEdit = true;
			this.lvFiles.Location = new System.Drawing.Point(0, 0);
			this.lvFiles.MultiSelect = false;
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.ShowItemToolTips = true;
			this.lvFiles.Size = new System.Drawing.Size(214, 426);
			this.lvFiles.SmallImageList = this.ilDataManager;
			this.lvFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvFiles.TabIndex = 0;
			this.lvFiles.UseCompatibleStateImageBehavior = false;
			this.lvFiles.View = System.Windows.Forms.View.Details;
			this.lvFiles.Resize += new System.EventHandler(this.lvFiles_Resize);
			this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
			this.lvFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvFiles_AfterLabelEdit);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 200;
			// 
			// ilDataManager
			// 
			this.ilDataManager.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilDataManager.ImageStream")));
			this.ilDataManager.TransparentColor = System.Drawing.Color.Transparent;
			this.ilDataManager.Images.SetKeyName(0, "db.ico");
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tsModeling);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 426);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(214, 40);
			this.panel2.TabIndex = 2;
			// 
			// tsModeling
			// 
			this.tsModeling.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsModeling.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.btnDelete});
			this.tsModeling.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.tsModeling.Location = new System.Drawing.Point(0, 0);
			this.tsModeling.Name = "tsModeling";
			this.tsModeling.Size = new System.Drawing.Size(214, 36);
			this.tsModeling.TabIndex = 7;
			this.tsModeling.Text = "toolStrip2";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCSV,
            this.menuSSV});
			this.toolStripDropDownButton1.Image = global::GPStudio.Client.Properties.Resources.dbs;
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(52, 33);
			this.toolStripDropDownButton1.Text = "Import";
			this.toolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// menuCSV
			// 
			this.menuCSV.Name = "menuCSV";
			this.menuCSV.Size = new System.Drawing.Size(231, 22);
			this.menuCSV.Text = "Comma Separated Values (US)";
			this.menuCSV.Click += new System.EventHandler(this.menuCSV_Click);
			// 
			// menuSSV
			// 
			this.menuSSV.Name = "menuSSV";
			this.menuSSV.Size = new System.Drawing.Size(231, 22);
			this.menuSSV.Text = "Semi-Colon Separated Values";
			this.menuSSV.Click += new System.EventHandler(this.menuCSV_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Image = global::GPStudio.Client.Properties.Resources.error;
			this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(42, 33);
			this.btnDelete.Text = "Delete";
			this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(442, 444);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dgView);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(434, 418);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Tabular";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// dgView
			// 
			this.dgView.AllowUserToAddRows = false;
			this.dgView.AllowUserToDeleteRows = false;
			this.dgView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgView.Location = new System.Drawing.Point(3, 3);
			this.dgView.Name = "dgView";
			this.dgView.ReadOnly = true;
			this.dgView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dgView.RowHeadersWidth = 75;
			this.dgView.RowTemplate.ReadOnly = true;
			this.dgView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgView.Size = new System.Drawing.Size(428, 412);
			this.dgView.TabIndex = 4;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.graphData);
			this.tabPage2.Controls.Add(this.panel4);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(434, 418);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Graphical";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// graphData
			// 
			this.graphData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphData.EditButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphData.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphData.IsAutoScrollRange = false;
			this.graphData.IsEnableHEdit = false;
			this.graphData.IsEnableHPan = true;
			this.graphData.IsEnableHZoom = true;
			this.graphData.IsEnableVEdit = false;
			this.graphData.IsEnableVPan = true;
			this.graphData.IsEnableVZoom = true;
			this.graphData.IsPrintFillPage = true;
			this.graphData.IsPrintKeepAspectRatio = true;
			this.graphData.IsScrollY2 = false;
			this.graphData.IsShowContextMenu = true;
			this.graphData.IsShowCopyMessage = true;
			this.graphData.IsShowCursorValues = false;
			this.graphData.IsShowHScrollBar = false;
			this.graphData.IsShowPointValues = true;
			this.graphData.IsShowVScrollBar = false;
			this.graphData.IsSynchronizeXAxes = false;
			this.graphData.IsSynchronizeYAxes = false;
			this.graphData.IsZoomOnMouseCenter = false;
			this.graphData.LinkButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphData.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphData.Location = new System.Drawing.Point(3, 3);
			this.graphData.Name = "graphData";
			this.graphData.PanButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphData.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
			this.graphData.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
			this.graphData.PanModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphData.PointDateFormat = "g";
			this.graphData.PointValueFormat = "G";
			this.graphData.ScrollMaxX = 0;
			this.graphData.ScrollMaxY = 0;
			this.graphData.ScrollMaxY2 = 0;
			this.graphData.ScrollMinX = 0;
			this.graphData.ScrollMinY = 0;
			this.graphData.ScrollMinY2 = 0;
			this.graphData.Size = new System.Drawing.Size(428, 353);
			this.graphData.TabIndex = 3;
			this.graphData.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphData.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
			this.graphData.ZoomModifierKeys = System.Windows.Forms.Keys.None;
			this.graphData.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphData.ZoomStepFraction = 0.1;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.groupBox1);
			this.panel4.Controls.Add(this.groupBox3);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(3, 356);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(428, 59);
			this.panel4.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkCount);
			this.groupBox1.Controls.Add(this.cbXAxis);
			this.groupBox1.Location = new System.Drawing.Point(178, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(245, 44);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "X-Axis";
			// 
			// chkCount
			// 
			this.chkCount.AutoSize = true;
			this.chkCount.Location = new System.Drawing.Point(6, 19);
			this.chkCount.Name = "chkCount";
			this.chkCount.Size = new System.Drawing.Size(54, 17);
			this.chkCount.TabIndex = 2;
			this.chkCount.Text = "Count";
			this.chkCount.UseVisualStyleBackColor = true;
			this.chkCount.CheckedChanged += new System.EventHandler(this.rdChartType_OptionChanged);
			// 
			// cbXAxis
			// 
			this.cbXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbXAxis.FormattingEnabled = true;
			this.cbXAxis.Location = new System.Drawing.Point(66, 17);
			this.cbXAxis.Name = "cbXAxis";
			this.cbXAxis.Size = new System.Drawing.Size(173, 21);
			this.cbXAxis.TabIndex = 0;
			this.cbXAxis.SelectedIndexChanged += new System.EventHandler(this.rdChartType_OptionChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rdChartTypeScatter);
			this.groupBox3.Controls.Add(this.rdChartTypeBar);
			this.groupBox3.Controls.Add(this.rdChartTypeXY);
			this.groupBox3.Location = new System.Drawing.Point(3, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(169, 44);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Chart Type";
			// 
			// rdChartTypeScatter
			// 
			this.rdChartTypeScatter.AutoSize = true;
			this.rdChartTypeScatter.Location = new System.Drawing.Point(53, 19);
			this.rdChartTypeScatter.Name = "rdChartTypeScatter";
			this.rdChartTypeScatter.Size = new System.Drawing.Size(59, 17);
			this.rdChartTypeScatter.TabIndex = 2;
			this.rdChartTypeScatter.TabStop = true;
			this.rdChartTypeScatter.Text = "Scatter";
			this.rdChartTypeScatter.UseVisualStyleBackColor = true;
			this.rdChartTypeScatter.CheckedChanged += new System.EventHandler(this.rdChartType_OptionChanged);
			// 
			// rdChartTypeBar
			// 
			this.rdChartTypeBar.AutoSize = true;
			this.rdChartTypeBar.Location = new System.Drawing.Point(118, 19);
			this.rdChartTypeBar.Name = "rdChartTypeBar";
			this.rdChartTypeBar.Size = new System.Drawing.Size(41, 17);
			this.rdChartTypeBar.TabIndex = 1;
			this.rdChartTypeBar.Text = "Bar";
			this.rdChartTypeBar.UseVisualStyleBackColor = true;
			this.rdChartTypeBar.CheckedChanged += new System.EventHandler(this.rdChartType_OptionChanged);
			// 
			// rdChartTypeXY
			// 
			this.rdChartTypeXY.AutoSize = true;
			this.rdChartTypeXY.Checked = true;
			this.rdChartTypeXY.Location = new System.Drawing.Point(6, 19);
			this.rdChartTypeXY.Name = "rdChartTypeXY";
			this.rdChartTypeXY.Size = new System.Drawing.Size(45, 17);
			this.rdChartTypeXY.TabIndex = 0;
			this.rdChartTypeXY.TabStop = true;
			this.rdChartTypeXY.Text = "Line";
			this.rdChartTypeXY.UseVisualStyleBackColor = true;
			this.rdChartTypeXY.CheckedChanged += new System.EventHandler(this.rdChartType_OptionChanged);
			// 
			// statusData
			// 
			this.statusData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabel,
            this.tsProgress});
			this.statusData.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.statusData.Location = new System.Drawing.Point(0, 444);
			this.statusData.Name = "statusData";
			this.statusData.Size = new System.Drawing.Size(442, 22);
			this.statusData.SizingGrip = false;
			this.statusData.TabIndex = 0;
			this.statusData.Text = "statusStrip1";
			// 
			// tsLabel
			// 
			this.tsLabel.AutoSize = false;
			this.tsLabel.Name = "tsLabel";
			this.tsLabel.Size = new System.Drawing.Size(125, 17);
			this.tsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsProgress
			// 
			this.tsProgress.Name = "tsProgress";
			this.tsProgress.Size = new System.Drawing.Size(175, 16);
			// 
			// dlgFileOpen
			// 
			this.dlgFileOpen.DefaultExt = "*.csv";
			this.dlgFileOpen.Filter = "CSV Files (*.csv)|*.csv";
			this.dlgFileOpen.Title = "Open File";
			// 
			// fmDataManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(660, 466);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmDataManager";
			this.Text = "Data Manager";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmDataImport_FormClosed);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tsModeling.ResumeLayout(false);
			this.tsModeling.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.statusData.ResumeLayout(false);
			this.statusData.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ListView lvFiles;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.OpenFileDialog dlgFileOpen;
		private System.Windows.Forms.StatusStrip statusData;
		private System.Windows.Forms.ToolStripStatusLabel tsLabel;
		private System.Windows.Forms.ToolStripProgressBar tsProgress;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.DataGridView dgView;
		private System.Windows.Forms.TabPage tabPage2;
		private ZedGraph.ZedGraphControl graphData;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rdChartTypeBar;
		private System.Windows.Forms.RadioButton rdChartTypeXY;
		private System.Windows.Forms.RadioButton rdChartTypeScatter;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cbXAxis;
		private System.Windows.Forms.ImageList ilDataManager;
		private System.Windows.Forms.CheckBox chkCount;
		private System.Windows.Forms.ToolStrip tsModeling;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripButton btnDelete;
		private System.Windows.Forms.ToolStripMenuItem menuCSV;
		private System.Windows.Forms.ToolStripMenuItem menuSSV;
	}
}