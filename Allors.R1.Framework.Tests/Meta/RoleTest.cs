//------------------------------------------------------------------------------------------------- 
// <copyright file="RoleTest.cs" company="Allors bvba">
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
// <summary>Defines the RoleTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections.Generic;

    using Allors.R1.Meta.Events;

    using NUnit.Framework;

    [TestFixture]
    public class RoleTest : AbstractTest
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

            relationType.RoleType.SendChangedEvent();

            Assert.AreEqual(1, this.metaObjectChangedEvents.Count);

            var args = this.metaObjectChangedEvents[0];
            Assert.AreEqual(relationType, args.MetaObject);

            this.metaObjectChangedEvents.Clear();

            relationType.RoleType.AssignedSingularName = "NoEvents";

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);
        }

        [Test]
        public void Defaults()
        {
            this.Populate();

            var roleId = Guid.NewGuid();
            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), roleId);

            var role = relationType.RoleType;

            Assert.IsTrue(role.ExistId);
            Assert.AreEqual(roleId, role.Id);

            role.ObjectType = this.Population.C1;
            role.AssignedSingularName = "Singular";
            role.AssignedSingularName = "Plural";
            role.IsMany = !role.IsMany;
            role.Scale = 3;
            role.Size = 30;

            role.Reset();

            Assert.IsTrue(role.ExistId);
            Assert.AreEqual(roleId, role.Id);

            Assert.IsTrue(role.IsAssignedPluralNameDefault);
            Assert.IsTrue(role.IsAssignedSingularNameDefault);
            Assert.IsTrue(role.IsScaleDefault);
            Assert.IsTrue(role.IsSizeDefault);
            Assert.IsTrue(role.IsIsManyDefault);
            Assert.IsTrue(role.IsObjectTypeDefault);
        }

        [Test]
        public void Delete()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C1;
            role.AssignedSingularName = "Singular";
            role.AssignedSingularName = "Plural";
            role.IsMany = !role.IsMany;
            role.Scale = 3;
            role.Size = 30;

            var exceptionThrown = false;
            try
            {
                role.Delete();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void HierarchyName()
        {
            var left = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            left.SingularName = "Left";
            left.PluralName = "Lefts";
            left.IsAbstract = true;

            var superLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superLeft.SingularName = "SuperLeft";
            superLeft.PluralName = "SuperLefts";
            superLeft.IsInterface = true;

            var subLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subLeft.SingularName = "SubLeft";
            subLeft.PluralName = "SubLefts";

            var subSuperLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subSuperLeft.SingularName = "SubSuperLeft";
            subSuperLeft.PluralName = "SubSuperLefts";
            subSuperLeft.IsInterface = true;

            var right = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            right.SingularName = "Right";
            right.PluralName = "Rights";

            var otherLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            otherLeft.SingularName = "OtherLeft";
            otherLeft.PluralName = "OtherLefts";

            var superRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superRight.SingularName = "SuperRight";
            superRight.PluralName = "SuperRights";
            superRight.IsAbstract = true;

            var otherRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            otherRight.SingularName = "OtherRight";
            otherRight.PluralName = "OtherRights";

            left.AddDirectSupertype(superLeft);
            subLeft.AddDirectSupertype(left);
            subLeft.AddDirectSupertype(subSuperLeft);

            right.AddDirectSupertype(superRight);

            // default
            var leftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftRight.AssociationType.ObjectType = left;
            leftRight.RoleType.ObjectType = right;

            Assert.AreEqual("Right", leftRight.RoleType.HierarchySingularName);

            var leftSuperRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftSuperRight.AssociationType.ObjectType = left;
            leftSuperRight.AssociationType.AssignedSingularName = "Links";
            leftSuperRight.RoleType.ObjectType = superRight;
            leftSuperRight.RoleType.AssignedSingularName = "Right";

            Assert.AreEqual("LeftRight", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("LinksRight", leftSuperRight.RoleType.HierarchySingularName);

            leftSuperRight.Delete();

            // left -> other right
            var leftOtherRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftOtherRight.AssociationType.ObjectType = left;
            leftOtherRight.AssociationType.AssignedSingularName = "Links";
            leftOtherRight.RoleType.ObjectType = otherRight;
            leftOtherRight.RoleType.AssignedSingularName = "Right";

            Assert.AreEqual("LeftRight", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.HierarchySingularName);

            leftOtherRight.Delete();

            // other left -> right
            var otherLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            otherLeftRight.AssociationType.ObjectType = otherLeft;
            otherLeftRight.RoleType.ObjectType = right;

            Assert.AreEqual("Right", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("Right", otherLeftRight.RoleType.HierarchySingularName);

            otherLeftRight.Delete();

            // super left -> right
            var superLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superLeftRight.AssociationType.ObjectType = subSuperLeft;
            superLeftRight.AssociationType.AssignedSingularName = "Links";
            superLeftRight.RoleType.ObjectType = right;

            Assert.AreEqual("LeftRight", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.HierarchySingularName);

            superLeftRight.Delete();

            // sub left -> right
            var subLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            subLeftRight.AssociationType.ObjectType = subSuperLeft;
            subLeftRight.AssociationType.AssignedSingularName = "Links";
            subLeftRight.RoleType.ObjectType = right;

            Assert.AreEqual("LeftRight", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.HierarchySingularName);

            subLeftRight.Delete();

            // subsuper left -> right
            var subSuperLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            subSuperLeftRight.AssociationType.ObjectType = subSuperLeft;
            subSuperLeftRight.AssociationType.AssignedSingularName = "Links";
            subSuperLeftRight.RoleType.ObjectType = right;

            Assert.AreEqual("LeftRight", leftRight.RoleType.HierarchySingularName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.HierarchySingularName);

            subSuperLeftRight.Delete();
        }

        [Test]
        public void HierarchyNameDifferenParent()
        {
            this.Populate();

            var i = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            i.SingularName = "i";
            i.IsInterface = true;

            var i1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_c2.AssociationType.ObjectType = this.Population.I1;
            i1_c2.RoleType.ObjectType = this.Population.C2;

            var iC2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            iC2.AssociationType.ObjectType = i;
            iC2.RoleType.ObjectType = this.Population.C2;
            {
                Assert.AreEqual("c2", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("c2", iC2.RoleType.HierarchySingularName);
            }

            this.Population.C1.AddDirectSupertype(i);
            {
                Assert.AreEqual("i1c2", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ic2", iC2.RoleType.HierarchySingularName);
            }
        }

        [Test]
        public void HierarchyNameRootClass()
        {
            this.Populate();

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = this.Population.C1;
            c1_c2.RoleType.ObjectType = this.Population.C2;
            var c2_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c2_c2.AssociationType.ObjectType = this.Population.C2;
            c2_c2.RoleType.ObjectType = this.Population.C2;

            Assert.AreEqual("c2", c1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c2", c2_c2.RoleType.HierarchySingularName);

            var c3_c4 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c3_c4.AssociationType.ObjectType = this.Population.C3;
            c3_c4.RoleType.ObjectType = this.Population.C4;
            var c4_c4 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c4_c4.AssociationType.ObjectType = this.Population.C4;
            c4_c4.RoleType.ObjectType = this.Population.C4;

            Assert.AreEqual("c4", c3_c4.RoleType.HierarchySingularName);
            Assert.AreEqual("c4", c4_c4.RoleType.HierarchySingularName);
        }

        [Test]
        public void HierarchyNameWithAssociationName()
        {
            this.Populate();
            {
                // 1. default names
                var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                c1_c2.AssociationType.ObjectType = this.Population.C1;
                c1_c2.AssociationType.AssignedSingularName = "c";
                c1_c2.RoleType.ObjectType = this.Population.C2;

                var a1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                a1_c2.AssociationType.ObjectType = this.Population.A1;
                a1_c2.AssociationType.AssignedSingularName = "a";
                a1_c2.RoleType.ObjectType = this.Population.C2;

                var i1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                i1_c2.AssociationType.ObjectType = this.Population.I1;
                i1_c2.AssociationType.AssignedSingularName = "i";
                i1_c2.RoleType.ObjectType = this.Population.C2;

                Assert.AreEqual("ic2", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ac2", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("cc2", c1_c2.RoleType.HierarchySingularName);

                // 2. Explicit Names
                i1_c2.RoleType.AssignedSingularName = "c2";

                Assert.AreEqual("ic2", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ac2", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("cc2", c1_c2.RoleType.HierarchySingularName);

                a1_c2.RoleType.AssignedSingularName = "c2";
                c1_c2.RoleType.AssignedSingularName = "c2";

                Assert.AreEqual("ic2", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ac2", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("cc2", c1_c2.RoleType.HierarchySingularName);

                i1_c2.RoleType.AssignedSingularName = "x";

                Assert.AreEqual("x", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ac2", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("cc2", c1_c2.RoleType.HierarchySingularName);

                a1_c2.RoleType.AssignedSingularName = "x";

                Assert.AreEqual("ix", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ax", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("c2", c1_c2.RoleType.HierarchySingularName);

                c1_c2.RoleType.AssignedSingularName = "x";

                Assert.AreEqual("ix", i1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("ax", a1_c2.RoleType.HierarchySingularName);
                Assert.AreEqual("cx", c1_c2.RoleType.HierarchySingularName);
            }
        }

        [Test]
        public void HierarchyNameWithoutAssociationName()
        {
            this.Populate();

            // 1. default names
            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = this.Population.C1;
            c1_c2.RoleType.ObjectType = this.Population.C2;

            var a1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_c2.AssociationType.ObjectType = this.Population.A1;
            a1_c2.RoleType.ObjectType = this.Population.C2;

            var i1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_c2.AssociationType.ObjectType = this.Population.I1;
            i1_c2.RoleType.ObjectType = this.Population.C2;

            Assert.AreEqual("i1c2", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1c2", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c1c2", c1_c2.RoleType.HierarchySingularName);

            // 2. Explicit Names
            i1_c2.RoleType.AssignedSingularName = "c2";

            Assert.AreEqual("i1c2", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1c2", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c1c2", c1_c2.RoleType.HierarchySingularName);

            a1_c2.RoleType.AssignedSingularName = "c2";
            c1_c2.RoleType.AssignedSingularName = "c2";

            Assert.AreEqual("i1c2", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1c2", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c1c2", c1_c2.RoleType.HierarchySingularName);

            i1_c2.RoleType.AssignedSingularName = "x";

            Assert.AreEqual("x", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1c2", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c1c2", c1_c2.RoleType.HierarchySingularName);

            a1_c2.RoleType.AssignedSingularName = "x";

            Assert.AreEqual("i1x", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1x", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c2", c1_c2.RoleType.HierarchySingularName);

            c1_c2.RoleType.AssignedSingularName = "x";

            Assert.AreEqual("i1x", i1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("a1x", a1_c2.RoleType.HierarchySingularName);
            Assert.AreEqual("c1x", c1_c2.RoleType.HierarchySingularName);
        }

        [Test]
        public void Immutable()
        {
            var relation1 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var relation2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            try
            {
                relation2.RoleType = relation1.RoleType;
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

            var role = relationType.RoleType;

            role.IsMany = false;
            Assert.AreEqual(relationType.Id.ToString(), role.Name);
            role.IsMany = true;
            Assert.AreEqual(relationType.Id.ToString(), role.Name);
        }

        [Test]
        public void PluralName()
        {
            var company = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            var companyPerson = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            companyPerson.AssociationType.ObjectType = company;
            companyPerson.RoleType.ObjectType = person;
            companyPerson.RoleType.IsMany = true;

            Assert.AreEqual("Persons", companyPerson.RoleType.PluralName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";

            Assert.AreEqual("Persons", companyPerson.RoleType.PluralName);

            companyPerson.RoleType.AssignedPluralName = "Personen";

            Assert.AreEqual("Personen", companyPerson.RoleType.PluralName);

            person.PluralName = null;
            Assert.AreEqual("Personen", companyPerson.RoleType.PluralName);
        }

        [Test]
        public void Reset()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var role = relationType.RoleType;
            role.ObjectType = this.Population.C1;
            role.AssignedSingularName = "Singular";
            role.AssignedSingularName = "Plural";
            role.IsMany = !role.IsMany;
            role.Scale = 3;
            role.Size = 30;

            role.Reset();

            Assert.IsNull(role.AssignedPluralName);
            Assert.IsNull(role.AssignedSingularName);
            Assert.IsFalse(role.ExistScale);
            Assert.IsFalse(role.ExistSize);
            Assert.IsTrue(role.IsOne);
            Assert.IsNull(role.ObjectType);

            Assert.IsFalse(this.Population.C1.IsDeleted);
            Assert.IsFalse(relationType.IsDeleted);
            Assert.IsFalse(role.IsDeleted);
        }

        [Test]
        public void RootName()
        {
            var left = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            left.SingularName = "Left";
            left.PluralName = "Lefts";
            left.IsAbstract = true;

            var superLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superLeft.IsInterface = true;
            superLeft.SingularName = "SuperLeft";
            superLeft.PluralName = "SuperLefts";

            var subLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subLeft.SingularName = "SubLeft";
            subLeft.PluralName = "SubLefts";

            var subSuperLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            subSuperLeft.IsInterface = true;
            subSuperLeft.SingularName = "SubSuperLeft";
            subSuperLeft.PluralName = "SubSuperLefts";

            var right = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            right.SingularName = "Right";
            right.PluralName = "Rights";

            var otherLeft = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            otherLeft.SingularName = "OtherLeft";
            otherLeft.PluralName = "OtherLefts";

            var superRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superRight.SingularName = "OtherRight";
            superRight.PluralName = "OtherRights";
            superRight.IsInterface = true;

            var otherRight = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            otherRight.SingularName = "OtherRight";
            otherRight.PluralName = "OtherRights";

            left.AddDirectSupertype(superLeft);
            subLeft.AddDirectSupertype(left);
            subLeft.AddDirectSupertype(subSuperLeft);

            right.AddDirectSupertype(superRight);

            var leftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftRight.AssociationType.ObjectType = left;
            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.ObjectType = right;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);

            // left -> other right
            var leftOtherRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            leftOtherRight.AssociationType.ObjectType = left;
            leftOtherRight.AssociationType.AssignedSingularName = "Links";
            leftOtherRight.RoleType.ObjectType = otherRight;
            leftOtherRight.RoleType.AssignedSingularName = "Right";

            // left (1) -> other right (1)
            leftOtherRight.AssociationType.IsMany = false;
            leftOtherRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", leftOtherRight.RoleType.RootName);

            // left (many) -> other right (1)
            leftOtherRight.AssociationType.IsMany = true;
            leftOtherRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", leftOtherRight.RoleType.RootName);

            // left (1) -> other right (many)
            leftOtherRight.AssociationType.IsMany = false;
            leftOtherRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            // left (many) -> other right (many)
            leftOtherRight.AssociationType.IsMany = true;
            leftOtherRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", leftOtherRight.RoleType.RootName);

            leftOtherRight.Delete();

            // other left -> right
            var otherLeftRright = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            otherLeftRright.AssociationType.ObjectType = otherLeft;
            otherLeftRright.AssociationType.AssignedSingularName = "Links";
            otherLeftRright.RoleType.ObjectType = right;

            // other left (1) -> right (1)
            otherLeftRright.AssociationType.IsMany = false;
            otherLeftRright.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            // other left (many) -> right (1)
            otherLeftRright.AssociationType.IsMany = true;
            otherLeftRright.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", otherLeftRright.RoleType.RootName);

            // other left (1) -> right (many)
            otherLeftRright.AssociationType.IsMany = false;
            otherLeftRright.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            // other left (many) -> right (many)
            otherLeftRright.AssociationType.IsMany = true;
            otherLeftRright.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", otherLeftRright.RoleType.RootName);

            otherLeftRright.Delete();

            // super left -> right
            var superLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            superLeftRight.AssociationType.ObjectType = subSuperLeft;
            superLeftRight.AssociationType.AssignedSingularName = "Links";
            superLeftRight.RoleType.ObjectType = right;

            // super left (1) -> right (1)
            superLeftRight.AssociationType.IsMany = false;
            superLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", superLeftRight.RoleType.RootName);

            // super left (many) -> right (1)
            superLeftRight.AssociationType.IsMany = true;
            superLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", superLeftRight.RoleType.RootName);

            // super left (1) -> right (many)
            superLeftRight.AssociationType.IsMany = false;
            superLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            // super left (many) -> right (many)
            superLeftRight.AssociationType.IsMany = true;
            superLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", superLeftRight.RoleType.RootName);

            superLeftRight.Delete();

            // sub left -> right
            var subLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            subLeftRight.AssociationType.ObjectType = subSuperLeft;
            subLeftRight.AssociationType.AssignedSingularName = "Links";
            subLeftRight.RoleType.ObjectType = right;

            // sub left (1) -> right (1)
            subLeftRight.AssociationType.IsMany = false;
            subLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subLeftRight.RoleType.RootName);

            // sub left (many) -> right (1)
            subLeftRight.AssociationType.IsMany = true;
            subLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subLeftRight.RoleType.RootName);

            // sub left (1) -> right (many)
            subLeftRight.AssociationType.IsMany = false;
            subLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            // sub left (many) -> right (many)
            subLeftRight.AssociationType.IsMany = true;
            subLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subLeftRight.RoleType.RootName);

            subLeftRight.Delete();

            // subsuper left -> right
            var subSuperLeftRight = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            subSuperLeftRight.AssociationType.ObjectType = subSuperLeft;
            subSuperLeftRight.AssociationType.AssignedSingularName = "Links";
            subSuperLeftRight.RoleType.ObjectType = right;

            // subSuper left (1) -> right (1)
            subSuperLeftRight.AssociationType.IsMany = false;
            subSuperLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subSuperLeftRight.RoleType.RootName);

            // subSuper left (many) -> right (1)
            subSuperLeftRight.AssociationType.IsMany = true;
            subSuperLeftRight.RoleType.IsMany = false;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("Right", subSuperLeftRight.RoleType.RootName);

            // subSuper left (1) -> right (many)
            subSuperLeftRight.AssociationType.IsMany = false;
            subSuperLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            // subSuper left (many) -> right (many)
            subSuperLeftRight.AssociationType.IsMany = true;
            subSuperLeftRight.RoleType.IsMany = true;

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = false;

            Assert.AreEqual("Right", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = false;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            leftRight.AssociationType.IsMany = true;
            leftRight.RoleType.IsMany = true;

            Assert.AreEqual("LeftRight", leftRight.RoleType.RootName);
            Assert.AreEqual("LinksRight", subSuperLeftRight.RoleType.RootName);

            subSuperLeftRight.Delete();
        }

        [Test]
        public void SingularName()
        {
            var company = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            company.SingularName = "Company";
            company.PluralName = "Companies";

            var person = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            person.SingularName = "Person";
            person.PluralName = "Persons";

            var companyPerson = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            companyPerson.AssociationType.ObjectType = company;
            companyPerson.RoleType.ObjectType = person;

            Assert.AreEqual("Person", companyPerson.RoleType.SingularName);

            person.PluralName = "Persons";
            Assert.AreEqual("Person", companyPerson.RoleType.SingularName);

            companyPerson.RoleType.AssignedPluralName = "Personen";
            Assert.AreEqual("Person", companyPerson.RoleType.SingularName);

            companyPerson.RoleType.AssignedSingularName = "Persoon";
            Assert.AreEqual("Persoon", companyPerson.RoleType.SingularName);
        }

        [Test]
        public void DeriveSize()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.ExistSize);

            relationType.RoleType.ObjectType = this.Population.StringType;

            Assert.IsTrue(relationType.RoleType.ExistSize);
            Assert.AreEqual(256, relationType.RoleType.Size);

            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.ExistSize);
        }
        
        [Test]
        public void DerivePrecisionScale()
        {
            this.Populate();

            var relationType = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = this.Population.C1;
            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.ExistPrecision);
            Assert.IsFalse(relationType.RoleType.ExistScale);

            relationType.RoleType.ObjectType = this.Population.DecimalType;

            Assert.IsTrue(relationType.RoleType.ExistPrecision);
            Assert.AreEqual(19, relationType.RoleType.Precision);
            Assert.IsTrue(relationType.RoleType.ExistScale);
            Assert.AreEqual(2, relationType.RoleType.Scale);

            relationType.RoleType.ObjectType = this.Population.IntegerType;

            Assert.IsFalse(relationType.RoleType.ExistPrecision);
            Assert.IsFalse(relationType.RoleType.ExistScale);
        }
        

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            this.metaObjectChangedEvents.Add(args);
        }
    }

    public class RoleTestWithSuperDomains : RoleTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}