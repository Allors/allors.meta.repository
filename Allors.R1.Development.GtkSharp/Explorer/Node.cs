//------------------------------------------------------------------------------------------------- 
// <copyright file="Node.cs" company="Allors bvba">
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
    using System.Linq;

    using Gtk;

    public abstract class Node : TreeNode<Node>
    {
        private readonly Tree tree;

        protected Node(Tree tree, object tag)
            : base(tag)
        {
            this.tree = tree;
        }

        public Tree Tree
        {
            get
            {
                return this.tree;
            }
        }

        public TreePath Path
        {
            get
            {
                var path = new TreePath();
                this.CreatePath(path);
                return path;
            }
        }

        public int SortGroup { get; set; }

        [TreeNodeValue(Column = 0)]
        public abstract string Icon { get; }

        [TreeNodeValue(Column = 1)]
        public abstract string Name { get; }

        public abstract object PropertyGridDecorator { get; }

        public abstract void Sync();

        public abstract void PopupMenu();

        public void Sort()
        {
            this.Sort(
                (firstNode, secondNode) =>
                {
                    if (firstNode.SortGroup == secondNode.SortGroup)
                    {
                        if (firstNode.Name != null)
                        {
                            return string.Compare(firstNode.Name, secondNode.Name, StringComparison.OrdinalIgnoreCase);
                        }
                    }

                    return firstNode.SortGroup - secondNode.SortGroup;
                });
        }

        public void Remove()
        {
            var parent = this.Parent as Node;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        private void CreatePath(TreePath path)
        {
            var parent = this.Parent as Node;
            if (parent != null)
            {
                var index = parent.IndexOf(this);
                path.PrependIndex(index);
                parent.CreatePath(path);
            }
            else
            {
                var index = this.Tree.Nodes.Cast<Node>().TakeWhile(node => !node.Equals(this)).Count();
                path.PrependIndex(index);
            }
        }
    }
}
