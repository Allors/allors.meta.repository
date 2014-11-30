//------------------------------------------------------------------------------------------------- 
// <copyright file="PushDownInheritance.cs" company="Allors bvba">
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
    /// Push down a inheritance from a domain to a super domain.
    /// </summary>
    public class PushDownInheritance : PushDown
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushDownInheritance" /> class.
        /// </summary>
        /// <param name="repository">Push down to this repository.</param>
        /// <param name="inheritanceToPush">The inheritance to push.</param>
        /// <exception cref="System.ArgumentException">Inheritance is already in domain</exception>
        internal PushDownInheritance(Repository repository, Inheritance inheritanceToPush)
            : base(repository)
        {
            if (repository.Domain.Equals(inheritanceToPush.DomainWhereDeclaredInheritance))
            {
                throw new ArgumentException("Inheritance is already in domain", "inheritanceToPush");
            }
            
            this.ResolveDependencies(inheritanceToPush);
        }
    }
}