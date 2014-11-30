// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeEditorListBox.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Editors
{
    using System.Collections;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    // See http://www.vbinfozine.com/a_resimage.shtml for rationale and more info
    public class ObjectTypeEditorListBox : ListBox
    {
        private readonly IWindowsFormsEditorService svc;

        public ObjectTypeEditorListBox(IWindowsFormsEditorService svc, IList dataSource, object selectedItem) : this(svc)
        {
            this.DataSource = dataSource;
            this.SelectedItem = selectedItem;
        }

        public ObjectTypeEditorListBox(IWindowsFormsEditorService svc)
        {
            this.svc = svc;
            this.BorderStyle = BorderStyle.None;

            this.MouseUp += this.TypeEditorListBoxMouseUp;
        }

        public bool EscapePressed { get; private set; }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData.Equals(Keys.Return))
            {
                this.svc.CloseDropDown();
                return true;
            }

            if (keyData.Equals(Keys.Escape))
            {
                this.EscapePressed = true;
                this.svc.CloseDropDown();
                return true;
            }

            return false;
        }

        private void TypeEditorListBoxMouseUp(object sender, MouseEventArgs e)
        {
            this.svc.CloseDropDown();
        }
    }
}