//------------------------------------------------------------------------------------------------- 
// <copyright file="SuperDomainsNode.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;

    using Allors.R1.Development.Repository.GtkSharp.Wizards;
    using Allors.R1.Meta;

    using Gtk;

    public class SuperDomainsNode : Node
    {
        public static readonly Guid UniqueId = new Guid("179CC907-45F9-4321-A317-8E58F50B6D01");

        public SuperDomainsNode(Tree tree)
            : base(tree, UniqueId)
        {
            this.SortGroup = -2;
        }

        public override string Icon
        {
            get
            {
                // See Gtk.Stock
                return "gtk-directory";
            }
        }

        public override string Name
        {
            get
            {
                return "Super Domains";
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return null;
            }
        }

        public override void Sync()
        {
            var domain = this.Tree.Repository.Domain;
            var superDomains = new HashSet<Domain>(domain.DirectSuperDomains);

            // existing and stale nodes
            foreach (var node in this)
            {
                var superDomainNode = node as SuperDomainNode;
                if (superDomainNode != null)
                {
                    var superDomain = superDomainNode.Domain;
                    if (superDomain != null)
                    {
                        if (superDomains.Contains(superDomain))
                        {
                            superDomainNode.Sync();
                            superDomains.Remove(superDomain);
                        }
                        else
                        {
                            this.RemoveChild(superDomainNode);
                        }
                    }
                }
            }

            // new nodes
            foreach (var superDomain in superDomains)
            {
                if (!superDomain.IsAllorsUnitDomain)
                {
                    var node = new SuperDomainNode(this.Tree, superDomain);
                    this.AddChild(node);
                    node.Sync();
                }
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            var addSuperDomainMenuItem = new MenuItem("Add Super domain");
            addSuperDomainMenuItem.Activated += this.AddSuperDomainMenuItemActivated;
            menu.Add(addSuperDomainMenuItem);

            menu.ShowAll();
            menu.Popup();
        }

        private void AddSuperDomainMenuItemActivated(object sender, EventArgs e)
        {
            var wizard = new AddSuperDomainWizard(this.Tree.Repository);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(wizard.SuperDomain.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }
    }
}
