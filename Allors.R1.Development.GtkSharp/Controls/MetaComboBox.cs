//------------------------------------------------------------------------------------------------- 
// <copyright file="MetaComboBox.cs" company="Allors bvba">
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

    using Gtk;

    public class MetaComboBox<T> : TextComboBox
        where T : MetaObject
    {
        protected MetaComboBox(Repository repository)
            : base(true)
        {
            this.Repository = repository;
        }

        public T ActiveItem 
        {
            get
            {
                TreeIter iter;
                if (this.GetActiveIter(out iter))
                {
                    var id = (int)this.ListStore.GetValue(iter, 0);
                    return (T)this.Repository.Domain.AllorsSession.Instantiate(id);
                }

                return null;
            }

            set
            {
                if (value == null)
                {
                    this.SetActiveIter(TreeIter.Zero);
                    return;
                }

                this.ListStore.Foreach(
                    (model, path, iter) =>
                        {
                            if (this.ListStore.GetValue(iter, 0).Equals(value.AllorsObjectId))
                            {
                                this.SetActiveIter(iter);
                                return true;
                            }

                            return false;
                        });
            }
        }

        protected Repository Repository { get; private set; }
    }
}