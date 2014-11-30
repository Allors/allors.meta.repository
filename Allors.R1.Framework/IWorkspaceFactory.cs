// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkspaceFactory.cs" company="Allors bvba">
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
    /// A factory for new workspace objects.
    /// </summary>
    public interface IWorkspaceFactory
    {
        /// <summary>
        /// Creates a new workspace.
        /// </summary>
        /// <param name="database">The database</param>
        /// <returns>The workspace</returns>
        IWorkspace CreateWorkspace(IDatabase database);
    }
}