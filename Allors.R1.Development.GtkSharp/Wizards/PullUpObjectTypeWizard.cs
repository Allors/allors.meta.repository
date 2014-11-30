//------------------------------------------------------------------------------------------------- 
// <copyright file="PullUpObjectTypeWizard.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Wizards
{
    using System;

    using Allors.R1.Development.Repository.Commands;
    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    using Gtk;

    public class PullUpObjectTypeWizard : Dialog
    {
        private readonly Entry objectTypeEntry;
        private readonly SuperDomainComboBox superDomainComboBox;
        private readonly ErrorMessage superDomainErrorMessage;
        private readonly CheckButton includeMembersCheckButton;
        private readonly DependencyTextView dependencyTextView;

        private readonly Repository repository;
        private readonly ObjectType objectType;

        private PullUpObjectType pullUp;

        public PullUpObjectTypeWizard(Repository repository, ObjectType objectType)
        {
            this.repository = repository;
            this.objectType = objectType;
            
            this.Title = Mono.Unix.Catalog.GetString("Pull up Object Type Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.DefaultWidth = 640;
            this.DefaultHeight = 400;
            
            var headerBox = new VBox
                                 {
                                     Spacing = 10, 
                                     BorderWidth = 10
                                 };
            this.VBox.PackStart(headerBox, false, false, 0);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\">Welcome to the Allors Pull Up Object Type Wizard</span>", 0.5f));
            headerBox.PackStart(new HtmlLabel("This wizard allows you to pull an object type to a super domain.", 0.5f));
           
            var form = new Form();
            this.VBox.PackStart(form);

            var objectTypeLabel = new HtmlLabel("Object type");
            this.objectTypeEntry = new Entry { Sensitive = false, Text = this.objectType.Name };

            var superDomainLabel = new HtmlLabel("Pull up to super domain");
            this.superDomainComboBox = new SuperDomainComboBox(repository, objectType.DomainWhereDeclaredObjectType);
            this.superDomainComboBox.Changed += (sender, args) => this.CreatePullUp();
            this.superDomainErrorMessage = new ErrorMessage();

            var includeMembersLabel = new HtmlLabel("Include members");
            this.includeMembersCheckButton = new CheckButton("object types, inheritances and relation types");
            this.includeMembersCheckButton.Toggled += (sender, args) => this.CreatePullUp();

            this.dependencyTextView = new DependencyTextView();
            var scrolledDepenceyTextView = new ScrolledWindow { this.dependencyTextView };

            var buttonCancel = new Button
                                    {
                                        CanDefault = true,
                                        UseStock = true,
                                        UseUnderline = true,
                                        Label = "gtk-cancel"
                                    };
            this.AddActionWidget(buttonCancel, -6);
            
            var buttonOk = new Button
                                {
                                    CanDefault = true,
                                    Name = "buttonOk",
                                    UseStock = true,
                                    UseUnderline = true,
                                    Label = "gtk-ok"
                                };
            buttonOk.Clicked += this.OnButtonOkClicked;
            this.ActionArea.PackStart(buttonOk);

            // Layout
            form.Attach(objectTypeLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
            form.Attach(this.objectTypeEntry, 0, 1, 1, 2, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);

            form.Attach(superDomainLabel, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
            form.Attach(this.superDomainComboBox, 0, 1, 3, 4, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
            form.Attach(this.superDomainErrorMessage, 0, 1, 4, 5, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);

            form.Attach(includeMembersLabel, 0, 1, 5, 6, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
            form.Attach(this.includeMembersCheckButton, 0, 1, 6, 7, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);

            form.Attach(scrolledDepenceyTextView, 0, 1, 7, 8, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill | AttachOptions.Expand, 0, 0);

            this.ShowAll();

            this.ResetErrorMessages();
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            var domain = this.repository.Domain;
            bool error = false;

            var superDomain = this.superDomainComboBox.ActiveItem;
            if (superDomain == null)
            {
                error = true;
                this.superDomainErrorMessage.Text = "Super domain is mandatory";
            }
            
            if (!error)
            {
                var validationReport = domain.Validate();

                if (!validationReport.ContainsErrors)
                {
                    this.pullUp.Execute();
                    this.Respond(ResponseType.Ok);
                }
                else
                {
                    var message = string.Join("\n", validationReport.Messages);
                    var md = new MessageDialog(
                        this,
                        DialogFlags.DestroyWithParent,
                        MessageType.Error,
                        ButtonsType.Close,
                        message);
                    md.Run();
                    md.Destroy();
                }
            }

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }

        private void CreatePullUp()
        {
            if (this.superDomainComboBox.ActiveItem == null)
            {
                return;
            }

            var superDomain = this.superDomainComboBox.ActiveItem;
            var includeContainedItems = this.includeMembersCheckButton.Active;

            this.pullUp = this.repository.PullUp(superDomain, this.objectType, includeContainedItems);

            this.dependencyTextView.Update(this.pullUp);
        }

        private void ResetErrorMessages()
        {
            this.superDomainErrorMessage.Text = null;

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }
    }
}