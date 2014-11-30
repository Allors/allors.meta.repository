// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddRepositoryWizard.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms.Wizards
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using Allors.R1.Development.Repository;

    public class AddRepositoryWizard : Form
    {
        private Label titleLabel;

        private Button browseButton;
        private Panel buttonLineDarkPanel;
        private Panel buttonLineLightPanel;
        private Button cancelButton;

        private TextBox directoryTextBox;
        private Label domainNameLabel;
        private Button finishButton;
        private Button helpButton;
        private TextBox nameTextBox;
        private Label outputDirectoryLabel;
        private Repository repository;
        private Label titleExplanationLabel;
        private Panel titleLineDarkPanel;
        private Panel titleLineLightPanel;
        private Panel titlePanel;

        public AddRepositoryWizard()
        {
            this.InitializeComponent();
            this.nameTextBox.Focus();
        }

        public Repository Repository
        {
            get { return this.repository; }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titlePanel = new System.Windows.Forms.Panel();
            this.titleExplanationLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.buttonLineDarkPanel = new System.Windows.Forms.Panel();
            this.buttonLineLightPanel = new System.Windows.Forms.Panel();
            this.titleLineLightPanel = new System.Windows.Forms.Panel();
            this.titleLineDarkPanel = new System.Windows.Forms.Panel();
            this.finishButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.domainNameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.directoryTextBox = new System.Windows.Forms.TextBox();
            this.outputDirectoryLabel = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.Controls.Add(this.titleExplanationLabel);
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(590, 72);
            this.titlePanel.TabIndex = 0;
            // 
            // titleExplanationLabel
            // 
            this.titleExplanationLabel.Location = new System.Drawing.Point(24, 32);
            this.titleExplanationLabel.Name = "titleExplanationLabel";
            this.titleExplanationLabel.Size = new System.Drawing.Size(480, 16);
            this.titleExplanationLabel.TabIndex = 2;
            this.titleExplanationLabel.Text = "This wizard adds a new repository.";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(16, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(488, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Welcome to the Allors Add Repository Wizard";
            // 
            // buttonLineDarkPanel
            // 
            this.buttonLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonLineDarkPanel.Location = new System.Drawing.Point(0, 185);
            this.buttonLineDarkPanel.Name = "buttonLineDarkPanel";
            this.buttonLineDarkPanel.Size = new System.Drawing.Size(594, 1);
            this.buttonLineDarkPanel.TabIndex = 1;
            // 
            // buttonLineLightPanel
            // 
            this.buttonLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLineLightPanel.Location = new System.Drawing.Point(0, 186);
            this.buttonLineLightPanel.Name = "buttonLineLightPanel";
            this.buttonLineLightPanel.Size = new System.Drawing.Size(594, 1);
            this.buttonLineLightPanel.TabIndex = 15;
            // 
            // titleLineLightPanel
            // 
            this.titleLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLineLightPanel.Location = new System.Drawing.Point(0, 74);
            this.titleLineLightPanel.Name = "titleLineLightPanel";
            this.titleLineLightPanel.Size = new System.Drawing.Size(594, 1);
            this.titleLineLightPanel.TabIndex = 9;
            // 
            // titleLineDarkPanel
            // 
            this.titleLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.titleLineDarkPanel.Location = new System.Drawing.Point(0, 73);
            this.titleLineDarkPanel.Name = "titleLineDarkPanel";
            this.titleLineDarkPanel.Size = new System.Drawing.Size(594, 1);
            this.titleLineDarkPanel.TabIndex = 3;
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Location = new System.Drawing.Point(338, 202);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 22);
            this.finishButton.TabIndex = 6;
            this.finishButton.Text = "Finish";
            this.finishButton.Click += new System.EventHandler(this.FinishButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(418, 202);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(498, 202);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 22);
            this.helpButton.TabIndex = 8;
            this.helpButton.Text = "Help";
            this.helpButton.Click += new System.EventHandler(this.HelpButtonClick);
            // 
            // domainNameLabel
            // 
            this.domainNameLabel.Location = new System.Drawing.Point(15, 80);
            this.domainNameLabel.Name = "domainNameLabel";
            this.domainNameLabel.Size = new System.Drawing.Size(112, 16);
            this.domainNameLabel.TabIndex = 10;
            this.domainNameLabel.Text = "Domain name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(15, 100);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(556, 20);
            this.nameTextBox.TabIndex = 0;
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Location = new System.Drawing.Point(15, 142);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(477, 20);
            this.directoryTextBox.TabIndex = 3;
            // 
            // outputDirectoryLabel
            // 
            this.outputDirectoryLabel.Location = new System.Drawing.Point(15, 123);
            this.outputDirectoryLabel.Name = "outputDirectoryLabel";
            this.outputDirectoryLabel.Size = new System.Drawing.Size(112, 16);
            this.outputDirectoryLabel.TabIndex = 13;
            this.outputDirectoryLabel.Text = "Directory:";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(498, 142);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(73, 23);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "browse";
            this.browseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // AddRepositoryWizard
            // 
            this.AcceptButton = this.finishButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(590, 235);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.outputDirectoryLabel);
            this.Controls.Add(this.directoryTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.domainNameLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.titleLineLightPanel);
            this.Controls.Add(this.titleLineDarkPanel);
            this.Controls.Add(this.buttonLineLightPanel);
            this.Controls.Add(this.buttonLineDarkPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "AddRepositoryWizard";
            this.Text = "Allors Add Repository Wizard";
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BrowseButtonClick(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog
                                          {
                                              Description = "Select the location where to create the Allors Repository",
                                              ShowNewFolderButton = true
                                          };

            if (folderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            {
                var directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                this.directoryTextBox.Text = directoryInfo.FullName;
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FinishButtonClick(object sender, EventArgs e)
        {
            var name = this.nameTextBox.Text.Trim();

            DirectoryInfo directoryInfo;
            try
            {
                directoryInfo = new DirectoryInfo(this.directoryTextBox.Text);
            }
            catch
            {
                directoryInfo = null;
            }

            if (name.Length == 0)
            {
                MessageBox.Show("Name is required", "Allors Add Repository Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.nameTextBox.Focus();
            }
            else if (directoryInfo == null || !directoryInfo.Exists)
            {
                MessageBox.Show("Directory does not exist", "Allors Add Repository Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.directoryTextBox.Focus();
            }
            else
            {
                this.repository = new Repository(directoryInfo, true);
                this.repository.Domain.Name = name;
                this.repository.Domain.SendChangedEvent();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void HelpButtonClick(object sender, EventArgs e)
        {
        }
    }
}