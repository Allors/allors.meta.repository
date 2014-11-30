// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateXml.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
//   // 
//   // Dual Licensed under
//   //   a) the Lesser General Public Licence v3 (LGPL)
//   //   b) the Allors License
//   // 
//   // The LGPL License is included in the file lgpl.txt.
//   // The Allors License is an addendum to your contract.
//   // 
//   // Allors Platform is distributed in the hope that it will be useful,
//   // but WITHOUT ANY WARRANTY; without even the implied warranty of
//   // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   // GNU General Public License for more details.
//   // 
//   // For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.Generation.Xml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using Allors.R1.Meta;

    [Serializable]
    [XmlRoot("template", Namespace = "")]
    public class TemplateXml
    {
        [XmlAttribute("allors")] 
        public string allors;

        public string extension;
       
        public string name;

        public string output;

        [XmlElement("settings", typeof(SettingsXml))]
        public List<SettingsXml> settings = new List<SettingsXml>();

        public string source;

        [XmlElement("configuration", typeof(SettingsXml))]
        public ArrayList supers = new ArrayList();
 
        private FileInfo fileInfo;

        public TemplateXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected TemplateXml()
        {
        }

        public Guid Id
        {
            get
            {
                var id = Path.GetFileNameWithoutExtension(this.fileInfo.Name);
                return new Guid(id);
            }
        }

        internal static TemplateXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(TemplateXml));
            TemplateXml templateXml;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                templateXml = (TemplateXml)xmlSerializer.Deserialize(textReader);
                templateXml.fileInfo = fileInfo;
                textReader.Close();
            }

            return templateXml;
        }

        internal void Delete()
        {
            this.fileInfo.Refresh();
            if (this.fileInfo.Exists)
            {
                this.fileInfo.Delete();
            }
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

            var xmlSerializer = new XmlSerializer(typeof(TemplateXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }
    }
}