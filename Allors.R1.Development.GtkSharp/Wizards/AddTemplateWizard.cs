//------------------------------------------------------------------------------------------------- 
// <copyright file="AddTemplateWizard.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.Generation;

    using Gtk;

    public class AddTemplateWizard : Dialog
    {
        private readonly ErrorMessage nameErrorMessage;
        private readonly ErrorMessage locationErrorMessage;

        private readonly Entry nameEntry;
        private readonly Entry locationEntry;

        private readonly Repository repository;

        private Template template;

        public AddTemplateWizard(Repository repository)
        {
            this.repository = repository;

            this.Title = Mono.Unix.Catalog.GetString("Add Template Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.DefaultWidth = 640;
            this.DefaultHeight = 1;
            
            var headerBox = new VBox
                                 {
                                     Spacing = 10, 
                                     BorderWidth = 10
                                 };
            this.VBox.PackStart(headerBox, false, false, 0);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\">Welcome to the Allors Add Template Wizard</span>", 0.5f));
            headerBox.PackStart(new HtmlLabel("This wizard adds a new template to your repository.", 0.5f));
           
            var form = new Form();
            this.VBox.PackStart(form);

            var nameLabel = new HtmlLabel("Name");
            this.nameEntry = new Entry();
            this.nameErrorMessage = new ErrorMessage();

            var locationLabel = new HtmlLabel("Location (Url or filename)");
            this.locationEntry = new Entry();
            this.locationErrorMessage = new ErrorMessage();
           
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
            form.Attach(nameLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.nameEntry, 0, 1, 1, 2, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.nameErrorMessage, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(locationLabel, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.locationEntry, 0, 1, 4, 5, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.locationErrorMessage, 0, 1, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            
            this.ShowAll();

            this.ResetErrorMessages();
        }

        public Template Template 
        {
            get
            {
                return this.template;
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs eventArgs)
        {
            var error = false;
            this.ResetErrorMessages();

            var name = this.nameEntry.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                error = true;
                this.nameErrorMessage.Text = "Name is mandatory";
            }

            var location = this.locationEntry.Text.Trim();
            if (string.IsNullOrEmpty(location))
            {
                error = true;
                this.locationErrorMessage.Text = "Location is mandatory";
            }

            try
            {
                var uri = new Uri(location);
                if (!StringTemplate.IsValid(uri))
                {
                    error = true;
                    this.locationErrorMessage.Text = "Template Source is not valid.";
                }
            }
            catch (Exception e)
            {
                error = true;
                this.locationErrorMessage.Text = "Template Source is an illegal URL.\n\n" + e.Message;
            }

            if (!error)
            {
                try
                {
                    this.template = this.repository.AddTemplate();
                    this.template.Name = name;
                    this.template.Source = new Uri(location);
                    this.Respond(ResponseType.Ok);
                }
                catch (Exception e)
                {
                    this.locationErrorMessage.Text = "Could not create Template.\n\n" + e.Message;
                }
            }

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }

        private void ResetErrorMessages()
        {
            this.nameErrorMessage.Text = null;
            this.locationErrorMessage.Text = null;

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }
    }
}

