// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationTypeXml.cs" company="Allors bvba">
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
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors.R1.Meta;

    [Serializable]
    [XmlRoot("relationType", Namespace = "")]
    public class RelationTypeXml
    {
        [XmlAttribute("allors")] 
        public string allors;
        
        public AssociationTypeXml associationType;
      
        [XmlAttribute("id")] 
        public Guid id;

        public string isDerived;
        
        public string isIndexed;
        
        public RoleTypeXml roleType;

        private FileInfo fileInfo;

        internal RelationTypeXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected RelationTypeXml()
        {
        }

        internal static RelationTypeXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(RelationTypeXml));
            RelationTypeXml relationTypeXml;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                relationTypeXml = (RelationTypeXml)xmlSerializer.Deserialize(textReader);
                relationTypeXml.fileInfo = fileInfo;
                textReader.Close();
            }

            return relationTypeXml;
        }

        internal void Delete()
        {
            this.fileInfo.Delete();
        }

        internal void Save()
        {
            this.allors = Domain.Version.ToString();

            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);

            var xmlSerializer = new XmlSerializer(typeof(RelationTypeXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }

        internal void SyncFromMeta(RelationType relationType)
        {
            this.id = relationType.Id;

            this.isIndexed = null;
            if (!relationType.IsIsIndexedDefault)
            {
                this.isIndexed = XmlConvert.ToString(relationType.IsIndexed);
            }

            this.isDerived = null;
            if (!relationType.IsIsDerivedDefault)
            {
                this.isDerived = XmlConvert.ToString(relationType.IsDerived);
            }

            this.associationType = new AssociationTypeXml();
            this.associationType.SyncFromMeta(relationType.AssociationType);

            this.roleType = new RoleTypeXml();
            this.roleType.SyncFromMeta(relationType.RoleType);
        }

        internal void SyncToMeta(Repository repository, RelationType relationType)
        {
            Repository.CheckAllorsVersion(this.allors, this.fileInfo.Name);

            relationType.Reset();

            if (!relationType.ExistId)
            {
                relationType.Id = this.id;
            }

            if (this.isIndexed != null)
            {
                relationType.IsIndexed = bool.Parse(this.isIndexed);
            }

            if (this.isDerived != null)
            {
                relationType.IsDerived = bool.Parse(this.isDerived);
            }

            if (this.associationType != null)
            {
                this.associationType.SyncToMeta(repository, relationType.AssociationType);
            }

            if (this.roleType != null)
            {
                this.roleType.SyncToMeta(repository, relationType.RoleType);
            }
        }
    }
}