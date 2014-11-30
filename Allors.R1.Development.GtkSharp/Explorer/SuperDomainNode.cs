//------------------------------------------------------------------------------------------------- 
// <copyright file="SuperDomainNode.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Explorer
{
    using System.Linq;

    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Meta;

    using Gtk;

    public class SuperDomainNode : Node
    {
        private readonly Domain domain;

        public SuperDomainNode(Tree tree, Domain domain)
            : base(tree, domain.Id)
        {
            this.domain = domain;
            this.SortGroup = 0;
        }

        public Domain Domain
        {
            get
            {
                return this.domain;
            }
        }

        public override string Icon
        {
            get
            {
                var isLocked = this.domain.DomainsWhereDirectSuperDomain.Contains(this.Tree.Repository.Domain);
                return isLocked ? "allors-domain-locked" : "allors-domain";
            }
        }

        public override string Name
        {
            get
            {
                return this.domain.Name;
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return new SuperDomainDecorator(this.domain);
            }
        }

        public override void Sync()
        {
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            var deleteMenuItem = new MenuItem("Delete");
            deleteMenuItem.Activated += this.DeleteMenuItemActivated;
            menu.Add(deleteMenuItem);

            menu.ShowAll();
            menu.Popup();
        }

        private void DeleteMenuItemActivated(object sender, System.EventArgs e)
        {
            var messageDialog = new MessageDialog(
                    this.Tree.Window,
                    DialogFlags.DestroyWithParent,
                    MessageType.Question,
                    ButtonsType.OkCancel,
                    "Do you really want to delete {0}?",
                    this.domain.Name);

            messageDialog.Response += delegate(object o, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    this.Tree.Repository.RemoveSuper(this.domain);
                }
            };

            try
            {
                messageDialog.Run();
            }
            finally
            {
                messageDialog.Destroy();
            }
        }
    }
}
