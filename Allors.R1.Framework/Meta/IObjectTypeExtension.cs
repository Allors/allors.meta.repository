//------------------------------------------------------------------------------------------------- 
// <copyright file="IObjectTypeExtension.cs" company="Allors bvba">
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
// <summary>Defines the Domain type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    /// <summary>
    /// An ObjectTypeExtension allows you to extend an object type programmatically.
    /// ObjectTypeExtensions are available in the second phase of a two phase generation.
    /// </summary>
    public interface IObjectTypeExtension
    {
        /// <summary>
        /// Gets the object type.
        /// </summary>
        ObjectType ObjectType { get; }
    }
}