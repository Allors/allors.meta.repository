// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VersioningTest
    {
        private DirectoryInfo directoryInfo;
        private List<string> generationErrors;
        private Repository repository;

        private DirectoryInfo TemplatesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "templates")); }
        }

        private DirectoryInfo InheritancesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "inheritances")); }
        }

        private DirectoryInfo ObjectTypesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "objectTypes")); }
        }

        private DirectoryInfo RelationTypesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "relationTypes")); }
        }

        [SetUp]
        public void SetUp()
        {
            this.generationErrors = new List<string>();

            this.directoryInfo = new DirectoryInfo("domain");
            this.directoryInfo.DeleteRecursive();

            this.directoryInfo.Create();

            this.repository = new Repository(this.directoryInfo, true);
        }

        [Test]
        public void DomainIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            foreach (var version in versions)
            {
                var domainXmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));

                var domainXml = @"
<domain allors=""" + version + @""" id=""" + Guid.NewGuid() + @""">
    <name>Test</name>
</domain>";
                File.WriteAllText(domainXmlFileInfo.FullName, domainXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                    Assert.Fail();
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual("allors.domain has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void DomainNoVersion()
        {
            var domainXmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.domain"));

            var domainXml = @"
<domain id=""" + Guid.NewGuid() + @""">
    <name>Test</name>
</domain>";
            File.WriteAllText(domainXmlFileInfo.FullName, domainXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual("allors.domain has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void TemplateIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            foreach (var version in versions)
            {
                var templateXmlFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, "e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template".ToLower()));

                var templateXml = @"
<template allors=""" + version + @""">
    <name>MyTemplate</name>
    <outputPath>prefix</outputPath>
</template>
";
                File.WriteAllText(templateXmlFileInfo.FullName, templateXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual("e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void TemplateNoVersion()
        {
            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            var templateXmlFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, "e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template".ToLower()));

            const string TemplateXml = @"
<template>
    <name>MyTemplate</name>
    <outputPath>prefix</outputPath>
</template>
";
            File.WriteAllText(templateXmlFileInfo.FullName, TemplateXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual("e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void InheritanceIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            var domain = this.repository.Domain;
            domain.Name = "MySuperDomain";

            var abstractClassId = Guid.NewGuid();
            var abstractClass = domain.AddDeclaredObjectType(abstractClassId);
            abstractClass.SingularName = "AbstractClass";
            abstractClass.PluralName = "AbstractClasses";
            abstractClass.IsAbstract = true;

            var concreteClassId = Guid.NewGuid();
            var concreteClass = domain.AddDeclaredObjectType(concreteClassId);
            concreteClass.SingularName = "ConcreteClass";
            concreteClass.PluralName = "ConcreteClasses";

            Assert.IsTrue(domain.IsValid);

            abstractClass.SendChangedEvent();
            concreteClass.SendChangedEvent();
            domain.SendChangedEvent();

            var inheritanceId = Guid.NewGuid();

            foreach (var version in versions)
            {
                var inheritanceXmlFileInfo = new FileInfo(Path.Combine(this.InheritancesDirectoryInfo.FullName, inheritanceId + ".inheritance"));

                var inheritanceXml =
                    @"<inheritance id=""" + inheritanceId + @""" allors=""" + version + @""">
    <subtype idref=""" + concreteClassId + @"""/>
    <supertype idref=""" + abstractClassId + @"""/>
</inheritance>";
                File.WriteAllText(inheritanceXmlFileInfo.FullName, inheritanceXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual(inheritanceId + ".inheritance has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void InheritanceNoVersion()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var abstractClassId = Guid.NewGuid();
            var abstractClass = domain.AddDeclaredObjectType(abstractClassId);
            abstractClass.SingularName = "AbstractClass";
            abstractClass.PluralName = "AbstractClasses";
            abstractClass.IsAbstract = true;

            var concreteClassId = Guid.NewGuid();
            var concreteClass = domain.AddDeclaredObjectType(concreteClassId);
            concreteClass.SingularName = "ConcreteClass";
            concreteClass.PluralName = "ConcreteClasses";

            Assert.IsTrue(domain.IsValid);

            abstractClass.SendChangedEvent();
            concreteClass.SendChangedEvent();
            domain.SendChangedEvent();

            var inheritanceId = Guid.NewGuid();

            var inheritanceXmlFileInfo = new FileInfo(Path.Combine(this.InheritancesDirectoryInfo.FullName, inheritanceId + ".inheritance"));

            var inheritanceXml =
@"<inheritance id=""" + inheritanceId + @""">
    <subtype idref=""" + concreteClassId + @"""/>
    <supertype idref=""" + abstractClassId + @"""/>
</inheritance>";
            File.WriteAllText(inheritanceXmlFileInfo.FullName, inheritanceXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual(inheritanceId + ".inheritance has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void RelationIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            var domain = this.repository.Domain;
            domain.Name = "MySuperDomain";

            domain.SendChangedEvent();

            var relationId = Guid.NewGuid();

            foreach (var version in versions)
            {
                var relationXmlFileInfo = new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationId + ".relationType"));

                var relationXml = @"<relationType id=""" + relationId + @""" allors=""" + version + @"""/>";
                File.WriteAllText(relationXmlFileInfo.FullName, relationXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual(relationId + ".relationType has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void RelationNoVersion()
        {
            var domain = this.repository.Domain;
            domain.Name = "MySuperDomain";

            domain.SendChangedEvent();

            var relationId = Guid.NewGuid();

            var relationXmlFileInfo = new FileInfo(Path.Combine(this.RelationTypesDirectoryInfo.FullName, relationId + ".relationType"));

            var relationXml = @"<relationType id=""" + relationId + @"""/>";
            File.WriteAllText(relationXmlFileInfo.FullName, relationXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual(relationId + ".relationType has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void RepositoryIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            foreach (var version in versions)
            {
                var repositoryXmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.repository"));

                var repositoryXml = @"<repository allors=""" + version + @""" />";
                File.WriteAllText(repositoryXmlFileInfo.FullName, repositoryXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual("allors.repository has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void RepositoryNoVersion()
        {
            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            var repositoryXmlFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.repository"));

            const string RepositoryXml = @" <repository />";
            File.WriteAllText(repositoryXmlFileInfo.FullName, RepositoryXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual("allors.repository has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void StringTemplateIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            var template = this.repository.AddTemplate();
            template.Name = "MyTemplate";

            foreach (var version in versions)
            {
                this.generationErrors.Clear();

                var templateFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, template.Id + ".stg"));

                var templateSource = @"group TestTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0>>
TemplateAllors() ::= <<" + version + @">>
TemplateConfiguration(domain,settings) ::= <<
<generations/>
>>
";
                File.WriteAllText(templateFileInfo.FullName, templateSource);

                var duplicate = new Repository(this.directoryInfo);
                duplicate.Templates[0].Generate(new VersioningTestLog(this));

                Assert.AreEqual(1, this.generationErrors.Count);
                Assert.AreEqual(template.Id + ".stg has incompatible Allors version", this.generationErrors[0]);
            }
        }

        [Test]
        public void StringTemplateNoVersion()
        {
            this.repository.Domain.Name = "Domain";
            this.repository.Domain.SendChangedEvent();

            var template = this.repository.AddTemplate();
            template.Name = "MyTemplate";
 
            var templateFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, template.Id + ".stg"));

            var templateSource = @"group TestTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0>>
TemplateConfiguration(domain,settings) ::= <<
<generations/>
>>
";
            File.WriteAllText(templateFileInfo.FullName, templateSource);

            var duplicate = new Repository(this.directoryInfo);
            duplicate.Templates[0].Generate(new VersioningTestLog(this));

            Assert.AreEqual(1, this.generationErrors.Count);
            Assert.AreEqual(template.Id + ".stg has no Allors version", this.generationErrors[0]);
        }

        [Test]
        public void TypeIncompatibleVersion()
        {
            string[] versions = { "0.9", "1.1", "1.0.0", "1.0.1" };

            var domain = this.repository.Domain;
            domain.Name = "MySuperDomain";

            domain.SendChangedEvent();

            var typeId = Guid.NewGuid();

            foreach (var version in versions)
            {
                var typeXmlFileInfo = new FileInfo(Path.Combine(this.ObjectTypesDirectoryInfo.FullName, typeId + ".objectType"));

                var typeXml = @"<objectType id=""" + typeId + @""" allors=""" + version + @"""/>";
                File.WriteAllText(typeXmlFileInfo.FullName, typeXml);

                var exceptionThrown = false;
                try
                {
                    new Repository(this.directoryInfo);
                }
                catch (ArgumentException e)
                {
                    exceptionThrown = true;
                    Assert.AreEqual(typeId + ".objectType has incompatible Allors version", e.Message);
                }

                Assert.IsTrue(exceptionThrown);
            }
        }

        [Test]
        public void TypeNoVersion()
        {
            var domain = this.repository.Domain;
            domain.Name = "MySuperDomain";

            domain.SendChangedEvent();

            var typeId = Guid.NewGuid();

            var typeXmlFileInfo = new FileInfo(Path.Combine(this.ObjectTypesDirectoryInfo.FullName, typeId + ".objectType"));

            var typeXml = @"<objectType id=""" + typeId + @"""/>";
            File.WriteAllText(typeXmlFileInfo.FullName, typeXml);

            var exceptionThrown = false;
            try
            {
                new Repository(this.directoryInfo);
            }
            catch (ArgumentException e)
            {
                exceptionThrown = true;
                Assert.AreEqual(typeId + ".objectType has no Allors version", e.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        private class VersioningTestLog : Log
        {
            private readonly VersioningTest versioningTest;

            public VersioningTestLog(VersioningTest versioningTest)
            {
                this.versioningTest = versioningTest;
            }

            public override void Error(object sender, string message)
            {
                this.versioningTest.generationErrors.Add(message);
            }
        }
    }
}