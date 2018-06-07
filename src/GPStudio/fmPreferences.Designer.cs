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
	partial class fmPreferences
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbDatabase = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.txtInternetName = new System.Windows.Forms.TextBox();
			this.txtFriendlyName = new System.Windows.Forms.TextBox();
			this.lvServers = new ListViewEx.ListViewEx();
			this.colEnabled = new System.Windows.Forms.ColumnHeader();
			this.colDescription = new System.Windows.Forms.ColumnHeader();
			this.colInternetName = new System.Windows.Forms.ColumnHeader();
			this.colPort = new System.Windows.Forms.ColumnHeader();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.odDatabase = new System.Windows.Forms.OpenFileDialog();
			this.colProtocol = new System.Windows.Forms.ColumnHeader();
			this.cmbProtocol = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lbDatabase);
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(467, 72);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Database";
			// 
			// lbDatabase
			// 
			this.lbDatabase.AutoEllipsis = true;
			this.lbDatabase.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.lbDatabase.Location = new System.Drawing.Point(6, 17);
			this.lbDatabase.Name = "lbDatabase";
			this.lbDatabase.Size = new System.Drawing.Size(455, 23);
			this.lbDatabase.TabIndex = 1;
			this.lbDatabase.Text = "<Database>";
			this.lbDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(6, 43);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 0;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmbProtocol);
			this.groupBox2.Controls.Add(this.txtPort);
			this.groupBox2.Controls.Add(this.txtInternetName);
			this.groupBox2.Controls.Add(this.txtFriendlyName);
			this.groupBox2.Controls.Add(this.lvServers);
			this.groupBox2.Controls.Add(this.btnTest);
			this.groupBox2.Controls.Add(this.btnRemove);
			this.groupBox2.Controls.Add(this.btnAdd);
			this.groupBox2.Location = new System.Drawing.Point(12, 90);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(467, 292);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Distributed Servers";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(333, 209);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(51, 20);
			this.txtPort.TabIndex = 7;
			this.txtPort.Visible = false;
			// 
			// txtInternetName
			// 
			this.txtInternetName.Location = new System.Drawing.Point(211, 209);
			this.txtInternetName.Name = "txtInternetName";
			this.txtInternetName.Size = new System.Drawing.Size(100, 20);
			this.txtInternetName.TabIndex = 6;
			this.txtInternetName.Visible = false;
			// 
			// txtFriendlyName
			// 
			this.txtFriendlyName.Location = new System.Drawing.Point(91, 209);
			this.txtFriendlyName.Name = "txtFriendlyName";
			this.txtFriendlyName.Size = new System.Drawing.Size(100, 20);
			this.txtFriendlyName.TabIndex = 5;
			this.txtFriendlyName.Visible = false;
			// 
			// lvServers
			// 
			this.lvServers.AllowColumnReorder = true;
			this.lvServers.CheckBoxes = true;
			this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEnabled,
            this.colDescription,
            this.colInternetName,
            this.colPort,
            this.colProtocol});
			this.lvServers.DoubleClickActivation = false;
			this.lvServers.FullRowSelect = true;
			this.lvServers.GridLines = true;
			this.lvServers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvServers.HideSelection = false;
			this.lvServers.Location = new System.Drawing.Point(10, 19);
			this.lvServers.MultiSelect = false;
			this.lvServers.Name = "lvServers";
			this.lvServers.Size = new System.Drawing.Size(451, 235);
			this.lvServers.TabIndex = 4;
			this.lvServers.UseCompatibleStateImageBehavior = false;
			this.lvServers.View = System.Windows.Forms.View.Details;
			this.lvServers.SubItemClicked += new ListViewEx.SubItemEventHandler(this.lvServers_SubItemClicked);
			this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);
			// 
			// colEnabled
			// 
			this.colEnabled.Text = "Enabled";
			// 
			// colDescription
			// 
			this.colDescription.Text = "Description";
			this.colDescription.Width = 125;
			// 
			// colInternetName
			// 
			this.colInternetName.Text = "Internet Name";
			this.colInternetName.Width = 125;
			// 
			// colPort
			// 
			this.colPort.Text = "Port";
			// 
			// btnTest
			// 
			this.btnTest.Enabled = false;
			this.btnTest.Location = new System.Drawing.Point(172, 260);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 3;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Enabled = false;
			this.btnRemove.Location = new System.Drawing.Point(91, 260);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.button4_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(10, 260);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(297, 388);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Accept";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(378, 388);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Cancel";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// odDatabase
			// 
			this.odDatabase.DefaultExt = "*.mdb";
			this.odDatabase.Filter = "MS Access Files (*.mdb)|*.mdb";
			this.odDatabase.Title = "Select GPStudio Database";
			// 
			// colProtocol
			// 
			this.colProtocol.Text = "Protocol";
			// 
			// cmbProtocol
			// 
			this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProtocol.FormattingEnabled = true;
			this.cmbProtocol.Items.AddRange(new object[] {
            "Tcp",
            "Http"});
			this.cmbProtocol.Location = new System.Drawing.Point(390, 208);
			this.cmbProtocol.Name = "cmbProtocol";
			this.cmbProtocol.Size = new System.Drawing.Size(52, 21);
			this.cmbProtocol.TabIndex = 8;
			this.cmbProtocol.Visible = false;
			// 
			// fmPreferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(491, 417);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "fmPreferences";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Program Preferences";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lbDatabase;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.OpenFileDialog odDatabase;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private ListViewEx.ListViewEx lvServers;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.ColumnHeader colInternetName;
		private System.Windows.Forms.ColumnHeader colPort;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.TextBox txtInternetName;
		private System.Windows.Forms.TextBox txtFriendlyName;
		private System.Windows.Forms.ColumnHeader colEnabled;
		private System.Windows.Forms.ColumnHeader colProtocol;
		private System.Windows.Forms.ComboBox cmbProtocol;
	}
}