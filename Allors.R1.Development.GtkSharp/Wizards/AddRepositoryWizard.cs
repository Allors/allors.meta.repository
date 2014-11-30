//------------------------------------------------------------------------------------------------- 
// <copyright file="AddRepositoryWizard.cs" company="Allors bvba">
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
    using System.IO;

    using Allors.R1.Development.Repository;

    using Gtk;

    public class AddRepositoryWizard : Dialog
    {
        private readonly Entry nameEntry;
        private readonly ErrorMessage nameErrorMessage;
 
        private readonly Entry locationEntry;
        private readonly Button locationOpenButton;
        private readonly ErrorMessage locationErrorMessage;

        private readonly Button buttonOk;

        private DirectoryInfo directoryInfo;

        public AddRepositoryWizard()
        {
            this.Title = Mono.Unix.Catalog.GetString("Allors Add New Repository Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.DefaultWidth = 640;
            this.DefaultHeight = 1;
            
            var headerBox = new VBox
                                 {
                                     Spacing = 5, 
                                     BorderWidth = 5
                                 };
            this.VBox.Add(headerBox);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\">Welcome to the Allors Add New Repository Wizard</span>", 0.5f));
            headerBox.PackStart(new HtmlLabel("Create a new repository.", 0.5f));

            var form = new Form();
            this.VBox.PackStart(form);

            var nameLabel = new HtmlLabel("Domain name");
            this.nameEntry = new Entry();
            this.nameErrorMessage = new ErrorMessage();

            var locationLabel = new HtmlLabel("Location");
            var locationHBox = new HBox { Spacing = 6 };
            this.locationEntry = new Entry();
            locationHBox.PackStart(this.locationEntry);
            this.locationOpenButton = new Button
                                  {
                                      UseStock = true, 
                                      UseUnderline = true, 
                                      Label = "gtk-open"
                                  };
            this.locationOpenButton.Clicked += this.OnLocationOpenButtonClicked;
            locationHBox.PackStart(this.locationOpenButton);
            var openButtonLayout = (Box.BoxChild)locationHBox[this.locationOpenButton];
            openButtonLayout.Expand = false;
            this.locationErrorMessage = new ErrorMessage();

            var buttonCancel = new Button
                                    {
                                        CanDefault = true, 
                                        UseStock = true, 
                                        UseUnderline = true, 
                                        Label = "gtk-cancel"
                                    };
            this.AddActionWidget(buttonCancel, -6);
            
            this.buttonOk = new Button
                                {
                                    CanDefault = true, 
                                    UseStock = true, 
                                    UseUnderline = true, 
                                    Label = "gtk-ok"
                                };
            this.buttonOk.Clicked += this.OnButtonOkClicked;
            this.ActionArea.PackStart(this.buttonOk);
            
            // Layout
            form.Attach(nameLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.nameEntry, 0, 1, 1, 2, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.nameErrorMessage, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(locationLabel, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(locationHBox, 0, 1, 4, 5, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.locationErrorMessage, 0, 1, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            this.ShowAll();

            this.ResetErrorMessages();
        }

        public Repository Repository { get; private set; }

        protected void OnLocationOpenButtonClicked(object sender, System.EventArgs e)
        {
            var fileChooser = new FileChooserDialog(
                   "Select Location",
                   this,
                   FileChooserAction.SelectFolder,
                   "Cancel",
                   ResponseType.Cancel,
                   "Select",
                   ResponseType.Accept);

            try
            {
                if (fileChooser.Run() == (int)ResponseType.Accept)
                {
                    this.directoryInfo = new DirectoryInfo(fileChooser.Filename);
                    this.locationEntry.Text = this.directoryInfo.FullName;
                }
            }
            finally
            {
                fileChooser.Destroy();
            }
        }

        protected void OnButtonOkClicked(object sender, System.EventArgs e)
        {
            var error = false;
            this.ResetErrorMessages();

            var name = this.nameEntry.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                error = true;
                this.nameErrorMessage.Text = "domain name is mandatory";
            }

            if (string.IsNullOrEmpty(this.locationEntry.Text))
            {
                error = true;
                this.locationErrorMessage.Text = "Location is mandatory";
            }

            this.directoryInfo = new DirectoryInfo(this.locationEntry.Text);
            if (!this.directoryInfo.Exists)
            {
                this.directoryInfo.Create();
                this.directoryInfo.Refresh();
                if (!this.directoryInfo.Exists)
                {
                    error = true;
                    this.locationErrorMessage.Text = "Could not create directory";
                }
            }

            if (!error)
            {
                this.Repository = new Repository(this.directoryInfo, true);
                this.Repository.Domain.Name = name;
                this.Repository.Domain.SendChangedEvent();

                this.Respond(ResponseType.Ok);
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