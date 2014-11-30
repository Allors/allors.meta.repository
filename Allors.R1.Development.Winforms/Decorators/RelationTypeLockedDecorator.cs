// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationTypeLockedDecorator.cs" company="Allors bvba">
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

    [TypeConverter(typeof(PropertySorter))]
    public class RelationTypeLockedDecorator
    {
        private readonly AssociationLockedDecorator associationTypeDecorator;
        private readonly RelationType relationType;
        private readonly RoleTypeLockedDecorator roleTypeDecorator;

        public RelationTypeLockedDecorator(RelationType relationType)
        {
            this.relationType = relationType;
            this.associationTypeDecorator = new AssociationLockedDecorator(relationType.AssociationType);
            this.roleTypeDecorator = new RoleTypeLockedDecorator(relationType.RoleType);
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
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(3)]
        public bool IsIndexed
        {
            get { return this.relationType.IsIndexed; }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(4)]
        public bool IsDerived
        {
            get
            {
                return this.relationType.IsDerived;
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Endpoints")]
        [DisplayName("Association Type")]
        public AssociationLockedDecorator AssociationType
        {
            get { return this.associationTypeDecorator; }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Endpoints")]
        [DisplayName("Role Type")]
        public RoleTypeLockedDecorator RoleType
        {
            get { return this.roleTypeDecorator; }
        }
    }
}