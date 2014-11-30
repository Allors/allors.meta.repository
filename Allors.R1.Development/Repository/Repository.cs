//------------------------------------------------------------------------------------------------- 
// <copyright file="Repository.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Allors.R1.Development.Repository.Commands;
    using Allors.R1.Development.Repository.Generation;
    using Allors.R1.Development.Repository.Meta;
    using Allors.R1.Development.Repository.Meta.Xml;
    using Allors.R1.Development.Repository.Storage;
    using Allors.R1.Development.Repository.Tasks;
    using Allors.R1.Development.Repository.Xml;
    using Allors.R1.Meta;
    using Allors.R1.Meta.Events;

    public class Repository : RepositoryObject
    {
        public const string FileFilter = "Repository files (" + FileName + ")|" + FileName;

        internal const string FileName = "allors.repository";

        private const string DefaultOutputDirectory = "../output";
        private const string DomainFileName = "allors.domain";
        private const string TemplatesDirectory = "templates";
        private const string Wildcard = "*";
        private const string ObjectTypeExtension = ".objectType";
        private const string ObjectTypesDirectory = "objectTypes";
        private const string ObjectTypeWildcard = Wildcard + ObjectTypeExtension;
        private const string InheritanceExtension = ".inheritance";
        private const string InheritancesDirectory = "inheritances";
        private const string InheritanceWildcard = Wildcard + InheritanceExtension;
        private const string RelationTypeExtension = ".relationType";
        private const string RelationTypesDirectory = "relationTypes";
        private const string RelationTypeWildcard = Wildcard + RelationTypeExtension;

        private DirectoryInfo objectTypesDirectoryInfo;
        private DirectoryInfo inheritancesDirectoryInfo;
        private DirectoryInfo relationsDirectoryInfo;

        private Dictionary<Guid, ObjectTypeXml> objectTypeXmlsById;
        private Dictionary<Guid, InheritanceXml> inheritanceXmlsById;
        private Dictionary<Guid, RelationTypeXml> relationTypeXmlsById;

        private FileInfo domainFileInfo;
        private Domain domain;
        private DomainXml domainXml;

        private Dictionary<Guid, Repository> repositoryBySuperId;

        private ArrayList unresolvedObjectTypeIds;
        private HashSet<RelationType> relationTypesWithNewIds;

        private List<Template> templates;

        private FileInfo fileInfo;
        private RepositoryXml xml;

        public Repository(DirectoryInfo directoryInfo, bool create = false)
        {
            this.DirectoryInfo = directoryInfo;
            this.Init(create);
        }

        public event EventHandler<RepositoryObjectChangedEventArgs> ObjectChanged;

        public event EventHandler<RepositoryObjectDeletedEventArgs> ObjectDeleted;

        public event EventHandler<RepositoryMetaObjectChangedEventArgs> MetaObjectChanged;

        public event EventHandler<RepositoryMetaObjectDeletedEventArgs> MetaObjectDeleted;

        public DirectoryInfo DirectoryInfo { get; set; }

        public Version Allors
        {
            get { return new Version(this.xml.allors); }
        }

        public override Guid Id
        {
            get
            {
                return this.Domain.Id;
            }
        }

        public Domain Domain
        {
            get { return this.domain; }
        }

        public DirectoryInfo TemplatesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, TemplatesDirectory)); }
        }

        public DirectoryInfo OutputDirectoryInfo
        {
            get
            {
                var directory = this.xml.output ?? DefaultOutputDirectory;

                return new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, directory));
            }

            set
            {
                this.xml.output = value == null ? null : value.ToString();

                this.xml.Save();
                this.SendChangedEvent(this);
            }
        }
        
        public string[] UnresolvedTypeIds
        {
            get { return (string[])this.unresolvedObjectTypeIds.ToArray(typeof(string)); }
        }

        public Template[] Templates
        {
            get
            {
                return this.templates.ToArray();
            }
        }

        public void Reload()
        {
            this.Init(false);
        }

        public Template AddTemplate()
        {
            var template = new Template(this);
            this.templates.Add(template);
            return template;
        }

        public Domain AddSuper(DirectoryInfo directoryInfo)
        {
            var superRepository = new Repository(directoryInfo);
            var superDomain = this.AddSuper(superRepository);

            this.Init(false);

            return superDomain;
        }

        public void RemoveSuper(Domain superDomainToRemove)
        {
            Repository repositoryToRemove;
            if (this.repositoryBySuperId.TryGetValue(superDomainToRemove.Id, out repositoryToRemove))
            {
                this.domain.RemoveDirectSuperDomain(superDomainToRemove);

                this.repositoryBySuperId.Remove(superDomainToRemove.Id);
                this.domainXml.SyncFromMeta(this.domain);
                this.domainXml.Save();

                this.xml.RemoveSuperDomainLocation(superDomainToRemove.Id);
                this.xml.Save();

                this.Init(false);
            }
        }

        public PullUpObjectType PullUp(Domain toSuperDomain, ObjectType objectTypeToPull, bool includeContainedItems)
        {
            return new PullUpObjectType(this, toSuperDomain, objectTypeToPull, includeContainedItems);
        }

        public PullUpInheritance PullUp(Domain toSuperDomain, Inheritance inheritanceToPull)
        {
            return new PullUpInheritance(this, toSuperDomain, inheritanceToPull);
        }

        public PullUpRelationType PullUp(Domain toSuperDomain, RelationType relationTypeToPull)
        {
            return new PullUpRelationType(this, toSuperDomain, relationTypeToPull);
        }

        public PushDownObjectType PushDown(ObjectType objectTypeToPush)
        {
            return new PushDownObjectType(this, objectTypeToPush);
        }

        public PushDownInheritance PushDown(Inheritance inheritanceToPush)
        {
            return new PushDownInheritance(this, inheritanceToPush);
        }

        public PushDownRelationType PushDown(RelationType relationTypeToPush)
        {
            return new PushDownRelationType(this, relationTypeToPush);
        }

        public void UpdateTemplates()
        {
            foreach (var template in this.Templates)
            {
                template.UpdateTemplate();
            }
        }

        public override int CompareTo(object obj)
        {
            if (this.Domain != null)
            {
                var that = obj as Repository;
                if (that != null)
                {
                    return this.Domain.CompareTo(that.Domain);
                }

                return 1;
            }

            return -1;
        }

        public override bool Equals(object obj)
        {
            var that = obj as Repository;
            return that != null && this.DirectoryInfo != null && that.DirectoryInfo != null && this.DirectoryInfo.FullName.Equals(that.DirectoryInfo.FullName);
        }

        public override int GetHashCode()
        {
            return this.DirectoryInfo.FullName.GetHashCode();
        }
        
        internal static void CheckAllorsVersion(string allors, string fileName)
        {
            if (string.IsNullOrEmpty(allors))
            {
                throw new ArgumentException(fileName + " has no Allors version");
            }

            if (!Domain.Version.Equals(new Version(allors)))
            {
                throw new ArgumentException(fileName + " has incompatible Allors version");
            }
        }

        internal static void StripXml(XmlDocument xmlDocument)
        {
            if (xmlDocument.FirstChild.NodeType.Equals(XmlNodeType.XmlDeclaration))
            {
                xmlDocument.RemoveChild(xmlDocument.FirstChild);
            }

            if (xmlDocument.DocumentElement != null)
            {
                var allorsAttributeValue = xmlDocument.DocumentElement.GetAttribute("allors");
                var idAttributeValue = xmlDocument.DocumentElement.GetAttribute("id");
                var versionAttributeValue = xmlDocument.DocumentElement.GetAttribute("version");
                xmlDocument.DocumentElement.Attributes.RemoveAll();
                if (!string.IsNullOrEmpty(allorsAttributeValue))
                {
                    xmlDocument.DocumentElement.SetAttribute("allors", allorsAttributeValue);
                }

                if (!string.IsNullOrEmpty(idAttributeValue))
                {
                    xmlDocument.DocumentElement.SetAttribute("id", idAttributeValue);
                }

                if (!string.IsNullOrEmpty(versionAttributeValue))
                {
                    xmlDocument.DocumentElement.SetAttribute("version", versionAttributeValue);
                }
            }
        }

        internal void SendChangedEvent(RepositoryObject repositoryObject)
        {
            var repositoryObjectChanged = this.ObjectChanged;
            if (repositoryObjectChanged != null)
            {
                repositoryObjectChanged(this, new RepositoryObjectChangedEventArgs(this, repositoryObject));
            }
        }

        internal void SendDeletedEvent(Guid repositoryObjectId)
        {
            var repositoryObjectDeleted = this.ObjectDeleted;
            if (repositoryObjectDeleted != null)
            {
                repositoryObjectDeleted(this, new RepositoryObjectDeletedEventArgs(this, repositoryObjectId));
            }
        }

        internal void AddUndefinedObjectType(string objectTypeId)
        {
            this.unresolvedObjectTypeIds.Add(objectTypeId);
        }

        internal void RemoveTemplate(Template template)
        {
            this.templates.Remove(template);
        }

        internal void Generate(int phase, GenerateLog log)
        {
            foreach (var template in this.templates)
            {
                switch (phase)
                {
                    case 1:
                        if (!template.ExistExtension)
                        {
                            template.Generate(log);
                        }

                        break;
                    case 2:
                        if (template.ExistExtension)
                        {
                            template.Generate(log);
                        }

                        break;
                    default:
                        throw new NotSupportedException("Phase must be either 1 or 2");
                }
            }
        }

        internal void Generate(string name, GenerateLog log)
        {
            foreach (var template in this.templates)
            {
                if (name.Equals(template.Name))
                {
                    template.Generate(log);
                }
            }
        }

        private void DomainMetaObjectChanged(object sender, MetaObjectChangedEventArgs args)
        {
            var changedDomain = args.MetaObject as Domain;
            if (changedDomain != null)
            {
                this.DomainChanged(changedDomain);
            }

            var changedObjectType = args.MetaObject as ObjectType;
            if (changedObjectType != null)
            {
                this.ObjectTypeChanged(changedObjectType);
            }

            var changedInheritance = args.MetaObject as Inheritance;
            if (changedInheritance != null)
            {
                this.InheritanceChanged(changedInheritance);
            }

            var changedRelationType = args.MetaObject as RelationType;
            if (changedRelationType != null)
            {
                this.RelationTypeChanged(changedRelationType);
            }

            var metaObjectChanged = this.MetaObjectChanged;
            if (metaObjectChanged != null)
            {
                metaObjectChanged(this, new RepositoryMetaObjectChangedEventArgs(this, args.MetaObject));
            }
        }

        private void DomainChanged(Domain changedDomain)
        {
            if (this.domain.Equals(changedDomain))
            {
                if (this.domainXml == null)
                {
                    this.domainXml = new DomainXml(this.domainFileInfo);
                }

                this.domainXml.SyncFromMeta(this.domain);
                this.domainXml.Save();
            }
        }

        private void ObjectTypeChanged(ObjectType changedObjectType)
        {
            if (this.domain.Equals(changedObjectType.DomainWhereDeclaredObjectType))
            {
                if (!this.objectTypeXmlsById.ContainsKey(changedObjectType.Id))
                {
                    var path = Path.Combine(this.objectTypesDirectoryInfo.FullName, changedObjectType.Id + ObjectTypeExtension);
                    var objectTypeFileInfo = new FileInfo(path);
                    this.objectTypeXmlsById[changedObjectType.Id] = new ObjectTypeXml(objectTypeFileInfo);
                }

                this.objectTypeXmlsById[changedObjectType.Id].SyncFromMeta(changedObjectType);
                this.objectTypeXmlsById[changedObjectType.Id].Save();

                // remove from super domain repositories
                foreach (var repository in this.repositoryBySuperId.Values)
                {
                    if (repository.objectTypeXmlsById.ContainsKey(changedObjectType.Id))
                    {
                        repository.Domain.Domain.Find(changedObjectType.Id).Delete();
                    }
                }
            }
            else
            {
                var superDomainRepository = this.repositoryBySuperId[changedObjectType.DomainWhereDeclaredObjectType.Id];
                var superDomainDomain = superDomainRepository.Domain;
                var superDomainObjectType = superDomainDomain.AddDeclaredObjectType(changedObjectType.Id);
                superDomainObjectType.Copy(changedObjectType);
                superDomainObjectType.SendChangedEvent();

                // remove from this repository
                this.Delete(changedObjectType.Id);

                // remove from other super domain repositories
                foreach (var repository in this.repositoryBySuperId.Values)
                {
                    if (!repository.Equals(superDomainRepository) && repository.objectTypeXmlsById.ContainsKey(changedObjectType.Id))
                    {
                        repository.Domain.Domain.Find(changedObjectType.Id).Delete();
                    }
                }
            }
        }

        private void InheritanceChanged(Inheritance changedInheritance)
        {
            if (this.domain.Equals(changedInheritance.DomainWhereDeclaredInheritance))
            {
                if (!this.inheritanceXmlsById.ContainsKey(changedInheritance.Id))
                {
                    var path = Path.Combine(this.inheritancesDirectoryInfo.FullName, changedInheritance.Id + InheritanceExtension);
                    var inheritanceFileInfo = new FileInfo(path);
                    this.inheritanceXmlsById[changedInheritance.Id] = new InheritanceXml(inheritanceFileInfo);
                }

                this.inheritanceXmlsById[changedInheritance.Id].SyncFromMeta(changedInheritance);
                this.inheritanceXmlsById[changedInheritance.Id].Save();

                // remove from super domain repositories
                foreach (var superDomainRepository in this.repositoryBySuperId.Values)
                {
                    if (superDomainRepository.inheritanceXmlsById.ContainsKey(changedInheritance.Id))
                    {
                        superDomainRepository.Domain.Domain.Find(changedInheritance.Id).Delete();
                    }
                }
            }
            else
            {
                var superDomainRepository = this.repositoryBySuperId[changedInheritance.DomainWhereDeclaredInheritance.Id];
                var superDomainDomain = superDomainRepository.Domain;
                var superDomainInheritance = superDomainDomain.AddDeclaredInheritance(changedInheritance.Id);
                superDomainInheritance.Copy(changedInheritance);
                superDomainInheritance.SendChangedEvent();

                // remove from this repository
                this.Delete(changedInheritance.Id);

                // remove from other super domain repositories
                foreach (var repository in this.repositoryBySuperId.Values)
                {
                    if (!repository.Equals(superDomainRepository) && repository.inheritanceXmlsById.ContainsKey(changedInheritance.Id))
                    {
                        repository.Domain.Domain.Find(changedInheritance.Id).Delete();
                    }
                }
            }
        }

        private void RelationTypeChanged(RelationType changedRelationType)
        {
            if (this.domain.Equals(changedRelationType.DomainWhereDeclaredRelationType))
            {
                if (!this.relationTypeXmlsById.ContainsKey(changedRelationType.Id))
                {
                    var path = Path.Combine(this.relationsDirectoryInfo.FullName, changedRelationType.Id + RelationTypeExtension);
                    var relationTypeFileInfo = new FileInfo(path);
                    this.relationTypeXmlsById[changedRelationType.Id] = new RelationTypeXml(relationTypeFileInfo);
                }

                this.relationTypeXmlsById[changedRelationType.Id].SyncFromMeta(changedRelationType);
                this.relationTypeXmlsById[changedRelationType.Id].Save();

                // remove from super domain repositories
                foreach (var superDomainRepository in this.repositoryBySuperId.Values)
                {
                    if (superDomainRepository.relationTypeXmlsById.ContainsKey(changedRelationType.Id))
                    {
                        superDomainRepository.Domain.Domain.Find(changedRelationType.Id).Delete();
                    }
                }
            }
            else
            {
                var superDomainRepository = this.repositoryBySuperId[changedRelationType.DomainWhereDeclaredRelationType.Id];
                var superDomainDomain = superDomainRepository.Domain;
                var superDomainRelationType = superDomainDomain.AddDeclaredRelationType(changedRelationType.Id, changedRelationType.AssociationType.Id, changedRelationType.RoleType.Id);
                superDomainRelationType.Copy(changedRelationType);
                superDomainRelationType.SendChangedEvent();

                // remove from this repository
                this.Delete(changedRelationType.Id);

                // remove from other super domain repositories
                foreach (var repository in this.repositoryBySuperId.Values)
                {
                    if (!repository.Equals(superDomainRepository) && repository.relationTypeXmlsById.ContainsKey(changedRelationType.Id))
                    {
                        repository.Domain.Domain.Find(changedRelationType.Id).Delete();
                    }
                }
            }
        }
        
        private void DomainMetaObjectDeleted(object sender, MetaObjectDeletedEventArgs args)
        {
            var deletedId = args.MetaObjectId;

            this.Delete(deletedId);

            var metaObjectDeleted = this.MetaObjectDeleted;
            if (metaObjectDeleted != null)
            {
                metaObjectDeleted(this, new RepositoryMetaObjectDeletedEventArgs(this, deletedId));
            }
        }

        private void Delete(Guid deletedId)
        {
            ObjectTypeXml objectTypeXml;
            if (this.objectTypeXmlsById.TryGetValue(deletedId, out objectTypeXml))
            {
                this.objectTypeXmlsById[deletedId].Delete();
                this.objectTypeXmlsById.Remove(deletedId);
            }

            InheritanceXml inheritanceXml;
            if (this.inheritanceXmlsById.TryGetValue(deletedId, out inheritanceXml))
            {
                this.inheritanceXmlsById[deletedId].Delete();
                this.inheritanceXmlsById.Remove(deletedId);
            }

            RelationTypeXml relationTypeXml;
            if (this.relationTypeXmlsById.TryGetValue(deletedId, out relationTypeXml))
            {
                this.relationTypeXmlsById[deletedId].Delete();
                this.relationTypeXmlsById.Remove(deletedId);
            }
        }

        private Domain AddSuper(Repository superDomainRepository)
        {
            if (superDomainRepository.Equals(this))
            {
                throw new ArgumentException("Super domain repository must be a different repository");
            }

            this.repositoryBySuperId.Add(superDomainRepository.Domain.Id, superDomainRepository);
            var superDomain = this.domain.Inherit(superDomainRepository.domain);

            this.domainXml.SyncFromMeta(this.domain);
            this.domainXml.Save();

            var superDomainId = superDomainRepository.domainXml.id;

            var superDomainDirectoryInfo = new AllorsDirectoryInfo(superDomainRepository.DirectoryInfo);
            var superDomainPath = superDomainDirectoryInfo.GetRelativeOrFullName(this.DirectoryInfo);

            this.xml.AddOrUpdateSuperDomainLocation(superDomainId, superDomainPath);
            this.xml.Save();

            this.domain.SendChangedEvent();

            return superDomain;
        }

        private void Init(bool create)
        {
            this.DirectoryInfo.Refresh();

            this.repositoryBySuperId = new Dictionary<Guid, Repository>();

            this.unresolvedObjectTypeIds = new ArrayList();

            this.relationTypesWithNewIds = new HashSet<RelationType>();

            this.fileInfo = new FileInfo(Path.Combine(this.DirectoryInfo.FullName, FileName));
            this.fileInfo.Refresh();

            if (create)
            {
                if (!this.DirectoryInfo.Exists)
                {
                    this.DirectoryInfo.Create();
                }

                if (this.fileInfo.Exists)
                {
                    throw new Exception("Repository already exists");
                }

                this.xml = new RepositoryXml(this.fileInfo);
                this.xml.Save();
            }
            else
            {
                if (!this.fileInfo.Exists)
                {
                    throw new Exception("Repository not found in location " + this.fileInfo.Directory);
                }

                this.xml = RepositoryXml.Load(this.fileInfo);
            }

            if (Domain.Version.Major != this.Allors.Major && Domain.Version.Minor != this.Allors.Minor)
            {
                throw new Exception("Incompatible version. Excpected " + Domain.Version + ", but was " + this.Allors);
            }

            this.objectTypesDirectoryInfo = new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, ObjectTypesDirectory));
            this.objectTypesDirectoryInfo.Refresh();
            if (!this.objectTypesDirectoryInfo.Exists)
            {
                this.objectTypesDirectoryInfo.Create();
            }

            this.inheritancesDirectoryInfo = new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, InheritancesDirectory));
            this.inheritancesDirectoryInfo.Refresh();
            if (!this.inheritancesDirectoryInfo.Exists)
            {
                this.inheritancesDirectoryInfo.Create();
            }

            this.relationsDirectoryInfo = new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, RelationTypesDirectory));
            this.relationsDirectoryInfo.Refresh();
            if (!this.relationsDirectoryInfo.Exists)
            {
                this.relationsDirectoryInfo.Create();
            }

            this.domainFileInfo = new FileInfo(Path.Combine(this.DirectoryInfo.FullName, DomainFileName));
            this.domainFileInfo.Refresh();

            if (this.domainFileInfo.Exists)
            {
                this.domainXml = DomainXml.Load(this.domainFileInfo);

                this.domain = Domain.Create(this.domainXml.id);

                this.domainXml.SyncToMeta(this.domain);

                foreach (IdrefXml partIdRef in this.domainXml.supers)
                {
                    var superDomainId = new Guid(partIdRef.idRef);
                    var superDomainLocationXml = this.xml.LookupSuperDomainLocation(superDomainId);
                    if (superDomainLocationXml == null)
                    {
                        throw new ArgumentException("No location for super domain with id " + superDomainId);
                    }

                    var superDomainDirectoryInfo = new DirectoryInfo(Path.Combine(this.DirectoryInfo.FullName, superDomainLocationXml.location));
                    var repository = new Repository(superDomainDirectoryInfo);
                    this.AddSuper(repository);
                }
            }
            else
            {
                this.domain = Domain.Create();
            }

            this.inheritanceXmlsById = new Dictionary<Guid, InheritanceXml>();
            this.objectTypeXmlsById = new Dictionary<Guid, ObjectTypeXml>();
            this.relationTypeXmlsById = new Dictionary<Guid, RelationTypeXml>();

            // first create ObjectTypes
            foreach (var typeFileInfo in this.objectTypesDirectoryInfo.GetFiles(ObjectTypeWildcard))
            {
                var objectTypeXml = ObjectTypeXml.Load(typeFileInfo);
                this.objectTypeXmlsById[objectTypeXml.id] = objectTypeXml;

                this.domain.AddDeclaredObjectType(objectTypeXml.id);
            }

            // then sync ObjectTypes
            foreach (var objectTypeXml in this.objectTypeXmlsById.Values)
            {
                var contentsObjectType = (ObjectType)this.domain.Domain.Find(objectTypeXml.id);
                objectTypeXml.SyncToMeta(this, contentsObjectType);
            }

            foreach (var inheritanceFileInfo in this.inheritancesDirectoryInfo.GetFiles(InheritanceWildcard))
            {
                var inheritanceXml = InheritanceXml.Load(inheritanceFileInfo);
                this.inheritanceXmlsById[inheritanceXml.id] = inheritanceXml;

                var inheritance = this.domain.AddDeclaredInheritance(inheritanceXml.id);
                inheritanceXml.SyncToMeta(this, inheritance);
            }

            // first create RelationTypes
            foreach (var relationFileInfo in this.relationsDirectoryInfo.GetFiles(RelationTypeWildcard))
            {
                var relationTypeXml = RelationTypeXml.Load(relationFileInfo);
                this.relationTypeXmlsById[relationTypeXml.id] = relationTypeXml;

                var associationTypeId = Guid.Empty;
                var roleTypeId = Guid.Empty;

                if (relationTypeXml.associationType != null)
                {
                    associationTypeId = relationTypeXml.associationType.id;
                }

                if (relationTypeXml.roleType != null)
                {
                    roleTypeId = relationTypeXml.roleType.id;
                }

                var newIds = false;

                if (associationTypeId == Guid.Empty)
                {
                    associationTypeId = Guid.NewGuid();
                    newIds = true;
                }

                if (roleTypeId == Guid.Empty)
                {
                    roleTypeId = Guid.NewGuid();
                    newIds = true;
                }

                var relationType = this.domain.AddDeclaredRelationType(relationTypeXml.id, associationTypeId, roleTypeId);

                if (newIds)
                {
                    this.relationTypesWithNewIds.Add(relationType);
                }
            }

            // then sync RelationTypes
            foreach (var relationTypeXml in this.relationTypeXmlsById.Values)
            {
                var contentsRelationType = (RelationType)this.domain.Domain.Find(relationTypeXml.id);
                relationTypeXml.SyncToMeta(this, contentsRelationType);
            }

            this.templates = new List<Template>();

            if (!this.TemplatesDirectoryInfo.Exists)
            {
                this.TemplatesDirectoryInfo.Create();
            }

            foreach (var configurationFileInfo in this.TemplatesDirectoryInfo.GetFiles(Template.TemplatesWildcard))
            {
                var template = new Template(this, configurationFileInfo);
                this.templates.Add(template);
            }

            this.domain.MetaObjectChanged += this.DomainMetaObjectChanged;
            this.domain.MetaObjectDeleted += this.DomainMetaObjectDeleted;

            // set defaults
            if (!this.domain.ExistId)
            {
                this.domain.Id = Guid.NewGuid();
            }

            this.domain.SendChangedEvent();
        }
    }
}