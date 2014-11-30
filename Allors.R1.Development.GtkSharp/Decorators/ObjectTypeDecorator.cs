// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeDecorator.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Decorators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Allors.R1.Development.Repository;
    using Allors.R1.Development.Repository.GtkSharp.Converters;
    using Allors.R1.Development.Repository.GtkSharp.PropertyDescriptors;
    using Allors.R1.Meta;

    using TypeConverter = System.ComponentModel.TypeConverter;

    public class ObjectTypeDecorator : ICustomTypeDescriptor
    {
        private readonly Repository repository;
        private readonly ObjectType objectType;

        public ObjectTypeDecorator(Repository repository, ObjectType objectType)
        {
            this.repository = repository;
            this.objectType = objectType;
        }

        [Category("\u200BGeneral")]
        public Guid Id
        {
            get { return this.objectType.Id; }
        }

        [Category("Modifiers")]
        public bool Abstract
        {
            get
            {
                return this.objectType.IsAbstract;
            }

            set
            {
                this.objectType.IsAbstract = value;
                this.SendChangedEvent();
            }
        }

        [Category("Modifiers")]
        public bool Interface
        {
            get
            {
                return this.objectType.IsInterface;
            }

            set
            {
                this.objectType.IsInterface = value;
                this.SendChangedEvent();
            }
        }

        [DisplayName("Plural Source")]
        [Category("\u200BGeneral")]
        public string PluralName
        {
            get
            {
                return this.objectType.PluralName;
            }

            set
            {
                this.objectType.PluralName = value;
                this.SendChangedEvent();
            }
        }

        [DisplayName("Singular Source")]
        [Category("\u200BGeneral")]
        public string SingularName
        {
            get
            {
                return this.objectType.SingularName;
            }

            set
            {
                this.objectType.SingularName = value;
                this.SendChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType ObjectType
        {
            get { return this.objectType; }
        }

        [Browsable(false)]
        public ObjectType[] PossibleSuperClasses
        {
            get
            {
                var superClasses = new ArrayList();
                foreach (var compositeObjectType in Domain.GetDomain(this.objectType).CompositeObjectTypes)
                {
                    if (!compositeObjectType.IsInterface && compositeObjectType.IsAbstract && !compositeObjectType.Equals(this.objectType))
                    {
                        superClasses.Add(compositeObjectType);
                    }
                }

                return (ObjectType[])superClasses.ToArray(typeof(ObjectType));
            }
        }

        [Category("Inheritance")]
        public ObjectType Superclass
        {
            get
            {
                return this.objectType.DirectSuperclass;
            }

            set
            {
                if (value == null)
                {
                    var inheritance = this.objectType.FindInheritanceWhereDirectSubtype(this.Superclass);
                    if (inheritance != null)
                    {
                        inheritance.Delete();
                    }
                }
                else
                {
                    this.objectType.AddDirectSupertype(value);
                }

                this.SendInheritanceChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType[] PossibleSuperinterfaces
        {
            get
            {
                var superinterfaces = new ArrayList();
                foreach (var compositeObjectType in Domain.GetDomain(this.objectType).CompositeObjectTypes)
                {
                    if (compositeObjectType.IsInterface)
                    {
                        superinterfaces.Add(compositeObjectType);
                    }
                }

                return (ObjectType[])superinterfaces.ToArray(typeof(ObjectType));
            }
        }

        [TypeConverter(typeof(SuperinterfacesConverter))]
        [Category("Inheritance")]
        public ObjectType[] Superinterfaces
        {
            get
            {
                return this.objectType.DirectSuperinterfaces;
            }

            set
            {
                this.objectType.SetDirectSuperinterfaces(value);
                this.SendInheritanceChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType SuperInterfaceObjectType
        {
            get { return this.objectType; }
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
            var isLocked = this.objectType.DomainWhereDeclaredObjectType.IsSuperDomain;
            var propertyDescriptors = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(this, true))
            {
                if (isLocked)
                {
                    propertyDescriptors.Add(new ReadOnlyPropertyDescriptor(propertyDescriptor));
                }
                else
                {
                    switch (propertyDescriptor.Name)
                    {
                        case "Superclass":
                            propertyDescriptors.Add(new AbstractClassPropertyDescriptor(this.repository, propertyDescriptor));
                            break;

                        case "Superinterfaces":
                            propertyDescriptors.Add(new InterfacesPropertyDescriptor(this.repository, propertyDescriptor));
                            break;

                        default:
                            propertyDescriptors.Add(propertyDescriptor);
                            break;
                    }
                }
            }

            return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
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
            if (this.objectType == null)
            {
                return string.Empty;
            }
            
            return this.objectType.SingularName;
        }

        private void SendChangedEvent()
        {
            this.objectType.SendChangedEvent();
        }

        private void SendInheritanceChangedEvent()
        {
            var domain = Domain.GetDomain(this.objectType);
            foreach (var inheritance in this.objectType.InheritancesWhereSubtype)
            {
                inheritance.SendChangedEvent();
            }

            domain.SendChangedEvent();
        }
    }
}