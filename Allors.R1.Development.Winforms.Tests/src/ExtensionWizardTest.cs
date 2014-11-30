//------------------------------------------------------------------------------------------------- 
// <copyright file="Default.cs" company="Allors bvba">
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
// <summary>Defines the Default type.</summary>
//-------------------------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Windows.Forms;
using Allors.Development.WinForms.Wizards;
using Allors.Meta;
using Allors.Meta.Extension;
using Allors.Testing.Winforms;
using Allors.Testing.Winforms.Testers;
using AllorsTestWindowsTests;
using NUnit.Framework;

namespace Allors.Development.Winforms.Tests
{
    [TestFixture]
    public class ExtemsionWizardTest : AllorsWithRepositoryTest
    {
        #region Setup/Teardown

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            domain = repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            explorer.AddRepository(repository);

            Namespace ns = domain.AddLocalNamespace(Guid.NewGuid());
            ns.ShortName = "MyNamespace";
            ns.LongName = "My.Namespace";

            domain.SendChangedEvent();

            this.extensionAssociation = ns.Extension;

            this.extensionWizard = new ExtensionWizard(this.extensionAssociation);

            this.nameTextBox = new TextBoxTester(Constants.NAME_TEXT_BOX);
            this.valueTextBox = new TextBoxTester(Constants.VALUE_TEXT_BOX);
            this.typeComboBox = new ComboBoxTester(Constants.TYPE_COMBO_BOX);
            this.idTextBox = new TextBoxTester(Constants.ID_TEXT_BOX);

            this.unitRadioButton = new RadioButtonTester(Constants.UNIT_RADIO_BUTTON);
            this.compositeRadioButton = new RadioButtonTester(Constants.COMPOSITE_RADIO_BUTTON);
            this.metaObjectRadioButton = new RadioButtonTester(Constants.META_OBJECT_RADIO_BUTTON);

            this.finishButtonTester = new ButtonTester(Constants.FINISH_BUTTON);
            this.cancelButtonTester = new ButtonTester(Constants.CANCEL_BUTTON);

            this.messageBoxText = null;
        }

        #endregion

        private Domain domain;
        private TestCaseSwitch testCaseSwitch;
        private ExtensionComposite extensionAssociation;
        private ExtensionWizard extensionWizard;
        private TextBoxTester nameTextBox;
        private TextBoxTester idTextBox;
        private TextBoxTester valueTextBox;
        private ButtonTester finishButtonTester;
        private ComboBoxTester typeComboBox;
        private RadioButtonTester unitRadioButton;
        private RadioButtonTester compositeRadioButton;
        private RadioButtonTester metaObjectRadioButton;
        private ButtonTester cancelButtonTester;
        private string messageBoxText;

        private enum TestCaseSwitch
        {
            None,
            String,
            StringCancel,
            StringNoName,
            StringEmpty,
            TitleAndLabels,
            Boolean,
            BooleanIllegal,
            Composite,
            CompositeCancel,
            MetaObjectWithId,
            MetaObjectWithoutId,
            MetaObjectNotFound,
            NameAlreadyExists
        }

        protected override void OnShown(AllorsEventOccuredEventArgs args)
        {
            string methodName = testCaseSwitch + "_OnShown" + ++onShownCount;
            MethodInfo method = this.GetType().GetMethod(methodName);

            if(method==null)
            {
                throw new Exception("No event handler for " + methodName);
            }

            object[] methodArgs = {args};
            method.Invoke(this, methodArgs);
        }

        [Test]
        public void TitleAndLabels()
        {
            testCaseSwitch = TestCaseSwitch.TitleAndLabels;

            Assert.AreEqual("Allors Extension Wizard", extensionWizard.Text);

            LabelTester titleLabel = new LabelTester(Constants.TITLE_LABEL);
            LabelTester titleExplanationLabel = new LabelTester(Constants.TITLE_EXPLANATION_LABEL);

            Assert.AreEqual("Welcome to the Allors Add Extension Wizard", titleLabel.Target.Text);
            Assert.AreEqual("This wizard adds an extension.", titleExplanationLabel.Target.Text);
        }

        [Test]
        public void NameAlreadyExists()
        {
            testCaseSwitch = TestCaseSwitch.NameAlreadyExists;

            extensionAssociation["Same"] = "Same";

            extensionWizard.ShowDialog();

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);

