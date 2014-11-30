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

namespace Allors.R1.Development.Repository.WinForms.Decorators
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;

    using Allors.R1.Meta;

    using Converters;
    using Editors;

    [TypeConverter(typeof(PropertySorter))]
    public class ObjectTypeDecorator : SuperclassTypeEditor.ISource, SuperinterfacesTypeEditor.ISource
    {
        private readonly ObjectType type;

        public ObjectTypeDecorator(ObjectType type)
        {
            this.type = type;
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(1)]
        public Guid Id
        {
            get { return this.type.Id; }
        }

        [DisplayName("Singular Name")]
        [Category("\u200BGeneral")]
        [PropertyOrder(2)]
        public string SingularName
        {
            get
            {
                return this.type.SingularName;
            }

            set
            {
                this.type.SingularName = value;
                this.SendChangedEvent();
            }
        }

        [DisplayName("Plural Name")]
        [Category("\u200BGeneral")]
        [PropertyOrder(2)]
        public string PluralName
        {
            get
            {
                return this.type.PluralName;
            }

            set
            {
                this.type.PluralName = value;
                this.SendChangedEvent();
            }
        }

        [Category("Modifiers")]
        [PropertyOrder(1)]
        public bool Interface
        {
            get
            {
                return this.type.IsInterface;
            }

            set
            {
                this.type.IsInterface = value;
                this.SendChangedEvent();
            }
        }

        [Category("Modifiers")]
        [PropertyOrder(2)]
        public bool Abstract
        {
            get
            {
                return this.type.IsAbstract;
            }

            set
            {
                this.type.IsAbstract = value;
                this.SendChangedEvent();
            }
        }

        [Editor(typeof(SuperclassTypeEditor), typeof(UITypeEditor))]
        [Category("Inheritance")]
        [PropertyOrder(1)]
        public ObjectType Superclass
        {
            get
            {
                return this.type.DirectSuperclass;
            }

            set
            {
                if (value == null)
                {
                    var inheritance = this.type.FindInheritanceWhereDirectSubtype(this.Superclass);
                    inheritance.Delete();
                }
                else
                {
                    this.type.AddDirectSupertype(value);
                }

                this.SendInheritanceChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType[] PossibleSuperClasses
        {
            get
            {
                var superClasses = new ArrayList();
                foreach (var compositeObjectType in Domain.GetDomain(this.type).CompositeObjectTypes)
                {
                    if (!compositeObjectType.IsInterface && compositeObjectType.IsAbstract && !compositeObjectType.Equals(this.type))
                    {
                        superClasses.Add(compositeObjectType);
                    }
                }

                return (ObjectType[])superClasses.ToArray(typeof(ObjectType));
            }
        }

        [TypeConverter(typeof(SuperinterfacesConverter))]
        [Editor(typeof(SuperinterfacesTypeEditor), typeof(UITypeEditor))]
        [Category("Inheritance")]
        [PropertyOrder(2)]
        public ObjectType[] Superinterfaces
        {
            get
            {
                return this.type.DirectSuperinterfaces;
            }

            set
            {
                this.type.SetDirectSuperinterfaces(value);
                this.SendInheritanceChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType[] PossibleSuperinterfaces
        {
            get
            {
                var superinterfaces = new ArrayList();
                foreach (var compositeObjectType in Domain.GetDomain(this.type).CompositeObjectTypes)
                {
                    if (compositeObjectType.IsInterface)
                    {
                        superinterfaces.Add(compositeObjectType);
                    }
                }

                return (ObjectType[])superinterfaces.ToArray(typeof(ObjectType));
            }
        }

        [Browsable(false)]
        public ObjectType SuperInterfaceType
        {
            get { return this.type; }
        }

        public override string ToString()
        {
            if (this.type == null)
            {
                return string.Empty;
            }
            
            return this.type.SingularName;
        }

        private void SendChangedEvent()
        {
            this.type.SendChangedEvent();
        }

        private void SendInheritanceChangedEvent()
        {
            var domain = Domain.GetDomain(this.type);
            foreach (var inheritance in this.type.InheritancesWhereSubtype)
            {
                inheritance.SendChangedEvent();
            }

            domain.SendChangedEvent();
        }
    }
}