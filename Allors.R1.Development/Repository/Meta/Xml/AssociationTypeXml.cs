// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociationTypeXml.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Meta.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors.R1.Development.Repository.Xml;
    using Allors.R1.Meta;

    public class AssociationTypeXml
    {
        [XmlAttribute("id")]
        public Guid id;

        public string isMany;

        [XmlElement("objectType")]
        public IdrefXml objectType;

        public string pluralName;

        public string singularName;
        
        internal AssociationTypeXml()
        {
        }
        
        public void SyncFromMeta(AssociationType associationType)
        {
            this.id = associationType.Id;

            this.objectType = null;
            if (!associationType.IsObjectTypeDefault)
            {
                this.objectType = new IdrefXml(associationType.ObjectType.Id);
            }

            this.singularName = null;
            if (!associationType.IsAssignedSingularNameDefault)
            {
                this.singularName = associationType.AssignedSingularName;
            }

            this.pluralName = null;
            if (!associationType.IsAssignedPluralNameDefault)
            {
                this.pluralName = associationType.AssignedPluralName;
            }

            this.isMany = null;
            if (!associationType.IsIsManyDefault)
            {
                this.isMany = XmlConvert.ToString(associationType.IsMany);
            }
        }

        public void SyncToMeta(Repository repository, AssociationType associationType)
        {
            associationType.Reset();

            if (!associationType.ExistId)
            {
                associationType.Id = this.id;
            }

            if (this.objectType != null)
            {
                var domain = Domain.GetDomain(associationType);

                var associationObjectTypeId = new Guid(this.objectType.idRef);
                var associationObjectType = (ObjectType)domain.Domain.Find(associationObjectTypeId);

                if (associationObjectType == null)
                {
                    repository.AddUndefinedObjectType(this.objectType.idRef);
                    associationObjectType = domain.AddDeclaredObjectType(associationObjectTypeId);
                }

                associationType.ObjectType = associationObjectType;
            }

            associationType.AssignedSingularName = this.singularName;
            associationType.AssignedPluralName = this.pluralName;
            
            if (this.isMany != null)
            {
                associationType.IsMany = bool.Parse(this.isMany);
            }
        }
    }
}