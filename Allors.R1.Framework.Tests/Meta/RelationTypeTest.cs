//------------------------------------------------------------------------------------------------- 
// <copyright file="RelationTypeTest.cs" company="Allors bvba">
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
// <summary>Defines the RelationTypeTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Allors.R1.Meta.Events;

    using NUnit.Framework;

    [TestFixture]
    public class RelationTypeTest : AbstractTest
    {
        private readonly List<MetaObjectChangedEventArgs> metaObjectChangedEvents = new List<MetaObjectChangedEventArgs>();

        [Test]
        public void ChangedEvent()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsTrue(this.Domain.IsValid);

            this.Domain.MetaObjectChanged += this.DomainMetaObjectChanged;

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);

            relationType.SendChangedEvent();

            Assert.AreEqual(1, this.metaObjectChangedEvents.Count);

            var args = this.metaObjectChangedEvents[0];
            Assert.AreEqual(relationType, args.MetaObject);

            this.metaObjectChangedEvents.Clear();

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);

            // No Events
            relationType.Reset();

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count); 
        }

        [Test]
        public void Defaults()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.IsIndexed = true;
            relationType.IsDerived = true;

            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.Reset();

            Assert.IsTrue(relationType.IsIsIndexedDefault);
            Assert.IsTrue(relationType.IsIsDerivedDefault);
        }

        [Test]
        public void Delete()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.Delete();

            Assert.IsFalse(this.Population.C1.IsDeleted);
            Assert.IsFalse(this.Population.C2.IsDeleted);
            Assert.IsTrue(relationType.IsDeleted);
            Assert.IsTrue(association.IsDeleted);
            Assert.IsTrue(role.IsDeleted);

            Assert.AreEqual(0, this.Population.Relations.Length);
            Assert.AreEqual(0, this.Population.Associations.Length);
            Assert.AreEqual(0, this.Population.Roles.Length);
        }

        [Test]
        public void HasExclusiveClassHierarchies()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);

            var association = relationType.AssociationType;

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);

            var role = relationType.RoleType;

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = this.Population.I2;

            Assert.IsTrue(relationType.ExistExclusiveRootClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = null;

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);

            association.ObjectType = null;
            role.ObjectType = this.Population.I2;

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);

            association.ObjectType = this.Population.I1;
            role.ObjectType = this.Population.I12;

            Assert.IsFalse(relationType.ExistExclusiveRootClasses);
        }

        [Test]
        public void Multiplicity()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.Domain.IsValid);

            Assert.IsTrue(relationType.IsOneToOne);

            role.IsMany = true;

            Assert.IsTrue(relationType.IsOneToMany);

            association.IsMany = true;

            Assert.IsTrue(relationType.IsManyToMany);

            role.IsMany = false;

            Assert.IsTrue(relationType.IsManyToOne);
        }

        [Test]
        public void Reset()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.IsIndexed = true;
            relationType.IsDerived = true;

            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.Reset();

            Assert.IsTrue(relationType.ExistId);
            Assert.IsFalse(relationType.IsIndexed);
            Assert.IsFalse(relationType.IsDerived);

            Assert.IsFalse(this.Population.C1.IsDeleted);
            Assert.IsFalse(this.Population.C2.IsDeleted);
            Assert.IsFalse(relationType.IsDeleted);
            Assert.IsFalse(association.IsDeleted);
            Assert.IsFalse(role.IsDeleted);
        }

        [Test]
        public void ValidateCompositeCardinality()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.C2;

            relationType.AssociationType.IsMany = false;
            relationType.RoleType.IsMany = false;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.AssociationType.IsMany = true;
            relationType.RoleType.IsMany = false;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.AssociationType.IsMany = false;
            relationType.RoleType.IsMany = true;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.AssociationType.IsMany = true;
            relationType.RoleType.IsMany = true;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.AssociationType.IsMany = false;
            relationType.RoleType.IsMany = false;

            Assert.IsTrue(this.Domain.IsValid);
        }

        [Test]
        public void ValidateDuplicateRelation()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.AssociationType.AssignedSingularName = "aa";
            relationType.AssociationType.AssignedPluralName = "aas";
            relationType.RoleType.ObjectType = this.Population.C2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsTrue(this.Domain.IsValid);

            var otherRelationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            otherRelationType.AssociationType.ObjectType = this.Population.C3;
            otherRelationType.AssociationType.AssignedSingularName = "aa";
            otherRelationType.AssociationType.AssignedPluralName = "aas";
            otherRelationType.RoleType.ObjectType = this.Population.C4;
            otherRelationType.RoleType.AssignedSingularName = "bb";
            otherRelationType.RoleType.AssignedPluralName = "bbs";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void ValidateDuplicateReverseRelation()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.AssociationType.AssignedSingularName = "aa";
            relationType.AssociationType.AssignedPluralName = "aas";
            relationType.RoleType.ObjectType = this.Population.C2;
            relationType.RoleType.AssignedSingularName = "bb";
            relationType.RoleType.AssignedPluralName = "bbs";
            Assert.IsTrue(this.Domain.IsValid);

            var otherRelationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            otherRelationType.AssociationType.ObjectType = this.Population.C3;
            otherRelationType.AssociationType.AssignedSingularName = "bb";
            otherRelationType.AssociationType.AssignedPluralName = "bbs";
            otherRelationType.RoleType.ObjectType = this.Population.C4;
            otherRelationType.RoleType.AssignedSingularName = "aa";
            otherRelationType.RoleType.AssignedPluralName = "aas";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void ValidateNameMinimumLength()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var association = relationType.AssociationType;
            var role = relationType.RoleType;
            association.ObjectType = this.Population.C1;
            role.ObjectType = this.Population.C2;

            Assert.IsTrue(this.Domain.IsValid);

            association.AssignedSingularName = "A";
            association.AssignedPluralName = "CD";

            Assert.IsFalse(this.Domain.IsValid);

            association.AssignedSingularName = "AB";

            Assert.IsTrue(this.Domain.IsValid);

            association.AssignedPluralName = "C";

            Assert.IsFalse(this.Domain.IsValid);

            association.AssignedPluralName = "CD";

            Assert.IsTrue(this.Domain.IsValid);

            role.AssignedSingularName = "E";
            role.AssignedPluralName = "GH";

            Assert.IsFalse(this.Domain.IsValid);

            role.AssignedSingularName = "EF";

            Assert.IsTrue(this.Domain.IsValid);

            role.AssignedPluralName = "G";

            Assert.IsFalse(this.Domain.IsValid);

            role.AssignedPluralName = "GH";

            Assert.IsTrue(this.Domain.IsValid);
        }

        [Test]
        public void DeriveUnitCardinality()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.IntegerType;

            relationType.AssociationType.IsMany = false;
            relationType.RoleType.IsMany = false;

            Assert.IsTrue(this.Domain.IsValid);

            relationType.AssociationType.IsMany = true;
            Assert.IsFalse(relationType.AssociationType.IsMany);

            relationType.RoleType.IsMany = true;
            Assert.IsFalse(relationType.RoleType.IsMany);
        }

        [Test]
        public void DefaultsAfterSaveAndLoad()
        {
            var objectType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.SingularName = "Object";
            objectType.PluralName = "Objects";

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = objectType;
            relationType.RoleType.ObjectType = objectType;
            relationType.RoleType.AssignedSingularName = "Relation";
            relationType.RoleType.AssignedPluralName = "Relations";

            var validation = this.Domain.Validate();
            Assert.IsFalse(validation.ContainsErrors);

            var xml = this.Domain.Xml;

            var reader = new XmlTextReader(new StringReader(xml));
            var copy = Domain.Load(reader);
            
            relationType = (RelationType)copy.Domain.Find(relationType.Id);

            Assert.IsFalse(relationType.IsIndexed);
            Assert.IsFalse(relationType.IsDerived);
        }

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            this.metaObjectChangedEvents.Add(args);
        }
    }

    public class RelationTypeTestWithSuperDomains : RelationTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}