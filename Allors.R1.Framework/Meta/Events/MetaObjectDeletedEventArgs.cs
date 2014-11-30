//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaObjectDeletedEventArgs.cs" company="Allors bvba">
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
// <summary>Defines the MetaObjectDeletedEventArgs type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1.Meta.Events
{
    using System;

    /// <summary>
    /// The domain changed event arguments.
    /// </summary>
    public class MetaObjectDeletedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetaObjectDeletedEventArgs"/> class.
        /// </summary>
        /// <param name="metaObjectId">
        /// The meta Object Id.
        /// </param>
        public MetaObjectDeletedEventArgs(Guid metaObjectId)
        {
            this.MetaObjectId = metaObjectId;
        }

        /// <summary>
        /// Gets the meta object id.
        /// </summary>
        public Guid MetaObjectId { get; private set; }
    }
}