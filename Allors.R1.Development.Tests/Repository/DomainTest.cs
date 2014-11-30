// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Allors.R1.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class DomainTest
    {
        private DirectoryInfo directoryInfo;

        private Repository repository;

        private DirectoryInfo InheritancesDirectoryInfo
        {
            get
            {
                return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "inheritances"));
            }
        }

        private DirectoryInfo ObjectTypesDirectoryInfo
        {
            get
            {
                return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "objectTypes"));
            }
        }

        private DirectoryInfo RelationTypesDirectoryInfo
        {
            get
            {
                return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "relationTypes"));
            }
        }

        [Test]
        public void DeleteRelation()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var associationObjectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            associationObjectType.SingularName = "AssociationType";
            associationObjectType.PluralName = "AssociationTypes";
            associationObjectType.SendChangedEvent();

            var roleObjectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            roleObjectType.SingularName = "RoleType";
            roleObjectType.PluralName = "RoleTypes";
            roleObjectType.SendChangedEvent();

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var association = relationType.AssociationType;
            association.ObjectType = associationObjectType;
            var role = relationType.RoleType;
            role.ObjectType = roleObjectType;
            relationType.SendChangedEvent();

            var xmlFileInfo =
                new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationType.Id + ".relationType"));

            xmlFileInfo.Refresh();
            Assert.IsTrue(xmlFileInfo.Exists);

            relationType.Delete();

            xmlFileInfo.Refresh();
            Assert.IsFalse(xmlFileInfo.Exists);
        }

        [Test]
        public void DeleteType()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            
            var typeId = Guid.NewGuid();
            var type = domain.AddDeclaredObjectType(typeId);
            type.SingularName = "SingularName";
            type.PluralName = "PluralName";

            Assert.IsTrue(domain.IsValid);

            type.SendChangedEvent();

            var xmlFileInfo = new FileInfo(
                Path.Combine(this.ObjectTypesDirectoryInfo.FullName, type.Id + ".objectType"));

            xmlFileInfo.Refresh();
            Assert.IsTrue(xmlFileInfo.Exists);

            type.Delete();

            xmlFileInfo.Refresh();
            Assert.IsFalse(xmlFileInfo.Exists);
        }

        [Test]
        public void Domain()
        {
            var domain = this.repository.Domain;

            var id = domain.Id;
            const string Name = "Test";

            domain.Name = Name;
            
            domain.SendChangedEvent();

            var xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.repository"));
            var expectedXml = @"<repository allors=""1.0"" />";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));
            expectedXml = @"<domain allors=""1.0"" id=""" + id + @""">
<name>" + Name + @"</name>
</domain>";
            this.AssertXml(expectedXml, xmlFileInfo);

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;

            Assert.AreEqual(id, duplicateDomain.Id);
            Assert.AreEqual(Name, duplicateDomain.Name);
        }

        [Test]
        public void DomainDefaultId()
        {
            const string Name = "Test";

            var xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));
            using (var writer = xmlFileInfo.CreateText())
            {
                writer.Write(@"
<domain allors=""1.0"">
    <name>" + Name + @"</name>
</domain>");
                writer.Close();
            }

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;

            Assert.IsTrue(duplicateDomain.ExistId);
            Assert.AreNotEqual(Guid.Empty, duplicateDomain.Id);
            Assert.AreEqual(Name, duplicateDomain.Name);

            xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));
            var expectedXml = @"
<domain allors=""1.0"" id=""" + duplicateDomain.Id
            + @""">
<name>" + Name
            + @"</name>
</domain>";
            this.AssertXml(expectedXml, xmlFileInfo);
        }

        [Test]
        public void IncompleteDomain()
        {
            // TODO: Unresolved namespace
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            Assert.IsTrue(domain.IsValid);

            var relationId = new Guid("4A657392-6E7F-40ef-BBD8-D27779740A61");
            var associationObjectTypeId = new Guid("5BA60E4D-645A-4490-BCE6-A3BF79384BAB");
            var roleTypeObjectId = new Guid("0A3FC7B0-A938-4644-8584-ECECBF6F3BB5");

            var relationXml = @"<relationType allors=""1.0"" id=""" + relationId + @""">
<associationType>
    <objectType idref=""" + associationObjectTypeId + @"""/>
</associationType>
<roleType>
    <objectType idref=""" + roleTypeObjectId + @"""/>
</roleType>
</relationType>";
            var relationFileInfo =
                new FileInfo(
                    Path.Combine(
                        this.RelationTypesDirectoryInfo.FullName, relationId.ToString().ToLower() + ".relationType"));
            File.WriteAllText(relationFileInfo.FullName, relationXml);

            var duplicateRepository = new Repository(this.directoryInfo);
            var duplicateDomain = duplicateRepository.Domain;
            var relationType = (RelationType)duplicateDomain.Domain.Find(relationId);

            Assert.IsTrue(relationType.AssociationType.ExistObjectType);
            var associationObjectType = relationType.AssociationType.ObjectType;
            Assert.AreEqual(associationObjectTypeId, associationObjectType.Id);

            Assert.IsTrue(relationType.RoleType.ExistObjectType);
            var roleObjectType = relationType.RoleType.ObjectType;
            Assert.AreEqual(roleTypeObjectId, roleObjectType.Id);

            Assert.AreEqual(2, duplicateRepository.UnresolvedTypeIds.Length);
            Assert.Contains(associationObjectTypeId.ToString(), duplicateRepository.UnresolvedTypeIds);
            Assert.Contains(roleTypeObjectId.ToString(), duplicateRepository.UnresolvedTypeIds);
        }

        [Test]
        public void Inheritance()
        {
            var superDomain = this.repository.Domain;
            superDomain.Name = "MySuperDomain";
            superDomain.SendChangedEvent();

            var abstractClassId = Guid.NewGuid();
            var abstractClass = superDomain.AddDeclaredObjectType(abstractClassId);
            abstractClass.SingularName = "AbstractClass";
            abstractClass.PluralName = "AbstractClasses";
            abstractClass.IsAbstract = true;
            abstractClass.SendChangedEvent();

            var concreteClassId = Guid.NewGuid();
            var concreteClass = superDomain.AddDeclaredObjectType(concreteClassId);
            concreteClass.SingularName = "ConcreteClass";
            concreteClass.PluralName = "ConcreteClasses";
            concreteClass.SendChangedEvent();

            var inheritanceId = Guid.NewGuid();
            var inheritance = superDomain.AddDeclaredInheritance(inheritanceId);
            inheritance.Subtype = concreteClass;
            inheritance.Supertype = abstractClass;
            inheritance.SendChangedEvent();

            Assert.IsTrue(superDomain.IsValid);

            var xmlFileInfo =
                new FileInfo(Path.Combine(this.InheritancesDirectoryInfo.FullName, inheritance.Id + ".inheritance"));
            xmlFileInfo.Refresh();
            Assert.IsTrue(xmlFileInfo.Exists);

            var expectedXml = 
@"<inheritance allors=""1.0"" id=""" + inheritance.Id.ToString().ToLower() + @""">
    <subtype idref=""" + concreteClassId + @"""/>
    <supertype idref=""" + abstractClassId + @"""/>
</inheritance>";
            this.AssertXml(expectedXml, xmlFileInfo);

            inheritance.Delete();

            xmlFileInfo.Refresh();
            Assert.IsFalse(xmlFileInfo.Exists);
        }

        [Test]
        public void MinimalRelationType()
        {
            var domain = this.repository.Domain;
            domain.SendChangedEvent();

            var relationTypeId = new Guid("{5131EDF5-F524-4b34-AF01-20EBB4CA3ABA}");
            var associationTypeId = new Guid("{5CF7B5E2-873A-469B-A3F4-BB009197E0B8}");
            var roleTypeId = new Guid("{785158F1-B0E9-45CC-B24D-22C9CDEAA857}");

            var relationType = domain.AddDeclaredRelationType(relationTypeId, associationTypeId, roleTypeId);
            relationType.SendChangedEvent();

            var xmlFileInfo = new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationTypeId.ToString().ToLower() + ".relationType"));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var expectedXml = this.ToComparableXml(@"<relationType allors=""1.0"" id=" + '"' + relationTypeId.ToString().ToLower() + '"' + @">
<associationType id=" + '"' + associationTypeId.ToString().ToLower() + '"' + @"/>
<roleType id=" + '"' + roleTypeId.ToString().ToLower() + '"' + @"/>
</relationType>");
            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;
            var duplicateRelationType = (RelationType)duplicateDomain.Domain.Find(relationTypeId);

            Assert.AreEqual(relationType.Id, duplicateRelationType.Id);
        }

        [Test]
        public void MinimalSuperDomain()
        {
            var domain = this.repository.Domain;

            var xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.repository"));
            var expectedXml = @"<repository allors=""1.0"" />";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));
            expectedXml = @"<domain allors=""1.0"" id=" + '"' + this.repository.Domain.Id + '"' + @"/>";
            this.AssertXml(expectedXml, xmlFileInfo);

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;

            Assert.AreEqual(domain.Id, duplicateDomain.Id);
        }

        [Test]
        public void MinimalObjectType()
        {
            var domain = this.repository.Domain;
            domain.SendChangedEvent();
            
            var id = new Guid("{665799E7-1C3B-45df-AA5F-335ED60CE7EB}");
            var objectType = domain.AddDeclaredObjectType(id);
            objectType.SendChangedEvent();

            var xmlFileInfo = new FileInfo(Path.Combine(this.ObjectTypesDirectoryInfo.FullName, id.ToString().ToLower() + ".objectType"));
            var expectedXml = @"<objectType allors=""1.0"" id=" + '"' + id.ToString().ToLower() + '"' + @"/>";
            this.AssertXml(expectedXml, xmlFileInfo);

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;
            var duplicateType = (ObjectType)duplicateDomain.Domain.Find(id);

            Assert.AreEqual(id, duplicateType.Id);
        }

        [Test]
        public void RelationType()
        {
            var superDomain = this.repository.Domain;
            superDomain.Name = "MySuperDomain";
            superDomain.SendChangedEvent();

            var associationObjectType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            associationObjectType.SingularName = "AssociationType";
            associationObjectType.PluralName = "AssociationTypes";
            associationObjectType.SendChangedEvent();

            var roleObjectType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            roleObjectType.SingularName = "RoleType";
            roleObjectType.PluralName = "RoleTypes";
            roleObjectType.SendChangedEvent();

            var associationSortRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            associationSortRelationType.AssociationType.ObjectType = associationObjectType;
            associationSortRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.StringId);
            associationSortRelationType.RoleType.Size = 100;
            associationSortRelationType.SendChangedEvent();

            var roleSortRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            roleSortRelationType.AssociationType.ObjectType = roleObjectType;
            roleSortRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.IntegerId);
            roleSortRelationType.SendChangedEvent();

            var role2SortRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            role2SortRelationType.AssociationType.ObjectType = roleObjectType;
            role2SortRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.DecimalId);
            role2SortRelationType.RoleType.Precision = 10;
            role2SortRelationType.RoleType.Scale = 2;
            role2SortRelationType.SendChangedEvent();

            var relationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.IsIndexed = true;

            var association = relationType.AssociationType;
            association.ObjectType = associationObjectType;
            association.AssignedSingularName = "AssociationType";
            association.AssignedPluralName = "AssociationTypes";
            association.IsMany = true;

            var role = relationType.RoleType;
            role.ObjectType = roleObjectType;
            role.AssignedSingularName = "Role";
            role.AssignedPluralName = "RoleTypes";
            role.IsMany = true;

            relationType.SendChangedEvent();

            Assert.IsTrue(superDomain.IsValid);

            var xmlFileInfo =
                new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationType.Id + ".relationType"));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var expectedXml = this.ToComparableXml(
@"<relationType allors=""1.0"" id=" + '"' + relationType.Id + '"' + @">
    <associationType id=""" + relationType.AssociationType.Id + @""">
        <isMany>true</isMany>
        <objectType idref=""" + associationObjectType.Id + @"""/>
        <pluralName>AssociationTypes</pluralName>
        <singularName>AssociationType</singularName>
    </associationType>
    
    <isIndexed>true</isIndexed>
    
    <roleType id=""" + relationType.RoleType.Id + @""">
        <isMany>true</isMany>
        <objectType idref=""" + roleObjectType.Id + @"""/>
        <pluralName>RoleTypes</pluralName>
        <singularName>Role</singularName>
    </roleType>

</relationType>");
            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;

            var duplicateRelationType = (RelationType)duplicateDomain.Domain.Find(relationType.Id);
            var duplicateAssociation = duplicateRelationType.AssociationType;
            var duplicateRole = duplicateRelationType.RoleType;

            Assert.AreEqual(relationType.Id, duplicateRelationType.Id);
            Assert.AreEqual(relationType.IsIndexed, duplicateRelationType.IsIndexed);

            Assert.AreEqual(association.ObjectType.Id, duplicateAssociation.ObjectType.Id);
            Assert.AreEqual(association.ObjectType.SingularName, duplicateAssociation.ObjectType.SingularName);
            Assert.AreEqual(association.ObjectType.PluralName, duplicateAssociation.ObjectType.PluralName);
            Assert.AreEqual(association.AssignedSingularName, duplicateAssociation.AssignedSingularName);
            Assert.AreEqual(association.AssignedPluralName, duplicateAssociation.AssignedPluralName);
            Assert.AreEqual(association.IsMany, duplicateAssociation.IsMany);

            Assert.AreEqual(role.ObjectType.Id, duplicateRole.ObjectType.Id);
            Assert.AreEqual(role.ObjectType.SingularName, duplicateRole.ObjectType.SingularName);
            Assert.AreEqual(role.ObjectType.PluralName, duplicateRole.ObjectType.PluralName);
            Assert.AreEqual(role.AssignedSingularName, duplicateRole.AssignedSingularName);
            Assert.AreEqual(role.AssignedPluralName, duplicateRole.AssignedPluralName);
            Assert.AreEqual(role.IsMany, duplicateRole.IsMany);
        }

        [Test]
        public void RelationTypeDefaults()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var associationObjectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            associationObjectType.SingularName = "AssociationType";
            associationObjectType.PluralName = "AssociationTypes";
            associationObjectType.SendChangedEvent();

            var roleObjectType = domain.AddDeclaredObjectType(Guid.NewGuid());
            roleObjectType.SingularName = "RoleType";
            roleObjectType.PluralName = "RoleTypes";
            roleObjectType.SendChangedEvent();

            var associationSortRelationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            associationSortRelationType.AssociationType.ObjectType = associationObjectType;
            associationSortRelationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);
            associationSortRelationType.RoleType.Size = 200;
            associationSortRelationType.SendChangedEvent();

            var roleSortRelationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            roleSortRelationType.AssociationType.ObjectType = roleObjectType;
            roleSortRelationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.IntegerId);
            roleSortRelationType.SendChangedEvent();

            var role2SortRelationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            role2SortRelationType.AssociationType.ObjectType = roleObjectType;
            role2SortRelationType.RoleType.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.DecimalId);
            role2SortRelationType.RoleType.Precision = 10;
            role2SortRelationType.RoleType.Scale = 2;
            role2SortRelationType.SendChangedEvent();

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.IsIndexed = true;
            relationType.IsDerived = true;

            var association = relationType.AssociationType;
            association.ObjectType = associationObjectType;
            association.AssignedSingularName = "AssociationType";
            association.AssignedPluralName = "AssociationTypes";
            association.IsMany = true;

            var role = relationType.RoleType;
            role.ObjectType = roleObjectType;
            role.AssignedSingularName = "Role";
            role.AssignedPluralName = "RoleTypes";
            role.IsMany = true;
            role.Size = 20;
            role.Scale = 3;

            relationType.SendChangedEvent();

            Assert.IsTrue(domain.IsValid);

            relationType.IsIndexed = false;
            relationType.IsDerived = false;

            association.RemoveObjectType();
            association.RemoveAssignedSingularName();
            association.RemoveAssignedPluralName();
            association.IsMany = false;

            role.RemoveObjectType();
            role.RemoveAssignedSingularName();
            role.RemoveAssignedPluralName();
            role.IsMany = false;
            role.RemoveSize();
            role.RemoveScale();

            relationType.SendChangedEvent();

            var xmlFileInfo =
                new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationType.Id + ".relationType"));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var expectedXml = this.ToComparableXml(
@"<relationType allors=""1.0"" id=" + '"' + relationType.Id + '"' + @">
    <associationType id=" + '"' + relationType.AssociationType.Id + '"' + @"/>
    <roleType id=" + '"' + relationType.RoleType.Id + '"' + @"/>
</relationType>");
            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);
        }

        [SetUp]
        public void SetUp()
        {
            this.directoryInfo = new DirectoryInfo("domain");
            this.directoryInfo.DeleteRecursive();

            this.directoryInfo.Create();

            this.repository = new Repository(this.directoryInfo, true);
        }

        [Test]
        public void ObjectType()
        {
            var superDomain = this.repository.Domain;
            superDomain.Name = "MySuperDomain";
            superDomain.SendChangedEvent();

            var superclassId = Guid.NewGuid();
            var superclass = superDomain.AddDeclaredObjectType(superclassId);
            superclass.SingularName = "SuperName";
            superclass.PluralName = "SuperNames";
            superclass.IsAbstract = true;
            superclass.SendChangedEvent();

            var typeId = Guid.NewGuid();
            var type = superDomain.AddDeclaredObjectType(typeId);
            type.SingularName = "SingularName";
            type.PluralName = "PluralName";
            type.SendChangedEvent();

            var inheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance.Subtype = type;
            inheritance.Supertype = superclass;
            inheritance.SendChangedEvent();

            superDomain.Validate();
            Assert.IsTrue(superDomain.IsValid);

            var xmlFileInfo = new FileInfo(
                Path.Combine(this.ObjectTypesDirectoryInfo.FullName, type.Id + ".objectType"));
            var expectedXml = 
@"<objectType allors=""1.0"" id=" + '"' + typeId + '"' + @">
    <pluralName>PluralName</pluralName>
    <singularName>SingularName</singularName>
</objectType>";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.InheritancesDirectoryInfo.FullName, inheritance.Id + ".inheritance"));
            expectedXml = 
@"<inheritance allors=""1.0"" id=""" + inheritance.Id.ToString().ToLower() + @""">
    <subtype idref=""" + typeId + @"""/>
    <supertype idref=""" + superclass.Id + @"""/>
</inheritance>";
            this.AssertXml(expectedXml, xmlFileInfo);

            // Duplicate
            var duplicate = new Repository(this.directoryInfo);
            var duplicateDomain = duplicate.Domain;
            var duplicateType = (ObjectType)duplicateDomain.Domain.Find(type.Id);
            var duplicateSuperclass = (ObjectType)duplicateDomain.Domain.Find(superclass.Id);

            Assert.AreNotEqual(type, duplicateType);
            Assert.AreNotSame(type, duplicateType);

            Assert.AreEqual(duplicateType, duplicateDomain.Domain.Find(type.Id));
            Assert.AreEqual(duplicateSuperclass, duplicateDomain.Domain.Find(superclass.Id));

            Assert.AreEqual(type.Id, duplicateType.Id);
            Assert.AreEqual(type.SingularName, duplicateType.SingularName);
            Assert.AreEqual(type.PluralName, duplicateType.PluralName);
            Assert.AreEqual(1, duplicateType.DirectSupertypes.Length);
            Assert.AreEqual(duplicateSuperclass, duplicateType.DirectSupertypes[0]);
        }

        [Test]
        public void ObjectTypeDefaults()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var superclassId = Guid.NewGuid();
            var superclass = domain.AddDeclaredObjectType(superclassId);
            superclass.SingularName = "SuperName";
            superclass.PluralName = "SuperNames";
            superclass.IsAbstract = true;
            superclass.SendChangedEvent();

            var typeId = Guid.NewGuid();
            var type = domain.AddDeclaredObjectType(typeId);
            type.SingularName = "SingularName";
            type.PluralName = "PluralName";
            type.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance.Subtype = type;
            inheritance.Supertype = superclass;
            inheritance.SendChangedEvent();

            type.IsAbstract = true;
            type.SendChangedEvent();

            Assert.IsTrue(domain.IsValid);

            // set back to defaults
            type.RemoveSingularName();
            type.RemovePluralName();

            foreach (var deleteInheritance in type.InheritancesWhereSubtype)
            {
                deleteInheritance.Delete();
            }

            type.IsAbstract = false;

            superclass.SendChangedEvent();
            type.SendChangedEvent();
            domain.SendChangedEvent();

            var xmlFileInfo = new FileInfo(Path.Combine(this.ObjectTypesDirectoryInfo.FullName, type.Id + ".objectType"));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var expectedXml = this.ToComparableXml(@"<objectType allors=""1.0"" id=" + '"' + typeId.ToString().ToLower() + '"' + @"/>");
            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);
        }
        
        [Test]
        public void Reload()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var superclassId = Guid.NewGuid();
            var superclass = domain.AddDeclaredObjectType(superclassId);
            superclass.SingularName = "SuperName";
            superclass.PluralName = "SuperNames";
            superclass.IsAbstract = true;
            superclass.SendChangedEvent();

            var subClassId = Guid.NewGuid();
            var subClas = domain.AddDeclaredObjectType(subClassId);
            subClas.SingularName = "SingularName";
            subClas.PluralName = "PluralName";
            subClas.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance.Subtype = subClas;
            inheritance.Supertype = superclass;
            inheritance.SendChangedEvent();
            
            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var association = relationType.AssociationType;
            association.ObjectType = subClas;
            var role = relationType.RoleType;
            role.ObjectType = (ObjectType)domain.Domain.Find(UnitTypeIds.StringId);
            role.Size = -1;
            relationType.SendChangedEvent();

            var validation = domain.Validate();
            Assert.IsFalse(validation.ContainsErrors);

            this.repository.Reload();

            domain = this.repository.Domain;

            Assert.AreEqual(1, domain.DeclaredInheritances.Length);
            Assert.AreEqual(2, domain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void ObjectTypeDefaults2()
        {
            // IsInterface && IsMultiple are mutually exclusive with IsAbstract
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var superclassId = Guid.NewGuid();
            var superclass = domain.AddDeclaredObjectType(superclassId);
            superclass.SingularName = "SuperName";
            superclass.PluralName = "SuperNames";

            var typeId = Guid.NewGuid();
            var type = domain.AddDeclaredObjectType(typeId);

            type.IsInterface = true;

            superclass.SendChangedEvent();
            type.SendChangedEvent();
            domain.SendChangedEvent();

            // set back to defautls
            type.IsInterface = false;

            superclass.SendChangedEvent();
            type.SendChangedEvent();
            domain.SendChangedEvent();

            var xmlFileInfo = new FileInfo(Path.Combine(this.ObjectTypesDirectoryInfo.FullName, type.Id + ".objectType"));
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var expectedXml = this.ToComparableXml(@"<objectType allors=""1.0"" id=" + '"' + typeId.ToString().ToLower() + '"' + @"/>");
            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);
        }

        private void AssertXml(string expectedXml, FileInfo xmlFileInfo)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(this.ToComparableXml(expectedXml), actualXml);
        }

        private string ToComparableXml(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            xmlDocument.Normalize();

            var stripWhiteSpace = new Regex(@">(\n|\s)*<");
            return stripWhiteSpace.Replace(xmlDocument.InnerXml, "><");
        }
    }
}