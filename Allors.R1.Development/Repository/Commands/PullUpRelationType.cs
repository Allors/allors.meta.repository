//------------------------------------------------------------------------------------------------- 
// <copyright file="PullUpRelationType.cs" company="Allors bvba">
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
    /// Pulls up an object type from a domain to a pullToSuperDomain.
    /// </summary>
    public class PullUpRelationType : PullUp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullUpRelationType" /> class.
        /// </summary>
        /// <param name="pullFromRepository">Pull from this repository.</param>
        /// <param name="pullToSuperDomain">Pull up to this super domain.</param>
        /// <param name="relationTypeToPull">The object type to pull.</param>
        /// <exception cref="System.ArgumentException">RelationType is not in domain</exception>
        internal PullUpRelationType(Repository pullFromRepository, Domain pullToSuperDomain, RelationType relationTypeToPull)
            : base(pullFromRepository, pullToSuperDomain)
        {
            if (!this.PullFromRepository.Domain.Equals(relationTypeToPull.DomainWhereDeclaredRelationType))
            {
                throw new ArgumentException("RelationType is not in domain", "relationTypeToPull");
            }

            this.ResolveDependencies(relationTypeToPull);
        }
    }
}