// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExplorerEvents.cs" company="Allors bvba">
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
    using System;

    public class SelectedEventArgs : EventArgs
    {
        private readonly object selection;
        private readonly object selectionForPropertyGrid;

        internal SelectedEventArgs(object selection, object selectionForPropertyGrid)
        {
            this.selection = selection;
            this.selectionForPropertyGrid = selectionForPropertyGrid;
        }

        public object Selection
        {
            get { return selection; }
        }

        public object SelectionForPropertyGrid
        {
            get { return selectionForPropertyGrid; }
        }
    }
}