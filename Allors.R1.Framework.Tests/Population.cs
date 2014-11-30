// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Population.cs" company="Allors bvba">
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
    using System.Collections;
    using System.Collections.Generic;

    using AllorsGenerated;

    public class Population
    {
        private readonly AllorsEmbeddedSession allorsSession;

        public Population()
        {
            this.Domain = Domain.Create();
            this.Domain.Name = "Domain";

            this.allorsSession = this.Domain.AllorsSession;

            var validationReport = this.Domain.Validate();
            if (validationReport.ContainsErrors)
            {
                throw new Exception("Domain invalid");
            }

            this.GetUnits();
        }

        public ObjectType A1 { get; set; }

        public ObjectType A2 { get; set; }

        public ObjectType A3 { get; set; }

        public ObjectType A34 { get; set; }

        public ObjectType A4 { get; set; }

        public ObjectType BinaryType { get; set; }

        public ObjectType BooleanType { get; set; }

        public ObjectType C1 { get; set; }

        public ObjectType C2 { get; set; }

        public ObjectType C3 { get; set; }

        public ObjectType C4 { get; set; }

        public ObjectType[] Classes { get; set; }

        public ObjectType[] CompositeAbstractClasses { get; set; }

        public ObjectType[] CompositeClasses { get; set; }

        public ObjectType[] CompositeConcreteClasses { get; set; }

        public ObjectType[] CompositeInterfaces { get; set; }

        public ObjectType[] CompositeTypes
        {
            get
            {
                var compositeTypes = new List<ObjectType>();
                foreach (ObjectType type in this.Types)
                {
                    if (type.IsComposite)
                    {
                        compositeTypes.Add(type);
                    }
                }

                return compositeTypes.ToArray();
            }
        }

        public ObjectType[] Composites { get; set; }

        public ObjectType DateTimeType { get; set; }

        public ObjectType DecimalType { get; set; }

        public ObjectType DoubleType { get; set; }

        public ObjectType I1 { get; set; }

        public ObjectType I12 { get; set; }

        public ObjectType I2 { get; set; }

        public ObjectType I3 { get; set; }

        public ObjectType I34 { get; set; }

        public ObjectType I4 { get; set; }

        public ObjectType IntegerType { get; set; }

        public ObjectType[] Interfaces { get; set; }

        public ObjectType LongType { get; set; }

        public ObjectType StringType { get; set; }

        public ObjectType UniqueType { get; set; }

        public ObjectType[] UnitTypes
        {
            get
            {
                var unitTypes = new List<ObjectType>();
                foreach (ObjectType type in this.Types)
                {
                    if (type.IsUnit)
                    {
                        unitTypes.Add(type);
                    }
                }

                return unitTypes.ToArray();
            }
        }

        internal Domain Domain { get; set; }

        internal AllorsEmbeddedObject[] Inheritances
        {
            get
            {
                return this.allorsSession.Extent(AllorsEmbeddedDomain.Inheritance);
            }
        }

        internal AllorsEmbeddedObject[] Relations
        {
            get
            {
                return this.allorsSession.Extent(AllorsEmbeddedDomain.RelationType);
            }
        }

        internal Hashtable RelationsById
        {
            get
            {
                var results = new Hashtable();
                foreach (RelationType relation in this.Relations)
                {
                    results.Add(relation.Id, relation);
                }

                return results;
            }
        }

        internal AllorsEmbeddedObject[] Associations
        {
            get
            {
                return this.allorsSession.Extent(AllorsEmbeddedDomain.AssociationType);
            }
        }

        internal AllorsEmbeddedObject[] Roles
        {
            get
            {
                return this.allorsSession.Extent(AllorsEmbeddedDomain.RoleType);
            }
        }

        internal Domain SuperDomain { get; set; }

        internal AllorsEmbeddedObject[] Types
        {
            get
            {
                return this.allorsSession.Extent(AllorsEmbeddedDomain.ObjectType);
            }
        }

        internal Hashtable TypesById
        {
            get
            {
                var results = new Hashtable();
                foreach (ObjectType allorsType in this.Types)
                {
                    results.Add(allorsType.Id, allorsType);
                }

                return results;
            }
        }

        public static ObjectType CreateAbstractClass(Domain domain, string name)
        {
            var type = CreateType(domain, name);
            type.IsAbstract = true;
            return type;
        }

        public static ObjectType CreateClass(Domain domain, string name)
        {
            var type = CreateType(domain, name);
            return type;
        }

        public static ObjectType CreateInterface(Domain domain, string name)
        {
            var type = CreateType(domain, name);
            type.IsInterface = true;
            return type;
        }

        public static ObjectType CreateMultiple(Domain domain, string name)
        {
            var type = CreateType(domain, name);
            type.IsInterface = true;
            return type;
        }

        public static ObjectType CreateType(Domain domain, string name)
        {
            var type = domain.AddDeclaredObjectType(Guid.NewGuid());
            type.SingularName = name;
            type.PluralName = name + "s";
            return type;
        }
        
        internal void Populate()
        {
            this.PopulateSuperDomain();

            // interfaces
            this.I12 = CreateInterface(this.Domain, "i12");
            this.I34 = CreateInterface(this.Domain, "i34");
            this.I1 = CreateInterface(this.Domain, "i1");
            this.I2 = CreateInterface(this.Domain, "i2");
            this.I3 = CreateInterface(this.Domain, "i3");
            this.I4 = CreateInterface(this.Domain, "i4");

            // abstract classes
            this.A1 = CreateAbstractClass(this.Domain, "a1");
            this.A2 = CreateAbstractClass(this.Domain, "a2");
            this.A3 = CreateAbstractClass(this.Domain, "a3");
            this.A34 = CreateAbstractClass(this.Domain, "a34");
            this.A4 = CreateAbstractClass(this.Domain, "a4");

            // classes
            this.C1 = CreateClass(this.Domain, "c1");
            this.C2 = CreateClass(this.Domain, "c2");
            this.C3 = CreateClass(this.Domain, "c3");
            this.C4 = CreateClass(this.Domain, "c4");

            this.CreateInheritance();

            this.CreateLists();

            this.Domain.Validate();

            if (!this.Domain.IsValid)
            {
                throw new Exception("Domain invalid");
            }
        }

        internal void PopulateWithSuperDomains()
        {
            this.PopulateSuperDomain();

            // interfaces
            this.I12 = CreateInterface(this.SuperDomain, "i12");
            this.I34 = CreateInterface(this.Domain, "i34");
            this.I1 = CreateInterface(this.SuperDomain, "i1");
            this.I2 = CreateInterface(this.SuperDomain, "i2");
            this.I3 = CreateInterface(this.Domain, "i3");
            this.I4 = CreateInterface(this.Domain, "i4");

            // abstract classes
            this.A1 = CreateAbstractClass(this.SuperDomain, "a1");
            this.A2 = CreateAbstractClass(this.SuperDomain, "a2");
            this.A3 = CreateAbstractClass(this.Domain, "a3");
            this.A34 = CreateAbstractClass(this.Domain, "a34");
            this.A4 = CreateAbstractClass(this.Domain, "a4");

            // classes
            this.C1 = CreateClass(this.Domain, "c1");
            this.C2 = CreateClass(this.Domain, "c2");
            this.C3 = CreateClass(this.Domain, "c3");
            this.C4 = CreateClass(this.Domain, "c4");

            this.CreateInheritance();

            this.CreateLists();

            this.Domain.Validate();

            if (!this.Domain.IsValid)
            {
                throw new Exception("Domain invalid");
            }
        }

        private void PopulateSuperDomain()
        {
            this.SuperDomain = this.Domain.AddDirectSuperDomain(Guid.NewGuid());
            this.SuperDomain.Name = "SuperDomain";
        }

        private void CreateInheritance()
        {
            this.A1.AddDirectSupertype(this.I1);
            this.A1.AddDirectSupertype(this.I12);

            this.A2.AddDirectSupertype(this.I2);
            this.A2.AddDirectSupertype(this.I12);

            this.A3.AddDirectSupertype(this.A34);
            this.A3.AddDirectSupertype(this.I3);
            this.A3.AddDirectSupertype(this.I34);

            this.A4.AddDirectSupertype(this.A34);
            this.A4.AddDirectSupertype(this.I4);
            this.A4.AddDirectSupertype(this.I34);

            this.C1.AddDirectSupertype(this.A1);

            this.C2.AddDirectSupertype(this.A2);

            this.C3.AddDirectSupertype(this.A3);

            this.C4.AddDirectSupertype(this.A4);
        }

        private void CreateLists()
        {
            var compositeConcreteClassList = new ArrayList { this.C1, this.C2, this.C3, this.C4 };

            this.CompositeConcreteClasses = (ObjectType[])compositeConcreteClassList.ToArray(typeof(ObjectType));

            var compositeAbstractClassList = new ArrayList { this.A1, this.A2, this.A3, this.A4, this.A34 };

            this.CompositeAbstractClasses = (ObjectType[])compositeAbstractClassList.ToArray(typeof(ObjectType));

            var compositeInterfaceList = new ArrayList { this.I1, this.I2, this.I3, this.I4, this.I12, this.I34 };
            this.CompositeInterfaces = (ObjectType[])compositeInterfaceList.ToArray(typeof(ObjectType));
            this.Interfaces = this.CompositeInterfaces;

            var compositeClassList = new ArrayList();
            compositeClassList.AddRange(this.CompositeConcreteClasses);
            compositeClassList.AddRange(this.CompositeAbstractClasses);
            this.CompositeClasses = (ObjectType[])compositeClassList.ToArray(typeof(ObjectType));

            var compositeList = new ArrayList();
            compositeList.AddRange(this.CompositeClasses);
            compositeList.AddRange(this.CompositeInterfaces);
            this.Composites = (ObjectType[])compositeClassList.ToArray(typeof(ObjectType));

            var classList = new ArrayList();
            classList.AddRange(this.UnitTypes);
            classList.AddRange(this.CompositeClasses);
            this.Classes = (ObjectType[])classList.ToArray(typeof(ObjectType));
        }

        private void GetUnits()
        {
            this.BinaryType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.BinaryId);
            this.BooleanType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.BooleanId);
            this.DateTimeType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.DatetimeId);
            this.DecimalType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.DecimalId);
            this.DoubleType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.DoubleId);
            this.IntegerType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.IntegerId);
            this.LongType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.LongId);
            this.StringType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.StringId);
            this.UniqueType = (ObjectType)this.Domain.Domain.Find(UnitTypeIds.Unique);
        }
    }
}