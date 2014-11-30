//------------------------------------------------------------------------------------------------- 
// <copyright file="SupertypeComboBox.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    [System.ComponentModel.ToolboxItem(true)]
    public class SupertypeComboBox : MetaComboBox<ObjectType>
    {
        public SupertypeComboBox(Repository repository, ObjectType subtype)
            : base(repository)
        {
            this.BeginUpdate();
            try
            {
                var excludedTypes = new List<ObjectType>(subtype.Subtypes) { subtype };
                
                foreach (var objectType in this.Repository.Domain.ObjectTypes)
                {
                    if (objectType.IsUnit || objectType.IsConcreteComposite || excludedTypes.Contains(objectType))
                    {
                        continue;
                    }

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