// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritanceXml.cs" company="Allors bvba">
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
    [XmlRoot("inheritance", Namespace = "")]
    public class InheritanceXml
    {
        [XmlAttribute("allors")]
        public string allors;
       
        [XmlAttribute("id")]
        public Guid id;
        
        public IdrefXml subtype;
        
        public IdrefXml supertype;

        private FileInfo fileInfo;

        internal InheritanceXml(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        protected InheritanceXml()
        {
        }

        internal static InheritanceXml Load(FileInfo fileInfo)
        {
            var xmlSerializer = new XmlSerializer(typeof(InheritanceXml));
            InheritanceXml loadInheritance;
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                loadInheritance = (InheritanceXml)xmlSerializer.Deserialize(textReader);
                loadInheritance.fileInfo = fileInfo;
                textReader.Close();
            }

            return loadInheritance;
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

            var xmlSerializer = new XmlSerializer(typeof(InheritanceXml));
            xmlSerializer.Serialize(xmlTextWriter, this);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringWriter.ToString());

            Repository.StripXml(xmlDocument);

            xmlDocument.Save(this.fileInfo.FullName);
        }

        internal void SyncFromMeta(Inheritance inheritance)
        {
            this.id = inheritance.Id;

            this.supertype = null;
            this.subtype = null;

            this.supertype = null;
            if (!inheritance.IsSupertypeDefault)
            {
                this.supertype = new IdrefXml(inheritance.Supertype.Id);
            }

            this.subtype = null;
            if (!inheritance.IsSubtypeDefault)
            {
                this.subtype = new IdrefXml(inheritance.Subtype.Id);
            }
        }

        internal void SyncToMeta(Repository repository, Inheritance inheritance)
        {
            Repository.CheckAllorsVersion(this.allors, this.fileInfo.Name);

            var domain = Domain.GetDomain(inheritance);

            inheritance.Reset();

            if (!inheritance.ExistId)
            {
                inheritance.Id = this.id;
            }

            if (this.subtype != null)
            {
                var subtypeId = new Guid(this.subtype.idRef);
                var inheritanceSubtype = (ObjectType)domain.Domain.Find(subtypeId);
                if (inheritanceSubtype == null)
                {
                    repository.AddUndefinedObjectType(this.subtype.idRef);
                    inheritanceSubtype = domain.AddDeclaredObjectType(subtypeId);
                }

                inheritance.Subtype = inheritanceSubtype;
            }

            if (this.supertype != null)
            {
                var supertypeId = new Guid(this.supertype.idRef);
                var inheritanceSupertype = (ObjectType)domain.Domain.Find(supertypeId);
                if (inheritanceSupertype == null)
                {
                    repository.AddUndefinedObjectType(this.supertype.idRef);
                    inheritanceSupertype = domain.AddDeclaredObjectType(supertypeId);
                }

                inheritance.Supertype = inheritanceSupertype;
            }
        }
    }
}