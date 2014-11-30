// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationLockedDecorator.cs" company="Allors bvba">
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
    using System.ComponentModel;

    using Allors.R1.Meta;

    using Converters;

    [TypeConverter(typeof(AssociationConverter))]
    public class AssociationLockedDecorator
    {
        private readonly AssociationType association;

        public AssociationLockedDecorator(AssociationType association)
        {
            this.association = association;
        }

        [DisplayName("Singular Name")]
        [PropertyOrder(1)]
        public string SingularName
        {
            get { return this.association.AssignedSingularName; }
        }

        [DisplayName("Plural Name")]
        [PropertyOrder(2)]
        public string PluralName
        {
            get { return this.association.AssignedPluralName; }
        }

        [DisplayName("Object Type")]
        [TypeConverter(typeof(TypeConverter))]
        [PropertyOrder(3)]
        public ObjectType ObjectType
        {
            get { return this.association.ObjectType; }
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
    }
}