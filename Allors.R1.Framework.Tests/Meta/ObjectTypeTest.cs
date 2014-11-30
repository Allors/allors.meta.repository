// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeTest.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Allors.R1.Meta.AllorsGenerated;
    using Allors.R1.Meta.Events;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectTypeTest : AbstractTest
    {
        private readonly List<MetaObjectChangedEventArgs> metaObjectChangedEvents = new List<MetaObjectChangedEventArgs>();

        [Test]
        public void AssociationCountGreaterThan32()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;

            var count = 0;
            RelationType c1_c2;
            for (; count < 31; count++)
            {
                c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                c1_c2.AssociationType.ObjectType = c1;
                c1_c2.RoleType.ObjectType = c2;
                c1_c2.RoleType.AssignedSingularName = count.ToString(CultureInfo.InvariantCulture);
            }

            Assert.AreEqual(31, c2.AssociationTypes.Length);
            Assert.IsFalse(c2.AssociationTypesCountGreaterThan32);

            c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;
            c1_c2.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(32, c2.AssociationTypes.Length);
            Assert.IsFalse(c2.AssociationTypesCountGreaterThan32);

            c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;
            c1_c2.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(33, c2.AssociationTypes.Length);
            Assert.IsTrue(c2.AssociationTypesCountGreaterThan32);

            c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;
            c1_c2.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(34, c2.AssociationTypes.Length);
            Assert.IsTrue(c2.AssociationTypesCountGreaterThan32);
        }

        [Test]
        public void Associations()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c2.AssociationTypes.Length);

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c2.AssociationTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(2, c2.AssociationTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(3, c2.AssociationTypes.Length);

            RemoveDirectSupertypes(a2);

            Assert.AreEqual(2, c2.AssociationTypes.Length);

            a2.AddDirectSupertype(i2);

            Assert.AreEqual(3, c2.AssociationTypes.Length);

            a2.FindInheritanceWhereDirectSubtype(i2).Delete();

            Assert.AreEqual(2, c2.AssociationTypes.Length);

            a2.AddDirectSupertype(i2);
            i1_i2.Delete();

            Assert.AreEqual(2, c2.AssociationTypes.Length);

            a1_a2.Delete();

            Assert.AreEqual(1, c2.AssociationTypes.Length);

            c1_c2.RoleType.ObjectType = c1;

            Assert.AreEqual(0, c2.AssociationTypes.Length);
        }

        [Test]
        public void ChangedEvent()
        {
            this.Populate();

            var thisType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "ThisTypes";

            Assert.IsTrue(this.Domain.IsValid);

            this.Domain.MetaObjectChanged += this.DomainMetaObjectChanged;

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);

            thisType.SendChangedEvent();

            Assert.AreEqual(1, this.metaObjectChangedEvents.Count);
            var args = this.metaObjectChangedEvents[0];
            Assert.AreEqual(thisType, args.MetaObject);

            this.metaObjectChangedEvents.Clear();

            thisType.SingularName = "NoEvent";

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);
        }

        [Test]
        public void CompositeRoleCountGreaterThan32()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;

            var count = 0;
            RelationType c1_c22;
            for (; count < 31; count++)
            {
                c1_c22 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                c1_c22.AssociationType.ObjectType = c1;
                c1_c22.RoleType.ObjectType = c2;
                c1_c22.RoleType.AssignedSingularName = count.ToString(CultureInfo.InvariantCulture);
            }

            Assert.AreEqual(31, c1.CompositeRoleTypes.Length);
            Assert.IsFalse(c1.CompositeRoleTypeCountGreaterThan32);

            c1_c22 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c22.AssociationType.ObjectType = c1;
            c1_c22.RoleType.ObjectType = c2;
            c1_c22.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(32, c1.CompositeRoleTypes.Length);
            Assert.IsFalse(c1.CompositeRoleTypeCountGreaterThan32);

            c1_c22 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c22.AssociationType.ObjectType = c1;
            c1_c22.RoleType.ObjectType = c2;
            c1_c22.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(33, c1.CompositeRoleTypes.Length);
            Assert.IsTrue(c1.CompositeRoleTypeCountGreaterThan32);

            c1_c22 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c22.AssociationType.ObjectType = c1;
            c1_c22.RoleType.ObjectType = c2;
            c1_c22.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(34, c1.CompositeRoleTypes.Length);
            Assert.IsTrue(c1.CompositeRoleTypeCountGreaterThan32);
        }

        [Test]
        public void CompositeRoles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            var allorsString = this.Population.IntegerType;

            Assert.AreEqual(0, c1.CompositeRoleTypes.Length);

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c1.CompositeRoleTypes.Length);

            var c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_string.AssociationType.ObjectType = c1;
            c1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(1, c1.CompositeRoleTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(2, c1.CompositeRoleTypes.Length);

            var a1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_string.AssociationType.ObjectType = a1;
            a1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(2, c1.CompositeRoleTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            var i1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_string.AssociationType.ObjectType = i1;
            i1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(2, c1.CompositeRoleTypes.Length);

            a1.AddDirectSupertype(i1);

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            a1.FindInheritanceWhereDirectSubtype(i1).Delete();

            Assert.AreEqual(2, c1.CompositeRoleTypes.Length);

            a1.AddDirectSupertype(i1);

            i1_string.Delete();

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            a1_string.Delete();

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            c1_string.Delete();

            Assert.AreEqual(3, c1.CompositeRoleTypes.Length);

            i1_i2.Delete();

            Assert.AreEqual(2, c1.CompositeRoleTypes.Length);

            a1_a2.Delete();

            Assert.AreEqual(1, c1.CompositeRoleTypes.Length);

            c1_c2.Delete();

            Assert.AreEqual(0, c1.CompositeRoleTypes.Length);
        }

        [Test]
        public void ContainsAssociation()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var c3 = this.Population.C3;
            var c4 = this.Population.C4;

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.IsFalse(c1.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsTrue(c2.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c3.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c4.ContainsAssociationType(c1_c2.AssociationType));

            var c1_c3 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c3.AssociationType.ObjectType = c1;
            c1_c3.RoleType.ObjectType = c3;

            Assert.IsFalse(c1.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsTrue(c2.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c3.ContainsAssociationType(c1_c2.AssociationType));
            Assert.IsFalse(c4.ContainsAssociationType(c1_c2.AssociationType));

            Assert.IsFalse(c1.ContainsAssociationType(c1_c3.AssociationType));
            Assert.IsFalse(c2.ContainsAssociationType(c1_c3.AssociationType));
            Assert.IsTrue(c3.ContainsAssociationType(c1_c3.AssociationType));
            Assert.IsFalse(c4.ContainsAssociationType(c1_c3.AssociationType));
        }

        [Test]
        public void ContainsRole()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var c3 = this.Population.C3;
            var c4 = this.Population.C4;

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.IsTrue(c1.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c2.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c3.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c4.ContainsRoleType(c1_c2.RoleType));

            var c1_c3 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c3.AssociationType.ObjectType = c1;
            c1_c3.RoleType.ObjectType = c3;

            Assert.IsTrue(c1.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c2.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c3.ContainsRoleType(c1_c2.RoleType));
            Assert.IsFalse(c4.ContainsRoleType(c1_c2.RoleType));

            Assert.IsTrue(c1.ContainsRoleType(c1_c3.RoleType));
            Assert.IsFalse(c2.ContainsRoleType(c1_c3.RoleType));
            Assert.IsFalse(c3.ContainsRoleType(c1_c3.RoleType));
            Assert.IsFalse(c4.ContainsRoleType(c1_c3.RoleType));
        }

        [Test]
        public void Defaults()
        {
            this.Populate();

            var thisType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "TheseTypes";
            thisType.IsAbstract = !thisType.IsAbstract;

            var thatType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thatType.SingularName = "ThatType";
            thatType.PluralName = "ThatTypes";

            var supertype = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            supertype.SingularName = "Supertype";
            supertype.PluralName = "Supertypes";
            supertype.IsInterface = true;

            thisType.AddDirectSupertype(supertype);

            var relationTypeWhereAssociation = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereAssociation.AssociationType.ObjectType = thisType;
            relationTypeWhereAssociation.RoleType.ObjectType = thatType;

            var relationTypeWhereRole = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereRole.AssociationType.ObjectType = thatType;
            relationTypeWhereRole.AssociationType.AssignedSingularName = "from";
            relationTypeWhereRole.AssociationType.AssignedPluralName = "froms";
            relationTypeWhereRole.RoleType.ObjectType = thisType;
            relationTypeWhereRole.RoleType.AssignedSingularName = "to";
            relationTypeWhereRole.RoleType.AssignedPluralName = "tos";

            Assert.IsTrue(this.Domain.IsValid);

            Assert.IsFalse(thisType.IsAssignedSingularNameDefault);
            Assert.IsFalse(thisType.IsAssignedPluralNameDefault);
            Assert.IsFalse(thisType.IsIsAbstractDefault);

            // Assert.IsFalse(thisType.IsDefaultIsInterface);
            // Assert.IsFalse(thisType.IsDefaultIsMultiple);
            // Assert.IsFalse(thisType.IsDefaultIsUnit);
            // Assert.IsFalse(thisType.IsDefaultUnitTag);
            thisType.Reset();

            Assert.IsTrue(thisType.IsAssignedSingularNameDefault);
            Assert.IsTrue(thisType.IsAssignedPluralNameDefault);
            Assert.IsTrue(thisType.IsIsAbstractDefault);
            Assert.IsTrue(thisType.IsIsInterfaceDefault);
            Assert.IsTrue(thisType.IsIsUnitDefault);
            Assert.IsTrue(thisType.IsUnitTagDefault);
        }

        [Test]
        public void Delete()
        {
            this.Populate();

            var thisType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "ThisTypes";

            var thatType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thatType.SingularName = "ThatType";
            thatType.PluralName = "ThatTypes";

            var superType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superType.SingularName = "Supertype";
            superType.PluralName = "Supertypes";
            superType.IsInterface = true;

            thisType.AddDirectSupertype(superType);

            var relationTypeWhereAssociation = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereAssociation.AssociationType.ObjectType = thisType;
            relationTypeWhereAssociation.RoleType.ObjectType = thatType;

            var relationTypeWhereRole = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereRole.AssociationType.ObjectType = thatType;
            relationTypeWhereRole.AssociationType.AssignedSingularName = "from";
            relationTypeWhereRole.AssociationType.AssignedPluralName = "froms";
            relationTypeWhereRole.RoleType.ObjectType = thisType;
            relationTypeWhereRole.RoleType.AssignedSingularName = "to";
            relationTypeWhereRole.RoleType.AssignedPluralName = "tos";

            Assert.IsTrue(this.Domain.IsValid);

            thisType.Delete();

            Assert.IsFalse(relationTypeWhereAssociation.IsDeleted);
            Assert.IsFalse(relationTypeWhereRole.IsDeleted);

            Assert.IsFalse(thatType.IsDeleted);
            Assert.IsFalse(superType.IsDeleted);
        }

        [Test]
        public void DirectSuperclasses()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;

            Assert.AreEqual(a1, c1.DirectSuperclass);
            Assert.IsTrue(c1.ExistDirectSuperclass);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(null, c1.DirectSuperclass);
            Assert.IsFalse(c1.ExistDirectSuperclass);

            c1.AddDirectSupertype(a1);

            Assert.AreEqual(a1, c1.DirectSuperclass);
            Assert.IsTrue(c1.ExistDirectSuperclass);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(null, c1.DirectSuperclass);
            Assert.IsFalse(c1.ExistDirectSuperclass);

            c1.AddDirectSupertype(a1);

            Assert.AreEqual(1, c1.DirectSupertypes.Length);

            c1.AddDirectSupertype(a2);

            Assert.AreEqual(1, c1.DirectSupertypes.Length);
            Assert.AreEqual(a2, c1.DirectSuperclass);
            Assert.IsTrue(c1.ExistDirectSuperclass);
        }

        [Test]
        public void DirectSuperinterfaces()
        {
            this.Populate();

            // Class
            var c1 = this.Population.C1;

            c1.AddDirectSupertype(this.Population.I1);

            Assert.AreEqual(1, c1.DirectSuperinterfaces.Length);

            c1.AddDirectSupertype(this.Population.I2);

            Assert.AreEqual(2, c1.DirectSuperinterfaces.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.DirectSuperinterfaces.Length);

            c1.AddDirectSupertype(this.Population.I1);

            Assert.AreEqual(1, c1.DirectSuperinterfaces.Length);

            c1.AddDirectSupertype(this.Population.I2);
            c1.FindInheritanceWhereDirectSubtype(this.Population.I1).Delete();

            Assert.AreEqual(1, c1.DirectSuperinterfaces.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.DirectSuperinterfaces.Length);
        }

        [Test]
        public void DirectSupertypes()
        {
            this.Populate();

            // Class
            var c1 = this.Population.C1;

            Assert.AreEqual(1, c1.DirectSupertypes.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.DirectSupertypes.Length);
        }

        [Test]
        public void ExclusiveAssociations()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;
            var i3 = this.Population.I3;

            // c1 -> a1 -> i1
            // -> i2 -> i3
            c1.AddDirectSupertype(i2);
            i2.AddDirectSupertype(i3);

            Assert.AreEqual(0, c2.ExclusiveAssociationTypes.Length);

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c2.ExclusiveAssociationTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(1, c2.ExclusiveAssociationTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(1, c2.ExclusiveAssociationTypes.Length);

            // TODO: see Exclusive RoleTypes
        }

        [Test]
        public void ExclusiveConcreteSubclass()
        {
            var c1 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);

            var a1 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            a1.IsAbstract = true;
            c1.AddDirectSupertype(a1);

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, a1.ExclusiveConcreteSubclass);

            var i1 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            i1.IsInterface = true;
            a1.AddDirectSupertype(i1);

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, a1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, i1.ExclusiveConcreteSubclass);

            var a2 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            a2.IsAbstract = true;

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, a1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, i1.ExclusiveConcreteSubclass);

            Assert.AreEqual(null, a2.ExclusiveConcreteSubclass);

            var c2 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            c2.AddDirectSupertype(a2);

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, a1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, i1.ExclusiveConcreteSubclass);

            Assert.AreEqual(c2, c2.ExclusiveConcreteSubclass);
            Assert.AreEqual(c2, a2.ExclusiveConcreteSubclass);

            var i12 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            i12.IsInterface = true;

            a1.AddDirectSupertype(i12);
            a2.AddDirectSupertype(i12);

            Assert.AreEqual(c1, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, a1.ExclusiveConcreteSubclass);
            Assert.AreEqual(c1, i1.ExclusiveConcreteSubclass);

            Assert.AreEqual(c2, c2.ExclusiveConcreteSubclass);
            Assert.AreEqual(c2, a2.ExclusiveConcreteSubclass);

            Assert.AreEqual(null, i12.ExclusiveConcreteSubclass);

            var c1X = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            c1X.AddDirectSupertype(a1);

            Assert.AreEqual(null, c1.ExclusiveConcreteSubclass);
            Assert.AreEqual(null, a1.ExclusiveConcreteSubclass);
            Assert.AreEqual(null, i1.ExclusiveConcreteSubclass);

            Assert.AreEqual(null, c1X.ExclusiveConcreteSubclass);

            Assert.AreEqual(c2, c2.ExclusiveConcreteSubclass);
            Assert.AreEqual(c2, a2.ExclusiveConcreteSubclass);

            Assert.AreEqual(null, i12.ExclusiveConcreteSubclass);
        }

        [Test]
        public void ExclusiveInterfaces()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            var i1_i2_array = new ObjectType[2];
            i1_i2_array[0] = i1;
            i1_i2_array[1] = i2;

            Assert.AreEqual(0, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(2, a1.ExclusiveSuperinterfaces.Length);

            RemoveDirectSupertypes(c1);
            c1.AddDirectSupertype(i1);
            c1.AddDirectSupertype(i2);

            Assert.AreEqual(2, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(2, a1.ExclusiveSuperinterfaces.Length);

            RemoveDirectSupertypes(c1);
            c1.AddDirectSupertype(a1);

            Assert.AreEqual(0, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(2, a1.ExclusiveSuperinterfaces.Length);

            c1.AddDirectSupertype(i2);

            Assert.AreEqual(1, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(2, a1.ExclusiveSuperinterfaces.Length);

            a1.AddDirectSupertype(i2);

            Assert.AreEqual(0, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(3, a1.ExclusiveSuperinterfaces.Length);

            a1.FindInheritanceWhereDirectSubtype(i1).Delete();

            Assert.AreEqual(0, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(2, a1.ExclusiveSuperinterfaces.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(1, c1.ExclusiveSuperinterfaces.Length);
            Assert.AreEqual(0, a1.ExclusiveSuperinterfaces.Length);
        }

        [Test]
        public void ExclusiveRoles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;
            var i3 = this.Population.I3;
            var i4 = this.Population.I4;

            // c1 -> a1 -> i1
            // -> i2 -> i3
            c1.AddDirectSupertype(i2);
            i2.AddDirectSupertype(i3);

            Assert.AreEqual(0, c1.ExclusiveRoleTypes.Length);

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c1.ExclusiveRoleTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(1, c1.ExclusiveRoleTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(1, c1.ExclusiveRoleTypes.Length);

            var i2_i3 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i2_i3.AssociationType.ObjectType = i2;
            i2_i3.RoleType.ObjectType = i3;

            Assert.AreEqual(2, c1.ExclusiveRoleTypes.Length);

            var i3_i4 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i3_i4.AssociationType.ObjectType = i3;
            i3_i4.RoleType.ObjectType = i4;

            Assert.AreEqual(3, c1.ExclusiveRoleTypes.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(1, c1.ExclusiveRoleTypes.Length);

            c1.AddDirectSupertype(i2);

            Assert.AreEqual(3, c1.ExclusiveRoleTypes.Length);

            i2.FindInheritanceWhereDirectSubtype(i3).Delete();

            Assert.AreEqual(2, c1.ExclusiveRoleTypes.Length);

            i2.AddDirectSupertype(i3);
            i3_i4.Delete();

            Assert.AreEqual(2, c1.ExclusiveRoleTypes.Length);

            i2_i3.AssociationType.ObjectType = c2;

            Assert.AreEqual(1, c1.ExclusiveRoleTypes.Length);

            c1_c2.AssociationType.ObjectType = c2;

            Assert.AreEqual(0, c1.ExclusiveRoleTypes.Length);
        }

        [Test]
        public void ExclusiveRootClass()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            var a2 = this.Population.A2;

            Assert.AreEqual(a1, c1.ExclusiveRootClass);
            Assert.AreEqual(a1, a1.ExclusiveRootClass);
            Assert.AreEqual(a1, i1.ExclusiveRootClass);

            a1.AddDirectSupertype(a2);

            Assert.AreEqual(a2, c1.ExclusiveRootClass);
            Assert.AreEqual(a2, a1.ExclusiveRootClass);
            Assert.AreEqual(a2, a2.ExclusiveRootClass);
            Assert.AreEqual(a2, i1.ExclusiveRootClass);

            var i12 = this.Population.I12;
            a1.FindInheritanceWhereDirectSubtype(a2).Delete();

            Assert.AreEqual(null, i12.ExclusiveRootClass);

            var i = c1.DomainWhereDeclaredObjectType.AddDeclaredObjectType(Guid.NewGuid());
            i.IsInterface = true;

            Assert.AreEqual(null, i.ExclusiveRootClass);

            c1.AddDirectSupertype(i);

            Assert.AreEqual(a1, i.ExclusiveRootClass);
        }

        [Test]
        public void Id()
        {
            this.Populate();

            var thisType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "ThisTypes";

            Assert.IsTrue(this.Domain.IsValid);

            foreach (var objectType in this.Domain.CompositeObjectTypes)
            {
                Assert.IsTrue(objectType.ExistId);
            }

            var exceptionThrown = false;
            try
            {
                thisType.Id = Guid.NewGuid();
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void Inheritance()
        {
            this.Populate();

            foreach (var interfacesToDelete in this.Domain.Inheritances)
            {
                interfacesToDelete.Delete();
            }

            var originalInheritance = this.Population.C1.AddDirectSupertype(this.Population.A1);
            var inheritance = this.Population.C1.AddDirectSupertype(this.Population.A2);

            Assert.AreEqual(1, this.Domain.Inheritances.Length);
            Assert.IsTrue(originalInheritance.IsDeleted);
            Assert.IsFalse(inheritance.IsDeleted);

            inheritance.Subtype = this.Population.C1;

            Assert.AreEqual(1, this.Domain.Inheritances.Length);
            Assert.IsTrue(originalInheritance.IsDeleted);
            Assert.IsFalse(inheritance.IsDeleted);

            inheritance.Supertype = this.Population.A1;

            Assert.AreEqual(1, this.Domain.Inheritances.Length);
            Assert.IsTrue(originalInheritance.IsDeleted);
            Assert.IsFalse(inheritance.IsDeleted);
        }

        [Test]
        public void IsClass()
        {
            this.Populate();

            foreach (var objectType in this.Domain.CompositeObjectTypes)
            {
                Assert.AreEqual(objectType.IsClass, !objectType.IsInterface);
            }

            foreach (var objectType in this.Population.Classes)
            {
                Assert.IsTrue(objectType.IsClass);
            }

            foreach (var objectType in this.Population.Interfaces)
            {
                Assert.IsFalse(objectType.IsClass);
            }
        }

        [Test]
        public void IsComposite()
        {
            this.Populate();

            foreach (var objectType in this.Domain.CompositeObjectTypes)
            {
                Assert.AreEqual(objectType.IsUnit, !objectType.IsComposite);
            }

            foreach (var objectType in this.Population.Composites)
            {
                Assert.IsTrue(objectType.IsComposite);
            }

            foreach (var objectType in this.Population.UnitTypes)
            {
                Assert.IsFalse(objectType.IsComposite);
            }
        }

        [Test]
        public void IsConcrete()
        {
            this.Populate();

            foreach (var objectType in this.Population.UnitTypes)
            {
                Assert.IsTrue(objectType.IsConcrete);
            }

            foreach (var objectType in this.Population.CompositeConcreteClasses)
            {
                Assert.IsTrue(objectType.IsConcrete);
            }

            foreach (var objectType in this.Population.CompositeAbstractClasses)
            {
                Assert.IsFalse(objectType.IsConcrete);
            }

            foreach (var objectType in this.Population.Interfaces)
            {
                Assert.IsFalse(objectType.IsConcrete);
            }
        }

        [Test]
        public void IsConcreteComposite()
        {
            this.Populate();

            foreach (var objectType in this.Population.CompositeConcreteClasses)
            {
                Assert.IsTrue(objectType.IsConcreteComposite);
            }

            foreach (var objectType in this.Population.UnitTypes)
            {
                Assert.IsFalse(objectType.IsConcreteComposite);
            }

            foreach (var objectType in this.Population.CompositeAbstractClasses)
            {
                Assert.IsFalse(objectType.IsConcreteComposite);
            }

            foreach (var objectType in this.Population.Interfaces)
            {
                Assert.IsFalse(objectType.IsConcreteComposite);
            }
        }

        [Test]
        public void IsInterface()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            Assert.AreEqual(1, c1.Superclasses.Length);
            Assert.AreEqual(2, i1.Subclasses.Length);

            a1.IsInterface = true;

            Assert.AreEqual(0, c1.Superclasses.Length);
            Assert.AreEqual(1, i1.Subclasses.Length);
        }

        [Test]
        public void IsRootClass()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            Assert.IsFalse(c1.IsRootClass);
            Assert.IsTrue(a1.IsRootClass);
            Assert.IsFalse(i1.IsRootClass);

            var a2 = this.Population.A2;
            Assert.IsTrue(a2.IsRootClass);

            a1.AddDirectSupertype(a2);

            Assert.IsFalse(c1.IsRootClass);
            Assert.IsFalse(a1.IsRootClass);
            Assert.IsFalse(i1.IsRootClass);

            Assert.IsTrue(a2.IsRootClass);
        }

        [Test]
        public void IsScaleable()
        {
            this.Populate();

            foreach (var objectType in this.Domain.CompositeObjectTypes)
            {
                if (objectType.IsDecimal)
                {
                    Assert.IsTrue(objectType.IsScaleRequired);
                }
                else
                {
                    Assert.IsFalse(objectType.IsScaleRequired);
                }
            }
        }

        [Test]
        public void IsSizeable()
        {
            this.Populate();

            foreach (var objectType in this.Domain.CompositeObjectTypes)
            {
                if (objectType.IsString || objectType.IsDecimal)
                {
                    Assert.IsTrue(objectType.IsSizeRequired);
                }
                else
                {
                    Assert.IsFalse(objectType.IsSizeRequired);
                }
            }
        }

        [Test]
        public void Reset()
        {
            this.Populate();

            var thisType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "TheseTypes";
            thisType.IsAbstract = !thisType.IsAbstract;
            thisType.IsAbstract = !thisType.IsAbstract;
            thisType.IsInterface = !thisType.IsInterface;
            thisType.IsUnit = !thisType.IsUnit;
            thisType.UnitTag = 1001;

            var thatType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            thatType.SingularName = "ThatType";
            thatType.PluralName = "ThatTypes";

            var superType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            superType.SingularName = "Supertype";
            superType.PluralName = "Supertypes";
            superType.IsInterface = true;

            thisType.AddDirectSupertype(superType);

            var relationTypeWhereAssociation = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereAssociation.AssociationType.ObjectType = thisType;
            relationTypeWhereAssociation.RoleType.ObjectType = thatType;

            var relationTypeWhereRole = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereRole.AssociationType.ObjectType = thatType;
            relationTypeWhereRole.AssociationType.AssignedSingularName = "from";
            relationTypeWhereRole.AssociationType.AssignedPluralName = "froms";
            relationTypeWhereRole.RoleType.ObjectType = thisType;
            relationTypeWhereRole.RoleType.AssignedSingularName = "to";
            relationTypeWhereRole.RoleType.AssignedPluralName = "tos";

            Assert.IsTrue(this.Domain.IsValid);

            thisType.Reset();

            Assert.IsFalse(thisType.ExistSingularName);
            Assert.IsFalse(thisType.ExistPluralName);
            Assert.IsFalse(thisType.IsAbstract);
            Assert.IsFalse(thisType.IsInterface);
            Assert.IsFalse(thisType.IsUnit);
            Assert.IsFalse(thisType.ExistUnitTag);

            Assert.IsFalse(relationTypeWhereAssociation.IsDeleted);
            Assert.IsFalse(relationTypeWhereRole.IsDeleted);
        }

        [Test]
        public void Methods()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c1.MethodTypes.Length);

            var methodC1 = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodC1.ObjectType = this.Population.C1;
            methodC1.Name = "MethodC1";

            Assert.AreEqual(1, c1.MethodTypes.Length);

            var methodA1 = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodA1.ObjectType = this.Population.A1;
            methodA1.Name = "MethodA1";

            Assert.AreEqual(2, c1.MethodTypes.Length);

            var methodI1 = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodI1.ObjectType = this.Population.I1;
            methodI1.Name = "MethodI1";

            Assert.AreEqual(3, c1.MethodTypes.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(2, c1.MethodTypes.Length);

            a1.AddDirectSupertype(i1);

            Assert.AreEqual(3, c1.MethodTypes.Length);

            a1.FindInheritanceWhereDirectSubtype(i1).Delete();

            Assert.AreEqual(2, c1.MethodTypes.Length);

            a1.AddDirectSupertype(i1);
            methodI1.Delete();

            Assert.AreEqual(2, c1.MethodTypes.Length);

            methodA1.ObjectType.Delete();

            Assert.AreEqual(1, c1.MethodTypes.Length);

            methodC1.ObjectType = c2;

            Assert.AreEqual(0, c1.MethodTypes.Length);
        }

        [Test]
        public void Roles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            Assert.AreEqual(0, c1.RoleTypes.Length);

            var c1_c2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c2.AssociationType.ObjectType = c1;
            c1_c2.RoleType.ObjectType = c2;

            Assert.AreEqual(1, c1.RoleTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(2, c1.RoleTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(3, c1.RoleTypes.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(2, c1.RoleTypes.Length);

            a1.AddDirectSupertype(i1);

            Assert.AreEqual(3, c1.RoleTypes.Length);

            a1.FindInheritanceWhereDirectSubtype(i1).Delete();

            Assert.AreEqual(2, c1.RoleTypes.Length);

            a1.AddDirectSupertype(i1);
            i1_i2.Delete();

            Assert.AreEqual(2, c1.RoleTypes.Length);

            a1_a2.AssociationType.ObjectType.Delete();

            Assert.AreEqual(1, c1.RoleTypes.Length);

            c1_c2.AssociationType.ObjectType = c2;

            Assert.AreEqual(0, c1.RoleTypes.Length);
        }

        [Test]
        public void RootClasses()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            var a2 = this.Population.A2;

            Assert.AreEqual(1, c1.RootClasses.Length);
            Assert.AreEqual(a1, c1.RootClasses[0]);
            Assert.AreEqual(1, a1.RootClasses.Length);
            Assert.AreEqual(a1, a1.RootClasses[0]);
            Assert.AreEqual(1, i1.RootClasses.Length);
            Assert.AreEqual(a1, i1.RootClasses[0]);

            a1.AddDirectSupertype(a2);

            Assert.AreEqual(1, c1.RootClasses.Length);
            Assert.AreEqual(a2, c1.RootClasses[0]);
            Assert.AreEqual(1, a1.RootClasses.Length);
            Assert.AreEqual(a2, a1.RootClasses[0]);
            Assert.AreEqual(1, a2.RootClasses.Length);
            Assert.AreEqual(a2, a2.RootClasses[0]);
            Assert.AreEqual(1, i1.RootClasses.Length);
            Assert.AreEqual(a2, i1.RootClasses[0]);

            var i12 = this.Population.I12;
            a1.FindInheritanceWhereDirectSubtype(a2).Delete();

            Assert.AreEqual(1, c1.RootClasses.Length);
            Assert.AreEqual(1, a1.RootClasses.Length);
            Assert.AreEqual(1, i1.RootClasses.Length);
            Assert.AreEqual(2, i12.RootClasses.Length);

            var i = c1.DomainWhereDeclaredObjectType.AddDeclaredObjectType(Guid.NewGuid());
            i.IsInterface = true;

            Assert.AreEqual(0, i.RootClasses.Length);

            c1.AddDirectSupertype(i);

            Assert.AreEqual(1, i.RootClasses.Length);
        }

        [Test]
        public void SpecificUnitRoles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var c2 = this.Population.C2;

            Assert.AreEqual(0, c1.StringRoleTypes.Length);
            Assert.AreEqual(0, c2.StringRoleTypes.Length);

            var c1_x_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_x_string.AssociationType.ObjectType = c1;
            c1_x_string.RoleType.ObjectType = this.Population.StringType;
            c1_x_string.RoleType.Size = 100;
            c1_x_string.RoleType.AssignedSingularName = "x";

            Assert.AreEqual(1, c1.StringRoleTypes.Length);
            Assert.AreEqual(0, c2.StringRoleTypes.Length);

            var c2_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c2_string.AssociationType.ObjectType = c2;
            c2_string.RoleType.ObjectType = this.Population.StringType;
            c2_string.RoleType.Size = 50;

            Assert.AreEqual(1, c1.StringRoleTypes.Length);
            Assert.AreEqual(1, c2.StringRoleTypes.Length);

            var c1_y_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_y_string.AssociationType.ObjectType = c1;
            c1_y_string.RoleType.ObjectType = this.Population.StringType;
            c1_y_string.RoleType.Size = 200;
            c1_y_string.RoleType.AssignedSingularName = "y";

            Assert.AreEqual(2, c1.StringRoleTypes.Length);
            Assert.AreEqual(1, c2.StringRoleTypes.Length);
        }

        [Test]
        public void SubClasses()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            Assert.AreEqual(2, i1.Subclasses.Length);
            Assert.AreEqual(1, a1.Subclasses.Length);
            Assert.AreEqual(0, c1.Subclasses.Length);

            c1.Delete();

            Assert.AreEqual(1, i1.Subclasses.Length);
            Assert.AreEqual(0, a1.Subclasses.Length);
        }

        [Test]
        public void SubTypes()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            Assert.AreEqual(2, i1.Subtypes.Length);
            Assert.AreEqual(1, a1.Subclasses.Length);
            Assert.AreEqual(0, c1.Subclasses.Length);
        }

        [Test]
        public void SuperClasses()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;

            Assert.AreEqual(1, c1.Superclasses.Length);

            a1.AddDirectSupertype(this.Population.A2);

            Assert.AreEqual(2, c1.Superclasses.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.Superclasses.Length);

            c1.AddDirectSupertype(this.Population.A1);

            Assert.AreEqual(2, c1.Superclasses.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(1, c1.Superclasses.Length);

            a1.AddDirectSupertype(this.Population.A2);

            Assert.AreEqual(2, c1.Superclasses.Length);

            a1.FindInheritanceWhereDirectSubtype(this.Population.A2).Delete();

            Assert.AreEqual(1, c1.Superclasses.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.Superclasses.Length);
        }

        [Test]
        public void SuperInterfaces()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;

            Assert.AreEqual(2, c1.Superinterfaces.Length);

            c1.AddDirectSupertype(this.Population.I2);

            Assert.AreEqual(3, c1.Superinterfaces.Length);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.Superinterfaces.Length);

            c1.AddDirectSupertype(this.Population.A1);

            Assert.AreEqual(2, c1.Superinterfaces.Length);

            a1.AddDirectSupertype(this.Population.I2);
            a1.FindInheritanceWhereDirectSubtype(this.Population.I1).Delete();

            Assert.AreEqual(2, c1.Superinterfaces.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(0, c1.Superinterfaces.Length);
        }

        [Test]
        public void Supertypes()
        {
            this.Populate();

            var c1 = this.Population.C1;

            Assert.AreEqual(3, c1.Supertypes.Length);
            Assert.Contains(this.Population.A1, c1.Supertypes);
            Assert.Contains(this.Population.I1, c1.Supertypes);
            Assert.Contains(this.Population.I12, c1.Supertypes);

            c1.AddDirectSupertype(this.Population.A2);

            Assert.AreEqual(3, c1.Supertypes.Length);
            Assert.Contains(this.Population.A2, c1.Supertypes);
            Assert.Contains(this.Population.I2, c1.Supertypes);
            Assert.Contains(this.Population.I12, c1.Supertypes);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.Supertypes.Length);

            c1.AddDirectSupertype(this.Population.A1);

            Assert.AreEqual(3, c1.Supertypes.Length);
            Assert.Contains(this.Population.A1, c1.Supertypes);
            Assert.Contains(this.Population.I1, c1.Supertypes);
            Assert.Contains(this.Population.I12, c1.Supertypes);

            RemoveDirectSupertypes(c1);

            Assert.AreEqual(0, c1.Supertypes.Length);

            c1.AddDirectSupertype(this.Population.A1);

            Assert.AreEqual(3, c1.Supertypes.Length);
            Assert.Contains(this.Population.A1, c1.Supertypes);
            Assert.Contains(this.Population.I1, c1.Supertypes);
            Assert.Contains(this.Population.I12, c1.Supertypes);

            c1.AddDirectSupertype(this.Population.A2);

            Assert.AreEqual(3, c1.Supertypes.Length);
            Assert.Contains(this.Population.A2, c1.Supertypes);
            Assert.Contains(this.Population.I2, c1.Supertypes);
            Assert.Contains(this.Population.I12, c1.Supertypes);
        }

        [Test]
        public void UnitRoleCountGreaterThan32()
        {
            this.Populate();

            var c1 = this.Population.C1;

            var allorsString = this.Population.IntegerType;

            var count = 0;
            RelationType c1_string;
            for (; count < 31; count++)
            {
                c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                c1_string.AssociationType.ObjectType = c1;
                c1_string.RoleType.ObjectType = allorsString;
                c1_string.RoleType.AssignedSingularName = count.ToString(CultureInfo.InvariantCulture);
            }

            Assert.AreEqual(31, c1.UnitRoleTypes.Length);
            Assert.IsFalse(c1.UnitRoleTypesCountGreaterThan32);

            c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_string.AssociationType.ObjectType = c1;
            c1_string.RoleType.ObjectType = allorsString;
            c1_string.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(32, c1.UnitRoleTypes.Length);
            Assert.IsFalse(c1.UnitRoleTypesCountGreaterThan32);

            c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_string.AssociationType.ObjectType = c1;
            c1_string.RoleType.ObjectType = allorsString;
            c1_string.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(33, c1.UnitRoleTypes.Length);
            Assert.IsTrue(c1.UnitRoleTypesCountGreaterThan32);

            c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_string.AssociationType.ObjectType = c1;
            c1_string.RoleType.ObjectType = allorsString;
            c1_string.RoleType.AssignedSingularName = (++count).ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(34, c1.UnitRoleTypes.Length);
            Assert.IsTrue(c1.UnitRoleTypesCountGreaterThan32);
        }

        [Test]
        public void UnitRoles()
        {
            this.Populate();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var a2 = this.Population.A2;
            var i1 = this.Population.I1;
            var i2 = this.Population.I2;

            var allorsString = this.Population.IntegerType;

            Assert.AreEqual(0, c1.UnitRoleTypes.Length);

            var c1_c1 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_c1.AssociationType.ObjectType = c1;
            c1_c1.RoleType.ObjectType = c1;
            c1_c1.RoleType.AssignedSingularName = "me";

            Assert.AreEqual(0, c1.UnitRoleTypes.Length);

            var c1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            c1_string.AssociationType.ObjectType = c1;
            c1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(1, c1.UnitRoleTypes.Length);

            var a1_a2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_a2.AssociationType.ObjectType = a1;
            a1_a2.RoleType.ObjectType = a2;

            Assert.AreEqual(1, c1.UnitRoleTypes.Length);

            var a1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            a1_string.AssociationType.ObjectType = a1;
            a1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(2, c1.UnitRoleTypes.Length);

            var i1_i2 = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_i2.AssociationType.ObjectType = i1;
            i1_i2.RoleType.ObjectType = i2;

            Assert.AreEqual(2, c1.UnitRoleTypes.Length);

            var i1_string = this.Domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            i1_string.AssociationType.ObjectType = i1;
            i1_string.RoleType.ObjectType = allorsString;

            Assert.AreEqual(3, c1.UnitRoleTypes.Length);

            RemoveDirectSupertypes(a1);

            Assert.AreEqual(2, c1.UnitRoleTypes.Length);

            a1.AddDirectSupertype(i1);

            Assert.AreEqual(3, c1.UnitRoleTypes.Length);

            a1.FindInheritanceWhereDirectSubtype(i1).Delete();

            Assert.AreEqual(2, c1.UnitRoleTypes.Length);

            a1.AddDirectSupertype(i1);
            i1_i2.Delete();

            Assert.AreEqual(3, c1.UnitRoleTypes.Length);

            a1_a2.Delete();

            Assert.AreEqual(3, c1.UnitRoleTypes.Length);

            c1_c1.Delete();

            Assert.AreEqual(3, c1.UnitRoleTypes.Length);

            i1_string.Delete();

            Assert.AreEqual(2, c1.UnitRoleTypes.Length);

            a1_string.Delete();

            Assert.AreEqual(1, c1.UnitRoleTypes.Length);

            c1_string.Delete();

            Assert.AreEqual(0, c1.UnitRoleTypes.Length);
        }

        [Test]
        public void Units()
        {
            this.Populate();

            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9"))).IsString);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("ccd6f134-26de-4103-bff9-a37ec3e997a3"))).IsInteger);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("e8989069-024b-4389-ac77-a98c4dfff25a"))).IsLong);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("da866d8e-2c40-41a8-ae5b-5f6dae0b89c8"))).IsDecimal);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("ffcabd07-f35f-4083-bef6-f6c47970ca5d"))).IsDouble);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("b5ee6cea-4e2b-498e-a5dd-24671d896477"))).IsBoolean);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("c4c09343-61d3-418c-ade2-fe6fd588f128"))).IsDateTime);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("6DC0A1A8-88A4-4614-ADB4-92DD3D017C0E"))).IsUnique);
            Assert.IsTrue(((ObjectType)this.Domain.Domain.Find(new Guid("c28e515b-cae8-4d6b-95bf-062aec8042fc"))).IsBinary);
        }

        [Test]
        public void Validate()
        {
            var type = this.Domain.AddDeclaredObjectType(Guid.NewGuid());

            var validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            var errors = validationReport.Errors;
            Assert.AreEqual(2, errors.Length);

            foreach (var error in errors)
            {
                Assert.AreEqual(1, error.Members.Length);
                var member = error.Members[0];

                if (member.Equals(AllorsEmbeddedDomain.ObjectTypeSingularName))
                {
                    Assert.AreEqual(ValidationKind.Required, error.Kind);
                }
                else if (member.Equals(AllorsEmbeddedDomain.ObjectTypePluralName))
                {
                    Assert.AreEqual(ValidationKind.Required, error.Kind);
                }
            }

            // SingularName
            type.SingularName = string.Empty;
            type.PluralName = "Plural";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypeSingularName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.MinimumLength, validationReport.Errors[0].Kind);

            type.SingularName = "_a";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypeSingularName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "a_";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypeSingularName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "11";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypeSingularName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.SingularName = "a1";

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            type.SingularName = "aa";

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            // PluralName
            type.SingularName = "SingularName";
            type.PluralName = null;

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypePluralName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Required, validationReport.Errors[0].Kind);

            type.PluralName = "_a";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypePluralName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "a_";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypePluralName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "11";

            validationReport = this.Domain.Validate();
            Assert.IsTrue(validationReport.ContainsErrors);
            Assert.AreEqual(1, validationReport.Errors.Length);
            Assert.AreEqual(type, validationReport.Errors[0].Source);
            Assert.AreEqual(1, validationReport.Errors[0].Members.Length);
            Assert.AreEqual(AllorsEmbeddedDomain.ObjectTypePluralName, validationReport.Errors[0].Members[0]);
            Assert.AreEqual(ValidationKind.Format, validationReport.Errors[0].Kind);

            type.PluralName = "a1";

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);

            type.PluralName = "aa";

            validationReport = this.Domain.Validate();
            Assert.IsFalse(validationReport.ContainsErrors);
            Assert.AreEqual(0, validationReport.Errors.Length);
        }

        [Test]
        public void ValidateDuplicateFullName()
        {
            this.Populate();

            var type = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            type.SingularName = "Type1";
            type.PluralName = "Type1s";

            var anotherType = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            anotherType.SingularName = "Type1";
            anotherType.PluralName = "Type1s";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void ValidateDuplicateName()
        {
            this.Populate();

            var objectType1 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType1.SingularName = "ObjectType1";
            objectType1.PluralName = "ObjectType1s";
            var objectType2 = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            objectType2.SingularName = "ObjectType1";
            objectType2.PluralName = "ObjectType1s";

            Assert.IsFalse(this.Domain.IsValid);
        }

        [Test]
        public void ValidateNameMinimumLength()
        {
            this.Populate();

            var type = this.Domain.AddDeclaredObjectType(Guid.NewGuid());
            type.SingularName = "A";
            type.PluralName = "CD";

            Assert.IsFalse(this.Domain.IsValid);

            type.SingularName = "AB";

            Assert.IsTrue(this.Domain.IsValid);

            type.PluralName = "C";

            Assert.IsFalse(this.Domain.IsValid);

            type.PluralName = "CD";

            Assert.IsTrue(this.Domain.IsValid);
        }

        internal static void RemoveDirectSupertypes(ObjectType subType)
        {
            foreach (var inheritance in subType.InheritancesWhereSubtype)
            {
                inheritance.Delete();
            }
        }

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            this.metaObjectChangedEvents.Add(args);
        }
    }

    public class ObjectTypeTestWithSuperDomains : ObjectTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}