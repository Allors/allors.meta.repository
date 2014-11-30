//------------------------------------------------------------------------------------------------- 
// <copyright file="SupertypeNode.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.GtkSharp.Wizards;
    using Allors.R1.Meta;

    using Gtk;

    public class SupertypeNode : Node
    {
        private readonly Inheritance inheritance;

        public SupertypeNode(Tree tree, Inheritance inheritance)
            : base(tree, inheritance.Id)
        {
            this.inheritance = inheritance;
            this.SortGroup = 0;
        }

        public Inheritance Inheritance
        {
            get
            {
                return this.inheritance;
            }
        }

        public ObjectType SuperObjectType
        {
            get
            {
                return this.inheritance.Supertype;
            }
        }

        public override string Icon
        {
            get
            {
                var isLocked = this.inheritance.DomainWhereDeclaredInheritance.IsSuperDomain;
                if (isLocked)
                {
                    return this.SuperObjectType.IsClass ? "allors-class-locked" : "allors-interface-locked";
                }

                return this.SuperObjectType.IsClass ? "allors-class" : "allors-interface";
            }
        }

        public override string Name
        {
            get
            {
                return this.SuperObjectType.Name;
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                // TODO: 
                //return new InheritanceDecorator(this.Tree.Repository, this.inheritance);
                return null;
            }
        }

        public override void Sync()
        {
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            if (!this.inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
            {
                var pullUpMenuItem = new MenuItem("Pull Up");
                pullUpMenuItem.Activated += this.PullUpMenuItemActivated;
                menu.Add(pullUpMenuItem);

                var separator = new SeparatorMenuItem();
                menu.Append(separator);

                var removeMenuItem = new MenuItem("Remove");
                removeMenuItem.Activated += this.RemoveMenuItemActivated;
                menu.Add(removeMenuItem);
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
            var wizard = new PullUpInheritanceWizard(this.Tree.Repository, this.inheritance);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.Inheritance.Id);
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
            var wizard = new PushDownInheritanceWizard(this.Tree.Repository, this.inheritance);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.Inheritance.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }

        private void RemoveMenuItemActivated(object sender, System.EventArgs e)
        {
            var messageDialog = new MessageDialog(
                    this.Tree.Window,
                    DialogFlags.DestroyWithParent,
                    MessageType.Question,
                    ButtonsType.OkCancel,
                    "Do you really want to delete {0}?",
                    this.inheritance);

            messageDialog.Response += delegate(object o, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    // TODO:
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
