// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTypeLockedDecorator.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    using Allors.R1.Meta;

    using Converters;

    [TypeConverter(typeof(RoleTypeConverter))]
    public class RoleTypeLockedDecorator : ICustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection allPropertyDescriptors;
        private readonly RoleType roleType;

        public RoleTypeLockedDecorator(RoleType roleType)
        {
            this.roleType = roleType;
            this.allPropertyDescriptors = TypeDescriptor.GetProperties(this, true);
        }

        [DisplayName("Singular Name")]
        [PropertyOrder(1)]
        public string SingularName
        {
            get { return this.roleType.AssignedSingularName; }
        }

        [DisplayName("Plural Name")]
        [PropertyOrder(2)]
        public string PluralName
        {
            get { return this.roleType.AssignedPluralName; }
        }

        [DisplayName("Object Type")]
        [TypeConverter(typeof(TypeConverter))]
        [PropertyOrder(3)]
        public ObjectType ObjectType
        {
            get
            {
                return this.roleType.ObjectType;
            }
        }

        [PropertyOrder(4)]
        public int Size
        {
            get { return this.roleType.Size; }
        }

        [PropertyOrder(5)]
        public string Scale
        {
            get
            {
                if (this.roleType.ExistScale)
                {
                    return this.roleType.Scale.ToString(CultureInfo.InvariantCulture);
                }

                return string.Empty;
            }
        }

        [PropertyOrder(6)]
        public string Precision
        {
            get
            {
                if (this.roleType.ExistPrecision)
                {
                    return this.roleType.Precision.ToString(CultureInfo.InvariantCulture);
                }

                return string.Empty;
            }
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            var properties = new List<PropertyDescriptor>
                                 {
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Role, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Type, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Many, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Pluralname, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Singularname, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Visible, false),
                                     this.allPropertyDescriptors.Find(RoleTypePropertyNames.Sort, false)
                                 };

            // TODO: Tests for ExistObjectType
            if (this.roleType.ExistObjectType)
            {
                if (this.roleType.ObjectType.IsSizeRequired)
                {
                    properties.Add(this.allPropertyDescriptors.Find(RoleTypePropertyNames.Size, false));
                }

                if (this.roleType.ObjectType.IsPrecisionRequired)
                {
                    properties.Add(this.allPropertyDescriptors.Find(RoleTypePropertyNames.Precision, false));
                }

                if (this.roleType.ObjectType.IsScaleRequired)
                {
                    properties.Add(this.allPropertyDescriptors.Find(RoleTypePropertyNames.Scale, false));
                }
            }

            return new PropertyDescriptorCollection(properties.ToArray());
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public override string ToString()
        {
            if (this.roleType.ExistObjectType && !this.roleType.SingularName.Equals(this.roleType.ObjectType.SingularName))
            {
                return this.roleType.SingularName + " (" + this.roleType.ObjectType.SingularName + ")";
            }

            return this.roleType.SingularName;
        }
    }
}