            Assert.IsNull(extensionRelation);
            Assert.AreEqual(1, this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual("Same", this.extensionAssociation["Same"]);
        }

        public void NameAlreadyExists_OnShown1(AllorsEventOccuredEventArgs args)
        {
            nameTextBox.Target.Text = "Same";
            valueTextBox.Target.Text = "Different";

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.AreEqual("Extension with name Same already exists", messageBoxText);

            cancelButtonTester.Click();
        }

        public void NameAlreadyExists_OnShown2(AllorsEventOccuredEventArgs args)
        {
            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
            messageBoxTester.Target.DialogResult = DialogResult.OK;
            this.messageBoxText = messageBoxTester.Target.Text;
        }


        [Test]
        public void String()
        {
            testCaseSwitch = TestCaseSwitch.String;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
            
            this.extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.OK, extensionWizard.DialogResult);
 
            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.IsNotNull(extensionRelation);
            Assert.AreEqual("MyExtension", extensionRelation.Name);
            Assert.IsTrue(extensionRelation.ExistExtensionRole);
            Assert.AreEqual(ExtensionRoleKind.Unit, extensionRelation.ExtensionRoleKind);
            Assert.AreEqual("MyValue", extensionRelation.ExtensionRoleValue);

            Assert.AreEqual(1,this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual(extensionRelation, this.extensionAssociation.ExtensionRelations[0]);
        }

        public void String_OnShown1(AllorsEventOccuredEventArgs args)
        {
            nameTextBox.Target.Text = "MyExtension";
            valueTextBox.Target.Text = "MyValue";

            Assert.IsTrue(valueTextBox.Target.Enabled);
            Assert.IsTrue(typeComboBox.Target.Enabled);
            Assert.AreEqual("String", typeComboBox.Target.Text);
            Assert.IsFalse(idTextBox.Target.Enabled);
            
            Assert.IsTrue(unitRadioButton.Target.Checked);
            Assert.IsFalse(compositeRadioButton.Target.Checked);
            Assert.IsFalse(metaObjectRadioButton.Target.Checked);

            finishButtonTester.Click();
        }

