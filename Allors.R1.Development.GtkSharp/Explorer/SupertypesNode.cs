//------------------------------------------------------------------------------------------------- 
// <copyright file="SuperTypesNode.cs" company="Allors bvba">
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

    using Allors.R1.Meta;

    public class SupertypesNode : Node
    {
        public static readonly Guid UniqueId = new Guid("23338A12-68B0-4904-A353-8CBDFF2BC8C2");

        private readonly ObjectType subtype;

        public SupertypesNode(Tree tree, ObjectType subtype)
            : base(tree, UniqueId)
        {
            this.subtype = subtype;
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
                return "Supertypes";
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
            var inheritances = new HashSet<Inheritance>(this.subtype.InheritancesWhereSubtype);

            // existing and stale nodes
            foreach (var node in this)
            {
                var inheritanceNode = node as SupertypeNode;
                if (inheritanceNode != null)
                {
                    var inheritance = inheritanceNode.Inheritance;
                    if (inheritance != null)
                    {
                        if (inheritances.Contains(inheritance))
                        {
                            inheritanceNode.Sync();
                            inheritances.Remove(inheritance);
                        }
                        else
                        {
                            this.RemoveChild(inheritanceNode);
                        }
                    }
                }
            }

            // new nodes
            foreach (var inheritance in inheritances)
            {
                var node = new SupertypeNode(this.Tree, inheritance);
                this.AddChild(node);
                node.Sync();
            }

            this.Sort();
        }

        public override void PopupMenu()
        {
        }
    }
}
