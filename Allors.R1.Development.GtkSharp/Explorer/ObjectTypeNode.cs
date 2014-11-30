//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectTypeNode.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Development.Repository.GtkSharp.Wizards;
    using Allors.R1.Meta;

    using Gtk;

    public class ObjectTypeNode : Node
    {
        private readonly ObjectType objectType;

        public ObjectTypeNode(Tree tree, ObjectType objectType)
            : base(tree, objectType.Id)
        {
            this.objectType = objectType;
            this.SortGroup = 1;
        }

        public ObjectType ObjectType
        {
            get
            {
                return this.objectType;
            }
        }

        public override string Icon
        {
            get
            {
                var isLocked = this.objectType.DomainWhereDeclaredObjectType.IsSuperDomain;
                if (isLocked)
                {
                    return this.ObjectType.IsClass ? "allors-class-locked" : "allors-interface-locked";
                }

                return this.ObjectType.IsClass ? "allors-class" : "allors-interface";
            }
        }

        public override string Name
        {
            get
            {
                return this.ObjectType.Name;
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return new ObjectTypeDecorator(this.Tree.Repository, this.objectType);
            }
        }

        public override void Sync()
        {
            var relationTypes = new HashSet<RelationType>();
            foreach (var associationType in this.ObjectType.AssociationTypesWhereObjectType)
            {
                relationTypes.Add(associationType.RelationType);
            }

            SupertypesNode supertypesNode = null;

            // existing and stale nodes
            foreach (var node in this)
            {
                var inheritancesNode = node as SupertypesNode;
                if (inheritancesNode != null)
                {
                    supertypesNode = inheritancesNode;
                }
                
                var roleTypeNode = node as RoleTypeNode;
                if (roleTypeNode != null)
                {
                    var relationType = roleTypeNode.RelationType;
                    if (relationType != null)
                    {
                        if (relationTypes.Contains(relationType))
                        {
                            roleTypeNode.Sync();
                            relationTypes.Remove(relationType);
                        }
                        else
                        {
                            this.RemoveChild(roleTypeNode);
                        }
                    }
                }
            }

            // new nodes
            if (supertypesNode == null)
            {
                supertypesNode = new SupertypesNode(this.Tree, this.objectType);
                this.AddChild(supertypesNode);
            }

            supertypesNode.Sync();
            
            foreach (var roleType in relationTypes)
            {
                var roleNode = new RoleTypeNode(this.Tree, roleType);
                this.AddChild(roleNode);
                roleNode.Sync();
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            var addRelationTypeMenuItem = new MenuItem("Add Relation Type");
            addRelationTypeMenuItem.Activated += this.AddRelationTypeMenuItemActivated;
            menu.Add(addRelationTypeMenuItem);
            
            if (!this.objectType.DomainWhereDeclaredObjectType.IsSuperDomain)
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

        private void AddRelationTypeMenuItemActivated(object sender, EventArgs e)
        {
            var wizard = new AddRelationTypeWizard(this.Tree.Repository, this.objectType);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(wizard.RelationType.RoleType.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }

        private void PullUpMenuItemActivated(object sender, System.EventArgs e)
        {
            var wizard = new PullUpObjectTypeWizard(this.Tree.Repository, this.objectType);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.objectType.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }

        private void PushDownMenuItemActivated(object sender, EventArgs e)
        {
            var wizard = new PushDownObjectTypeWizard(this.Tree.Repository, this.objectType);
            wizard.Response += (o, args) =>
            {
                if (args.ResponseId == ResponseType.Ok)
                {
                    var node = this.SelectSingleNode(this.objectType.Id);
                    if (node != null)
                    {
                        this.Tree.ExpandToPath(node.Path);
                    }
                }
            };
            wizard.Run();
            wizard.Destroy();
        }
        
        private void DeleteMenuItemActivated(object sender, EventArgs e)
        {
            var messageDialog = new MessageDialog(
                this.Tree.Window, 
                DialogFlags.DestroyWithParent, 
                MessageType.Question, 
                ButtonsType.OkCancel, 
                "Do you really want to delete {0} and all its relations?", 
                this.ObjectType.Name);

            messageDialog.Response += delegate(object o, ResponseArgs resp)
                {
                    if (resp.ResponseId == ResponseType.Ok)
                    {
                        this.ObjectType.DeleteRecursive();
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