        [Test]
        public void StringCancel()
        {
            testCaseSwitch = TestCaseSwitch.StringCancel;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();
            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.AreEqual(DialogResult.Cancel,extensionWizard.DialogResult);
            
            Assert.IsNull(extensionRelation);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void StringCancel_OnShown1(AllorsEventOccuredEventArgs args)
        {
            nameTextBox.Target.Text = "MyExtension";
            valueTextBox.Target.Text = "MyValue";

            cancelButtonTester.Click();
        }

        [Test]
        public void StringNoName()
        {
            testCaseSwitch = TestCaseSwitch.StringNoName;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);

            Assert.IsNull(extensionRelation);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void StringNoName_OnShown1(AllorsEventOccuredEventArgs args)
        {
            valueTextBox.Target.Text = "MyValue";

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.AreEqual("Name is required", messageBoxText);

            cancelButtonTester.Click();
        }

        public void StringNoName_OnShown2(AllorsEventOccuredEventArgs args)
        {
            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
            messageBoxTester.Target.DialogResult = DialogResult.OK;
            this.messageBoxText = messageBoxTester.Target.Text;
        }
        
        [Test]
        public void StringEmpty()
        {
            testCaseSwitch = TestCaseSwitch.StringEmpty;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            this.extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.OK, extensionWizard.DialogResult);

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.IsNotNull(extensionRelation);
            Assert.AreEqual("MyExtension", extensionRelation.Name);
            Assert.IsTrue(extensionRelation.ExistExtensionRole);
            Assert.AreEqual(ExtensionRoleKind.Unit, extensionRelation.ExtensionRoleKind);
            Assert.AreEqual("", extensionRelation.ExtensionRoleValue);

            Assert.AreEqual(1, this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual(extensionRelation, this.extensionAssociation.ExtensionRelations[0]);

        }

        public void StringEmpty_OnShown1(AllorsEventOccuredEventArgs args)
        {
            nameTextBox.Target.Text = "MyExtension";

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();
        }
        
        [Test]
        public void Boolean()
        {
            testCaseSwitch = TestCaseSwitch.Boolean;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.OK, extensionWizard.DialogResult);

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.IsNotNull(extensionRelation);
            Assert.AreEqual("MyExtension", extensionRelation.Name);
            Assert.IsTrue(extensionRelation.ExistExtensionRole);
            Assert.AreEqual(ExtensionRoleKind.Unit, extensionRelation.ExtensionRoleKind);
            Assert.AreEqual(true, extensionRelation.ExtensionRoleValue);

            Assert.AreEqual(1, this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual(extensionRelation, this.extensionAssociation.ExtensionRelations[0]);
        }

        public void Boolean_OnShown1(AllorsEventOccuredEventArgs args)
        {
            typeComboBox.Target.SelectedItem = UnitTypeTag.AllorsBoolean;

            nameTextBox.Target.Text = "MyExtension";
            valueTextBox.Target.Text = "true";

            Assert.IsTrue(valueTextBox.Target.Enabled);
            Assert.IsTrue(typeComboBox.Target.Enabled);
            Assert.AreEqual("Boolean", typeComboBox.Target.Text);
            Assert.IsFalse(idTextBox.Target.Enabled);
           
            Assert.IsTrue(unitRadioButton.Target.Checked);
            Assert.IsFalse(compositeRadioButton.Target.Checked);
            Assert.IsFalse(metaObjectRadioButton.Target.Checked);

            finishButtonTester.Click();
            
        }
        
        [Test]
        public void BooleanIllegal()
        {
            testCaseSwitch = TestCaseSwitch.BooleanIllegal;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);

            Assert.IsNull(extensionRelation);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void BooleanIllegal_OnShown1(AllorsEventOccuredEventArgs args)
        {
            typeComboBox.Target.SelectedItem = UnitTypeTag.AllorsBoolean;

            nameTextBox.Target.Text = "MyExtension";
            valueTextBox.Target.Text = "Oops";

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.AreEqual("The string 'Oops' is not a valid Boolean value.", messageBoxText);

            cancelButtonTester.Click();
        }

        public void BooleanIllegal_OnShown2(AllorsEventOccuredEventArgs args)
        {
            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
            messageBoxTester.Target.DialogResult = DialogResult.OK;
            this.messageBoxText = messageBoxTester.Target.Text;
        }

        [Test]
        public void Composite()
        {
            testCaseSwitch = TestCaseSwitch.Composite;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.OK, extensionWizard.DialogResult);

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.IsNotNull(extensionRelation);
            Assert.AreEqual("MyExtension", extensionRelation.Name);
            Assert.IsTrue(extensionRelation.ExistExtensionRole);
            Assert.AreEqual(ExtensionRoleKind.Composite, extensionRelation.ExtensionRoleKind);
            Assert.AreEqual(typeof(ExtensionComposite), extensionRelation.ExtensionRoleValue.GetType());

            ExtensionComposite extensionComposite = (ExtensionComposite)extensionRelation.ExtensionRoleValue;
            Assert.AreNotEqual(extensionAssociation, extensionComposite);

            Assert.AreEqual(1, this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual(extensionRelation, this.extensionAssociation.ExtensionRelations[0]);
        }

        public void Composite_OnShown1(AllorsEventOccuredEventArgs args)
        {
            compositeRadioButton.Target.Checked = true;

            nameTextBox.Target.Text = "MyExtension";

            Assert.IsFalse(valueTextBox.Target.Enabled);
            Assert.IsFalse(typeComboBox.Target.Enabled);
            Assert.IsFalse(idTextBox.Target.Enabled);

            Assert.IsFalse(unitRadioButton.Target.Checked);
            Assert.IsTrue(compositeRadioButton.Target.Checked);
            Assert.IsFalse(metaObjectRadioButton.Target.Checked);

            finishButtonTester.Click();

        }
        
        [Test]
        public void CompositeCancel()
        {
            testCaseSwitch = TestCaseSwitch.CompositeCancel;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();
            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);

            Assert.IsNull(extensionRelation);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void CompositeCancel_OnShown1(AllorsEventOccuredEventArgs args)
        {
            compositeRadioButton.Target.Checked = true;

            nameTextBox.Target.Text = "MyExtension";

            cancelButtonTester.Click();
        }
        
        [Test]
        public void MetaObjectWithId()
        {
            testCaseSwitch = TestCaseSwitch.MetaObjectWithId;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.OK, extensionWizard.DialogResult);

            ExtensionRelation extensionRelation = extensionWizard.ExtensionRelation;

            Assert.IsNotNull(extensionRelation);
            Assert.AreEqual("MyExtension", extensionRelation.Name);
            Assert.IsTrue(extensionRelation.ExistExtensionRole);
            Assert.AreEqual(ExtensionRoleKind.MetaObject, extensionRelation.ExtensionRoleKind);

            Assert.AreEqual(typeof(Domain), extensionRelation.ExtensionRoleValue.GetType());
            Assert.AreEqual(domain, extensionRelation.ExtensionRoleValue);

            Assert.AreEqual(1, this.extensionAssociation.ExtensionRelations.Length);
            Assert.AreEqual(extensionRelation, this.extensionAssociation.ExtensionRelations[0]);
        }

