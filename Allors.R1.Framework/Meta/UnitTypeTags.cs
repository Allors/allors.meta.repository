//------------------------------------------------------------------------------------------------- 
// <copyright file="UnitTypeTags.cs" company="Allors bvba">
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
// <summary>Defines the UnitTypeTag type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1.Meta
{
    using Allors.R1.Meta.AllorsGenerated;

    /// <summary>
    /// The tags for <see cref="AllorsClassObjectType.UnitTag"/>s.
    /// Do not use tags for long term persistence, UnitTypeIds should be used for that.
    /// </summary>
    public enum UnitTypeTags
    {
        /// <summary>
        /// The tag for the binary <see cref="ObjectType"/>.
        /// </summary>
        AllorsBinary,

        /// <summary>
        /// The tag for the boolean <see cref="ObjectType"/>.
        /// </summary>
        AllorsBoolean,

        /// <summary>
        /// The tag for the date time <see cref="ObjectType"/>.
        /// </summary>
        AllorsDateTime,

        /// <summary>
        /// The tag for the decimal <see cref="ObjectType"/>.
        /// </summary>
        AllorsDecimal,

        /// <summary>
        /// The tag for the double <see cref="ObjectType"/>.
        /// </summary>
        AllorsDouble,

        /// <summary>
        /// The tag for the integer <see cref="ObjectType"/>.
        /// </summary>
        AllorsInteger,

        /// <summary>
        /// The tag for the long integer <see cref="ObjectType"/>.
        /// </summary>
        AllorsLong,

        /// <summary>
        /// The tag for the string <see cref="ObjectType"/>.
        /// </summary>
        AllorsString,

        /// <summary>
        /// The tag for the unique <see cref="ObjectType"/>.
        /// </summary>
        AllorsUnique,
    }
}