// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerSorter.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
//   // 
//   // Dual Licensed under
//   //   a) the Lesser General Public Licence v3 (LGPL)
//   //   b) the Allors License
//   // 
//   // The LGPL License is included in the file lgpl.txt.
//   // The Allors License is an addendum to your contract.
//   // 
//   // Allors Platform is distributed in the hope that it will be useful,
//   // but WITHOUT ANY WARRANTY; without even the implied warranty of
//   // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   // GNU General Public License for more details.
//   // 
//   // For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.WinForms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Allors.R1.Development.Repository.WinForms;

    public class ExplorerSorter : IComparer
    {
        private static List<Type> compareOrder =
            new List<Type>(
                new[]
                    {
                        typeof(RepositoriesTag), 
                        typeof(RepositoryTag), 
                        typeof(SuperDomainsTag), 
                        typeof(SuperDomainTag), 
                        typeof(TemplatesTag),
                        typeof(TemplateTag), 
                        typeof(DomainTag), 
                        typeof(ObjectTypeTag), 
                        typeof(InheritancesTag), 
                        typeof(InheritanceTag), 
                        typeof(RelationTypeTag), 
                    });

        public int Compare(object fromObject, object toObject)
        {
            var from = (TreeNode)fromObject;
            var to = (TreeNode)toObject;

            var fromTagType = from.Tag.GetType();
            var toTagType = to.Tag.GetType();

            if (fromTagType == toTagType)
            {
                return string.CompareOrdinal(@from.Text, to.Text);
            }

            var fromIndex = compareOrder.IndexOf(fromTagType);
            var toIndex = compareOrder.IndexOf(toTagType);

            return fromIndex - toIndex;
        }
    }
}
