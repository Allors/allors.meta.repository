// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddRelationTypeWizard.cs" company="Allors bvba">
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
    using System.Collections;
   using System.ComponentModel;
    using System.Windows.Forms;

    using Allors.R1.Meta;

    public class AddRelationTypeWizard : Form
    {
        private const string Many2Many = "Many to Many";
        private const string Many2One = "Many to One";
        private const string One2Many = "One to Many";
        private const string One2One = "One to One";
        
        private readonly Domain domain;

        private Panel titlePanel;
        private Label associationLabel;
        private Label associationPluralNameLabel;
        private TextBox associationPluralNameTextBox;
        private Label associationSingularNameLabel;
        private TextBox associationSingularNameTextBox;
        private ComboBox associationTypeComboBox;
        private Label associationTypeLabel;
        private Panel buttonLineDarkPanel;
        private Panel buttonLineLightPanel;
        private Button cancelButton;
        private Button finishButton;
        private Button helpButton;
        private CheckBox isIndexedCheckBox;
        private ComboBox multiplicityComboBox;
        private Label multiplicityExplanationLabel;
        private TextBox multiplicityExplanationTextBox;
        private Label multiplicityLabel;
        private ObjectType previouslySelectedAssociationType;
        private ObjectType previouslySelectedRoleType;
        private Label relationLabel;

        private Label roleLabel;
        private Label rolePluralNameLabel;
        private TextBox rolePluralNameTextBox;
        private Label roleSingularNameLabel;
        private TextBox roleSingularNameTextBox;
        private ComboBox roleTypeComboBox;
        private Label roleTypeLabel;
        private Label titleExplanationLabel;
        private Label titleLabel;
        private Panel titleLineDarkPanel;
        private Panel titleLineLightPanel;

        private RelationType relationType;

        public AddRelationTypeWizard(ObjectType associationObjectType)
        {
            this.InitializeComponent();

            this.domain = Domain.GetDomain(associationObjectType);

            this.multiplicityComboBox.Items.Add(One2One);
            this.multiplicityComboBox.Items.Add(One2Many);
            this.multiplicityComboBox.Items.Add(Many2One);
            this.multiplicityComboBox.Items.Add(Many2Many);

            var associationClasses = new ArrayList(this.domain.CompositeObjectTypes);
            associationClasses.Sort();
            this.associationTypeComboBox.DataSource = associationClasses;
            this.associationTypeComboBox.SelectedItem = associationObjectType;

            var roleClasses = new ArrayList(this.domain.ObjectTypes);
            roleClasses.Sort();
            this.roleTypeComboBox.DataSource = roleClasses;
            this.roleTypeComboBox.SelectedItem = this.domain.Domain.Find(UnitTypeIds.StringId);

            this.associationSingularNameTextBox.Focus();
        }

        private ObjectType SelectedAssociationType
        {
            get { return (ObjectType)this.associationTypeComboBox.SelectedItem; }
        }

        private ObjectType SelectedRoleType
        {
            get { return (ObjectType)this.roleTypeComboBox.SelectedItem; }
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
            this.associationSingularNameLabel = new System.Windows.Forms.Label();
            this.associationSingularNameTextBox = new System.Windows.Forms.TextBox();
            this.associationPluralNameTextBox = new System.Windows.Forms.TextBox();
            this.associationPluralNameLabel = new System.Windows.Forms.Label();
            this.associationLabel = new System.Windows.Forms.Label();
            this.associationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.associationTypeLabel = new System.Windows.Forms.Label();
            this.roleLabel = new System.Windows.Forms.Label();
            this.rolePluralNameLabel = new System.Windows.Forms.Label();
            this.roleTypeLabel = new System.Windows.Forms.Label();
            this.roleTypeComboBox = new System.Windows.Forms.ComboBox();
            this.rolePluralNameTextBox = new System.Windows.Forms.TextBox();
            this.roleSingularNameTextBox = new System.Windows.Forms.TextBox();
            this.roleSingularNameLabel = new System.Windows.Forms.Label();
            this.relationLabel = new System.Windows.Forms.Label();
            this.multiplicityLabel = new System.Windows.Forms.Label();
            this.multiplicityComboBox = new System.Windows.Forms.ComboBox();
            this.multiplicityExplanationTextBox = new System.Windows.Forms.TextBox();
            this.multiplicityExplanationLabel = new System.Windows.Forms.Label();
            this.isIndexedCheckBox = new System.Windows.Forms.CheckBox();
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
            this.titleExplanationLabel.Text = "This wizard adds a relation to your domain.";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(16, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(488, 16);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Welcome to the Allors Add RelationType Wizard";
            // 
            // buttonLineDarkPanel
            // 
            this.buttonLineDarkPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineDarkPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonLineDarkPanel.Location = new System.Drawing.Point(0, 274);
            this.buttonLineDarkPanel.Name = "buttonLineDarkPanel";
            this.buttonLineDarkPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineDarkPanel.TabIndex = 1;
            // 
            // buttonLineLightPanel
            // 
            this.buttonLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLineLightPanel.Location = new System.Drawing.Point(0, 275);
            this.buttonLineLightPanel.Name = "buttonLineLightPanel";
            this.buttonLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.buttonLineLightPanel.TabIndex = 25;
            // 
            // titleLineLightPanel
            // 
            this.titleLineLightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLineLightPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.titleLineLightPanel.Location = new System.Drawing.Point(0, 74);
            this.titleLineLightPanel.Name = "titleLineLightPanel";
            this.titleLineLightPanel.Size = new System.Drawing.Size(604, 1);
            this.titleLineLightPanel.TabIndex = 11;
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
            this.finishButton.Location = new System.Drawing.Point(348, 291);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 22);
            this.finishButton.TabIndex = 8;
            this.finishButton.Text = "Finish";
            this.finishButton.Click += new System.EventHandler(this.FinishButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(428, 291);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 22);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.Location = new System.Drawing.Point(508, 291);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 22);
            this.helpButton.TabIndex = 10;
            this.helpButton.Text = "Help";
            // 
            // associationSingularNameLabel
            // 
            this.associationSingularNameLabel.Location = new System.Drawing.Point(207, 91);
            this.associationSingularNameLabel.Name = "associationSingularNameLabel";
            this.associationSingularNameLabel.Size = new System.Drawing.Size(138, 16);
            this.associationSingularNameLabel.TabIndex = 14;
            this.associationSingularNameLabel.Text = "Singular name:";
            // 
            // associationSingularNameTextBox
            // 
            this.associationSingularNameTextBox.Location = new System.Drawing.Point(207, 107);
            this.associationSingularNameTextBox.Name = "associationSingularNameTextBox";
            this.associationSingularNameTextBox.Size = new System.Drawing.Size(184, 20);
            this.associationSingularNameTextBox.TabIndex = 1;
            // 
            // associationPluralNameTextBox
            // 
            this.associationPluralNameTextBox.Location = new System.Drawing.Point(399, 107);
            this.associationPluralNameTextBox.Name = "associationPluralNameTextBox";
            this.associationPluralNameTextBox.Size = new System.Drawing.Size(184, 20);
            this.associationPluralNameTextBox.TabIndex = 2;
            // 
            // associationPluralNameLabel
            // 
            this.associationPluralNameLabel.Location = new System.Drawing.Point(396, 91);
            this.associationPluralNameLabel.Name = "associationPluralNameLabel";
            this.associationPluralNameLabel.Size = new System.Drawing.Size(112, 16);
            this.associationPluralNameLabel.TabIndex = 15;
            this.associationPluralNameLabel.Text = "Plural name:";
            // 
            // associationLabel
            // 
            this.associationLabel.AutoSize = true;
            this.associationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.associationLabel.Location = new System.Drawing.Point(16, 77);
            this.associationLabel.Name = "associationLabel";
            this.associationLabel.Size = new System.Drawing.Size(100, 13);
            this.associationLabel.TabIndex = 12;
            this.associationLabel.Text = "AssociationType";
            // 
            // associationTypeComboBox
            // 
            this.associationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.associationTypeComboBox.Location = new System.Drawing.Point(18, 107);
            this.associationTypeComboBox.Name = "associationTypeComboBox";
            this.associationTypeComboBox.Size = new System.Drawing.Size(185, 21);
            this.associationTypeComboBox.TabIndex = 0;
            this.associationTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.AssociationTypeComboBoxSelectedIndexChanged);
            // 
            // associationTypeLabel
            // 
            this.associationTypeLabel.Location = new System.Drawing.Point(16, 91);
            this.associationTypeLabel.Name = "associationTypeLabel";
            this.associationTypeLabel.Size = new System.Drawing.Size(112, 16);
            this.associationTypeLabel.TabIndex = 13;
            this.associationTypeLabel.Text = "ObjectType:";
            // 
            // roleLabel
            // 
            this.roleLabel.AutoSize = true;
            this.roleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roleLabel.Location = new System.Drawing.Point(16, 130);
            this.roleLabel.Name = "roleLabel";
            this.roleLabel.Size = new System.Drawing.Size(33, 13);
            this.roleLabel.TabIndex = 16;
            this.roleLabel.Text = "Role";
            // 
            // rolePluralNameLabel
            // 
            this.rolePluralNameLabel.Location = new System.Drawing.Point(397, 143);
            this.rolePluralNameLabel.Name = "rolePluralNameLabel";
            this.rolePluralNameLabel.Size = new System.Drawing.Size(112, 16);
            this.rolePluralNameLabel.TabIndex = 19;
            this.rolePluralNameLabel.Text = "Plural name:";
            // 
            // roleTypeLabel
            // 
            this.roleTypeLabel.Location = new System.Drawing.Point(17, 142);
            this.roleTypeLabel.Name = "roleTypeLabel";
            this.roleTypeLabel.Size = new System.Drawing.Size(112, 16);
            this.roleTypeLabel.TabIndex = 17;
            this.roleTypeLabel.Text = "ObjectType:";
            // 
            // roleTypeComboBox
            // 
            this.roleTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.roleTypeComboBox.Location = new System.Drawing.Point(19, 158);
            this.roleTypeComboBox.Name = "roleTypeComboBox";
            this.roleTypeComboBox.Size = new System.Drawing.Size(184, 21);
            this.roleTypeComboBox.TabIndex = 3;
            this.roleTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.RoleTypeComboBoxSelectedIndexChanged);
            // 
            // rolePluralNameTextBox
            // 
            this.rolePluralNameTextBox.Location = new System.Drawing.Point(397, 159);
            this.rolePluralNameTextBox.Name = "rolePluralNameTextBox";
            this.rolePluralNameTextBox.Size = new System.Drawing.Size(184, 20);
            this.rolePluralNameTextBox.TabIndex = 5;
            // 
            // roleSingularNameTextBox
            // 
            this.roleSingularNameTextBox.Location = new System.Drawing.Point(207, 159);
            this.roleSingularNameTextBox.Name = "roleSingularNameTextBox";
            this.roleSingularNameTextBox.Size = new System.Drawing.Size(184, 20);
            this.roleSingularNameTextBox.TabIndex = 4;
            // 
            // roleSingularNameLabel
            // 
            this.roleSingularNameLabel.Location = new System.Drawing.Point(207, 143);
            this.roleSingularNameLabel.Name = "roleSingularNameLabel";
            this.roleSingularNameLabel.Size = new System.Drawing.Size(138, 16);
            this.roleSingularNameLabel.TabIndex = 18;
            this.roleSingularNameLabel.Text = "Singular name:";
            // 
            // relationLabel
            // 
            this.relationLabel.AutoSize = true;
            this.relationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.relationLabel.Location = new System.Drawing.Point(15, 182);
            this.relationLabel.Name = "relationLabel";
            this.relationLabel.Size = new System.Drawing.Size(82, 13);
            this.relationLabel.TabIndex = 20;
            this.relationLabel.Text = "RelationType";
            // 
            // multiplicityLabel
            // 
            this.multiplicityLabel.Location = new System.Drawing.Point(16, 195);
            this.multiplicityLabel.Name = "multiplicityLabel";
            this.multiplicityLabel.Size = new System.Drawing.Size(112, 16);
            this.multiplicityLabel.TabIndex = 21;
            this.multiplicityLabel.Text = "Multiplicity:";
            // 
            // multiplicityComboBox
            // 
            this.multiplicityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.multiplicityComboBox.Location = new System.Drawing.Point(19, 212);
            this.multiplicityComboBox.Name = "multiplicityComboBox";
            this.multiplicityComboBox.Size = new System.Drawing.Size(183, 21);
            this.multiplicityComboBox.TabIndex = 6;
            this.multiplicityComboBox.SelectedIndexChanged += new System.EventHandler(this.MultiplicityComboBoxSelectedIndexChanged);
            // 
            // multiplicityExplanationTextBox
            // 
            this.multiplicityExplanationTextBox.Location = new System.Drawing.Point(206, 213);
            this.multiplicityExplanationTextBox.Name = "multiplicityExplanationTextBox";
            this.multiplicityExplanationTextBox.ReadOnly = true;
            this.multiplicityExplanationTextBox.Size = new System.Drawing.Size(376, 20);
            this.multiplicityExplanationTextBox.TabIndex = 23;
            // 
            // multiplicityExplanationLabel
            // 
            this.multiplicityExplanationLabel.Location = new System.Drawing.Point(205, 194);
            this.multiplicityExplanationLabel.Name = "multiplicityExplanationLabel";
            this.multiplicityExplanationLabel.Size = new System.Drawing.Size(138, 16);
            this.multiplicityExplanationLabel.TabIndex = 22;
            this.multiplicityExplanationLabel.Text = "Multiplicity explanation:";
            // 
            // isIndexedCheckBox
            // 
            this.isIndexedCheckBox.AutoSize = true;
            this.isIndexedCheckBox.Location = new System.Drawing.Point(209, 239);
            this.isIndexedCheckBox.Name = "isIndexedCheckBox";
            this.isIndexedCheckBox.Size = new System.Drawing.Size(139, 17);
            this.isIndexedCheckBox.TabIndex = 26;
            this.isIndexedCheckBox.Text = "RelationType is indexed";
            this.isIndexedCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddRelationTypeWizard
            // 
            this.AcceptButton = this.finishButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(600, 324);
            this.Controls.Add(this.isIndexedCheckBox);
            this.Controls.Add(this.multiplicityExplanationLabel);
            this.Controls.Add(this.multiplicityExplanationTextBox);
            this.Controls.Add(this.multiplicityLabel);
            this.Controls.Add(this.multiplicityComboBox);
            this.Controls.Add(this.relationLabel);
            this.Controls.Add(this.rolePluralNameLabel);
            this.Controls.Add(this.roleTypeLabel);
            this.Controls.Add(this.roleTypeComboBox);
            this.Controls.Add(this.rolePluralNameTextBox);
            this.Controls.Add(this.roleSingularNameTextBox);
            this.Controls.Add(this.roleSingularNameLabel);
            this.Controls.Add(this.associationPluralNameLabel);
            this.Controls.Add(this.associationTypeLabel);
            this.Controls.Add(this.associationTypeComboBox);
            this.Controls.Add(this.associationPluralNameTextBox);
            this.Controls.Add(this.associationSingularNameTextBox);
            this.Controls.Add(this.associationSingularNameLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.titleLineLightPanel);
            this.Controls.Add(this.titleLineDarkPanel);
            this.Controls.Add(this.buttonLineLightPanel);
            this.Controls.Add(this.buttonLineDarkPanel);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.associationLabel);
            this.Controls.Add(this.roleLabel);
            this.Controls.Add(this.finishButton);
            this.Name = "AddRelationTypeWizard";
            this.Text = "Allors Add RelationType Wizard";
            this.titlePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void UpdateMultiplicity()
        {
            this.UpdateMultiplicityExplanation();
            this.UpdateMultiplicityIsIndexed();
        }

        private void UpdateMultiplicityExplanation()
        {
            if (this.multiplicityComboBox.SelectedItem != null)
            {
                var explanation = this.associationSingularNameTextBox.Text;
                if (this.multiplicityComboBox.SelectedItem.Equals(One2One) || this.multiplicityComboBox.SelectedItem.Equals(Many2One))
                {
                    explanation += " has one " + this.roleSingularNameTextBox.Text;
                }
                else
                {
                    explanation += " has many " + this.rolePluralNameTextBox.Text;
                }

                if (this.SelectedRoleType.IsComposite)
                {
                    explanation += ", " + this.roleSingularNameTextBox.Text;
                    if (this.multiplicityComboBox.SelectedItem.Equals(One2One) || this.multiplicityComboBox.SelectedItem.Equals(One2Many))
                    {
                        explanation += " has one " + this.associationSingularNameTextBox.Text;
                    }
                    else
                    {
                        explanation += " has many " + this.associationPluralNameTextBox.Text;
                    }
                }

                this.multiplicityExplanationTextBox.Text = explanation;
            }
        }

        private void UpdateMultiplicityIsIndexed()
        {
            // Composite relations have a default for indexing
            this.isIndexedCheckBox.Checked = this.multiplicityComboBox.Enabled;
        }

        private void MultiplicityComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateMultiplicity();
        }

        private void RoleTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.previouslySelectedRoleType == null)
            {
                this.roleSingularNameTextBox.Text = this.SelectedRoleType.SingularName;
                this.rolePluralNameTextBox.Text = this.SelectedRoleType.PluralName;
            }
            else
            {
                if (this.roleSingularNameTextBox.Text.Equals(this.previouslySelectedRoleType.SingularName))
                {
                    this.roleSingularNameTextBox.Text = this.SelectedRoleType.SingularName;
                }

                if (this.rolePluralNameTextBox.Text.Equals(this.previouslySelectedRoleType.PluralName))
                {
                    this.rolePluralNameTextBox.Text = this.SelectedRoleType.PluralName;
                }
            }

            this.previouslySelectedRoleType = this.SelectedRoleType;

            if (this.SelectedRoleType.IsUnit)
            {
                this.multiplicityComboBox.SelectedItem = One2One;
                this.multiplicityComboBox.Enabled = false;
            }
            else
            {
                this.multiplicityComboBox.SelectedItem = Many2One;
                this.multiplicityComboBox.Enabled = true;
            }

            this.UpdateMultiplicity();
        }

        private void AssociationTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.previouslySelectedAssociationType == null)
            {
                this.associationSingularNameTextBox.Text = this.SelectedAssociationType.SingularName;
                this.associationPluralNameTextBox.Text = this.SelectedAssociationType.PluralName;
            }
            else
            {
                if (this.associationSingularNameTextBox.Text.Equals(this.previouslySelectedAssociationType.SingularName))
                {
                    this.associationSingularNameTextBox.Text = this.SelectedAssociationType.SingularName;
                }

                if (this.associationPluralNameTextBox.Text.Equals(this.previouslySelectedAssociationType.PluralName))
                {
                    this.associationPluralNameTextBox.Text = this.SelectedAssociationType.PluralName;
                }
            }

            this.previouslySelectedAssociationType = this.SelectedAssociationType;

            this.UpdateMultiplicityExplanation();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FinishButtonClick(object sender, EventArgs e)
        {
            this.relationType = this.domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var association = this.relationType.AssociationType;
            association.ObjectType = this.SelectedAssociationType;
            if (!this.associationSingularNameTextBox.Text.Equals(this.SelectedAssociationType.SingularName))
            {
                association.AssignedSingularName = this.associationSingularNameTextBox.Text;
            }

            if (!this.associationPluralNameTextBox.Text.Equals(this.SelectedAssociationType.PluralName))
            {
                association.AssignedPluralName = this.associationPluralNameTextBox.Text;
            }

            var role = this.relationType.RoleType;
            role.ObjectType = this.SelectedRoleType;
            if (!this.roleSingularNameTextBox.Text.Equals(this.SelectedRoleType.SingularName))
            {
                role.AssignedSingularName = this.roleSingularNameTextBox.Text;
            }

            if (!this.rolePluralNameTextBox.Text.Equals(this.SelectedRoleType.PluralName))
            {
                role.AssignedPluralName = this.rolePluralNameTextBox.Text;
            }

            association.IsMany = false;
            role.IsMany = false;
            if (role.ObjectType.IsComposite)
            {
                if (this.multiplicityComboBox.SelectedItem.Equals(Many2One) || this.multiplicityComboBox.SelectedItem.Equals(Many2Many))
                {
                    association.IsMany = true;
                }

                if (this.multiplicityComboBox.SelectedItem.Equals(One2Many) || this.multiplicityComboBox.SelectedItem.Equals(Many2Many))
                {
                    role.IsMany = true;
                }
            }

            this.relationType.IsIndexed = this.isIndexedCheckBox.Checked;

            var validationReport = this.domain.Validate();
            if (!validationReport.ContainsErrors)
            {
                this.relationType.SendChangedEvent();
                this.domain.SendChangedEvent();
                this.Close();
            }
            else
            {
                this.relationType.Delete();
                this.domain.SendChangedEvent();

                MessageBox.Show(string.Join("\n", validationReport.Messages), "Allors Add RelationType Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}