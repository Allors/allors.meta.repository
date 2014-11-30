// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationTypeDecorator.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.ComponentModel;

    using Allors.R1.Development.Repository.GtkSharp.PropertyDescriptors;
    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    public enum Multiplicity
    {
        /// <summary>
        /// A one to one relation
        /// </summary>
        OneToOne,

        /// <summary>
        /// A one to many relation
        /// </summary>
        OneToMany,

        /// <summary>
        /// A many to one relation
        /// </summary>
        ManyToOne,

        /// <summary>
        /// A many to many relation
        /// </summary>
        ManyToMany
    }

    public class RelationTypeDecorator : ICustomTypeDescriptor
    {
        private readonly Repository repository;
        private readonly RelationType relationType;
 
        public RelationTypeDecorator(Repository repository, RelationType relationType)
        {
            this.repository = repository;
            this.relationType = relationType;
        }

        [Category("\u200BGeneral")]
        public Guid Id
        {
            get { return this.relationType.Id; }
        }

        [Category("\u200BGeneral")]
        public bool IsIndexed
        {
            get
            {
                return this.relationType.IsIndexed;
            }

            set
            {
                this.relationType.IsIndexed = value;
                this.SendChangedEvent();
            }
        }

        [Category("\u200BGeneral")]
        public bool IsDerived
        {
            get
            {
                return this.relationType.IsDerived;
            }

            set
            {
                this.relationType.IsDerived = value;
                this.SendChangedEvent();
            }
        }

        [Category("\u200BGeneral")]
        public Multiplicity Multiplicity
        {
            get
            {
                if (this.relationType.IsOneToOne)
                {
                    return Multiplicity.OneToOne;
                }

                if (this.relationType.IsOneToMany)
                {
                    return Multiplicity.OneToMany;
                }

                if (this.relationType.IsManyToOne)
                {
                    return Multiplicity.ManyToOne;
                }

                return Multiplicity.ManyToMany;
            }

            set
            {
                switch (value)
                {
                    case Multiplicity.OneToOne:
                        this.relationType.AssociationType.IsMany = false;
                        this.relationType.RoleType.IsMany = false;
                        break;
                    case Multiplicity.OneToMany:
                        this.relationType.AssociationType.IsMany = false;
                        this.relationType.RoleType.IsMany = true;
                        break;
                    case Multiplicity.ManyToOne:
                        this.relationType.AssociationType.IsMany = true;
                        this.relationType.RoleType.IsMany = false;
                        break;
                    default:
                        this.relationType.AssociationType.IsMany = true;
                        this.relationType.RoleType.IsMany = true;
                        break;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("Plural Source")]
        [Category("Association")]
        public string AssociationPluralName
        {
            get
            {
                return this.relationType.AssociationType.AssignedPluralName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.relationType.AssociationType.RemoveAssignedPluralName();
                }
                else
                {
                    this.relationType.AssociationType.AssignedPluralName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("Singular Source")]
        [Category("Association")]
        public string AssociationSingularName
        {
            get
            {
                return this.relationType.AssociationType.AssignedSingularName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.relationType.AssociationType.RemoveAssignedSingularName();
                }
                else
                {
                    this.relationType.AssociationType.AssignedSingularName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("ObjectType")]
        [Category("Association")]
        public ObjectType AssociationObjectType
        {
            get
            {
                return this.relationType.AssociationType.ObjectType;
            }

            set
            {
                this.relationType.AssociationType.ObjectType = value;
                this.SendChangedEvent();
            }
        }

        [DisplayName("Plural Source")]
        [Category("Role")]
        public string RolePluralName
        {
            get
            {
                return this.relationType.RoleType.AssignedPluralName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.relationType.RoleType.RemoveAssignedPluralName();
                }
                else
                {
                    this.relationType.RoleType.AssignedPluralName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("Singular Source")]
        [Category("Role")]
        public string RoleSingularName
        {
            get
            {
                return this.relationType.RoleType.AssignedSingularName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.relationType.RoleType.RemoveAssignedSingularName();
                }
                else
                {
                    this.relationType.RoleType.AssignedSingularName = value;
                }

                this.SendChangedEvent();
            }
        }

        [DisplayName("ObjectType")]
        [Category("Role")]
        public ObjectType RoleObjectType
        {
            get
            {
                return this.relationType.RoleType.ObjectType;
            }

            set
            {
                this.relationType.RoleType.ObjectType = value;
                this.SendChangedEvent();
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
            var isLocked = this.relationType.DomainWhereDeclaredRelationType.IsSuperDomain;
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
                        case "AssociationObjectType":
                        case "RoleObjectType":
                            propertyDescriptors.Add(new ObjectTypePropertyDescriptor(this.repository, propertyDescriptor));
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
            if (this.relationType == null)
            {
                return string.Empty;
            }

            return this.relationType.RoleType.SingularName;
        }

        private void SendChangedEvent()
        {
            this.relationType.SendChangedEvent();
        }
    }
}