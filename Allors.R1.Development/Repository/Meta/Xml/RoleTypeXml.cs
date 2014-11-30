// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleTypeXml.cs" company="Allors bvba">
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

    public class RoleTypeXml
    {
        [XmlAttribute("id")]
        public Guid id;
        
        public string isMany;
        
        [XmlElement("objectType")] 
        public IdrefXml objectType;
        
        public string pluralName;
        
        public string precision;
        
        public string scale;
        
        public string singularName;
        
        public string size;

        internal RoleTypeXml()
        {
        }

        public void SyncFromMeta(RoleType roleType)
        {
            this.id = roleType.Id;

            this.objectType = null;
            if (!roleType.IsObjectTypeDefault)
            {
                this.objectType = new IdrefXml(roleType.ObjectType.Id);
            }

            this.singularName = null;
            if (!roleType.IsAssignedSingularNameDefault)
            {
                this.singularName = roleType.AssignedSingularName;
            }

            this.pluralName = null;
            if (!roleType.IsAssignedPluralNameDefault)
            {
                this.pluralName = roleType.AssignedPluralName;
            }

            this.isMany = null;
            if (!roleType.IsIsManyDefault)
            {
                this.isMany = XmlConvert.ToString(roleType.IsMany);
            }

            this.size = null;
            if (!roleType.IsSizeDefault)
            {
                this.size = XmlConvert.ToString(roleType.Size);
            }

            this.precision = null;
            if (!roleType.IsPrecisionDefault)
            {
                this.precision = XmlConvert.ToString(roleType.Precision);
            }

            this.scale = null;
            if (!roleType.IsScaleDefault)
            {
                this.scale = XmlConvert.ToString(roleType.Scale);
            }
        }

        public void SyncToMeta(Repository repository, RoleType roleType)
        {
            var domain = Domain.GetDomain(roleType);

            roleType.Reset();

            if (!roleType.ExistId)
            {
                roleType.Id = this.id;
            }

            if (this.objectType != null)
            {
                var roleTypeId = new Guid(this.objectType.idRef);
                var roleObjectType = (ObjectType)domain.Domain.Find(roleTypeId);

                if (roleObjectType == null)
                {
                    repository.AddUndefinedObjectType(this.objectType.idRef);
                    roleObjectType = domain.AddDeclaredObjectType(roleTypeId);
                }

                roleType.ObjectType = roleObjectType;
            }

            roleType.AssignedSingularName = this.singularName;
            roleType.AssignedPluralName = this.pluralName;
            
            if (this.isMany != null)
            {
                roleType.IsMany = bool.Parse(this.isMany);
            }
            
            if (this.size != null)
            {
                roleType.Size = int.Parse(this.size);
            }
            
            if (this.precision != null)
            {
                roleType.Precision = int.Parse(this.precision);
            }
            
            if (this.scale != null)
            {
                roleType.Scale = int.Parse(this.scale);
            }
        }
    }
}