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

    using NUnit.Framework;

    [TestFixture]
    public class MetaObjectTest : AbstractTest
    {
        [Test]
        public void Instantiate()
        {
            var domain = this.Domain;

            var superDomain = domain.AddDirectSuperDomain(Guid.NewGuid());
            superDomain.Name = "SuperDomain";

            var objectType = superDomain.AddDeclaredObjectType(Guid.NewGuid());
            objectType.SingularName = "Singular";
            objectType.PluralName = "Plural";

            Assert.AreEqual(objectType, domain.Domain.Find(objectType.Id));
            Assert.AreEqual(objectType, superDomain.Domain.Find(objectType.Id));
        }

        [Test]
        public void FindByIdAfterExtend()
        {
            var superDomainDomain = Domain.Create();
            superDomainDomain.Name = "SuperDomain";

            var objectTypeId = new Guid("6A10F333-4AD6-4812-AB84-46DB10858DCA");

            var objectType = superDomainDomain.AddDeclaredObjectType(objectTypeId);
            objectType.SingularName = "Singular";
            objectType.PluralName = "Plural";

            var domain = this.Domain;
            var superDomain = domain.Inherit(superDomainDomain);

            Assert.IsNotNull(domain.Domain.Find(objectTypeId));
            Assert.IsNotNull(superDomain.Domain.Find(objectTypeId));
        }
    }

    public class MetaObjectTestWithSuperDomains : MetaObjectTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}