//------------------------------------------------------------------------------------------------- 
// <copyright file="DomainNode.cs" company="Allors bvba">
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
    using System.Linq;

    using Allors.R1.Development.Repository.Generation;
    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Meta;

    using Gtk;

    public class RepositoryNode : Node
    {
        public RepositoryNode(Tree tree)
            : base(tree, tree.Repository)
        {
        }

        public override string Icon
        {
            get
            {
                return "allors-program";
            }
        }

        public override string Name
        {
            get
            {
                return "Repository";
            }
        }

        public override object PropertyGridDecorator
        {
            get
            {
                return new DomainDecorator(this.Tree.Repository.Domain);
            }
        }

        public override void Sync()
        {
            var repository = this.Tree.Repository;
            var domains = new HashSet<Domain>(repository.Domain.Domains.Where(domain => !domain.IsAllorsUnitDomain));
            var existingRelationTypes = new HashSet<RelationType>();
            
            // existing and stale nodes
            foreach (var node in this)
            {
                var domainNomde = node as DomainNode;
                if (domainNomde != null)
                {
                    var domain = domainNomde.Domain;
                    if (domain != null)
                    {
                        if (domains.Contains(domain))
                        {
                            domainNomde.Sync();
                            domains.Remove(domain);
                        }
                        else
                        {
                            this.RemoveChild(domainNomde);
                        }
                    }
                }
                
                var roleTypeNode = node as RoleTypeNode;
                if (roleTypeNode != null)
                {
                    var relationType = roleTypeNode.RelationType;
                    if (relationType.ExistAssociationType)
                    {
                        this.RemoveChild(roleTypeNode);
                    }
                    else
                    {
                        existingRelationTypes.Add(relationType);
                    }
                }
            }

            // new nodes
            foreach (var domain in domains)
            {
                var node = new DomainNode(this.Tree, domain);
                this.AddChild(node);
                node.Sync();
            }

            foreach (var relationType in repository.Domain.RelationTypes)
            {
                if (!relationType.ExistAssociationType && !existingRelationTypes.Contains(relationType))
                {
                    var node = new RoleTypeNode(this.Tree, relationType);
                    this.AddChild(node);
                    node.Sync();
                }
            }

            var superDomainsNode = this.SelectSingleNode(SuperDomainsNode.UniqueId);
            if (this.Tree.AdvancedView)
            {
                if (superDomainsNode == null)
                {
                    superDomainsNode = new SuperDomainsNode(this.Tree);
                    this.AddChild(superDomainsNode);
                }

                superDomainsNode.Sync();
            }
            else
            {
                if (superDomainsNode != null)
                {
                    this.RemoveChild(superDomainsNode);
                }
            }

            var templatesNode = this.SelectSingleNode(TemplatesNode.UniqueId);
            if (this.Tree.AdvancedView)
            {
                if (templatesNode == null)
                {
                    templatesNode = new TemplatesNode(this.Tree);
                    this.AddChild(templatesNode);
                }

                templatesNode.Sync();
            }
            else
            {
                if (templatesNode != null)
                {
                    this.RemoveChild(templatesNode);
                }
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
            var menu = new Menu();

            var generateMenuItem = new MenuItem("Generate");
            generateMenuItem.Activated += this.GenerateMenuItemActivated;
            menu.Add(generateMenuItem);

            menu.ShowAll();
            menu.Popup();
        }

        private void GenerateMenuItemActivated(object sender, EventArgs e)
        {
            var templates = this.Tree.Repository.Templates;

            var log = new Log();

            if (templates.Length > 0)
            {
                foreach (Template template in templates)
                {
                    template.Generate(log);
                }
            }
            else
            {
                using (
                    var messageDialog = new MessageDialog(
                        this.Tree.Window,
                        DialogFlags.DestroyWithParent,
                        MessageType.Error,
                        ButtonsType.Close,
                        "No templates defined"))
                {
                    messageDialog.Run();
                }
            }

            if (log.HasErrors)
            {
                var message = log.Message;

                using (
                     var messageDialog = new MessageDialog(
                         this.Tree.Window,
                         DialogFlags.DestroyWithParent,
                         MessageType.Error,
                         ButtonsType.Close,
                         message))
                {
                    messageDialog.Run();
                } 
            }
        }

        private class Log : Allors.R1.Development.Repository.Log
        {
            private readonly List<string> errors;

            public Log()
            {
                this.errors = new List<string>();
            }

            public bool HasErrors
            {
                get
                {
                    return this.errors.Count > 0;
                }
            }

            public string Message
            {
                get
                {
                    return string.Join("\n", this.errors);
                }
            }

            public override void Error(object sender, string message)
            {
                this.errors.Add(message);
            }
        }
    }
}
