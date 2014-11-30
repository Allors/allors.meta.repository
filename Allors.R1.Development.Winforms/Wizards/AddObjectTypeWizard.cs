// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddObjectTypeWizard.cs" company="Allors bvba">
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
    using System.Windows.Forms;

    using Allors.R1.Meta;
    using Allors.R1.Meta.AllorsGenerated;

    public class AddObjectTypeWizard : Form
    {
        private readonly Domain domain;
        
        private Panel titlePanel;
        private Panel buttonLineDarkPanel;
        private Panel buttonLineLightPanel;
        private Button cancelButton;

        private Button finishButton;
        private Button helpButton;
        private ObjectType objectType;
        private Label pluralNameLabel;
        private TextBox pluralNameTextBox;
        private Label singularNameLabel;
        private TextBox singularNameTextBox;
        private Label titleExplanationLabel;
        private Label titleLabel;
        private Panel titleLineDarkPanel;
        private Panel titleLineLightPanel;

        public AddObjectTypeWizard(Domain domain)
        {
            this.InitializeComponent();

            this.domain = domain;

            this.singularNameTextBox.Focus();
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
            this.singularNameLabel = new System.Windows.Forms.Label();
            this.singularNameTextBox = new System.Windows.Forms.TextBox();
            this.pluralNameTextBox = new System.Windows.Forms.TextBox();
            this.pluralNameLabel = new System.Windows.Forms.Label();
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
            this.titleExplanationLabel.Text = "This wizard adds a new object type to your domain.";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(16, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(488, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Welcome to the Allors Add ObjectType Wizard";
            // 
            // buttonLineDarkPanel
            // 
            this.buttonLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonLineDarkPanel.Location = new System.Drawing.Point(0, 193);
            this.buttonLineDarkPanel.Name = "buttonLineDarkPanel";
            this.buttonLineDarkPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineDarkPanel.TabIndex = 1;
            // 
            // buttonLineLightPanel
            // 
            this.buttonLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLineLightPanel.Location = new System.Drawing.Point(0, 194);
            this.buttonLineLightPanel.Name = "buttonLineLightPanel";
            this.buttonLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineLightPanel.TabIndex = 12;
            // 
            // titleLineLightPanel
            // 
            this.titleLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLineLightPanel.Location = new System.Drawing.Point(0, 74);
            this.titleLineLightPanel.Name = "titleLineLightPanel";
            this.titleLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.titleLineLightPanel.TabIndex = 7;
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
            this.finishButton.Location = new System.Drawing.Point(348, 210);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 22);
            this.finishButton.TabIndex = 4;
            this.finishButton.Text = "Finish";
            this.finishButton.Click += new System.EventHandler(this.FinishButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(428, 210);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(508, 210);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 22);
            this.helpButton.TabIndex = 6;
            this.helpButton.Text = "Help";
            this.helpButton.Click += new System.EventHandler(this.HelpButtonClick);
            // 
            // singularNameLabel
            // 
            this.singularNameLabel.Location = new System.Drawing.Point(16, 88);
            this.singularNameLabel.Name = "singularNameLabel";
            this.singularNameLabel.Size = new System.Drawing.Size(112, 16);
            this.singularNameLabel.TabIndex = 8;
            this.singularNameLabel.Text = "Singular name:";
            // 
            // singularNameTextBox
            // 
            this.singularNameTextBox.Location = new System.Drawing.Point(16, 104);
            this.singularNameTextBox.Name = "singularNameTextBox";
            this.singularNameTextBox.Size = new System.Drawing.Size(567, 22);
            this.singularNameTextBox.TabIndex = 0;
            // 
            // pluralNameTextBox
            // 
            this.pluralNameTextBox.Location = new System.Drawing.Point(16, 148);
            this.pluralNameTextBox.Name = "pluralNameTextBox";
            this.pluralNameTextBox.Size = new System.Drawing.Size(567, 22);
            this.pluralNameTextBox.TabIndex = 1;
            // 
            // pluralNameLabel
            // 
            this.pluralNameLabel.Location = new System.Drawing.Point(16, 129);
            this.pluralNameLabel.Name = "pluralNameLabel";
            this.pluralNameLabel.Size = new System.Drawing.Size(112, 16);
            this.pluralNameLabel.TabIndex = 9;
            this.pluralNameLabel.Text = "Plural name:";
            // 
            // AddObjectTypeWizard
            // 
            this.AcceptButton = this.finishButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(600, 243);
            this.Controls.Add(this.pluralNameLabel);
            this.Controls.Add(this.pluralNameTextBox);
            this.Controls.Add(this.singularNameTextBox);
            this.Controls.Add(this.singularNameLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.titleLineLightPanel);
            this.Controls.Add(this.titleLineDarkPanel);
            this.Controls.Add(this.buttonLineLightPanel);
            this.Controls.Add(this.buttonLineDarkPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "AddObjectTypeWizard";
            this.Text = "Allors Add ObjectType Wizard";
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FinishButtonClick(object sender, EventArgs e)
        {
            this.objectType = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            var singularName = this.singularNameTextBox.Text.Trim();
            this.objectType.SingularName = singularName.Length > 0 ? singularName : null;
            if (this.pluralNameTextBox.Text.Trim().Length > 0)
            {
                this.objectType.PluralName = this.pluralNameTextBox.Text.Trim();
            }

            var validationReport = this.domain.Validate();
            if (!validationReport.ContainsErrors)
            {
                this.objectType.SendChangedEvent();
                this.domain.SendChangedEvent();
                this.Close();
            }
            else
            {
                this.objectType.Delete();
                this.domain.SendChangedEvent();

                MessageBox.Show(string.Join("\n", validationReport.Messages), "Allors Add ObjectType Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                foreach (var validationError in validationReport.Errors)
                {
                    foreach (var member in validationError.Members)
                    {
                        if (member.Equals(AllorsEmbeddedDomain.ObjectTypeSingularName))
                        {
                            this.singularNameTextBox.Focus();
                        }
                    }
                }
            }
        }

        private void HelpButtonClick(object sender, EventArgs e)
        {
        }
    }
}