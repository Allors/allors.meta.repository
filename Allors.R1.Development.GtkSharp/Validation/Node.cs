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

namespace Allors.R1.Development.Repository.GtkSharp.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using Gtk;

    public abstract class Node : TreeNode<Node>, IEnumerable<Node>
    {
        protected Node(object tag)
            : base(tag)
        {
        }

        [TreeNodeValue(Column = 0)]
        public abstract string Icon { get; }

        [TreeNodeValue(Column = 1)]
        public abstract string Source { get; }

        [TreeNodeValue(Column = 2)]
        public abstract string Kind { get; }

        [TreeNodeValue(Column = 3)]
        public abstract string Message { get; }
        
        public virtual void Sort()
        {
            this.Sort(
                (node, node1) =>
                    {
                        if (node.Source != null)
                        {
                            return string.Compare(node.Source, node1.Source, StringComparison.OrdinalIgnoreCase);
                        }

                        return -1;
                    });
        }

        public override void Sort(Comparison<Node> comparer)
        {
            var list = new List<Node>(this);
            list.Sort(comparer);

            for (var i = 0; i < list.Count; i++)
            {
                var node = list[i];
                this.RemoveChild(node);
                this.AddChild(node, i);
            }
        }

        public override IEnumerator<Node> GetEnumerator()
        {
            var nodes = new Node[this.ChildCount];
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = (Node)this[i];
            }

            return ((IEnumerable<Node>)nodes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract void PopupMenu();

        public void Remove()
        {
            var parent = this.Parent as Node;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }
    }
}
