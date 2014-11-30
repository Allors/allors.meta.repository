//------------------------------------------------------------------------------------------------- 
// <copyright file="Nodes.cs" company="Allors bvba">
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

    public class Nodes : TreeNodes
    {
        public Nodes()
            : base(typeof(Node))
        {
        }

        public RepositoryNode RepositoryNode
        {
            get
            {
                return this.OfType<RepositoryNode>().FirstOrDefault();
            }
        }

        public Node[] SelectNodes(object tag)
        {
            return this.SelectNodes(tag, null);
        }

        public Node[] SelectNodes(object tag, Type nodeType)
        {
            var nodes = new List<Node>();
            foreach (Node node in this)
            {
                node.SelectNodes(nodes, tag, nodeType);
            }

            return nodes.ToArray();
        }

        public Node SelectSingleNode(object tag)
        {
            return this.SelectSingleNode(tag, null);
        }

        public Node SelectSingleNode(object tag, Type nodeType)
        {
            return (from Node node in this select node.SelectSingleNode(tag, nodeType)).FirstOrDefault(result => result != null);
        }
    }
}
