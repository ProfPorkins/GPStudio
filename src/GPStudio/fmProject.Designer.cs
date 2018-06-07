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
	partial class fmProject
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Profiles", System.Windows.Forms.HorizontalAlignment.Left);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmProject));
			this.tabProject = new System.Windows.Forms.TabControl();
			this.pageConfiguration = new System.Windows.Forms.TabPage();
			this.txtProjectName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.pnlProfilesToolbar = new System.Windows.Forms.Panel();
			this.tsProfiles = new System.Windows.Forms.ToolStrip();
			this.btnProfileAdd = new System.Windows.Forms.ToolStripButton();
			this.btnProfileDelete = new System.Windows.Forms.ToolStripButton();
			this.btnProfileSave = new System.Windows.Forms.ToolStripButton();
			this.btnProfileCopy = new System.Windows.Forms.ToolStripButton();
			this.tabProfile = new System.Windows.Forms.TabControl();
			this.tabModelingType = new System.Windows.Forms.TabPage();
			this.gbModeling_Regression = new System.Windows.Forms.GroupBox();
			this.udModeling_PredictionDistance = new System.Windows.Forms.NumericUpDown();
			this.udModeling_InputDimension = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.rdModeling_TimeSeries = new System.Windows.Forms.RadioButton();
			this.rdModeling_Regression = new System.Windows.Forms.RadioButton();
			this.tabProgramStructure = new System.Windows.Forms.TabPage();
			this.label8 = new System.Windows.Forms.Label();
			this.udADR = new System.Windows.Forms.NumericUpDown();
			this.lvADR = new ListViewEx.ListViewEx();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.btnADRDelete = new System.Windows.Forms.Button();
			this.btnADRAdd = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.udADF = new System.Windows.Forms.NumericUpDown();
			this.lvADF = new ListViewEx.ListViewEx();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colParams = new System.Windows.Forms.ColumnHeader();
			this.btnADFDelete = new System.Windows.Forms.Button();
			this.btnADFAdd = new System.Windows.Forms.Button();
			this.udStructure_MemoryCount = new System.Windows.Forms.NumericUpDown();
			this.chkStructure_UseMemory = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.udADL = new System.Windows.Forms.NumericUpDown();
			this.lvADL = new ListViewEx.ListViewEx();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.btnADLDelete = new System.Windows.Forms.Button();
			this.btnADLAdd = new System.Windows.Forms.Button();
			this.tabFunctionSet = new System.Windows.Forms.TabPage();
			this.lvFunctions = new System.Windows.Forms.ListView();
			this.colFunctionName = new System.Windows.Forms.ColumnHeader();
			this.colFunctionArity = new System.Windows.Forms.ColumnHeader();
			this.colFunctionDescription = new System.Windows.Forms.ColumnHeader();
			this.colFunctionCategory = new System.Windows.Forms.ColumnHeader();
			this.tabTerminalSet = new System.Windows.Forms.TabPage();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.txtTS_IntegerMax = new System.Windows.Forms.MaskedTextBox();
			this.txtTS_IntegerMin = new System.Windows.Forms.MaskedTextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.chkTS_RndDouble = new System.Windows.Forms.CheckBox();
			this.chkTS_RndInteger = new System.Windows.Forms.CheckBox();
			this.tabFitness = new System.Windows.Forms.TabPage();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.flowFitnessFunctions = new System.Windows.Forms.FlowLayoutPanel();
			this.groupBox13 = new System.Windows.Forms.GroupBox();
			this.chkFitness_MultiObjective = new System.Windows.Forms.CheckBox();
			this.chkFitness_Parsimony = new System.Windows.Forms.CheckBox();
			this.tabPopulationStructure = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.label24 = new System.Windows.Forms.Label();
			this.udDistributed_Topology_StarPercent = new System.Windows.Forms.NumericUpDown();
			this.rdDistributed_Topology_Star = new System.Windows.Forms.RadioButton();
			this.rdDistributed_Topology_Ring = new System.Windows.Forms.RadioButton();
			this.udDistributed_TransferCount = new System.Windows.Forms.NumericUpDown();
			this.label23 = new System.Windows.Forms.Label();
			this.groupBox16 = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.udStructure_Crossover = new System.Windows.Forms.NumericUpDown();
			this.udStructure_Mutation = new System.Windows.Forms.NumericUpDown();
			this.udStructure_Reproduction = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.udPopulation_TreeDepth = new System.Windows.Forms.NumericUpDown();
			this.label12 = new System.Windows.Forms.Label();
			this.cmbStructure_PopReproduction = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.cmbStructure_PopInit = new System.Windows.Forms.ComboBox();
			this.udStructure_PopSize = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.tabGenerationOptions = new System.Windows.Forms.TabPage();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.udGens_MaxNumber = new System.Windows.Forms.NumericUpDown();
			this.chkGens_HitsMaxed = new System.Windows.Forms.CheckBox();
			this.chkGens_RawFitness0 = new System.Windows.Forms.CheckBox();
			this.chkGens_MaxNumber = new System.Windows.Forms.CheckBox();
			this.lvProfiles = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.ilProfiles = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnDataValidationDelete = new System.Windows.Forms.Button();
			this.btnDataValidationAdd = new System.Windows.Forms.Button();
			this.cbDataValidation = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnDataTraining = new System.Windows.Forms.Button();
			this.txtDataTraining = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pageModeling = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label22 = new System.Windows.Forms.Label();
			this.cmbPopulationFitnessServers = new System.Windows.Forms.ComboBox();
			this.statusModeling = new System.Windows.Forms.StatusStrip();
			this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.pgStatus = new System.Windows.Forms.ToolStripProgressBar();
			this.tabModeling = new System.Windows.Forms.TabControl();
			this.pageModelFitness = new System.Windows.Forms.TabPage();
			this.graphFitness = new ZedGraph.ZedGraphControl();
			this.pagePopulationFitness = new System.Windows.Forms.TabPage();
			this.graphPopulationFitness = new ZedGraph.ZedGraphControl();
			this.pagePopulationComplexity = new System.Windows.Forms.TabPage();
			this.graphPopulationComplexity = new ZedGraph.ZedGraphControl();
			this.groupBox14 = new System.Windows.Forms.GroupBox();
			this.lvProfilesModeling = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.pnlModelingToolbar = new System.Windows.Forms.Panel();
			this.tsModeling = new System.Windows.Forms.ToolStrip();
			this.btnModel = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsModelInteractive = new System.Windows.Forms.ToolStripMenuItem();
			this.tsModelBatch = new System.Windows.Forms.ToolStripMenuItem();
			this.btnCancel = new System.Windows.Forms.ToolStripButton();
			this.pgModeling = new System.Windows.Forms.ToolStripProgressBar();
			this.pageResults = new System.Windows.Forms.TabPage();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.splitResults = new System.Windows.Forms.SplitContainer();
			this.lvProfilesResults = new System.Windows.Forms.ListView();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.cbResultsDataSet = new System.Windows.Forms.ComboBox();
			this.lvResultsPrograms = new System.Windows.Forms.ListView();
			this.colDateTime = new System.Windows.Forms.ColumnHeader();
			this.colFitness = new System.Windows.Forms.ColumnHeader();
			this.colHits = new System.Windows.Forms.ColumnHeader();
			this.colComplexity = new System.Windows.Forms.ColumnHeader();
			this.tabResults = new System.Windows.Forms.TabControl();
			this.pageResultsGraphical = new System.Windows.Forms.TabPage();
			this.graphResults = new ZedGraph.ZedGraphControl();
			this.panel3 = new System.Windows.Forms.Panel();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.chkModelOnly = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkCount = new System.Windows.Forms.CheckBox();
			this.cbXAxis = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rdChartTypeScatter = new System.Windows.Forms.RadioButton();
			this.rdChartTypeBar = new System.Windows.Forms.RadioButton();
			this.rdChartTypeXY = new System.Windows.Forms.RadioButton();
			this.pageResultsTabular = new System.Windows.Forms.TabPage();
			this.dgResults = new System.Windows.Forms.DataGridView();
			this.sbResultsTabular = new System.Windows.Forms.StatusStrip();
			this.tsTabularExport = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsExportCSV = new System.Windows.Forms.ToolStripMenuItem();
			this.tsExportSSV = new System.Windows.Forms.ToolStripMenuItem();
			this.tsExportTab = new System.Windows.Forms.ToolStripMenuItem();
			this.tsTabularFitnessFunction = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsTabularErrorAverage = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsTabularErrorMedian = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsTabularErrorStdDev = new System.Windows.Forms.ToolStripStatusLabel();
			this.pageProgram = new System.Windows.Forms.TabPage();
			this.pnlProgramText = new System.Windows.Forms.Panel();
			this.txtProgram = new System.Windows.Forms.RichTextBox();
			this.sbResultsProgram = new System.Windows.Forms.StatusStrip();
			this.tsmenuLanguage = new System.Windows.Forms.ToolStripDropDownButton();
			this.menuProgramC = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProgramCPP = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProgramCSharp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProgramVB = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProgramJava = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProgramFortran = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuProgramExport = new System.Windows.Forms.ToolStripMenuItem();
			this.pageModelingHistory = new System.Windows.Forms.TabPage();
			this.diagProgram = new Netron.Lithium.LithiumControl();
			this.btnDiagramConstruct = new System.Windows.Forms.Button();
			this.lvProgramHist = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.statusResults = new System.Windows.Forms.StatusStrip();
			this.tsProgressResults = new System.Windows.Forms.ToolStripProgressBar();
			this.contextMenuPrograms = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.dlgFileSaveSource = new System.Windows.Forms.SaveFileDialog();
			this.dlgFileSaveGrid = new System.Windows.Forms.SaveFileDialog();
			this.ttFunctionSet = new System.Windows.Forms.ToolTip(this.components);
			this.tabProject.SuspendLayout();
			this.pageConfiguration.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.pnlProfilesToolbar.SuspendLayout();
			this.tsProfiles.SuspendLayout();
			this.tabProfile.SuspendLayout();
			this.tabModelingType.SuspendLayout();
			this.gbModeling_Regression.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udModeling_PredictionDistance)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udModeling_InputDimension)).BeginInit();
			this.tabProgramStructure.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udADR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udADF)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_MemoryCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udADL)).BeginInit();
			this.tabFunctionSet.SuspendLayout();
			this.tabTerminalSet.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.tabFitness.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox13.SuspendLayout();
			this.tabPopulationStructure.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox12.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDistributed_Topology_StarPercent)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDistributed_TransferCount)).BeginInit();
			this.groupBox16.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Crossover)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Mutation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Reproduction)).BeginInit();
			this.groupBox9.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPopulation_TreeDepth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_PopSize)).BeginInit();
			this.tabGenerationOptions.SuspendLayout();
			this.groupBox10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGens_MaxNumber)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.pageModeling.SuspendLayout();
			this.panel1.SuspendLayout();
			this.statusModeling.SuspendLayout();
			this.tabModeling.SuspendLayout();
			this.pageModelFitness.SuspendLayout();
			this.pagePopulationFitness.SuspendLayout();
			this.pagePopulationComplexity.SuspendLayout();
			this.groupBox14.SuspendLayout();
			this.pnlModelingToolbar.SuspendLayout();
			this.tsModeling.SuspendLayout();
			this.pageResults.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.splitResults.Panel1.SuspendLayout();
			this.splitResults.Panel2.SuspendLayout();
			this.splitResults.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabResults.SuspendLayout();
			this.pageResultsGraphical.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.pageResultsTabular.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
			this.sbResultsTabular.SuspendLayout();
			this.pageProgram.SuspendLayout();
			this.pnlProgramText.SuspendLayout();
			this.sbResultsProgram.SuspendLayout();
			this.pageModelingHistory.SuspendLayout();
			this.diagProgram.SuspendLayout();
			this.statusResults.SuspendLayout();
			this.contextMenuPrograms.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabProject
			// 
			this.tabProject.Controls.Add(this.pageConfiguration);
			this.tabProject.Controls.Add(this.pageModeling);
			this.tabProject.Controls.Add(this.pageResults);
			this.tabProject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabProject.Location = new System.Drawing.Point(0, 0);
			this.tabProject.Name = "tabProject";
			this.tabProject.SelectedIndex = 0;
			this.tabProject.Size = new System.Drawing.Size(742, 491);
			this.tabProject.TabIndex = 0;
			// 
			// pageConfiguration
			// 
			this.pageConfiguration.Controls.Add(this.txtProjectName);
			this.pageConfiguration.Controls.Add(this.label1);
			this.pageConfiguration.Controls.Add(this.groupBox2);
			this.pageConfiguration.Controls.Add(this.groupBox1);
			this.pageConfiguration.Location = new System.Drawing.Point(4, 22);
			this.pageConfiguration.Name = "pageConfiguration";
			this.pageConfiguration.Padding = new System.Windows.Forms.Padding(3);
			this.pageConfiguration.Size = new System.Drawing.Size(734, 465);
			this.pageConfiguration.TabIndex = 0;
			this.pageConfiguration.Text = "Configuration";
			this.pageConfiguration.UseVisualStyleBackColor = true;
			// 
			// txtProjectName
			// 
			this.txtProjectName.Location = new System.Drawing.Point(85, 6);
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.Size = new System.Drawing.Size(304, 20);
			this.txtProjectName.TabIndex = 5;
			this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Project Name";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.pnlProfilesToolbar);
			this.groupBox2.Controls.Add(this.tabProfile);
			this.groupBox2.Controls.Add(this.lvProfiles);
			this.groupBox2.Location = new System.Drawing.Point(8, 32);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(727, 340);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Modeling Profiles";
			// 
			// pnlProfilesToolbar
			// 
			this.pnlProfilesToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlProfilesToolbar.Controls.Add(this.tsProfiles);
			this.pnlProfilesToolbar.Location = new System.Drawing.Point(7, 297);
			this.pnlProfilesToolbar.Name = "pnlProfilesToolbar";
			this.pnlProfilesToolbar.Size = new System.Drawing.Size(210, 38);
			this.pnlProfilesToolbar.TabIndex = 6;
			// 
			// tsProfiles
			// 
			this.tsProfiles.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsProfiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProfileAdd,
            this.btnProfileDelete,
            this.btnProfileSave,
            this.btnProfileCopy});
			this.tsProfiles.Location = new System.Drawing.Point(0, 0);
			this.tsProfiles.Name = "tsProfiles";
			this.tsProfiles.Size = new System.Drawing.Size(210, 36);
			this.tsProfiles.TabIndex = 0;
			this.tsProfiles.Text = "toolStrip1";
			// 
			// btnProfileAdd
			// 
			this.btnProfileAdd.Image = global::GPStudio.Client.Properties.Resources.newfolder;
			this.btnProfileAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProfileAdd.Name = "btnProfileAdd";
			this.btnProfileAdd.Size = new System.Drawing.Size(30, 33);
			this.btnProfileAdd.Text = "Add";
			this.btnProfileAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnProfileAdd.Click += new System.EventHandler(this.btnProfileAdd_Click);
			// 
			// btnProfileDelete
			// 
			this.btnProfileDelete.Enabled = false;
			this.btnProfileDelete.Image = global::GPStudio.Client.Properties.Resources.error;
			this.btnProfileDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProfileDelete.Name = "btnProfileDelete";
			this.btnProfileDelete.Size = new System.Drawing.Size(42, 33);
			this.btnProfileDelete.Text = "Delete";
			this.btnProfileDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnProfileDelete.Click += new System.EventHandler(this.btnProfileDelete_Click);
			// 
			// btnProfileSave
			// 
			this.btnProfileSave.Enabled = false;
			this.btnProfileSave.Image = global::GPStudio.Client.Properties.Resources.Save;
			this.btnProfileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProfileSave.Name = "btnProfileSave";
			this.btnProfileSave.Size = new System.Drawing.Size(35, 33);
			this.btnProfileSave.Text = "Save";
			this.btnProfileSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnProfileSave.Click += new System.EventHandler(this.btnProfileSave_Click);
			// 
			// btnProfileCopy
			// 
			this.btnProfileCopy.Enabled = false;
			this.btnProfileCopy.Image = global::GPStudio.Client.Properties.Resources.CopyFolder;
			this.btnProfileCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnProfileCopy.Name = "btnProfileCopy";
			this.btnProfileCopy.Size = new System.Drawing.Size(36, 33);
			this.btnProfileCopy.Text = "Copy";
			this.btnProfileCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnProfileCopy.Click += new System.EventHandler(this.btnProfileCopy_Click);
			// 
			// tabProfile
			// 
			this.tabProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabProfile.Controls.Add(this.tabModelingType);
			this.tabProfile.Controls.Add(this.tabProgramStructure);
			this.tabProfile.Controls.Add(this.tabFunctionSet);
			this.tabProfile.Controls.Add(this.tabTerminalSet);
			this.tabProfile.Controls.Add(this.tabFitness);
			this.tabProfile.Controls.Add(this.tabPopulationStructure);
			this.tabProfile.Controls.Add(this.tabGenerationOptions);
			this.tabProfile.Location = new System.Drawing.Point(223, 11);
			this.tabProfile.Name = "tabProfile";
			this.tabProfile.SelectedIndex = 0;
			this.tabProfile.Size = new System.Drawing.Size(492, 323);
			this.tabProfile.TabIndex = 4;
			// 
			// tabModelingType
			// 
			this.tabModelingType.Controls.Add(this.gbModeling_Regression);
			this.tabModelingType.Controls.Add(this.rdModeling_TimeSeries);
			this.tabModelingType.Controls.Add(this.rdModeling_Regression);
			this.tabModelingType.Location = new System.Drawing.Point(4, 22);
			this.tabModelingType.Name = "tabModelingType";
			this.tabModelingType.Size = new System.Drawing.Size(484, 297);
			this.tabModelingType.TabIndex = 5;
			this.tabModelingType.Text = "Type";
			this.tabModelingType.UseVisualStyleBackColor = true;
			// 
			// gbModeling_Regression
			// 
			this.gbModeling_Regression.Controls.Add(this.udModeling_PredictionDistance);
			this.gbModeling_Regression.Controls.Add(this.udModeling_InputDimension);
			this.gbModeling_Regression.Controls.Add(this.label14);
			this.gbModeling_Regression.Controls.Add(this.label13);
			this.gbModeling_Regression.Enabled = false;
			this.gbModeling_Regression.Location = new System.Drawing.Point(26, 57);
			this.gbModeling_Regression.Name = "gbModeling_Regression";
			this.gbModeling_Regression.Size = new System.Drawing.Size(182, 87);
			this.gbModeling_Regression.TabIndex = 10;
			this.gbModeling_Regression.TabStop = false;
			this.gbModeling_Regression.Text = "Time Series - Data Use";
			// 
			// udModeling_PredictionDistance
			// 
			this.udModeling_PredictionDistance.Location = new System.Drawing.Point(121, 54);
			this.udModeling_PredictionDistance.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.udModeling_PredictionDistance.Name = "udModeling_PredictionDistance";
			this.udModeling_PredictionDistance.Size = new System.Drawing.Size(49, 20);
			this.udModeling_PredictionDistance.TabIndex = 3;
			this.udModeling_PredictionDistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udModeling_PredictionDistance.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// udModeling_InputDimension
			// 
			this.udModeling_InputDimension.Location = new System.Drawing.Point(121, 25);
			this.udModeling_InputDimension.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.udModeling_InputDimension.Name = "udModeling_InputDimension";
			this.udModeling_InputDimension.Size = new System.Drawing.Size(49, 20);
			this.udModeling_InputDimension.TabIndex = 2;
			this.udModeling_InputDimension.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.udModeling_InputDimension.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(6, 54);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(99, 13);
			this.label14.TabIndex = 1;
			this.label14.Text = "Prediction Distance";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(6, 25);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(83, 13);
			this.label13.TabIndex = 0;
			this.label13.Text = "Input Dimension";
			// 
			// rdModeling_TimeSeries
			// 
			this.rdModeling_TimeSeries.AutoSize = true;
			this.rdModeling_TimeSeries.Location = new System.Drawing.Point(12, 34);
			this.rdModeling_TimeSeries.Name = "rdModeling_TimeSeries";
			this.rdModeling_TimeSeries.Size = new System.Drawing.Size(80, 17);
			this.rdModeling_TimeSeries.TabIndex = 9;
			this.rdModeling_TimeSeries.Text = "Time Series";
			this.rdModeling_TimeSeries.UseVisualStyleBackColor = true;
			this.rdModeling_TimeSeries.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// rdModeling_Regression
			// 
			this.rdModeling_Regression.AutoSize = true;
			this.rdModeling_Regression.Checked = true;
			this.rdModeling_Regression.Location = new System.Drawing.Point(12, 11);
			this.rdModeling_Regression.Name = "rdModeling_Regression";
			this.rdModeling_Regression.Size = new System.Drawing.Size(78, 17);
			this.rdModeling_Regression.TabIndex = 8;
			this.rdModeling_Regression.TabStop = true;
			this.rdModeling_Regression.Text = "Regression";
			this.rdModeling_Regression.UseVisualStyleBackColor = true;
			this.rdModeling_Regression.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// tabProgramStructure
			// 
			this.tabProgramStructure.Controls.Add(this.label8);
			this.tabProgramStructure.Controls.Add(this.udADR);
			this.tabProgramStructure.Controls.Add(this.lvADR);
			this.tabProgramStructure.Controls.Add(this.btnADRDelete);
			this.tabProgramStructure.Controls.Add(this.btnADRAdd);
			this.tabProgramStructure.Controls.Add(this.label7);
			this.tabProgramStructure.Controls.Add(this.udADF);
			this.tabProgramStructure.Controls.Add(this.lvADF);
			this.tabProgramStructure.Controls.Add(this.btnADFDelete);
			this.tabProgramStructure.Controls.Add(this.btnADFAdd);
			this.tabProgramStructure.Controls.Add(this.udStructure_MemoryCount);
			this.tabProgramStructure.Controls.Add(this.chkStructure_UseMemory);
			this.tabProgramStructure.Controls.Add(this.label6);
			this.tabProgramStructure.Controls.Add(this.udADL);
			this.tabProgramStructure.Controls.Add(this.lvADL);
			this.tabProgramStructure.Controls.Add(this.btnADLDelete);
			this.tabProgramStructure.Controls.Add(this.btnADLAdd);
			this.tabProgramStructure.Location = new System.Drawing.Point(4, 22);
			this.tabProgramStructure.Name = "tabProgramStructure";
			this.tabProgramStructure.Padding = new System.Windows.Forms.Padding(3);
			this.tabProgramStructure.Size = new System.Drawing.Size(484, 297);
			this.tabProgramStructure.TabIndex = 0;
			this.tabProgramStructure.Text = "Structure";
			this.tabProgramStructure.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label8.Location = new System.Drawing.Point(274, 13);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(110, 13);
			this.label8.TabIndex = 51;
			this.label8.Text = "Automatic Recursions";
			this.label8.Visible = false;
			// 
			// udADR
			// 
			this.udADR.Location = new System.Drawing.Point(348, 109);
			this.udADR.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.udADR.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADR.Name = "udADR";
			this.udADR.Size = new System.Drawing.Size(69, 20);
			this.udADR.TabIndex = 50;
			this.udADR.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADR.Visible = false;
			this.udADR.ValueChanged += new System.EventHandler(this.udADR_ValueChanged);
			// 
			// lvADR
			// 
			this.lvADR.AllowColumnReorder = true;
			this.lvADR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvADR.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9});
			this.lvADR.DoubleClickActivation = false;
			this.lvADR.FullRowSelect = true;
			this.lvADR.GridLines = true;
			this.lvADR.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvADR.HideSelection = false;
			this.lvADR.Location = new System.Drawing.Point(277, 29);
			this.lvADR.MultiSelect = false;
			this.lvADR.Name = "lvADR";
			this.lvADR.Size = new System.Drawing.Size(129, 116);
			this.lvADR.TabIndex = 49;
			this.lvADR.UseCompatibleStateImageBehavior = false;
			this.lvADR.View = System.Windows.Forms.View.Details;
			this.lvADR.Visible = false;
			this.lvADR.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvADR_SubItemClicked);
			this.lvADR.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvADR_ItemSelectionChanged);
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Name";
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Params";
			this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnADRDelete
			// 
			this.btnADRDelete.Enabled = false;
			this.btnADRDelete.Location = new System.Drawing.Point(348, 146);
			this.btnADRDelete.Name = "btnADRDelete";
			this.btnADRDelete.Size = new System.Drawing.Size(58, 23);
			this.btnADRDelete.TabIndex = 48;
			this.btnADRDelete.Text = "Delete";
			this.btnADRDelete.UseVisualStyleBackColor = true;
			this.btnADRDelete.Visible = false;
			this.btnADRDelete.Click += new System.EventHandler(this.btnADRDelete_Click);
			// 
			// btnADRAdd
			// 
			this.btnADRAdd.Location = new System.Drawing.Point(284, 146);
			this.btnADRAdd.Name = "btnADRAdd";
			this.btnADRAdd.Size = new System.Drawing.Size(58, 23);
			this.btnADRAdd.TabIndex = 47;
			this.btnADRAdd.Text = "Add";
			this.btnADRAdd.UseVisualStyleBackColor = true;
			this.btnADRAdd.Visible = false;
			this.btnADRAdd.Click += new System.EventHandler(this.btnADRAdd_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label7.Location = new System.Drawing.Point(6, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(103, 13);
			this.label7.TabIndex = 46;
			this.label7.Text = "Automatic Functions";
			// 
			// udADF
			// 
			this.udADF.Location = new System.Drawing.Point(78, 109);
			this.udADF.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.udADF.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADF.Name = "udADF";
			this.udADF.Size = new System.Drawing.Size(69, 20);
			this.udADF.TabIndex = 45;
			this.udADF.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADF.Visible = false;
			this.udADF.ValueChanged += new System.EventHandler(this.udADF_ValueChanged);
			// 
			// lvADF
			// 
			this.lvADF.AllowColumnReorder = true;
			this.lvADF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvADF.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colParams});
			this.lvADF.DoubleClickActivation = false;
			this.lvADF.FullRowSelect = true;
			this.lvADF.GridLines = true;
			this.lvADF.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvADF.HideSelection = false;
			this.lvADF.Location = new System.Drawing.Point(7, 29);
			this.lvADF.MultiSelect = false;
			this.lvADF.Name = "lvADF";
			this.lvADF.Size = new System.Drawing.Size(129, 116);
			this.lvADF.TabIndex = 44;
			this.lvADF.UseCompatibleStateImageBehavior = false;
			this.lvADF.View = System.Windows.Forms.View.Details;
			this.lvADF.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvADF_SubItemClicked);
			this.lvADF.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvADF_ItemSelectionChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			// 
			// colParams
			// 
			this.colParams.Text = "Params";
			this.colParams.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnADFDelete
			// 
			this.btnADFDelete.Enabled = false;
			this.btnADFDelete.Location = new System.Drawing.Point(78, 146);
			this.btnADFDelete.Name = "btnADFDelete";
			this.btnADFDelete.Size = new System.Drawing.Size(58, 23);
			this.btnADFDelete.TabIndex = 43;
			this.btnADFDelete.Text = "Delete";
			this.btnADFDelete.UseVisualStyleBackColor = true;
			this.btnADFDelete.Click += new System.EventHandler(this.btnADFDelete_Click);
			// 
			// btnADFAdd
			// 
			this.btnADFAdd.Location = new System.Drawing.Point(14, 146);
			this.btnADFAdd.Name = "btnADFAdd";
			this.btnADFAdd.Size = new System.Drawing.Size(58, 23);
			this.btnADFAdd.TabIndex = 42;
			this.btnADFAdd.Text = "Add";
			this.btnADFAdd.UseVisualStyleBackColor = true;
			this.btnADFAdd.Click += new System.EventHandler(this.btnADFAdd_Click);
			// 
			// udStructure_MemoryCount
			// 
			this.udStructure_MemoryCount.Location = new System.Drawing.Point(32, 219);
			this.udStructure_MemoryCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udStructure_MemoryCount.Name = "udStructure_MemoryCount";
			this.udStructure_MemoryCount.Size = new System.Drawing.Size(63, 20);
			this.udStructure_MemoryCount.TabIndex = 41;
			this.udStructure_MemoryCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udStructure_MemoryCount.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// chkStructure_UseMemory
			// 
			this.chkStructure_UseMemory.AutoSize = true;
			this.chkStructure_UseMemory.Location = new System.Drawing.Point(13, 196);
			this.chkStructure_UseMemory.Name = "chkStructure_UseMemory";
			this.chkStructure_UseMemory.Size = new System.Drawing.Size(104, 17);
			this.chkStructure_UseMemory.TabIndex = 40;
			this.chkStructure_UseMemory.Text = "Indexed Memory";
			this.chkStructure_UseMemory.UseVisualStyleBackColor = true;
			this.chkStructure_UseMemory.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label6.Location = new System.Drawing.Point(139, 13);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 13);
			this.label6.TabIndex = 39;
			this.label6.Text = "Automatic Loops";
			// 
			// udADL
			// 
			this.udADL.Location = new System.Drawing.Point(213, 109);
			this.udADL.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
			this.udADL.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADL.Name = "udADL";
			this.udADL.Size = new System.Drawing.Size(69, 20);
			this.udADL.TabIndex = 38;
			this.udADL.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udADL.Visible = false;
			this.udADL.ValueChanged += new System.EventHandler(this.udADL_ValueChanged);
			// 
			// lvADL
			// 
			this.lvADL.AllowColumnReorder = true;
			this.lvADL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvADL.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader7});
			this.lvADL.DoubleClickActivation = false;
			this.lvADL.FullRowSelect = true;
			this.lvADL.GridLines = true;
			this.lvADL.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvADL.HideSelection = false;
			this.lvADL.Location = new System.Drawing.Point(142, 29);
			this.lvADL.MultiSelect = false;
			this.lvADL.Name = "lvADL";
			this.lvADL.Size = new System.Drawing.Size(129, 116);
			this.lvADL.TabIndex = 37;
			this.lvADL.UseCompatibleStateImageBehavior = false;
			this.lvADL.View = System.Windows.Forms.View.Details;
			this.lvADL.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvADL_SubItemClicked);
			this.lvADL.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvADL_ItemSelectionChanged);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Params";
			this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnADLDelete
			// 
			this.btnADLDelete.Enabled = false;
			this.btnADLDelete.Location = new System.Drawing.Point(210, 146);
			this.btnADLDelete.Name = "btnADLDelete";
			this.btnADLDelete.Size = new System.Drawing.Size(58, 23);
			this.btnADLDelete.TabIndex = 36;
			this.btnADLDelete.Text = "Delete";
			this.btnADLDelete.UseVisualStyleBackColor = true;
			this.btnADLDelete.Click += new System.EventHandler(this.btnADLDelete_Click);
			// 
			// btnADLAdd
			// 
			this.btnADLAdd.Location = new System.Drawing.Point(146, 146);
			this.btnADLAdd.Name = "btnADLAdd";
			this.btnADLAdd.Size = new System.Drawing.Size(58, 23);
			this.btnADLAdd.TabIndex = 35;
			this.btnADLAdd.Text = "Add";
			this.btnADLAdd.UseVisualStyleBackColor = true;
			this.btnADLAdd.Click += new System.EventHandler(this.btnADLAdd_Click);
			// 
			// tabFunctionSet
			// 
			this.tabFunctionSet.Controls.Add(this.lvFunctions);
			this.tabFunctionSet.Location = new System.Drawing.Point(4, 22);
			this.tabFunctionSet.Name = "tabFunctionSet";
			this.tabFunctionSet.Size = new System.Drawing.Size(484, 297);
			this.tabFunctionSet.TabIndex = 2;
			this.tabFunctionSet.Text = "Functions";
			this.tabFunctionSet.UseVisualStyleBackColor = true;
			// 
			// lvFunctions
			// 
			this.lvFunctions.CheckBoxes = true;
			this.lvFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFunctionName,
            this.colFunctionArity,
            this.colFunctionDescription,
            this.colFunctionCategory});
			this.lvFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFunctions.FullRowSelect = true;
			this.lvFunctions.GridLines = true;
			this.lvFunctions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvFunctions.Location = new System.Drawing.Point(0, 0);
			this.lvFunctions.MultiSelect = false;
			this.lvFunctions.Name = "lvFunctions";
			this.lvFunctions.Size = new System.Drawing.Size(484, 297);
			this.lvFunctions.TabIndex = 1;
			this.lvFunctions.UseCompatibleStateImageBehavior = false;
			this.lvFunctions.View = System.Windows.Forms.View.Details;
			this.lvFunctions.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFunctions_ItemChecked);
			// 
			// colFunctionName
			// 
			this.colFunctionName.Text = "Name";
			this.colFunctionName.Width = 90;
			// 
			// colFunctionArity
			// 
			this.colFunctionArity.Text = "Params";
			this.colFunctionArity.Width = 50;
			// 
			// colFunctionDescription
			// 
			this.colFunctionDescription.Text = "Description";
			this.colFunctionDescription.Width = 190;
			// 
			// colFunctionCategory
			// 
			this.colFunctionCategory.Text = "Category";
			this.colFunctionCategory.Width = 75;
			// 
			// tabTerminalSet
			// 
			this.tabTerminalSet.Controls.Add(this.groupBox11);
			this.tabTerminalSet.Location = new System.Drawing.Point(4, 22);
			this.tabTerminalSet.Name = "tabTerminalSet";
			this.tabTerminalSet.Size = new System.Drawing.Size(484, 297);
			this.tabTerminalSet.TabIndex = 3;
			this.tabTerminalSet.Text = "Constants";
			this.tabTerminalSet.UseVisualStyleBackColor = true;
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.txtTS_IntegerMax);
			this.groupBox11.Controls.Add(this.txtTS_IntegerMin);
			this.groupBox11.Controls.Add(this.label18);
			this.groupBox11.Controls.Add(this.label19);
			this.groupBox11.Controls.Add(this.chkTS_RndDouble);
			this.groupBox11.Controls.Add(this.chkTS_RndInteger);
			this.groupBox11.Location = new System.Drawing.Point(3, 3);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(311, 109);
			this.groupBox11.TabIndex = 0;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "Numerical Constants";
			// 
			// txtTS_IntegerMax
			// 
			this.txtTS_IntegerMax.Location = new System.Drawing.Point(193, 42);
			this.txtTS_IntegerMax.Mask = "0000000000";
			this.txtTS_IntegerMax.Name = "txtTS_IntegerMax";
			this.txtTS_IntegerMax.PromptChar = ' ';
			this.txtTS_IntegerMax.Size = new System.Drawing.Size(86, 20);
			this.txtTS_IntegerMax.TabIndex = 7;
			this.txtTS_IntegerMax.Text = "100";
			this.txtTS_IntegerMax.TextChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// txtTS_IntegerMin
			// 
			this.txtTS_IntegerMin.Location = new System.Drawing.Point(78, 42);
			this.txtTS_IntegerMin.Mask = "0000000000";
			this.txtTS_IntegerMin.Name = "txtTS_IntegerMin";
			this.txtTS_IntegerMin.PromptChar = ' ';
			this.txtTS_IntegerMin.Size = new System.Drawing.Size(77, 20);
			this.txtTS_IntegerMin.TabIndex = 6;
			this.txtTS_IntegerMin.Text = "0";
			this.txtTS_IntegerMin.TextChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(161, 45);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(26, 13);
			this.label18.TabIndex = 5;
			this.label18.Text = "max";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(49, 45);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(23, 13);
			this.label19.TabIndex = 4;
			this.label19.Text = "min";
			// 
			// chkTS_RndDouble
			// 
			this.chkTS_RndDouble.AutoSize = true;
			this.chkTS_RndDouble.Location = new System.Drawing.Point(13, 79);
			this.chkTS_RndDouble.Name = "chkTS_RndDouble";
			this.chkTS_RndDouble.Size = new System.Drawing.Size(103, 17);
			this.chkTS_RndDouble.TabIndex = 1;
			this.chkTS_RndDouble.Text = "Random Double";
			this.chkTS_RndDouble.UseVisualStyleBackColor = true;
			this.chkTS_RndDouble.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// chkTS_RndInteger
			// 
			this.chkTS_RndInteger.AutoSize = true;
			this.chkTS_RndInteger.Location = new System.Drawing.Point(13, 19);
			this.chkTS_RndInteger.Name = "chkTS_RndInteger";
			this.chkTS_RndInteger.Size = new System.Drawing.Size(102, 17);
			this.chkTS_RndInteger.TabIndex = 0;
			this.chkTS_RndInteger.Text = "Random Integer";
			this.chkTS_RndInteger.UseVisualStyleBackColor = true;
			this.chkTS_RndInteger.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// tabFitness
			// 
			this.tabFitness.Controls.Add(this.groupBox7);
			this.tabFitness.Controls.Add(this.groupBox13);
			this.tabFitness.Location = new System.Drawing.Point(4, 22);
			this.tabFitness.Name = "tabFitness";
			this.tabFitness.Size = new System.Drawing.Size(484, 297);
			this.tabFitness.TabIndex = 4;
			this.tabFitness.Text = "Fitness";
			this.tabFitness.UseVisualStyleBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox7.Controls.Add(this.flowFitnessFunctions);
			this.groupBox7.Location = new System.Drawing.Point(9, 9);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(213, 272);
			this.groupBox7.TabIndex = 8;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Fitness Function";
			// 
			// flowFitnessFunctions
			// 
			this.flowFitnessFunctions.AutoScroll = true;
			this.flowFitnessFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowFitnessFunctions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowFitnessFunctions.Location = new System.Drawing.Point(3, 16);
			this.flowFitnessFunctions.Name = "flowFitnessFunctions";
			this.flowFitnessFunctions.Size = new System.Drawing.Size(207, 253);
			this.flowFitnessFunctions.TabIndex = 8;
			// 
			// groupBox13
			// 
			this.groupBox13.Controls.Add(this.chkFitness_MultiObjective);
			this.groupBox13.Controls.Add(this.chkFitness_Parsimony);
			this.groupBox13.Location = new System.Drawing.Point(228, 9);
			this.groupBox13.Name = "groupBox13";
			this.groupBox13.Size = new System.Drawing.Size(153, 80);
			this.groupBox13.TabIndex = 2;
			this.groupBox13.TabStop = false;
			this.groupBox13.Text = "Program Size Reduction";
			// 
			// chkFitness_MultiObjective
			// 
			this.chkFitness_MultiObjective.AutoSize = true;
			this.chkFitness_MultiObjective.Location = new System.Drawing.Point(6, 26);
			this.chkFitness_MultiObjective.Name = "chkFitness_MultiObjective";
			this.chkFitness_MultiObjective.Size = new System.Drawing.Size(96, 17);
			this.chkFitness_MultiObjective.TabIndex = 11;
			this.chkFitness_MultiObjective.Text = "Multi Objective";
			this.chkFitness_MultiObjective.UseVisualStyleBackColor = true;
			this.chkFitness_MultiObjective.Click += new System.EventHandler(this.chkFitness_MultiObjective_Click);
			this.chkFitness_MultiObjective.CheckedChanged += new System.EventHandler(this.chkFitness_MultiObjective_CheckedChanged);
			// 
			// chkFitness_Parsimony
			// 
			this.chkFitness_Parsimony.AutoSize = true;
			this.chkFitness_Parsimony.Location = new System.Drawing.Point(6, 49);
			this.chkFitness_Parsimony.Name = "chkFitness_Parsimony";
			this.chkFitness_Parsimony.Size = new System.Drawing.Size(119, 17);
			this.chkFitness_Parsimony.TabIndex = 10;
			this.chkFitness_Parsimony.Text = "Adaptive Parsimony";
			this.chkFitness_Parsimony.UseVisualStyleBackColor = true;
			this.chkFitness_Parsimony.CheckedChanged += new System.EventHandler(this.chkFitness_Parsimony_CheckedChanged);
			// 
			// tabPopulationStructure
			// 
			this.tabPopulationStructure.Controls.Add(this.groupBox8);
			this.tabPopulationStructure.Controls.Add(this.groupBox16);
			this.tabPopulationStructure.Controls.Add(this.groupBox9);
			this.tabPopulationStructure.Location = new System.Drawing.Point(4, 22);
			this.tabPopulationStructure.Name = "tabPopulationStructure";
			this.tabPopulationStructure.Padding = new System.Windows.Forms.Padding(3);
			this.tabPopulationStructure.Size = new System.Drawing.Size(484, 297);
			this.tabPopulationStructure.TabIndex = 6;
			this.tabPopulationStructure.Text = "Population";
			this.tabPopulationStructure.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.groupBox12);
			this.groupBox8.Controls.Add(this.udDistributed_TransferCount);
			this.groupBox8.Controls.Add(this.label23);
			this.groupBox8.Location = new System.Drawing.Point(160, 115);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(237, 123);
			this.groupBox8.TabIndex = 17;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Distributed Options";
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.label24);
			this.groupBox12.Controls.Add(this.udDistributed_Topology_StarPercent);
			this.groupBox12.Controls.Add(this.rdDistributed_Topology_Star);
			this.groupBox12.Controls.Add(this.rdDistributed_Topology_Ring);
			this.groupBox12.Location = new System.Drawing.Point(12, 46);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(198, 70);
			this.groupBox12.TabIndex = 17;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "Topology";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(100, 44);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(15, 13);
			this.label24.TabIndex = 22;
			this.label24.Text = "%";
			// 
			// udDistributed_Topology_StarPercent
			// 
			this.udDistributed_Topology_StarPercent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.udDistributed_Topology_StarPercent.Location = new System.Drawing.Point(56, 40);
			this.udDistributed_Topology_StarPercent.Name = "udDistributed_Topology_StarPercent";
			this.udDistributed_Topology_StarPercent.Size = new System.Drawing.Size(41, 20);
			this.udDistributed_Topology_StarPercent.TabIndex = 21;
			this.udDistributed_Topology_StarPercent.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.udDistributed_Topology_StarPercent.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// rdDistributed_Topology_Star
			// 
			this.rdDistributed_Topology_Star.AutoSize = true;
			this.rdDistributed_Topology_Star.Location = new System.Drawing.Point(6, 42);
			this.rdDistributed_Topology_Star.Name = "rdDistributed_Topology_Star";
			this.rdDistributed_Topology_Star.Size = new System.Drawing.Size(44, 17);
			this.rdDistributed_Topology_Star.TabIndex = 1;
			this.rdDistributed_Topology_Star.TabStop = true;
			this.rdDistributed_Topology_Star.Text = "Star";
			this.rdDistributed_Topology_Star.UseVisualStyleBackColor = true;
			this.rdDistributed_Topology_Star.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// rdDistributed_Topology_Ring
			// 
			this.rdDistributed_Topology_Ring.AutoSize = true;
			this.rdDistributed_Topology_Ring.Location = new System.Drawing.Point(6, 19);
			this.rdDistributed_Topology_Ring.Name = "rdDistributed_Topology_Ring";
			this.rdDistributed_Topology_Ring.Size = new System.Drawing.Size(47, 17);
			this.rdDistributed_Topology_Ring.TabIndex = 0;
			this.rdDistributed_Topology_Ring.TabStop = true;
			this.rdDistributed_Topology_Ring.Text = "Ring";
			this.rdDistributed_Topology_Ring.UseVisualStyleBackColor = true;
			this.rdDistributed_Topology_Ring.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// udDistributed_TransferCount
			// 
			this.udDistributed_TransferCount.Location = new System.Drawing.Point(124, 20);
			this.udDistributed_TransferCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udDistributed_TransferCount.Name = "udDistributed_TransferCount";
			this.udDistributed_TransferCount.Size = new System.Drawing.Size(86, 20);
			this.udDistributed_TransferCount.TabIndex = 16;
			this.udDistributed_TransferCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.udDistributed_TransferCount.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(9, 22);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(109, 13);
			this.label23.TabIndex = 15;
			this.label23.Text = "Programs To Transfer";
			// 
			// groupBox16
			// 
			this.groupBox16.Controls.Add(this.label21);
			this.groupBox16.Controls.Add(this.label20);
			this.groupBox16.Controls.Add(this.label16);
			this.groupBox16.Controls.Add(this.udStructure_Crossover);
			this.groupBox16.Controls.Add(this.udStructure_Mutation);
			this.groupBox16.Controls.Add(this.udStructure_Reproduction);
			this.groupBox16.Controls.Add(this.label5);
			this.groupBox16.Controls.Add(this.label4);
			this.groupBox16.Controls.Add(this.label9);
			this.groupBox16.Location = new System.Drawing.Point(6, 115);
			this.groupBox16.Name = "groupBox16";
			this.groupBox16.Size = new System.Drawing.Size(148, 107);
			this.groupBox16.TabIndex = 16;
			this.groupBox16.TabStop = false;
			this.groupBox16.Text = "Operators";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(121, 75);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(15, 13);
			this.label21.TabIndex = 31;
			this.label21.Text = "%";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(121, 49);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(15, 13);
			this.label20.TabIndex = 30;
			this.label20.Text = "%";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(121, 22);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(15, 13);
			this.label16.TabIndex = 29;
			this.label16.Text = "%";
			// 
			// udStructure_Crossover
			// 
			this.udStructure_Crossover.Location = new System.Drawing.Point(73, 73);
			this.udStructure_Crossover.Name = "udStructure_Crossover";
			this.udStructure_Crossover.Size = new System.Drawing.Size(42, 20);
			this.udStructure_Crossover.TabIndex = 28;
			this.udStructure_Crossover.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
			this.udStructure_Crossover.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// udStructure_Mutation
			// 
			this.udStructure_Mutation.Location = new System.Drawing.Point(73, 46);
			this.udStructure_Mutation.Name = "udStructure_Mutation";
			this.udStructure_Mutation.Size = new System.Drawing.Size(42, 20);
			this.udStructure_Mutation.TabIndex = 27;
			this.udStructure_Mutation.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.udStructure_Mutation.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// udStructure_Reproduction
			// 
			this.udStructure_Reproduction.Location = new System.Drawing.Point(73, 19);
			this.udStructure_Reproduction.Name = "udStructure_Reproduction";
			this.udStructure_Reproduction.Size = new System.Drawing.Size(42, 20);
			this.udStructure_Reproduction.TabIndex = 26;
			this.udStructure_Reproduction.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.udStructure_Reproduction.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(1, 75);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(54, 13);
			this.label5.TabIndex = 25;
			this.label5.Text = "Crossover";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(1, 49);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 13);
			this.label4.TabIndex = 24;
			this.label4.Text = "Mutation";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(1, 22);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(71, 13);
			this.label9.TabIndex = 23;
			this.label9.Text = "Reproduction";
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.udPopulation_TreeDepth);
			this.groupBox9.Controls.Add(this.label12);
			this.groupBox9.Controls.Add(this.cmbStructure_PopReproduction);
			this.groupBox9.Controls.Add(this.label11);
			this.groupBox9.Controls.Add(this.cmbStructure_PopInit);
			this.groupBox9.Controls.Add(this.udStructure_PopSize);
			this.groupBox9.Controls.Add(this.label10);
			this.groupBox9.Controls.Add(this.label15);
			this.groupBox9.Location = new System.Drawing.Point(6, 6);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(391, 103);
			this.groupBox9.TabIndex = 15;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Parameters";
			// 
			// udPopulation_TreeDepth
			// 
			this.udPopulation_TreeDepth.Location = new System.Drawing.Point(303, 22);
			this.udPopulation_TreeDepth.Name = "udPopulation_TreeDepth";
			this.udPopulation_TreeDepth.Size = new System.Drawing.Size(41, 20);
			this.udPopulation_TreeDepth.TabIndex = 20;
			this.udPopulation_TreeDepth.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.udPopulation_TreeDepth.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(261, 25);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(36, 13);
			this.label12.TabIndex = 19;
			this.label12.Text = "Depth";
			// 
			// cmbStructure_PopReproduction
			// 
			this.cmbStructure_PopReproduction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cmbStructure_PopReproduction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbStructure_PopReproduction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStructure_PopReproduction.FormattingEnabled = true;
			this.cmbStructure_PopReproduction.Items.AddRange(new object[] {
            "Tournament",
            "Overselection"});
			this.cmbStructure_PopReproduction.Location = new System.Drawing.Point(78, 46);
			this.cmbStructure_PopReproduction.Name = "cmbStructure_PopReproduction";
			this.cmbStructure_PopReproduction.Size = new System.Drawing.Size(172, 21);
			this.cmbStructure_PopReproduction.TabIndex = 18;
			this.cmbStructure_PopReproduction.SelectedIndexChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(2, 49);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(71, 13);
			this.label11.TabIndex = 17;
			this.label11.Text = "Reproduction";
			// 
			// cmbStructure_PopInit
			// 
			this.cmbStructure_PopInit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cmbStructure_PopInit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbStructure_PopInit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStructure_PopInit.FormattingEnabled = true;
			this.cmbStructure_PopInit.Items.AddRange(new object[] {
            "Grow",
            "Full",
            "Ramped Half-Half"});
			this.cmbStructure_PopInit.Location = new System.Drawing.Point(78, 19);
			this.cmbStructure_PopInit.Name = "cmbStructure_PopInit";
			this.cmbStructure_PopInit.Size = new System.Drawing.Size(172, 21);
			this.cmbStructure_PopInit.TabIndex = 16;
			this.cmbStructure_PopInit.SelectedIndexChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// udStructure_PopSize
			// 
			this.udStructure_PopSize.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.udStructure_PopSize.Location = new System.Drawing.Point(78, 73);
			this.udStructure_PopSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.udStructure_PopSize.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.udStructure_PopSize.Name = "udStructure_PopSize";
			this.udStructure_PopSize.Size = new System.Drawing.Size(86, 20);
			this.udStructure_PopSize.TabIndex = 14;
			this.udStructure_PopSize.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.udStructure_PopSize.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			this.udStructure_PopSize.Validated += new System.EventHandler(this.udStructure_PopSize_Validated);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(2, 75);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(27, 13);
			this.label10.TabIndex = 8;
			this.label10.Text = "Size";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(2, 22);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(61, 13);
			this.label15.TabIndex = 7;
			this.label15.Text = "Initialization";
			// 
			// tabGenerationOptions
			// 
			this.tabGenerationOptions.Controls.Add(this.groupBox10);
			this.tabGenerationOptions.Location = new System.Drawing.Point(4, 22);
			this.tabGenerationOptions.Name = "tabGenerationOptions";
			this.tabGenerationOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tabGenerationOptions.Size = new System.Drawing.Size(484, 297);
			this.tabGenerationOptions.TabIndex = 1;
			this.tabGenerationOptions.Text = "Generations";
			this.tabGenerationOptions.UseVisualStyleBackColor = true;
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.udGens_MaxNumber);
			this.groupBox10.Controls.Add(this.chkGens_HitsMaxed);
			this.groupBox10.Controls.Add(this.chkGens_RawFitness0);
			this.groupBox10.Controls.Add(this.chkGens_MaxNumber);
			this.groupBox10.Location = new System.Drawing.Point(8, 6);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(200, 90);
			this.groupBox10.TabIndex = 1;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "Termination Criteria";
			// 
			// udGens_MaxNumber
			// 
			this.udGens_MaxNumber.Location = new System.Drawing.Point(133, 18);
			this.udGens_MaxNumber.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.udGens_MaxNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udGens_MaxNumber.Name = "udGens_MaxNumber";
			this.udGens_MaxNumber.Size = new System.Drawing.Size(56, 20);
			this.udGens_MaxNumber.TabIndex = 4;
			this.udGens_MaxNumber.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.udGens_MaxNumber.ValueChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// chkGens_HitsMaxed
			// 
			this.chkGens_HitsMaxed.AutoSize = true;
			this.chkGens_HitsMaxed.Checked = true;
			this.chkGens_HitsMaxed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGens_HitsMaxed.Location = new System.Drawing.Point(6, 65);
			this.chkGens_HitsMaxed.Name = "chkGens_HitsMaxed";
			this.chkGens_HitsMaxed.Size = new System.Drawing.Size(96, 17);
			this.chkGens_HitsMaxed.TabIndex = 2;
			this.chkGens_HitsMaxed.Text = "Hits Maximized";
			this.chkGens_HitsMaxed.UseVisualStyleBackColor = true;
			this.chkGens_HitsMaxed.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// chkGens_RawFitness0
			// 
			this.chkGens_RawFitness0.AutoSize = true;
			this.chkGens_RawFitness0.Checked = true;
			this.chkGens_RawFitness0.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGens_RawFitness0.Location = new System.Drawing.Point(6, 42);
			this.chkGens_RawFitness0.Name = "chkGens_RawFitness0";
			this.chkGens_RawFitness0.Size = new System.Drawing.Size(103, 17);
			this.chkGens_RawFitness0.TabIndex = 1;
			this.chkGens_RawFitness0.Text = "Raw Fitness is 0";
			this.chkGens_RawFitness0.UseVisualStyleBackColor = true;
			this.chkGens_RawFitness0.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// chkGens_MaxNumber
			// 
			this.chkGens_MaxNumber.AutoSize = true;
			this.chkGens_MaxNumber.Checked = true;
			this.chkGens_MaxNumber.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkGens_MaxNumber.Location = new System.Drawing.Point(6, 19);
			this.chkGens_MaxNumber.Name = "chkGens_MaxNumber";
			this.chkGens_MaxNumber.Size = new System.Drawing.Size(106, 17);
			this.chkGens_MaxNumber.TabIndex = 0;
			this.chkGens_MaxNumber.Text = "Max Generations";
			this.chkGens_MaxNumber.UseVisualStyleBackColor = true;
			this.chkGens_MaxNumber.CheckedChanged += new System.EventHandler(this.Event_ProfileDirty);
			// 
			// lvProfiles
			// 
			this.lvProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lvProfiles.BackColor = System.Drawing.SystemColors.Window;
			this.lvProfiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvProfiles.FullRowSelect = true;
			listViewGroup1.Header = "Profiles";
			listViewGroup1.Name = "listViewGroup1";
			this.lvProfiles.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			this.lvProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvProfiles.HideSelection = false;
			this.lvProfiles.LabelEdit = true;
			this.lvProfiles.Location = new System.Drawing.Point(3, 16);
			this.lvProfiles.MultiSelect = false;
			this.lvProfiles.Name = "lvProfiles";
			this.lvProfiles.ShowGroups = false;
			this.lvProfiles.Size = new System.Drawing.Size(210, 280);
			this.lvProfiles.SmallImageList = this.ilProfiles;
			this.lvProfiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvProfiles.TabIndex = 1;
			this.lvProfiles.UseCompatibleStateImageBehavior = false;
			this.lvProfiles.View = System.Windows.Forms.View.Details;
			this.lvProfiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvProfiles_ItemSelectionChanged);
			this.lvProfiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvProfiles_AfterLabelEdit);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 200;
			// 
			// ilProfiles
			// 
			this.ilProfiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilProfiles.ImageStream")));
			this.ilProfiles.TransparentColor = System.Drawing.Color.Transparent;
			this.ilProfiles.Images.SetKeyName(0, "idr_dll.ico");
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.btnDataValidationDelete);
			this.groupBox1.Controls.Add(this.btnDataValidationAdd);
			this.groupBox1.Controls.Add(this.cbDataValidation);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btnDataTraining);
			this.groupBox1.Controls.Add(this.txtDataTraining);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(8, 378);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(435, 81);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data Sources";
			// 
			// btnDataValidationDelete
			// 
			this.btnDataValidationDelete.Enabled = false;
			this.btnDataValidationDelete.Location = new System.Drawing.Point(369, 49);
			this.btnDataValidationDelete.Name = "btnDataValidationDelete";
			this.btnDataValidationDelete.Size = new System.Drawing.Size(54, 23);
			this.btnDataValidationDelete.TabIndex = 8;
			this.btnDataValidationDelete.Text = "Delete";
			this.btnDataValidationDelete.UseVisualStyleBackColor = true;
			this.btnDataValidationDelete.Click += new System.EventHandler(this.btnDataValidationDelete_Click);
			// 
			// btnDataValidationAdd
			// 
			this.btnDataValidationAdd.Enabled = false;
			this.btnDataValidationAdd.Location = new System.Drawing.Point(311, 49);
			this.btnDataValidationAdd.Name = "btnDataValidationAdd";
			this.btnDataValidationAdd.Size = new System.Drawing.Size(54, 23);
			this.btnDataValidationAdd.TabIndex = 7;
			this.btnDataValidationAdd.Text = "Add";
			this.btnDataValidationAdd.UseVisualStyleBackColor = true;
			this.btnDataValidationAdd.Click += new System.EventHandler(this.btnDataValidationAdd_Click);
			// 
			// cbDataValidation
			// 
			this.cbDataValidation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataValidation.FormattingEnabled = true;
			this.cbDataValidation.Location = new System.Drawing.Point(74, 51);
			this.cbDataValidation.Name = "cbDataValidation";
			this.cbDataValidation.Size = new System.Drawing.Size(231, 21);
			this.cbDataValidation.TabIndex = 6;
			this.cbDataValidation.SelectedIndexChanged += new System.EventHandler(this.cbValidation_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Validation";
			// 
			// btnDataTraining
			// 
			this.btnDataTraining.Location = new System.Drawing.Point(311, 18);
			this.btnDataTraining.Name = "btnDataTraining";
			this.btnDataTraining.Size = new System.Drawing.Size(54, 23);
			this.btnDataTraining.TabIndex = 4;
			this.btnDataTraining.Text = "Select";
			this.btnDataTraining.UseVisualStyleBackColor = true;
			this.btnDataTraining.Click += new System.EventHandler(this.button3_Click);
			// 
			// txtDataTraining
			// 
			this.txtDataTraining.Location = new System.Drawing.Point(74, 20);
			this.txtDataTraining.Name = "txtDataTraining";
			this.txtDataTraining.ReadOnly = true;
			this.txtDataTraining.Size = new System.Drawing.Size(231, 20);
			this.txtDataTraining.TabIndex = 2;
			this.txtDataTraining.TextChanged += new System.EventHandler(this.txtDataTraining_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Training";
			// 
			// pageModeling
			// 
			this.pageModeling.Controls.Add(this.panel1);
			this.pageModeling.Controls.Add(this.statusModeling);
			this.pageModeling.Controls.Add(this.tabModeling);
			this.pageModeling.Controls.Add(this.groupBox14);
			this.pageModeling.Controls.Add(this.pnlModelingToolbar);
			this.pageModeling.Location = new System.Drawing.Point(4, 22);
			this.pageModeling.Name = "pageModeling";
			this.pageModeling.Size = new System.Drawing.Size(734, 465);
			this.pageModeling.TabIndex = 3;
			this.pageModeling.Text = "Modeling";
			this.pageModeling.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label22);
			this.panel1.Controls.Add(this.cmbPopulationFitnessServers);
			this.panel1.Location = new System.Drawing.Point(219, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(515, 30);
			this.panel1.TabIndex = 11;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(8, 9);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(38, 13);
			this.label22.TabIndex = 8;
			this.label22.Text = "Server";
			// 
			// cmbPopulationFitnessServers
			// 
			this.cmbPopulationFitnessServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPopulationFitnessServers.FormattingEnabled = true;
			this.cmbPopulationFitnessServers.Location = new System.Drawing.Point(52, 6);
			this.cmbPopulationFitnessServers.Name = "cmbPopulationFitnessServers";
			this.cmbPopulationFitnessServers.Size = new System.Drawing.Size(281, 21);
			this.cmbPopulationFitnessServers.TabIndex = 7;
			this.cmbPopulationFitnessServers.SelectedIndexChanged += new System.EventHandler(this.cmbPopulationFitnessServers_SelectedIndexChanged);
			// 
			// statusModeling
			// 
			this.statusModeling.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this.statusModeling.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus,
            this.pgStatus});
			this.statusModeling.Location = new System.Drawing.Point(0, 443);
			this.statusModeling.Name = "statusModeling";
			this.statusModeling.Size = new System.Drawing.Size(734, 22);
			this.statusModeling.TabIndex = 10;
			this.statusModeling.Text = "statusStrip1";
			// 
			// lbStatus
			// 
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.Size = new System.Drawing.Size(0, 17);
			this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pgStatus
			// 
			this.pgStatus.Enabled = false;
			this.pgStatus.Name = "pgStatus";
			this.pgStatus.Size = new System.Drawing.Size(100, 16);
			this.pgStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.pgStatus.Visible = false;
			// 
			// tabModeling
			// 
			this.tabModeling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabModeling.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabModeling.Controls.Add(this.pageModelFitness);
			this.tabModeling.Controls.Add(this.pagePopulationFitness);
			this.tabModeling.Controls.Add(this.pagePopulationComplexity);
			this.tabModeling.Location = new System.Drawing.Point(219, 30);
			this.tabModeling.Multiline = true;
			this.tabModeling.Name = "tabModeling";
			this.tabModeling.SelectedIndex = 0;
			this.tabModeling.Size = new System.Drawing.Size(515, 410);
			this.tabModeling.TabIndex = 9;
			this.tabModeling.SelectedIndexChanged += new System.EventHandler(this.tabModeling_SelectedIndexChanged);
			// 
			// pageModelFitness
			// 
			this.pageModelFitness.Controls.Add(this.graphFitness);
			this.pageModelFitness.Location = new System.Drawing.Point(4, 25);
			this.pageModelFitness.Name = "pageModelFitness";
			this.pageModelFitness.Size = new System.Drawing.Size(507, 381);
			this.pageModelFitness.TabIndex = 0;
			this.pageModelFitness.Text = "Fitness";
			this.pageModelFitness.UseVisualStyleBackColor = true;
			// 
			// graphFitness
			// 
			this.graphFitness.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphFitness.EditButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphFitness.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphFitness.IsAutoScrollRange = false;
			this.graphFitness.IsEnableHEdit = false;
			this.graphFitness.IsEnableHPan = true;
			this.graphFitness.IsEnableHZoom = true;
			this.graphFitness.IsEnableVEdit = false;
			this.graphFitness.IsEnableVPan = true;
			this.graphFitness.IsEnableVZoom = true;
			this.graphFitness.IsPrintFillPage = true;
			this.graphFitness.IsPrintKeepAspectRatio = true;
			this.graphFitness.IsScrollY2 = false;
			this.graphFitness.IsShowContextMenu = true;
			this.graphFitness.IsShowCopyMessage = true;
			this.graphFitness.IsShowCursorValues = false;
			this.graphFitness.IsShowHScrollBar = false;
			this.graphFitness.IsShowPointValues = true;
			this.graphFitness.IsShowVScrollBar = false;
			this.graphFitness.IsSynchronizeXAxes = false;
			this.graphFitness.IsSynchronizeYAxes = false;
			this.graphFitness.IsZoomOnMouseCenter = false;
			this.graphFitness.LinkButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphFitness.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphFitness.Location = new System.Drawing.Point(0, 0);
			this.graphFitness.Name = "graphFitness";
			this.graphFitness.PanButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphFitness.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
			this.graphFitness.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
			this.graphFitness.PanModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphFitness.PointDateFormat = "g";
			this.graphFitness.PointValueFormat = "G";
			this.graphFitness.ScrollMaxX = 0;
			this.graphFitness.ScrollMaxY = 0;
			this.graphFitness.ScrollMaxY2 = 0;
			this.graphFitness.ScrollMinX = 0;
			this.graphFitness.ScrollMinY = 0;
			this.graphFitness.ScrollMinY2 = 0;
			this.graphFitness.Size = new System.Drawing.Size(507, 381);
			this.graphFitness.TabIndex = 3;
			this.graphFitness.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphFitness.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
			this.graphFitness.ZoomModifierKeys = System.Windows.Forms.Keys.None;
			this.graphFitness.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphFitness.ZoomStepFraction = 0.1;
			// 
			// pagePopulationFitness
			// 
			this.pagePopulationFitness.Controls.Add(this.graphPopulationFitness);
			this.pagePopulationFitness.Location = new System.Drawing.Point(4, 25);
			this.pagePopulationFitness.Name = "pagePopulationFitness";
			this.pagePopulationFitness.Size = new System.Drawing.Size(507, 381);
			this.pagePopulationFitness.TabIndex = 2;
			this.pagePopulationFitness.Text = "Population Fitness";
			this.pagePopulationFitness.UseVisualStyleBackColor = true;
			// 
			// graphPopulationFitness
			// 
			this.graphPopulationFitness.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphPopulationFitness.EditButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationFitness.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphPopulationFitness.IsAutoScrollRange = false;
			this.graphPopulationFitness.IsEnableHEdit = false;
			this.graphPopulationFitness.IsEnableHPan = true;
			this.graphPopulationFitness.IsEnableHZoom = true;
			this.graphPopulationFitness.IsEnableVEdit = false;
			this.graphPopulationFitness.IsEnableVPan = true;
			this.graphPopulationFitness.IsEnableVZoom = true;
			this.graphPopulationFitness.IsPrintFillPage = true;
			this.graphPopulationFitness.IsPrintKeepAspectRatio = true;
			this.graphPopulationFitness.IsScrollY2 = false;
			this.graphPopulationFitness.IsShowContextMenu = true;
			this.graphPopulationFitness.IsShowCopyMessage = true;
			this.graphPopulationFitness.IsShowCursorValues = false;
			this.graphPopulationFitness.IsShowHScrollBar = false;
			this.graphPopulationFitness.IsShowPointValues = true;
			this.graphPopulationFitness.IsShowVScrollBar = false;
			this.graphPopulationFitness.IsSynchronizeXAxes = false;
			this.graphPopulationFitness.IsSynchronizeYAxes = false;
			this.graphPopulationFitness.IsZoomOnMouseCenter = false;
			this.graphPopulationFitness.LinkButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationFitness.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphPopulationFitness.Location = new System.Drawing.Point(0, 0);
			this.graphPopulationFitness.Name = "graphPopulationFitness";
			this.graphPopulationFitness.PanButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationFitness.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
			this.graphPopulationFitness.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
			this.graphPopulationFitness.PanModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphPopulationFitness.PointDateFormat = "g";
			this.graphPopulationFitness.PointValueFormat = "G";
			this.graphPopulationFitness.ScrollMaxX = 0;
			this.graphPopulationFitness.ScrollMaxY = 0;
			this.graphPopulationFitness.ScrollMaxY2 = 0;
			this.graphPopulationFitness.ScrollMinX = 0;
			this.graphPopulationFitness.ScrollMinY = 0;
			this.graphPopulationFitness.ScrollMinY2 = 0;
			this.graphPopulationFitness.Size = new System.Drawing.Size(507, 381);
			this.graphPopulationFitness.TabIndex = 5;
			this.graphPopulationFitness.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationFitness.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
			this.graphPopulationFitness.ZoomModifierKeys = System.Windows.Forms.Keys.None;
			this.graphPopulationFitness.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphPopulationFitness.ZoomStepFraction = 0.1;
			// 
			// pagePopulationComplexity
			// 
			this.pagePopulationComplexity.Controls.Add(this.graphPopulationComplexity);
			this.pagePopulationComplexity.Location = new System.Drawing.Point(4, 25);
			this.pagePopulationComplexity.Name = "pagePopulationComplexity";
			this.pagePopulationComplexity.Size = new System.Drawing.Size(507, 381);
			this.pagePopulationComplexity.TabIndex = 1;
			this.pagePopulationComplexity.Text = "Population Complexity";
			this.pagePopulationComplexity.UseVisualStyleBackColor = true;
			// 
			// graphPopulationComplexity
			// 
			this.graphPopulationComplexity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphPopulationComplexity.EditButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationComplexity.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphPopulationComplexity.IsAutoScrollRange = false;
			this.graphPopulationComplexity.IsEnableHEdit = false;
			this.graphPopulationComplexity.IsEnableHPan = true;
			this.graphPopulationComplexity.IsEnableHZoom = true;
			this.graphPopulationComplexity.IsEnableVEdit = false;
			this.graphPopulationComplexity.IsEnableVPan = true;
			this.graphPopulationComplexity.IsEnableVZoom = true;
			this.graphPopulationComplexity.IsPrintFillPage = true;
			this.graphPopulationComplexity.IsPrintKeepAspectRatio = true;
			this.graphPopulationComplexity.IsScrollY2 = false;
			this.graphPopulationComplexity.IsShowContextMenu = true;
			this.graphPopulationComplexity.IsShowCopyMessage = true;
			this.graphPopulationComplexity.IsShowCursorValues = false;
			this.graphPopulationComplexity.IsShowHScrollBar = false;
			this.graphPopulationComplexity.IsShowPointValues = true;
			this.graphPopulationComplexity.IsShowVScrollBar = false;
			this.graphPopulationComplexity.IsSynchronizeXAxes = false;
			this.graphPopulationComplexity.IsSynchronizeYAxes = false;
			this.graphPopulationComplexity.IsZoomOnMouseCenter = false;
			this.graphPopulationComplexity.LinkButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationComplexity.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphPopulationComplexity.Location = new System.Drawing.Point(0, 0);
			this.graphPopulationComplexity.Name = "graphPopulationComplexity";
			this.graphPopulationComplexity.PanButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationComplexity.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
			this.graphPopulationComplexity.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
			this.graphPopulationComplexity.PanModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphPopulationComplexity.PointDateFormat = "g";
			this.graphPopulationComplexity.PointValueFormat = "G";
			this.graphPopulationComplexity.ScrollMaxX = 0;
			this.graphPopulationComplexity.ScrollMaxY = 0;
			this.graphPopulationComplexity.ScrollMaxY2 = 0;
			this.graphPopulationComplexity.ScrollMinX = 0;
			this.graphPopulationComplexity.ScrollMinY = 0;
			this.graphPopulationComplexity.ScrollMinY2 = 0;
			this.graphPopulationComplexity.Size = new System.Drawing.Size(507, 381);
			this.graphPopulationComplexity.TabIndex = 4;
			this.graphPopulationComplexity.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphPopulationComplexity.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
			this.graphPopulationComplexity.ZoomModifierKeys = System.Windows.Forms.Keys.None;
			this.graphPopulationComplexity.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphPopulationComplexity.ZoomStepFraction = 0.1;
			// 
			// groupBox14
			// 
			this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox14.Controls.Add(this.lvProfilesModeling);
			this.groupBox14.Location = new System.Drawing.Point(1, 3);
			this.groupBox14.Name = "groupBox14";
			this.groupBox14.Size = new System.Drawing.Size(215, 397);
			this.groupBox14.TabIndex = 8;
			this.groupBox14.TabStop = false;
			this.groupBox14.Text = "Modeling Profiles";
			// 
			// lvProfilesModeling
			// 
			this.lvProfilesModeling.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvProfilesModeling.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
			this.lvProfilesModeling.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvProfilesModeling.FullRowSelect = true;
			this.lvProfilesModeling.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvProfilesModeling.HideSelection = false;
			this.lvProfilesModeling.Location = new System.Drawing.Point(3, 16);
			this.lvProfilesModeling.MultiSelect = false;
			this.lvProfilesModeling.Name = "lvProfilesModeling";
			this.lvProfilesModeling.ShowGroups = false;
			this.lvProfilesModeling.ShowItemToolTips = true;
			this.lvProfilesModeling.Size = new System.Drawing.Size(209, 378);
			this.lvProfilesModeling.SmallImageList = this.ilProfiles;
			this.lvProfilesModeling.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvProfilesModeling.TabIndex = 6;
			this.lvProfilesModeling.UseCompatibleStateImageBehavior = false;
			this.lvProfilesModeling.View = System.Windows.Forms.View.Details;
			this.lvProfilesModeling.SelectedIndexChanged += new System.EventHandler(this.lvProfilesModeling_SelectedIndexChanged);
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Name";
			this.columnHeader3.Width = 200;
			// 
			// pnlModelingToolbar
			// 
			this.pnlModelingToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlModelingToolbar.Controls.Add(this.tsModeling);
			this.pnlModelingToolbar.Location = new System.Drawing.Point(1, 403);
			this.pnlModelingToolbar.Name = "pnlModelingToolbar";
			this.pnlModelingToolbar.Size = new System.Drawing.Size(216, 37);
			this.pnlModelingToolbar.TabIndex = 7;
			// 
			// tsModeling
			// 
			this.tsModeling.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsModeling.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnModel,
            this.btnCancel,
            this.pgModeling});
			this.tsModeling.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.tsModeling.Location = new System.Drawing.Point(0, 0);
			this.tsModeling.Name = "tsModeling";
			this.tsModeling.Size = new System.Drawing.Size(216, 36);
			this.tsModeling.TabIndex = 6;
			this.tsModeling.Text = "toolStrip2";
			// 
			// btnModel
			// 
			this.btnModel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsModelInteractive,
            this.tsModelBatch});
			this.btnModel.Image = global::GPStudio.Client.Properties.Resources.FormRun;
			this.btnModel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnModel.Name = "btnModel";
			this.btnModel.Size = new System.Drawing.Size(48, 33);
			this.btnModel.Text = "Model";
			this.btnModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsModelInteractive
			// 
			this.tsModelInteractive.Name = "tsModelInteractive";
			this.tsModelInteractive.Size = new System.Drawing.Size(138, 22);
			this.tsModelInteractive.Text = "Interactive";
			this.tsModelInteractive.Click += new System.EventHandler(this.btnModel_Click);
			// 
			// tsModelBatch
			// 
			this.tsModelBatch.Name = "tsModelBatch";
			this.tsModelBatch.Size = new System.Drawing.Size(138, 22);
			this.tsModelBatch.Text = "Batch";
			this.tsModelBatch.Click += new System.EventHandler(this.tsModelBatch_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Enabled = false;
			this.btnCancel.Image = global::GPStudio.Client.Properties.Resources.error;
			this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(43, 33);
			this.btnCancel.Text = "Cancel";
			this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pgModeling
			// 
			this.pgModeling.Minimum = 1;
			this.pgModeling.Name = "pgModeling";
			this.pgModeling.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.pgModeling.Size = new System.Drawing.Size(100, 33);
			this.pgModeling.Step = 1;
			this.pgModeling.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pgModeling.Value = 1;
			// 
			// pageResults
			// 
			this.pageResults.Controls.Add(this.splitContainer3);
			this.pageResults.Location = new System.Drawing.Point(4, 22);
			this.pageResults.Name = "pageResults";
			this.pageResults.Size = new System.Drawing.Size(734, 465);
			this.pageResults.TabIndex = 5;
			this.pageResults.Text = "Results";
			this.pageResults.UseVisualStyleBackColor = true;
			// 
			// splitContainer3
			// 
			this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.splitResults);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.tabResults);
			this.splitContainer3.Panel2.Controls.Add(this.statusResults);
			this.splitContainer3.Size = new System.Drawing.Size(734, 465);
			this.splitContainer3.SplitterDistance = 177;
			this.splitContainer3.TabIndex = 2;
			// 
			// splitResults
			// 
			this.splitResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitResults.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitResults.Location = new System.Drawing.Point(0, 0);
			this.splitResults.Name = "splitResults";
			// 
			// splitResults.Panel1
			// 
			this.splitResults.Panel1.Controls.Add(this.lvProfilesResults);
			this.splitResults.Panel1.Controls.Add(this.groupBox5);
			// 
			// splitResults.Panel2
			// 
			this.splitResults.Panel2.Controls.Add(this.lvResultsPrograms);
			this.splitResults.Size = new System.Drawing.Size(732, 175);
			this.splitResults.SplitterDistance = 202;
			this.splitResults.TabIndex = 7;
			// 
			// lvProfilesResults
			// 
			this.lvProfilesResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvProfilesResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6});
			this.lvProfilesResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvProfilesResults.FullRowSelect = true;
			this.lvProfilesResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvProfilesResults.HideSelection = false;
			this.lvProfilesResults.Location = new System.Drawing.Point(0, 0);
			this.lvProfilesResults.MultiSelect = false;
			this.lvProfilesResults.Name = "lvProfilesResults";
			this.lvProfilesResults.ShowGroups = false;
			this.lvProfilesResults.ShowItemToolTips = true;
			this.lvProfilesResults.Size = new System.Drawing.Size(202, 136);
			this.lvProfilesResults.SmallImageList = this.ilProfiles;
			this.lvProfilesResults.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvProfilesResults.TabIndex = 8;
			this.lvProfilesResults.UseCompatibleStateImageBehavior = false;
			this.lvProfilesResults.View = System.Windows.Forms.View.Details;
			this.lvProfilesResults.SelectedIndexChanged += new System.EventHandler(this.lvProfilesResults_SelectedIndexChanged);
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Name";
			this.columnHeader6.Width = 175;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.cbResultsDataSet);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox5.Location = new System.Drawing.Point(0, 136);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(202, 39);
			this.groupBox5.TabIndex = 7;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Data Set";
			// 
			// cbResultsDataSet
			// 
			this.cbResultsDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbResultsDataSet.FormattingEnabled = true;
			this.cbResultsDataSet.Location = new System.Drawing.Point(7, 12);
			this.cbResultsDataSet.Name = "cbResultsDataSet";
			this.cbResultsDataSet.Size = new System.Drawing.Size(189, 21);
			this.cbResultsDataSet.TabIndex = 7;
			this.cbResultsDataSet.SelectedIndexChanged += new System.EventHandler(this.cbResultsDataSet_SelectedIndexChanged);
			// 
			// lvResultsPrograms
			// 
			this.lvResultsPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDateTime,
            this.colFitness,
            this.colHits,
            this.colComplexity});
			this.lvResultsPrograms.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResultsPrograms.FullRowSelect = true;
			this.lvResultsPrograms.GridLines = true;
			this.lvResultsPrograms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvResultsPrograms.HideSelection = false;
			this.lvResultsPrograms.Location = new System.Drawing.Point(0, 0);
			this.lvResultsPrograms.MultiSelect = false;
			this.lvResultsPrograms.Name = "lvResultsPrograms";
			this.lvResultsPrograms.Size = new System.Drawing.Size(526, 175);
			this.lvResultsPrograms.TabIndex = 7;
			this.lvResultsPrograms.UseCompatibleStateImageBehavior = false;
			this.lvResultsPrograms.View = System.Windows.Forms.View.Details;
			this.lvResultsPrograms.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvResultsPrograms_ItemSelectionChanged);
			// 
			// colDateTime
			// 
			this.colDateTime.Text = "Date/Time Created";
			this.colDateTime.Width = 130;
			// 
			// colFitness
			// 
			this.colFitness.Text = "Fitness Error";
			this.colFitness.Width = 90;
			// 
			// colHits
			// 
			this.colHits.Text = "Hits";
			// 
			// colComplexity
			// 
			this.colComplexity.Text = "Complexity";
			this.colComplexity.Width = 90;
			// 
			// tabResults
			// 
			this.tabResults.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabResults.Controls.Add(this.pageResultsGraphical);
			this.tabResults.Controls.Add(this.pageResultsTabular);
			this.tabResults.Controls.Add(this.pageProgram);
			this.tabResults.Controls.Add(this.pageModelingHistory);
			this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabResults.Location = new System.Drawing.Point(0, 0);
			this.tabResults.Multiline = true;
			this.tabResults.Name = "tabResults";
			this.tabResults.SelectedIndex = 0;
			this.tabResults.Size = new System.Drawing.Size(732, 282);
			this.tabResults.TabIndex = 4;
			// 
			// pageResultsGraphical
			// 
			this.pageResultsGraphical.Controls.Add(this.graphResults);
			this.pageResultsGraphical.Controls.Add(this.panel3);
			this.pageResultsGraphical.Location = new System.Drawing.Point(4, 25);
			this.pageResultsGraphical.Name = "pageResultsGraphical";
			this.pageResultsGraphical.Padding = new System.Windows.Forms.Padding(3);
			this.pageResultsGraphical.Size = new System.Drawing.Size(724, 253);
			this.pageResultsGraphical.TabIndex = 0;
			this.pageResultsGraphical.Text = "Graphical";
			this.pageResultsGraphical.UseVisualStyleBackColor = true;
			// 
			// graphResults
			// 
			this.graphResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphResults.EditButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphResults.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphResults.IsAutoScrollRange = false;
			this.graphResults.IsEnableHEdit = false;
			this.graphResults.IsEnableHPan = true;
			this.graphResults.IsEnableHZoom = true;
			this.graphResults.IsEnableVEdit = false;
			this.graphResults.IsEnableVPan = true;
			this.graphResults.IsEnableVZoom = true;
			this.graphResults.IsPrintFillPage = true;
			this.graphResults.IsPrintKeepAspectRatio = true;
			this.graphResults.IsScrollY2 = false;
			this.graphResults.IsShowContextMenu = true;
			this.graphResults.IsShowCopyMessage = true;
			this.graphResults.IsShowCursorValues = false;
			this.graphResults.IsShowHScrollBar = false;
			this.graphResults.IsShowPointValues = true;
			this.graphResults.IsShowVScrollBar = false;
			this.graphResults.IsSynchronizeXAxes = false;
			this.graphResults.IsSynchronizeYAxes = false;
			this.graphResults.IsZoomOnMouseCenter = false;
			this.graphResults.LinkButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphResults.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
			this.graphResults.Location = new System.Drawing.Point(3, 3);
			this.graphResults.Name = "graphResults";
			this.graphResults.PanButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphResults.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
			this.graphResults.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
			this.graphResults.PanModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphResults.PointDateFormat = "g";
			this.graphResults.PointValueFormat = "G";
			this.graphResults.ScrollMaxX = 0;
			this.graphResults.ScrollMaxY = 0;
			this.graphResults.ScrollMaxY2 = 0;
			this.graphResults.ScrollMinX = 0;
			this.graphResults.ScrollMinY = 0;
			this.graphResults.ScrollMinY2 = 0;
			this.graphResults.Size = new System.Drawing.Size(718, 200);
			this.graphResults.TabIndex = 1;
			this.graphResults.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
			this.graphResults.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
			this.graphResults.ZoomModifierKeys = System.Windows.Forms.Keys.None;
			this.graphResults.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
			this.graphResults.ZoomStepFraction = 0.1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.groupBox6);
			this.panel3.Controls.Add(this.groupBox4);
			this.panel3.Controls.Add(this.groupBox3);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(3, 203);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(718, 47);
			this.panel3.TabIndex = 0;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.chkModelOnly);
			this.groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox6.Location = new System.Drawing.Point(396, 0);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(103, 47);
			this.groupBox6.TabIndex = 4;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "View";
			// 
			// chkModelOnly
			// 
			this.chkModelOnly.AutoSize = true;
			this.chkModelOnly.Location = new System.Drawing.Point(6, 20);
			this.chkModelOnly.Name = "chkModelOnly";
			this.chkModelOnly.Size = new System.Drawing.Size(79, 17);
			this.chkModelOnly.TabIndex = 1;
			this.chkModelOnly.Text = "Model Only";
			this.chkModelOnly.UseVisualStyleBackColor = true;
			this.chkModelOnly.CheckedChanged += new System.EventHandler(this.rdChartOption_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.chkCount);
			this.groupBox4.Controls.Add(this.cbXAxis);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox4.Location = new System.Drawing.Point(165, 0);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(231, 47);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "X-Axis";
			// 
			// chkCount
			// 
			this.chkCount.AutoSize = true;
			this.chkCount.Location = new System.Drawing.Point(6, 20);
			this.chkCount.Name = "chkCount";
			this.chkCount.Size = new System.Drawing.Size(54, 17);
			this.chkCount.TabIndex = 1;
			this.chkCount.Text = "Count";
			this.chkCount.UseVisualStyleBackColor = true;
			this.chkCount.Click += new System.EventHandler(this.rdChartOption_Click);
			// 
			// cbXAxis
			// 
			this.cbXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbXAxis.FormattingEnabled = true;
			this.cbXAxis.Location = new System.Drawing.Point(66, 18);
			this.cbXAxis.Name = "cbXAxis";
			this.cbXAxis.Size = new System.Drawing.Size(157, 21);
			this.cbXAxis.TabIndex = 0;
			this.cbXAxis.SelectedIndexChanged += new System.EventHandler(this.rdChartOption_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rdChartTypeScatter);
			this.groupBox3.Controls.Add(this.rdChartTypeBar);
			this.groupBox3.Controls.Add(this.rdChartTypeXY);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(165, 47);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Chart Type";
			// 
			// rdChartTypeScatter
			// 
			this.rdChartTypeScatter.AutoSize = true;
			this.rdChartTypeScatter.Location = new System.Drawing.Point(57, 19);
			this.rdChartTypeScatter.Name = "rdChartTypeScatter";
			this.rdChartTypeScatter.Size = new System.Drawing.Size(59, 17);
			this.rdChartTypeScatter.TabIndex = 3;
			this.rdChartTypeScatter.TabStop = true;
			this.rdChartTypeScatter.Text = "Scatter";
			this.rdChartTypeScatter.UseVisualStyleBackColor = true;
			this.rdChartTypeScatter.Click += new System.EventHandler(this.rdChartOption_Click);
			// 
			// rdChartTypeBar
			// 
			this.rdChartTypeBar.AutoSize = true;
			this.rdChartTypeBar.Location = new System.Drawing.Point(122, 19);
			this.rdChartTypeBar.Name = "rdChartTypeBar";
			this.rdChartTypeBar.Size = new System.Drawing.Size(41, 17);
			this.rdChartTypeBar.TabIndex = 1;
			this.rdChartTypeBar.Text = "Bar";
			this.rdChartTypeBar.UseVisualStyleBackColor = true;
			this.rdChartTypeBar.Click += new System.EventHandler(this.rdChartOption_Click);
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
			this.rdChartTypeXY.Click += new System.EventHandler(this.rdChartOption_Click);
			// 
			// pageResultsTabular
			// 
			this.pageResultsTabular.Controls.Add(this.dgResults);
			this.pageResultsTabular.Controls.Add(this.sbResultsTabular);
			this.pageResultsTabular.Location = new System.Drawing.Point(4, 25);
			this.pageResultsTabular.Name = "pageResultsTabular";
			this.pageResultsTabular.Size = new System.Drawing.Size(724, 253);
			this.pageResultsTabular.TabIndex = 1;
			this.pageResultsTabular.Text = "Tabular";
			this.pageResultsTabular.UseVisualStyleBackColor = true;
			// 
			// dgResults
			// 
			this.dgResults.AllowUserToAddRows = false;
			this.dgResults.AllowUserToDeleteRows = false;
			this.dgResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dgResults.Location = new System.Drawing.Point(0, 0);
			this.dgResults.Name = "dgResults";
			this.dgResults.ReadOnly = true;
			this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dgResults.RowHeadersWidth = 75;
			this.dgResults.Size = new System.Drawing.Size(724, 231);
			this.dgResults.TabIndex = 3;
			// 
			// sbResultsTabular
			// 
			this.sbResultsTabular.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsTabularExport,
            this.tsTabularFitnessFunction,
            this.tsTabularErrorAverage,
            this.tsTabularErrorMedian,
            this.tsTabularErrorStdDev});
			this.sbResultsTabular.Location = new System.Drawing.Point(0, 231);
			this.sbResultsTabular.Name = "sbResultsTabular";
			this.sbResultsTabular.Size = new System.Drawing.Size(724, 22);
			this.sbResultsTabular.TabIndex = 2;
			this.sbResultsTabular.Text = "statusStrip1";
			// 
			// tsTabularExport
			// 
			this.tsTabularExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsTabularExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsExportCSV,
            this.tsExportSSV,
            this.tsExportTab});
			this.tsTabularExport.Image = ((System.Drawing.Image)(resources.GetObject("tsTabularExport.Image")));
			this.tsTabularExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsTabularExport.Name = "tsTabularExport";
			this.tsTabularExport.Size = new System.Drawing.Size(52, 20);
			this.tsTabularExport.Text = "Export";
			this.tsTabularExport.ToolTipText = "Export Data To File";
			// 
			// tsExportCSV
			// 
			this.tsExportCSV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsExportCSV.Name = "tsExportCSV";
			this.tsExportCSV.Size = new System.Drawing.Size(223, 22);
			this.tsExportCSV.Text = "Comma Separated File (.csv)";
			this.tsExportCSV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsExportCSV.Click += new System.EventHandler(this.tsExportCSV_Click);
			// 
			// tsExportSSV
			// 
			this.tsExportSSV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsExportSSV.Name = "tsExportSSV";
			this.tsExportSSV.Size = new System.Drawing.Size(223, 22);
			this.tsExportSSV.Text = "Semi-Colon Separated (.ssv)";
			this.tsExportSSV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsExportSSV.Click += new System.EventHandler(this.tsExportSSV_Click);
			// 
			// tsExportTab
			// 
			this.tsExportTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsExportTab.Name = "tsExportTab";
			this.tsExportTab.Size = new System.Drawing.Size(223, 22);
			this.tsExportTab.Text = "Tab Separated (.txt)";
			this.tsExportTab.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsExportTab.Click += new System.EventHandler(this.tsExportTab_Click);
			// 
			// tsTabularFitnessFunction
			// 
			this.tsTabularFitnessFunction.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsTabularFitnessFunction.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.tsTabularFitnessFunction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsTabularFitnessFunction.Name = "tsTabularFitnessFunction";
			this.tsTabularFitnessFunction.Size = new System.Drawing.Size(89, 17);
			this.tsTabularFitnessFunction.Text = "Fitness Function";
			// 
			// tsTabularErrorAverage
			// 
			this.tsTabularErrorAverage.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsTabularErrorAverage.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.tsTabularErrorAverage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsTabularErrorAverage.Name = "tsTabularErrorAverage";
			this.tsTabularErrorAverage.Size = new System.Drawing.Size(61, 17);
			this.tsTabularErrorAverage.Text = "Avg. Error";
			// 
			// tsTabularErrorMedian
			// 
			this.tsTabularErrorMedian.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsTabularErrorMedian.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.tsTabularErrorMedian.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsTabularErrorMedian.Name = "tsTabularErrorMedian";
			this.tsTabularErrorMedian.Size = new System.Drawing.Size(45, 17);
			this.tsTabularErrorMedian.Text = "Median";
			// 
			// tsTabularErrorStdDev
			// 
			this.tsTabularErrorStdDev.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsTabularErrorStdDev.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.tsTabularErrorStdDev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsTabularErrorStdDev.Name = "tsTabularErrorStdDev";
			this.tsTabularErrorStdDev.Size = new System.Drawing.Size(57, 17);
			this.tsTabularErrorStdDev.Text = "Std. Dev.";
			// 
			// pageProgram
			// 
			this.pageProgram.Controls.Add(this.pnlProgramText);
			this.pageProgram.Controls.Add(this.sbResultsProgram);
			this.pageProgram.Location = new System.Drawing.Point(4, 25);
			this.pageProgram.Name = "pageProgram";
			this.pageProgram.Size = new System.Drawing.Size(724, 253);
			this.pageProgram.TabIndex = 3;
			this.pageProgram.Text = "Program";
			this.pageProgram.UseVisualStyleBackColor = true;
			// 
			// pnlProgramText
			// 
			this.pnlProgramText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlProgramText.Controls.Add(this.txtProgram);
			this.pnlProgramText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlProgramText.Location = new System.Drawing.Point(0, 0);
			this.pnlProgramText.Name = "pnlProgramText";
			this.pnlProgramText.Size = new System.Drawing.Size(724, 231);
			this.pnlProgramText.TabIndex = 5;
			// 
			// txtProgram
			// 
			this.txtProgram.BackColor = System.Drawing.SystemColors.Control;
			this.txtProgram.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProgram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtProgram.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtProgram.Location = new System.Drawing.Point(0, 0);
			this.txtProgram.Name = "txtProgram";
			this.txtProgram.ReadOnly = true;
			this.txtProgram.Size = new System.Drawing.Size(722, 229);
			this.txtProgram.TabIndex = 4;
			this.txtProgram.Text = "";
			this.txtProgram.WordWrap = false;
			// 
			// sbResultsProgram
			// 
			this.sbResultsProgram.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmenuLanguage});
			this.sbResultsProgram.Location = new System.Drawing.Point(0, 231);
			this.sbResultsProgram.Name = "sbResultsProgram";
			this.sbResultsProgram.Size = new System.Drawing.Size(724, 22);
			this.sbResultsProgram.TabIndex = 4;
			this.sbResultsProgram.Text = "statusStrip1";
			// 
			// tsmenuLanguage
			// 
			this.tsmenuLanguage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsmenuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProgramC,
            this.menuProgramCPP,
            this.menuProgramCSharp,
            this.menuProgramVB,
            this.menuProgramJava,
            this.menuProgramFortran,
            this.toolStripMenuItem2,
            this.menuProgramExport});
			this.tsmenuLanguage.Image = ((System.Drawing.Image)(resources.GetObject("tsmenuLanguage.Image")));
			this.tsmenuLanguage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsmenuLanguage.Name = "tsmenuLanguage";
			this.tsmenuLanguage.Size = new System.Drawing.Size(35, 20);
			this.tsmenuLanguage.Text = "C#";
			this.tsmenuLanguage.ToolTipText = "Program Language";
			// 
			// menuProgramC
			// 
			this.menuProgramC.Name = "menuProgramC";
			this.menuProgramC.Size = new System.Drawing.Size(162, 22);
			this.menuProgramC.Text = "C";
			this.menuProgramC.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// menuProgramCPP
			// 
			this.menuProgramCPP.Name = "menuProgramCPP";
			this.menuProgramCPP.Size = new System.Drawing.Size(162, 22);
			this.menuProgramCPP.Text = "C++";
			this.menuProgramCPP.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// menuProgramCSharp
			// 
			this.menuProgramCSharp.Name = "menuProgramCSharp";
			this.menuProgramCSharp.Size = new System.Drawing.Size(162, 22);
			this.menuProgramCSharp.Text = "C#";
			this.menuProgramCSharp.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// menuProgramVB
			// 
			this.menuProgramVB.Name = "menuProgramVB";
			this.menuProgramVB.Size = new System.Drawing.Size(162, 22);
			this.menuProgramVB.Text = "Visual Basic.NET";
			this.menuProgramVB.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// menuProgramJava
			// 
			this.menuProgramJava.Name = "menuProgramJava";
			this.menuProgramJava.Size = new System.Drawing.Size(162, 22);
			this.menuProgramJava.Text = "Java";
			this.menuProgramJava.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// menuProgramFortran
			// 
			this.menuProgramFortran.Name = "menuProgramFortran";
			this.menuProgramFortran.Size = new System.Drawing.Size(162, 22);
			this.menuProgramFortran.Text = "Fortran";
			this.menuProgramFortran.Click += new System.EventHandler(this.rdLanguage_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(159, 6);
			// 
			// menuProgramExport
			// 
			this.menuProgramExport.Name = "menuProgramExport";
			this.menuProgramExport.Size = new System.Drawing.Size(162, 22);
			this.menuProgramExport.Text = "Export";
			this.menuProgramExport.Click += new System.EventHandler(this.btnProgramExport_Click);
			// 
			// pageModelingHistory
			// 
			this.pageModelingHistory.Controls.Add(this.diagProgram);
			this.pageModelingHistory.Controls.Add(this.lvProgramHist);
			this.pageModelingHistory.Location = new System.Drawing.Point(4, 25);
			this.pageModelingHistory.Name = "pageModelingHistory";
			this.pageModelingHistory.Size = new System.Drawing.Size(724, 253);
			this.pageModelingHistory.TabIndex = 2;
			this.pageModelingHistory.Text = "Structure";
			this.pageModelingHistory.UseVisualStyleBackColor = true;
			// 
			// diagProgram
			// 
			this.diagProgram.AutoScroll = true;
			this.diagProgram.BranchHeight = 70;
			this.diagProgram.ConnectionType = Netron.Lithium.ConnectionType.Bezier;
			this.diagProgram.Controls.Add(this.btnDiagramConstruct);
			this.diagProgram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diagProgram.LayoutDirection = Netron.Lithium.TreeDirection.Vertical;
			this.diagProgram.LayoutEnabled = true;
			this.diagProgram.Location = new System.Drawing.Point(141, 0);
			this.diagProgram.Name = "diagProgram";
			this.diagProgram.Size = new System.Drawing.Size(583, 253);
			this.diagProgram.TabIndex = 2;
			this.diagProgram.TabStop = false;
			this.diagProgram.WordSpacing = 20;
			// 
			// btnDiagramConstruct
			// 
			this.btnDiagramConstruct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDiagramConstruct.Location = new System.Drawing.Point(3, 3);
			this.btnDiagramConstruct.Name = "btnDiagramConstruct";
			this.btnDiagramConstruct.Size = new System.Drawing.Size(71, 23);
			this.btnDiagramConstruct.TabIndex = 0;
			this.btnDiagramConstruct.Text = "Construct";
			this.btnDiagramConstruct.UseVisualStyleBackColor = true;
			this.btnDiagramConstruct.Click += new System.EventHandler(this.btnDiagramConstruct_Click);
			// 
			// lvProgramHist
			// 
			this.lvProgramHist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
			this.lvProgramHist.Dock = System.Windows.Forms.DockStyle.Left;
			this.lvProgramHist.FullRowSelect = true;
			this.lvProgramHist.GridLines = true;
			this.lvProgramHist.Location = new System.Drawing.Point(0, 0);
			this.lvProgramHist.MultiSelect = false;
			this.lvProgramHist.Name = "lvProgramHist";
			this.lvProgramHist.ShowGroups = false;
			this.lvProgramHist.Size = new System.Drawing.Size(141, 253);
			this.lvProgramHist.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvProgramHist.TabIndex = 1;
			this.lvProgramHist.UseCompatibleStateImageBehavior = false;
			this.lvProgramHist.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Name";
			this.columnHeader4.Width = 90;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Count";
			// 
			// statusResults
			// 
			this.statusResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgressResults});
			this.statusResults.Location = new System.Drawing.Point(0, 260);
			this.statusResults.Name = "statusResults";
			this.statusResults.Size = new System.Drawing.Size(732, 22);
			this.statusResults.SizingGrip = false;
			this.statusResults.TabIndex = 3;
			this.statusResults.Text = "statusStrip1";
			this.statusResults.Visible = false;
			// 
			// tsProgressResults
			// 
			this.tsProgressResults.Name = "tsProgressResults";
			this.tsProgressResults.Size = new System.Drawing.Size(300, 16);
			// 
			// contextMenuPrograms
			// 
			this.contextMenuPrograms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.contextMenuPrograms.Name = "contextMenuStrip1";
			this.contextMenuPrograms.Size = new System.Drawing.Size(117, 26);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Image = global::GPStudio.Client.Properties.Resources.error;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
			this.toolStripMenuItem1.Text = "Delete";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
			// 
			// dlgFileSaveSource
			// 
			this.dlgFileSaveSource.Title = "Save Program Source As";
			// 
			// dlgFileSaveGrid
			// 
			this.dlgFileSaveGrid.Title = "Save Tabular Results";
			// 
			// fmProject
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 491);
			this.Controls.Add(this.tabProject);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "fmProject";
			this.Text = "GP Project - <name>";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmProject_FormClosing);
			this.tabProject.ResumeLayout(false);
			this.pageConfiguration.ResumeLayout(false);
			this.pageConfiguration.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.pnlProfilesToolbar.ResumeLayout(false);
			this.pnlProfilesToolbar.PerformLayout();
			this.tsProfiles.ResumeLayout(false);
			this.tsProfiles.PerformLayout();
			this.tabProfile.ResumeLayout(false);
			this.tabModelingType.ResumeLayout(false);
			this.tabModelingType.PerformLayout();
			this.gbModeling_Regression.ResumeLayout(false);
			this.gbModeling_Regression.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udModeling_PredictionDistance)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udModeling_InputDimension)).EndInit();
			this.tabProgramStructure.ResumeLayout(false);
			this.tabProgramStructure.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udADR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udADF)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_MemoryCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udADL)).EndInit();
			this.tabFunctionSet.ResumeLayout(false);
			this.tabTerminalSet.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.groupBox11.PerformLayout();
			this.tabFitness.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox13.ResumeLayout(false);
			this.groupBox13.PerformLayout();
			this.tabPopulationStructure.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox12.ResumeLayout(false);
			this.groupBox12.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udDistributed_Topology_StarPercent)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDistributed_TransferCount)).EndInit();
			this.groupBox16.ResumeLayout(false);
			this.groupBox16.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Crossover)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Mutation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_Reproduction)).EndInit();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPopulation_TreeDepth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udStructure_PopSize)).EndInit();
			this.tabGenerationOptions.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.groupBox10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udGens_MaxNumber)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.pageModeling.ResumeLayout(false);
			this.pageModeling.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.statusModeling.ResumeLayout(false);
			this.statusModeling.PerformLayout();
			this.tabModeling.ResumeLayout(false);
			this.pageModelFitness.ResumeLayout(false);
			this.pagePopulationFitness.ResumeLayout(false);
			this.pagePopulationComplexity.ResumeLayout(false);
			this.groupBox14.ResumeLayout(false);
			this.pnlModelingToolbar.ResumeLayout(false);
			this.pnlModelingToolbar.PerformLayout();
			this.tsModeling.ResumeLayout(false);
			this.tsModeling.PerformLayout();
			this.pageResults.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.Panel2.PerformLayout();
			this.splitContainer3.ResumeLayout(false);
			this.splitResults.Panel1.ResumeLayout(false);
			this.splitResults.Panel2.ResumeLayout(false);
			this.splitResults.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.tabResults.ResumeLayout(false);
			this.pageResultsGraphical.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.pageResultsTabular.ResumeLayout(false);
			this.pageResultsTabular.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
			this.sbResultsTabular.ResumeLayout(false);
			this.sbResultsTabular.PerformLayout();
			this.pageProgram.ResumeLayout(false);
			this.pageProgram.PerformLayout();
			this.pnlProgramText.ResumeLayout(false);
			this.sbResultsProgram.ResumeLayout(false);
			this.sbResultsProgram.PerformLayout();
			this.pageModelingHistory.ResumeLayout(false);
			this.diagProgram.ResumeLayout(false);
			this.statusResults.ResumeLayout(false);
			this.statusResults.PerformLayout();
			this.contextMenuPrograms.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabProject;
		private System.Windows.Forms.TabPage pageConfiguration;
		private System.Windows.Forms.TabPage pageModeling;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListView lvProfiles;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.TextBox txtDataTraining;
		private System.Windows.Forms.Button btnDataTraining;
		private System.Windows.Forms.TextBox txtProjectName;
		private System.Windows.Forms.TabPage pageResults;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.SplitContainer splitResults;
		private System.Windows.Forms.ListView lvResultsPrograms;
		private System.Windows.Forms.ColumnHeader colHits;
		private System.Windows.Forms.ColumnHeader colFitness;
		private System.Windows.Forms.ColumnHeader colDateTime;
		private System.Windows.Forms.ColumnHeader colComplexity;
		private System.Windows.Forms.ContextMenuStrip contextMenuPrograms;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.TabControl tabResults;
		private System.Windows.Forms.TabPage pageResultsGraphical;
		private System.Windows.Forms.TabPage pageResultsTabular;
		private System.Windows.Forms.TabPage pageProgram;
		private System.Windows.Forms.TabPage pageModelingHistory;
		private System.Windows.Forms.StatusStrip statusResults;
		private System.Windows.Forms.ToolStripProgressBar tsProgressResults;
		private System.Windows.Forms.Panel panel3;
		private ZedGraph.ZedGraphControl graphResults;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rdChartTypeScatter;
		private System.Windows.Forms.RadioButton rdChartTypeBar;
		private System.Windows.Forms.RadioButton rdChartTypeXY;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ComboBox cbXAxis;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ListView lvProfilesResults;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.StatusStrip sbResultsTabular;
		private System.Windows.Forms.DataGridView dgResults;
		private System.Windows.Forms.ToolStripStatusLabel tsTabularErrorAverage;
		private System.Windows.Forms.ToolStripStatusLabel tsTabularErrorStdDev;
		private System.Windows.Forms.ToolStripStatusLabel tsTabularErrorMedian;
		private System.Windows.Forms.CheckBox chkCount;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.CheckBox chkModelOnly;
		private Netron.Lithium.LithiumControl diagProgram;
		private System.Windows.Forms.Button btnDiagramConstruct;
		private System.Windows.Forms.ListView lvProgramHist;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.TabControl tabProfile;
		private System.Windows.Forms.TabPage tabModelingType;
		private System.Windows.Forms.TabPage tabProgramStructure;
		private System.Windows.Forms.TabPage tabGenerationOptions;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.NumericUpDown udGens_MaxNumber;
		private System.Windows.Forms.CheckBox chkGens_HitsMaxed;
		private System.Windows.Forms.CheckBox chkGens_RawFitness0;
		private System.Windows.Forms.CheckBox chkGens_MaxNumber;
		private System.Windows.Forms.TabPage tabFunctionSet;
		private System.Windows.Forms.ListView lvFunctions;
		private System.Windows.Forms.ColumnHeader colFunctionName;
		private System.Windows.Forms.ColumnHeader colFunctionArity;
		private System.Windows.Forms.ColumnHeader colFunctionDescription;
		private System.Windows.Forms.TabPage tabTerminalSet;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.MaskedTextBox txtTS_IntegerMax;
		private System.Windows.Forms.MaskedTextBox txtTS_IntegerMin;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.CheckBox chkTS_RndDouble;
		private System.Windows.Forms.CheckBox chkTS_RndInteger;
		private System.Windows.Forms.TabPage tabFitness;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.TabPage tabPopulationStructure;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.NumericUpDown udPopulation_TreeDepth;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cmbStructure_PopReproduction;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cmbStructure_PopInit;
		private System.Windows.Forms.NumericUpDown udStructure_PopSize;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlProfilesToolbar;
		private System.Windows.Forms.ToolStrip tsProfiles;
		private System.Windows.Forms.ToolStripButton btnProfileAdd;
		private System.Windows.Forms.ToolStripButton btnProfileDelete;
		private System.Windows.Forms.ToolStripButton btnProfileSave;
		private System.Windows.Forms.ToolStripButton btnProfileCopy;
		private System.Windows.Forms.ImageList ilProfiles;
		private System.Windows.Forms.GroupBox gbModeling_Regression;
		private System.Windows.Forms.NumericUpDown udModeling_PredictionDistance;
		private System.Windows.Forms.NumericUpDown udModeling_InputDimension;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.RadioButton rdModeling_TimeSeries;
		private System.Windows.Forms.RadioButton rdModeling_Regression;
		private System.Windows.Forms.Button btnDataValidationDelete;
		private System.Windows.Forms.Button btnDataValidationAdd;
		private System.Windows.Forms.ComboBox cbDataValidation;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbResultsDataSet;
		private System.Windows.Forms.TabControl tabModeling;
		private System.Windows.Forms.TabPage pageModelFitness;
		private ZedGraph.ZedGraphControl graphFitness;
		private System.Windows.Forms.TabPage pagePopulationFitness;
		private ZedGraph.ZedGraphControl graphPopulationFitness;
		private System.Windows.Forms.TabPage pagePopulationComplexity;
		private ZedGraph.ZedGraphControl graphPopulationComplexity;
		private System.Windows.Forms.GroupBox groupBox14;
		private System.Windows.Forms.ListView lvProfilesModeling;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Panel pnlModelingToolbar;
		private System.Windows.Forms.ToolStrip tsModeling;
		private System.Windows.Forms.ToolStripButton btnCancel;
		private System.Windows.Forms.ToolStripProgressBar pgModeling;
		private System.Windows.Forms.GroupBox groupBox16;
		private System.Windows.Forms.NumericUpDown udStructure_Crossover;
		private System.Windows.Forms.NumericUpDown udStructure_Mutation;
		private System.Windows.Forms.NumericUpDown udStructure_Reproduction;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown udADR;
		private ListViewEx.ListViewEx lvADR;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.Button btnADRDelete;
		private System.Windows.Forms.Button btnADRAdd;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown udADF;
		private ListViewEx.ListViewEx lvADF;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colParams;
		private System.Windows.Forms.Button btnADFDelete;
		private System.Windows.Forms.Button btnADFAdd;
		private System.Windows.Forms.NumericUpDown udStructure_MemoryCount;
		private System.Windows.Forms.CheckBox chkStructure_UseMemory;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown udADL;
		private ListViewEx.ListViewEx lvADL;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.Button btnADLDelete;
		private System.Windows.Forms.Button btnADLAdd;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ColumnHeader colFunctionCategory;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.FlowLayoutPanel flowFitnessFunctions;
		private System.Windows.Forms.StatusStrip statusModeling;
		private System.Windows.Forms.ToolStripStatusLabel lbStatus;
		private System.Windows.Forms.ToolStripProgressBar pgStatus;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.ComboBox cmbPopulationFitnessServers;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.NumericUpDown udDistributed_TransferCount;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.RadioButton rdDistributed_Topology_Star;
		private System.Windows.Forms.RadioButton rdDistributed_Topology_Ring;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.NumericUpDown udDistributed_Topology_StarPercent;
		private System.Windows.Forms.CheckBox chkFitness_MultiObjective;
		private System.Windows.Forms.CheckBox chkFitness_Parsimony;
		private System.Windows.Forms.ToolStripDropDownButton btnModel;
		private System.Windows.Forms.ToolStripMenuItem tsModelInteractive;
		private System.Windows.Forms.ToolStripMenuItem tsModelBatch;
		private System.Windows.Forms.SaveFileDialog dlgFileSaveSource;
		private System.Windows.Forms.ToolStripDropDownButton tsTabularExport;
		private System.Windows.Forms.ToolStripMenuItem tsExportCSV;
		private System.Windows.Forms.ToolStripMenuItem tsExportSSV;
		private System.Windows.Forms.ToolStripMenuItem tsExportTab;
		private System.Windows.Forms.SaveFileDialog dlgFileSaveGrid;
		private System.Windows.Forms.ToolStripStatusLabel tsTabularFitnessFunction;
		private System.Windows.Forms.StatusStrip sbResultsProgram;
		private System.Windows.Forms.ToolStripDropDownButton tsmenuLanguage;
		private System.Windows.Forms.ToolStripMenuItem menuProgramExport;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem menuProgramFortran;
		private System.Windows.Forms.ToolStripMenuItem menuProgramJava;
		private System.Windows.Forms.ToolStripMenuItem menuProgramVB;
		private System.Windows.Forms.ToolStripMenuItem menuProgramCSharp;
		private System.Windows.Forms.ToolStripMenuItem menuProgramCPP;
		private System.Windows.Forms.ToolStripMenuItem menuProgramC;
		private System.Windows.Forms.Panel pnlProgramText;
		private System.Windows.Forms.RichTextBox txtProgram;
		private System.Windows.Forms.ToolTip ttFunctionSet;
	}
}