//------------------------------------------------------------------------------------------------- 
// <copyright file="ErrorNode.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Validation
{
    using Allors.R1.Meta;

    using Gtk;

    public class ErrorNode : Node
    {
        private readonly ValidationError error;

        public ErrorNode(ValidationError error)
            : base(error)
        {
            this.error = error;
        }

        public override string Icon
        {
            get
            {
                return "gtk-dialog-error";
            }
        }

        public override string Source
        {
            get
            {
                return this.error.Source.ToString();
            }
        }

        public override string Kind
        {
            get
            {
                return this.error.Kind.ToString();
            }
        }

        public override string Message
        {
            get
            {
                return this.error.Message;
            }
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            menu.ShowAll();
            menu.Popup();
        }
    }
}
