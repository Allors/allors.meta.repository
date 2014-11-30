// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTypePropertyNames.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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

namespace Allors.R1.Development.Repository.WinForms.Decorators
{
    using System.ComponentModel;
    using Converters;

    [TypeConverter(typeof(RoleTypeConverter))]
    public static class RoleTypePropertyNames 
    {
        public const string Pluralname = "PluralName";
        public const string Precision = "Precision";
        public const string Role = "Role";
        public const string Scale = "Scale";
        public const string Singularname = "SingularName";
        public const string Size = "Size";
        public const string Sort = "Sort";
        public const string Type = "ObjectType";
        public const string Visible = "Visible";
        public const string Many = "Many";
        public const string Max = "Max";
    }
}