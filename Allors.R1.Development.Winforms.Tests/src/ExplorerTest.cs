// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerTest.cs" company="Allors bvba">
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
    using System.IO;
    using System.Windows.Forms;

    using Allors.R1.Meta;
    using Allors.Testing.Winforms;
    using Allors.Testing.Winforms.Testers;

    using AllorsTestWindowsTests;

    using NUnit.Framework;

    //TODO: Test with same treeNode.Text (namespace, type, relation, ...)
    [TestFixture]
    public class ExplorerTest : AllorsWithRepositoryTest
    {
        private TestCaseSwitch testCaseSwitch;

        #region Test case switch
        private enum TestCaseSwitch
        {
            /// <summary>
            /// OpenRepository Test.
            /// </summary>
            OpenRepository,

            /// <summary>
            /// AddSuper Test.
            /// </summary>
            AddSuperDomain,

            /// <summary>
            /// DeleteType Test.
            /// </summary>
            DeleteType,

            /// <summary>
            /// DeleteRelation Test.
            /// </summary>
            DeleteRelation,

            /// <summary>
            /// DeleteSuperDomain Test.
            /// </summary>
            DeleteSuperDomain,

            /// <summary>
            /// DeleteTypeWithCascading Test.
            /// </summary>
            DeleteTypeWithCascading,

            /// <summary>
            /// UpdateTemplate Test.
            /// </summary>
            UpdateTemplate,

            /// <summary>
            /// InheritedTypeNoMenus Test.
            /// </summary>
            InheritedTypeNoMenus,

            /// <summary>
            /// Generate Test.
            /// </summary>
            Generate,
        }
        #endregion

        private DirectoryInfo SuperDomainDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.Directory.FullName, "superdomain")); }
        }

        private FileInfo SuperDomainRepositoryFileInfo
        {
            get { return new FileInfo(Path.Combine(this.SuperDomainDirectoryInfo.FullName, "allors.repository")); }
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void OpenRepository()
        {
            this.testCaseSwitch = TestCaseSwitch.OpenRepository;

            this.Explorer.RemoveRepository(this.Repository);

            var repositories = this.Explorer.Repositories;
            Assert.AreEqual(0, repositories.Length);

            this.TreeViewTester.SelectNode(this.RepositoriesNode);

            var openRepository = new MenuItemTester(Constants.OpenRepository);

            Assert.IsTrue(openRepository.Target.Visible);

            openRepository.Click();

            Assert.IsTrue(openRepository.Target.Visible);

            Assert.IsTrue(RepositoriesNode.Target.IsExpanded);

            repositories = this.Explorer.Repositories;
            Assert.AreEqual(1, repositories.Length);

            this.Repository = repositories[0];
            Assert.AreEqual(RepositoryDirectory.FullName, this.Repository.DirectoryInfo.FullName);
        }

        [Test]
        public void AddSuperDomain()
        {
            this.testCaseSwitch = TestCaseSwitch.AddSuperDomain;

            var superDomainRepository = new Repository(this.SuperDomainDirectoryInfo, true);
            var superDomainDomain = superDomainRepository.Domain;

            superDomainDomain.Name = "SuperDomain";

            var superDomainClass = superDomainDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainClass.SingularName = "SuperDomainClass";
            superDomainClass.PluralName = "SuperDomainClasses";

            var superDomainAbstractClass = superDomainDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAbstractClass.SingularName = "SuperDomainAbstractClass";
            superDomainAbstractClass.PluralName = "SuperDomainAbstractClasses";

            superDomainDomain.Validate();
            Assert.IsTrue(superDomainDomain.IsValid);

            superDomainClass.SendChangedEvent();
            superDomainAbstractClass.SendChangedEvent();
            superDomainDomain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.ExtendedView = true;

            this.TreeViewTester.SelectNode(this.ExtendedViewSuperDomainsNode);

            var addSuperDomain = new MenuItemTester(Constants.AddSuperDomain);

            Assert.IsTrue(addSuperDomain.Target.Visible);

            addSuperDomain.Click();

            Assert.AreEqual(1, this.ExtendedViewSuperDomainsNode.Target.Nodes.Count);

            // TODO: remove to Constants
            var superDomainTester = this.TreeViewTester.FindNode(0, 0, 3);

            Assert.AreEqual("SuperDomain", superDomainTester.Target.Text);
            Assert.AreEqual(2, superDomainTester.Target.Nodes.Count);

            var superDomainAbstractClassNodeTester = superDomainTester[0];
            var superDomainClassNodeTester = superDomainTester[1];

            Assert.AreEqual("SuperDomainAbstractClass", superDomainAbstractClassNodeTester.Target.Text);
            Assert.AreEqual("SuperDomainClass", superDomainClassNodeTester.Target.Text);
        }

        [Test]
        public void CreateDomainAfterShowingExplorer()
        {
            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            Assert.AreEqual("MyDomain", RepositoryNode.Target.Text);
            Assert.AreEqual(new RepositoryTag(this.Repository), RepositoryNode.Target.Tag);
            Assert.IsTrue(RepositoryNode.Target.IsExpanded);
        }

        [Test]
        public void CreateDomainBeforeShowingExplorer()
        {
            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            Assert.AreEqual("MyDomain", RepositoryNode.Target.Text);
            Assert.AreEqual(new RepositoryTag(this.Repository), RepositoryNode.Target.Tag);
            Assert.IsFalse(RepositoryNode.Target.IsExpanded);
        }

        [Test]
        public void DeleteRelation()
        {
            this.testCaseSwitch = TestCaseSwitch.DeleteRelation;

            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var objectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.SingularName = "Singular";
            objectType.PluralName = "Plural";

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = objectType;
            relationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);

            var relation2 = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relation2.AssociationType.ObjectType = objectType;
            relation2.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);
            relation2.RoleType.AssignedSingularName = "AllorsString2";
            relation2.RoleType.AssignedPluralName = "AllorsStrings2";
            relation2.RoleType.Size = -1;

            objectType.SendChangedEvent();
            relationType.SendChangedEvent();
            relation2.SendChangedEvent();
            domain.SendChangedEvent();

            TypeNode.Target.Expand();

            this.TreeViewTester.SelectNode(this.RelationNode);

            var deleteRelation = new MenuItemTester(Constants.Delete);

            Assert.IsTrue(deleteRelation.Target.Visible);

            Assert.AreEqual(2, domain.RelationTypes.Length);

            deleteRelation.Click();

            Assert.IsTrue(domain.IsValid);

            Assert.AreEqual(1, domain.RelationTypes.Length);

            Assert.AreEqual(2, TypeNode.Target.Nodes.Count);
        }

        [Test]
        public void DeleteSuperDomain()
        {
            this.testCaseSwitch = TestCaseSwitch.DeleteSuperDomain;

            var superDomainRepository = new Repository(this.SuperDomainDirectoryInfo, true);
            var superDomain = superDomainRepository.Domain;

            superDomain.Name = "SuperDomain";
            superDomain.SendChangedEvent();

            var domain = this.Repository.Domain;
            domain.Name = "Domain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            this.Repository.AddSuper(this.SuperDomainDirectoryInfo);

            this.ExtendedView = true;

            this.ExtendedViewSuperDomainsNode.Target.Expand();

            this.TreeViewTester.SelectNode(0, 0, 0, 0);

            var delete = new MenuItemTester(Constants.Delete);

            Assert.IsTrue(delete.Target.Visible);

            Assert.AreEqual(2, domain.DirectSuperDomains.Length);

            delete.Click();

            domain = this.Repository.Domain;
            Assert.AreEqual(1, domain.DirectSuperDomains.Length);
        }

        [Test]
        public void DeleteType()
        {
            this.testCaseSwitch = TestCaseSwitch.DeleteType;

            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var objectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.SingularName = "Singular";
            objectType.PluralName = "Plural";

            var objectType2 = domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType2.SingularName = "Singular2";
            objectType2.PluralName = "Plural2";

            objectType.SendChangedEvent();
            objectType2.SendChangedEvent();
            domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.TypeNode);

            var deleteType = new MenuItemTester(Constants.Delete);

            Assert.IsTrue(deleteType.Target.Visible);

            Assert.AreEqual(2, domain.CompositeObjectTypes.Length);

            deleteType.Click();

            Assert.AreEqual(1, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(1, DomainNode.Target.Nodes.Count);
        }

        [Test]
        public void DeleteTypeWithCascading()
        {
            this.testCaseSwitch = TestCaseSwitch.DeleteTypeWithCascading;

            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var superType = domain.AddDeclaredObjectType(Guid.NewGuid());
            superType.SingularName = "Super";
            superType.PluralName = "Supers";
            superType.IsInterface = true;

            var objectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.SingularName = "Singular";
            objectType.PluralName = "Plural";
            objectType.AddDirectSupertype(superType);

            var objectType2 = domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType2.SingularName = "Singular2";
            objectType2.PluralName = "Plural2";

            var superRelationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superRelationType.AssociationType.ObjectType = superType;
            superRelationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = objectType;
            relationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);

            var relation2 = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relation2.AssociationType.ObjectType = objectType2;
            relation2.RoleType.ObjectType = objectType;

            superType.SendChangedEvent();
            objectType.SendChangedEvent();
            objectType2.SendChangedEvent();
            superRelationType.SendChangedEvent();
            relationType.SendChangedEvent();
            relation2.SendChangedEvent();
            domain.SendChangedEvent();

            this.TreeViewTester.SelectNode(this.TypeNode);

            var deleteType = new MenuItemTester(Constants.Delete);

            Assert.IsTrue(deleteType.Target.Visible);

            Assert.AreEqual(3, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(3, domain.RelationTypes.Length);

            deleteType.Click();

            Assert.AreEqual(2, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(2, domain.RelationTypes.Length);

            Assert.AreEqual(2, DomainNode.Target.Nodes.Count);

            Assert.IsTrue(objectType.IsDeleted);
            Assert.IsFalse(objectType2.IsDeleted);

            Assert.IsFalse(superRelationType.IsDeleted);
            Assert.IsTrue(relationType.IsDeleted);
            Assert.IsFalse(relation2.RoleType.ExistObjectType);
        }

        [Test]
        public void EmptyRelation()
        {
            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            relationType.SendChangedEvent();
            domain.SendChangedEvent();

            Assert.AreEqual(1, RepositoryNode.FindByTagType(typeof(RelationTypeTag)).Length);

            var relationNode = RepositoryNode.FindByTagType(typeof(RelationTypeTag))[0];

            Assert.AreEqual(relationType.Id.ToString(), relationNode.Target.Text);
            Assert.AreEqual(new RelationTypeTag(this.Repository, relationType), relationNode.Target.Tag);
        }

        [Test]
        public void EmptyType()
        {
            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var objectType = domain.AddDeclaredObjectType(Guid.NewGuid());

            objectType.SendChangedEvent();
            domain.SendChangedEvent();

            Assert.AreEqual(1, DomainNode.FindByTagType(typeof(ObjectTypeTag)).Length);

            var typeNode = DomainNode.FindByTagType(typeof(ObjectTypeTag))[0];

            Assert.AreEqual(objectType.Id.ToString(), typeNode.Target.Text);
            Assert.AreEqual(new ObjectTypeTag(this.Repository, objectType), typeNode.Target.Tag);
        }

        [Test]
        public void Generate()
        {
            this.testCaseSwitch = TestCaseSwitch.Generate;

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            this.TreeViewTester.SelectNode(this.RepositoryNode);
            var generate = new MenuItemTester(Constants.Generate);

            Assert.IsTrue(generate.Target.Visible);

            generate.Click();
        }

        [Test]
        public void InheritedTypeNoMenus()
        {
            this.testCaseSwitch = TestCaseSwitch.InheritedTypeNoMenus;

            this.Explorer.AddRepository(this.Repository);

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";

            var myInterface = domain.AddDeclaredObjectType(Guid.NewGuid());
            myInterface.SingularName = "MyInterface";
            myInterface.PluralName = "MyInterfaces";
            myInterface.IsInterface = true;

            var myClass = domain.AddDeclaredObjectType(Guid.NewGuid());
            myClass.SingularName = "MyClass";
            myClass.PluralName = "MyClasses";

            myClass.AddDirectSupertype(myInterface);

            myInterface.SendChangedEvent();
            myClass.SendChangedEvent();
            domain.SendChangedEvent();

            SuperTypesNode.Target.ExpandAll();

            this.TreeViewTester.SelectNode(0, 0, 0, 0, 0, 0);

            var deleteType = new MenuItemTester(Constants.Delete);

            Assert.IsFalse(deleteType.Target.Visible);

            var addRelation = new MenuItemTester(Constants.AddRelationType);

            Assert.IsFalse(addRelation.Target.Visible);
        }

        [Test]
        public void NoRepository()
        {
            Assert.AreEqual(1, this.TreeViewTester.Target.Nodes.Count);
            Assert.AreEqual(0, this.TreeViewTester.Target.Nodes[0].Nodes.Count);
        }

        [Test]
        public void UpdateTemplate()
        {
            this.testCaseSwitch = TestCaseSwitch.UpdateTemplate;

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            var template = this.Repository.AddTemplate();
            template.Name = "MyGeneration";

            var templatesNodeTester = this.TreeViewTester.FindNode(0, 0, 1);
            templatesNodeTester.Target.ExpandAll();

            this.TreeViewTester.SelectNode(0, 0, 1, 0);
            var templateNodeTester = this.TreeViewTester.FindNode(0, 0, 1, 0);
            Assert.IsNotNull(templateNodeTester.Target);

            var updateTemplate = new MenuItemTester(Constants.UpdateTemplate);

            Assert.IsTrue(updateTemplate.Target.Visible);

            var templateFileInfo = SaveTemplate(
                "updated.stg", 
@"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<UpdatedTemplate>>
TemplateVersion() ::= <<1.0.1>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
");

            updateTemplate.Click();

            template.Source = new Uri(templateFileInfo.FullName);
            updateTemplate.Click();
            Assert.AreEqual("UpdatedTemplate", this.Repository.Templates[0].StringTemplate.Name);

            SaveTemplate(
                "updated.stg",
@"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateVersion() ::= <<1.0.1>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
");
            updateTemplate.Click();
            Assert.AreEqual("UpdatedTemplate", this.Repository.Templates[0].StringTemplate.Name);

            File.Delete(templateFileInfo.FullName);
            updateTemplate.Click();
            Assert.AreEqual("UpdatedTemplate", this.Repository.Templates[0].StringTemplate.Name);
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (this.testCaseSwitch)
            {
                case TestCaseSwitch.OpenRepository:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var openFileDialogTester = new OpenFileDialogTester(args.Tester);
                            openFileDialogTester.Target.DialogResult = DialogResult.OK;
                            openFileDialogTester.Target.FileName = RepositoryFile.FullName;
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.AddSuperDomain:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var openFileDialogTester = new OpenFileDialogTester(args.Tester);
                            openFileDialogTester.Target.DialogResult = DialogResult.OK;
                            openFileDialogTester.Target.FileName = this.SuperDomainRepositoryFileInfo.FullName;
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.DeleteType:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Do you really want to delete Singular and all its relations?", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.DeleteTypeWithCascading:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var deleteMessageBoxTester = new MessageBoxTester(args.Tester);
                            deleteMessageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Do you really want to delete Singular and all its relations?", deleteMessageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.DeleteRelation:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Do you really want to delete Singular.AllorsString?", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.DeleteSuperDomain:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Do you really want to delete SuperDomain?", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.UpdateTemplate:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Could not update template\n\nTemplate Url can not be null.", messageBoxTester.Target.Text);
                            break;
                        case 2:
                            messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.IsTrue(messageBoxTester.Target.Text.StartsWith("Could not update template\n\nMissing property TemplateName"));
                            break;
                        case 3:
                            messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.IsTrue(messageBoxTester.Target.Text.StartsWith("Could not update template"));
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.Generate:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("No templates defined.", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }
    }
}