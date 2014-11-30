//------------------------------------------------------------------------------------------------- 
// <copyright file="MethodTypeTest.cs" company="Allors bvba">
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
// <summary>Defines the MethodTypeTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;
    using System.Collections.Generic;

    using Allors.R1.Meta.Events;

    using NUnit.Framework;

    [TestFixture]
    public class MethodTypeTest : AbstractTest
    {
        private readonly List<MetaObjectChangedEventArgs> metaObjectChangedEvents = new List<MetaObjectChangedEventArgs>();

        [Test]
        public void ChangedEvent()
        {
            this.Populate();

            var methodType = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyMethod";

            Assert.IsTrue(this.Domain.IsValid);

            this.Domain.MetaObjectChanged += this.DomainMetaObjectChanged;

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);

            methodType.SendChangedEvent();

            Assert.AreEqual(1, this.metaObjectChangedEvents.Count);

            var args = this.metaObjectChangedEvents[0];
            Assert.AreEqual(methodType, args.MetaObject);
            
            this.metaObjectChangedEvents.Clear();

            methodType.Name = "NoEvents";

            Assert.AreEqual(0, this.metaObjectChangedEvents.Count);
        }
     
        [Test]
        public void Delete()
        {
            this.Populate();

            var methodType = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsTrue(this.Domain.IsValid);

            methodType.Delete();

            Assert.IsFalse(this.Population.C1.IsDeleted);
            Assert.IsTrue(methodType.IsDeleted);
        }

        [Test]
        public void Reset()
        {
            this.Populate();
            var methodType = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsTrue(this.Domain.IsValid);

            methodType.Reset();

            Assert.IsTrue(methodType.IsNameDefault);
            Assert.IsTrue(methodType.IsObjectTypeDefault);

            Assert.IsFalse(methodType.ExistName);
            Assert.IsFalse(methodType.ExistObjectType);
        }

        [Test]
        public void ValidateDuplicateMethod()
        {
            this.Populate();

            var methodType = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsTrue(this.Domain.IsValid);

            var otherMethodType = this.Domain.AddDeclaredMethodType(Guid.NewGuid());
            methodType.ObjectType = this.Population.C1;
            methodType.Name = "MyName";

            Assert.IsFalse(this.Domain.IsValid);
        }

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            this.metaObjectChangedEvents.Add(args);
        }
    }

    public class MethodTypeTestWithSuperDomains : MethodTypeTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}