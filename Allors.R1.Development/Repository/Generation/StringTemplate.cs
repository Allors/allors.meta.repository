// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Template.cs" company="Allors bvba">
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
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.Generation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Xml;

    using Allors.R1.Meta;

    using Antlr4.StringTemplate;
    using Antlr4.StringTemplate.Misc;

    using Storage;

    //TODO: change name to context
    public class StringTemplate : IComparable
    {
        public static readonly string FileFilter = "Template files ( *.stg )| *.stg";
        
        private const string TemplateId = "TemplateId";
        private const string TemplateName = "TemplateName";
        private const string TemplateVersion = "TemplateVersion";
        private const string TemplateAllors = "TemplateAllors";
        private const string TemplateConfiguration = "TemplateConfiguration";

        private const string TemplateKey = "template";
        private const string DomainKey = "domain";
        private const string SettingsKey = "settings";
        private const string InputKey = "input";
        private const string OutputKey = "output";
        private const string GenerationKey = "generation";
        private const string InheritanceKey = "inheritance";
        private const string ObjectTypeKey = "objectType";
        private const string RelationTypeKey = "relationType";
        
        private readonly string allors;
        
        private readonly FileInfo fileInfo;
        private readonly Guid id;
        private readonly string name;
        private readonly string version;

        internal StringTemplate(FileInfo fileInfo, Uri templateUrl)
        {
            this.fileInfo = fileInfo;

            this.fileInfo.Refresh();
            if (!this.fileInfo.Exists)
            {
                try
                {
                    var templateContents = GetTemplateContents(templateUrl);
                    File.WriteAllText(this.fileInfo.FullName, templateContents);
                }
                catch
                {
                    throw new Exception("Could not download " + templateUrl);
                }
            }

            TemplateGroup group = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');
            
            this.name = Render(group, TemplateName);
            this.id = Render(group, TemplateId) != null ? new Guid(Render(group, TemplateId)) : Guid.Empty;
            this.allors = Render(group, TemplateAllors); 
            this.version = Render(group,TemplateVersion);
        }

        public string Allors
        {
            get { return this.allors; }
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Version
        {
            get { return this.version; }
        }

        public static bool IsValid(Uri templateSourceUri)
        {
            try
            {
                AssertIsValid(templateSourceUri);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public int CompareTo(object obj)
        {
            var that = obj as StringTemplate;
            if (that != null)
            {
                return string.CompareOrdinal(this.Name, that.Name);
            }

            return -1;
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal static void AssertIsValid(Uri templateUri)
        {
            var errors = new List<string>();

            var templateContents = GetTemplateContents(templateUri);
            TemplateGroup group = new TemplateGroupString("template", templateContents, '$', '$');

            if(string.IsNullOrWhiteSpace(Render(group,TemplateName)))
            {
                errors.Add("Missing property " + TemplateName);
            }

            if (string.IsNullOrWhiteSpace(Render(group, TemplateId)))
            {
                errors.Add("Missing property " + TemplateId);
            }
            else
            {
                Guid id;
                if (!Guid.TryParse(Render(group, TemplateId), out id))
                {
                    errors.Add("Property " + TemplateId + " is not a unique id");
                }
            }

            if (string.IsNullOrWhiteSpace(Render(group, TemplateAllors)))
            {
                errors.Add("Missing property " + TemplateAllors);
            }

            if (string.IsNullOrWhiteSpace(Render(group, TemplateVersion)))
            {
                errors.Add("Missing property " + TemplateVersion);
            }

            try
            {
                var configurationTemplate = group.GetInstanceOf(TemplateConfiguration);
                configurationTemplate.Add(DomainKey, Domain.Create());
                var configurationXml = new XmlDocument();
                configurationXml.LoadXml(configurationTemplate.Render());
            }
            catch
            {
                errors.Add("Missing or invalid " + TemplateConfiguration);
            }

            if (errors.Count > 0)
            {
                throw new Exception(String.Join("\n", errors));
            }
        }

        internal static string GetTemplateContents(Uri uri)
        {
            var webRequest = WebRequest.Create(uri);
            using (var webResponse = webRequest.GetResponse())
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        internal void Delete()
        {
            this.fileInfo.Refresh();
            this.fileInfo.Delete();
        }

        internal void Generate(Domain domain, Settings settings, DirectoryInfo outputDirectory, Log log)
        {
            //if (!domain.IsValid)
            //{
            //    log.Error(this, "Domain " + domain + " has validation errors.");
            //    return;
            //}

            try
            {
                Repository.CheckAllorsVersion(this.Allors, this.fileInfo.Name);
            }
            catch (Exception e)
            {
                log.Error(this, e.Message);
            }

            try
            {
                TemplateGroup group = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

                group.ErrorManager = new ErrorManager(new LogAdapter(log));
                
                var configurationTemplate = group.GetInstanceOf(TemplateConfiguration);
                configurationTemplate.Add(DomainKey, domain);
                configurationTemplate.Add(SettingsKey, settings.CreateIndexedObject());

                var configurationXml = new XmlDocument();
                configurationXml.LoadXml(configurationTemplate.Render());

                var location = new Location(outputDirectory);
                foreach (XmlElement generation in configurationXml.DocumentElement.SelectNodes(GenerationKey))
                {
                    var template = group.GetInstanceOf(generation.GetAttribute(TemplateKey));
                    var output = generation.GetAttribute(OutputKey);

                    template.Add(DomainKey, domain);
                    template.Add(SettingsKey, settings.CreateIndexedObject());
                    if (generation.HasAttribute(InputKey))
                    {
                        var input = new Guid(generation.GetAttribute(InputKey));
                        var objectType = domain.Domain.Find(input) as ObjectType;
                        if (objectType != null)
                        {
                            template.Add(ObjectTypeKey, objectType);
                        }
                        else
                        {
                            var relationType = domain.Domain.Find(input) as RelationType;
                            if (relationType != null)
                            {
                                template.Add(RelationTypeKey, relationType);
                            }
                            else
                            {
                                var inheritance = domain.Domain.Find(input) as Inheritance;
                                if (inheritance != null)
                                {
                                    template.Add(InheritanceKey, inheritance);
                                }
                                else
                                {
                                    throw new ArgumentException(input + " was not found");
                                }
                            }
                        }
                        //TODO: Super Domains
                    }

                    var result = template.Render();
                    location.Save(output, result);
                }
            }
            catch (Exception e)
            {
                log.Error(this, "Generation error : " + e.Message + "\n" + e.StackTrace);
            }
        }

        private static string Render(TemplateGroup templateGroup, string templateName)
        {
            var template = templateGroup.GetInstanceOf(templateName);
            if (template != null)
            {
                return template.Render();
            }

            return null;
        }

        private class LogAdapter : ITemplateErrorListener
        {
            private readonly Log log;

            public LogAdapter(Log log)
            {
                this.log = log;
            }

            public void CompiletimeError(TemplateMessage msg)
            {
                log.Error(msg, msg.ToString());
            }

            public void RuntimeError(TemplateMessage msg)
            {
                log.Error(msg, msg.ToString());
            }

            public void IOError(TemplateMessage msg)
            {
                log.Error(msg, msg.ToString());
            }

            public void InternalError(TemplateMessage msg)
            {
                log.Error(msg, msg.ToString());
            }
        }
    }
}