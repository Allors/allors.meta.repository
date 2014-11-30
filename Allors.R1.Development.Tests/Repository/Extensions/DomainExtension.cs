// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtension.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository
{
    using System;

    using Allors.R1.Meta;

    public static class DomainExtension
    {
        public static ObjectType BuildObjectType(this Domain domain, Guid id, string singularName, string pluralName)
        {
            var objectType = domain.AddDeclaredObjectType(id);
            objectType.SingularName = singularName;
            objectType.PluralName = pluralName;
            objectType.SendChangedEvent();
            return objectType;
        }

        public static Inheritance BuildInheritance(this Domain domain, Guid id, ObjectType subType, ObjectType superType)
        {
            var inheritance = domain.AddDeclaredInheritance(id);
            inheritance.Subtype = subType;
            inheritance.Supertype = superType;
            inheritance.SendChangedEvent();
            return inheritance;
        }

        public static RelationType BuildRelationType(this Domain domain, Guid id, Guid associationTypeId, ObjectType associationObjectType, Guid roleTypeId, ObjectType roleObjectType)
        {
            var relaionType = domain.AddDeclaredRelationType(id, associationTypeId, roleTypeId);
            relaionType.AssociationType.ObjectType = associationObjectType;
            relaionType.RoleType.ObjectType = roleObjectType;
            relaionType.SendChangedEvent();
            return relaionType;
        }
    }
}