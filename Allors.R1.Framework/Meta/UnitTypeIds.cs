//------------------------------------------------------------------------------------------------- 
// <copyright file="UnitTypeIds.cs" company="Allors bvba">
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
// <summary>Defines the UnitTypeTags type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;

    /// <summary>
    /// The ids for unit ObjectTypes.
    /// Ids can be used for long term persistence and should therefore never be changed.
    /// </summary>
    public static class UnitTypeIds
    {
        /// <summary>
        /// The id of the binary type.
        /// </summary>
        public static readonly Guid BinaryId = new Guid("c28e515b-cae8-4d6b-95bf-062aec8042fc");

        /// <summary>
        /// The id of the boolean type.
        /// </summary>
        public static readonly Guid BooleanId = new Guid("b5ee6cea-4E2b-498e-a5dd-24671d896477");

        /// <summary>
        /// The id of the datetime type.
        /// </summary>
        public static readonly Guid DatetimeId = new Guid("c4c09343-61d3-418c-ade2-fe6fd588f128");

        /// <summary>
        /// The id of the decimal type.
        /// </summary>
        public static readonly Guid DecimalId = new Guid("da866d8e-2c40-41a8-ae5b-5f6dae0b89c8");

        /// <summary>
        /// The id of the double type.
        /// </summary>
        public static readonly Guid DoubleId = new Guid("ffcabd07-f35f-4083-bef6-f6c47970ca5d");

        /// <summary>
        /// The id of the integer type.
        /// </summary>
        public static readonly Guid IntegerId = new Guid("ccd6f134-26de-4103-bff9-a37ec3e997a3");

        /// <summary>
        /// The id of the long integer type.
        /// </summary>
        public static readonly Guid LongId = new Guid("e8989069-024b-4389-ac77-a98c4dfff25a");

        /// <summary>
        /// The id of the string type.
        /// </summary>
        public static readonly Guid StringId = new Guid("ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9");

        /// <summary>
        /// The id of the unique type.
        /// </summary>
        public static readonly Guid Unique = new Guid("6dc0a1a8-88a4-4614-adb4-92dd3d017c0e");
    }
}