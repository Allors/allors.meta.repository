// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Winforms.Program
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using Allors.R1.Meta;

    using Meta;
    using WinForms;
    using WinForms.Controls;

    /// <summary>
    /// The Main Form of the Allors Development Winforms Program
    /// </summary>
    public class MainForm : Form
    {
        private const string Title = "Allors Development";

        private Explorer explorer;
        private MetavalidationView metavalidationView;
        private Panel panel;
        private PropertyGrid propertyGrid;
        private Splitter splitter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            this.explorer.NewMenuEnabled = true;
            this.explorer.OpenMenuEnabled = true;

            this.propertyGrid.PropertySort = PropertySort.Categorized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the repository.
        /// </param>
        /// <param name="create">
        /// Create a repository if a repository does not exist.
        /// </param>
        public MainForm(string path, bool create) : this()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var repository = new Repository(directoryInfo, create);
                this.explorer.AddRepository(repository);
                this.metavalidationView.Validate(this.explorer.Repositories[0].Domain);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                MessageBox.Show("Could not load Population: " + e.Message, Title + " - Error");
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.explorer = new Development.Repository.WinForms.Explorer();
            this.splitter = new System.Windows.Forms.Splitter();
            this.metavalidationView = new Development.Repository.WinForms.Controls.MetavalidationView();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.propertyGrid);
            this.panel.Controls.Add(this.explorer);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(800, 328);
            this.panel.TabIndex = 0;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(355, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(445, 328);
            this.propertyGrid.TabIndex = 1;
            // 
            // explorer
            // 
            this.explorer.Dock = System.Windows.Forms.DockStyle.Left;
            this.explorer.Location = new System.Drawing.Point(0, 0);
            this.explorer.Name = "explorer";
            this.explorer.NewMenuEnabled = false;
            this.explorer.OpenMenuEnabled = false;
            this.explorer.Size = new System.Drawing.Size(355, 328);
            this.explorer.TabIndex = 0;
            this.explorer.Selected += this.ExplorerSelected;
            this.explorer.RepositoryObjectChanged += this.ExplorerRepositoryObjectChanged;
            this.explorer.RepositoryObjectDeleted += this.ExplorerRepositoryObjectDeleted;
            this.explorer.MetaObjectChanged += this.ExplorerMetaObjectChanged;
            this.explorer.MetaObjectDeleted += this.ExplorerMetaObjectDeleted;
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter.Location = new System.Drawing.Point(0, 324);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(800, 4);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // metavalidationView
            // 
            this.metavalidationView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metavalidationView.Location = new System.Drawing.Point(0, 328);
            this.metavalidationView.Name = "metavalidationView";
            this.metavalidationView.Size = new System.Drawing.Size(800, 156);
            this.metavalidationView.TabIndex = 2;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 484);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.metavalidationView);
            this.Name = "MainForm";
            this.Text = "Allors Repositories";
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ExplorerSelected(object sender, SelectedEventArgs args)
        {
            try
            {
                this.propertyGrid.SelectedObject = args.SelectionForPropertyGrid;
                this.propertyGrid.ExpandAllGridItems();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.ToString(), "Allors Development Environment - Can not show property", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExplorerMetaObjectChanged(object sender, RepositoryMetaObjectChangedEventArgs args)
        {
            this.metavalidationView.Validate(Domain.GetDomain(args.Repository.Domain));
        }

        private void ExplorerMetaObjectDeleted(object sender, RepositoryMetaObjectDeletedEventArgs args)
        {
            this.metavalidationView.Validate(Domain.GetDomain(args.Repository.Domain));
        }

        private void ExplorerRepositoryObjectChanged(object sender, RepositoryObjectChangedEventArgs args)
        {
            this.metavalidationView.Validate(Domain.GetDomain(args.Repository.Domain));
        }

        private void ExplorerRepositoryObjectDeleted(object sender, RepositoryObjectDeletedEventArgs args)
        {
            this.metavalidationView.Validate(Domain.GetDomain(args.Repository.Domain));
        }
    }
}