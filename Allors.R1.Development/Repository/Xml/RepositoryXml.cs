// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryXml.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Xml
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors.R1.Meta;

    [Serializable]
    [XmlRoot("repository", Namespace = "")]
    public class RepositoryXml
    {
        [XmlAttribute("allors")]
        public string allors;
        
        private FileInfo fileInfo;
        
        public string output;
        
        [XmlElement("super", typeof(SuperXml))]
        public ArrayList supers = new ArrayList();

        internal RepositoryXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected RepositoryXml()
        {
        }

        internal FileInfo FileInfo
        {
            get { return this.fileInfo; }
        }

        public void RemoveSuperDomainLocation(Guid partId)
        {
            var locationXml = this.LookupSuperDomainLocation(partId);
            if (locationXml != null)
            {
                this.supers.Remove(locationXml);
            }
        }

        internal void AddOrUpdateSuperDomainLocation(Guid partId, string path)
        {
            var locationXml = this.LookupSuperDomainLocation(partId);
            if (locationXml == null)
            {
                locationXml = new SuperXml(partId, path);
                this.supers.Add(locationXml);
            }

            this.supers.Sort();
        }

        internal void Delete()
        {
            this.fileInfo.Delete();
            if (this.fileInfo.Exists)
            {
                this.fileInfo.Delete();
            }
        }

        internal static RepositoryXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(RepositoryXml));
            RepositoryXml repositoryXml;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                repositoryXml = (RepositoryXml) xmlSerializer.Deserialize(textReader);
                repositoryXml.fileInfo = fileInfo;
                textReader.Close();
            }

            Repository.CheckAllorsVersion(repositoryXml.allors, Repository.FileName);

            return repositoryXml;
        }

        internal SuperXml LookupSuperDomainLocation(Guid partId)
        {
            foreach (SuperXml superDomain in this.supers)
            {
                if (superDomain.id  == partId)
                {
                    return superDomain;
                }
            }

            return null;
        }

        internal void Save()
        {
            this.allors = Domain.Version.ToString();

            this.fileInfo.Directory.Refresh();

            if (!this.fileInfo.Directory.Exists)
            {
                this.fileInfo.Directory.Create();
            }

            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);

            var xmlSerializer = new XmlSerializer(typeof (RepositoryXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }
    }
}