// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddTemplateWizard.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.Generation;

    public class AddTemplateWizard : Form
    {
        private readonly Repository repository;

        private Panel titlePanel;

        private Panel buttonLineDarkPanel;
        private Panel buttonLineLightPanel;
        private Button cancelButton;
        private Button finishButton;
        private Template template;
        private Button helpButton;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Button outputLocationButton;
        private Label templateSourceLabel;
        private TextBox templateSourceTextBox;
        private Label titleExplanationLabel;
        private Label titleLabel;
        private Panel titleLineDarkPanel;
        private Panel titleLineLightPanel;
        
        public AddTemplateWizard(Repository repository)
        {
            this.InitializeComponent();

            this.repository = repository;
            this.nameTextBox.Focus();
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.templateSourceTextBox = new System.Windows.Forms.TextBox();
            this.templateSourceLabel = new System.Windows.Forms.Label();
            this.outputLocationButton = new System.Windows.Forms.Button();
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
            this.titlePanel.Size = new System.Drawing.Size(600, 72);
            this.titlePanel.TabIndex = 0;
            // 
            // titleExplanationLabel
            // 
            this.titleExplanationLabel.Location = new System.Drawing.Point(24, 32);
            this.titleExplanationLabel.Name = "titleExplanationLabel";
            this.titleExplanationLabel.Size = new System.Drawing.Size(480, 16);
            this.titleExplanationLabel.TabIndex = 2;
            this.titleExplanationLabel.Text = "This wizard adds a template to your repository.";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(16, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(488, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Welcome to the Allors Add Template Wizard";
            // 
            // buttonLineDarkPanel
            // 
            this.buttonLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonLineDarkPanel.Location = new System.Drawing.Point(0, 188);
            this.buttonLineDarkPanel.Name = "buttonLineDarkPanel";
            this.buttonLineDarkPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineDarkPanel.TabIndex = 1;
            // 
            // buttonLineLightPanel
            // 
            this.buttonLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLineLightPanel.Location = new System.Drawing.Point(0, 189);
            this.buttonLineLightPanel.Name = "buttonLineLightPanel";
            this.buttonLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineLightPanel.TabIndex = 15;
            // 
            // titleLineLightPanel
            // 
            this.titleLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLineLightPanel.Location = new System.Drawing.Point(0, 74);
            this.titleLineLightPanel.Name = "titleLineLightPanel";
            this.titleLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.titleLineLightPanel.TabIndex = 9;
            // 
            // titleLineDarkPanel
            // 
            this.titleLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.titleLineDarkPanel.Location = new System.Drawing.Point(0, 73);
            this.titleLineDarkPanel.Name = "titleLineDarkPanel";
            this.titleLineDarkPanel.Size = new System.Drawing.Size(604, 1);
            this.titleLineDarkPanel.TabIndex = 3;
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.finishButton.Location = new System.Drawing.Point(348, 205);
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
            this.cancelButton.Location = new System.Drawing.Point(428, 205);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(508, 205);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 22);
            this.helpButton.TabIndex = 8;
            this.helpButton.Text = "Help";
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(15, 80);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(112, 16);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(15, 100);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(568, 20);
            this.nameTextBox.TabIndex = 0;
            // 
            // templateSourceTextBox
            // 
            this.templateSourceTextBox.Location = new System.Drawing.Point(15, 142);
            this.templateSourceTextBox.Name = "templateSourceTextBox";
            this.templateSourceTextBox.Size = new System.Drawing.Size(489, 20);
            this.templateSourceTextBox.TabIndex = 3;
            // 
            // templateSourceLabel
            // 
            this.templateSourceLabel.Location = new System.Drawing.Point(15, 123);
            this.templateSourceLabel.Name = "templateSourceLabel";
            this.templateSourceLabel.Size = new System.Drawing.Size(128, 16);
            this.templateSourceLabel.TabIndex = 13;
            this.templateSourceLabel.Text = "Template Source:";
            // 
            // outputLocationButton
            // 
            this.outputLocationButton.Location = new System.Drawing.Point(510, 142);
            this.outputLocationButton.Name = "outputLocationButton";
            this.outputLocationButton.Size = new System.Drawing.Size(73, 23);
            this.outputLocationButton.TabIndex = 4;
            this.outputLocationButton.Text = "browse";
            this.outputLocationButton.Click += new System.EventHandler(this.TemplateSourceButtonClick);
            // 
            // AddTemplateWizard
            // 
            this.AcceptButton = this.finishButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(600, 238);
            this.Controls.Add(this.outputLocationButton);
            this.Controls.Add(this.templateSourceLabel);
            this.Controls.Add(this.templateSourceTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.titleLineLightPanel);
            this.Controls.Add(this.titleLineDarkPanel);
            this.Controls.Add(this.buttonLineLightPanel);
            this.Controls.Add(this.buttonLineDarkPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "AddTemplateWizard";
            this.Text = "Allors Add Template Wizard";
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TemplateSourceButtonClick(object sender, EventArgs e)
        {
            var templateSourceFileDialog = new OpenFileDialog { Filter = StringTemplate.FileFilter };
            if (templateSourceFileDialog.ShowDialog().Equals(DialogResult.OK))
            {
                var fileInfo = new FileInfo(templateSourceFileDialog.FileName);
                this.templateSourceTextBox.Text = new Uri(fileInfo.FullName).ToString();
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FinishButtonClick(object sender, EventArgs eventArgs)
        {
            var name = this.nameTextBox.Text.Trim();
            var templateSource = this.templateSourceTextBox.Text.Trim();

            if (name.Length == 0)
            {
                MessageBox.Show("Name is required.", "Allors Add Template Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.nameTextBox.Focus();
                return;
            }

            if (templateSource.Length == 0)
            {
                MessageBox.Show("Template Source is required.", "Allors Add Template Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.templateSourceTextBox.Focus();
                return;
            }

            Uri templateSourceUri;
            try
            {
                templateSourceUri = new Uri(templateSource);
            }
            catch (Exception e)
            {
                MessageBox.Show("Template Source is an illegal URL.\n\n" + e.Message, "Allors Add Template Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.templateSourceTextBox.Focus();
                return;
            }

            if (!StringTemplate.IsValid(templateSourceUri))
            {
                MessageBox.Show("Template Source is not valid.", "Allors Add Template Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.templateSourceTextBox.Focus();
                return;
            }

            try
            {
                this.template = this.repository.AddTemplate();
                this.template.Name = name;
                this.template.Source = templateSourceUri;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not create Template.\n\n" + e.Message, "Allors Add Template Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.template.Delete();
                return;
            }

            this.Close();
        }
    }
}