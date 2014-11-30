// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainXml.cs" company="Allors bvba">
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
    using System.Collections;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors.R1.Development.Repository.Xml;
    using Allors.R1.Meta;

    [Serializable]
    [XmlRoot("domain", Namespace = "")]
    public class DomainXml
    {
        [XmlAttribute("allors")]
        public string allors;
       
        [XmlAttribute("id")]
        public Guid id;
        
        public string name;
        
        [XmlElement("super", typeof (IdrefXml))]
        public ArrayList supers;

        private FileInfo fileInfo;

        internal DomainXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected DomainXml()
        {
        }

        public void Save()
        {
            this.allors = Domain.Version.ToString();

            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);

            var xmlSerializer = new XmlSerializer(typeof(DomainXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }

        internal static DomainXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(DomainXml));
            DomainXml domainXml;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                domainXml = (DomainXml)xmlSerializer.Deserialize(textReader);
                domainXml.fileInfo = fileInfo;
                textReader.Close();
            }

            return domainXml;
        }

        internal void Delete()
        {
            this.fileInfo.Delete();
        }

        internal void SyncFromMeta(Domain domain)
        {
            this.id = domain.Id;

            this.name = domain.Name;
            this.supers = new ArrayList();
            foreach (var parentDomain in domain.DirectSuperDomains)
            {
                if (!parentDomain.IsAllorsUnitDomain)
                {
                    this.supers.Add(new IdrefXml(parentDomain.Id.ToString()));
                }
            }

            this.supers.Sort();
        }

        internal void SyncToMeta(Domain domain)
        {
            Repository.CheckAllorsVersion(this.allors, "allors.domain");

            if (!domain.ExistId)
            {
                domain.Id = this.id;
            }

            domain.Name = this.name;
        }
    }
}