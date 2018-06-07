using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Lithium;
namespace FrontEnd
{
	/// <summary>
	/// A simple host to demonstrate the various functions of the Lithium control
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Fields
		private System.Windows.Forms.MenuItem mnuSaveAs;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuRandomDiagram;
		private System.Windows.Forms.MenuItem mnuNewDiagram;
		private System.Windows.Forms.MenuItem mnuAddRandomNode;
		private System.Windows.Forms.MenuItem mnuCenterRoot;
		private System.Windows.Forms.MenuItem mnuCollapseAll;
		private System.Windows.Forms.MenuItem mnuExpandAll;
		private System.Windows.Forms.MenuItem mnuForceLayout;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuDash1;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuHelpHelp;
		private System.Windows.Forms.MenuItem mnuDash5;
		private System.Windows.Forms.MenuItem mnuGotoGraphLib;
		private System.Windows.Forms.MenuItem mnuGotoNetronLight;
		private System.Windows.Forms.MenuItem mnuTheNetronProject;
		private System.Windows.Forms.MenuItem mnuDash2;
		private System.Windows.Forms.MenuItem mnuDash3;
		private System.Windows.Forms.MenuItem mnuDash4;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.Panel rightPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel mainPanel;
		private Netron.Lithium.LithiumControl lithiumControl;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.TabControl rightTabControl;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.TextBox outputBox;
		private System.Windows.Forms.ContextMenu lithiumMenu;
		private System.Windows.Forms.MenuItem cmnuCenterRoot;
		private System.Windows.Forms.MenuItem cmnuNewDiagram;
		private System.Windows.Forms.MenuItem cmnuDash1;
		private System.Windows.Forms.MenuItem cmnuAddChild;
		private System.Windows.Forms.MenuItem cmnuDelete;
		private System.Windows.Forms.MenuItem cmnuDash2;
		private System.Windows.Forms.MenuItem mnuAddChild;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabPage tabOutput;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuDFTInfo;
		private System.Windows.Forms.MenuItem mnuBFTInfo;
		private System.Windows.Forms.MenuItem mnuLayoutDirection;
		private System.Windows.Forms.MenuItem mnuVerticalDirection;
		private System.Windows.Forms.MenuItem mnuHorizontalDirection;
		private System.Windows.Forms.MenuItem mnuXMLExport;
		private System.Windows.Forms.MenuItem mnuColorLevels;
		private System.Windows.Forms.MenuItem mnuClassReference;

		#endregion
		private IContainer components;

		#region construtor
		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

//			this.lithiumControl. AddOTM();			

