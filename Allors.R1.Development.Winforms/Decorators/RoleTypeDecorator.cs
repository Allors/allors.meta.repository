// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTypeDecorator.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
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
    using System.Drawing.Design;
    using System.Globalization;

    using Allors.R1.Development.Repository.WinForms.Converters;
    using Allors.R1.Development.Repository.WinForms.Editors;
    using Allors.R1.Meta;

    [TypeConverter(typeof(RoleTypeConverter))]
    public class RoleTypeDecorator : ICustomTypeDescriptor, ObjectTypeTypeEditor.ISource
    {
        private readonly PropertyDescriptorCollection allPropertyDescriptors;
        private readonly RoleType roleType;

        public RoleTypeDecorator(RoleType role)
        {
            this.roleType = role;
            this.allPropertyDescriptors = TypeDescriptor.GetProperties(this, true);
        }

        [Browsable(false)]
        public ObjectType[] EditorObjectTypes
        {
            get
            {
                var sortedTypes = new List<ObjectType>(Domain.GetDomain(this.RoleType).ObjectTypes);
                sortedTypes.Sort();
                return sortedTypes.ToArray();
            }
        }

        [DisplayName("Singular Name")]
        [PropertyOrder(1)]
        public string SingularName
        {
            get
            {
                return this.roleType.AssignedSingularName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.roleType.RemoveAssignedSingularName();
                }
                else
                {
                    this.roleType.AssignedSingularName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("Plural Name")]
        [PropertyOrder(2)]
        public string PluralName
        {
            get
            {
                return this.roleType.AssignedPluralName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.roleType.RemoveAssignedPluralName();
                }
                else
                {
                    this.roleType.AssignedPluralName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("Object Type")]
        [TypeConverter(typeof(TypeConverter))]
        [Editor(typeof(ObjectTypeTypeEditor), typeof(UITypeEditor))]
        [PropertyOrder(3)]
        public ObjectType ObjectType
        {
            get
            {
                return this.roleType.ObjectType;
            }

            set
            {
                this.roleType.ObjectType = value;
                this.SendChangedEvent();
            }
        }

        [PropertyOrder(4)]
        public string Size
        {
            get
            {
                if (this.roleType.ExistSize)
                {
                    if (RoleType.MaximumSize.Equals(this.roleType.Size))
                    {
                        return RoleTypePropertyNames.Max;
                    }

                    return this.roleType.Size.ToString(CultureInfo.InvariantCulture);
                }

                return string.Empty;
            }

            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.roleType.RemoveSize();
                    }
                    else
                    {
                        this.roleType.Size = RoleTypePropertyNames.Max.Equals(value) ? RoleType.MaximumSize : int.Parse(value);
                    }

                    this.SendChangedEvent();
                }
                catch (Exception e)
                {
                    throw new Exception(this.Size + " has illegal value: " + e.Message);
                }
            }
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

            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.roleType.RemoveScale();
                    }
                    else
                    {
                        this.roleType.Scale = int.Parse(value);
                        this.SendChangedEvent();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(RoleTypePropertyNames.Precision + " has illegal value: " + e.Message);
                }
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

            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.roleType.RemovePrecision();
                    }
                    else
                    {
                        this.roleType.Precision = int.Parse(value);
                        this.SendChangedEvent();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(RoleTypePropertyNames.Precision + " has illegal value: " + e.Message);
                }
            }
        }

        [Browsable(false)]
        public RoleType RoleType
        {
            get
            {
                return this.roleType;
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

        private void SendChangedEvent()
        {
            this.roleType.SendChangedEvent();
        }
    }
}