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

namespace Allors.R1.Development.Repository.GtkSharp
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    using Gtk;

    [ToolboxItem(true)]
    public class InterfaceList : NodeView
    {
        [Category("Allors")]
        public InterfaceList(Repository repository)
            : base(new ListItems(typeof(InterfaceListItem)))
        {
            var column = new TreeViewColumn();
            this.AppendColumn(column);
            var toggleRenderer = new CellRendererToggle { Activatable = true };
            toggleRenderer.Toggled += this.ToggleRendererToggled;
            column.PackStart(toggleRenderer, false);
            column.SetAttributes(toggleRenderer, "active", 0);

            column = new TreeViewColumn();
            this.AppendColumn(column);
            var textRenderer = new CellRendererText();
            column.PackStart(textRenderer, true);
            column.SetAttributes(textRenderer, "text", 1);

            this.HeadersVisible = false;

            var objectTypes = repository.Domain.ObjectTypes.Where(objectType => objectType.IsInterface).OrderBy(objectType => objectType.Name).ToList();
            foreach (var objectType in objectTypes)
            {
                this.NodeStore.AddNode(new InterfaceListItem(objectType));
            }

            this.ShowAll();
        }

        public ObjectType[] ActiveItems
        {
            get
            {
                return (from InterfaceListItem interfaceListItem in this.NodeStore where interfaceListItem.Active select interfaceListItem.ObjectType).ToArray();
            }

            set
            {
                var activeItems = new HashSet<ObjectType>(value);
                foreach (InterfaceListItem interfaceListItem in this.NodeStore)
                {
                    interfaceListItem.Active = activeItems.Contains(interfaceListItem.ObjectType);
                }
            }
        }

        private void ToggleRendererToggled(object o, ToggledArgs args)
        {
            var path = new TreePath(args.Path);
            var listItem = (InterfaceListItem)this.NodeStore.GetNode(path);
            listItem.Active = !listItem.Active;
        }

        private class InterfaceListItem : ListItem
        {
            private readonly ObjectType objectType;

            private bool active;

            public InterfaceListItem(ObjectType objectType)
                : base(objectType.Id)
            {
                this.objectType = objectType;
            }

            public ObjectType ObjectType
            {
                get
                {
                    return this.objectType;
                }
            }

            [TreeNodeValue(Column = 0)]
            public bool Active 
            {
                get
                {
                    return this.active;
                }

                set
                {
                    this.active = value;
                    this.OnChanged();
                }
            }

            [TreeNodeValue(Column = 1)]
            public string Name 
            {
                get
                {
                    return this.objectType.Name;
                }
            }
        }
    }
}