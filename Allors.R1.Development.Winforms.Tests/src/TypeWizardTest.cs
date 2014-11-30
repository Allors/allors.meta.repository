// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeWizardTest.cs" company="Allors bvba">
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

    using Allors.R1.Meta;

    using AllorsTestWindowsTests;

    using NUnit.Framework;

    [TestFixture]
    public class TypeWizardTest : AllorsWithRepositoryTest
    {
        private Domain domain;
        private TestCaseSwitch testCaseSwitch;

        #region Test case switch
        private enum TestCaseSwitch
        {
            /// <summary>
            /// Minimal Test.
            /// </summary>
            Minimal,
            
            /// <summary>
            /// Maximal Test.
            /// </summary>
            Maximal,
            
            /// <summary>
            /// NoSingularName Test.
            /// </summary>
            NoSingularName,
            
            /// <summary>
            /// NoPluralName Test.
            /// </summary>
            NoPluralName,
            
            /// <summary>
            /// IncorrectSingularName Test.
            /// </summary>
            IncorrectSingularName,
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

            this.domain = this.Repository.Domain;
            this.domain.Name = "MyDomain";
            this.domain.SendChangedEvent();
        }

        [Test]
        public void IncorrectSingularName()
        {
            this.testCaseSwitch = TestCaseSwitch.IncorrectSingularName;

            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.DomainNode);

            var addType = new MenuItemTester(Constants.AddObjectType);

            Assert.IsTrue(addType.Target.Visible);

            addType.Click();

            Assert.AreEqual(0, this.domain.CompositeObjectTypes.Length);

            Assert.AreEqual(0, DomainNode.Target.Nodes.Count);

            Assert.AreEqual(2, this.OnShownCount);
        }

        [Test]
        public void Maximal()
        {
            this.testCaseSwitch = TestCaseSwitch.Maximal;

            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            Assert.IsTrue(this.domain.IsValid);

            this.TreeViewTester.SelectNode(this.DomainNode);

            var addType = new MenuItemTester(Constants.AddObjectType);

            Assert.IsTrue(addType.Target.Visible);

            addType.Click();

            Assert.AreEqual(1, this.domain.CompositeObjectTypes.Length);
            var objectType = this.domain.CompositeObjectTypes[0];
            Assert.AreEqual("SingularName", objectType.SingularName);
            Assert.AreEqual("PluralName", objectType.PluralName);

            Assert.AreEqual(1, DomainNode.Target.Nodes.Count);

            Assert.AreEqual("SingularName", TypeNode.Target.Text);
            Assert.AreEqual(new ObjectTypeTag(this.Repository, objectType), TypeNode.Target.Tag);
        }

        [Test]
        public void Minimal()
        {
            this.testCaseSwitch = TestCaseSwitch.Minimal;

            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.DomainNode);

            var addType = new MenuItemTester(Constants.AddObjectType);

            Assert.IsTrue(addType.Target.Visible);

            addType.Click();

            Assert.AreEqual(1, this.domain.CompositeObjectTypes.Length);
            var objectType = this.domain.CompositeObjectTypes[0];
            Assert.AreEqual("MyType", objectType.SingularName);

            Assert.AreEqual(1, DomainNode.Target.Nodes.Count);

            Assert.AreEqual("MyType", TypeNode.Target.Text);
            Assert.AreEqual(new ObjectTypeTag(this.Repository, objectType), TypeNode.Target.Tag);
        }

        [Test]
        public void NoPluralName()
        {
            this.testCaseSwitch = TestCaseSwitch.NoPluralName;

            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.DomainNode);

            var addType = new MenuItemTester(Constants.AddObjectType);

            Assert.IsTrue(addType.Target.Visible);

            addType.Click();

            Assert.AreEqual(0, this.domain.CompositeObjectTypes.Length);

            Assert.AreEqual(0, DomainNode.Target.Nodes.Count);

            Assert.AreEqual(2, this.OnShownCount);
        }

        [Test]
        public void NoSingularName()
        {
            this.testCaseSwitch = TestCaseSwitch.NoSingularName;

            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.DomainNode);

            var addType = new MenuItemTester(Constants.AddObjectType);

            Assert.IsTrue(addType.Target.Visible);

            addType.Click();

            Assert.AreEqual(0, this.domain.CompositeObjectTypes.Length);

            Assert.AreEqual(0, DomainNode.Target.Nodes.Count);

            Assert.AreEqual(2, this.OnShownCount);
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (this.testCaseSwitch)
            {
                case TestCaseSwitch.Minimal:
                    this.MinimalWizard();
                    break;
                case TestCaseSwitch.Maximal:
                    this.MaximalWizard();
                    break;
                case TestCaseSwitch.NoSingularName:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.NoSingularName_Wizard();
                            break;
                        case 2:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.IsTrue(messageBoxTester.Target.Text.StartsWith("object type"));
                            Assert.IsTrue(messageBoxTester.Target.Text.EndsWith("has no singular name"));
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.NoPluralName:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.NoPluralNameWizard();
                            break;
                        case 2:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.IsTrue(messageBoxTester.Target.Text.StartsWith("object type"));
                            Assert.IsTrue(messageBoxTester.Target.Text.EndsWith("has no plural name"));
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.IncorrectSingularName:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.IncorrectSingularNameWizard();
                            break;
                        case 2:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("object type My_Type's singular name should only contain alfanumerical characters", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        private void IncorrectSingularNameWizard()
        {
            var singularNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.SingularNameTextBox);
            singularNameTextBoxTester.Target.Text = "My_Type";
            var pluralNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.PluralNameTextBox);
            pluralNameTextBoxTester.Target.Text = "PluralName";

            var finishButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();

            var cancelButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.CancelButton);
            cancelButtonTester.Click();
        }

        private void MaximalWizard()
        {
            var singularNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.SingularNameTextBox);
            singularNameTextBoxTester.Target.Text = "SingularName";
            var pluralNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.PluralNameTextBox);
            pluralNameTextBoxTester.Target.Text = "PluralName";

            var finishButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }

        private void MinimalWizard()
        {
            var singularNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.SingularNameTextBox);
            singularNameTextBoxTester.Target.Text = "MyType";
            var pluralNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.PluralNameTextBox);
            pluralNameTextBoxTester.Target.Text = "MyTypes";

            var finishButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }

        private void NoPluralNameWizard()
        {
            var singularNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.SingularNameTextBox);
            singularNameTextBoxTester.Target.Text = "SingularName";

            var finishButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();

            var cancelButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.CancelButton);
            cancelButtonTester.Click();
        }

        private void NoSingularName_Wizard()
        {
            var pluralNameTextBoxTester = new TextBoxTester(Constants.AddObjectTypeWizard, Constants.PluralNameTextBox);
            pluralNameTextBoxTester.Target.Text = "PluralName";

            var finishButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();

            var cancelButtonTester = new ButtonTester(Constants.AddObjectTypeWizard, Constants.CancelButton);
            cancelButtonTester.Click();
        }
    }
}