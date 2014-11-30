//------------------------------------------------------------------------------------------------- 
// <copyright file="AddSuperDomainWizard.cs" company="Allors bvba">
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
    using System.IO;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    using Gtk;

    public class AddSuperDomainWizard : Dialog
    {
        private readonly FileChooserButton fileChooserButton;
        private readonly ErrorMessage superDomainErrorMessage;

        private readonly Repository repository;

        private Domain superDomain;

        public AddSuperDomainWizard(Repository repository)
        {
            this.repository = repository;

            this.Title = Mono.Unix.Catalog.GetString("Add Super Domain Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.WindowPosition = WindowPosition.CenterOnParent;
            this.DefaultWidth = 640;
            this.DefaultHeight = 1;

            var headerBox = new VBox
                                 {
                                     Spacing = 10, 
                                     BorderWidth = 10
                                 };
            this.VBox.PackStart(headerBox, false, false, 0);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\">Welcome to the Allors Add Super Domain Wizard</span>", 0.5f));
            headerBox.PackStart(new HtmlLabel("This wizard makes this domain inherit from a new super domain.", 0.5f));

            var form = new Form();
            this.VBox.PackStart(form);

            this.fileChooserButton = new FileChooserButton(Mono.Unix.Catalog.GetString("Select a Domain"), 0);

            this.superDomainErrorMessage = new ErrorMessage();
            
            this.ActionArea.Spacing = 10;
            this.ActionArea.BorderWidth = 5;
            this.ActionArea.LayoutStyle = ButtonBoxStyle.End;

            var buttonCancel = new Button
                                    {
                                        CanDefault = true,
                                        CanFocus = true,
                                        UseStock = true,
                                        UseUnderline = true,
                                        Label = "gtk-cancel"
                                    };
            this.AddActionWidget(buttonCancel, -6);
            
            var buttonOk = new Button
                                {
                                    CanDefault = true,
                                    CanFocus = true,
                                    UseStock = true,
                                    UseUnderline = true,
                                    Label = "gtk-ok"
                                };
            buttonOk.Clicked += this.OnButtonOkClicked;
            this.ActionArea.PackStart(buttonOk);
            
            // Layout
            form.Attach(this.fileChooserButton, 0, 1, 0, 1, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.superDomainErrorMessage, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            this.ShowAll();

            this.ResetErrorMessages();

            var filter = new FileFilter { Name = "Allors repository (*.repository)" };
            filter.AddPattern("*.repository");
            this.fileChooserButton.AddFilter(filter);
        }

        public Domain SuperDomain
        {
            get
            {
                return this.superDomain;
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            var error = false;
            this.ResetErrorMessages();

            var superDomainFileName = this.fileChooserButton.Filename;
            if (string.IsNullOrWhiteSpace(superDomainFileName) || !new FileInfo(superDomainFileName).Exists)
            {
                error = true;
                this.superDomainErrorMessage.Text = "File does not exist";
            }

            if (!error)
            {
                var directoryInfo = new FileInfo(superDomainFileName).Directory;
                this.superDomain = this.repository.AddSuper(directoryInfo);

                this.repository.Domain.SendChangedEvent();
                this.Respond(ResponseType.Ok);
            }

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }

        private void ResetErrorMessages()
        {
            this.superDomainErrorMessage.Text = null;

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }
    }
}