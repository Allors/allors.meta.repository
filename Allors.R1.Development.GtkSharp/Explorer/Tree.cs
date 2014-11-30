//------------------------------------------------------------------------------------------------- 
// <copyright file="Tree.cs" company="Allors bvba">
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
    using System.ComponentModel;

    using Allors.R1.Development.Repository;
    using Allors.R1.Development.Repository.Meta;

    using Gdk;

    using Gtk;

    using Window = Gtk.Window;

    [ToolboxItem(true)]
    public class Tree : NodeView
    {
        private readonly Window window;
        private Repository repository;
        private bool advancedView;

        [Category("Allors")]
        public Tree(Window window)
        {
            Icons.Init();

            this.window = window;
            this.SetSizeRequest(200, 300);

            this.NodeStore = new Nodes();

            var column = new TreeViewColumn();
            this.AppendColumn(column);
            var imageRenderer = new CellRendererPixbuf();
            column.PackStart(imageRenderer, false);
            column.SetAttributes(imageRenderer, "stock_id", 0);

            var textRenderer = new CellRendererText();
            column.PackStart(textRenderer, true);
            column.SetAttributes(textRenderer, "text", 1);

            this.HeadersVisible = false;

            this.ShowAll();
        }

        public event SelectedEventHandler Selected;

        public event EventHandler<RepositoryOpenedEventArgs> RepositoryLoaded;

        public event EventHandler<RepositoryObjectChangedEventArgs> RepositoryObjectChanged;

        public event EventHandler<RepositoryObjectDeletedEventArgs> RepositoryObjectDeleted;

        public event EventHandler<RepositoryMetaObjectChangedEventArgs> MetaObjectChanged;

        public event EventHandler<RepositoryMetaObjectDeletedEventArgs> MetaObjectDeleted;

        public Nodes Nodes
        {
            get
            {
                return (Nodes)this.NodeStore;
            }
        }

        public bool AdvancedView 
        {
            get
            {
                return this.advancedView;
            }

            set
            {
                this.advancedView = value;
                this.Nodes.RepositoryNode.Sync();
            }
        }

        public Repository Repository
        {
            get
            {
                return this.repository;
            }
        }

        public Window Window
        {
            get
            {
                return this.window;
            }
        }

        public void Open(Repository repositoryToOpen)
        {
            this.repository = repositoryToOpen;
            this.repository.ObjectChanged += this.OnRepositoryObjectChanged;
            this.repository.ObjectDeleted += this.OnRepositoryObjectDeleted;
            this.repository.MetaObjectChanged += this.OnMetaObjectChanged;
            this.repository.MetaObjectDeleted += this.OnMetaObjectDeleted;

            foreach (Node node in this)
            {
                this.NodeStore.RemoveNode(node);
            }

            var repositoryNode = new RepositoryNode(this);
            this.NodeStore.AddNode(repositoryNode);
            repositoryNode.Sync();

            this.ExpandRow(new TreePath("0"), false);

            var repositoryLoaded = this.RepositoryLoaded;
            if (repositoryLoaded != null)
            {
                repositoryLoaded(this, new RepositoryOpenedEventArgs(this.repository));
            }
        }

        protected override void OnCursorChanged()
        {
            base.OnCursorChanged();

            if (this.Selected != null)
            {
                var rows = this.Selection.GetSelectedRows();
                if (rows.Length > 0)
                {
                    var node = (Node)this.NodeStore.GetNode(rows[0]);
                    this.Selected(this, new SelectedEventArgs(this.repository, node.Tag, node.PropertyGridDecorator));
                }
            }
        }

        protected override bool OnButtonPressEvent(EventButton evnt)
        {
            TreePath treePath;
            if (this.GetPathAtPos((int)evnt.X, (int)evnt.Y, out treePath))
            {
                var node = this.NodeStore.GetNode(treePath) as Node;
                if (node != null)
                {
                    // check if the right button was pushed
                    if ((int)evnt.Button == 3)
                    {
                        node.PopupMenu();
                    }

                    this.Selected(this, new SelectedEventArgs(this.repository, node.Tag, node.PropertyGridDecorator));
                }
            }

            return base.OnButtonPressEvent(evnt);
        }

        private void OnMetaObjectChanged(object sender, RepositoryMetaObjectChangedEventArgs args)
        {
            var metaObjectChanged = this.MetaObjectChanged;
            if (metaObjectChanged != null)
            {
                metaObjectChanged(this, args);
            }

            this.Nodes.RepositoryNode.Sync();
        }

        private void OnMetaObjectDeleted(object sender, RepositoryMetaObjectDeletedEventArgs args)
        {
            var metaObjectDeleted = this.MetaObjectDeleted;
            if (metaObjectDeleted != null)
            {
                metaObjectDeleted(this, args);
            }

            var metaObjectId = args.MetaObjectId;
            var nodes = this.Nodes.SelectSingleNode(metaObjectId);
            foreach (var node in nodes)
            {
                node.Remove();
            }

            this.Nodes.RepositoryNode.Sync();
        }

        private void OnRepositoryObjectChanged(object sender, RepositoryObjectChangedEventArgs args)
        {
            var objectChanged = this.RepositoryObjectChanged;
            if (objectChanged != null)
            {
                objectChanged(this, args);
            }

            this.Nodes.RepositoryNode.Sync();
        }

        private void OnRepositoryObjectDeleted(object sender, RepositoryObjectDeletedEventArgs args)
        {
            var objectDeleted = this.RepositoryObjectDeleted;
            if (objectDeleted != null)
            {
                objectDeleted(this, args);
            }

            this.Nodes.RepositoryNode.Sync();
        }
    }
}