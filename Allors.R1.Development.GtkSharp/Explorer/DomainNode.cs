//------------------------------------------------------------------------------------------------- 
// <copyright file="NamespaceNode.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Development.Repository.GtkSharp.Wizards;
    using Allors.R1.Meta;

    using Gtk;

    public class DomainNode : Node
    {
        private readonly Domain domain;

        public DomainNode(Tree tree, Domain domain)
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
                var isLocked = this.domain.IsSuperDomain;
                return isLocked ? "allors-namespace-locked" : "allors-namespace";
            }
        }

        public override string Name
        {
            get
            {
                return this.Domain.Name;
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return new DomainDecorator(this.domain);
            }
        }

        public override void Sync()
        {
            var objectTypes = new HashSet<ObjectType>(this.Domain.DeclaredObjectTypes);

            // existing and stale nodes
            foreach (var node in this)
            {
                var objectTypeNode = node as ObjectTypeNode;
                if (objectTypeNode != null)
                {
                    var objectType = objectTypeNode.ObjectType;
                    if (objectType != null)
                    {
                        if (objectTypes.Contains(objectType))
                        {
                            objectTypeNode.Sync();
                            objectTypes.Remove(objectType);
                        }
                        else
                        {
                            this.RemoveChild(objectTypeNode);
                        }
                    }
                }
            }

            // new nodes
            foreach (var objectType in objectTypes)
            {
                var objectTypeNode = new ObjectTypeNode(this.Tree, objectType);
                this.AddChild(objectTypeNode);
                objectTypeNode.Sync();
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
            var menu = new Menu();
           
            if (this.domain.IsSuperDomain)
            {
                var deleteMenuItem = new MenuItem("Delete");
                deleteMenuItem.Activated += this.DeleteMenuItemActivated;
                menu.Add(deleteMenuItem);
            }
            else
            {
                var addObjectTypeMenuItem = new MenuItem("Add Object Type");
                addObjectTypeMenuItem.Activated += this.AddObjectTypeMenuItemActivated;
                menu.Add(addObjectTypeMenuItem);
            }

            menu.ShowAll();
            menu.Popup();
        }
        
        private void AddObjectTypeMenuItemActivated(object sender, System.EventArgs e)
        {
            var wizard = new AddObjectTypeWizard(this.Tree.Repository);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(wizard.ObjectType.Id);
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
            // TODO: Remove superdomain
        }
    }
}
