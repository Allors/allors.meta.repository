#region Copyright & License
// Copyright 2002-2008 Allors bvba.
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
#endregion

using System;
using System.Windows.Forms;
using Allors.Meta;
using Allors.Testing.Winforms.Domain;
using Allors.Testing.Winforms.Domain.Testers;
using AllorsTestWindowsTests;
using NUnit.Framework;

namespace Allors.Design.Winforms.Tests
{
    [TestFixture]
    public class DomainWizardTest : AllorsWithRepositoryTest
    {
        private enum TestCaseSwitch
        {
            None,
            Minimal,
            Maximal,
            NoName,
            IncorrectName
        }

        private TestCaseSwitch testCaseSwitch;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (testCaseSwitch)
            {
                case TestCaseSwitch.Minimal:
                    Minimal_Wizard(args);
                    break;
                case TestCaseSwitch.Maximal:
                    Maximal_Wizard(args);
                    break;
                case TestCaseSwitch.NoName:
                    switch (onShownCount++)
                    {
                        case 0:
                            NoName_Wizard(args);
                            break;
                        case 1:
                            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Domain has no name", messageBoxTester.Target.Text);
                            break;
                        case 2:
                            NoName_Wizard2(args);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }
                    break;
                case TestCaseSwitch.IncorrectName:
                    switch (onShownCount++)
                    {
                        case 0:
                            IncorrectName_Wizard(args);
                            break;
                        case 1:
                            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Domain A_Name_With_Underscores should only contain alfanumerical characters)", messageBoxTester.Target.Text);
                            break;
                        case 2:
                            IncorrectName_Wizard2(args);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }
                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        [Test]
        public void Minimal()
        {
            testCaseSwitch = TestCaseSwitch.Minimal;

            explorer.Repositories.Add(repository);

            TreeNodeTester repositoryNodeTester = treeViewTester.FindNode(0, 0);

            repositoryNodeTester.Select();

            MenuItemTester addDomain = new MenuItemTester(Constants.ADD_DOMAIN);

            Assert.IsTrue(addDomain.Target.Enabled);
            Assert.IsTrue(addDomain.Target.Visible);

            addDomain.Click();

            Assert.AreEqual(1, repositoryNodeTester.TreeNode.Nodes.Count);
            Assert.AreEqual(1, repository.MetaDomains.Count);

            Assert.IsTrue(repositoryNodeTester.TreeNode.IsExpanded);

            MetaDomain metaDomain = repository.MetaDomains[0];
            Assert.AreEqual("MyDomain", metaDomain.Name);
        }
        private void Minimal_Wizard(AllorsEventOccuredEventArgs args)
        {
            TextBoxTester nameTextBoxTester = new TextBoxTester(Constants.DOMAIN_WIZARD, Constants.NAME_TEXT_BOX);
            nameTextBoxTester.Target.Text = "MyDomain";

            ButtonTester finishButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.FINISH_BUTTON);
            finishButtonTester.Click();
        }

        [Test]
        public void Maximal()
        {
            testCaseSwitch = TestCaseSwitch.Maximal;

            explorer.Repositories.Add(repository);

            TreeNodeTester repositoryNodeTester = treeViewTester.FindNode(Constants.RepositoryTreeNodeIndeces);

            repositoryNodeTester.Select();

            MenuItemTester addDomain = new MenuItemTester(Constants.ADD_DOMAIN);

            Assert.IsTrue(addDomain.Target.Enabled);
            Assert.IsTrue(addDomain.Target.Visible);

            addDomain.Click();

            Assert.AreEqual(1, repositoryNodeTester.TreeNode.Nodes.Count);
            Assert.AreEqual(1, repository.MetaDomains.Count);
            MetaDomain metaDomain = repository.MetaDomains[0];
            Assert.AreEqual("MyDomain", metaDomain.Name);
            Assert.AreEqual("Documentation", metaDomain.Documentation);
        }
        private void Maximal_Wizard(AllorsEventOccuredEventArgs args)
        {
            TextBoxTester nameTextBoxTester = new TextBoxTester(Constants.DOMAIN_WIZARD, Constants.NAME_TEXT_BOX);
            nameTextBoxTester.Target.Text = "MyDomain";

            TextBoxTester docTextBoxTester = new TextBoxTester(Constants.DOMAIN_WIZARD, Constants.DOCUMENTATION_TEXT_BOX);
            docTextBoxTester.Target.Text = "Documentation";

            ButtonTester finishButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.FINISH_BUTTON);
            finishButtonTester.Click();
        }

        [Test]
        public void NoName()
        {
            testCaseSwitch = TestCaseSwitch.NoName;
            onShownCount = 0;

            explorer.Repositories.Add(repository);

            TreeNodeTester repositoryNodeTester = treeViewTester.FindNode(Constants.RepositoryTreeNodeIndeces);

            repositoryNodeTester.Select();

            MenuItemTester addDomain = new MenuItemTester(Constants.ADD_DOMAIN);

            Assert.IsTrue(addDomain.Target.Enabled);
            Assert.IsTrue(addDomain.Target.Visible);

            addDomain.Click();

            Assert.AreEqual(0, repositoryNodeTester.TreeNode.Nodes.Count);
            Assert.AreEqual(0, repository.MetaDomains.Count);
        }
        private void NoName_Wizard(AllorsEventOccuredEventArgs args)
        {
            TextBoxTester nameTextBoxTester = new TextBoxTester(Constants.DOMAIN_WIZARD, Constants.NAME_TEXT_BOX);
            nameTextBoxTester.Target.Text = "";

            ButtonTester finishButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.FINISH_BUTTON);
            finishButtonTester.Click();
        }
        private void NoName_Wizard2(AllorsEventOccuredEventArgs args)
        {
            ButtonTester cancelButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.CANCEL_BUTTON);
            cancelButtonTester.Click();
        }

        [Test]
        public void IncorrectName()
        {
            testCaseSwitch = TestCaseSwitch.IncorrectName;
            onShownCount = 0;

            explorer.Repositories.Add(repository);

            TreeNodeTester repositoryNodeTester = treeViewTester.FindNode(Constants.RepositoryTreeNodeIndeces);

            repositoryNodeTester.Select();

            MenuItemTester addDomain = new MenuItemTester(Constants.ADD_DOMAIN);

            Assert.IsTrue(addDomain.Target.Enabled);
            Assert.IsTrue(addDomain.Target.Visible);

            addDomain.Click();

            Assert.AreEqual(0, repositoryNodeTester.TreeNode.Nodes.Count);
            Assert.AreEqual(0, repository.MetaDomains.Count);

            Assert.IsTrue(DomainsDirectory.Exists);
            Assert.AreEqual(0, DomainsDirectory.GetFiles().Length);
            Assert.AreEqual(0, TypesDirectory.GetFiles().Length);
            Assert.AreEqual(0, RelationsDirectory.GetFiles().Length);
        }
        private void IncorrectName_Wizard(AllorsEventOccuredEventArgs args)
        {
            TextBoxTester nameTextBoxTester = new TextBoxTester(Constants.DOMAIN_WIZARD, Constants.NAME_TEXT_BOX);
            nameTextBoxTester.Target.Text = "A_Name_With_Underscores";

            ButtonTester finishButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.FINISH_BUTTON);
            finishButtonTester.Click();
        }
        private void IncorrectName_Wizard2(AllorsEventOccuredEventArgs args)
        {
            ButtonTester cancelButtonTester = new ButtonTester(Constants.DOMAIN_WIZARD, Constants.CANCEL_BUTTON);
            cancelButtonTester.Click();
        }
    }
}