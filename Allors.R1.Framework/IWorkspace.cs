// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkspace.cs" company="Allors bvba">
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
    /// A workspace is an <see cref="IPopulation"/> that is connected
    /// to a <see cref="IPopulation"/> and allows
    /// changes to be synced back to its database when needed.
    /// This allows for transparent long running transactions.
    /// </summary>
    public interface IWorkspace : IPopulation
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        IDatabase Database { get; }

        /// <summary>
        /// Creates a workspace session.
        /// </summary>
        /// <returns>
        /// The created workspace session.
        /// </returns>
        new IWorkspaceSession CreateSession();
    }
}