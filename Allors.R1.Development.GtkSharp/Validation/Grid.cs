//------------------------------------------------------------------------------------------------- 
// <copyright file="Grid.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository;

    using Gtk;

    [System.ComponentModel.ToolboxItem(true)]
    public class Grid : NodeView
    {
        public Grid(Explorer.Tree explorer)
        {
            this.SetSizeRequest(200, 300);

            this.NodeStore = new NodeStore(typeof(Node));

            var column = new TreeViewColumn();
            var imageRenderer = new CellRendererPixbuf();
            column.PackStart(imageRenderer, false);
            column.SetAttributes(imageRenderer, "stock_id", 0);
            this.AppendColumn(column);

            column = new TreeViewColumn();
            var textRenderer = new CellRendererText();
            column.Title = "Source";
            column.PackStart(textRenderer, true);
            column.SetAttributes(textRenderer, "text", 1);
            this.AppendColumn(column);

            column = new TreeViewColumn();
            textRenderer = new CellRendererText();
            column.Title = "Kind";
            column.PackStart(textRenderer, true);
            column.SetAttributes(textRenderer, "text", 2);
            this.AppendColumn(column);

            column = new TreeViewColumn();
            var messageTextRenderer = new CellRendererText();
            column.Title = "Message";
            column.PackStart(messageTextRenderer, true);
            column.SetAttributes(messageTextRenderer, "text", 3);
            this.AppendColumn(column);

            this.HeadersVisible = true;

            this.ShowAll();

            explorer.RepositoryLoaded += (sender, args) => this.Validate(args.Repository);
            explorer.RepositoryObjectChanged += (sender, args) => this.Validate(args.Repository);
            explorer.RepositoryObjectDeleted += (sender, args) => this.Validate(args.Repository);
            explorer.MetaObjectChanged += (sender, args) => this.Validate(args.Repository);
            explorer.MetaObjectDeleted += (sender, args) => this.Validate(args.Repository);
        }

        public NodeStore Nodes
        {
            get
            {
                return (NodeStore)this.NodeStore;
            }
        }

        private void Validate(Repository repository)
        {
            var validationLog = repository.Domain.Validate();

            this.Nodes.Clear();

            foreach (var error in validationLog.Errors)
            {
                this.Nodes.AddNode(new ErrorNode(error));
            }
        }
    }
}