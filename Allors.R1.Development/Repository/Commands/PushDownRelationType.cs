//------------------------------------------------------------------------------------------------- 
// <copyright file="PushDownRelationType.cs" company="Allors bvba">
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
    /// Push down a relationType from a domain to a super domain.
    /// </summary>
    public class PushDownRelationType : PushDown
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushDownRelationType" /> class.
        /// </summary>
        /// <param name="repository">Push down to this repository.</param>
        /// <param name="relationTypeToPush">The relationType to push.</param>
        /// <exception cref="System.ArgumentException">RelationType is already in domain</exception>
        internal PushDownRelationType(Repository repository, RelationType relationTypeToPush)
            : base(repository)
        {
            if (repository.Domain.Equals(relationTypeToPush.DomainWhereDeclaredRelationType))
            {
                throw new ArgumentException("RelationType is already in domain", "relationTypeToPush");
            }
            
            this.ResolveDependencies(relationTypeToPush);
        }
    }
}