//------------------------------------------------------------------------------------------------- 
// <copyright file="MultiplicityComboBox.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.GtkSharp.Decorators;

    using Gtk;

    [System.ComponentModel.ToolboxItem(true)]
    public class MultiplicityComboBox : TextComboBox
    {
        public MultiplicityComboBox()
            : base(false)
        {
            this.BeginUpdate();
            try
            {
                this.ListStore.AppendValues((int)Multiplicity.OneToOne, "One to One");
                this.ListStore.AppendValues((int)Multiplicity.OneToMany, "One to May");
                this.ListStore.AppendValues((int)Multiplicity.ManyToOne, "Many to One");
                this.ListStore.AppendValues((int)Multiplicity.ManyToMany, "Many to Many");
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public Multiplicity? ActiveItem
        {
            get
            {
                TreeIter iter;
                if (this.GetActiveIter(out iter))
                {
                    return (Multiplicity)this.ListStore.GetValue(iter, 0);
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
                            var iterValue = this.ListStore.GetValue(iter, 0);
                            if (iterValue.Equals((int)value.Value))
                            {
                                this.SetActiveIter(iter);
                                return true;
                            }

                            return false;
                        });
            }
        }
    }
}