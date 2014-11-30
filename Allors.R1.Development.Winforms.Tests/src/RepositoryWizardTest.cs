// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryWizardTest.cs" company="Allors bvba">
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
    using System;
    using System.Windows.Forms;

    using Allors.Testing.Winforms;
    using Allors.Testing.Winforms.Testers;

    using AllorsTestWindowsTests;
    using NUnit.Framework;

    [TestFixture]
    public class RepositoryWizardTest : AllorsWithoutRepositoryTest
    {
        private TestCaseSwitch testCaseSwitch;

        #region Test case switch
        private enum TestCaseSwitch
        {
            /// <summary>
            /// Create Test.
            /// </summary>
            Create,
        }
        #endregion

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            RepositoryDirectory.Refresh();
            if (RepositoryDirectory.Exists)
            {
                RepositoryDirectory.Delete();
            }

            RepositoryDirectory.Create();
        }

        [Test]
        public void Minimal()
        {
            this.testCaseSwitch = TestCaseSwitch.Create;

            var repositoriesNodeTester = this.TreeViewTester.FindNode(0);
            repositoriesNodeTester.Select();

            var createRepository = new MenuItemTester(Constants.AddRepository);

            Assert.IsTrue(createRepository.Target.Visible);

            createRepository.Click();

            var repositories = this.Explorer.Repositories;
            Assert.AreEqual(1, repositories.Length);

            this.Repository = repositories[0];
            Assert.AreEqual(RepositoryDirectory.FullName, this.Repository.DirectoryInfo.FullName);

            Assert.AreEqual("MyDomain", this.Repository.Domain.Name);

            var duplicateRepository = new Repository(this.Repository.DirectoryInfo);
            Assert.AreEqual("MyDomain", duplicateRepository.Domain.Name);
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (this.testCaseSwitch)
            {
                case TestCaseSwitch.Create:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.MinimalWizard();
                            break;
                        case 2:
                            var folderBrowserDialogTester = new FolderBrowserDialogTester(args.Tester);
                            folderBrowserDialogTester.Target.DialogResult = DialogResult.OK;
                            folderBrowserDialogTester.Target.SelectedPath = RepositoryDirectory.FullName;
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        private void MinimalWizard()
        {
            var nameTextBoxTester = new TextBoxTester(Constants.AddRepositoryWizard, Constants.NameTextBox);
            nameTextBoxTester.Target.Text = "MyDomain";

            var browseButtonTester = new ButtonTester(Constants.AddRepositoryWizard, Constants.BrowseButton);
            browseButtonTester.Click();

            var directoryTextBoxTester = new TextBoxTester(Constants.AddRepositoryWizard, Constants.DirectoryTextBox);
            Assert.AreEqual(RepositoryDirectory.FullName, directoryTextBoxTester.Target.Text);

            var finishButtonTester = new ButtonTester(Constants.AddRepositoryWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }
    }
}