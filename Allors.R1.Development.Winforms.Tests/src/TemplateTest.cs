// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Tests
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using Allors.Testing.Winforms;
    using Allors.Testing.Winforms.Testers;

    using Allors.R1.Meta;

    using AllorsTestWindowsTests;

    using NUnit.Framework;

    [TestFixture]
    public class TemplateTest : AllorsWithRepositoryTest
    {
        private FileInfo templateFileInfo;
        private FileInfo templateWithoutAConfigurationFileInfo;
        private FileInfo templateWithoutAnAllorsFileInfo;
        private FileInfo templateWithoutANameFileInfo;
        private FileInfo templateWithoutAnIdFileInfo;
        private FileInfo templateWithoutAVersionFileInfo;
        private TestCaseSwitch testCaseSwitch;

        #region Test case switch
        private enum TestCaseSwitch
        {
            Minimal,

            MissingTemplate,

            IllegalTemplate,

        }
        #endregion

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var template = @"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<DefaultTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
";
            this.templateFileInfo = this.SaveTemplate(
                "template.stg",
                template);

            template = @"group AllorsTemplate;

TemplateName() ::= <<DefaultTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
";
            this.templateWithoutAnIdFileInfo = this.SaveTemplate(
                "templateWithoutAnId.stg",
                template);

            template = @"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
";
            this.templateWithoutANameFileInfo = this.SaveTemplate(
                "templateWithoutAName.stg",
                template);

            template = @"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<DefaultTemplate>>
TemplateAllors() ::= <<" + Domain.Version + @">>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
";
            this.templateWithoutAVersionFileInfo = this.SaveTemplate(
                "templateWithoutAVersion.stg",
                template);

            template = @"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<DefaultTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateConfiguration(domain) ::= <<
<generations/>
>>
";
            this.templateWithoutAnAllorsFileInfo = this.SaveTemplate(
                "templateWithoutAnAllors.stg",
                template);

            template = @"group AllorsTemplate;

TemplateId() ::= <<" + Guid.NewGuid() + @">>
TemplateName() ::= <<DefaultTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<" + Domain.Version + @">>
";
            this.templateWithoutAConfigurationFileInfo = this.SaveTemplate(
                "templateWithoutAConfiguration.stg",
                template);
        }

        [Test]
        public void Empty()
        {
            this.testCaseSwitch = TestCaseSwitch.Minimal;

            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            Assert.AreEqual("Templates", this.ExtendedViewTemplatesNode.Target.Text);

            this.ExtendedViewTemplatesNode.Target.ExpandAll();

            Assert.AreEqual(0, this.ExtendedViewTemplatesNode.Target.Nodes.Count);
        }

        [Test]
        public void IllegalTemplate()
        {
            this.testCaseSwitch = TestCaseSwitch.IllegalTemplate;

            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            this.TreeViewTester.SelectNode(this.ExtendedViewTemplatesNode);

            var addTemplate = new MenuItemTester(Constants.AddTemplate);
            addTemplate.Target.PerformClick();

            Assert.AreEqual(0, this.Repository.Templates.Length);
        }

        [Test]
        public void Minimal()
        {
            this.testCaseSwitch = TestCaseSwitch.Minimal;

            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            this.TreeViewTester.SelectNode(this.ExtendedViewTemplatesNode);

            var addTemplate = new MenuItemTester(Constants.AddTemplate);
            addTemplate.Target.PerformClick();

            Assert.AreEqual(1, this.Repository.Templates.Length);
            var template = this.Repository.Templates[0];
            Assert.AreEqual("MyConfig", template.Name);
            Assert.AreEqual(domain, template.Domain);
        }

        [Test]
        public void MissingTemplate()
        {
            this.testCaseSwitch = TestCaseSwitch.MissingTemplate;

            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            this.TreeViewTester.SelectNode(this.ExtendedViewTemplatesNode);

            var addTemplate = new MenuItemTester(Constants.AddTemplate);
            addTemplate.Target.PerformClick();

            Assert.AreEqual(0, this.Repository.Templates.Length);
        }

        [Test]
        public void Existing()
        {
            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var templateSourceFileInfo = new FileInfo(Path.Combine(this.Directory.FullName, "MyTemplate.stg"));
            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
                    @"group AllorsBaseReference;
domain(domain) ::= <<
<domain>$domain.Name$</domain>
>>

TemplateConfiguration(domain) ::= <<
<allors version=""1.0"">
	<generation template=""domain"" output=""test/domain.xml""/> 
</allors>
>>

TemplateId() ::= <<{E615D7A2-4EF3-4903-BF0E-A58493EF5F7B}>>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<1.0>>
");
                writer.Close();
            }

            var template = this.Repository.AddTemplate();
            template.Name = "MyTemplate";
            template.Source = new Uri(templateSourceFileInfo.FullName);

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            var templatesNode = this.ExtendedViewTemplatesNode;
            
            Assert.AreEqual(1, templatesNode.Target.GetNodeCount(false));

            var templateNodes = templatesNode.FindByTagType(typeof(TemplateTag));
            
            Assert.AreEqual(1, templateNodes.Length);

            var templateNode = templateNodes[0];

            Assert.AreEqual("MyTemplate", templateNode.Target.Text);
            Assert.AreEqual(domain, template.Domain);
        }
        
        [Test]
        public void Update()
        {
            this.SaveTemplate();

            var domain = this.Repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var templateSourceFileInfo = new FileInfo(Path.Combine(this.Directory.FullName, "MyTemplate.stg"));
            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
                    @"group AllorsBaseReference;
domain(domain) ::= <<
<domain>$domain.Name$</domain>
>>

TemplateConfiguration(domain) ::= <<
<allors version=""1.0"">
	<generation template=""domain"" output=""test/domain.xml""/> 
</allors>
>>

TemplateId() ::= <<{E615D7A2-4EF3-4903-BF0E-A58493EF5F7B}>>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<1.0>>
");
                writer.Close();
            }

            var template = this.Repository.AddTemplate();
            template.Name = "MyTemplate";
            template.Source = new Uri(templateSourceFileInfo.FullName);

            Assert.AreEqual("1.0.0", template.StringTemplate.Version);

            this.Explorer.AddRepository(this.Repository);

            this.ExtendedView = true;

            using (var writer = templateSourceFileInfo.CreateText())
            {
                writer.Write(
                    @"group AllorsBaseReference;
domain(domain) ::= <<
<domain>$domain.Name$</domain>
>>

TemplateConfiguration(domain) ::= <<
<allors version=""1.0"">
	<generation template=""domain"" output=""test/domain.xml""/> 
</allors>
>>

TemplateId() ::= <<{E615D7A2-4EF3-4903-BF0E-A58493EF5F7B}>>
TemplateName() ::= <<MyTemplate>>
TemplateVersion() ::= <<1.0.1>>
TemplateAllors() ::= <<1.0>>
");
                writer.Close();
            }
            
            this.TreeViewTester.SelectNode(this.ExtendedViewFirstTemplateNode);
            
            var addTemplate = new MenuItemTester(Constants.UpdateTemplate);
            addTemplate.Target.PerformClick();

            Assert.AreEqual("1.0.1", template.StringTemplate.Version);

            // Bug that changed text of templatesNode to name of template
            Assert.AreEqual("Templates", this.ExtendedViewTemplatesNode.Target.Text);
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            switch (this.testCaseSwitch)
            {
                case TestCaseSwitch.Minimal:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.MinimalWizard();
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.MissingTemplate:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.MissingTemplate_Wizard();
                            break;
                        case 2:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Template Source is not valid.", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                case TestCaseSwitch.IllegalTemplate:
                    switch (++this.OnShownCount)
                    {
                        case 1:
                            this.IllegalTemplateWizard();
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            var messageBoxTester = new MessageBoxTester(args.Tester);
                            messageBoxTester.Target.DialogResult = DialogResult.OK;
                            Assert.AreEqual("Template Source is not valid.", messageBoxTester.Target.Text);
                            break;
                        default:
                            throw new Exception("No event handler for Onshown");
                    }

                    break;
                default:
                    throw new Exception("No event handler for Onshown");
            }
        }

        private void IllegalTemplateWizard()
        {
            var nameTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.NameTextBox);
            var templateSourceTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.TemplateSourceTextBox);
            var finishButtonTester = new ButtonTester(Constants.AddTemplateWizard, Constants.FinishButton);

            nameTextBoxTester.Target.Text = "templateWithoutAnIdFileInfo";
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateWithoutAnIdFileInfo.FullName).ToString();
            finishButtonTester.Click();

            nameTextBoxTester.Target.Text = "templateWithoutANameFileInfo";
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateWithoutANameFileInfo.FullName).ToString();
            finishButtonTester.Click();

            nameTextBoxTester.Target.Text = "templateWithoutAVersionFileInfo";
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateWithoutAVersionFileInfo.FullName).ToString();
            finishButtonTester.Click();

            nameTextBoxTester.Target.Text = "templateWithoutAnAllorsFileInfo";
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateWithoutAnAllorsFileInfo.FullName).ToString();
            finishButtonTester.Click();

            nameTextBoxTester.Target.Text = "templateWithoutAConfigurationFileInfo";
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateWithoutAConfigurationFileInfo.FullName).ToString();
            finishButtonTester.Click();

            var cancelButtonTester = new ButtonTester(Constants.AddTemplateWizard, Constants.CancelButton);
            cancelButtonTester.Click();
        }

        private void MinimalWizard()
        {
            var nameTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.NameTextBox);
            nameTextBoxTester.Target.Text = "MyConfig";

            var templateSourceTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.TemplateSourceTextBox);
            templateSourceTextBoxTester.Target.Text = new Uri(this.templateFileInfo.FullName).ToString();

            var finishButtonTester = new ButtonTester(Constants.AddTemplateWizard, Constants.FinishButton);
            finishButtonTester.Click();
        }

        private void MissingTemplate_Wizard()
        {
            var nameTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.NameTextBox);
            nameTextBoxTester.Target.Text = "MyConfig";

            var templateSourceTextBoxTester = new TextBoxTester(Constants.AddTemplateWizard, Constants.TemplateSourceTextBox);
            templateSourceTextBoxTester.Target.Text = "file:///is/not/here";

            var finishButtonTester = new ButtonTester(Constants.AddTemplateWizard, Constants.FinishButton);
            finishButtonTester.Click();

            var cancelButtonTester = new ButtonTester(Constants.AddTemplateWizard, Constants.CancelButton);
            cancelButtonTester.Click();
        }

        private void SaveTemplate()
        {
            var fileInfo = new FileInfo(Path.Combine(TemplatesDirectory.FullName, "test.stg"));

            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            using (var writer = fileInfo.CreateText())
            {
                writer.Write(@"group AllorsBaseReference;

TemplateId() ::= <<83AE97FF-128D-4629-ACF3-C1C76FC75995>>
TemplateUrl() ::= <<file:///D|/temp/test.stg>>
TemplateName() ::= <<Test>>
TemplateVersion() ::= <<1.0.0>>
TemplateAllors() ::= <<1.0>>
TemplateConfiguration(domain) ::= <<
<generations>
    <generation template=""domain"" output=""test/domain.xml""/>
</generations>
>>

domain(domain) ::= <<
<domain/>
>>
");
                writer.Close();
            }
        }
    }
}