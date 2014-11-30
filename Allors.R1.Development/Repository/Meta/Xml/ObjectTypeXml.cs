// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeXml.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository.Xml;
    using Allors.R1.Meta;

    [Serializable]
    [XmlRoot("objectType", Namespace = "")]
    public class ObjectTypeXml
    {
        [XmlAttribute("allors")] 
        public string allors;

        [XmlAttribute("id")]
        public Guid id;
        
        public string isAbstract;
        
        public string isInterface;
       
        [XmlElement("namespace")]
        public IdrefXml nameSpace;
        
        public string pluralName;
        
        public string singularName;

        private FileInfo fileInfo;

        internal ObjectTypeXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected ObjectTypeXml()
        {
        }

        internal static ObjectTypeXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(ObjectTypeXml));
            ObjectTypeXml loadObjectType;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                loadObjectType = (ObjectTypeXml)xmlSerializer.Deserialize(textReader);
                loadObjectType.fileInfo = fileInfo;
                textReader.Close();
            }

            return loadObjectType;
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

            var xmlSerializer = new XmlSerializer(typeof(ObjectTypeXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }

        internal void SyncFromMeta(ObjectType objectType)
        {
            this.id = objectType.Id;
            this.singularName = null;
            this.pluralName = null;
            this.isAbstract = null;
            this.nameSpace = null;
            this.isInterface = null;

            this.singularName = null;
            if (!objectType.IsAssignedSingularNameDefault)
            {
                this.singularName = objectType.SingularName;
            }

            this.pluralName = null;
            if (!objectType.IsAssignedPluralNameDefault)
            {
                this.pluralName = objectType.PluralName;
            }

            this.isAbstract = null;
            if (!objectType.IsIsAbstractDefault)
            {
                this.isAbstract = null;
                if (objectType.ExistIsAbstract)
                {
                    this.isAbstract = XmlConvert.ToString(objectType.IsAbstract);
                }
            }

            this.isInterface = null;
            if (!objectType.IsIsInterfaceDefault)
            {
                this.isInterface = null;
                if (objectType.ExistIsInterface)
                {
                    this.isInterface = XmlConvert.ToString(objectType.IsInterface);
                }
            }
        }

        internal void SyncToMeta(Repository repository, ObjectType objectType)
        {
            Repository.CheckAllorsVersion(this.allors, this.fileInfo.Name);

            objectType.Reset();

            if (!objectType.ExistId)
            {
                objectType.Id = this.id;
            }

            objectType.SingularName = this.singularName;
            objectType.PluralName = this.pluralName;

            if (this.isAbstract != null)
            {
                objectType.IsAbstract = bool.Parse(this.isAbstract);
            }

            if (this.isInterface != null)
            {
                objectType.IsInterface = bool.Parse(this.isInterface);
            }
        }
    }
}