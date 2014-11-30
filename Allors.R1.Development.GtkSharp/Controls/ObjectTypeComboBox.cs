//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectTypeComboBox.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    [System.ComponentModel.ToolboxItem(true)]
    public class ObjectTypeComboBox : MetaComboBox<ObjectType>
    {
        public ObjectTypeComboBox(Repository repository)
            : base(repository)
        {
            this.BeginUpdate();
            try
            {
                foreach (var objectType in this.Repository.Domain.ObjectTypes)
                {
                    this.ListStore.AppendValues(objectType.AllorsObjectId, objectType.SingularName);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }
    }
}