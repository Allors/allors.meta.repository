//------------------------------------------------------------------------------------------------- 
// <copyright file="TestNode.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the AssociationTest type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.GtkSharp.Decorators
{
    using Allors.R1.Development.Repository.GtkSharp.Explorer;

    using Tree = Allors.R1.Development.Repository.GtkSharp.Explorer.Tree;

    public class TestNode : Node
    {
        private readonly string icon;

        private readonly string name;

        private readonly object propertyGridDecorator;

        public TestNode(Tree tree, string icon, string name, object tag, object propertyGridDecorator)
            : base(tree, tag)
        {
            this.icon = icon;
            this.name = name;
            this.propertyGridDecorator = propertyGridDecorator;
        }

        public override string Icon 
        {
            get
            {
                return this.icon;
            }
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }
        
        public override object PropertyGridDecorator
        {
            get
            {
                return this.propertyGridDecorator;
            }
        }

        public override void Sync()
        {
        }

        public override void PopupMenu()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
