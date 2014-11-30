// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerateTest.cs" company="Allors bvba">
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
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml;
    using Allors.R1.Meta;
    using NUnit.Framework;

    [TestFixture]
    public class GenerateTest
    {
        private DirectoryInfo directoryInfo;
        private Repository repository;

        private bool errorOccured;

        private DirectoryInfo TemplatesDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "templates")); }
        }

        private DirectoryInfo TemplatesSourceDirectoryInfo
        {
            get { return new DirectoryInfo(Path.Combine(this.directoryInfo.FullName, "templatesSource")); }
        }

        [Test]
        public void Delete()
        {
            var templateId = Guid.NewGuid();

            var templateSourceFileInfo = new FileInfo(Path.Combine(this.TemplatesSourceDirectoryInfo.FullName, "test.stg"));

            if (!templateSourceFileInfo.Directory.Exists)
            {
                templateSourceFileInfo.Directory.Create();
            }

            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
                    @"group AllorsBaseReference;

TemplateId() ::= <<" + templateId.ToString().ToLower() +
                    @">>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0>>
TemplateAllors() ::= <<1.0>>
TemplateConfiguration(domain) ::= <<
<generations>
    <generation template=""domain"" output=""test/domain.xml""/>
$domain.CompositeObjectTypes:configureTypes()$
</generations>
>>

configureRelations(relationType) ::= <<
	<generation template=""relations"" input=""$relationType.Id $"" output=""test/$relationType.Id $.xml""/> 
>>

configureTypes(type) ::= <<
	<generation template=""types"" input=""$type.Id $"" output=""test/$type.Id $.xml""/> 
	$type.RoleTypes:{roleType | $roleType.RelationTypeWhereRole:configureRelations()$}$
>>

relations(domain,relationType) ::= <<
<relationType id=""$relationType.Id $"" role=""$relationType.Role.AssignedSingularName$""/>
>>

types(domain,type) ::= <<
<type id=""$type.Id $""/>
>>

domain(domain) ::= <<
<domain/>
>>
");
                writer.Close();
            }

            var repositoryFileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, "allors.repository"));
            this.repository = new Repository(repositoryFileInfo.Directory);

            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var typeId = new Guid("{07B000C7-800F-459b-A6BB-17DC9E690A06}");
            var type = domain.AddDeclaredObjectType(typeId);
            type.SingularName = "Company";
            type.PluralName = "Companies";

            var relationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationType.AssociationType.ObjectType = type;
            relationType.RoleType.ObjectType = type;
            relationType.RoleType.AssignedSingularName = "role";
            relationType.RoleType.AssignedPluralName = "roles";

            var relation2 = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relation2.AssociationType.ObjectType = type;
            relation2.RoleType.ObjectType = type;
            relation2.RoleType.AssignedSingularName = "role2";
            relation2.RoleType.AssignedPluralName = "roles2";

            Assert.IsTrue(domain.IsValid);

            var template = this.repository.AddTemplate();
            template.Name = "My Config";
            template.Source = new Uri(templateSourceFileInfo.FullName);
            template.Output = "prefix";
            
            var templateFileInfo = new FileInfo(Path.Combine(this.repository.TemplatesDirectoryInfo.FullName, template.Id + ".template"));
            var stringTemplateFileInfo = new FileInfo(Path.Combine(this.repository.TemplatesDirectoryInfo.FullName, template.Id + ".stg"));

            templateFileInfo.Refresh();
            Assert.IsTrue(templateFileInfo.Exists);

            template.Delete();

            templateFileInfo.Refresh();
            stringTemplateFileInfo.Refresh();

            Assert.IsFalse(templateFileInfo.Exists);
            Assert.IsFalse(stringTemplateFileInfo.Exists);
        }

        [Test]
        public void Created()
        {
            var templateId = Guid.NewGuid();

            var templateSourceFileInfo = new FileInfo(Path.Combine(this.TemplatesSourceDirectoryInfo.FullName, "test.stg"));
            if (!templateSourceFileInfo.Directory.Exists)
            {
                templateSourceFileInfo.Directory.Create();
            }

            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
@"group AllorsBaseReference;

TemplateId() ::= <<" + templateId.ToString().ToLower() + @">>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<1.0>>
TemplateConfiguration(domain, settings) ::= <<
<generations>
    <generation template=""domain"" output=""test/domain.xml""/>
$domain.CompositeObjectTypes:configureTypes()$
</generations>
>>

configureRelations(relationType) ::= <<
	<generation template=""relations"" input=""$relationType.Id$"" output=""test/$relationType.Id$.xml""/> 
>>

configureTypes(objectType) ::= <<
	<generation template=""objectTypes"" input=""$objectType.Id$"" output=""test/$objectType.Id$.xml""/> 
	$objectType.RoleTypes:{roleType | $roleType.RelationTypeWhereRoleType:configureRelations()$}$
>>

relations(domain,settings,relationType) ::= <<
<relations id=""$relationType.Id$"" role=""$relationType.RoleType.AssignedSingularName$"" roleId=""$relationType.RoleType.IdAsString$"" associationId=""$relationType.AssociationType.IdAsString$""/>
>>

objectTypes(domain,settings,objectType) ::= <<
<objectType id=""$objectType.Id$""/>
>>

domain(domain,settings) ::= <<
<domain/>
>>
");
                writer.Close();
            }

            this.repository = new Repository(this.directoryInfo);

            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var typeId = new Guid("{07B000C7-800F-459b-A6BB-17DC9E690A06}");
            var type = domain.AddDeclaredObjectType(typeId);
            type.SingularName = "Company";
            type.PluralName = "Companies";

            var relationId = new Guid("{3C48C2D5-79E2-4da2-BDB4-2EE7307DB5EF}");
            var associationId = new Guid("{690AD171-FB3B-4051-8B07-F9E11F9CDAE0}");
            var roleId = new Guid("{C0FA21CE-95E1-4543-94C0-B10EC66CAC69}");
            var relationType = domain.AddDeclaredRelationType(relationId, associationId, roleId);
            relationType.AssociationType.ObjectType = type;
            relationType.RoleType.ObjectType = type;
            relationType.RoleType.AssignedSingularName = "role";
            relationType.RoleType.AssignedPluralName = "roles";

            var relation2Id = new Guid("{ABB975B6-5374-4654-96A2-E98D5C2A1786}");
            var association2Id = new Guid("{31B821DD-9FE6-4E08-9336-5F0D68CD2DBD}");
            var role2Id = new Guid("{C38B57FB-589C-4B63-BCAE-F714E2437614}");
            var relation2 = domain.AddDeclaredRelationType(relation2Id, association2Id, role2Id);
            relation2.AssociationType.ObjectType = type;
            relation2.RoleType.ObjectType = type;
            relation2.RoleType.AssignedSingularName = "role2";
            relation2.RoleType.AssignedPluralName = "roles2";

            Assert.IsTrue(domain.IsValid);

            var template = this.repository.AddTemplate();
            template.Name = "MyTemplate";
            template.Source = new Uri(templateSourceFileInfo.FullName);
            template.Output = "prefix";

            var xmlFileInfo = new FileInfo(Path.Combine(this.repository.TemplatesDirectoryInfo.FullName, template.Id + ".template"));

            Assert.IsTrue(xmlFileInfo.Exists);
            var xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            var expectedXml = this.ToComparableXml(
@"<template allors=""1.0"">
    <name>MyTemplate</name>
    <output>prefix</output>
    <source>" + new Uri(templateSourceFileInfo.FullName) + @"</source>
</template>
");
            var actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            template.Generate(new GenerateTestLog(this));

            Assert.IsFalse(this.errorOccured);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "prefix/test/domain.xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<domain/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.DirectoryInfo.FullName, "../output/prefix/test/" + typeId.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<objectType id=""" + typeId.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "../output/prefix/test/" + relationId.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<relations id=""" + relationId.ToString().ToLower() + @""" role=""role"" roleId=""" + roleId.ToString().ToLower() + @""" associationId=""" + associationId.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "../output/prefix/test/" + relation2Id.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<relations id=""" + relation2Id.ToString().ToLower() + @""" role=""role2"" roleId=""" + role2Id.ToString().ToLower() + @""" associationId=""" + association2Id.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            relationType.RoleType.AssignedSingularName = "role3";
            Assert.IsTrue(domain.IsValid);
        }

        [Test]
        public void GenerateFromFile()
        {
            var templateId = Guid.NewGuid();

            this.TemplatesDirectoryInfo.Create();
            var configurationFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, "e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template"));
            using (var writer = configurationFileInfo.CreateText())
            {
                writer.Write(
                    @"
<template allors=""1.0"">
    <name>MyTemplate</name>
    <output>prefix</output>
</template>
");
                writer.Close();
            }

            this.TemplatesSourceDirectoryInfo.Create();
            var templateSourceFileInfo = new FileInfo(Path.Combine(this.TemplatesSourceDirectoryInfo.FullName, "template.stg"));
            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
@"TemplateId() ::= <<" + templateId.ToString().ToLower() + @">>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0>>
TemplateAllors() ::= <<1.0>>
TemplateConfiguration(domain,settings) ::= <<
<generations>
    <generation template=""domain"" output=""test/domain.xml""/>
$domain.CompositeObjectTypes:configureTypes()$
</generations>
>>

configureRelations(relationType) ::= <<
	<generation template=""relations"" input=""$relationType.Id$"" output=""test/$relationType.Id$.xml""/> 
>>

configureTypes(objectType) ::= <<
	<generation template=""objectTypes"" input=""$objectType.Id$"" output=""test/$objectType.Id$.xml""/> 
	$objectType.RoleTypes:{roleType | $roleType.RelationTypeWhereRoleType:configureRelations()$}$
>>

relations(domain,settings,relationType) ::= <<
<relations id=""$relationType.Id$"" role=""$relationType.RoleType.AssignedSingularName$"" roleId=""$relationType.RoleType.IdAsString$"" associationId=""$relationType.AssociationType.IdAsString$""/>
>>

objectTypes(domain,settings,objectType) ::= <<
<objectType id=""$objectType.Id$""/>
>>

domain(domain,settings) ::= <<
<domain/>
>>
");
                writer.Close();
            }

            this.repository = new Repository(this.directoryInfo);

            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            var typeId = new Guid("{07B000C7-800F-459b-A6BB-17DC9E690A06}");
            var type = domain.AddDeclaredObjectType(typeId);
            type.SingularName = "Company";
            type.PluralName = "Companies";

            var relationId = new Guid("{3C48C2D5-79E2-4da2-BDB4-2EE7307DB5EF}");
            var associationId = new Guid("{F2F58792-D232-4915-B4E6-96F84A125BB3}");
            var roleId = new Guid("{D09AA85E-17FA-4039-81CA-A50ED0BD8C35}");
            var relationType = domain.AddDeclaredRelationType(relationId, associationId, roleId);
            relationType.AssociationType.ObjectType = type;
            relationType.RoleType.ObjectType = type;
            relationType.RoleType.AssignedSingularName = "role";
            relationType.RoleType.AssignedPluralName = "roles";

            var relation2Id = new Guid("{ABB975B6-5374-4654-96A2-E98D5C2A1786}");
            var association2Id = new Guid("{8A8456FF-D0D2-4AA9-924C-8360B6773A2A}");
            var role2Id = new Guid("{C6536E84-CA85-4D7E-9979-847E81AC2E9A}");
            var relation2 = domain.AddDeclaredRelationType(relation2Id, association2Id, role2Id);
            relation2.AssociationType.ObjectType = type;
            relation2.RoleType.ObjectType = type;
            relation2.RoleType.AssignedSingularName = "role2";
            relation2.RoleType.AssignedPluralName = "roles2";

            Assert.IsTrue(domain.IsValid);

            Assert.AreEqual(1, this.repository.Templates.Length);

            var template = this.repository.Templates[0];
            template.Source = new Uri(templateSourceFileInfo.FullName);

            Assert.AreEqual("MyTemplate", template.Name);
            Assert.AreEqual("prefix", template.Output);

            template.Generate(new GenerateTestLog(this));

            Assert.IsFalse(this.errorOccured);

            var xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "prefix/test/domain.xml"));
            var xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            var expectedXml = this.ToComparableXml(@"<domain/>");
            var actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "prefix/test/" + typeId.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<objectType id=""" + typeId.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "prefix/test/" + relationId.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<relations id=""" + relationId.ToString().ToLower() + @""" role=""role"" roleId=""" + roleId.ToString().ToLower() + @""" associationId=""" + associationId.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            xmlFileInfo = new FileInfo(Path.Combine(this.repository.OutputDirectoryInfo.FullName, "prefix/test/" + relation2Id.ToString().ToLower() + ".xml"));
            xmlDocumnt = new XmlDocument();
            xmlDocumnt.Load(xmlFileInfo.FullName);
            expectedXml = this.ToComparableXml(@"<relations id=""" + relation2Id.ToString().ToLower() + @""" role=""role2"" roleId=""" + role2Id.ToString().ToLower() + @""" associationId=""" + association2Id.ToString().ToLower() + @"""/>");
            actualXml = this.ToComparableXml(xmlDocumnt.InnerXml);
            Assert.AreEqual(expectedXml, actualXml);

            relationType.RoleType.AssignedSingularName = "role3";
            Assert.IsTrue(domain.IsValid);
        }

        [Test]
        public void UpdateTemplate()
        {
            this.TemplatesDirectoryInfo.Create();
            var configurationFileInfo = new FileInfo(Path.Combine(this.TemplatesDirectoryInfo.FullName, "e72a13a7-5fdd-44c5-b40b-ce9f7eaca7d4.template"));
            using (var writer = configurationFileInfo.CreateText())
            {
                writer.Write(
                    @"
<template allors=""1.0"">
    <name>MyTemplate</name>
    <output>prefix</output>
</template>
");
                writer.Close();
            }

            this.TemplatesSourceDirectoryInfo.Create();
            var templateSourceFileInfo = new FileInfo(Path.Combine(this.TemplatesSourceDirectoryInfo.FullName, "template.stg"));
            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(@"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<OriginalTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
");
                writer.Close();
            }

            this.repository = new Repository(this.directoryInfo);

            var domain = this.repository.Domain;
            domain.Name = "MyDomain";

            Assert.IsTrue(domain.IsValid);

            var templates = this.repository.Templates;
            Assert.AreEqual(1, templates.Length);

            var template = templates[0];
            template.Source = new Uri(templateSourceFileInfo.FullName);

            Assert.AreEqual("OriginalTemplate", template.StringTemplate.Name);
            Assert.AreEqual("1.0.0", template.StringTemplate.Version);

            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(@"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<UpdatedTemplate>>
TemplateVersion() ::= <<1.0.1>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
");
                writer.Close();
            }

            template.UpdateTemplate();

            Assert.AreEqual("UpdatedTemplate", template.StringTemplate.Name);
            Assert.AreEqual("1.0.1", template.StringTemplate.Version);
        }

        [SetUp]
        public void SetUp()
        {
            this.directoryInfo = new DirectoryInfo("test");
            this.directoryInfo.DeleteRecursive();

            this.directoryInfo.Create();
            this.repository = new Repository(this.directoryInfo, true);
        }

        private string ToComparableXml(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            xmlDocument.Normalize();

            var stripWhiteSpace = new Regex(@">(\n|\s)*<");
            return stripWhiteSpace.Replace(xmlDocument.InnerXml, "><");
        }

        private class GenerateTestLog : Log
        {
            private readonly GenerateTest generateTest;

            public GenerateTestLog(GenerateTest generateTest)
            {
                this.generateTest = generateTest;
            }

            public override void Error(object sender, string message)
            {
                this.generateTest.errorOccured = true;
            }
        }
    }
}