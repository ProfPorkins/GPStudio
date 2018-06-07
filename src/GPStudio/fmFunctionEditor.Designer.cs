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
	partial class fmFunctionEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmFunctionEditor));
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvFunctions = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.txtSource = new System.Windows.Forms.RichTextBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.tabFooter = new System.Windows.Forms.TabControl();
			this.pageDescription = new System.Windows.Forms.TabPage();
			this.txtDescription = new System.Windows.Forms.RichTextBox();
			this.pageErrors = new System.Windows.Forms.TabPage();
			this.lvErrors = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lbTerminalsOnly = new System.Windows.Forms.Label();
			this.chkTerminalParameters = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbCategory = new System.Windows.Forms.ComboBox();
			this.chkValidated = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbPrototype = new System.Windows.Forms.Label();
			this.udParamterCount = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFunctionName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsMainAdd = new System.Windows.Forms.ToolStripButton();
			this.tsMainDelete = new System.Windows.Forms.ToolStripButton();
			this.tsMainSave = new System.Windows.Forms.ToolStripButton();
			this.tsMainSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainValidate = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsMainLanguage = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsMainLanguage_C = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguage_CPP = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguageItem_CSharp = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguage_VisualBasic = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguage_VBNET = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguage_Java = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMainLanguage_Fortran = new System.Windows.Forms.ToolStripMenuItem();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.panel4.SuspendLayout();
			this.tabFooter.SuspendLayout();
			this.pageDescription.SuspendLayout();
			this.pageErrors.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udParamterCount)).BeginInit();
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
			this.splitMain.Size = new System.Drawing.Size(655, 529);
			this.splitMain.SplitterDistance = 196;
			this.splitMain.TabIndex = 1;
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
			listViewGroup1.Name = "listViewGroup1";
			this.lvFunctions.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			this.lvFunctions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFunctions.HideSelection = false;
			this.lvFunctions.Location = new System.Drawing.Point(0, 0);
			this.lvFunctions.MultiSelect = false;
			this.lvFunctions.Name = "lvFunctions";
			this.lvFunctions.ShowItemToolTips = true;
			this.lvFunctions.Size = new System.Drawing.Size(196, 529);
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
			this.txtSource.Location = new System.Drawing.Point(0, 145);
			this.txtSource.Name = "txtSource";
			this.txtSource.Size = new System.Drawing.Size(455, 251);
			this.txtSource.TabIndex = 2;
			this.txtSource.Text = "";
			this.txtSource.WordWrap = false;
			this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.tabFooter);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 396);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(455, 133);
			this.panel4.TabIndex = 1;
			// 
			// tabFooter
			// 
			this.tabFooter.Controls.Add(this.pageDescription);
			this.tabFooter.Controls.Add(this.pageErrors);
			this.tabFooter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabFooter.Location = new System.Drawing.Point(0, 0);
			this.tabFooter.Name = "tabFooter";
			this.tabFooter.SelectedIndex = 0;
			this.tabFooter.Size = new System.Drawing.Size(455, 133);
			this.tabFooter.TabIndex = 2;
			// 
			// pageDescription
			// 
			this.pageDescription.Controls.Add(this.txtDescription);
			this.pageDescription.Location = new System.Drawing.Point(4, 22);
			this.pageDescription.Name = "pageDescription";
			this.pageDescription.Padding = new System.Windows.Forms.Padding(3);
			this.pageDescription.Size = new System.Drawing.Size(447, 107);
			this.pageDescription.TabIndex = 0;
			this.pageDescription.Text = "Description";
			this.pageDescription.UseVisualStyleBackColor = true;
			// 
			// txtDescription
			// 
			this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDescription.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDescription.Location = new System.Drawing.Point(3, 3);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(441, 101);
			this.txtDescription.TabIndex = 2;
			this.txtDescription.Text = "";
			this.txtDescription.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
			// 
			// pageErrors
			// 
			this.pageErrors.Controls.Add(this.lvErrors);
			this.pageErrors.Location = new System.Drawing.Point(4, 22);
			this.pageErrors.Name = "pageErrors";
			this.pageErrors.Padding = new System.Windows.Forms.Padding(3);
			this.pageErrors.Size = new System.Drawing.Size(447, 107);
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
			this.lvErrors.Size = new System.Drawing.Size(441, 101);
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
			this.panel3.Controls.Add(this.lbTerminalsOnly);
			this.panel3.Controls.Add(this.chkTerminalParameters);
			this.panel3.Controls.Add(this.label4);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.cbCategory);
			this.panel3.Controls.Add(this.chkValidated);
			this.panel3.Controls.Add(this.groupBox1);
			this.panel3.Controls.Add(this.udParamterCount);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.txtFunctionName);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(455, 145);
			this.panel3.TabIndex = 0;
			// 
			// lbTerminalsOnly
			// 
			this.lbTerminalsOnly.AutoSize = true;
			this.lbTerminalsOnly.Location = new System.Drawing.Point(137, 58);
			this.lbTerminalsOnly.Name = "lbTerminalsOnly";
			this.lbTerminalsOnly.Size = new System.Drawing.Size(127, 13);
			this.lbTerminalsOnly.TabIndex = 11;
			this.lbTerminalsOnly.Text = "Terminal Parameters Only";
			// 
			// chkTerminalParameters
			// 
			this.chkTerminalParameters.AutoSize = true;
			this.chkTerminalParameters.Location = new System.Drawing.Point(116, 58);
			this.chkTerminalParameters.Name = "chkTerminalParameters";
			this.chkTerminalParameters.Size = new System.Drawing.Size(15, 14);
			this.chkTerminalParameters.TabIndex = 10;
			this.chkTerminalParameters.UseVisualStyleBackColor = true;
			this.chkTerminalParameters.CheckedChanged += new System.EventHandler(this.chkTerminalParameters_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(168, 34);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(49, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Category";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(370, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Validated";
			// 
			// cbCategory
			// 
			this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCategory.FormattingEnabled = true;
			this.cbCategory.Location = new System.Drawing.Point(223, 31);
			this.cbCategory.Name = "cbCategory";
			this.cbCategory.Size = new System.Drawing.Size(131, 21);
			this.cbCategory.TabIndex = 7;
			this.cbCategory.TextChanged += new System.EventHandler(this.cbCategory_TextChanged);
			// 
			// chkValidated
			// 
			this.chkValidated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkValidated.AutoSize = true;
			this.chkValidated.Enabled = false;
			this.chkValidated.Location = new System.Drawing.Point(425, 35);
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
			this.groupBox1.Location = new System.Drawing.Point(7, 78);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(437, 63);
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
			this.lbPrototype.Size = new System.Drawing.Size(431, 44);
			this.lbPrototype.TabIndex = 0;
			// 
			// udParamterCount
			// 
			this.udParamterCount.Enabled = false;
			this.udParamterCount.Location = new System.Drawing.Point(116, 32);
			this.udParamterCount.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.udParamterCount.Name = "udParamterCount";
			this.udParamterCount.Size = new System.Drawing.Size(44, 20);
			this.udParamterCount.TabIndex = 3;
			this.udParamterCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udParamterCount.ValueChanged += new System.EventHandler(this.udParamterCount_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Parameter Count";
			// 
			// txtFunctionName
			// 
			this.txtFunctionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFunctionName.Location = new System.Drawing.Point(116, 6);
			this.txtFunctionName.Name = "txtFunctionName";
			this.txtFunctionName.Size = new System.Drawing.Size(324, 20);
			this.txtFunctionName.TabIndex = 1;
			this.txtFunctionName.TextChanged += new System.EventHandler(this.txtFunctionName_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Function Name";
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainAdd,
            this.tsMainDelete,
            this.tsMainSave,
            this.tsMainSeparator,
            this.tsMainValidate,
            this.toolStripSeparator2,
            this.tsMainLanguage});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(655, 36);
			this.tsMain.TabIndex = 2;
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
			this.tsMainAdd.Click += new System.EventHandler(this.btnNew_Click);
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
			this.tsMainDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
			this.tsMainSave.Click += new System.EventHandler(this.btnSave_Click);
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
			this.tsMainValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 36);
			// 
			// tsMainLanguage
			// 
			this.tsMainLanguage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMainLanguage_C,
            this.tsMainLanguage_CPP,
            this.tsMainLanguageItem_CSharp,
            this.tsMainLanguage_VisualBasic,
            this.tsMainLanguage_VBNET,
            this.tsMainLanguage_Java,
            this.tsMainLanguage_Fortran});
			this.tsMainLanguage.Image = ((System.Drawing.Image)(resources.GetObject("tsMainLanguage.Image")));
			this.tsMainLanguage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMainLanguage.Name = "tsMainLanguage";
			this.tsMainLanguage.Size = new System.Drawing.Size(13, 33);
			this.tsMainLanguage.ToolTipText = "Language";
			this.tsMainLanguage.TextChanged += new System.EventHandler(this.tsMainLanguage_TextChanged);
			this.tsMainLanguage.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsMainLanguage_DropDownItemClicked);
			// 
			// tsMainLanguage_C
			// 
			this.tsMainLanguage_C.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage_C.Name = "tsMainLanguage_C";
			this.tsMainLanguage_C.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_C.Tag = "1";
			this.tsMainLanguage_C.Text = "C";
			// 
			// tsMainLanguage_CPP
			// 
			this.tsMainLanguage_CPP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage_CPP.Name = "tsMainLanguage_CPP";
			this.tsMainLanguage_CPP.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_CPP.Tag = "2";
			this.tsMainLanguage_CPP.Text = "C++";
			// 
			// tsMainLanguageItem_CSharp
			// 
			this.tsMainLanguageItem_CSharp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguageItem_CSharp.Name = "tsMainLanguageItem_CSharp";
			this.tsMainLanguageItem_CSharp.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguageItem_CSharp.Tag = "3";
			this.tsMainLanguageItem_CSharp.Text = "C#";
			// 
			// tsMainLanguage_VisualBasic
			// 
			this.tsMainLanguage_VisualBasic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage_VisualBasic.Name = "tsMainLanguage_VisualBasic";
			this.tsMainLanguage_VisualBasic.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_VisualBasic.Tag = "4";
			this.tsMainLanguage_VisualBasic.Text = "Visual Basic";
			this.tsMainLanguage_VisualBasic.Visible = false;
			// 
			// tsMainLanguage_VBNET
			// 
			this.tsMainLanguage_VBNET.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage_VBNET.Name = "tsMainLanguage_VBNET";
			this.tsMainLanguage_VBNET.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_VBNET.Tag = "5";
			this.tsMainLanguage_VBNET.Text = "Visual Basic.NET";
			// 
			// tsMainLanguage_Java
			// 
			this.tsMainLanguage_Java.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsMainLanguage_Java.Name = "tsMainLanguage_Java";
			this.tsMainLanguage_Java.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_Java.Tag = "6";
			this.tsMainLanguage_Java.Text = "Java";
			// 
			// tsMainLanguage_Fortran
			// 
			this.tsMainLanguage_Fortran.Name = "tsMainLanguage_Fortran";
			this.tsMainLanguage_Fortran.Size = new System.Drawing.Size(162, 22);
			this.tsMainLanguage_Fortran.Tag = "7";
			this.tsMainLanguage_Fortran.Text = "FORTRAN";
			// 
			// fmFunctionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(655, 565);
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.tsMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmFunctionEditor";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "User Defined Functions";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmFunctionSet_FormClosed);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.tabFooter.ResumeLayout(false);
			this.pageDescription.ResumeLayout(false);
			this.pageErrors.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udParamterCount)).EndInit();
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
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.NumericUpDown udParamterCount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFunctionName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lbPrototype;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TabControl tabFooter;
		private System.Windows.Forms.TabPage pageDescription;
		private System.Windows.Forms.RichTextBox txtDescription;
		private System.Windows.Forms.TabPage pageErrors;
		private System.Windows.Forms.ListView lvErrors;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton tsMainAdd;
		private System.Windows.Forms.ToolStripButton tsMainDelete;
		private System.Windows.Forms.ToolStripButton tsMainSave;
		private System.Windows.Forms.ToolStripButton tsMainValidate;
		private System.Windows.Forms.ToolStripSeparator tsMainSeparator;
		private System.Windows.Forms.ToolStripDropDownButton tsMainLanguage;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_C;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_CPP;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguageItem_CSharp;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_VisualBasic;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_VBNET;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_Java;
		private System.Windows.Forms.ToolStripMenuItem tsMainLanguage_Fortran;
		private System.Windows.Forms.CheckBox chkValidated;
		private System.Windows.Forms.ComboBox cbCategory;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkTerminalParameters;
		private System.Windows.Forms.Label lbTerminalsOnly;
	}
}