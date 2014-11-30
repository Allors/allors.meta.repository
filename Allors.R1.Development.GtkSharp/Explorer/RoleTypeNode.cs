//------------------------------------------------------------------------------------------------- 
// <copyright file="RoleTypeNode.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Development.Repository.GtkSharp.Wizards;
    using Allors.R1.Meta;

    using Gtk;

    public class RoleTypeNode : Node
    {
        private readonly RelationType relationType;

        public RoleTypeNode(Tree tree, RelationType relationType)
            : base(tree, relationType.Id)
        {
            this.relationType = relationType;
            this.SortGroup = 2;
        }

        public RelationType RelationType
        {
            get
            {
                return this.relationType;
            }
        }

        public override string Icon
        {
            get
            {
                var isLocked = this.relationType.DomainWhereDeclaredRelationType.IsSuperDomain;
                return isLocked ? "allors-role-locked" : "allors-role";
            }
        }

        public override string Name
        {
            get
            {
                return this.RelationType.Name;
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return new RelationTypeDecorator(this.Tree.Repository, this.RelationType);
            }
        }

        public override void Sync()
        {
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            if (!this.relationType.DomainWhereDeclaredRelationType.IsSuperDomain)
            {
                var pullUpMenuItem = new MenuItem("Pull Up");
                pullUpMenuItem.Activated += this.PullUpMenuItemActivated;
                menu.Add(pullUpMenuItem);

                var separator = new SeparatorMenuItem();
                menu.Append(separator);

                var deleteMenuItem = new MenuItem("Delete");
                deleteMenuItem.Activated += this.DeleteMenuItemActivated;
                menu.Add(deleteMenuItem);
            }
            else
            {
                var pushDownMenuItem = new MenuItem("Push Down");
                pushDownMenuItem.Activated += this.PushDownMenuItemActivated;
                menu.Add(pushDownMenuItem);
            }

            menu.ShowAll();
            menu.Popup();
        }

        private void PullUpMenuItemActivated(object sender, System.EventArgs e)
        {
            var wizard = new PullUpRelationTypeWizard(this.Tree.Repository, this.RelationType);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.relationType.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }

        private void PushDownMenuItemActivated(object sender, System.EventArgs e)
        {
            var wizard = new PushDownRelationTypeWizard(this.Tree.Repository, this.RelationType);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.relationType.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }

        private void DeleteMenuItemActivated(object sender, System.EventArgs e)
        {
            var messageDialog = new MessageDialog(
                    this.Tree.Window,
                    DialogFlags.DestroyWithParent,
                    MessageType.Question,
                    ButtonsType.OkCancel,
                    "Do you really want to delete {0}?",
                    this.RelationType.Name);

            messageDialog.Response += delegate(object o, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    this.RelationType.Delete();
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
