//------------------------------------------------------------------------------------------------- 
// <copyright file="IChangeSet.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the IChangeSet type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1
{
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// A change set is created during a checkpoint
    /// and contains all changes that have
    /// occurred in a <see cref="ISession"/> either starting
    /// from the beginning of the transaction or from a 
    /// previous checkpoint.
    /// </summary>
    public interface IChangeSet
    {
        /// <summary>
        /// Gets the created objects.
        /// </summary>
        ISet<ObjectId> Created { get; }

        /// <summary>
        /// Gets the deleted objects.
        /// </summary>
        ISet<ObjectId> Deleted { get; }

        /// <summary>
        /// Gets the changed associations.
        /// </summary>
        ISet<ObjectId> Associations { get; }

        /// <summary>
        /// Gets the changed roles.
        /// </summary>
        ISet<ObjectId> Roles { get; }

        /// <summary>
        /// Gets the changed role types by association.
        /// </summary>
        IDictionary<ObjectId, ISet<RoleType>> RoleTypesByAssociation { get; }

        /// <summary>
        /// Get the changed role types for the association.
        /// </summary>
        /// <param name="association">
        /// The association.
        /// </param>
        /// <returns>
        /// The role types.
        /// </returns>
        ISet<RoleType> GetRoleTypes(ObjectId association);
    }
}