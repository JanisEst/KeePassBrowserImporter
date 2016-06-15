namespace KeePassBrowserImporter
{
	partial class ProviderForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.chromiumIconBox = new System.Windows.Forms.PictureBox();
			this.chromiumRadioButton = new System.Windows.Forms.RadioButton();
			this.operaIconBox = new System.Windows.Forms.PictureBox();
			this.operaRadioButton = new System.Windows.Forms.RadioButton();
			this.chromeIconBox = new System.Windows.Forms.PictureBox();
			this.chromeRadioButton = new System.Windows.Forms.RadioButton();
			this.internetExplorerIconBox = new System.Windows.Forms.PictureBox();
			this.internetExplorerRadioButton = new System.Windows.Forms.RadioButton();
			this.firefoxIconBox = new System.Windows.Forms.PictureBox();
			this.firefoxRadioButton = new System.Windows.Forms.RadioButton();
			this.profileGroupBox = new System.Windows.Forms.GroupBox();
			this.profilePathLabel = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.searchProfileButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.profileComboBox = new System.Windows.Forms.ComboBox();
			this.masterPasswordGroupBox = new System.Windows.Forms.GroupBox();
			this.masterPasswordTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.extractIconCheckBox = new System.Windows.Forms.CheckBox();
			this.extractTitleCheckBox = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupComboBox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chromiumIconBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.operaIconBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chromeIconBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.internetExplorerIconBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.firefoxIconBox)).BeginInit();
			this.profileGroupBox.SuspendLayout();
			this.masterPasswordGroupBox.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.chromiumIconBox);
			this.groupBox1.Controls.Add(this.chromiumRadioButton);
			this.groupBox1.Controls.Add(this.operaIconBox);
			this.groupBox1.Controls.Add(this.operaRadioButton);
			this.groupBox1.Controls.Add(this.chromeIconBox);
			this.groupBox1.Controls.Add(this.chromeRadioButton);
			this.groupBox1.Controls.Add(this.internetExplorerIconBox);
			this.groupBox1.Controls.Add(this.internetExplorerRadioButton);
			this.groupBox1.Controls.Add(this.firefoxIconBox);
			this.groupBox1.Controls.Add(this.firefoxRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(366, 153);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Browser";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(236, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Select the browser to import the credentials from.";
			// 
			// chromiumIconBox
			// 
			this.chromiumIconBox.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_Chromium;
			this.chromiumIconBox.Location = new System.Drawing.Point(9, 127);
			this.chromiumIconBox.Name = "chromiumIconBox";
			this.chromiumIconBox.Size = new System.Drawing.Size(16, 16);
			this.chromiumIconBox.TabIndex = 9;
			this.chromiumIconBox.TabStop = false;
			// 
			// chromiumRadioButton
			// 
			this.chromiumRadioButton.AutoSize = true;
			this.chromiumRadioButton.Location = new System.Drawing.Point(31, 127);
			this.chromiumRadioButton.Name = "chromiumRadioButton";
			this.chromiumRadioButton.Size = new System.Drawing.Size(71, 17);
			this.chromiumRadioButton.TabIndex = 8;
			this.chromiumRadioButton.TabStop = true;
			this.chromiumRadioButton.Text = "Chromium";
			this.chromiumRadioButton.UseVisualStyleBackColor = true;
			this.chromiumRadioButton.CheckedChanged += new System.EventHandler(this.SelectBrowser_CheckedChanged);
			// 
			// operaIconBox
			// 
			this.operaIconBox.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_Opera;
			this.operaIconBox.Location = new System.Drawing.Point(9, 104);
			this.operaIconBox.Name = "operaIconBox";
			this.operaIconBox.Size = new System.Drawing.Size(16, 16);
			this.operaIconBox.TabIndex = 7;
			this.operaIconBox.TabStop = false;
			// 
			// operaRadioButton
			// 
			this.operaRadioButton.AutoSize = true;
			this.operaRadioButton.Location = new System.Drawing.Point(31, 104);
			this.operaRadioButton.Name = "operaRadioButton";
			this.operaRadioButton.Size = new System.Drawing.Size(54, 17);
			this.operaRadioButton.TabIndex = 6;
			this.operaRadioButton.TabStop = true;
			this.operaRadioButton.Text = "Opera";
			this.operaRadioButton.UseVisualStyleBackColor = true;
			this.operaRadioButton.CheckedChanged += new System.EventHandler(this.SelectBrowser_CheckedChanged);
			// 
			// chromeIconBox
			// 
			this.chromeIconBox.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_Chrome;
			this.chromeIconBox.Location = new System.Drawing.Point(9, 81);
			this.chromeIconBox.Name = "chromeIconBox";
			this.chromeIconBox.Size = new System.Drawing.Size(16, 16);
			this.chromeIconBox.TabIndex = 5;
			this.chromeIconBox.TabStop = false;
			// 
			// chromeRadioButton
			// 
			this.chromeRadioButton.AutoSize = true;
			this.chromeRadioButton.Location = new System.Drawing.Point(31, 81);
			this.chromeRadioButton.Name = "chromeRadioButton";
			this.chromeRadioButton.Size = new System.Drawing.Size(150, 17);
			this.chromeRadioButton.TabIndex = 4;
			this.chromeRadioButton.TabStop = true;
			this.chromeRadioButton.Text = "Chrome / Chrome Portable";
			this.chromeRadioButton.UseVisualStyleBackColor = true;
			this.chromeRadioButton.CheckedChanged += new System.EventHandler(this.SelectBrowser_CheckedChanged);
			// 
			// internetExplorerIconBox
			// 
			this.internetExplorerIconBox.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_InternetExplorer;
			this.internetExplorerIconBox.Location = new System.Drawing.Point(9, 58);
			this.internetExplorerIconBox.Name = "internetExplorerIconBox";
			this.internetExplorerIconBox.Size = new System.Drawing.Size(16, 16);
			this.internetExplorerIconBox.TabIndex = 3;
			this.internetExplorerIconBox.TabStop = false;
			// 
			// internetExplorerRadioButton
			// 
			this.internetExplorerRadioButton.AutoSize = true;
			this.internetExplorerRadioButton.Location = new System.Drawing.Point(31, 58);
			this.internetExplorerRadioButton.Name = "internetExplorerRadioButton";
			this.internetExplorerRadioButton.Size = new System.Drawing.Size(138, 17);
			this.internetExplorerRadioButton.TabIndex = 2;
			this.internetExplorerRadioButton.TabStop = true;
			this.internetExplorerRadioButton.Text = "Internet Explorer / Edge";
			this.internetExplorerRadioButton.UseVisualStyleBackColor = true;
			this.internetExplorerRadioButton.CheckedChanged += new System.EventHandler(this.SelectBrowser_CheckedChanged);
			// 
			// firefoxIconBox
			// 
			this.firefoxIconBox.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_Firefox;
			this.firefoxIconBox.Location = new System.Drawing.Point(9, 35);
			this.firefoxIconBox.Name = "firefoxIconBox";
			this.firefoxIconBox.Size = new System.Drawing.Size(16, 16);
			this.firefoxIconBox.TabIndex = 1;
			this.firefoxIconBox.TabStop = false;
			// 
			// firefoxRadioButton
			// 
			this.firefoxRadioButton.AutoSize = true;
			this.firefoxRadioButton.Location = new System.Drawing.Point(31, 35);
			this.firefoxRadioButton.Name = "firefoxRadioButton";
			this.firefoxRadioButton.Size = new System.Drawing.Size(140, 17);
			this.firefoxRadioButton.TabIndex = 0;
			this.firefoxRadioButton.TabStop = true;
			this.firefoxRadioButton.Text = "Firefox / Firefox Portable";
			this.firefoxRadioButton.UseVisualStyleBackColor = true;
			this.firefoxRadioButton.CheckedChanged += new System.EventHandler(this.SelectBrowser_CheckedChanged);
			// 
			// profileGroupBox
			// 
			this.profileGroupBox.Controls.Add(this.profilePathLabel);
			this.profileGroupBox.Controls.Add(this.label6);
			this.profileGroupBox.Controls.Add(this.searchProfileButton);
			this.profileGroupBox.Controls.Add(this.label2);
			this.profileGroupBox.Controls.Add(this.profileComboBox);
			this.profileGroupBox.Location = new System.Drawing.Point(12, 171);
			this.profileGroupBox.Name = "profileGroupBox";
			this.profileGroupBox.Size = new System.Drawing.Size(366, 97);
			this.profileGroupBox.TabIndex = 1;
			this.profileGroupBox.TabStop = false;
			this.profileGroupBox.Text = "Profile";
			// 
			// profilePathLabel
			// 
			this.profilePathLabel.Location = new System.Drawing.Point(9, 76);
			this.profilePathLabel.Name = "profilePathLabel";
			this.profilePathLabel.Size = new System.Drawing.Size(348, 13);
			this.profilePathLabel.TabIndex = 4;
			this.profilePathLabel.Text = "<>";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 3;
			this.label6.Text = "Profile-Path:";
			// 
			// searchProfileButton
			// 
			this.searchProfileButton.Location = new System.Drawing.Point(294, 36);
			this.searchProfileButton.Name = "searchProfileButton";
			this.searchProfileButton.Size = new System.Drawing.Size(32, 21);
			this.searchProfileButton.TabIndex = 2;
			this.searchProfileButton.Text = "...";
			this.searchProfileButton.UseVisualStyleBackColor = true;
			this.searchProfileButton.Click += new System.EventHandler(this.searchProfileButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(218, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Select the browser profile you want to import.";
			// 
			// profileComboBox
			// 
			this.profileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.profileComboBox.FormattingEnabled = true;
			this.profileComboBox.Location = new System.Drawing.Point(9, 36);
			this.profileComboBox.Name = "profileComboBox";
			this.profileComboBox.Size = new System.Drawing.Size(279, 21);
			this.profileComboBox.TabIndex = 0;
			this.profileComboBox.SelectedIndexChanged += new System.EventHandler(this.profileComboBox_SelectedIndexChanged);
			// 
			// masterPasswordGroupBox
			// 
			this.masterPasswordGroupBox.Controls.Add(this.masterPasswordTextBox);
			this.masterPasswordGroupBox.Controls.Add(this.label3);
			this.masterPasswordGroupBox.Location = new System.Drawing.Point(12, 275);
			this.masterPasswordGroupBox.Name = "masterPasswordGroupBox";
			this.masterPasswordGroupBox.Size = new System.Drawing.Size(366, 66);
			this.masterPasswordGroupBox.TabIndex = 2;
			this.masterPasswordGroupBox.TabStop = false;
			this.masterPasswordGroupBox.Text = "Master Password";
			// 
			// masterPasswordTextBox
			// 
			this.masterPasswordTextBox.Location = new System.Drawing.Point(9, 35);
			this.masterPasswordTextBox.Name = "masterPasswordTextBox";
			this.masterPasswordTextBox.Size = new System.Drawing.Size(279, 20);
			this.masterPasswordTextBox.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(241, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "If you have set a master password provide it here.";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.extractIconCheckBox);
			this.groupBox2.Controls.Add(this.extractTitleCheckBox);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(12, 421);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(366, 94);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Settings";
			// 
			// extractIconCheckBox
			// 
			this.extractIconCheckBox.AutoSize = true;
			this.extractIconCheckBox.Checked = true;
			this.extractIconCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.extractIconCheckBox.Location = new System.Drawing.Point(12, 69);
			this.extractIconCheckBox.Name = "extractIconCheckBox";
			this.extractIconCheckBox.Size = new System.Drawing.Size(170, 17);
			this.extractIconCheckBox.TabIndex = 2;
			this.extractIconCheckBox.Text = "Use website icon as entry icon";
			this.extractIconCheckBox.UseVisualStyleBackColor = true;
			// 
			// extractTitleCheckBox
			// 
			this.extractTitleCheckBox.AutoSize = true;
			this.extractTitleCheckBox.Checked = true;
			this.extractTitleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.extractTitleCheckBox.Location = new System.Drawing.Point(12, 46);
			this.extractTitleCheckBox.Name = "extractTitleCheckBox";
			this.extractTitleCheckBox.Size = new System.Drawing.Size(162, 17);
			this.extractTitleCheckBox.TabIndex = 1;
			this.extractTitleCheckBox.Text = "Use website title as entry title";
			this.extractTitleCheckBox.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(348, 26);
			this.label4.TabIndex = 0;
			this.label4.Text = "Both settings need to connect to the webpage to extract the information.\r\nThis pr" +
    "ocess could take some time, be patient.";
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(303, 521);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 5;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			// 
			// startButton
			// 
			this.startButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.startButton.Image = global::KeePassBrowserImporter.Properties.Resources.B16x16_Apply;
			this.startButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.startButton.Location = new System.Drawing.Point(201, 521);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(96, 23);
			this.startButton.TabIndex = 4;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.groupComboBox);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Location = new System.Drawing.Point(12, 348);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(366, 66);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "KeePass";
			// 
			// groupComboBox
			// 
			this.groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.groupComboBox.FormattingEnabled = true;
			this.groupComboBox.Location = new System.Drawing.Point(9, 36);
			this.groupComboBox.Name = "groupComboBox";
			this.groupComboBox.Size = new System.Drawing.Size(279, 21);
			this.groupComboBox.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(277, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Select the group in which the entries should get imported.";
			// 
			// ProviderForm
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(389, 553);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.masterPasswordGroupBox);
			this.Controls.Add(this.profileGroupBox);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ProviderForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "KeePassBrowserImporter";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.chromiumIconBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.operaIconBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chromeIconBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.internetExplorerIconBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.firefoxIconBox)).EndInit();
			this.profileGroupBox.ResumeLayout(false);
			this.profileGroupBox.PerformLayout();
			this.masterPasswordGroupBox.ResumeLayout(false);
			this.masterPasswordGroupBox.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox firefoxIconBox;
		private System.Windows.Forms.RadioButton firefoxRadioButton;
		private System.Windows.Forms.PictureBox chromiumIconBox;
		private System.Windows.Forms.RadioButton chromiumRadioButton;
		private System.Windows.Forms.PictureBox operaIconBox;
		private System.Windows.Forms.RadioButton operaRadioButton;
		private System.Windows.Forms.PictureBox chromeIconBox;
		private System.Windows.Forms.RadioButton chromeRadioButton;
		private System.Windows.Forms.PictureBox internetExplorerIconBox;
		private System.Windows.Forms.RadioButton internetExplorerRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox profileGroupBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox profileComboBox;
		private System.Windows.Forms.GroupBox masterPasswordGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox masterPasswordTextBox;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox extractIconCheckBox;
		private System.Windows.Forms.CheckBox extractTitleCheckBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox groupComboBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button searchProfileButton;
		private System.Windows.Forms.Label profilePathLabel;
		private System.Windows.Forms.Label label6;
	}
}