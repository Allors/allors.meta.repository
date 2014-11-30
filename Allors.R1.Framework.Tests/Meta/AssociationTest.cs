//------------------------------------------------------------------------------------------------- 
// <copyright file="AssociationTest.cs" company="Allors bvba">
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
// <summary>Defines the AssociationTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections.Generic;

    using Allors.R1.Meta.Events;

    using NUnit.Framework;

    [TestFixture]
    public class AssociationTest : AbstractTest
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

            relationType.AssociationType.SendChangedEvent();

            Assert.AreEqual(1, this.metaObjectChangedEvents.Count);

            var args = this.metaObjectChangedEvents[0];
            Assert.AreEqual(relationType, args.MetaObject);

            this.metaObjectChangedEvents.Clear();

            relationType.AssociationType.AssignedSingularName = "NoEvents";

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);
        }

        [Test]
        public void Defaults()
        {
            this.Populate();

            var associationId = Guid.NewGuid();
            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), associationId, Guid.NewGuid());

            var association = relationType.AssociationType;

            Assert.IsTrue(association.ExistId);
            Assert.AreEqual(associationId, association.Id);

            association.ObjectType = this.Population.C1;
            association.AssignedSingularName = "Singular";
            association.AssignedSingularName = "Plural";
            association.IsMany = !association.IsMany;

            association.Reset();

            Assert.IsTrue(association.ExistId);
            Assert.AreEqual(associationId, association.Id);
            Assert.IsTrue(association.IsAssignedPluralNameDefault);
            Assert.IsTrue(association.IsAssignedSingularNameDefault);
            Assert.IsTrue(association.IsIsManyDefault);
            Assert.IsTrue(association.IsObjectTypeDefault);
        }

        [Test]
        public void Delete()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;

            var exceptionThrown = false;
            try
            {
                association.Delete();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void Immutable()
        {
            var relation1 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var relation2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            try
            {
                relation2.AssociationType = relation1.AssociationType;
                Assert.Fail();
            }
            catch
            {
            }
        }

        [Test]
        public void Name()
        {
            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var association = relationType.AssociationType;

            Assert.AreEqual(relationType.Id.ToString().ToLower(), association.SingularName);
            Assert.AreEqual(relationType.Id.ToString().ToLower(), association.PluralName);
        }

        [Test]
        public void PluralName()
        {
            var company = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";

            var companyPerson = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            companyPerson.AssociationType.ObjectType = company;
            companyPerson.AssociationType.IsMany = true;
            companyPerson.RoleType.ObjectType = person;

            Assert.AreEqual("Companies", companyPerson.AssociationType.PluralName);

            companyPerson.AssociationType.AssignedSingularName = "Bedrijf";

            Assert.AreEqual("Companies", companyPerson.AssociationType.PluralName);

            companyPerson.AssociationType.AssignedPluralName = "Bedrijven";

            Assert.AreEqual("Bedrijven", companyPerson.AssociationType.PluralName);
        }

        [Test]
        public void Reset()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var association = relationType.AssociationType;
            association.ObjectType = this.Population.C1;
            association.AssignedSingularName = "Singular";
            association.AssignedSingularName = "Plural";
            association.IsMany = !association.IsMany;

            association.Reset();

            Assert.IsNull(association.AssignedPluralName);
            Assert.IsNull(association.AssignedSingularName);
            Assert.IsTrue(association.IsOne);
            Assert.IsNull(association.ObjectType);

            Assert.IsFalse(this.Population.C1.IsDeleted);
            Assert.IsFalse(relationType.IsDeleted);
            Assert.IsFalse(association.IsDeleted);
        }

        [Test]
        public void RootType()
        {
            var left = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            left.SingularName = "Left";
            left.PluralName = "Lefts";
            left.IsAbstract = true;

            var right = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            right.IsInterface = true;
            right.SingularName = "Right";
            right.PluralName = "Rights";

            var superRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superRight.IsInterface = true;
            superRight.SingularName = "SuperRight";
            superRight.PluralName = "SuperRights";

            var subRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subRight.SingularName = "SubRight";
            subRight.PluralName = "SubRights";

            var subSuperRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subSuperRight.IsInterface = true;
            subSuperRight.SingularName = "SubSuperRight";
            subSuperRight.PluralName = "SubSuperRights";

            var superLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superLeft.IsInterface = true;
            superLeft.SingularName = "SuperLeft";
            superLeft.PluralName = "SuperLefts";

            var otherRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            otherRight.SingularName = "OtherRight";
            otherRight.PluralName = "OtherRights";

            right.AddDirectSupertype(superRight);
            subRight.AddDirectSupertype(right);
            subRight.AddDirectSupertype(subSuperRight);

            left.AddDirectSupertype(superLeft);

            // Default
            var leftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftRight.AssociationType.ObjectType = left;
            leftRight.RoleType.ObjectType = right;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);

            // left -> left
            var leftLeft = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftLeft.AssociationType.ObjectType = left;
            leftLeft.RoleType.ObjectType = otherRight;
            leftLeft.RoleType.AssignedSingularName = "Links";
            leftLeft.RoleType.AssignedPluralName = "Linksen";

            // left (1) -> left (1)
            leftLeft.AssociationType.IsMany = false;
            leftLeft.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            // left (many) -> left (1)
            leftLeft.AssociationType.IsMany = true;
            leftLeft.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            // left (1) -> left (many)
            leftLeft.AssociationType.IsMany = false;
            leftLeft.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftLeft.AssociationType.RootName);

            // left (many) -> left (many)
            leftLeft.AssociationType.IsMany = true;
            leftLeft.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("LinksLeft", leftLeft.AssociationType.RootName);

            leftLeft.Delete();

            // left -> other right
            var leftOtherRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftOtherRight.AssociationType.ObjectType = left;
            leftOtherRight.RoleType.ObjectType = otherRight;
            leftOtherRight.RoleType.AssignedSingularName = "Rechts";

            // left (1) -> other right (1)
            leftOtherRight.AssociationType.IsMany = false;
            leftOtherRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            // left (many) -> other right (1)
            leftOtherRight.AssociationType.IsMany = true;
            leftOtherRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            // left (1) -> other right (many)
            leftOtherRight.AssociationType.IsMany = false;
            leftOtherRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftOtherRight.AssociationType.RootName);

            // left (many) -> other right (many)
            leftOtherRight.AssociationType.IsMany = true;
            leftOtherRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftOtherRight.AssociationType.RootName);

            leftOtherRight.Delete();

            // super left -> right
            var superLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superLeftRight.AssociationType.ObjectType = left;
            superLeftRight.AssociationType.AssignedSingularName = "Left";
            superLeftRight.RoleType.ObjectType = right;
            superLeftRight.RoleType.AssignedSingularName = "Rechts";

            // super left (1) -> right (1)
            superLeftRight.AssociationType.IsMany = false;
            superLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            // super left (many) -> right (1)
            superLeftRight.AssociationType.IsMany = true;
            superLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            // super left (1) -> right (many)
            superLeftRight.AssociationType.IsMany = false;
            superLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", superLeftRight.AssociationType.RootName);

            // super left (many) -> right (many)
            superLeftRight.AssociationType.IsMany = true;
            superLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", superLeftRight.AssociationType.RootName);

            // left -> sub super right
            var leftSubSuperRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftSubSuperRight.AssociationType.ObjectType = left;
            leftSubSuperRight.AssociationType.AssignedSingularName = "Left";
            leftSubSuperRight.RoleType.ObjectType = right;
            leftSubSuperRight.RoleType.AssignedSingularName = "Rechts";

            // left (1) -> sub super right (1)
            leftSubSuperRight.AssociationType.IsMany = false;
            leftSubSuperRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            // left (many) -> sub super right (1)
            leftSubSuperRight.AssociationType.IsMany = true;
            leftSubSuperRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            // left (1) -> sub super right (many)
            leftSubSuperRight.AssociationType.IsMany = false;
            leftSubSuperRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("Left", leftSubSuperRight.AssociationType.RootName);

            // left (many) -> sub super right (many)
            leftSubSuperRight.AssociationType.IsMany = true;
            leftSubSuperRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("Left", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("RightLeft", leftRight.AssociationType.RootName);
            Assert.AreEqual("RechtsLeft", leftSubSuperRight.AssociationType.RootName);
        }

        [Test]
        public void SingularName()
        {
            var company = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";

            var person = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";

            var companyPerson = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            companyPerson.AssociationType.ObjectType = company;
            companyPerson.AssociationType.IsMany = false;
            companyPerson.RoleType.ObjectType = person;

            Assert.AreEqual("Company", companyPerson.AssociationType.SingularName);

            company.PluralName = "Companies";

            Assert.AreEqual("Company", companyPerson.AssociationType.SingularName);

            companyPerson.AssociationType.AssignedPluralName = "Bedrijven";

            Assert.AreEqual("Company", companyPerson.AssociationType.SingularName);

            companyPerson.AssociationType.AssignedSingularName = "Bedrijf";

            Assert.AreEqual("Bedrijf", companyPerson.AssociationType.SingularName);
        }

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            this.metaObjectChangedEvents.Add(args);
        }
    }

    public class AssociationTestWithSuperDomains : AssociationTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}