//------------------------------------------------------------------------------------------------- 
// <copyright file="NodeTests.cs" company="Allors bvba">
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
using Gtk;

namespace Allors.R1.Development.Repository.GtkSharp.Decorators
{
    using System;

    using Allors.R1.Development.Repository.GtkSharp.Explorer;

    using NUnit.Framework;

    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void Find()
        {
            Application.Init();
            var nodes = new Nodes();

            var rootTag = Guid.NewGuid();
            var rootNode = new TestNode(new Tree(new Window("test")), "root-icon", "root", rootTag, new object());
            nodes.AddNode(rootNode);

            Assert.AreEqual(rootNode, nodes.SelectSingleNode(rootTag));

            var childTag = Guid.NewGuid();
            var childNode = new TestNode(new Tree(new Window("test")), "child-icon", "child", childTag, new object());
            rootNode.AddChild(childNode);

            Assert.AreEqual(childNode, nodes.SelectSingleNode(childTag));

            var grandchildTag = Guid.NewGuid();
            var grandchildNode = new TestNode(new Tree(new Window("test")), "grandchild-icon", "grandchild", grandchildTag, new object());
            childNode.AddChild(grandchildNode);

            Assert.AreEqual(grandchildNode, nodes.SelectSingleNode(grandchildTag));
        }

        [Test]
        public void FindById()
        {
            Application.Init();
            var nodes = new Nodes();

            var rootTag = Guid.NewGuid();
            var rootNode = new TestNode(new Tree(new Window("test")), "root-icon", "root", rootTag, new object());
            nodes.AddNode(rootNode);

            Assert.AreEqual(rootNode, nodes.SelectSingleNode(rootTag));

            var childTag = Guid.NewGuid();
            var childNode = new TestNode(new Tree(new Window("test")), "child-icon", "child", childTag, new object());
            rootNode.AddChild(childNode);

            Assert.AreEqual(childNode, nodes.SelectSingleNode(childTag));

            var grandchildTag = new object();
            var grandchildNode = new TestNode(new Tree(new Window("test")), "grandchild-icon", "grandchild", grandchildTag, new object());
            childNode.AddChild(grandchildNode);

            Assert.AreEqual(grandchildNode, nodes.SelectSingleNode(grandchildTag));
        }
    }
}