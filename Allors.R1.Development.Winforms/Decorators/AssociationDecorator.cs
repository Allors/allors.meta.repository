// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationDecorator.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    using Allors.R1.Meta;

    using Converters;
    using Editors;

    [TypeConverter(typeof(AssociationConverter))]
    public class AssociationDecorator : ObjectTypeTypeEditor.ISource
    {
        private readonly AssociationType association;

        public AssociationDecorator(AssociationType association)
        {
            this.association = association;
        }

        [DisplayName("Singular Name")]
        [PropertyOrder(1)]
        public string SingularName
        {
            get
            {
                return this.association.AssignedSingularName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.association.RemoveAssignedSingularName();
                }
                else
                {
                    this.association.AssignedSingularName = value;
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
                return this.association.AssignedPluralName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.association.RemoveAssignedPluralName();
                }
                else
                {
                    this.association.AssignedPluralName = value;
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
                return this.association.ObjectType;
            }

            set
            {
                this.association.ObjectType = value;
                this.SendChangedEvent();
            }
        }

        [Browsable(false)]
        public ObjectType[] EditorObjectTypes
        {
            get
            {
                var sortedTypes = new List<ObjectType>(Domain.GetDomain(this.Association).CompositeObjectTypes);
                sortedTypes.Sort();
                return sortedTypes.ToArray();
            }
        }

        [Browsable(false)]
        public AssociationType Association
        {
            get { return this.association; }
        }

        public override string ToString()
        {
            var result = this.association.ToString();
            if (this.association.ExistObjectType)
            {
                result += " " + this.association.ObjectType;
            }

            return result;
        }

        private void SendChangedEvent()
        {
            this.association.SendChangedEvent();
        }
    }
}