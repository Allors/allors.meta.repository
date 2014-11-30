//------------------------------------------------------------------------------------------------- 
// <copyright file="DomainTest.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the DomainTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    using Allors.R1.Meta.AllorsGenerated;

    using NUnit.Framework;

    [TestFixture]
    public class DomainTest : AbstractTest
    {
        private Inheritance sharedSuperDomainInterfaceInheritance;

        [Test]
        public void Composites()
        {
            this.Populate();

            Assert.AreEqual(15, this.Domain.CompositeObjectTypes.Length);
            Assert.AreEqual(15, this.Population.CompositeTypes.Length);

            var objectType = this.Population.SuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.IsUnit = false;

            Assert.AreEqual(16, this.Domain.CompositeObjectTypes.Length);
            Assert.AreEqual(16, this.Population.CompositeTypes.Length);

            objectType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.IsUnit = true;

            Assert.AreEqual(16, this.Domain.CompositeObjectTypes.Length);
            Assert.AreEqual(16, this.Population.CompositeTypes.Length);

            var domainDefinedTypes = new ArrayList(this.Domain.DeclaredObjectTypes);
            domainDefinedTypes.Remove(this.Population.C1);
            try
            {
                this.Domain.DeclaredObjectTypes = (ObjectType[])domainDefinedTypes.ToArray(typeof(ObjectType));
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(16, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(16, this.Population.CompositeTypes.Length);
            }

            try
            {
                this.Domain.RemoveDeclaredObjectType(this.Population.C2);
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(16, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(16, this.Population.CompositeTypes.Length);
            }

            try
            {
                this.Domain.RemoveDeclaredObjectTypes();
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(16, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(16, this.Population.CompositeTypes.Length);
            }
        }

        [Test]
        public void Create()
        {
            var newDomain = Domain.Create();
            newDomain.Name = "MyDomain";

            var validationReport = newDomain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(2, newDomain.Domains.Length);

            Assert.Contains(newDomain, newDomain.Domains);
            Assert.Contains(newDomain.UnitDomain, newDomain.Domains);

            Assert.AreEqual(9, this.Population.Types.Length);
            Assert.AreEqual(0, this.Population.Relations.Length);
            Assert.AreEqual(0, this.Population.Roles.Length);
        }

        [Test]
        public void Delete()
        {
            var exceptionThrown = false;
            try
            {
                this.Domain.Delete();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void Lookups()
        {
            this.Populate();

            var typeId = new Guid("89ff164f-ff6c-4b0d-916c-4e4507f97250");
            var type = this.Population.SuperDomain.AddDeclaredObjectType(typeId);
            Assert.AreEqual(type, this.Domain.Domain.Find(typeId));

            var relationId = new Guid("2B03B8A9-6BE0-4809-A536-092DBE09032A");
            var relationType = this.Population.SuperDomain.AddDeclaredRelationType(relationId, Guid.NewGuid(), Guid.NewGuid());

            Assert.AreEqual(relationType, this.Domain.Domain.Find(relationId));

            Assert.AreEqual(this.Population.SuperDomain, this.Domain.Domain.Find(this.Population.SuperDomain.Id));

            var inheritanceId = this.Population.C1.FindInheritanceWhereDirectSubtype(this.Population.A1).Id;
            var inheritance = (Inheritance)this.Domain.Domain.Find(inheritanceId);
            Assert.IsNotNull(inheritance);
            Assert.AreEqual(this.Population.C1, inheritance.Subtype);
            Assert.AreEqual(this.Population.A1, inheritance.Supertype);
        }

        [Test]
        public void SaveAndLoad()
        {
            this.Populate();

            var xml = this.Domain.Xml;

            var reader = new XmlTextReader(new StringReader(xml));
            var copy = Domain.Load(reader);

            Assert.AreEqual(xml, copy.Xml);
        }

        [Test]
        public void SetOrRemoveTypes()
        {
            this.Populate();

            var domainDefinedTypes = new ArrayList(this.Domain.DeclaredObjectTypes);
            domainDefinedTypes.Remove(this.Population.C1);
            try
            {
                this.Domain.DeclaredObjectTypes = (ObjectType[])domainDefinedTypes.ToArray(typeof(ObjectType));
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(15, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(15, this.Population.CompositeTypes.Length);
            }

            try
            {
                this.Domain.RemoveDeclaredObjectType(this.Population.C2);
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(15, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(15, this.Population.CompositeTypes.Length);
            }

            try
            {
                this.Domain.RemoveDeclaredObjectTypes();
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(15, this.Domain.CompositeObjectTypes.Length);
                Assert.AreEqual(15, this.Population.CompositeTypes.Length);
            }
        }

        [Test]
        public void Units()
        {
            this.Populate();

            Assert.AreEqual(9, this.Domain.UnitObjectTypes.Length);
            Assert.AreEqual(9, this.Population.UnitTypes.Length);

            var unit = this.Population.SuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            unit.IsUnit = true;

            Assert.AreEqual(10, this.Domain.UnitObjectTypes.Length);
            Assert.AreEqual(10, this.Population.UnitTypes.Length);

            var composite = this.Population.SuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            composite.IsUnit = false;

            Assert.AreEqual(10, this.Domain.UnitObjectTypes.Length);
            Assert.AreEqual(10, this.Population.UnitTypes.Length);
        }

        [Test]
        public void Validate()
        {
            var domain = Domain.Create();
            domain.Name = "MySuperDomain";

            var validationReport = domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            domain.Name = string.Empty;

            validationReport = domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.DomainName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Required, validationReport.Errors[0].Kind);

            domain.Name = "_";

            validationReport = domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.DomainName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "a_";

            validationReport = domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.DomainName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "1";

            validationReport = domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(domain, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.DomainName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            domain.Name = "a1";

            validationReport = domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            domain.Name = "a";

            validationReport = domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);
        }

        [Test]
        public void ValidateDuplicateRelationAndType()
        {
            this.Populate();

            var relationType = this.Population.SuperDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.AssociationType.AssignedSingularName = "aa";
            relationType.AssociationType.AssignedPluralName = "aas";
            relationType.RoleType.ObjectType = this.Population.C2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsTrue(this.Domain.IsValid);

            var type = this.Population.SuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            type.SingularName = "aabb";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void ValidateDuplicateReverseRelationAndType()
        {
            this.Populate();

            var relationType = this.Population.SuperDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.AssociationType.AssignedSingularName = "bb";
            relationType.AssociationType.AssignedPluralName = "bbs";
            relationType.RoleType.ObjectType = this.Population.C2;
            relationType.RoleType.AssignedSingularName = "aa";
            relationType.RoleType.AssignedPluralName = "aas";

            Assert.IsTrue(this.Domain.IsValid);

            var type = this.Population.SuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            type.SingularName = "aabb";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void Xml()
        {
            this.Populate();

            var xml = this.Domain.Xml;

            Assert.IsFalse(xml.StartsWith("<?"));

            var document = new XmlDocument();
            document.LoadXml(xml);
        }

        [Test]
        public void MetaObjectById()
        {
            var superDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            var c1 = Population.CreateClass(superDomain, "C1");
            var c2 = Population.CreateClass(superDomain, "C2");

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.IsIndexed = true;

            var association = relationType.AssociationType;
            association.ObjectType = c1;

            var role = relationType.RoleType;
            role.ObjectType = c2;

            var xml = this.Domain.Xml;
            var reader = new XmlTextReader(new StringReader(xml));
            var copy = Domain.Load(reader);

            var copyC1 = (ObjectType)copy.Domain.Find(c1.Id);
            var copyC2 = (ObjectType)copy.Domain.Find(c2.Id);

            var copyRelationType = (RelationType)copy.Domain.Find(relationType.Id);

            Assert.AreEqual(c1.Name, copyC1.Name);
            Assert.AreEqual(c2.Name, copyC2.Name);
            Assert.AreEqual(relationType.Name, copyRelationType.Name);

            Assert.AreEqual(copyC1, copy.Domain.Find(c1.Id));
            Assert.AreEqual(copyC2, copy.Domain.Find(c2.Id));
            Assert.AreEqual(copyRelationType, copy.Domain.Find(relationType.Id));
        }

        [Test]
        public void DerivedInheritances()
        {
            var domain = this.Domain;

            var superDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            Assert.AreEqual(0, domain.Inheritances.Length);
            Assert.AreEqual(0, superDomain.Inheritances.Length);

            var i1 = Population.CreateInterface(superDomain, "I1");
            var i2 = Population.CreateInterface(superDomain, "I2");
            var i3 = Population.CreateInterface(superDomain, "I3");
            var i4 = Population.CreateInterface(superDomain, "I4");
            var c1 = Population.CreateClass(superDomain, "C1");

            var domainC1I1 = domain.AddDeclaredInheritance(Guid.NewGuid());
            domainC1I1.Subtype = c1;
            domainC1I1.Supertype = i1;

            Assert.AreEqual(1, domain.Inheritances.Length);
            Assert.AreEqual(0, superDomain.Inheritances.Length);

            var superDomainC1I2 = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainC1I2.Subtype = c1;
            superDomainC1I2.Supertype = i2;

            Assert.AreEqual(2, domain.Inheritances.Length);
            Assert.AreEqual(1, superDomain.Inheritances.Length);

            var domainC1I3 = domain.AddDeclaredInheritance(Guid.NewGuid());
            domainC1I3.Subtype = c1;
            domainC1I3.Supertype = i3;

            Assert.AreEqual(3, domain.Inheritances.Length);
            Assert.AreEqual(1, superDomain.Inheritances.Length);

            var superDomainC1I4 = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainC1I4.Subtype = c1;
            superDomainC1I4.Supertype = i4;

            Assert.AreEqual(4, domain.Inheritances.Length);
            Assert.AreEqual(2, superDomain.Inheritances.Length);

            var reader = new XmlTextReader(new StringReader(domain.Xml));
            var copy = Domain.Load(reader);
            var copySuperDomain = (Domain)copy.Domain.Find(superDomain.Id);

            Assert.AreEqual(4, copy.Inheritances.Length);
            Assert.AreEqual(2, copySuperDomain.Inheritances.Length);
        }

        [Test]
        public void DerivedObjectTypes()
        {
            var domain = this.Domain;

            var superDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            Assert.AreEqual(9, domain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(0, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(9, superDomain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(0, domain.CompositeObjectTypes.Length);

            Population.CreateClass(domain, "C1");

            Assert.AreEqual(10, domain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(1, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(9, superDomain.ObjectTypes.Length);
            Assert.AreEqual(9, superDomain.UnitObjectTypes.Length);
            Assert.AreEqual(0, superDomain.CompositeObjectTypes.Length);

            Population.CreateClass(superDomain, "C2");

            Assert.AreEqual(11, domain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(2, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(10, superDomain.ObjectTypes.Length);
            Assert.AreEqual(9, superDomain.UnitObjectTypes.Length);
            Assert.AreEqual(1, superDomain.CompositeObjectTypes.Length);

            Population.CreateClass(domain, "C3");

            Assert.AreEqual(12, domain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(3, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(10, superDomain.ObjectTypes.Length);
            Assert.AreEqual(9, superDomain.UnitObjectTypes.Length);
            Assert.AreEqual(1, superDomain.CompositeObjectTypes.Length);

            Population.CreateClass(superDomain, "C4");

            Assert.AreEqual(13, domain.ObjectTypes.Length);
            Assert.AreEqual(9, domain.UnitObjectTypes.Length);
            Assert.AreEqual(4, domain.CompositeObjectTypes.Length);
            Assert.AreEqual(11, superDomain.ObjectTypes.Length);
            Assert.AreEqual(9, superDomain.UnitObjectTypes.Length);
            Assert.AreEqual(2, superDomain.CompositeObjectTypes.Length);

            var reader = new XmlTextReader(new StringReader(domain.Xml));
            var copy = Domain.Load(reader);
            var copySuperDomain = (Domain)copy.Domain.Find(superDomain.Id);

            Assert.AreEqual(13, copy.ObjectTypes.Length);
            Assert.AreEqual(9, copy.UnitObjectTypes.Length);
            Assert.AreEqual(4, copy.CompositeObjectTypes.Length);
            Assert.AreEqual(11, copySuperDomain.ObjectTypes.Length);
            Assert.AreEqual(9, copySuperDomain.UnitObjectTypes.Length);
            Assert.AreEqual(2, copySuperDomain.CompositeObjectTypes.Length);
        }

        [Test]
        public void DerivedRelationTypes()
        {
            var domain = this.Domain;

            var superDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            var i1 = Population.CreateInterface(superDomain, "I1");
            var i2 = Population.CreateInterface(superDomain, "I2");
            var i3 = Population.CreateInterface(superDomain, "I3");
            var i4 = Population.CreateInterface(superDomain, "I4");
            var c1 = Population.CreateClass(superDomain, "C1");

            Assert.AreEqual(0, domain.RelationTypes.Length);
            Assert.AreEqual(0, superDomain.RelationTypes.Length);

            var c1i1 = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1i1.AssociationType.ObjectType = c1;
            c1i1.RoleType.ObjectType = i1;

            Assert.AreEqual(1, domain.RelationTypes.Length);
            Assert.AreEqual(0, superDomain.RelationTypes.Length);

            var c1i2 = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1i1.AssociationType.ObjectType = c1;
            c1i1.RoleType.ObjectType = i1;

            Assert.AreEqual(2, domain.RelationTypes.Length);
            Assert.AreEqual(1, superDomain.RelationTypes.Length);

            var reader = new XmlTextReader(new StringReader(domain.Xml));
            var copy = Domain.Load(reader);
            var copySuperDomain = (Domain)copy.Domain.Find(superDomain.Id);

            Assert.AreEqual(2, copy.RelationTypes.Length);
            Assert.AreEqual(1, copySuperDomain.RelationTypes.Length);
        }

        [Test]
        public void DerivedMethodType()
        {
            var domain = this.Domain;

            var superDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            domain.AddDeclaredMethodType(Guid.NewGuid());

            Assert.AreEqual(1, domain.MethodTypes.Length);
            Assert.AreEqual(0, superDomain.MethodTypes.Length);

            superDomain.AddDeclaredMethodType(Guid.NewGuid());

            Assert.AreEqual(2, domain.MethodTypes.Length);
            Assert.AreEqual(1, superDomain.MethodTypes.Length);

            domain.AddDeclaredMethodType(Guid.NewGuid());

            Assert.AreEqual(3, domain.MethodTypes.Length);
            Assert.AreEqual(1, superDomain.MethodTypes.Length);

            superDomain.AddDeclaredMethodType(Guid.NewGuid());

            Assert.AreEqual(4, domain.MethodTypes.Length);
            Assert.AreEqual(2, superDomain.MethodTypes.Length);

            var reader = new XmlTextReader(new StringReader(domain.Xml));
            var copy = Domain.Load(reader);
            var copySuperDomain = (Domain)copy.Domain.Find(superDomain.Id);

            Assert.AreEqual(4, copy.MethodTypes.Length);
            Assert.AreEqual(2, copySuperDomain.MethodTypes.Length);
        }

        [Test]
        public void DerivedSuperDomains()
        {
            var domain = Domain.Create();
            domain.Name = "Domain";

            Assert.AreEqual(1, domain.SuperDomains.Length);
            Assert.AreEqual(domain.UnitDomain, domain.SuperDomains[0]);

            var parent = domain.AddDirectSuperDomain(Guid.NewGuid());
            parent.Name = "Parent";

            Assert.AreEqual(1, parent.SuperDomains.Length);
            Assert.AreEqual(parent.UnitDomain, parent.SuperDomains[0]);

            Assert.AreEqual(2, domain.SuperDomains.Length);
            Assert.Contains(domain.UnitDomain, domain.SuperDomains);
            Assert.Contains(parent, domain.SuperDomains);

            Assert.AreEqual(domain.UnitDomain, parent.UnitDomain);

            var grandParent = parent.AddDirectSuperDomain(Guid.NewGuid());
            grandParent.Name = "Grandparent";

            Assert.AreEqual(1, grandParent.SuperDomains.Length);
            Assert.AreEqual(grandParent.UnitDomain, grandParent.SuperDomains[0]);

            Assert.AreEqual(2, parent.SuperDomains.Length);
            Assert.Contains(parent.UnitDomain, parent.SuperDomains);
            Assert.Contains(grandParent, parent.SuperDomains);

            Assert.AreEqual(3, domain.SuperDomains.Length);
            Assert.Contains(domain.UnitDomain, domain.SuperDomains);
            Assert.Contains(parent, domain.SuperDomains);
            Assert.Contains(grandParent, domain.SuperDomains);

            Assert.AreEqual(domain.UnitDomain, parent.UnitDomain);
            Assert.AreEqual(domain.UnitDomain, grandParent.UnitDomain);

            var reader = new XmlTextReader(new StringReader(domain.Xml));
            var copy = Domain.Load(reader);
            var copyParent = (Domain)copy.Domain.Find(parent.Id);
            var copyGrandparent = (Domain)copy.Domain.Find(grandParent.Id);

            Assert.AreEqual(3, copy.SuperDomains.Length);
            Assert.Contains(copy.UnitDomain, copy.SuperDomains);
            Assert.Contains(copyParent, copy.SuperDomains);
            Assert.Contains(copyGrandparent, copy.SuperDomains);
        }

        [Test]
        public void CircularSuperDomains()
        {
            var domain = Domain.Create();
            domain.Name = "Domain";

            Assert.AreEqual(1, domain.SuperDomains.Length);
            Assert.AreEqual(domain.UnitDomain, domain.SuperDomains[0]);

            var parent = domain.AddDirectSuperDomain(Guid.NewGuid());
            parent.Name = "Parent";

            var exceptionThrown = false;
            try
            {
                parent.AddDirectSuperDomain(domain);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);

            exceptionThrown = false;
            try
            {
                domain.AddDirectSuperDomain(domain);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [Test]
        public void ExtendEmptySuperDomain()
        {
            Assert.AreEqual(9, this.Population.Types.Length);
            Assert.AreEqual(0, this.Population.Relations.Length);
            Assert.AreEqual(0, this.Population.Roles.Length);

            var newSuperDomain = Domain.Create();
            newSuperDomain.Name = "SuperDomain";

            this.Domain.Inherit(newSuperDomain);

            Assert.AreEqual(9, newSuperDomain.ObjectTypes.Length);
            Assert.AreEqual(0, newSuperDomain.Inheritances.Length);
            Assert.AreEqual(0, newSuperDomain.RelationTypes.Length);
        }

        [Test]
        public void ExtendSuperDomain()
        {
            // TODO: Documentation & Extensions
            Assert.AreEqual(9, this.Population.Types.Length);
            Assert.AreEqual(0, this.Population.Relations.Length);
            Assert.AreEqual(0, this.Population.Roles.Length);

            var importDomain = Domain.Create();
            importDomain.Name = "Import";

            var importAssociationType = importDomain.AddDeclaredObjectType(Guid.NewGuid());
            importAssociationType.SingularName = "SuperDomainAssociationType";
            importAssociationType.PluralName = "SuperDomainAssociationTypes";

            var importRoleType = importDomain.AddDeclaredObjectType(Guid.NewGuid());
            importRoleType.SingularName = "SuperDomainRoleType";
            importRoleType.PluralName = "SuperDomainRoleTypes";

            var importAbstractType = importDomain.AddDeclaredObjectType(Guid.NewGuid());
            importAbstractType.SingularName = "SuperDomainAbstractType";
            importAbstractType.PluralName = "SuperDomainAbstractTypes";
            importAbstractType.IsAbstract = true;

            var importInterfaceType = importDomain.AddDeclaredObjectType(Guid.NewGuid());
            importInterfaceType.SingularName = "SuperDomainInterfaceType";
            importInterfaceType.PluralName = "SuperDomainInterfaceTypes";
            importInterfaceType.IsInterface = true;

            var importAbstractInheritance = importAssociationType.AddDirectSupertype(importAbstractType);
            var partInterfaceInheritance = importRoleType.AddDirectSupertype(importInterfaceType);

            var importStringRelationType = importDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            importStringRelationType.AssociationType.ObjectType = importAssociationType;
            importStringRelationType.RoleType.ObjectType = (ObjectType)importDomain.Domain.Find(UnitTypeIds.StringId);
            importStringRelationType.RoleType.Size = 100;

            var importDecimalRelationType = importDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            importDecimalRelationType.AssociationType.ObjectType = importAssociationType;
            importDecimalRelationType.RoleType.ObjectType = (ObjectType)importDomain.Domain.Find(UnitTypeIds.DecimalId);
            importDecimalRelationType.RoleType.Precision = 30;
            importDecimalRelationType.RoleType.Scale = 4;

            var importCompositeRelationType = importDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            importCompositeRelationType.AssociationType.ObjectType = importAssociationType;
            importCompositeRelationType.RoleType.ObjectType = importRoleType;

            var validationReport = importDomain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            Assert.AreEqual(1, importDomain.DirectSuperDomains.Length);
            Assert.AreEqual(2, importDomain.Inheritances.Length);
            Assert.AreEqual(13, importDomain.ObjectTypes.Length);
            Assert.AreEqual(3, importDomain.RelationTypes.Length);

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            this.Domain.Inherit(importDomain);

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            Assert.AreEqual(2, this.Domain.DirectSuperDomains.Length);
            Assert.AreEqual(2, this.Domain.Inheritances.Length);
            Assert.AreEqual(13, this.Domain.ObjectTypes.Length);
            Assert.AreEqual(3, this.Domain.RelationTypes.Length);

            var importedSuperDomain = (Domain)this.Domain.Domain.Find(importDomain.Id);

            Assert.IsNotNull(this.Domain.Domain.Find(importAssociationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importRoleType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importAbstractType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importInterfaceType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importStringRelationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importDecimalRelationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(importCompositeRelationType.Id));

            Assert.IsNotNull(importedSuperDomain.Domain.Find(importAssociationType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importRoleType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importAbstractType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importInterfaceType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importStringRelationType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importDecimalRelationType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(importCompositeRelationType.Id));

            var importedAssociationType = (ObjectType)this.Domain.Domain.Find(importAssociationType.Id);
            var importedRoleType = (ObjectType)this.Domain.Domain.Find(importRoleType.Id);
            var importedAbstractType = (ObjectType)this.Domain.Domain.Find(importAbstractType.Id);
            var importedInterfaceType = (ObjectType)this.Domain.Domain.Find(importInterfaceType.Id);
            var importedStringRelationType = (RelationType)this.Domain.Domain.Find(importStringRelationType.Id);
            var importedDecimalRelationType = (RelationType)this.Domain.Domain.Find(importDecimalRelationType.Id);
            var importedCompositeRelationType = (RelationType)this.Domain.Domain.Find(importCompositeRelationType.Id);

            var importedAbstractInheritance = (Inheritance)this.Domain.Domain.Find(importAbstractInheritance.Id);
            var importedInterfaceInheritance = (Inheritance)this.Domain.Domain.Find(partInterfaceInheritance.Id);

            Assert.IsNotNull(importedAbstractInheritance);
            Assert.AreEqual(importedAssociationType, importedAbstractInheritance.Subtype);
            Assert.AreEqual(importedAbstractType, importedAbstractInheritance.Supertype);
            Assert.IsNotNull(importedInterfaceInheritance);
            Assert.AreEqual(importedRoleType, importedInterfaceInheritance.Subtype);
            Assert.AreEqual(importedInterfaceType, importedInterfaceInheritance.Supertype);

            Assert.AreEqual(importAssociationType.Id, importedAssociationType.Id);
            Assert.AreEqual(importAssociationType.SingularName, importedAssociationType.SingularName);
            Assert.AreEqual(importAssociationType.PluralName, importedAssociationType.PluralName);

            Assert.AreEqual(importRoleType.Id, importedRoleType.Id);
            Assert.AreEqual(importRoleType.SingularName, importedRoleType.SingularName);
            Assert.AreEqual(importRoleType.PluralName, importedRoleType.PluralName);

            Assert.AreEqual(importAbstractType.Id, importedAbstractType.Id);
            Assert.AreEqual(importAbstractType.SingularName, importedAbstractType.SingularName);
            Assert.AreEqual(importAbstractType.PluralName, importedAbstractType.PluralName);

            Assert.AreEqual(importInterfaceType.Id, importedInterfaceType.Id);
            Assert.AreEqual(importInterfaceType.SingularName, importedInterfaceType.SingularName);
            Assert.AreEqual(importInterfaceType.PluralName, importedInterfaceType.PluralName);

            Assert.AreEqual(importStringRelationType.AssociationType.ObjectType.Id, importedStringRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(importStringRelationType.RoleType.ObjectType.Id, importedStringRelationType.RoleType.ObjectType.Id);
            Assert.AreEqual(importStringRelationType.RoleType.Size, importedStringRelationType.RoleType.Size);

            Assert.AreEqual(importDecimalRelationType.AssociationType.ObjectType.Id, importedDecimalRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(importDecimalRelationType.RoleType.ObjectType.Id, importedDecimalRelationType.RoleType.ObjectType.Id);
            Assert.AreEqual(importDecimalRelationType.RoleType.Precision, importedDecimalRelationType.RoleType.Precision);
            Assert.AreEqual(importDecimalRelationType.RoleType.Scale, importedDecimalRelationType.RoleType.Scale);

            Assert.AreEqual(importCompositeRelationType.AssociationType.ObjectType.Id, importedCompositeRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(importCompositeRelationType.RoleType.ObjectType.Id, importedCompositeRelationType.RoleType.ObjectType.Id);
        }

        [Test]
        public void Extend()
        {
            // TODO: Documentation & Extensions
            Assert.AreEqual(9, this.Population.Types.Length);
            Assert.AreEqual(0, this.Population.Relations.Length);
            Assert.AreEqual(0, this.Population.Roles.Length);

            // Shared Super Domain
            var sharedSuperDomain = Domain.Create();
            sharedSuperDomain.Name = "SharedSuperDomain";

            var sharedSuperDomainAssociationType = sharedSuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            sharedSuperDomainAssociationType.SingularName = "SharedSuperDomainAssociationType";
            sharedSuperDomainAssociationType.PluralName = "SharedSuperDomainAssociationTypes";

            var sharedSuperDomainRoleType = sharedSuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            sharedSuperDomainRoleType.SingularName = "SharedSuperDomainRoleType";
            sharedSuperDomainRoleType.PluralName = "SharedSuperDomainRoleTypes";

            var sharedSuperDomainAbstractType = sharedSuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            sharedSuperDomainAbstractType.SingularName = "SharedSuperDomainAbstractType";
            sharedSuperDomainAbstractType.PluralName = "SharedSuperDomainAbstractTypes";
            sharedSuperDomainAbstractType.IsAbstract = true;

            var sharedSuperDomainInterfaceType = sharedSuperDomain.AddDeclaredObjectType(Guid.NewGuid());
            sharedSuperDomainInterfaceType.SingularName = "SharedSuperDomainInterfaceType";
            sharedSuperDomainInterfaceType.PluralName = "SharedSuperDomainInterfaceTypes";
            sharedSuperDomainInterfaceType.IsInterface = true;

            var sharedSuperDomainAbstractInheritance = sharedSuperDomain.AddDeclaredInheritance(Guid.NewGuid());
            sharedSuperDomainAbstractInheritance.Subtype = sharedSuperDomainAssociationType;
            sharedSuperDomainAbstractInheritance.Supertype = sharedSuperDomainAbstractType;

            this.sharedSuperDomainInterfaceInheritance = sharedSuperDomain.AddDeclaredInheritance(Guid.NewGuid());
            this.sharedSuperDomainInterfaceInheritance.Subtype = sharedSuperDomainRoleType;
            this.sharedSuperDomainInterfaceInheritance.Supertype = sharedSuperDomainInterfaceType;

            var sharedSuperDomainUnitRelationType = sharedSuperDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            sharedSuperDomainUnitRelationType.AssociationType.ObjectType = sharedSuperDomainAssociationType;
            sharedSuperDomainUnitRelationType.RoleType.ObjectType = (ObjectType)sharedSuperDomain.Domain.Find(UnitTypeIds.StringId);
            sharedSuperDomainUnitRelationType.RoleType.Size = 100;

            var sharedSuperDomainCompositeRelationType = sharedSuperDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            sharedSuperDomainCompositeRelationType.AssociationType.ObjectType = sharedSuperDomainAssociationType;
            sharedSuperDomainCompositeRelationType.RoleType.ObjectType = sharedSuperDomainRoleType;

            // Super Domain
            var superDomain = Domain.Create();
            superDomain.Name = "superDomain";

            var superDomainAssociationType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAssociationType.SingularName = "SuperDomainAssociationType";
            superDomainAssociationType.PluralName = "SuperDomainAssociationTypes";

            var superDomainRoleType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainRoleType.SingularName = "SuperDomainRoleType";
            superDomainRoleType.PluralName = "SuperDomainRoleTypes";

            var superDomainAbstractType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainAbstractType.SingularName = "SuperDomainAbstractType";
            superDomainAbstractType.PluralName = "SuperDomainAbstractTypes";
            superDomainAbstractType.IsAbstract = true;

            var superDomainInterfaceType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            superDomainInterfaceType.SingularName = "SuperDomainInterfaceType";
            superDomainInterfaceType.PluralName = "SuperDomainInterfaceTypes";
            superDomainInterfaceType.IsInterface = true;

            var superDomainAbstractInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainAbstractInheritance.Subtype = superDomainAssociationType;
            superDomainAbstractInheritance.Supertype = superDomainAbstractType;

            var superDomainInterfaceInheritance = superDomain.AddDeclaredInheritance(Guid.NewGuid());
            superDomainInterfaceInheritance.Subtype = superDomainRoleType;
            superDomainInterfaceInheritance.Supertype = superDomainInterfaceType;

            var superDomainUnitRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainUnitRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainUnitRelationType.RoleType.ObjectType = (ObjectType)superDomain.Domain.Find(UnitTypeIds.StringId);
            superDomainUnitRelationType.RoleType.Size = 200;

            var superDomainCompositeRelationType = superDomain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superDomainCompositeRelationType.AssociationType.ObjectType = superDomainAssociationType;
            superDomainCompositeRelationType.RoleType.ObjectType = superDomainRoleType;

            var validationReport = sharedSuperDomain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            Assert.AreEqual(1, this.Domain.DirectSuperDomains.Length);
            Assert.AreEqual(0, this.Domain.Inheritances.Length);
            Assert.AreEqual(9, this.Domain.ObjectTypes.Length);
            Assert.AreEqual(0, this.Domain.RelationTypes.Length);

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            superDomain.Inherit(sharedSuperDomain);

            validationReport = superDomain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            this.Domain.Inherit(superDomain);

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);

            Assert.AreEqual(2, this.Domain.DirectSuperDomains.Length);
            Assert.AreEqual(4, this.Domain.Inheritances.Length);
            Assert.AreEqual(17, this.Domain.ObjectTypes.Length);
            Assert.AreEqual(4, this.Domain.RelationTypes.Length);

            var sharedImportedSuperDomain = (Domain)this.Domain.Domain.Find(sharedSuperDomain.Id);

            // Shared SuperDomain
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainAssociationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainRoleType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainAbstractType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainInterfaceType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainUnitRelationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(sharedSuperDomainCompositeRelationType.Id));

            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainAssociationType.Id));
            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainRoleType.Id));
            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainAbstractType.Id));
            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainInterfaceType.Id));
            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainUnitRelationType.Id));
            Assert.IsNotNull(sharedImportedSuperDomain.Domain.Find(sharedSuperDomainCompositeRelationType.Id));

            var sharedImportedAssociationType = (ObjectType)this.Domain.Domain.Find(sharedSuperDomainAssociationType.Id);
            var sharedImportedRoleType = (ObjectType)this.Domain.Domain.Find(sharedSuperDomainRoleType.Id);
            var sharedImportedAbstractType = (ObjectType)this.Domain.Domain.Find(sharedSuperDomainAbstractType.Id);
            var sharedImportedInterfaceType = (ObjectType)this.Domain.Domain.Find(sharedSuperDomainInterfaceType.Id);
            var sharedImportedUnitRelationType = (RelationType)this.Domain.Domain.Find(sharedSuperDomainUnitRelationType.Id);
            var sharedImportedCompositeRelationType = (RelationType)this.Domain.Domain.Find(sharedSuperDomainCompositeRelationType.Id);

            var sharedImportedAbstractInheritance = (Inheritance)this.Domain.Domain.Find(sharedSuperDomainAbstractInheritance.Id);
            var sharedImportedInterfaceInheritance = (Inheritance)this.Domain.Domain.Find(this.sharedSuperDomainInterfaceInheritance.Id);

            Assert.IsNotNull(sharedImportedAbstractInheritance);
            Assert.AreEqual(sharedImportedAssociationType, sharedImportedAbstractInheritance.Subtype);
            Assert.AreEqual(sharedImportedAbstractType, sharedImportedAbstractInheritance.Supertype);
            Assert.IsNotNull(sharedImportedInterfaceInheritance);
            Assert.AreEqual(sharedImportedRoleType, sharedImportedInterfaceInheritance.Subtype);
            Assert.AreEqual(sharedImportedInterfaceType, sharedImportedInterfaceInheritance.Supertype);

            Assert.AreEqual(sharedSuperDomainAssociationType.Id, sharedImportedAssociationType.Id);
            Assert.AreEqual(sharedSuperDomainAssociationType.SingularName, sharedImportedAssociationType.SingularName);
            Assert.AreEqual(sharedSuperDomainAssociationType.PluralName, sharedImportedAssociationType.PluralName);

            Assert.AreEqual(sharedSuperDomainRoleType.Id, sharedImportedRoleType.Id);
            Assert.AreEqual(sharedSuperDomainRoleType.SingularName, sharedImportedRoleType.SingularName);
            Assert.AreEqual(sharedSuperDomainRoleType.PluralName, sharedImportedRoleType.PluralName);

            Assert.AreEqual(sharedSuperDomainAbstractType.Id, sharedImportedAbstractType.Id);
            Assert.AreEqual(sharedSuperDomainAbstractType.SingularName, sharedImportedAbstractType.SingularName);
            Assert.AreEqual(sharedSuperDomainAbstractType.PluralName, sharedImportedAbstractType.PluralName);

            Assert.AreEqual(sharedSuperDomainInterfaceType.Id, sharedImportedInterfaceType.Id);
            Assert.AreEqual(sharedSuperDomainInterfaceType.SingularName, sharedImportedInterfaceType.SingularName);
            Assert.AreEqual(sharedSuperDomainInterfaceType.PluralName, sharedImportedInterfaceType.PluralName);

            Assert.AreEqual(sharedSuperDomainUnitRelationType.AssociationType.ObjectType.Id, sharedImportedUnitRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(sharedSuperDomainUnitRelationType.RoleType.ObjectType.Id, sharedImportedUnitRelationType.RoleType.ObjectType.Id);

            Assert.AreEqual(sharedSuperDomainCompositeRelationType.AssociationType.ObjectType.Id, sharedImportedCompositeRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(sharedSuperDomainCompositeRelationType.RoleType.ObjectType.Id, sharedImportedCompositeRelationType.RoleType.ObjectType.Id);

            // Super Domain
            var importedSuperDomain = (Domain)this.Domain.Domain.Find(superDomain.Id);

            Assert.IsNotNull(this.Domain.Domain.Find(superDomainAssociationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(superDomainRoleType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(superDomainAbstractType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(superDomainInterfaceType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(superDomainUnitRelationType.Id));
            Assert.IsNotNull(this.Domain.Domain.Find(superDomainCompositeRelationType.Id));

            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainAssociationType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainRoleType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainAbstractType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainInterfaceType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainUnitRelationType.Id));
            Assert.IsNotNull(importedSuperDomain.Domain.Find(superDomainCompositeRelationType.Id));

            var importedAssociationType = (ObjectType)this.Domain.Domain.Find(superDomainAssociationType.Id);
            var importedRoleType = (ObjectType)this.Domain.Domain.Find(superDomainRoleType.Id);
            var importedAbstractType = (ObjectType)this.Domain.Domain.Find(superDomainAbstractType.Id);
            var importedInterfaceType = (ObjectType)this.Domain.Domain.Find(superDomainInterfaceType.Id);
            var importedUnitRelationType = (RelationType)this.Domain.Domain.Find(superDomainUnitRelationType.Id);
            var importedCompositeRelationType = (RelationType)this.Domain.Domain.Find(superDomainCompositeRelationType.Id);

            var importedAbstractInheritance = (Inheritance)this.Domain.Domain.Find(superDomainAbstractInheritance.Id);
            var importedInterfaceInheritance = (Inheritance)this.Domain.Domain.Find(superDomainInterfaceInheritance.Id);

            Assert.IsNotNull(importedAbstractInheritance);
            Assert.AreEqual(importedAssociationType, importedAbstractInheritance.Subtype);
            Assert.AreEqual(importedAbstractType, importedAbstractInheritance.Supertype);
            Assert.IsNotNull(importedInterfaceInheritance);
            Assert.AreEqual(importedRoleType, importedInterfaceInheritance.Subtype);
            Assert.AreEqual(importedInterfaceType, importedInterfaceInheritance.Supertype);

            Assert.AreEqual(superDomainAssociationType.Id, importedAssociationType.Id);
            Assert.AreEqual(superDomainAssociationType.SingularName, importedAssociationType.SingularName);
            Assert.AreEqual(superDomainAssociationType.PluralName, importedAssociationType.PluralName);

            Assert.AreEqual(superDomainRoleType.Id, importedRoleType.Id);
            Assert.AreEqual(superDomainRoleType.SingularName, importedRoleType.SingularName);
            Assert.AreEqual(superDomainRoleType.PluralName, importedRoleType.PluralName);

            Assert.AreEqual(superDomainAbstractType.Id, importedAbstractType.Id);
            Assert.AreEqual(superDomainAbstractType.SingularName, importedAbstractType.SingularName);
            Assert.AreEqual(superDomainAbstractType.PluralName, importedAbstractType.PluralName);

            Assert.AreEqual(superDomainInterfaceType.Id, importedInterfaceType.Id);
            Assert.AreEqual(superDomainInterfaceType.SingularName, importedInterfaceType.SingularName);
            Assert.AreEqual(superDomainInterfaceType.PluralName, importedInterfaceType.PluralName);

            Assert.AreEqual(superDomainUnitRelationType.AssociationType.ObjectType.Id, importedUnitRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(superDomainUnitRelationType.RoleType.ObjectType.Id, importedUnitRelationType.RoleType.ObjectType.Id);

            Assert.AreEqual(superDomainCompositeRelationType.AssociationType.ObjectType.Id, importedCompositeRelationType.AssociationType.ObjectType.Id);
            Assert.AreEqual(superDomainCompositeRelationType.RoleType.ObjectType.Id, importedCompositeRelationType.RoleType.ObjectType.Id);
        }
    }

    public class DomainTestWithSuperDomains : DomainTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}