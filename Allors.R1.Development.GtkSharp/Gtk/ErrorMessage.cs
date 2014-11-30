//------------------------------------------------------------------------------------------------- 
// <copyright file="ErrorMessage.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp
{
    using Allors.R1.Development.Repository.GtkSharp.Extensions;

    using Gtk;

    public class ErrorMessage : HBox
    {
        private readonly Image image;
        private readonly Label label;

        public ErrorMessage()
        {
            this.Spacing = 6;
            this.BorderWidth = 1;
            
            this.image = new Image
            {
                WidthRequest = 15,
                Pixbuf = Icons.LoadIcon(this, "gtk-dialog-error", IconSize.Menu)
            };
            this.PackStart(this.image);
            var roleObjectTypeErrorImageLayout = (BoxChild)this[this.image];
            roleObjectTypeErrorImageLayout.Expand = false;

            this.label = new Label
            {
                Xalign = 0,
            };
            this.PackStart(this.label);
        }

        public string Text
        {
            get
            {
                return this.label.Text;
            }

            set
            {
                this.label.Text = value.TrimToNull();

                var visible = !string.IsNullOrWhiteSpace(this.Text);
                this.image.Visible = visible;
                this.label.Visible = visible; 
            }
        }
    }
}
