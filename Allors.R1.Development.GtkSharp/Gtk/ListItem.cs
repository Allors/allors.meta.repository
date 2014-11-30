//------------------------------------------------------------------------------------------------- 
// <copyright file="ListItem.cs" company="Allors bvba">
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

    using Gtk;

    public abstract class ListItem : ITreeNode
    {
        private readonly int id;
        private readonly object tag;

        protected ListItem(object tag)
        {
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
                return null;
            }
        }

        public int ChildCount
        {
            get
            {
                return 0;
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
                return null;
            }
        }

        public int IndexOf(object o)
        {
            return -1;
        }

        public void OnChanged()
        {
            var changed = this.Changed;
            if (changed != null)
            {
                changed(this, EventArgs.Empty);
            }
        }
    }
}
