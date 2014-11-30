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
    using System.Linq;

    public class Nodes : TreeNodes
    {
        public Nodes(Type nodeType)
            : base(nodeType)
        {
        }

        public Node Find(object tag, Type nodeType)
        {
            return (from Node node in this select node.SelectSingleNode(tag, nodeType)).FirstOrDefault(result => result != null);
        }
    }
}
