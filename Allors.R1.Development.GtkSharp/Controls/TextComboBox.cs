//------------------------------------------------------------------------------------------------- 
// <copyright file="TextComboBox.cs" company="Allors bvba">
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
    using System;


    using GLib;

    using Gtk;

    public class TextComboBox : ComboBox
    {
        protected TextComboBox(bool sort)
        {
            this.ListStore = new ListStore(new[] { GType.Int, GType.String });
            this.Model = this.ListStore;

            var textRenderer = new CellRendererText();
            this.PackStart(textRenderer, true);
            this.SetAttributes(textRenderer, "text", 1);

            if (sort)
            {
                this.ListStore.SetSortColumnId(0, SortType.Ascending);
                this.ListStore.SetSortFunc(
                    0,
                    (model, firstIter, secondIter) =>
                    {
                        var firstValue = (string)model.GetValue(firstIter, 1);
                        var secondValue = (string)model.GetValue(secondIter, 1);
                        return firstValue == null ? -1 : string.Compare(firstValue, secondValue, StringComparison.OrdinalIgnoreCase);
                    });
            }
        }

        protected ListStore ListStore { get; private set; }

        protected void BeginUpdate()
        {
            this.Model = null;
        }

        protected void EndUpdate()
        {
            this.Model = this.ListStore;
        }
    }
}