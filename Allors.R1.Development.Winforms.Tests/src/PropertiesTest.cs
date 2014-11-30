// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesTest.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    using Allors.R1.Development.Repository.WinForms.Decorators;
    using Allors.R1.Development.Repository.WinForms.Editors;
    using Allors.R1.Meta;
    using Allors.Testing.Winforms;
    using Allors.Testing.Winforms.Testers;

    using AllorsTestWindowsTests;

    using NUnit.Framework;

    [TestFixture]
    public class PropertiesTest : AllorsWithRepositoryTest
    {
        private Domain domain;
        private TestCaseSwitch testCaseSwitch;

        #region Test case switch
        private enum TestCaseSwitch
        {
            /// <summary>
            ///   The extension.
            /// </summary>
            Extension, 

            /// <summary>
            ///   The interfaces.
            /// </summary>
            Interfaces
        }
        #endregion

        private DirectoryInfo SuperDomainDirectoryInfo
        {
            get
            {
                return new DirectoryInfo(Path.Combine(this.Directory.FullName, "superdomain"));
            }
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

        [Test]
        public void Interfaces()
        {
            this.testCaseSwitch = TestCaseSwitch.Interfaces;

            this.Explorer.AddRepository(this.Repository);

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            var role = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            role.IsInterface = true;
            role.SingularName = "Role";
            role.PluralName = "RoleTypes";

            role.SendChangedEvent();
            person.SendChangedEvent();
            this.domain.SendChangedEvent();

            // Muldec
            this.TreeViewTester.SelectNode(0, 0, 0, 0);

            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");

            var typeDecorator = (ObjectTypeDecorator)testPropertyGrid.Target.SelectedObject;
            var editorMock = new EditorMock(typeDecorator);
            var superinterfacesTypeEditor = new SuperinterfacesTypeEditor();
            var interfaces =
                (ObjectType[])
                superinterfacesTypeEditor.EditValue(editorMock, editorMock, typeDecorator.PossibleSuperinterfaces);
            typeDecorator.Superinterfaces = interfaces;

            Assert.AreEqual(1, interfaces.Length);
            Assert.AreEqual(role, interfaces[0]);
            Assert.AreEqual(1, person.DirectSuperinterfaces.Length);
        }

        [Test]
        public void Relation()
        {
            this.Explorer.AddRepository(this.Repository);

            var company = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            company.SendChangedEvent();
            person.SendChangedEvent();

            var employeeRelationType = this.domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            employeeRelationType.AssociationType.ObjectType = company;
            employeeRelationType.AssociationType.IsOne = true;
            employeeRelationType.AssociationType.AssignedSingularName = "Employer";
            employeeRelationType.AssociationType.AssignedPluralName = "Employers";
            employeeRelationType.RoleType.ObjectType = person;
            employeeRelationType.RoleType.IsMany = true;
            employeeRelationType.RoleType.AssignedSingularName = "Employee";
            employeeRelationType.RoleType.AssignedPluralName = "Employees";

            var companyName = this.domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            companyName.IsIndexed = true;
            companyName.IsDerived = true;
            companyName.AssociationType.ObjectType = company;
            companyName.RoleType.ObjectType = (ObjectType)this.domain.Domain.Find(UnitTypeIds.StringId);
            companyName.RoleType.AssignedSingularName = "Name";
            companyName.RoleType.AssignedPluralName = "Names";

            employeeRelationType.SendChangedEvent();
            companyName.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TypeNode.Target.ExpandAll();
            var relationNodeTesters = this.TypeNode.FindByTagType(typeof(RelationTypeTag));
            var employeeNodeTester = relationNodeTesters[0];
            var nameNodeTester = relationNodeTesters[1];

            // Indexed
            this.TreeViewTester.SelectNode(employeeNodeTester);
            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");
            var employeeRelationIsIndexedTester = testPropertyGrid.FindGridItem("IsIndexed");
            this.TreeViewTester.SelectNode(nameNodeTester);
            testPropertyGrid = new PropertyGridTester("testPropertyGrid");
            var nameRelationIsIndexedTester = testPropertyGrid.FindGridItem("IsIndexed");

            Assert.AreEqual(false, employeeRelationIsIndexedTester.Value);
            Assert.AreEqual(true, nameRelationIsIndexedTester.Value);

            employeeRelationIsIndexedTester.Value = true;
            nameRelationIsIndexedTester.Value = false;

            Assert.AreEqual(true, employeeRelationType.IsIndexed);
            Assert.AreEqual(false, companyName.IsIndexed);

            // Derived
            this.TreeViewTester.SelectNode(employeeNodeTester);
            testPropertyGrid = new PropertyGridTester("testPropertyGrid");
            var employeeRelationIsDerivedTester = testPropertyGrid.FindGridItem("IsDerived");
            this.TreeViewTester.SelectNode(nameNodeTester);
            testPropertyGrid = new PropertyGridTester("testPropertyGrid");
            var nameRelationIsDerivedTester = testPropertyGrid.FindGridItem("IsDerived");

            Assert.AreEqual(false, employeeRelationIsDerivedTester.Value);
            Assert.AreEqual(true, nameRelationIsDerivedTester.Value);

            employeeRelationIsDerivedTester.Value = true;
            nameRelationIsDerivedTester.Value = false;

            Assert.AreEqual(true, employeeRelationType.IsDerived);
            Assert.AreEqual(false, companyName.IsDerived);

            // Names
            this.TreeViewTester.SelectNode(employeeNodeTester);
            testPropertyGrid = new PropertyGridTester("testPropertyGrid");

            var employeeAssociationSingularName = testPropertyGrid.FindGridItem("AssociationType", "SingularName");
            employeeAssociationSingularName.Value = string.Empty;
            Assert.IsFalse(employeeRelationType.AssociationType.ExistAssignedSingularName);

            var employeeAssociationPluralName = testPropertyGrid.FindGridItem("AssociationType", "PluralName");
            employeeAssociationPluralName.Value = string.Empty;
            Assert.IsFalse(employeeRelationType.AssociationType.ExistAssignedPluralName);

            Assert.IsNotNull(testPropertyGrid.FindGridItem("AssociationType", "ObjectType"));

            var employeeRoleSingularName = testPropertyGrid.FindGridItem("RoleType", "SingularName");
            employeeRoleSingularName.Value = string.Empty;
            Assert.IsFalse(employeeRelationType.RoleType.ExistAssignedSingularName);

            var employeeRolePluralName = testPropertyGrid.FindGridItem("RoleType", "PluralName");
            employeeRolePluralName.Value = string.Empty;
            Assert.IsFalse(employeeRelationType.RoleType.ExistAssignedPluralName);

            Assert.IsNotNull(testPropertyGrid.FindGridItem("RoleType", "ObjectType"));
        }

        [Test]
        public void RelationType()
        {
            this.Explorer.AddRepository(this.Repository);

            var company = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            company.SendChangedEvent();
            person.SendChangedEvent();

            var employeeRelationType = this.domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            employeeRelationType.AssociationType.ObjectType = company;
            employeeRelationType.AssociationType.IsOne = true;
            employeeRelationType.AssociationType.AssignedSingularName = "Employer";
            employeeRelationType.AssociationType.AssignedPluralName = "Employers";
            employeeRelationType.RoleType.ObjectType = person;
            employeeRelationType.RoleType.IsMany = true;
            employeeRelationType.RoleType.AssignedSingularName = "Employee";
            employeeRelationType.RoleType.AssignedPluralName = "Employees";

            employeeRelationType.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TypeNode.Target.ExpandAll();
            var relationNodeTesters = this.TypeNode.FindByTagType(typeof(RelationTypeTag));
            var employeeNodeTester = relationNodeTesters[0];

            this.TreeViewTester.SelectNode(employeeNodeTester);
            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");

            var relationDecorator = (RelationTypeDecorator)testPropertyGrid.Target.SelectedObject;
            var roleDecorator = relationDecorator.RoleType;
            var editorMock = new EditorMock(roleDecorator);
            editorMock.EditorMockDropDown += EditorMockEditorMockDropDown;
            var typeTypeEditor = new ObjectTypeTypeEditor();
            typeTypeEditor.EditValue(editorMock, editorMock, roleDecorator.ObjectType);
        }

        [Test]
        public void Repositories()
        {
            this.Explorer.AddRepository(this.Repository);

            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.RepositoriesNode);
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
        public void SuperClass()
        {
            this.Explorer.AddRepository(this.Repository);

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            var employee = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            employee.SingularName = "Employee";
            employee.PluralName = "Employees";
            employee.IsAbstract = true;

            var adressable = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            adressable.SingularName = "Adressable";
            adressable.PluralName = "Adressable";
            adressable.IsAbstract = true;

            person.SendChangedEvent();
            employee.SendChangedEvent();
            adressable.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.TreeViewTester.FindNode(0, 0, 0, 2));

            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");
            var superClassGridItem = testPropertyGrid.FindGridItem("Superclass");

            Assert.AreEqual(null, superClassGridItem.Value);

            var typeDecorator = (ObjectTypeDecorator)testPropertyGrid.Target.SelectedObject;
            var editorMock = new EditorMock(typeDecorator);
            editorMock.EditorMockDropDown += SuperClassEditorMockDropDown;
            var superclassTypeEditorTypeEditor = new SuperclassTypeEditor();
            var superClass =
                (ObjectType)superclassTypeEditorTypeEditor.EditValue(editorMock, editorMock, typeDecorator.Superclass);

            Assert.AreEqual(superClass, employee);

            superClassGridItem.Value = superClass;

            Assert.AreEqual(superClass, person.DirectSuperclass);
        }

        [Test]
        public void SuperTypes()
        {
            this.Explorer.AddRepository(this.Repository);

            var classA = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            classA.SingularName = "A";
            classA.PluralName = "As";

            var abstractClassA = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            abstractClassA.IsAbstract = true;
            abstractClassA.SingularName = "AA";
            abstractClassA.PluralName = "AAs";

            var interfaceA = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            interfaceA.IsInterface = true;
            interfaceA.SingularName = "IA";
            interfaceA.PluralName = "IAs";

            ObjectType[] interfaces = { interfaceA };

            classA.SendChangedEvent();
            abstractClassA.SendChangedEvent();
            interfaceA.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.TypeNode);

            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");

            var typeDecorator = (ObjectTypeDecorator)testPropertyGrid.Target.SelectedObject;

            typeDecorator.Superclass = abstractClassA;

            Assert.AreEqual(abstractClassA, typeDecorator.Superclass);
            Assert.AreEqual(0, typeDecorator.Superinterfaces.Length);

            typeDecorator.Superinterfaces = interfaces;

            Assert.AreEqual(abstractClassA, typeDecorator.Superclass);
            Assert.AreEqual(1, typeDecorator.Superinterfaces.Length);
            Assert.AreEqual(interfaceA, typeDecorator.Superinterfaces[0]);

            typeDecorator.Superinterfaces = new ObjectType[0];

            Assert.AreEqual(abstractClassA, typeDecorator.Superclass);
            Assert.AreEqual(0, typeDecorator.Superinterfaces.Length);

            typeDecorator.Superclass = null;

            Assert.IsNull(typeDecorator.Superclass);
            Assert.AreEqual(0, typeDecorator.Superinterfaces.Length);
        }

        [Test]
        public void Type()
        {
            this.Explorer.AddRepository(this.Repository);

            var person = this.domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            person.SendChangedEvent();
            this.domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.TypeNode);

            var testPropertyGrid = new PropertyGridTester("testPropertyGrid");

            var singularNameTester = testPropertyGrid.FindGridItem("SingularName");
            var pluralNameTester = testPropertyGrid.FindGridItem("PluralName");

            singularNameTester.Value = "Persoon";
            pluralNameTester.Value = "Personen";

            Assert.AreEqual("Persoon", person.SingularName);
            Assert.AreEqual("Personen", person.PluralName);
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (this.testCaseSwitch)
            {
                case TestCaseSwitch.Extension:
                    ExtensionEditor();
                    break;
                case TestCaseSwitch.Interfaces:
                    InterfacesEditor();
                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        private static void EditorMockEditorMockDropDown(object sender, EditorMockDropDownEventArgs args)
        {
            var listBox = (ListBox)args.Control;

            var types = (ObjectType[])listBox.DataSource;

            Assert.AreEqual(11, types.Length);

            var typeNames = new List<string>();
            foreach (var type in types)
            {
                typeNames.Add(type.Name);
            }

            Assert.Contains("Company", typeNames);
            Assert.Contains("Person", typeNames);

            Assert.Contains("AllorsBinary", typeNames);
            Assert.Contains("AllorsBoolean", typeNames);
            Assert.Contains("AllorsDateTime", typeNames);
            Assert.Contains("AllorsDecimal", typeNames);
            Assert.Contains("AllorsDouble", typeNames);
            Assert.Contains("AllorsInteger", typeNames);
            Assert.Contains("AllorsLong", typeNames);
            Assert.Contains("AllorsString", typeNames);
            Assert.Contains("AllorsUnique", typeNames);
        }

        private static void ExtensionEditor()
        {
            var textBoxTester = new TextBoxTester(Constants.ExtensionEditor, Constants.TextBox);
            textBoxTester.Target.Text = "<extension><value>OK</value></extension>";

            var okButtonTester = new ButtonTester(Constants.ExtensionEditor, Constants.OkButton);
            okButtonTester.Click();
        }

        private static void InterfacesEditor()
        {
            var superinterfacesCheckedListBoxTester = new CheckedListBoxTester(
                Constants.SuperinterfacesEditor, Constants.SuperinterfacesCheckedlistbox);
            superinterfacesCheckedListBoxTester.Target.SetItemChecked(0, true);

            var okButtonTester = new ButtonTester(Constants.SuperinterfacesEditor, Constants.OkButton);
            okButtonTester.Click();
        }

        private static void SuperClassEditorMockDropDown(object sender, EditorMockDropDownEventArgs args)
        {
            var listBox = (ListBox)args.Control;

            Assert.AreEqual(3, listBox.Items.Count);

            listBox.SelectedIndex = 0;
            Assert.AreEqual(string.Empty, listBox.Text);

            listBox.SelectedIndex = 1;
            Assert.AreEqual("Adressable", listBox.Text);

            listBox.SelectedIndex = 2;
            Assert.AreEqual("Employee", listBox.Text);
        }
    }
}