//------------------------------------------------------------------------------------------------- 
// <copyright file="AddObjectTypeWizard.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;
    using Allors.R1.Meta.AllorsGenerated;

    using Gtk;

    public class AddObjectTypeWizard : Dialog
    {
        private readonly ErrorMessage singularNameErrorMessage;
        private readonly ErrorMessage pluralNameErrorMessage;

        private readonly Entry singularNameEntry;
        private readonly Entry pluralNameEntry;

        private readonly Repository repository;

        public AddObjectTypeWizard(Repository repository)
        {
            this.repository = repository;

            this.Title = Mono.Unix.Catalog.GetString("Allors Add ObjectType Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.DefaultWidth = 640;
            this.DefaultHeight = 1;
            
            var headerBox = new VBox
                                 {
                                     Spacing = 10, 
                                     BorderWidth = 10
                                 };
            this.VBox.PackStart(headerBox, false, false, 0);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\">Welcome to the Allors Add ObjectType Wizard</span>", 0.5f));
            headerBox.Add(new HtmlLabel("This wizard adds a new object type to your domain.", 0.5f));
            
            var form = new Form();
            this.VBox.PackStart(form);

            this.singularNameErrorMessage = new ErrorMessage();

            this.pluralNameErrorMessage = new ErrorMessage();

            this.pluralNameEntry = new Entry();
            
            var pluralNameLabel = new HtmlLabel("Plural name");
            
            this.singularNameEntry = new Entry();
            
            var singularNameLabel = new HtmlLabel("Singular name");

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
            form.Attach(singularNameLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.singularNameEntry, 0, 1, 1, 2, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.singularNameErrorMessage, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(pluralNameLabel, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.pluralNameEntry, 0, 1, 4, 5, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.pluralNameErrorMessage, 0, 1, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            this.ShowAll();

            this.ResetErrorMessages();
        }

        public ObjectType ObjectType { get; private set; }

        protected void OnButtonOkClicked(object sender, System.EventArgs e)
        {
            var error = false;
            this.ResetErrorMessages();

            var singularName = this.singularNameEntry.Text.Trim();
            if (string.IsNullOrEmpty(singularName))
            {
                error = true;
                this.singularNameErrorMessage.Text = "Singular name is mandatory";
            }

            var pluralName = this.pluralNameEntry.Text.Trim();
            if (string.IsNullOrEmpty(pluralName))
            {
                error = true;
                this.pluralNameErrorMessage.Text = "Plural name is mandatory";
            }

            if (!error)
            {
                var domain = this.repository.Domain;
                this.ObjectType = domain.AddDeclaredObjectType(Guid.NewGuid());
                this.ObjectType.SingularName = singularName;
                this.ObjectType.PluralName = pluralName;

                var validationReport = domain.Validate();
                if (!validationReport.ContainsErrors)
                {
                    this.ObjectType.SendChangedEvent();
                    domain.SendChangedEvent();
                    this.Respond(ResponseType.Ok);
                }
                else
                {
                    this.ObjectType.Delete();
                    domain.SendChangedEvent();

                    // MessageBox.Show(string.Join("\n", validationReport.Messages), "Allors Add ObjectType Wizard - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    foreach (var validationError in validationReport.Errors)
                    {
                        foreach (var member in validationError.Members)
                        {
                            if (member.Equals(AllorsEmbeddedDomain.ObjectTypeSingularName))
                            {
                                this.singularNameErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.ObjectTypePluralName))
                            {
                                this.pluralNameErrorMessage.Text = validationError.Message;
                            }
                        }
                    }
                }
            }

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }

        private void ResetErrorMessages()
        {
            this.singularNameErrorMessage.Text = null;
            this.pluralNameErrorMessage.Text = null;

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }
    }
}