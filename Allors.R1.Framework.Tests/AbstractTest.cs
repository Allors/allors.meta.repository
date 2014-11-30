//------------------------------------------------------------------------------------------------- 
// <copyright file="AbstractTest.cs" company="Allors bvba">
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
// <summary>Defines the AbstractTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta.Static
{
    using System;

    using NUnit.Framework;

    public abstract class AbstractTest : IDisposable
    {
        protected Domain Domain { get; set; }

        protected Population Population { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.Population = new Population();
            this.Domain = this.Population.Domain;
            Assert.IsTrue(this.Domain.IsValid);
        }

        [TearDown]
        public void Dispose()
        {
            this.Population = null;
            this.Domain = null;
        }

        protected virtual void Populate()
        {
            this.Population.Populate();
        }

        protected void RemoveInheritances()
        {
            foreach (var inheritance in this.Domain.Inheritances)
            {
                inheritance.Delete();
            }
        }
   }
}