// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTypeConverter.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Decorators;

    public class RoleTypeConverter : ExpandablePropertySorter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is RoleTypeDecorator)
            {
                var role = ((RoleTypeDecorator) value).RoleType;
                if (role.ExistObjectType && !role.SingularName.Equals(role.ObjectType.SingularName))
                {
                    return role.SingularName + " (" + role.ObjectType.SingularName + ")";
                }

                return role.SingularName;
            }

            return base.ConvertTo(context, culture, value, destType);
        }
    }
}