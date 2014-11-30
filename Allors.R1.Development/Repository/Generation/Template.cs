// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Template.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Generation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;
    using System.Text;
    using System.Windows.Forms.VisualStyles;

    using Allors.R1.Development.Repository.Generation.Xml;
    using Allors.R1.Meta;

    public class Template : RepositoryObject
    {
        internal const string TemplatesWildcard = Wildcard + TemplateExtension;
        internal const string StringTemplateExtension = ".stg";
        private const string Wildcard = "*";
        private const string TemplateExtension = ".template";

        private readonly Repository repository;
        private readonly TemplateXml xml;
        private readonly Settings settings = new Settings();

        private StringTemplate stringTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class. 
        /// Instantiate an existing <see cref="Template"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="fileInfo">
        /// The template Xml file
        /// </param>
        internal Template(Repository repository, FileInfo fileInfo)
        {
            this.repository = repository;

            fileInfo.Refresh();
            if (!fileInfo.Exists)
            {
                throw new Exception("Template not found");
            }

            this.xml = TemplateXml.Load(fileInfo);

            this.settings.Load(this.xml);

            Repository.CheckAllorsVersion(this.xml.allors, fileInfo.Name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class. 
        /// Creates a new <see cref="Template"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        internal Template(Repository repository)
        {
            this.repository = repository;

            var id = Guid.NewGuid().ToString().ToLower();

            var fileName = Path.Combine(repository.TemplatesDirectoryInfo.FullName, id + TemplateExtension);
            var fileInfo = new FileInfo(fileName);

            this.xml = new TemplateXml(fileInfo) { allors = Domain.Version.ToString() };

            this.settings.Save(this.xml);
            
            Repository.CheckAllorsVersion(this.xml.allors, fileInfo.Name);

            this.Save();
            this.Repository.SendChangedEvent(this);
        }

        public Domain Domain
        {
            get { return this.Repository.Domain; }
        }

        public Repository Repository
        {
            get { return this.repository; }
        }

        public StringTemplate StringTemplate
        {
            get
            {
                this.LazyLoadTemplate();
                return this.stringTemplate;
            }
        }

        public Settings Settings
        {
            get
            {
                return this.settings;
            }
        }

        public override Guid Id
        {
            get { return this.xml.Id; }
        }

        public string Name
        {
            get
            {
                return this.xml.name;
            }

            set
            {
                if (value != null && value.Trim().Length == 0)
                {
                    this.xml.name = null;
                }
                else
                {
                    this.xml.name = value;
                }

                this.Save();
                this.Repository.SendChangedEvent(this);
            }
        }

        public string Extension
        {
            get
            {
                return this.xml.extension;
            }

            set
            {
                if (value != null && value.Trim().Length == 0)
                {
                    this.xml.extension = null;
                }
                else
                {
                    this.xml.extension = value;
                }

                this.Save();
                this.Repository.SendChangedEvent(this);
            }
        }

        public string Output
        {
            get
            {
                return this.xml.output;
            }

            set
            {
                if (value != null && value.Trim().Length == 0)
                {
                    this.xml.output = null;
                }
                else
                {
                    this.xml.output = value;
                }

                this.Save();
                this.Repository.SendChangedEvent(this);
            }
        }

        public Uri Source
        {
            get
            {
                if (this.xml.source == null || this.xml.source.Trim().Length == 0)
                {
                    return null;
                }

                return new Uri(this.xml.source);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Template Url can not be null.");
                }

                StringTemplate.AssertIsValid(value);

                this.xml.source = value.ToString();

                if (this.stringTemplate != null)
                {
                    this.stringTemplate.Delete();
                    this.stringTemplate = null;
                }

                this.Save();
                this.LazyLoadTemplate();
                this.Repository.SendChangedEvent(this);
            }
        }

        public bool ExistExtension
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Extension);
            }
        }

        public void SendChangedEvent()
        {
            this.Save();
            this.repository.SendChangedEvent(this);
        }

        public override bool Equals(object obj)
        {
            var that = obj as Template;
            return that != null && this.Id.Equals(that.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override int CompareTo(object obj)
        {
            if (this.Name != null)
            {
                var that = obj as Template;
                if (that != null)
                {
                    return string.CompareOrdinal(this.Name, that.Name);
                }

                return 1;
            }

            return -1;
        }

        public void Generate(Log log)
        {
            var repositoryOutputDirectoryInfo = this.Repository.OutputDirectoryInfo;

            var relativeOutputDirectoryName = this.Name;
            if (!string.IsNullOrEmpty(this.Output))
            {
                relativeOutputDirectoryName = this.Output;
            }

            var outputDirectoryName = Path.Combine(repositoryOutputDirectoryInfo.FullName, relativeOutputDirectoryName);
            var outputDirectoryInfo = new DirectoryInfo(outputDirectoryName);

            var domain = this.Domain;

            if (this.ExistExtension)
            {
                Assembly assembly;

                try
                {
                    if (Path.IsPathRooted(this.Extension))
                    {
                        assembly = Assembly.LoadFrom(this.Extension);
                    }
                    else
                    {
                        var fullPath = Path.Combine(this.Repository.DirectoryInfo.FullName + this.Extension);
                        var fileInfo = new FileInfo(fullPath);
                        assembly = Assembly.LoadFrom(fileInfo.FullName);
                    }
                }
                catch (Exception e)
                {
                    log.Error(this, e.Message);
                    return;
                }

                var domainExtensionType = assembly.GetTypes().FirstOrDefault(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(IDomainExtension)));
                if (domainExtensionType == null)
                {
                    log.Error(this, "Could not find domain extension in assembly " + assembly.FullName);
                    return;
                }

                try
                {
                    var instancePropertyInfo = domainExtensionType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    var domainExtension = (IDomainExtension)instancePropertyInfo.GetValue(null, null);
                    domain = domainExtension.Domain;
                }
                catch (Exception e)
                {
                    var messageBuilder = new StringBuilder();

                    if (e.InnerException == null)
                    {
                        messageBuilder.Append(e.Message + "\n");
                        messageBuilder.Append(e.StackTrace);
                    }
                    else
                    {
                        while (e.InnerException != null)
                        {
                            messageBuilder.Append(e.InnerException.Message + "\n");
                            e = e.InnerException;
                        }

                        messageBuilder.Append(e.StackTrace + "\n");
                    }

                    messageBuilder.Append("\n");

                    log.Error(this, messageBuilder.ToString());
                    return;
                }
            }

            this.StringTemplate.Generate(domain, this.settings, outputDirectoryInfo, log);
        }

        public void Save()
        {
            this.Settings.Save(this.xml);
            this.xml.Save();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void UpdateTemplate()
        {
            this.Source = this.Source;
        }

        public void Delete()
        {
            this.xml.Delete();
            this.StringTemplate.Delete();

            this.Repository.RemoveTemplate(this);

            this.Repository.SendDeletedEvent(this.Id);
        }

        private void LazyLoadTemplate()
        {
            if (this.stringTemplate == null)
            {
                var fileName = Path.Combine(this.Repository.TemplatesDirectoryInfo.FullName, this.Id + StringTemplateExtension);
                var fileInfo = new FileInfo(fileName);

                this.stringTemplate = new StringTemplate(fileInfo, this.Source);
            }
        }
    }
}