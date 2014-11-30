//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaObjectChangedEventArgs.cs" company="Allors bvba">
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
// <summary>Defines the MetaObjectChangedEventArgs type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1.Meta.Events
{
    using System;

    /// <summary>
    /// The domain changed event arguments.
    /// </summary>
    public class MetaObjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetaObjectChangedEventArgs"/> class.
        /// </summary>
        /// <param name="metaObject">The changed meta object.</param>
        public MetaObjectChangedEventArgs(MetaObject metaObject)
        {
            this.MetaObject = metaObject;
        }

        /// <summary>
        /// Gets the meta object.
        /// </summary>
        public MetaObject MetaObject { get; private set; }
    }
}