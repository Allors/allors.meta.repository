// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestForm.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Tests
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TestForm : Form
    {
        public Explorer Explorer;

        private PropertyGrid testPropertyGrid;
        private IContainer components;
 
        public TestForm()
        {
            this.InitializeComponent();

            this.Explorer.NewMenuEnabled = true;
            this.Explorer.OpenMenuEnabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ExplorerSelected(object sender, SelectedEventArgs args)
        {
            this.testPropertyGrid.SelectedObject = args.SelectionForPropertyGrid;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Explorer = new Explorer();
            this.testPropertyGrid = new Allors.Testing.Winforms.Substitutes.PropertyGrid();
            this.SuspendLayout();
            // 
            // explorer
            // 
            this.Explorer.Dock = DockStyle.Left;
            this.Explorer.Location = new Point(0, 0);
            this.Explorer.Name = "Explorer";
            this.Explorer.NewMenuEnabled = false;
            this.Explorer.OpenMenuEnabled = false;
            this.Explorer.Size = new Size(298, 472);
            this.Explorer.TabIndex = 0;
            this.Explorer.Selected += this.ExplorerSelected;
            // 
            // testPropertyGrid
            // 
            this.testPropertyGrid.Dock = DockStyle.Right;
            this.testPropertyGrid.Location = new Point(302, 0);
            this.testPropertyGrid.Name = "testPropertyGrid";
            this.testPropertyGrid.Size = new Size(259, 472);
            this.testPropertyGrid.TabIndex = 1;
            // 
            // TestForm
            // 
            this.ClientSize = new Size(561, 472);
            this.Controls.Add(this.testPropertyGrid);
            this.Controls.Add(this.Explorer);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
        }
    }
}