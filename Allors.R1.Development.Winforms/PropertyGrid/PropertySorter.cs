// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySorter.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class PropertySorter : TypeConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var properties = new List<KeyValuePair<int, PropertyDescriptor>>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(value, attributes))
            {
                var attribute = propertyDescriptor.Attributes[typeof(PropertyOrderAttribute)] as PropertyOrderAttribute;
                var order = attribute != null ? attribute.Order : 0;
                properties.Add(new KeyValuePair<int, PropertyDescriptor>(order, propertyDescriptor));
            }

            properties.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            return new PropertyDescriptorCollection(properties.Select(pair => pair.Value).ToArray());
        }
    }
}