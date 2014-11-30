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

namespace Allors.R1.Development.Repository.WinForms.Decorators
{
    using System;
    using System.ComponentModel;

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

    [TypeConverter(typeof(PropertySorter))]
    public class RelationTypeDecorator
    {
        private readonly AssociationDecorator associationDecorator;
        private readonly RelationType relationType;
        private readonly RoleTypeDecorator roleTypeDecorator;

        public RelationTypeDecorator(RelationType relationType)
        {
            this.relationType = relationType;
            this.associationDecorator = new AssociationDecorator(relationType.AssociationType);
            this.roleTypeDecorator = new RoleTypeDecorator(relationType.RoleType);
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(1)]
        public Guid Id
        {
            get { return this.relationType.Id; }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(2)]
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

        [Category("\u200BGeneral")]
        [PropertyOrder(3)]
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
        [PropertyOrder(4)]
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

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Endpoints")]
        [DisplayName("Association Type")]
        [PropertyOrder(1)]
        public AssociationDecorator AssociationType
        {
            get { return this.associationDecorator; }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Endpoints")]
        [DisplayName("Role Type")]
        [PropertyOrder(2)]
        public RoleTypeDecorator RoleType
        {
            get { return this.roleTypeDecorator; }
        }

        private void SendChangedEvent()
        {
            this.relationType.SendChangedEvent();
        }
    }
}