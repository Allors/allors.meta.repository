//------------------------------------------------------------------------------------------------- 
// <copyright file="TemplatesNode.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository.Generation;
    using Allors.R1.Development.Repository.GtkSharp.Wizards;

    using Gtk;

    public class TemplatesNode : Node
    {
        public static readonly Guid UniqueId = new Guid("01B3FE6D-41AF-42E0-B309-B4A89A932E8C");

        public TemplatesNode(Tree tree)
            : base(tree, UniqueId)
        {
            this.SortGroup = -1;
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
                return "Templates";
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
            var repository = this.Tree.Repository;
            var templates = new HashSet<Template>(repository.Templates);

            // existing and stale nodes
            foreach (var node in this)
            {
                var templateNode = node as TemplateNode;
                if (templateNode != null)
                {
                    var template = templateNode.Template;
                    if (template != null)
                    {
                        if (templates.Contains(template))
                        {
                            templateNode.Sync();
                            templates.Remove(template);
                        }
                        else
                        {
                            this.RemoveChild(templateNode);
                        }
                    }
                }
            }

            // new nodes
            foreach (var template in templates)
            {
                var node = new TemplateNode(this.Tree, template);
                this.AddChild(node);
                node.Sync();
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            var addTemplateMenuItem = new MenuItem("Add Template");
            addTemplateMenuItem.Activated += this.AddTemplateMenuItemActivated;
            menu.Add(addTemplateMenuItem);

            menu.ShowAll();
            menu.Popup();
        }

        private void AddTemplateMenuItemActivated(object sender, System.EventArgs e)
        {
            var wizard = new AddTemplateWizard(this.Tree.Repository);
            wizard.Response += (o, args) =>
                {
                    if (args.ResponseId == ResponseType.Ok)
                    {
                        var node = this.SelectSingleNode(wizard.Template.Id);
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
