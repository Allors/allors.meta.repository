// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullUpObjectTypeTest.cs" company="Allors bvba">
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

    using Allors.R1.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class PullUpObjectTypeTest
    {
        private DirectoryInfo domainDirectoryInfo;
        private Repository domainRepository;

        private DirectoryInfo superDomainDirectoryInfo;
        private Repository superDomainRepository;

        private DirectoryInfo parentSuperDomainDirectoryInfo;
        private Repository parentSuperDomainRepository;

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
            
            this.parentSuperDomainDirectoryInfo = new DirectoryInfo("repository/parentsuperdomain");
            this.parentSuperDomainDirectoryInfo.DeleteRecursive();
            this.parentSuperDomainDirectoryInfo.Create();
            this.parentSuperDomainRepository = new Repository(this.parentSuperDomainDirectoryInfo, true);
        }

        [Test]
        public void WithoutDependencies()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var superTypeId = new Guid("EE47525F-2529-4089-A373-9CF76E67DE2E");
            var inheritanceId = new Guid("53DEDB02-4693-44A5-9D27-712754E3AE90");
            var relationTypeId = new Guid("0B5D66B3-B480-497C-A5A6-50C0250FBAFB");
            var associationTypeId = new Guid("1DCD8E15-D57F-4F66-9C47-6F4273135D1A");
            var roleTypeId = new Guid("DAF2DAE4-9382-48D0-9252-CB7AF07FCF0D");

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            var objectType = domain.AddDeclaredObjectType(objectTypeId);
            objectType.SingularName = "ObjectType";
            objectType.PluralName = "ObjectTypes";
            objectType.SendChangedEvent();

            var superType = domain.AddDeclaredObjectType(superTypeId);
            superType.SingularName = "SuperType";
            superType.PluralName = "SuperTypes";
            superType.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(inheritanceId);
            inheritance.Subtype = objectType;
            inheritance.Supertype = superType;
            inheritance.SendChangedEvent();

            var relationType = domain.AddDeclaredRelationType(relationTypeId, associationTypeId, roleTypeId);
            relationType.AssociationType.ObjectType = objectType;
            relationType.RoleType.ObjectType = superType;
            relationType.SendChangedEvent();

            var pullUp = this.domainRepository.PullUp(domainSuperDomain, objectType, false);

            Assert.AreEqual(0, pullUp.InheritancesToPull.Length);

            Assert.AreEqual(0, pullUp.RelationTypesToPull.Length);

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            pullUp.Execute();

            Assert.AreEqual(1, pullUp.MetaObjectsToPull.Length);
            Assert.AreEqual(objectType, pullUp.MetaObjectsToPull[0]);

            this.superDomainRepository.Reload();
            var superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);
            var superDomainObjectType = (ObjectType)superDomainDomain.Domain.Find(objectTypeId);
            Assert.IsNotNull(superDomainObjectType);
            Assert.AreEqual(superDomainDomain, superDomainObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("ObjectType", superDomainObjectType.SingularName);
            Assert.AreEqual("ObjectTypes", superDomainObjectType.PluralName);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(1, domainSuperDomain.DeclaredObjectTypes.Length);
            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domainSuperDomain, objectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);
            superType = (ObjectType)domain.Domain.Find(superTypeId);
            Assert.AreEqual("SuperType", superType.SingularName);
            Assert.AreEqual("SuperTypes", superType.PluralName);
            Assert.AreEqual(domain, superType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);
            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(superType, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(1, domain.DeclaredRelationTypes.Length);
            relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(associationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(objectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(roleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(superType, relationType.RoleType.ObjectType);
            Assert.AreEqual(domain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void IncludesWithoutDependencies()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var superTypeId = new Guid("EE47525F-2529-4089-A373-9CF76E67DE2E");
            var inheritanceId = new Guid("53DEDB02-4693-44A5-9D27-712754E3AE90");
            var relationTypeId = new Guid("0B5D66B3-B480-497C-A5A6-50C0250FBAFB");
            var associationTypeId = new Guid("1DCD8E15-D57F-4F66-9C47-6F4273135D1A");
            var roleTypeId = new Guid("DAF2DAE4-9382-48D0-9252-CB7AF07FCF0D");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainSuperType = superDomainDomain.AddDeclaredObjectType(superTypeId);
            superDomainSuperType.SingularName = "SuperType";
            superDomainSuperType.PluralName = "SuperTypes";
            superDomainSuperType.SendChangedEvent();

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            var superType = (ObjectType)domain.Domain.Find(superTypeId);

            var objectType = domain.AddDeclaredObjectType(objectTypeId);
            objectType.SingularName = "ObjectType";
            objectType.PluralName = "ObjectTypes";
            objectType.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(inheritanceId);
            inheritance.Subtype = objectType;
            inheritance.Supertype = superType;
            inheritance.SendChangedEvent();

            var relationType = domain.AddDeclaredRelationType(relationTypeId, associationTypeId, roleTypeId);
            relationType.AssociationType.ObjectType = objectType;
            relationType.RoleType.ObjectType = superType;
            relationType.SendChangedEvent();

            var pullUp = this.domainRepository.PullUp(domainSuperDomain, objectType, true);

            Assert.AreEqual(1, pullUp.InheritancesToPull.Length);
            Assert.AreEqual(inheritance, pullUp.InheritancesToPull[0]);

            Assert.AreEqual(1, pullUp.RelationTypesToPull.Length);
            Assert.AreEqual(relationType, pullUp.RelationTypesToPull[0]);

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            pullUp.Execute();

            Assert.AreEqual(3, pullUp.MetaObjectsToPull.Length);
            Assert.Contains(objectType, pullUp.MetaObjectsToPull);
            Assert.Contains(inheritance, pullUp.MetaObjectsToPull);
            Assert.Contains(relationType, pullUp.MetaObjectsToPull);

            this.superDomainRepository.Reload();
            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);
            var superDomainObjectType = (ObjectType)superDomainDomain.Domain.Find(objectTypeId);
            Assert.IsNotNull(superDomainObjectType);
            Assert.AreEqual(superDomainDomain, superDomainObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("ObjectType", superDomainObjectType.SingularName);
            Assert.AreEqual("ObjectTypes", superDomainObjectType.PluralName);

            superDomainSuperType = (ObjectType)superDomainDomain.Domain.Find(superTypeId);
            Assert.IsNotNull(superDomainSuperType);
            Assert.AreEqual(superDomainDomain, superDomainSuperType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("SuperType", superDomainSuperType.SingularName);
            Assert.AreEqual("SuperTypes", superDomainSuperType.PluralName);

            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);
            var superDomainInheritance = (Inheritance)superDomainDomain.Domain.Find(inheritanceId);
            Assert.AreEqual(superDomainObjectType, superDomainInheritance.Subtype);
            Assert.AreEqual(superDomainSuperType, superDomainInheritance.Supertype);
            Assert.AreEqual(superDomainDomain, superDomainInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);
            var superDomainRelationType = (RelationType)superDomainDomain.Domain.Find(relationTypeId);
            Assert.AreEqual(associationTypeId, superDomainRelationType.AssociationType.Id);
            Assert.AreEqual(superDomainObjectType, superDomainRelationType.AssociationType.ObjectType);
            Assert.AreEqual(roleTypeId, superDomainRelationType.RoleType.Id);
            Assert.AreEqual(superDomainSuperType, superDomainRelationType.RoleType.ObjectType);
            Assert.AreEqual(superDomainDomain, superDomainRelationType.DomainWhereDeclaredRelationType);

            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domainSuperDomain, objectType.DomainWhereDeclaredObjectType);

            superType = (ObjectType)domain.Domain.Find(superTypeId);
            Assert.AreEqual("SuperType", superType.SingularName);
            Assert.AreEqual("SuperTypes", superType.PluralName);
            Assert.AreEqual(domainSuperDomain, superType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(0, domain.DeclaredInheritances.Length);
            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(superType, inheritance.Supertype);
            Assert.AreEqual(domainSuperDomain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
            relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(associationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(objectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(roleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(superType, relationType.RoleType.ObjectType);
            Assert.AreEqual(domainSuperDomain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void WithDependentRelationTypeInParentSuperDomain()
        {
            var fromObjectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var toObjectTypeId = new Guid("15EB1B2D-A4B2-4836-B163-98C236B0365B");
            var relationTypeId = new Guid("183E8A7A-55A1-4813-855B-ED8CD4313926");

            var parentSuperDomainDomain = this.parentSuperDomainRepository.Domain;

            var toObjectType = parentSuperDomainDomain.AddDeclaredObjectType(toObjectTypeId);
            toObjectType.SingularName = "To";
            toObjectType.PluralName = "Tos";
            toObjectType.SendChangedEvent();

            this.superDomainRepository.AddSuper(this.parentSuperDomainDirectoryInfo);
            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            toObjectType = (ObjectType)domain.Domain.Find(toObjectTypeId);

            var fromObjectType = domain.AddDeclaredObjectType(fromObjectTypeId);
            fromObjectType.SingularName = "From";
            fromObjectType.PluralName = "Froms";
            fromObjectType.SendChangedEvent();

            var relationType = domain.AddDeclaredRelationType(relationTypeId, Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = fromObjectType;
            relationType.RoleType.ObjectType = toObjectType;
            relationType.SendChangedEvent();

            var pullUp = this.domainRepository.PullUp(domainSuperDomain, fromObjectType, true);

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(fromObjectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(1, pullUp.RelationTypesToPull.Length);
            Assert.AreEqual(relationType, pullUp.RelationTypesToPull[0]);

            pullUp.Execute();

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(fromObjectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(1, pullUp.RelationTypesToPull.Length);
            Assert.AreEqual(relationType, pullUp.RelationTypesToPull[0]);

            this.domainRepository.Reload();
            this.superDomainRepository.Reload();
            this.parentSuperDomainRepository.Reload();

            var superDomainDomain = this.superDomainRepository.Domain;
            var superDomainDomainParentSuperDomain = (Domain)superDomainDomain.Domain.Find(this.parentSuperDomainRepository.Id);

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);
            fromObjectType = superDomainDomain.DeclaredObjectTypes[0];
            Assert.IsNotNull(fromObjectType);
            Assert.AreEqual(superDomainDomain, fromObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("From", fromObjectType.SingularName);
            Assert.AreEqual("Froms", fromObjectType.PluralName);

            Assert.AreEqual(1, superDomainDomainParentSuperDomain.DeclaredObjectTypes.Length);
            toObjectType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[0];
            Assert.IsNotNull(toObjectType);
            Assert.AreEqual(superDomainDomainParentSuperDomain, toObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("To", toObjectType.SingularName);
            Assert.AreEqual("Tos", toObjectType.PluralName);

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);
            relationType = superDomainDomain.DeclaredRelationTypes[0];
            Assert.IsNotNull(relationType);
            Assert.AreEqual(superDomainDomain, relationType.DomainWhereDeclaredRelationType);
            Assert.AreEqual(fromObjectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(toObjectType, relationType.RoleType.ObjectType);
        }

        [Test]
        public void WithDependentInheritanceInParentSuperDomain()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var superObjectTypeId = new Guid("15EB1B2D-A4B2-4836-B163-98C236B0365B");
            var inheritanceId = new Guid("FF8F388A-B6F8-43C4-B9D4-47FD5917B223");
             
            var parentSuperDomainDomain = this.parentSuperDomainRepository.Domain;

            var superObjectType = parentSuperDomainDomain.AddDeclaredObjectType(superObjectTypeId);
            superObjectType.SingularName = "SuperType";
            superObjectType.PluralName = "SuperTypes";
            superObjectType.SendChangedEvent();

            this.superDomainRepository.AddSuper(this.parentSuperDomainDirectoryInfo);
            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            superObjectType = (ObjectType)domain.Domain.Find(superObjectTypeId);

            var objectType = domain.AddDeclaredObjectType(objectTypeId);
            objectType.SingularName = "ObjectType";
            objectType.PluralName = "ObjectTypes";
            objectType.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(inheritanceId);
            inheritance.Subtype = objectType;
            inheritance.Supertype = superObjectType;
            inheritance.SendChangedEvent();

            var pullUp = this.domainRepository.PullUp(domainSuperDomain, objectType, true);

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(1, pullUp.InheritancesToPull.Length);
            Assert.AreEqual(inheritance, pullUp.InheritancesToPull[0]);

            pullUp.Execute();

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(1, pullUp.InheritancesToPull.Length);
            Assert.AreEqual(inheritance, pullUp.InheritancesToPull[0]);

            this.domainRepository.Reload();
            this.superDomainRepository.Reload();
            this.parentSuperDomainRepository.Reload();

            var superDomainDomain = this.superDomainRepository.Domain;
            var superDomainDomainParentSuperDomain = (Domain)superDomainDomain.Domain.Find(this.parentSuperDomainRepository.Id);

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);
            objectType = superDomainDomain.DeclaredObjectTypes[0];
            Assert.IsNotNull(objectType);
            Assert.AreEqual(superDomainDomain, objectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);

            Assert.AreEqual(1, superDomainDomainParentSuperDomain.DeclaredObjectTypes.Length);
            var superType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[0];
            Assert.IsNotNull(superType);
            Assert.AreEqual(superDomainDomainParentSuperDomain, superType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("SuperType", superType.SingularName);
            Assert.AreEqual("SuperTypes", superType.PluralName);

            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);
            inheritance  = superDomainDomain.DeclaredInheritances[0];
            Assert.IsNotNull(inheritance);
            Assert.AreEqual(superDomainDomain, inheritance.DomainWhereDeclaredInheritance);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(superType, inheritance.Supertype);
        }

        [Test]
        public void WithDependentInheritanceRelationTypeInParentSuperDomain()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var superObjectTypeId = new Guid("15EB1B2D-A4B2-4836-B163-98C236B0365B");
            var toObjectTypeId = new Guid("05433F9B-6B1C-46F3-B0BB-9F6D1FF918F9");
            var relationTypeId = new Guid("34BBEE99-7D8B-4314-B5B6-7162662B74A3");
            var inheritanceId = new Guid("FF8F388A-B6F8-43C4-B9D4-47FD5917B223");

            var parentSuperDomainDomain = this.parentSuperDomainRepository.Domain;

            var superObjectType = parentSuperDomainDomain.AddDeclaredObjectType(superObjectTypeId);
            superObjectType.SingularName = "SuperType";
            superObjectType.PluralName = "SuperTypes";
            superObjectType.SendChangedEvent();

            var toObjectType = parentSuperDomainDomain.AddDeclaredObjectType(toObjectTypeId);
            toObjectType.SingularName = "To";
            toObjectType.PluralName = "Tos";
            toObjectType.SendChangedEvent();

            var relationType = parentSuperDomainDomain.AddDeclaredRelationType(relationTypeId, Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = superObjectType;
            relationType.RoleType.ObjectType = toObjectType;
            relationType.SendChangedEvent();

            this.superDomainRepository.AddSuper(this.parentSuperDomainDirectoryInfo);
            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            superObjectType = (ObjectType)domain.Domain.Find(superObjectTypeId);

            var objectType = domain.AddDeclaredObjectType(objectTypeId);
            objectType.SingularName = "ObjectType";
            objectType.PluralName = "ObjectTypes";
            objectType.SendChangedEvent();

            var inheritance = domain.AddDeclaredInheritance(inheritanceId);
            inheritance.Subtype = objectType;
            inheritance.Supertype = superObjectType;
            inheritance.SendChangedEvent();

            var pullUp = this.domainRepository.PullUp(domainSuperDomain, objectType, true);

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(0, pullUp.RelationTypesToPull.Length);

            Assert.AreEqual(1, pullUp.InheritancesToPull.Length);
            Assert.AreEqual(inheritance, pullUp.InheritancesToPull[0]);

            pullUp.Execute();

            Assert.AreEqual(1, pullUp.ObjectTypesToPull.Length);
            Assert.AreEqual(objectType, pullUp.ObjectTypesToPull[0]);

            Assert.AreEqual(0, pullUp.RelationTypesToPull.Length);

            Assert.AreEqual(1, pullUp.InheritancesToPull.Length);
            Assert.AreEqual(inheritance, pullUp.InheritancesToPull[0]);

            this.domainRepository.Reload();
            this.superDomainRepository.Reload();
            this.parentSuperDomainRepository.Reload();

            var superDomainDomain = this.superDomainRepository.Domain;
            var superDomainDomainParentSuperDomain = (Domain)superDomainDomain.Domain.Find(this.parentSuperDomainRepository.Id);

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);
            objectType = superDomainDomain.DeclaredObjectTypes[0];
            Assert.IsNotNull(objectType);
            Assert.AreEqual(superDomainDomain, objectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);

            Assert.AreEqual(2, superDomainDomainParentSuperDomain.DeclaredObjectTypes.Length);
            superObjectType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[0];
            toObjectType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[1];

            if (!superObjectType.SingularName.Equals("SuperType"))
            {
                superObjectType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[1];
                toObjectType = superDomainDomainParentSuperDomain.DeclaredObjectTypes[0];
            }

            Assert.IsNotNull(superObjectType);
            Assert.AreEqual(superDomainDomainParentSuperDomain, superObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("SuperType", superObjectType.SingularName);
            Assert.AreEqual("SuperTypes", superObjectType.PluralName);

            Assert.IsNotNull(toObjectType);
            Assert.AreEqual(superDomainDomainParentSuperDomain, superObjectType.DomainWhereDeclaredObjectType);
            Assert.AreEqual("To", toObjectType.SingularName);
            Assert.AreEqual("Tos", toObjectType.PluralName);

            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);
            inheritance = superDomainDomain.DeclaredInheritances[0];
            Assert.IsNotNull(inheritance);
            Assert.AreEqual(superDomainDomain, inheritance.DomainWhereDeclaredInheritance);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(superObjectType, inheritance.Supertype);

            Assert.AreEqual(1, superDomainDomainParentSuperDomain.DeclaredRelationTypes.Length);
            relationType = superDomainDomainParentSuperDomain.DeclaredRelationTypes[0];
            Assert.IsNotNull(relationType);
            Assert.AreEqual(superDomainDomainParentSuperDomain, relationType.DomainWhereDeclaredRelationType);
            Assert.AreEqual(superObjectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(toObjectType, relationType.RoleType.ObjectType);
        }
    }
}