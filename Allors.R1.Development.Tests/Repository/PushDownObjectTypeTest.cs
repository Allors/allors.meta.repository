// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PushDownObjectTypeTest.cs" company="Allors bvba">
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
    public class PushDownObjectTypeTest
    {
        private DirectoryInfo domainDirectoryInfo;
        private Repository domainRepository;

        private DirectoryInfo superDomainDirectoryInfo;
        private Repository superDomainRepository;

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
        public void WithDependencies()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var superTypeId = new Guid("EE47525F-2529-4089-A373-9CF76E67DE2E");
            var subTypeId = new Guid("2DA4EB64-96D8-4E2E-8B31-FA511F4CB920");
            var roleObjectTypeId = new Guid("6FF52EE9-F236-4CDB-BED4-CD8D5AD3C503");
            var associationObjectTypeId = new Guid("C4350ED0-93D0-4D2D-B831-802665122BE3");
            var inheritanceWhereSubTypeId = new Guid("53DEDB02-4693-44A5-9D27-712754E3AE90");
            var inheritanceWhereSuperTypeId = new Guid("18FD28F9-A678-4A16-811A-97EE2EE7D3AE");
            var relationTypeWhereAssociationId = new Guid("0B5D66B3-B480-497C-A5A6-50C0250FBAFB");
            var relationTypeWhereAssociationAssociationTypeId = new Guid("1DCD8E15-D57F-4F66-9C47-6F4273135D1A");
            var relationTypeWhereAssociationRoleTypeId = new Guid("DAF2DAE4-9382-48D0-9252-CB7AF07FCF0D");
            var relationTypeWhereRoleId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeWhereRoleAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeWhereRoleRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSuperType = superDomainDomain.BuildObjectType(superTypeId, "SuperType", "SuperTypes");
            var superDomainSubType = superDomainDomain.BuildObjectType(subTypeId, "SubType", "SubTypes");
            var superDomainRoleObjectType = superDomainDomain.BuildObjectType(roleObjectTypeId, "RoleObjectType", "RoleObjectTypes");
            var superDomainAssociationObjectType = superDomainDomain.BuildObjectType(associationObjectTypeId, "AssociationObjectType", "AssociationObjectTypes");
            var superDomainInheritanceWhereSubType = superDomainDomain.BuildInheritance(inheritanceWhereSubTypeId, superDomainObjectType, superDomainSuperType);
            var superDomainInheritanceWhereSuperType = superDomainDomain.BuildInheritance(inheritanceWhereSuperTypeId, superDomainSubType, superDomainObjectType);
            var superDomainRelationTypeWhereAssociation = superDomainDomain.BuildRelationType(relationTypeWhereAssociationId, relationTypeWhereAssociationAssociationTypeId, superDomainObjectType, relationTypeWhereAssociationRoleTypeId, superDomainRoleObjectType);
            var superDomainRelationTypeWhereRole = superDomainDomain.BuildRelationType(relationTypeWhereRoleId, relationTypeWhereRoleAssociationTypeId, superDomainAssociationObjectType, relationTypeWhereRoleRoleTypeId, superDomainObjectType);
            
            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritanceWhereSupertype = (Inheritance)domain.Domain.Find(inheritanceWhereSuperTypeId);
            var inheritanceWhereSubtype = (Inheritance)domain.Domain.Find(inheritanceWhereSubTypeId);
            var relationTypeWhereAssociation = (RelationType)domain.Domain.Find(relationTypeWhereAssociationId);
            var relationTypeWhereRole = (RelationType)domain.Domain.Find(relationTypeWhereRoleId);

            var pushDown = this.domainRepository.PushDown(objectType);

            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.Contains(objectType, pushDown.ObjectTypesToPush);

            Assert.AreEqual(2, pushDown.InheritancesToPush.Length);
            Assert.Contains(inheritanceWhereSupertype, pushDown.InheritancesToPush);
            Assert.Contains(inheritanceWhereSubtype, pushDown.InheritancesToPush);

            Assert.AreEqual(2, pushDown.RelationTypesToPush.Length);
            Assert.Contains(relationTypeWhereAssociation, pushDown.RelationTypesToPush);
            Assert.Contains(relationTypeWhereRole, pushDown.RelationTypesToPush);

            pushDown.Execute();

            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;
            
            Assert.AreEqual(4, superDomainDomain.DeclaredObjectTypes.Length);

            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainAssociationObjectType = (ObjectType)superDomainDomain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", superDomainAssociationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", superDomainAssociationObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainAssociationObjectType.DomainWhereDeclaredObjectType);

            superDomainRoleObjectType = (ObjectType)superDomainDomain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", superDomainRoleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", superDomainRoleObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainRoleObjectType.DomainWhereDeclaredObjectType);

            superDomainSubType = (ObjectType)superDomainDomain.Domain.Find(subTypeId);
            Assert.AreEqual("SubType", superDomainSubType.SingularName);
            Assert.AreEqual("SubTypes", superDomainSubType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubType.DomainWhereDeclaredObjectType);

            superDomainSuperType = (ObjectType)superDomainDomain.Domain.Find(superTypeId);
            Assert.AreEqual("SuperType", superDomainSuperType.SingularName);
            Assert.AreEqual("SuperTypes", superDomainSuperType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSuperType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceWhereSubTypeId));
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceWhereSuperTypeId));

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(relationTypeWhereAssociationId));
            Assert.IsNull(superDomainDomain.Domain.Find(relationTypeWhereRoleId));

            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(4, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);
            
            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subType = (ObjectType)domain.Domain.Find(subTypeId);
            Assert.AreEqual("SubType", subType.SingularName);
            Assert.AreEqual("SubTypes", subType.PluralName);
            Assert.AreEqual(domainSuperDomain, subType.DomainWhereDeclaredObjectType);

            var superType = (ObjectType)domain.Domain.Find(superTypeId);
            Assert.AreEqual("SuperType", superType.SingularName);
            Assert.AreEqual("SuperTypes", superType.PluralName);
            Assert.AreEqual(domainSuperDomain, superType.DomainWhereDeclaredObjectType);

            var associationObjectType = (ObjectType)domain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", associationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", associationObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, associationObjectType.DomainWhereDeclaredObjectType);

            var roleObjectType = (ObjectType)domain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", roleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", roleObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, roleObjectType.DomainWhereDeclaredObjectType);
            
            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(2, domain.DeclaredInheritances.Length);
            
            inheritanceWhereSubtype = (Inheritance)domain.Domain.Find(inheritanceWhereSubTypeId);
            Assert.AreEqual(objectType, inheritanceWhereSubtype.Subtype);
            Assert.AreEqual(superType, inheritanceWhereSubtype.Supertype);
            Assert.AreEqual(domain, inheritanceWhereSubtype.DomainWhereDeclaredInheritance);

            inheritanceWhereSupertype = (Inheritance)domain.Domain.Find(inheritanceWhereSuperTypeId);
            Assert.AreEqual(objectType, inheritanceWhereSupertype.Supertype);
            Assert.AreEqual(subType, inheritanceWhereSupertype.Subtype);
            Assert.AreEqual(domain, inheritanceWhereSupertype.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(2, domain.DeclaredRelationTypes.Length);

            relationTypeWhereAssociation = (RelationType)domain.Domain.Find(relationTypeWhereAssociationId);
            Assert.AreEqual(relationTypeWhereAssociationAssociationTypeId, relationTypeWhereAssociation.AssociationType.Id);
            Assert.AreEqual(objectType, relationTypeWhereAssociation.AssociationType.ObjectType);
            Assert.AreEqual(roleObjectType, relationTypeWhereAssociation.RoleType.ObjectType);
            Assert.AreEqual(domain, relationTypeWhereAssociation.DomainWhereDeclaredRelationType);

            relationTypeWhereRole = (RelationType)domain.Domain.Find(relationTypeWhereRoleId);
            Assert.AreEqual(relationTypeWhereRoleRoleTypeId, relationTypeWhereRole.RoleType.Id);
            Assert.AreEqual(objectType, relationTypeWhereRole.RoleType.ObjectType);
            Assert.AreEqual(relationTypeWhereRoleRoleTypeId, relationTypeWhereRole.RoleType.Id);
            Assert.AreEqual(associationObjectType, relationTypeWhereRole.AssociationType.ObjectType);
            Assert.AreEqual(domain, relationTypeWhereRole.DomainWhereDeclaredRelationType);
        }
        
        [Test]
        public void NonDependentObjectType()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var otherObjectTypeId = new Guid("7DC9A2D6-6D8A-4F70-91BC-6666816446B4");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainOtherObjectType = superDomainDomain.BuildObjectType(otherObjectTypeId, "OtherObjectType", "OtherObjectTypes");

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(0, pushDown.InheritancesToPush.Length);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainOtherObjectType = (ObjectType)superDomainDomain.Domain.Find(otherObjectTypeId);
            Assert.AreEqual("OtherObjectType", superDomainOtherObjectType.SingularName);
            Assert.AreEqual("OtherObjectTypes", superDomainOtherObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainOtherObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var otherObjectType = (ObjectType)domain.Domain.Find(otherObjectTypeId);
            Assert.AreEqual("OtherObjectType", otherObjectType.SingularName);
            Assert.AreEqual("OtherObjectTypes", otherObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, otherObjectType.DomainWhereDeclaredObjectType);
        }

        [Test]
        public void NonDependentInheritance()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var subtypeId = new Guid("7DC9A2D6-6D8A-4F70-91BC-6666816446B4");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSubtype = superDomainDomain.BuildObjectType(subtypeId, "Subtype", "Subtypes");
            var superDomainSupertype = superDomainDomain.BuildObjectType(supertypeId, "Supertype", "Supertypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainSubtype, superDomainSupertype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(0, pushDown.InheritancesToPush.Length);
           
            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainSubtype = (ObjectType)superDomainDomain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", superDomainSubtype.SingularName);
            Assert.AreEqual("Subtypes", superDomainSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubtype.DomainWhereDeclaredObjectType);

            superDomainSupertype = (ObjectType)superDomainDomain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", superDomainSupertype.SingularName);
            Assert.AreEqual("Supertypes", superDomainSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSupertype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);

            superDomainInheritance = (Inheritance)superDomainDomain.Domain.Find(inheritanceId);
            Assert.AreEqual(superDomainSubtype, superDomainInheritance.Subtype);
            Assert.AreEqual(superDomainSupertype, superDomainInheritance.Supertype);
            Assert.AreEqual(superDomainDomain, superDomainInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);
            
            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subtype = (ObjectType)domain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", subtype.SingularName);
            Assert.AreEqual("Subtypes", subtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subtype.DomainWhereDeclaredObjectType);

            var supertype = (ObjectType)domain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", supertype.SingularName);
            Assert.AreEqual("Supertypes", supertype.PluralName);
            Assert.AreEqual(domainSuperDomain, supertype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(0, domain.DeclaredInheritances.Length);

            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(subtype, inheritance.Subtype);
            Assert.AreEqual(supertype, inheritance.Supertype);
            Assert.AreEqual(domainSuperDomain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void NonDependentSuperSuperInheritance()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var superSupertypeId = new Guid("7DC9A2D6-6D8A-4F70-91BC-6666816446B4");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var superInheritanceId = new Guid("60A43568-D0BB-49F0-BC63-C2CBE8166818");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSupertype = superDomainDomain.BuildObjectType(supertypeId, "Supertype", "Supertypes");
            var superDomainSuperSupertype = superDomainDomain.BuildObjectType(superSupertypeId, "SuperSupertype", "SuperSupertypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainObjectType, superDomainSupertype);
            var superDomainSuperInheritance = superDomainDomain.BuildInheritance(superInheritanceId, superDomainSupertype, superDomainSuperSupertype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainSupertype = (ObjectType)superDomainDomain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", superDomainSupertype.SingularName);
            Assert.AreEqual("Supertypes", superDomainSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSupertype.DomainWhereDeclaredObjectType);

            superDomainSuperSupertype = (ObjectType)superDomainDomain.Domain.Find(superSupertypeId);
            Assert.AreEqual("SuperSupertype", superDomainSuperSupertype.SingularName);
            Assert.AreEqual("SuperSupertypes", superDomainSuperSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSuperSupertype.DomainWhereDeclaredObjectType);
            
            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);

            superDomainSuperInheritance = (Inheritance)superDomainDomain.Domain.Find(superInheritanceId);
            Assert.AreEqual(superDomainSupertype, superDomainSuperInheritance.Subtype);
            Assert.AreEqual(superDomainSuperSupertype, superDomainSuperInheritance.Supertype);
            Assert.AreEqual(superDomainDomain, superDomainSuperInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var supertype = (ObjectType)domain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", supertype.SingularName);
            Assert.AreEqual("Supertypes", supertype.PluralName);
            Assert.AreEqual(domainSuperDomain, supertype.DomainWhereDeclaredObjectType);

            var superSupertype = (ObjectType)domain.Domain.Find(superSupertypeId);
            Assert.AreEqual("SuperSupertype", superSupertype.SingularName);
            Assert.AreEqual("SuperSupertypes", superSupertype.PluralName);
            Assert.AreEqual(domainSuperDomain, superSupertype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);
            
            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(supertype, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            var superInheritance = (Inheritance)domain.Domain.Find(superInheritanceId);
            Assert.AreEqual(supertype, superInheritance.Subtype);
            Assert.AreEqual(superSupertype, superInheritance.Supertype);
            Assert.AreEqual(domainSuperDomain, superInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void NonDependentSubSubInheritance()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var subtypeId = new Guid("7DC9A2D6-6D8A-4F70-91BC-6666816446B4");
            var subSubtypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var subInheritanceId = new Guid("60A43568-D0BB-49F0-BC63-C2CBE8166818");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSubtype = superDomainDomain.BuildObjectType(subtypeId, "Subtype", "Subtypes");
            var superDomainSubSubtype = superDomainDomain.BuildObjectType(subSubtypeId, "SubSubtype", "SubSubtypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainSubtype, superDomainObjectType);
            var superDomainSubInheritance = superDomainDomain.BuildInheritance(subInheritanceId, superDomainSubSubtype, superDomainSubtype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainSubtype = (ObjectType)superDomainDomain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", superDomainSubtype.SingularName);
            Assert.AreEqual("Subtypes", superDomainSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubtype.DomainWhereDeclaredObjectType);

            superDomainSubSubtype = (ObjectType)superDomainDomain.Domain.Find(subSubtypeId);
            Assert.AreEqual("SubSubtype", superDomainSubSubtype.SingularName);
            Assert.AreEqual("SubSubtypes", superDomainSubSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubSubtype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, superDomainDomain.DeclaredInheritances.Length);

            superDomainSubInheritance = (Inheritance)superDomainDomain.Domain.Find(subInheritanceId);
            Assert.AreEqual(superDomainSubSubtype, superDomainSubInheritance.Subtype);
            Assert.AreEqual(superDomainSubtype, superDomainSubInheritance.Supertype);
            Assert.AreEqual(superDomainDomain, superDomainSubInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subtype = (ObjectType)domain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", subtype.SingularName);
            Assert.AreEqual("Subtypes", subtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subtype.DomainWhereDeclaredObjectType);

            var subSubtype = (ObjectType)domain.Domain.Find(subSubtypeId);
            Assert.AreEqual("SubSubtype", subSubtype.SingularName);
            Assert.AreEqual("SubSubtypes", subSubtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subSubtype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(1, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(subtype, inheritance.Subtype);
            Assert.AreEqual(objectType, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            var subInheritance = (Inheritance)domain.Domain.Find(subInheritanceId);
            Assert.AreEqual(subSubtype, subInheritance.Subtype);
            Assert.AreEqual(subtype, subInheritance.Supertype);
            Assert.AreEqual(domainSuperDomain, subInheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void NonDependentSuperAssociation()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var roleObjectTypeId = new Guid("A192E0E4-907F-4AEB-A75F-291C1C356D73");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var relationTypeId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSupertype = superDomainDomain.BuildObjectType(supertypeId, "Supertype", "Supertypes");
            var superDomainRoleObjectType = superDomainDomain.BuildObjectType(roleObjectTypeId, "RoleObjectType", "RoleObjectTypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainObjectType, superDomainSupertype);
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainSupertype, relationTypeRoleTypeId, superDomainRoleObjectType);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainSupertype = (ObjectType)superDomainDomain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", superDomainSupertype.SingularName);
            Assert.AreEqual("Supertypes", superDomainSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSupertype.DomainWhereDeclaredObjectType);

            superDomainRoleObjectType = (ObjectType)superDomainDomain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", superDomainRoleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", superDomainRoleObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainRoleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);

            superDomainRelationType = (RelationType)superDomainDomain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, superDomainRelationType.AssociationType.Id);
            Assert.AreEqual(superDomainSupertype, superDomainRelationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, superDomainRelationType.RoleType.Id);
            Assert.AreEqual(superDomainRoleObjectType, superDomainRelationType.RoleType.ObjectType);
            Assert.AreEqual(superDomainDomain, superDomainRelationType.DomainWhereDeclaredRelationType);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var supertype = (ObjectType)domain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", supertype.SingularName);
            Assert.AreEqual("Supertypes", supertype.PluralName);
            Assert.AreEqual(domainSuperDomain, supertype.DomainWhereDeclaredObjectType);

            var roleObjectType = (ObjectType)domain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", roleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", roleObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, roleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(supertype, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);

            var relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(supertype, relationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(roleObjectType, relationType.RoleType.ObjectType);
            Assert.AreEqual(domainSuperDomain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void NonDependentSuperRole()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var associationObjectTypeId = new Guid("A192E0E4-907F-4AEB-A75F-291C1C356D73");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var relationTypeId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSupertype = superDomainDomain.BuildObjectType(supertypeId, "Supertype", "Supertypes");
            var superDomainAssociationObjectType = superDomainDomain.BuildObjectType(associationObjectTypeId, "AssociationObjectType", "AssociationObjectTypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainObjectType, superDomainSupertype);
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainAssociationObjectType, relationTypeRoleTypeId, superDomainSupertype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainSupertype = (ObjectType)superDomainDomain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", superDomainSupertype.SingularName);
            Assert.AreEqual("Supertypes", superDomainSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSupertype.DomainWhereDeclaredObjectType);

            superDomainAssociationObjectType = (ObjectType)superDomainDomain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", superDomainAssociationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", superDomainAssociationObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainAssociationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);

            superDomainRelationType = (RelationType)superDomainDomain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, superDomainRelationType.AssociationType.Id);
            Assert.AreEqual(superDomainAssociationObjectType, superDomainRelationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, superDomainRelationType.RoleType.Id);
            Assert.AreEqual(superDomainSupertype, superDomainRelationType.RoleType.ObjectType);
            Assert.AreEqual(superDomainDomain, superDomainRelationType.DomainWhereDeclaredRelationType);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var supertype = (ObjectType)domain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", supertype.SingularName);
            Assert.AreEqual("Supertypes", supertype.PluralName);
            Assert.AreEqual(domainSuperDomain, supertype.DomainWhereDeclaredObjectType);

            var associationObjectType = (ObjectType)domain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", associationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", associationObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, associationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(supertype, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);

            var relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(associationObjectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(supertype, relationType.RoleType.ObjectType);
            Assert.AreEqual(domainSuperDomain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void NonDependentSubAssociation()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var subtypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var roleObjectTypeId = new Guid("A192E0E4-907F-4AEB-A75F-291C1C356D73");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var relationTypeId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSubtype = superDomainDomain.BuildObjectType(subtypeId, "Subtype", "Subtypes");
            var superDomainRoleObjectType = superDomainDomain.BuildObjectType(roleObjectTypeId, "RoleObjectType", "RoleObjectTypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainSubtype, superDomainObjectType);
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainSubtype, relationTypeRoleTypeId, superDomainRoleObjectType);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainSubtype = (ObjectType)superDomainDomain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", superDomainSubtype.SingularName);
            Assert.AreEqual("Subtypes", superDomainSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubtype.DomainWhereDeclaredObjectType);

            superDomainRoleObjectType = (ObjectType)superDomainDomain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", superDomainRoleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", superDomainRoleObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainRoleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);

            superDomainRelationType = (RelationType)superDomainDomain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, superDomainRelationType.AssociationType.Id);
            Assert.AreEqual(superDomainSubtype, superDomainRelationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, superDomainRelationType.RoleType.Id);
            Assert.AreEqual(superDomainRoleObjectType, superDomainRelationType.RoleType.ObjectType);
            Assert.AreEqual(superDomainDomain, superDomainRelationType.DomainWhereDeclaredRelationType);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subtype = (ObjectType)domain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", subtype.SingularName);
            Assert.AreEqual("Subtypes", subtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subtype.DomainWhereDeclaredObjectType);

            var roleObjectType = (ObjectType)domain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", roleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", roleObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, roleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(subtype, inheritance.Subtype);
            Assert.AreEqual(objectType, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);

            var relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(subtype, relationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(roleObjectType, relationType.RoleType.ObjectType);
            Assert.AreEqual(domainSuperDomain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void NonDependentSubRole()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var subtypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var associationObjectTypeId = new Guid("A192E0E4-907F-4AEB-A75F-291C1C356D73");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");
            var relationTypeId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSubtype = superDomainDomain.BuildObjectType(subtypeId, "Subtype", "Subtypes");
            var superDomainAssociationObjectType = superDomainDomain.BuildObjectType(associationObjectTypeId, "AssociationObjectType", "AssociationObjectTypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainSubtype, superDomainObjectType);
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainAssociationObjectType, relationTypeRoleTypeId, superDomainSubtype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(2, superDomainDomain.DeclaredObjectTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainSubtype = (ObjectType)superDomainDomain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", superDomainSubtype.SingularName);
            Assert.AreEqual("Subtypes", superDomainSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubtype.DomainWhereDeclaredObjectType);

            superDomainAssociationObjectType = (ObjectType)superDomainDomain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", superDomainAssociationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", superDomainAssociationObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainAssociationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(1, superDomainDomain.DeclaredRelationTypes.Length);

            superDomainRelationType = (RelationType)superDomainDomain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, superDomainRelationType.AssociationType.Id);
            Assert.AreEqual(superDomainAssociationObjectType, superDomainRelationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, superDomainRelationType.RoleType.Id);
            Assert.AreEqual(superDomainSubtype, superDomainRelationType.RoleType.ObjectType);
            Assert.AreEqual(superDomainDomain, superDomainRelationType.DomainWhereDeclaredRelationType);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(2, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subtype = (ObjectType)domain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", subtype.SingularName);
            Assert.AreEqual("Subtypes", subtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subtype.DomainWhereDeclaredObjectType);

            var associationObjectType = (ObjectType)domain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", associationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", associationObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, associationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(subtype, inheritance.Subtype);
            Assert.AreEqual(objectType, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(1, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);

            var relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(associationObjectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(subtype, relationType.RoleType.ObjectType);
            Assert.AreEqual(domainSuperDomain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void DependentSuperInheritance()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSupertype = superDomainDomain.BuildObjectType(supertypeId, "Supertype", "Supertypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainObjectType, superDomainSupertype);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainSupertype = (ObjectType)superDomainDomain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", superDomainSupertype.SingularName);
            Assert.AreEqual("Supertypes", superDomainSupertype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSupertype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(1, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var supertype = (ObjectType)domain.Domain.Find(supertypeId);
            Assert.AreEqual("Supertype", supertype.SingularName);
            Assert.AreEqual("Supertypes", supertype.PluralName);
            Assert.AreEqual(domainSuperDomain, supertype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(objectType, inheritance.Subtype);
            Assert.AreEqual(supertype, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void DependentSubInheritance()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var subtypeId = new Guid("7DC9A2D6-6D8A-4F70-91BC-6666816446B4");
            var supertypeId = new Guid("296E8F8D-1A45-4635-9E9D-0CDE26EAF936");
            var inheritanceId = new Guid("495827B2-F9DB-4141-95CC-471633D090E8");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainSubtype = superDomainDomain.BuildObjectType(subtypeId, "Subtype", "Subtypes");
            var superDomainInheritance = superDomainDomain.BuildInheritance(inheritanceId, superDomainSubtype, superDomainObjectType);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var inheritance = (Inheritance)domain.Domain.Find(inheritanceId);

            var pushDown = this.domainRepository.PushDown(objectType);

            // check pushdown
            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.AreEqual(objectType, pushDown.ObjectTypesToPush[0]);

            Assert.AreEqual(1, pushDown.InheritancesToPush.Length);
            Assert.AreEqual(inheritance, pushDown.InheritancesToPush[0]);

            Assert.AreEqual(0, pushDown.RelationTypesToPush.Length);

            pushDown.Execute();

            // check super domain repository
            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);

            superDomainSubtype = (ObjectType)superDomainDomain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", superDomainSubtype.SingularName);
            Assert.AreEqual("Subtypes", superDomainSubtype.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainSubtype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(inheritanceId));

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);

            // check domain repository
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(1, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var subtype = (ObjectType)domain.Domain.Find(subtypeId);
            Assert.AreEqual("Subtype", subtype.SingularName);
            Assert.AreEqual("Subtypes", subtype.PluralName);
            Assert.AreEqual(domainSuperDomain, subtype.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(1, domain.DeclaredInheritances.Length);

            inheritance = (Inheritance)domain.Domain.Find(inheritanceId);
            Assert.AreEqual(subtype, inheritance.Subtype);
            Assert.AreEqual(objectType, inheritance.Supertype);
            Assert.AreEqual(domain, inheritance.DomainWhereDeclaredInheritance);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(0, domain.DeclaredRelationTypes.Length);
        }

        [Test]
        public void DependentRole()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var roleObjectTypeId = new Guid("6FF52EE9-F236-4CDB-BED4-CD8D5AD3C503");
            var relationTypeId = new Guid("0B5D66B3-B480-497C-A5A6-50C0250FBAFB");
            var relationTypeAssociationTypeId = new Guid("1DCD8E15-D57F-4F66-9C47-6F4273135D1A");
            var relationTypeRoleTypeId = new Guid("DAF2DAE4-9382-48D0-9252-CB7AF07FCF0D");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainRoleObjectType = superDomainDomain.BuildObjectType(roleObjectTypeId, "RoleObjectType", "RoleObjectTypes");
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainObjectType, relationTypeRoleTypeId, superDomainRoleObjectType);
            
            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var relationType = (RelationType)domain.Domain.Find(relationTypeId);

            var pushDown = this.domainRepository.PushDown(objectType);

            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.Contains(objectType, pushDown.ObjectTypesToPush);

            Assert.AreEqual(0, pushDown.InheritancesToPush.Length);

            Assert.AreEqual(1, pushDown.RelationTypesToPush.Length);
            Assert.Contains(relationType, pushDown.RelationTypesToPush);

            pushDown.Execute();

            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);

            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainRoleObjectType = (ObjectType)superDomainDomain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", superDomainRoleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", superDomainRoleObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainRoleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);
            
            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(relationTypeId));
            
            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(1, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var roleObjectType = (ObjectType)domain.Domain.Find(roleObjectTypeId);
            Assert.AreEqual("RoleObjectType", roleObjectType.SingularName);
            Assert.AreEqual("RoleObjectTypes", roleObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, roleObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(0, domain.DeclaredInheritances.Length);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(1, domain.DeclaredRelationTypes.Length);

            relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeAssociationTypeId, relationType.AssociationType.Id);
            Assert.AreEqual(objectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(roleObjectType, relationType.RoleType.ObjectType);
            Assert.AreEqual(domain, relationType.DomainWhereDeclaredRelationType);
        }

        [Test]
        public void DependentAssociation()
        {
            var objectTypeId = new Guid("C2C633D4-0735-4B24-8D9B-82BD970A43D6");
            var associationObjectTypeId = new Guid("C4350ED0-93D0-4D2D-B831-802665122BE3");
            var relationTypeId = new Guid("0DBBD5C6-B84E-4982-A477-D7C41F4019D3");
            var relationTypeAssociationTypeId = new Guid("A27DF3DC-38DD-4CBB-8242-EEE36E1F2697");
            var relationTypeRoleTypeId = new Guid("C47E0BBF-C6FE-4EBC-BCAA-47C1B2666478");

            var superDomainDomain = this.superDomainRepository.Domain;

            var superDomainObjectType = superDomainDomain.BuildObjectType(objectTypeId, "ObjectType", "ObjectTypes");
            var superDomainAssociationObjectType = superDomainDomain.BuildObjectType(associationObjectTypeId, "AssociationObjectType", "AssociationObjectTypes");
            var superDomainRelationType = superDomainDomain.BuildRelationType(relationTypeId, relationTypeAssociationTypeId, superDomainAssociationObjectType, relationTypeRoleTypeId, superDomainObjectType);

            this.domainRepository.AddSuper(this.superDomainDirectoryInfo);

            var domain = this.domainRepository.Domain;

            var objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            var relationType = (RelationType)domain.Domain.Find(relationTypeId);

            var pushDown = this.domainRepository.PushDown(objectType);

            Assert.AreEqual(1, pushDown.ObjectTypesToPush.Length);
            Assert.Contains(objectType, pushDown.ObjectTypesToPush);

            Assert.AreEqual(0, pushDown.InheritancesToPush.Length);

            Assert.AreEqual(1, pushDown.RelationTypesToPush.Length);
            Assert.Contains(relationType, pushDown.RelationTypesToPush);

            pushDown.Execute();

            this.superDomainRepository.Reload();

            superDomainDomain = this.superDomainRepository.Domain;

            Assert.AreEqual(1, superDomainDomain.DeclaredObjectTypes.Length);

            Assert.IsNull(superDomainDomain.Domain.Find(objectTypeId));

            superDomainAssociationObjectType = (ObjectType)superDomainDomain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", superDomainAssociationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", superDomainAssociationObjectType.PluralName);
            Assert.AreEqual(superDomainDomain, superDomainAssociationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, superDomainDomain.DeclaredInheritances.Length);

            Assert.AreEqual(0, superDomainDomain.DeclaredRelationTypes.Length);
            Assert.IsNull(superDomainDomain.Domain.Find(relationTypeId));

            this.domainRepository.Reload();
            domain = this.domainRepository.Domain;
            var domainSuperDomain = (Domain)domain.Domain.Find(this.superDomainRepository.Id);

            Assert.AreEqual(1, domainSuperDomain.DeclaredObjectTypes.Length);
            Assert.AreEqual(1, domain.DeclaredObjectTypes.Length);

            objectType = (ObjectType)domain.Domain.Find(objectTypeId);
            Assert.AreEqual("ObjectType", objectType.SingularName);
            Assert.AreEqual("ObjectTypes", objectType.PluralName);
            Assert.AreEqual(domain, objectType.DomainWhereDeclaredObjectType);

            var associationObjectType = (ObjectType)domain.Domain.Find(associationObjectTypeId);
            Assert.AreEqual("AssociationObjectType", associationObjectType.SingularName);
            Assert.AreEqual("AssociationObjectTypes", associationObjectType.PluralName);
            Assert.AreEqual(domainSuperDomain, associationObjectType.DomainWhereDeclaredObjectType);

            Assert.AreEqual(0, domainSuperDomain.DeclaredInheritances.Length);
            Assert.AreEqual(0, domain.DeclaredInheritances.Length);

            Assert.AreEqual(0, domainSuperDomain.DeclaredRelationTypes.Length);
            Assert.AreEqual(1, domain.DeclaredRelationTypes.Length);

            relationType = (RelationType)domain.Domain.Find(relationTypeId);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(objectType, relationType.RoleType.ObjectType);
            Assert.AreEqual(relationTypeRoleTypeId, relationType.RoleType.Id);
            Assert.AreEqual(associationObjectType, relationType.AssociationType.ObjectType);
            Assert.AreEqual(domain, relationType.DomainWhereDeclaredRelationType);
        }
    }
}