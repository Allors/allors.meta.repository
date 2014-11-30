// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNodeExtension.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;

    public static class TreeNodeExtension
    {
        private static readonly ReadOnlyCollection<TreeNode> EmptyTreeNodeList = new List<TreeNode>().AsReadOnly();  

        public static TreeNode Find(this TreeNode treeNode, Tag tag)
        {
            var treeNodes = treeNode.Nodes;
            for (var i = 0; i < treeNodes.Count; i++)
            {
                if (treeNodes[i].Tag.Equals(tag))
                {
                    return treeNodes[i];
                }
            }

            return null;
        }

        public static void Retain<T>(this TreeNode treeNode, IList<T> tagsToRetain) where T : Tag
        {
            List<TreeNode> treeNodesToRemove = null;
            foreach (TreeNode child in treeNode.Nodes)
            {
                var tag = child.Tag as T;
                if (tag != null && !tagsToRetain.Contains(tag))
                {
                    if (treeNodesToRemove == null)
                    {
                        treeNodesToRemove = new List<TreeNode>();
                    }

                    treeNodesToRemove.Add(child);
                }
            }

            if (treeNodesToRemove != null)
            {
                foreach (var treeNodeToDelete in treeNodesToRemove)
                {
                    treeNodeToDelete.Remove();
                }
            }
        }

        public static void SyncText(this TreeNode treeNode, string text)
        {
            if (!treeNode.Text.Equals(text))
            {
                treeNode.Text = text;
            }
        }

        public static void SyncImageIndex(this TreeNode treeNode, int imageIndex)
        {
            if (!treeNode.ImageIndex.Equals(imageIndex))
            {
                treeNode.ImageIndex = imageIndex;
            }
        }

        public static void SyncSelectedImageIndex(this TreeNode treeNode, int selectedImageIndex)
        {
            if (!treeNode.SelectedImageIndex.Equals(selectedImageIndex))
            {
                treeNode.SelectedImageIndex = selectedImageIndex;
            }
        }

        public static void SyncExpand(this TreeNode treeNode)
        {
            if (!treeNode.IsExpanded)
            {
                treeNode.Expand();
            }
        }

        public static IList<TreeNode> FindAll<T>(this TreeNode treeNode) where T : Tag
        {
            List<TreeNode> matches = null;
            foreach (TreeNode child in treeNode.Nodes)
            {
                var tag = child.Tag as T;
                if (tag != null)
                {
                    if (matches == null)
                    {
                        matches = new List<TreeNode>();
                    }

                    matches.Add(child);
                }
            }

            if (matches == null)
            {
                return EmptyTreeNodeList;
            }

            return matches;
        }
    }
}
