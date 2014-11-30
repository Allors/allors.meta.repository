// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperinterfacesEditor.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Editors
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    using Allors.R1.Meta;

    public class SuperinterfacesEditor : Form
    {
        private readonly ObjectType objectType;

        private CheckedListBox superinterfacesCheckedListBox;
        private Button cancelButton;

        private Button okButton;
        private Label superTypesLabel;

        public SuperinterfacesEditor(ObjectType objectType, ObjectType[] currentSuperinterfaces, ObjectType[] possibleSupertinterfaces)
        {
            this.InitializeComponent();

            this.objectType = objectType;

            var checkedSuperinterfaces = new ArrayList(currentSuperinterfaces);
            var superinterfaces = new ArrayList(possibleSupertinterfaces);

            superinterfaces.Sort();

            foreach (ObjectType superType in superinterfaces)
            {
                var index = this.superinterfacesCheckedListBox.Items.Add(superType);
                if (checkedSuperinterfaces.Contains(superType))
                {
                    this.superinterfacesCheckedListBox.SetItemChecked(index, true);

                    var inheritance = objectType.FindInheritanceWhereDirectSubtype(superType);
                    {
                        if (inheritance != null && inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
                        {
                            this.superinterfacesCheckedListBox.SetItemCheckState(index, CheckState.Indeterminate);
                        }
                    }
                }
            }
        }

        public ObjectType[] Superinterfaces
        {
            get { return (ObjectType[])new ArrayList(this.superinterfacesCheckedListBox.CheckedItems).ToArray(typeof(ObjectType)); }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.superinterfacesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.superTypesLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // superinterfacesCheckedListBox
            // 
            this.superinterfacesCheckedListBox.CheckOnClick = true;
            this.superinterfacesCheckedListBox.Location = new System.Drawing.Point(131, 17);
            this.superinterfacesCheckedListBox.Name = "superinterfacesCheckedListBox";
            this.superinterfacesCheckedListBox.Size = new System.Drawing.Size(349, 289);
            this.superinterfacesCheckedListBox.TabIndex = 0;
            this.superinterfacesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.SuperinterfacesCheckedListBoxSelectedIndexChanged);
            // 
            // superTypesLabel
            // 
            this.superTypesLabel.AutoSize = true;
            this.superTypesLabel.Location = new System.Drawing.Point(12, 17);
            this.superTypesLabel.Name = "superTypesLabel";
            this.superTypesLabel.Size = new System.Drawing.Size(84, 13);
            this.superTypesLabel.TabIndex = 1;
            this.superTypesLabel.Text = "Superinterfaces:";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(211, 331);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(131, 331);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 10;
            this.okButton.Text = "OK";
            // 
            // SuperinterfacesEditor
            // 
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(492, 366);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.superTypesLabel);
            this.Controls.Add(this.superinterfacesCheckedListBox);
            this.Name = "SuperinterfacesEditor";
            this.Text = "Superinterfaces";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void SuperinterfacesCheckedListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var superType = (ObjectType)this.superinterfacesCheckedListBox.SelectedItem;
            var inheritance = this.objectType.FindInheritanceWhereDirectSubtype(superType);

            if (inheritance != null && !inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
            {
                this.superinterfacesCheckedListBox.SetItemCheckState(this.superinterfacesCheckedListBox.SelectedIndex, CheckState.Indeterminate);
            }
        }
    }
}