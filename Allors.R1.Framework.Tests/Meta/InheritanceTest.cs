//------------------------------------------------------------------------------------------------- 
// <copyright file="InheritanceTest.cs" company="Allors bvba">
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
// <summary>Defines the InheritanceTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class InheritanceTest : AbstractTest
    {
        [Test]
        public void CycleDifferentType()
        {
            this.Populate();
            this.RemoveInheritances();

            ObjectType[] ones = { this.Population.C1, this.Population.A1, this.Population.I1 };
            ObjectType[] twos = { this.Population.C2, this.Population.A2, this.Population.I2 };

            foreach (var one in ones)
            {
                foreach (var two in twos)
                {
                    this.CycleDifferentTypeCheck(one, two);
                }
            }
        }

        [Test]
        public void CycleSameType()
        {
            this.Populate();
            this.RemoveInheritances();

            var c1 = this.Population.C1;
            var a1 = this.Population.A1;
            var i1 = this.Population.I1;

            var i2 = this.Population.I2;

            this.CycleSameTypeSet(c1);
            this.CycleSameTypeSet(a1);
            this.CycleSameTypeSet(i1);

            this.CycleSameTypeReset(c1, i2);
            this.CycleSameTypeReset(a1, i2);
            this.CycleSameTypeReset(i1, i2);
        }

        [Test]
        public void Validate()
        {
            this.Populate();
            this.RemoveInheritances();

            // Concrete supertype
            var c1_c2_inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            c1_c2_inheritance.Subtype = this.Population.C1;
            c1_c2_inheritance.Supertype = this.Population.C2;

            var validationReport = this.Domain.Validate();
            Assert.AreEqual(1, validationReport.Errors.Length);

            c1_c2_inheritance.Delete();

            // interface with abstract superclass
            var i1_a1_inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            i1_a1_inheritance.Subtype = this.Population.I1;
            i1_a1_inheritance.Supertype = this.Population.A1;

            validationReport = this.Domain.Validate();
            Assert.AreEqual(1, validationReport.Errors.Length);

            i1_a1_inheritance.Delete();

            // Cyclic
            var c1_a1_inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            c1_a1_inheritance.Subtype = this.Population.C1;
            c1_a1_inheritance.Supertype = this.Population.A1;
            var a1_c1_inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            a1_c1_inheritance.Subtype = this.Population.C1;
            a1_c1_inheritance.Supertype = this.Population.A1;

            validationReport = this.Domain.Validate();
            Assert.AreEqual(2, validationReport.Errors.Length);

            c1_a1_inheritance.Delete();
            a1_c1_inheritance.Delete();
        }

        private void CycleDifferentTypeCheck(ObjectType type1, ObjectType type2)
        {
            var inheritance1 = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance1.Subtype = type1;
            inheritance1.Supertype = type2;
            var inheritance2 = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance2.Subtype = type2;
            inheritance2.Supertype = type1;

            Assert.AreEqual(1, type1.DirectSupertypes.Length);
            Assert.AreEqual(1, type1.DirectSubtypes.Length);
            Assert.AreEqual(1, type2.DirectSupertypes.Length);
            Assert.AreEqual(1, type2.DirectSubtypes.Length);

            inheritance1.Delete();
            inheritance2.Delete();
        }

        private void CycleSameTypeReset(ObjectType type1, ObjectType type2)
        {
            var inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance.Subtype = type1;
            inheritance.Supertype = type2;
            try
            {
                inheritance.Supertype = type1;
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(1, this.Population.Inheritances.Length);
                Assert.AreEqual(1, type1.DirectSupertypes.Length);
                Assert.AreEqual(1, type2.DirectSubtypes.Length);
            }

            inheritance.Delete();

            inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            inheritance.Subtype = type1;
            inheritance.Supertype = type2;
            try
            {
                inheritance.Subtype = type1;
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(1, this.Population.Inheritances.Length);
                Assert.AreEqual(1, type1.DirectSupertypes.Length);
                Assert.AreEqual(1, type2.DirectSubtypes.Length);
            }

            inheritance.Delete();
        }

        private void CycleSameTypeSet(ObjectType type)
        {
            var inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            try
            {
                inheritance.Subtype = type;
                inheritance.Supertype = type;
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(1, this.Population.Inheritances.Length);
                Assert.AreEqual(0, type.DirectSupertypes.Length);
                Assert.AreEqual(0, type.DirectSubtypes.Length);
            }

            inheritance.Delete();

            inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
            try
            {
                inheritance.Supertype = type;
                inheritance.Subtype = type;
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(1, this.Population.Inheritances.Length);
                Assert.AreEqual(0, type.DirectSupertypes.Length);
                Assert.AreEqual(0, type.DirectSubtypes.Length);
            }

            inheritance.Delete();
        }
    }

    public class InheritanceTestWithSuperDomains : InheritanceTest
    {
        protected override void Populate()
        {
            this.Population.PopulateWithSuperDomains();
        }
    }
}