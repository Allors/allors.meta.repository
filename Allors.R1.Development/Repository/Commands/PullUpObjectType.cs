//------------------------------------------------------------------------------------------------- 
// <copyright file="PullUpObjectType.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.Commands
{
    using System;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    /// <summary>
    /// Pulls up an object type from a domain to a super domain.
    /// </summary>
    public class PullUpObjectType : PullUp
    {
        private readonly bool includeContainedItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullUpObjectType" /> class.
        /// </summary>
        /// <param name="pullFromRepository">Pull from this repository.</param>
        /// <param name="pullToSuperDomain">Pull up to this super domain.</param>
        /// <param name="objectTypeToPull">The object type to pull.</param>
        /// <param name="includeContainedItems">if set to <c>true</c> then include contained items (ObjectTypes, Inheritances and RelationType).</param>
        /// <exception cref="System.ArgumentException">ObjectType is not in domain</exception>
        internal PullUpObjectType(Repository pullFromRepository, Domain pullToSuperDomain, ObjectType objectTypeToPull, bool includeContainedItems)
            : base(pullFromRepository, pullToSuperDomain)
        {
            this.includeContainedItems = includeContainedItems;

            if (!this.PullFromRepository.Domain.Equals(objectTypeToPull.DomainWhereDeclaredObjectType))
            {
                throw new ArgumentException("ObjectType is not in domain", "objectTypeToPull");
            }

            this.ResolveDependencies(objectTypeToPull);
            
            if (this.includeContainedItems)
            {
                foreach (var inheritance in objectTypeToPull.InheritancesWhereSubtype)
                {
                    this.ResolveDependencies(inheritance);
                }

                foreach (var roleType in objectTypeToPull.RoleTypes)
                {
                    this.ResolveDependencies(roleType.RelationType);
                }
            }
        }
    }
}