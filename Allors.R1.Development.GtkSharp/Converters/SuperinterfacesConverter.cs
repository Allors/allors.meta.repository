// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperinterfacesConverter.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using Allors.R1.Meta;

    public class SuperinterfacesConverter : TypeConverter
    {
        public static string ToString(ObjectType[] superinterfaces)
        {
            switch (superinterfaces.Length)
            {
                case 0:
                    return "No interfaces";
                case 1:
                    return superinterfaces[0].ToString();
                case 2:
                    return superinterfaces[0] + ", " + superinterfaces[1];
            }

            return superinterfaces[0] + ", " + superinterfaces[1] + ", ..."; 
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ObjectType[])
            {
                var superinterfaces = (ObjectType[])value;
                return ToString(superinterfaces);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}