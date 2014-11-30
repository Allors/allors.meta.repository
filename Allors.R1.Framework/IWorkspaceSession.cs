// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkspaceSession.cs" company="Allors bvba">
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
    using Meta;

    /// <summary>
    /// Extends <see cref="ISession"/> with workspace capabilities. 
    /// </summary>
    public interface IWorkspaceSession : ISession
    {
        /// <summary>
        /// Gets the database session.
        /// </summary>
        /// <value>The database session.</value>
        IDatabaseSession DatabaseSession { get; }

        /// <summary>
        /// Gets the workspace.
        /// </summary>
        IWorkspace Workspace { get; }

        /// <summary>
        /// Gets the conflicts.
        /// </summary>
        IConflict[] Conflicts { get; }

        /// <summary>
        /// Sync the changes from this workspace back to the database.
        /// </summary>
        void Sync();

        /// <summary>
        /// Resolve a single conflict.
        /// </summary>
        /// <param name="conflict">
        /// The conflict.
        /// </param>
        void Resolve(IConflict conflict);

        /// <summary>
        /// Resolve multiple conflicts.
        /// </summary>
        /// <param name="conflicts">
        /// The conflicts.
        /// </param>
        void Resolve(IConflict[] conflicts);

        /// <summary>
        /// Get all <see cref="IObject"/>s that are local in this workspace.
        /// </summary>
        /// <returns>
        /// The local extent.
        /// </returns>
        IObject[] LocalExtent();

        /// <summary>
        /// Get all <see cref="IObject"/>s that are local in this workspace and of the specified type.
        /// </summary>
        /// <param name="objectType">
        /// The object Type.
        /// </param>
        /// <returns>
        /// The local extent.
        /// </returns>
        Extent LocalExtent(ObjectType objectType);
    }
}