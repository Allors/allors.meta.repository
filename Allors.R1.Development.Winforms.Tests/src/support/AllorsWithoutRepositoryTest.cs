// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsWithoutRepositoryTest.cs" company="Allors bvba">
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

namespace AllorsTestWindowsTests
{
    using System.IO;
    using System.Threading;

    using Allors.R1.Development.Repository;
    using Allors.R1.Development.Repository.WinForms;
    using Allors.R1.Development.Repository.WinForms.Tests;
    using Allors.Testing.Winforms.Domain;
    using Allors.Testing.Winforms.Testers;

    public abstract class AllorsWithoutRepositoryTest : AllorsWinformsTest
    {
        private TestForm testForm;
        private FileInfo repositoryFile;

        protected Repository Repository { get; set; }

        protected bool ExtendedView
        {
            get
            {
                var toolBarTester = new ToolBarTester(Constants.Toolbar);
                var toolBarButtonTester = new ToolBarButtonTester(AllorsTest.Singleton.FindTester(toolBarTester.Target.Buttons[0]));

                return toolBarButtonTester.Target.Pushed;
            }

            set
            {
                var toolBarTester = new ToolBarTester(Constants.Toolbar);
                var toolBarButtonTester = new ToolBarButtonTester(AllorsTest.Singleton.FindTester(toolBarTester.Target.Buttons[0]));

                toolBarButtonTester.Target.Pushed = value;
                toolBarButtonTester.Click();
            }
        }

        protected int OnShownCount { get; set; }

        protected TreeViewTester TreeViewTester { get; private set; }

        protected Explorer Explorer { get; private set; }

        protected DirectoryInfo TemplatesDirectory { get; private set; }

        protected DirectoryInfo Directory { get; private set; }

        protected DirectoryInfo RepositoryDirectory
        {
            get
            {
                this.Directory.Refresh();
                return this.Directory;
            }
        }

        protected FileInfo RepositoryFile
        {
            get
            {
                this.repositoryFile.Refresh();
                return this.repositoryFile;
            }
        }

        protected TreeNodeTester ExtendedViewTemplatesNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 1); }
        }

        protected TreeNodeTester ExtendedViewFirstTemplateNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 1, 0); }
        }

        protected TreeNodeTester ExtendedViewDomainNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 2); }
        }

        protected TreeNodeTester ExtendedViewSuperDomainsNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 0); }
        }

        protected TreeNodeTester DomainNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 0); }
        }

        protected TreeNodeTester RelationNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 0, 0, 1); }
        }

        protected TreeNodeTester RepositoriesNode
        {
            get { return this.TreeViewTester.FindNode(0); }
        }

        protected TreeNodeTester RepositoryNode
        {
            get { return this.TreeViewTester.FindNode(0, 0); }
        }

        protected TreeNodeTester SuperTypesNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 0, 0, 0); }
        }

        protected TreeNodeTester TypeNode
        {
            get { return this.TreeViewTester.FindNode(0, 0, 0, 0); }
        }

        public override void SetUp()
        {
            base.SetUp();

            this.OnShownCount = 0;

            this.Directory = new DirectoryInfo(GetType().ToString());

            this.Directory.Refresh();
            if (this.Directory.Exists)
            {
                try
                {
                    this.Directory.Delete(true);
                }
                catch
                {
                    Thread.Sleep(100);

                    this.Directory.Refresh();
                    this.Directory.Delete(true);
                }
            }

            this.repositoryFile = new FileInfo(Path.Combine(this.Directory.FullName, "allors.repository"));

            this.TemplatesDirectory = new DirectoryInfo(Path.Combine(this.Directory.FullName, "templates"));

            this.testForm = new TestForm();
            this.testForm.Show();

            this.Explorer = this.testForm.Explorer;

            this.TreeViewTester = new TreeViewTester(Constants.Treeview);
        }

        public override void Dispose()
        {
            this.testForm.Close();
            this.testForm.Dispose();
        }
    }
}