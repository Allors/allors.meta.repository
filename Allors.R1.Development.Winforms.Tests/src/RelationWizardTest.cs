// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationWizardTest.cs" company="Allors bvba">
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

    using Allors.R1.Meta;
    using Allors.Testing.Winforms;
    using Allors.Testing.Winforms.Testers;

    using AllorsTestWindowsTests;
    using NUnit.Framework;

    [TestFixture]
    public class RelationWizardTest : AllorsWithRepositoryTest
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
            Maximal
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
        public void Maximal()
        {
            this.testCaseSwitch = TestCaseSwitch.Maximal;

            this.Explorer.AddRepository(this.Repository);

            var company = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            company.SendChangedEvent();
            person.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(TypeNode);

            var addRelation = new MenuItemTester(Constants.AddRelationType);

            Assert.IsTrue(addRelation.Target.Visible);

            addRelation.Click();

            Assert.AreEqual(1, this.domain.RelationTypes.Length);
            var relationType = this.domain.RelationTypes[0];
            Assert.AreEqual("EmployerEmployee", relationType.Name);
            Assert.IsTrue(relationType.AssociationType.IsOne);
            Assert.IsTrue(relationType.RoleType.IsMany);
            Assert.IsTrue(relationType.IsIndexed);

            this.TreeViewTester.Target.ExpandAll();

            Assert.AreEqual(1, TypeNode.FindByTagType(typeof(RelationTypeTag)).Length);

            Assert.AreEqual("Employees", RelationNode.Target.Text);
            Assert.AreEqual(new RelationTypeTag(this.Repository, relationType), this.RelationNode.Target.Tag);
        }

        [Test]
        public void Minimal()
        {
            this.testCaseSwitch = TestCaseSwitch.Minimal;

            this.Explorer.AddRepository(this.Repository);

            var c1 = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            c1.SingularName = "C1";
            c1.PluralName = "C1s";

            c1.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(TypeNode);

            var addRelation = new MenuItemTester(Constants.AddRelationType);

            Assert.IsTrue(addRelation.Target.Visible);

            addRelation.Click();

            Assert.AreEqual(1, this.domain.RelationTypes.Length);
            var relationType = this.domain.RelationTypes[0];
            Assert.AreEqual("C1AllorsString", relationType.Name);
            Assert.IsTrue(relationType.AssociationType.IsOne);
            Assert.IsTrue(relationType.RoleType.IsOne);
            Assert.IsFalse(relationType.IsIndexed);

            this.TreeViewTester.Target.ExpandAll();

            Assert.AreEqual(1, TypeNode.FindByTagType(typeof(RelationTypeTag)).Length);

            Assert.AreEqual("AllorsString", RelationNode.Target.Text);
            Assert.AreEqual(new RelationTypeTag(this.Repository, relationType), this.RelationNode.Target.Tag);
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
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        private void MaximalWizard()
        {
            var associationTypeComboBoxTester = new ComboBoxTester(Constants.AddRelationTypeWizard, Constants.AssociationTypeComboBox);
            associationTypeComboBoxTester.Select("Company");
            var associationSingularNameTextBoxTester = new TextBoxTester(Constants.AddRelationTypeWizard, Constants.AssociationSingularNameTextBox);
            associationSingularNameTextBoxTester.Target.Text = "Employer";
            var associationPluralNameTextBoxTester = new TextBoxTester(Constants.AddRelationTypeWizard, Constants.AssociationPluralNameTextBox);
            associationPluralNameTextBoxTester.Target.Text = "Employers";

            var roleTypeComboBoxTester = new ComboBoxTester(Constants.AddRelationTypeWizard, Constants.RoleTypeComboBox);
            roleTypeComboBoxTester.Select("Person");
            var roleSingularNameTextBoxTester = new TextBoxTester(Constants.AddRelationTypeWizard, Constants.RoleSingularNameTextBox);
            roleSingularNameTextBoxTester.Target.Text = "Employee";
            var rolePluralNameTextBoxTester = new TextBoxTester(Constants.AddRelationTypeWizard, Constants.RolePluralNameTextBox);
            rolePluralNameTextBoxTester.Target.Text = "Employees";

            var multiplicityComboBoxTester = new ComboBoxTester(Constants.AddRelationTypeWizard, Constants.MultiplicityComboBox);
            multiplicityComboBoxTester.Select("One to Many");

            var finishButtonTester = new ButtonTester(Constants.AddRelationTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }

        private void MinimalWizard()
        {
            var finishButtonTester = new ButtonTester(Constants.AddRelationTypeWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }
    }
}