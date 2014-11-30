// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PushDownObjectTypeWizard.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
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

    using Allors.R1.Development.Repository.Commands;
    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    public class PushDownObjectTypeWizard : Form
    {
        private readonly Repository repository;
        private readonly ObjectType objectType;

        private readonly PushDownObjectType pushDown;

        private Panel buttonLineDarkPanel;
        private Panel buttonLineLightPanel;

        private Button cancelButton;
        private Button finishButton;
        private Button helpButton;

        private Label titleExplanationLabel;
        private Label titleLabel;
        private Panel titleLineDarkPanel;
        private Panel titleLineLightPanel;
        private Panel titlePanel;
        private TextBox namespaceTextBox;
        private Label label1;
        private RichTextBox dependencyRichTextBox;
        private Label namespaceLabel;

        public PushDownObjectTypeWizard(Repository repository, ObjectType objectType)
        {
            this.InitializeComponent();

            this.repository = repository;
            this.objectType = objectType;

            this.namespaceTextBox.Text = objectType.ToString();

            this.pushDown = this.repository.PushDown(this.objectType);
            this.dependencyRichTextBox.UpdateDependencies(this.pushDown);
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FinishButtonClick(object sender, EventArgs eventArgs)
        {
            this.pushDown.Execute();

            this.Close();
        }

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
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
            this.namespaceLabel = new System.Windows.Forms.Label();
            this.namespaceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dependencyRichTextBox = new System.Windows.Forms.RichTextBox();
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
            this.titlePanel.Size = new System.Drawing.Size(599, 72);
            this.titlePanel.TabIndex = 0;
            // 
            // titleExplanationLabel
            // 
            this.titleExplanationLabel.Location = new System.Drawing.Point(24, 32);
            this.titleExplanationLabel.Name = "titleExplanationLabel";
            this.titleExplanationLabel.Size = new System.Drawing.Size(480, 16);
            this.titleExplanationLabel.TabIndex = 2;
            this.titleExplanationLabel.Text = "This wizard allows you to push an object type down to the domain.";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(16, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(488, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Welcome to the Allors Push Object Type Down Wizard";
            // 
            // buttonLineDarkPanel
            // 
            this.buttonLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonLineDarkPanel.Location = new System.Drawing.Point(0, 380);
            this.buttonLineDarkPanel.Name = "buttonLineDarkPanel";
            this.buttonLineDarkPanel.Size = new System.Drawing.Size(603, 1);
            this.buttonLineDarkPanel.TabIndex = 1;
            // 
            // buttonLineLightPanel
            // 
            this.buttonLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLineLightPanel.Location = new System.Drawing.Point(0, 381);
            this.buttonLineLightPanel.Name = "buttonLineLightPanel";
            this.buttonLineLightPanel.Size = new System.Drawing.Size(603, 1);
            this.buttonLineLightPanel.TabIndex = 15;
            // 
            // titleLineLightPanel
            // 
            this.titleLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLineLightPanel.Location = new System.Drawing.Point(0, 74);
            this.titleLineLightPanel.Name = "titleLineLightPanel";
            this.titleLineLightPanel.Size = new System.Drawing.Size(603, 1);
            this.titleLineLightPanel.TabIndex = 9;
            // 
            // titleLineDarkPanel
            // 
            this.titleLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.titleLineDarkPanel.Location = new System.Drawing.Point(0, 73);
            this.titleLineDarkPanel.Name = "titleLineDarkPanel";
            this.titleLineDarkPanel.Size = new System.Drawing.Size(603, 1);
            this.titleLineDarkPanel.TabIndex = 3;
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.finishButton.Location = new System.Drawing.Point(347, 397);
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
            this.cancelButton.Location = new System.Drawing.Point(427, 397);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(507, 397);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 22);
            this.helpButton.TabIndex = 8;
            this.helpButton.Text = "Help";
            // 
            // namespaceLabel
            // 
            this.namespaceLabel.Location = new System.Drawing.Point(15, 80);
            this.namespaceLabel.Name = "namespaceLabel";
            this.namespaceLabel.Size = new System.Drawing.Size(128, 16);
            this.namespaceLabel.TabIndex = 10;
            this.namespaceLabel.Text = "Namespace:";
            // 
            // namespaceTextBox
            // 
            this.namespaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namespaceTextBox.Enabled = false;
            this.namespaceTextBox.Location = new System.Drawing.Point(15, 99);
            this.namespaceTextBox.Name = "namespaceTextBox";
            this.namespaceTextBox.Size = new System.Drawing.Size(567, 20);
            this.namespaceTextBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 29;
            this.label1.Text = "Dependencies:";
            // 
            // dependencyRichTextBox
            // 
            this.dependencyRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dependencyRichTextBox.Location = new System.Drawing.Point(15, 144);
            this.dependencyRichTextBox.Name = "dependencyRichTextBox";
            this.dependencyRichTextBox.Size = new System.Drawing.Size(567, 216);
            this.dependencyRichTextBox.TabIndex = 28;
            this.dependencyRichTextBox.Text = "";
            // 
            // PushDownObjectTypeWizard
            // 
            this.AcceptButton = this.finishButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(599, 430);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dependencyRichTextBox);
            this.Controls.Add(this.namespaceTextBox);
            this.Controls.Add(this.namespaceLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.titleLineLightPanel);
            this.Controls.Add(this.titleLineDarkPanel);
            this.Controls.Add(this.buttonLineLightPanel);
            this.Controls.Add(this.buttonLineDarkPanel);
            this.Controls.Add(this.titlePanel);
            this.Name = "PushDownObjectTypeWizard";
            this.Text = "Allors Push Object Type Down Wizard";
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}