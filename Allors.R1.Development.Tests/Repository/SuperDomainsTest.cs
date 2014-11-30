// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperDomainsTest.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository.Meta;
    using Allors.R1.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class SuperDomainsTest
    {
        private DirectoryInfo domainDirectoryInfo;
        private Repository domainRepository;
        private bool domainRepositoryInvalidatedCalled;

        private DirectoryInfo superDomainDirectoryInfo;
        private Repository superDomainRepository;

        private DirectoryInfo InheritancesDirectoryInfo
        {
            get
            {
                return new DirectoryInfo(Path.Combine(this.domainDirectoryInfo.FullName, "inheritances"));
            }
        }

        private DirectoryInfo ObjectTypesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.domainDirectoryInfo.FullName, "objectTypes")); }
        }

        private DirectoryInfo RelationTypesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.domainDirectoryInfo.FullName, "relationTypes")); }
        }

        [SetUp]
        public void SetUp()
        {
            this.domainDirectoryInfo = new DirectoryInfo("repository/domain");
            this.domainDirectoryInfo.DeleteRecursive();

            this.domainDirectoryInfo.Create();

            this.domainRepository = new Repository(this.domainDirectoryInfo, true);

            this.superDomainDirectoryInfo = new DirectoryInfo("repository/superdomain");
            this.superDomainDirectoryInfo.DeleteRecursive();

            this.superDomainDirectoryInfo.Create();

            this.superDomainRepository = new Repository(this.superDomainDirectoryInfo, true);
        }

        [Test]
        public void AddSameSuperDomain()
        {
            var exceptionThrown = false;
            try
            {
                this.domainRepository.AddSuper(this.domainDirectoryInfo);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            exceptionThrown = false;
            try
            {
                this.domainRepository.AddSuper(this.domainDirectoryInfo);
                Assert.Fail();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            exceptionThrown = false;
            try
            {
                this.domainRepository.AddSuper(this.domainDirectoryInfo);
                Assert.Fail();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void AddSuperDomain()
        {
            var superDomain = this.superDomainRepository.Domain;

            var superDomainAssociationType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAssociationType.SingularName = "SuperDomainAssociationType";
            superDomainAssociationType.PluralName = "SuperDomainAssociationTypes";

            var superDomainRoleType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainRoleType.SingularName = "SuperDomainRoleType";
            superDomainRoleType.PluralName = "SuperDomainRoleTypes";

            var superDomainAbstractType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAbstractType.SingularName = "SuperDomainAbstractType";
            superDomainAbstractType.PluralName = "SuperDomainAbstractTypes";

            var superDomainInterfaceType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainInterfaceType.SingularName = "SuperDomainInterfaceType";
            superDomainInterfaceType.PluralName = "SuperDomainInterfaceTypes";

            var superDomainAbstractInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainAbstractInheritance.Subtype = superDomainAssociationType;
            superDomainAbstractInheritance.Supertype = superDomainAbstractType;

            var superDomainInterfaceInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainInterfaceInheritance.Subtype = superDomainRoleType;
            superDomainInterfaceInheritance.Supertype = superDomainInterfaceType;

            var superDomainUnitRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainUnitRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainUnitRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.StringId);

            var superDomainCompositeRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainCompositeRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainCompositeRelationType.RoleType.ObjectType = superDomainRoleType;

            superDomainAssociationType.SendChangedEvent();
            superDomainRoleType.SendChangedEvent();
            superDomainAbstractType.SendChangedEvent();
            superDomainInterfaceType.SendChangedEvent();
            superDomainAbstractInheritance.SendChangedEvent();
            superDomainInterfaceInheritance.SendChangedEvent();
            superDomainUnitRelationType.SendChangedEvent();
            superDomainCompositeRelationType.SendChangedEvent();

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.repository"));
            var expectedXml =
@"<repository allors=""1.0"">
    <super id=""" + superDomain.Id + @""">
        <location>.." + Path.DirectorySeparatorChar + this.superDomainRepository.DirectoryInfo.Name + @"</location>
    </super>
</repository>";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.domain"));
            expectedXml = 
@"<domain allors=""1.0"" id=""" + domain.Id + @""">
    <super idref=""" + superDomain.Id + @"""/>
</domain>";
            this.AssertXml(expectedXml, xmlFileInfo);

            var domainSuperDomain = (Domain)domain.Domain.Find(superDomain.Id);
            var domainAssociationType = (ObjectType)domain.Domain.Find(superDomainAssociationType.Id);
            var domainRoleType = (ObjectType)domain.Domain.Find(superDomainRoleType.Id);
            var domainAbstractType = (ObjectType)domain.Domain.Find(superDomainAbstractType.Id);
            var domainInterfaceType = (ObjectType)domain.Domain.Find(superDomainInterfaceType.Id);
            var domainAbstractInheritance = (Inheritance)domain.Domain.Find(superDomainAbstractInheritance.Id);
            var domainInterfaceInheritance = (Inheritance)domain.Domain.Find(superDomainInterfaceInheritance.Id);
            var domainUnitRelationType = (RelationType)domain.Domain.Find(superDomainUnitRelationType.Id);
            var domainCompositeRelationType = (RelationType)domain.Domain.Find(superDomainCompositeRelationType.Id);
            
            Assert.AreEqual(0, this.ObjectTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.RelationTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.InheritancesDirectoryInfo.GetFiles().Length);
            
            Assert.AreEqual(0, this.ObjectTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.RelationTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.InheritancesDirectoryInfo.GetFiles().Length);

            Assert.AreEqual(superDomain.Id, domainSuperDomain.Id);
            Assert.AreEqual(superDomainAssociationType.Id, domainAssociationType.Id);
            Assert.AreEqual(superDomainRoleType.Id, domainRoleType.Id);
            Assert.AreEqual(superDomainAbstractType.Id, domainAbstractType.Id);
            Assert.AreEqual(superDomainInterfaceType.Id, domainInterfaceType.Id);
            Assert.AreEqual(superDomainAbstractInheritance.Id, domainAbstractInheritance.Id);
            Assert.AreEqual(superDomainInterfaceInheritance.Id, domainInterfaceInheritance.Id);
            Assert.AreEqual(superDomainUnitRelationType.Id, domainUnitRelationType.Id);
            Assert.AreEqual(superDomainCompositeRelationType.Id, domainCompositeRelationType.Id);

            xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.repository"));
            expectedXml =
@"<repository allors=""1.0"">
    <super id=""" + superDomain.Id + @""">
        <location>.." + Path.DirectorySeparatorChar + this.superDomainRepository.DirectoryInfo.Name + @"</location>
    </super>
</repository>";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.domain"));
            expectedXml = 
@"<domain allors=""1.0"" id=""" + domain.Id + @""">
    <super idref=""" + superDomain.Id + @"""/>
</domain>";
            this.AssertXml(expectedXml, xmlFileInfo);

            var dupclicateRepository = new Repository(this.domainDirectoryInfo);
            var duplicateDomain = dupclicateRepository.Domain;

            Assert.IsFalse(domain.IsValid);

            var duplicateSuperDomain = (Domain)duplicateDomain.Domain.Find(superDomain.Id);
            var duplicateAssociationType = (ObjectType)duplicateDomain.Domain.Find(superDomainAssociationType.Id);
            var duplicateRoleType = (ObjectType)duplicateDomain.Domain.Find(superDomainRoleType.Id);
            var duplicateAbstractType = (ObjectType)duplicateDomain.Domain.Find(superDomainAbstractType.Id);
            var duplicateInterfaceType = (ObjectType)duplicateDomain.Domain.Find(superDomainInterfaceType.Id);
            var duplicateAbstractInheritance = (Inheritance)duplicateDomain.Domain.Find(superDomainAbstractInheritance.Id);
            var duplicateInterfaceInheritance = (Inheritance)duplicateDomain.Domain.Find(superDomainInterfaceInheritance.Id);
            var duplicateUnitRelationType = (RelationType)duplicateDomain.Domain.Find(superDomainUnitRelationType.Id);
            var duplicateCompositeRelationType = (RelationType)duplicateDomain.Domain.Find(superDomainCompositeRelationType.Id);
            
            Assert.AreEqual(0, this.ObjectTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.RelationTypesDirectoryInfo.GetFiles().Length);
            Assert.AreEqual(0, this.InheritancesDirectoryInfo.GetFiles().Length);

            Assert.AreEqual(superDomain.Id, duplicateSuperDomain.Id);
            Assert.AreEqual(superDomainAssociationType.Id, duplicateAssociationType.Id);
            Assert.AreEqual(superDomainRoleType.Id, duplicateRoleType.Id);
            Assert.AreEqual(superDomainAbstractType.Id, duplicateAbstractType.Id);
            Assert.AreEqual(superDomainInterfaceType.Id, duplicateInterfaceType.Id);
            Assert.AreEqual(superDomainAbstractInheritance.Id, duplicateAbstractInheritance.Id);
            Assert.AreEqual(superDomainInterfaceInheritance.Id, duplicateInterfaceInheritance.Id);
            Assert.AreEqual(superDomainUnitRelationType.Id, duplicateUnitRelationType.Id);
            Assert.AreEqual(superDomainCompositeRelationType.Id, duplicateCompositeRelationType.Id);
        }

        [Test]
        public void DeleteSuperDomain()
        {
            var superDomain = this.superDomainRepository.Domain;

            var superDomainAssociationType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAssociationType.SingularName = "SuperDomainAssociationType";
            superDomainAssociationType.PluralName = "SuperDomainAssociationTypes";

            var superDomainRoleType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainRoleType.SingularName = "SuperDomainRoleType";
            superDomainRoleType.PluralName = "SuperDomainRoleTypes";

            var superDomainAbstractType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAbstractType.SingularName = "SuperDomainAbstractType";
            superDomainAbstractType.PluralName = "SuperDomainAbstractTypes";

            var superDomainInterfaceType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainInterfaceType.SingularName = "SuperDomainInterfaceType";
            superDomainInterfaceType.PluralName = "SuperDomainInterfaceTypes";

            var superDomainAbstractInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainAbstractInheritance.Subtype = superDomainAssociationType;
            superDomainAbstractInheritance.Supertype = superDomainAbstractType;

            var superDomainInterfaceInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainInterfaceInheritance.Subtype = superDomainRoleType;
            superDomainInterfaceInheritance.Supertype = superDomainInterfaceType;

            var superDomainUnitRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainUnitRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainUnitRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.StringId);

            var superDomainCompositeRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainCompositeRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainCompositeRelationType.RoleType.ObjectType = superDomainRoleType;

            superDomainAssociationType.SendChangedEvent();
            superDomainRoleType.SendChangedEvent();
            superDomainAbstractType.SendChangedEvent();
            superDomainInterfaceType.SendChangedEvent();
            superDomainAbstractInheritance.SendChangedEvent();
            superDomainInterfaceInheritance.SendChangedEvent();
            superDomainUnitRelationType.SendChangedEvent();
            superDomainCompositeRelationType.SendChangedEvent();

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var integratedSuperDomain = (Domain)domain.Domain.Find(superDomain.Id);

            this.domainRepository.MetaObjectChanged += this.DomainRepositoryMetaObjectChanged;

            this.domainRepository.RemoveSuper(integratedSuperDomain);

            var xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.repository"));
            var expectedXml = @"<repository allors=""1.0""/>";
            this.AssertXml(expectedXml, xmlFileInfo);

            xmlFileInfo = new FileInfo(Path.Combine(this.domainDirectoryInfo.FullName, "allors.domain"));
            expectedXml = @"
                        <domain allors=""1.0"" id=""" + domain.Id + @"""/>";
            this.AssertXml(expectedXml, xmlFileInfo);

            Assert.IsTrue(this.domainRepositoryInvalidatedCalled);

            this.domainRepository = new Repository(this.domainRepository.DirectoryInfo);

            domain = this.domainRepository.Domain;
            integratedSuperDomain = (Domain)domain.Domain.Find(superDomain.Id);

            Assert.IsNull(integratedSuperDomain);
        }

        private void AssertXml(string expectedXml, FileInfo xmlFileInfo)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);

            var actualXml = this.ToComparableXml(xmlDocument.InnerXml);
            Assert.AreEqual(this.ToComparableXml(expectedXml), actualXml);
        }

        private void DomainRepositoryMetaObjectChanged(object sender, RepositoryMetaObjectChangedEventArgs args)
        {
            if (args.MetaObject is Domain)
            {
                this.domainRepositoryInvalidatedCalled = true;
            }
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