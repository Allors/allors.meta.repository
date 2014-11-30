// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatabase.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.R1
{
    /// <summary>
    /// A database is an online <see cref="IPopulation"/>.
    /// </summary>
    public interface IDatabase : IPopulation
    {
        /// <summary>
        /// Occurs when an object could not be loaded.
        /// </summary>
        event ObjectNotLoadedEventHandler ObjectNotLoaded;

        /// <summary>
        /// Occurs when a relation could not be loaded.
        /// </summary>
        event RelationNotLoadedEventHandler RelationNotLoaded;

        /// <summary>
        /// Gets a value indicating whether this database is shared with other databases with the same name.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this database is shared; otherwise, <c>false</c>.
        /// </value>
        bool IsShared { get; }

        /// <summary>
        /// Initializes the database. If this population is persistent then
        /// all existing objects will be deleted.
        /// </summary>
        void Init();

        /// <summary>
        /// Creates a new database Session.
        /// </summary>
        /// <returns>a newly created AllorsSession</returns>
        new IDatabaseSession CreateSession();

        /// <summary>
        /// Creates a new workspace for this database.
        /// </summary>
        /// <returns>
        /// The <see cref="IWorkspace"/>.
        /// </returns>
        IWorkspace CreateWorkspace();
    }
}