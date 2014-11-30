// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeTypeEditor.cs" company="Allors bvba">
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
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    using Allors.R1.Meta;

    public class ObjectTypeTypeEditor : UITypeEditor
    {
        private IWindowsFormsEditorService editorService;

        public interface ISource
        {
            ObjectType[] EditorObjectTypes { get; }

            ObjectType ObjectType { get; set; }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            this.editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (this.editorService != null)
            {
                var source = context.Instance as ISource;
                if (source != null)
                {
                    var types = source.EditorObjectTypes;
                    if (types.Length > 0)
                    {
                        var type = source.ObjectType ?? types[0];

                        var listBox = new ObjectTypeEditorListBox(this.editorService, types, type);
                        this.editorService.DropDownControl(listBox);
                        if (!listBox.EscapePressed)
                        {
                            return listBox.SelectedItem;
                        }
                    }
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}