        public void MetaObjectWithId_OnShown1(AllorsEventOccuredEventArgs args)
        {
            metaObjectRadioButton.Target.Checked = true;

            nameTextBox.Target.Text = "MyExtension";
            idTextBox.Target.Text = domain.IdAsString;
            
            Assert.IsFalse(valueTextBox.Target.Enabled);
            Assert.IsFalse(typeComboBox.Target.Enabled);
            Assert.IsTrue(idTextBox.Target.Enabled);
            
            Assert.IsFalse(unitRadioButton.Target.Checked);
            Assert.IsFalse(compositeRadioButton.Target.Checked);
            Assert.IsTrue(metaObjectRadioButton.Target.Checked);

            finishButtonTester.Click();
        }

        [Test]
        public void MetaObjectWithoutId()
        {
            testCaseSwitch = TestCaseSwitch.MetaObjectWithoutId;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void MetaObjectWithoutId_OnShown1(AllorsEventOccuredEventArgs args)
        {
            metaObjectRadioButton.Target.Checked = true;

            nameTextBox.Target.Text = "MyExtension";

            Assert.IsFalse(valueTextBox.Target.Enabled);
            Assert.IsFalse(typeComboBox.Target.Enabled);
            Assert.IsTrue(idTextBox.Target.Enabled);

            Assert.IsFalse(unitRadioButton.Target.Checked);
            Assert.IsFalse(compositeRadioButton.Target.Checked);
            Assert.IsTrue(metaObjectRadioButton.Target.Checked);

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.IsNotNull(messageBoxText);
            Assert.AreEqual("Id is required", messageBoxText);

            cancelButtonTester.Click();
        }

        public void MetaObjectWithoutId_OnShown2(AllorsEventOccuredEventArgs args)
        {
            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
            messageBoxTester.Target.DialogResult = DialogResult.OK;
            this.messageBoxText = messageBoxTester.Target.Text;
        }

        [Test]
        public void MetaObjectNotFound()
        {
            testCaseSwitch = TestCaseSwitch.MetaObjectNotFound;

            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);

            extensionWizard.ShowDialog();

            Assert.AreEqual(DialogResult.Cancel, extensionWizard.DialogResult);
            Assert.AreEqual(0, this.extensionAssociation.ExtensionRelations.Length);
        }

        public void MetaObjectNotFound_OnShown1(AllorsEventOccuredEventArgs args)
        {
            metaObjectRadioButton.Target.Checked = true;

            nameTextBox.Target.Text = "MyExtension";
            idTextBox.Target.Text = Guid.NewGuid().ToString();

            Assert.IsFalse(valueTextBox.Target.Enabled);
            Assert.IsFalse(typeComboBox.Target.Enabled);
            Assert.IsTrue(idTextBox.Target.Enabled);

            Assert.IsFalse(unitRadioButton.Target.Checked);
            Assert.IsFalse(compositeRadioButton.Target.Checked);
            Assert.IsTrue(metaObjectRadioButton.Target.Checked);

            Assert.IsNull(messageBoxText);

            finishButtonTester.Click();

            Assert.IsNotNull(messageBoxText);
            Assert.AreEqual("Meta object with id " + idTextBox.Target.Text + " does not exist.", messageBoxText);

            cancelButtonTester.Click();
        }

        public void MetaObjectNotFound_OnShown2(AllorsEventOccuredEventArgs args)
        {
            MessageBoxTester messageBoxTester = new MessageBoxTester(args.Tester);
            messageBoxTester.Target.DialogResult = DialogResult.OK;
            this.messageBoxText = messageBoxTester.Target.Text;
        }

    }
}