//------------------------------------------------------------------------------------------------- 
// <copyright file="TreeNode.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
 
    using Gtk;

    public abstract class TreeNode<T> : ITreeNode, IEnumerable<T>
        where T : TreeNode<T>
    {
        private readonly List<T> children;
        private readonly int id;
        private readonly object tag;

        private ITreeNode parent;

        protected TreeNode(object tag)
        {
            this.children = new List<T>();
            this.id = TreeNodes.NextIdCounter();
            this.tag = tag;
        }

        public event EventHandler Changed;

        public event TreeNodeAddedHandler ChildAdded;

        public event TreeNodeRemovedHandler ChildRemoved;

        public int ID
        {
            get
            {
                return this.id;
            }
        }

        public ITreeNode Parent
        {
            get
            {
                return this.parent;
            }
        }

        public int ChildCount
        {
            get
            {
                return this.children.Count;
            }
        }

        public object Tag 
        { 
            get
            {
                return this.tag;
            }
        }

        public ITreeNode this[int index]
        {
            get
            {
                return index < this.ChildCount ? this.children[index] : null;
            }
        }

        public int IndexOf(object o)
        {
            return this.children.IndexOf((T)o);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.children.ToArray().Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.children.ToArray().GetEnumerator();
        }

        public void AddChild(T child)
        {
            this.children.Add(child);
            child.parent = this;
            this.OnChildAdded(child);
        }

        public void AddChild(T child, int position)
        {
            this.children.Insert(position, child);
            child.parent = this;
            this.OnChildAdded(child);
        }

        public void RemoveChild(T child)
        {
            var oldPosition = this.children.IndexOf(child);
            if (oldPosition < 0)
            {
                return;
            }

            this.children.Remove(child);
            child.parent = null;
            this.OnChildRemoved(child, oldPosition);
        }

        public void SelectNodes(List<T> nodes, object tag, Type nodeType)
        {
            if (object.Equals(this.Tag, tag) && 
                (nodeType == null || object.Equals(this.GetType(), nodeType)))
            {
                nodes.Add((T)this); 
            }

            foreach (var node in this)
            {
                node.SelectNodes(nodes, tag, nodeType);
            }
        }

        public T SelectSingleNode(object tag)
        {
            return this.SelectSingleNode(tag, null);
        }

        public T SelectSingleNode(object tag, Type nodeType)
        {
            if (object.Equals(this.Tag, tag) && 
                (nodeType == null || object.Equals(this.GetType(), nodeType)))
            {
                return (T)this;
            }

            return this.Select(childNode => childNode.SelectSingleNode(tag, nodeType)).FirstOrDefault(result => result != null);
        }

        public virtual void Sort(Comparison<T> comparer)
        {
            this.children.Sort(comparer);
            this.OnChanged();
        }

        public void OnChanged()
        {
            var changed = this.Changed;
            if (changed != null)
            {
                changed(this, EventArgs.Empty);
            }
        }

        private void OnChildAdded(T child)
        {
            var childAdded = this.ChildAdded;
            if (childAdded != null)
            {
                childAdded(this, child);
            }
        }

        private void OnChildRemoved(T child, int oldPosition)
        {
            var childRemoved = this.ChildRemoved;
            if (childRemoved != null)
            {
                childRemoved(this, child, oldPosition);
            }
        }
    }
}