			MakeExample();
			this.lithiumControl.Root.Expand();
			this.lithiumControl.DrawTree();
			}

		#endregion

		#region Methods
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuSaveAs = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuDash1 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuNewDiagram = new System.Windows.Forms.MenuItem();
			this.mnuRandomDiagram = new System.Windows.Forms.MenuItem();
			this.mnuDash4 = new System.Windows.Forms.MenuItem();
			this.mnuAddRandomNode = new System.Windows.Forms.MenuItem();
			this.mnuCenterRoot = new System.Windows.Forms.MenuItem();
			this.mnuAddChild = new System.Windows.Forms.MenuItem();
			this.mnuDash3 = new System.Windows.Forms.MenuItem();
			this.mnuCollapseAll = new System.Windows.Forms.MenuItem();
			this.mnuExpandAll = new System.Windows.Forms.MenuItem();
			this.mnuDash2 = new System.Windows.Forms.MenuItem();
			this.mnuForceLayout = new System.Windows.Forms.MenuItem();
			this.mnuLayoutDirection = new System.Windows.Forms.MenuItem();
			this.mnuVerticalDirection = new System.Windows.Forms.MenuItem();
			this.mnuHorizontalDirection = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuDFTInfo = new System.Windows.Forms.MenuItem();
			this.mnuBFTInfo = new System.Windows.Forms.MenuItem();
			this.mnuXMLExport = new System.Windows.Forms.MenuItem();
			this.mnuColorLevels = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpHelp = new System.Windows.Forms.MenuItem();
			this.mnuDash5 = new System.Windows.Forms.MenuItem();
			this.mnuGotoGraphLib = new System.Windows.Forms.MenuItem();
			this.mnuGotoNetronLight = new System.Windows.Forms.MenuItem();
			this.mnuTheNetronProject = new System.Windows.Forms.MenuItem();
			this.mnuClassReference = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.rightPanel = new System.Windows.Forms.Panel();
			this.rightTabControl = new System.Windows.Forms.TabControl();
			this.tabProperties = new System.Windows.Forms.TabPage();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.tabOutput = new System.Windows.Forms.TabPage();
			this.outputBox = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.lithiumMenu = new System.Windows.Forms.ContextMenu();
			this.cmnuNewDiagram = new System.Windows.Forms.MenuItem();
			this.cmnuDash1 = new System.Windows.Forms.MenuItem();
			this.cmnuCenterRoot = new System.Windows.Forms.MenuItem();
			this.cmnuAddChild = new System.Windows.Forms.MenuItem();
			this.cmnuDelete = new System.Windows.Forms.MenuItem();
			this.cmnuDash2 = new System.Windows.Forms.MenuItem();
			this.lithiumControl = new Netron.Lithium.LithiumControl();
			this.rightPanel.SuspendLayout();
			this.rightTabControl.SuspendLayout();
			this.tabProperties.SuspendLayout();
			this.tabOutput.SuspendLayout();
			this.mainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.menuItem3,
            this.menuItem1,
            this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSaveAs,
            this.mnuOpen,
            this.mnuDash1,
            this.mnuExit});
			this.mnuFile.Text = "File";
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 0;
			this.mnuSaveAs.Text = "Save as...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 1;
			this.mnuOpen.Text = "Open...";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuDash1
			// 
			this.mnuDash1.Index = 2;
			this.mnuDash1.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 3;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNewDiagram,
            this.mnuRandomDiagram,
            this.mnuDash4,
            this.mnuAddRandomNode,
            this.mnuCenterRoot,
            this.mnuAddChild,
            this.mnuDash3,
            this.mnuCollapseAll,
            this.mnuExpandAll,
            this.mnuDash2,
            this.mnuForceLayout,
            this.mnuLayoutDirection});
			this.menuItem3.Text = "Diagram";
			// 
			// mnuNewDiagram
			// 
			this.mnuNewDiagram.Index = 0;
			this.mnuNewDiagram.Text = "New diagram";
			this.mnuNewDiagram.Click += new System.EventHandler(this.mnuNewDiagram_Click);
			// 
			// mnuRandomDiagram
			// 
			this.mnuRandomDiagram.Index = 1;
			this.mnuRandomDiagram.Text = "Random diagram";
			this.mnuRandomDiagram.Click += new System.EventHandler(this.mnuRandomDiagram_Click);
			// 
			// mnuDash4
			// 
			this.mnuDash4.Index = 2;
			this.mnuDash4.Text = "-";
			// 
			// mnuAddRandomNode
			// 
			this.mnuAddRandomNode.Index = 3;
			this.mnuAddRandomNode.Text = "Add random node";
			this.mnuAddRandomNode.Click += new System.EventHandler(this.mnuAddRandom_Click);
			// 
			// mnuCenterRoot
			// 
			this.mnuCenterRoot.Index = 4;
			this.mnuCenterRoot.Text = "Center root";
			this.mnuCenterRoot.Click += new System.EventHandler(this.mnuCenterRoot_Click);
			// 
			// mnuAddChild
			// 
			this.mnuAddChild.Index = 5;
			this.mnuAddChild.Text = "Add child";
			this.mnuAddChild.Click += new System.EventHandler(this.mnuAddChild_Click);
			// 
			// mnuDash3
			// 
			this.mnuDash3.Index = 6;
			this.mnuDash3.Text = "-";
			// 
			// mnuCollapseAll
			// 
			this.mnuCollapseAll.Index = 7;
			this.mnuCollapseAll.Text = "Collapse all";
			this.mnuCollapseAll.Click += new System.EventHandler(this.mnuCollapseAll_Click);
			// 
			// mnuExpandAll
			// 
			this.mnuExpandAll.Index = 8;
			this.mnuExpandAll.Text = "Expand all";
			this.mnuExpandAll.Click += new System.EventHandler(this.mnuExpandAll_Click);
			// 
			// mnuDash2
			// 
			this.mnuDash2.Index = 9;
			this.mnuDash2.Text = "-";
			// 
			// mnuForceLayout
			// 
			this.mnuForceLayout.Checked = true;
			this.mnuForceLayout.Index = 10;
			this.mnuForceLayout.Text = "Force layout";
			this.mnuForceLayout.Click += new System.EventHandler(this.mnuForceLayout_Click);
			// 
			// mnuLayoutDirection
			// 
			this.mnuLayoutDirection.Index = 11;
			this.mnuLayoutDirection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuVerticalDirection,
            this.mnuHorizontalDirection});
			this.mnuLayoutDirection.Text = "Layout direction";
			// 
			// mnuVerticalDirection
			// 
			this.mnuVerticalDirection.Index = 0;
			this.mnuVerticalDirection.Text = "Vertical";
			this.mnuVerticalDirection.Click += new System.EventHandler(this.mnuVerticalDirection_Click);
			// 
			// mnuHorizontalDirection
			// 
			this.mnuHorizontalDirection.Checked = true;
			this.mnuHorizontalDirection.Index = 1;
			this.mnuHorizontalDirection.Text = "Horizontal";
			this.mnuHorizontalDirection.Click += new System.EventHandler(this.mnuHorizontalDirection_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDFTInfo,
            this.mnuBFTInfo,
            this.mnuXMLExport,
            this.mnuColorLevels});
			this.menuItem1.Text = "Examples";
			// 
			// mnuDFTInfo
			// 
			this.mnuDFTInfo.Index = 0;
			this.mnuDFTInfo.Text = "DFT of the diagram";
			this.mnuDFTInfo.Click += new System.EventHandler(this.mnuDFTInfo_Click);
			// 
			// mnuBFTInfo
			// 
			this.mnuBFTInfo.Index = 1;
			this.mnuBFTInfo.Text = "BFT of the diagram";
			this.mnuBFTInfo.Click += new System.EventHandler(this.mnuBFTInfo_Click);
			// 
			// mnuXMLExport
			// 
			this.mnuXMLExport.Index = 2;
			this.mnuXMLExport.Text = "XML export";
			this.mnuXMLExport.Click += new System.EventHandler(this.mnuXMLExport_Click);
			// 
			// mnuColorLevels
			// 
			this.mnuColorLevels.Index = 3;
			this.mnuColorLevels.Text = "Color levels";
			this.mnuColorLevels.Click += new System.EventHandler(this.mnuColorLevels_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 3;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHelpHelp,
            this.mnuDash5,
            this.mnuGotoGraphLib,
            this.mnuGotoNetronLight,
            this.mnuTheNetronProject,
            this.mnuClassReference});
			this.mnuHelp.Text = "Help";
			// 
			// mnuHelpHelp
			// 
			this.mnuHelpHelp.Index = 0;
			this.mnuHelpHelp.Text = "Lithium Help";
			this.mnuHelpHelp.Click += new System.EventHandler(this.mnuHelpHelp_Click);
			// 
			// mnuDash5
			// 
			this.mnuDash5.Index = 1;
			this.mnuDash5.Text = "-";
			// 
			// mnuGotoGraphLib
			// 
			this.mnuGotoGraphLib.Index = 2;
			this.mnuGotoGraphLib.Text = "Graph Library";
			this.mnuGotoGraphLib.Click += new System.EventHandler(this.mnuGotoGraphLib_Click);
			// 
			// mnuGotoNetronLight
			// 
			this.mnuGotoNetronLight.Index = 3;
			this.mnuGotoNetronLight.Text = "Netron Light";
			this.mnuGotoNetronLight.Click += new System.EventHandler(this.mnuGotoNetronLight_Click);
			// 
			// mnuTheNetronProject
			// 
			this.mnuTheNetronProject.Index = 4;
			this.mnuTheNetronProject.Text = "The Netron Project";
			this.mnuTheNetronProject.Click += new System.EventHandler(this.mnuTheNetronProject_Click);
			// 
			// mnuClassReference
			// 
			this.mnuClassReference.Index = 5;
			this.mnuClassReference.Text = "Class reference";
			this.mnuClassReference.Click += new System.EventHandler(this.mnuClassReference_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 510);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(832, 22);
			this.statusBar.TabIndex = 0;
			// 
			// rightPanel
			// 
			this.rightPanel.Controls.Add(this.rightTabControl);
			this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightPanel.Location = new System.Drawing.Point(632, 0);
			this.rightPanel.Name = "rightPanel";
			this.rightPanel.Size = new System.Drawing.Size(200, 510);
			this.rightPanel.TabIndex = 3;
			// 
			// rightTabControl
			// 
			this.rightTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.rightTabControl.Controls.Add(this.tabProperties);
			this.rightTabControl.Controls.Add(this.tabOutput);
			this.rightTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rightTabControl.Location = new System.Drawing.Point(0, 0);
			this.rightTabControl.Name = "rightTabControl";
			this.rightTabControl.SelectedIndex = 0;
			this.rightTabControl.Size = new System.Drawing.Size(200, 510);
			this.rightTabControl.TabIndex = 0;
			// 
			// tabProperties
			// 
			this.tabProperties.Controls.Add(this.propertyGrid);
			this.tabProperties.Location = new System.Drawing.Point(4, 25);
			this.tabProperties.Name = "tabProperties";
			this.tabProperties.Size = new System.Drawing.Size(192, 481);
			this.tabProperties.TabIndex = 0;
			this.tabProperties.Text = "Properties";
			// 
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(192, 481);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.ToolbarVisible = false;
			// 
			// tabOutput
			// 
			this.tabOutput.Controls.Add(this.outputBox);
			this.tabOutput.Location = new System.Drawing.Point(4, 25);
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.Size = new System.Drawing.Size(192, 502);
			this.tabOutput.TabIndex = 1;
			this.tabOutput.Text = "Output";
			// 
			// outputBox
			// 
			this.outputBox.BackColor = System.Drawing.SystemColors.Info;
			this.outputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.outputBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputBox.Location = new System.Drawing.Point(0, 0);
			this.outputBox.Multiline = true;
			this.outputBox.Name = "outputBox";
			this.outputBox.ReadOnly = true;
			this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.outputBox.Size = new System.Drawing.Size(192, 502);
			this.outputBox.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(629, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 510);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// mainPanel
			// 
			this.mainPanel.Controls.Add(this.lithiumControl);
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(0, 0);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(629, 510);
			this.mainPanel.TabIndex = 5;
			// 
			// lithiumMenu
			// 
			this.lithiumMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuNewDiagram,
            this.cmnuDash1,
            this.cmnuCenterRoot,
            this.cmnuAddChild,
            this.cmnuDelete,
            this.cmnuDash2});
			// 
			// cmnuNewDiagram
			// 
			this.cmnuNewDiagram.Index = 0;
			this.cmnuNewDiagram.Text = "New diagram";
			this.cmnuNewDiagram.Click += new System.EventHandler(this.mnuNewDiagram_Click);
			// 
			// cmnuDash1
			// 
			this.cmnuDash1.Index = 1;
			this.cmnuDash1.Text = "-";
			// 
			// cmnuCenterRoot
			// 
			this.cmnuCenterRoot.Index = 2;
			this.cmnuCenterRoot.Text = "Center root";
			this.cmnuCenterRoot.Click += new System.EventHandler(this.mnuCenterRoot_Click);
			// 
			// cmnuAddChild
			// 
			this.cmnuAddChild.Index = 3;
			this.cmnuAddChild.Text = "Add child";
			this.cmnuAddChild.Click += new System.EventHandler(this.mnuAddChild_Click);
			// 
			// cmnuDelete
			// 
			this.cmnuDelete.Index = 4;
			this.cmnuDelete.Text = "Delete";
			this.cmnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// cmnuDash2
			// 
			this.cmnuDash2.Index = 5;
			this.cmnuDash2.Text = "-";
			// 
			// lithiumControl
			// 
			this.lithiumControl.AutoScroll = true;
			this.lithiumControl.BackColor = System.Drawing.Color.WhiteSmoke;
			this.lithiumControl.BranchHeight = 120;
			this.lithiumControl.ConnectionType = Netron.Lithium.ConnectionType.Traditional;
			this.lithiumControl.ContextMenu = this.lithiumMenu;
			this.lithiumControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lithiumControl.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lithiumControl.LayoutDirection = Netron.Lithium.TreeDirection.Horizontal;
			this.lithiumControl.LayoutEnabled = true;
			this.lithiumControl.Location = new System.Drawing.Point(0, 0);
			this.lithiumControl.Name = "lithiumControl";
			this.lithiumControl.Size = new System.Drawing.Size(629, 510);
			this.lithiumControl.TabIndex = 3;
			this.lithiumControl.Text = "lithiumControl1";
			this.lithiumControl.WordSpacing = 20;
			this.lithiumControl.OnShowProps += new Netron.Lithium.ShowProps(this.lithiumControl_OnShowProps);
			this.lithiumControl.OnNewNode += new Netron.Lithium.ShapeData(this.lithiumControl_OnNewNode);
			this.lithiumControl.OnDeleteNode += new Netron.Lithium.ShapeData(this.lithiumControl_OnDeleteNode);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(832, 532);
			this.Controls.Add(this.mainPanel);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.rightPanel);
			this.Controls.Add(this.statusBar);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tree control example";
			this.rightPanel.ResumeLayout(false);
			this.rightTabControl.ResumeLayout(false);
			this.tabProperties.ResumeLayout(false);
			this.tabOutput.ResumeLayout(false);
			this.tabOutput.PerformLayout();
			this.mainPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Makes a simple demo diagram (not scientifically correct)
		/// </summary>
		private void MakeExample()
		{
			this.lithiumControl.Root.Text = "Ecosystem";
			ShapeBase root = this.lithiumControl.Root;

			ShapeBase ert = root.AddChild("Earth");
			ShapeBase com = root.AddChild("Community");
			
			ShapeBase pop = com.AddChild("Population");
			ShapeBase org = com.AddChild("Organism");
			ShapeBase bac = com.AddChild("Bacteria");
			ShapeBase vir = com.AddChild("Virus");
			
			com.Expand();

			ShapeBase euk = bac.AddChild("Eukaryotes");
			ShapeBase arch = bac.AddChild("Archea");
			ShapeBase pro = euk.AddChild("Protista");
			ShapeBase fla = pro.AddChild("Flagellates");
			ShapeBase amo = pro.AddChild("Amoeba");
			ShapeBase alg = pro.AddChild("Algae");
			ShapeBase par = pro.AddChild("Parasites");


			ShapeBase anim = pop.AddChild("Animals");
			ShapeBase pla = pop.AddChild("Plants");
			ShapeBase ins = pop.AddChild("Insects");
			
			ShapeBase ant = ins.AddChild("Ants");

			ShapeBase mus = pop.AddChild("Mushrooms");
			

			//re-order and center a bit to please the eyes
			root.Move(new Point(20-root.X, Convert.ToInt32(this.lithiumControl.Height/2)-root.Y));
			bac.Collapse(true);

			//as well as some coloring
			this.mnuColorLevels_Click(null, EventArgs.Empty);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Menu handlers

		#region Open/save
		/// <summary>
		/// Menu handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			OpenDiagram();
		}
		/// <summary>
		/// Menu handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveDiagram();			
		}
		/// <summary>
		/// Opens a diagram
		/// </summary>
		private void OpenDiagram()
		{
			OpenFileDialog fileChooser= new OpenFileDialog();
			fileChooser.Filter = "Diagram file (*.gxl)|*.gxl";
			DialogResult result =fileChooser.ShowDialog();
			string filename;			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				this.lithiumControl.OpenGraph(filename);
				MessageBox.Show("Project opened from '" + filename + "'");
			}
		}

		/// <summary>
		/// Saves a diagram
		/// </summary>
		private void SaveDiagram()
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.Filter = "Diagram file (*.gxl)|*.gxl";
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			fileChooser.CheckFileExists=false;
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				this.lithiumControl.SaveGraphAs(filename);				
				MessageBox.Show("The diagram was saved to '" + filename  + "'","Diagram saved",MessageBoxButtons.OK,MessageBoxIcon.Information);

			}
		}
		#endregion

		private void mnuLayout_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.DrawTree();
		}

		private void mnuAddRandom_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.AddRandomNode();
		}

		private void mnuShift_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.MoveDiagram(new Point(20,0));
		}
		
		private void mnuCenterRoot_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.CenterRoot();
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			DialogResult res = MessageBox.Show(this, "Save before exit?", "Save diagram", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if(res==DialogResult.Cancel)
				return;
			if(res==DialogResult.Yes)
			{
				SaveDiagram();
			}
			Application.Exit();
		}
		
		private void mnuNewDiagram_Click(object sender, System.EventArgs e)
		{
			DialogResult res = MessageBox.Show(this, "Save before exit?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if(res==DialogResult.Cancel)
				return;
			if(res==DialogResult.Yes)
			{
				SaveDiagram();
			}
			this.lithiumControl.NewDiagram();
			
		}

		private void mnuRandomDiagram_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.NewDiagram();

			int graphSize = 20; //a few thousand nodes still works fine if you have enough RAM
			for(int k=0; k<graphSize; k++)
			{
				this.lithiumControl.AddRandomNode("Node " + k);
			}

		}

		private void mnuCollapseAll_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.CollapseAll();
		}

		private void mnuForceLayout_Click(object sender, System.EventArgs e)
		{
			mnuForceLayout.Checked = !mnuForceLayout.Checked;
			lithiumControl.LayoutEnabled = mnuForceLayout.Checked;

		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.Delete();
		}

		private void mnuAddChild_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.AddChild();
		}

		private void mnuExpandAll_Click(object sender, System.EventArgs e)
		{
			this.lithiumControl.ExpandAll();
		}
		
		private void mnuHelpHelp_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://netron.sourceforge.net/ewiki/netron.php?id=LithiumControl");
		}

		private void mnuGotoGraphLib_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://netron.sourceforge.net/ewiki/netron.php?id=NetronGraphLib");
		}

		private void mnuGotoNetronLight_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://www.codeproject.com/cs/miscctrl/NetronLight.asp");
		}

		private void mnuTheNetronProject_Click(object sender, System.EventArgs e)
		{
			Process.Start("http://netron.sf.net");
		}

		private void mnuHorizontalDirection_Click(object sender, System.EventArgs e)
		{
			if(mnuHorizontalDirection.Checked) return	;	
			mnuVerticalDirection.Checked = false;
			mnuHorizontalDirection.Checked = true;
			CheckDirection();
		}
		private void mnuVerticalDirection_Click(object sender, System.EventArgs e)
		{
				if(mnuVerticalDirection.Checked) return;		
			mnuVerticalDirection.Checked = true;
			mnuHorizontalDirection.Checked = false;
			CheckDirection();
		}


		#region Examples of the visitor interface
		private void mnuColorLevels_Click(object sender, System.EventArgs e)
		{
			
			Output("");
			Output("Breadth first traversal of the diagram and coloring of levels.");
			
			ColoringVisitor visitor = new ColoringVisitor();			
			lithiumControl.BreadthFirstTraversal(visitor);
			

		}
		private void mnuDFTInfo_Click(object sender, System.EventArgs e)
		{
			Output("");
			Output("Depth first traversal of the diagram.");
			Output("----------------");
			InfoVisitor visitor = new InfoVisitor();
			visitor.OnInfo+=new Messager(visitor_OnInfo);
			lithiumControl.DepthFirstTraversal(visitor);
			Output("----------------");
		}
		
		private void mnuXMLExport_Click(object sender, System.EventArgs e)
		{
			Output("");
			Output("XML output example using the PrePost visitor interface.");
			Output("Copy/paste the XML to file and edit in browser or an XML-editor to view the tree-like structure");
			Output("----------------");
			XmlVisitor visitor = new XmlVisitor();
			visitor.OnInfo+=new Messager(visitor_OnInfo);
			lithiumControl.DepthFirstTraversal(visitor);
			Output("----------------");
		}
		private void mnuBFTInfo_Click(object sender, System.EventArgs e)
		{
			Output("");
			Output("Breadth first traversal of the diagram.");
			Output("----------------");
			InfoVisitor visitor = new InfoVisitor();
			visitor.OnInfo+=new Messager(visitor_OnInfo);
			lithiumControl.BreadthFirstTraversal(visitor);
			Output("----------------");
		}

		#endregion

		#endregion

		#region Diverse event handlers
		/// <summary>
		/// Occurse when a visitor sends out some info
		/// </summary>
		/// <param name="message"></param>
		private void visitor_OnInfo(string message)
		{
			Output(message);
		}


		/// <summary>
		/// Changes the layout direction
		/// </summary>
		private void CheckDirection()
		{
			if(mnuHorizontalDirection.Checked)
				this.lithiumControl.LayoutDirection = TreeDirection.Horizontal;
			else
				this.lithiumControl.LayoutDirection = TreeDirection.Vertical;
		}

		/// <summary>
		/// Occurs when the lithium control broadcasts info
		/// </summary>
		/// <param name="ent"></param>
		private void lithiumControl_OnShowProps(object ent)
		{
			ShowProps(ent);
		}
		/// <summary>
		/// Occurs when some properties have to be shown
		/// </summary>
		/// <param name="obj"></param>
		private void ShowProps(object obj)
		{
			this.propertyGrid.SelectedObject = obj;
			rightTabControl.SelectedTab = tabProperties;
		}
		/// <summary>
		/// Generic output to the output box
		/// </summary>
		/// <param name="text"></param>
		private void Output(string text)
		{
			this.outputBox.Text += text + Environment.NewLine;
			//move down to the new postioning so things stay in view
			this.outputBox.SelectionLength = this.outputBox.Text.Length;
			this.outputBox.ScrollToCaret();
			rightTabControl.SelectedTab = tabOutput;
		}


		/// <summary>
		/// Occurs when a new node is added to the diagram
		/// </summary>
		/// <param name="shape"></param>
		private void lithiumControl_OnNewNode(Netron.Lithium.ShapeBase shape)
		{
			Output("New node: " + shape.Text);
		}
		/// <summary>
		/// Occurs when a node is removed from the diagram
		/// </summary>
		/// <param name="shape"></param>
		private void lithiumControl_OnDeleteNode(Netron.Lithium.ShapeBase shape)
		{
			Output("Shape '"  + shape.Text +"' deleted");
		}

		#endregion

		private void mnuClassReference_Click(object sender, System.EventArgs e)
		{
			try
			{
				Process.Start(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Lithium.chm");
			}
			catch
			{
				MessageBox.Show("The 'Lithium.chm' ships with this application but was not found in its original location.", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		

		
		#endregion
	}
}
