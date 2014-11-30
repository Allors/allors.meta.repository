// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperinterfacesTypeEditor.cs" company="Allors bvba">
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
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    using Allors.R1.Meta;

    public class SuperinterfacesTypeEditor : UITypeEditor
    {
        public interface ISource
        {
            ObjectType[] PossibleSuperinterfaces { get; }

            ObjectType[] Superinterfaces { get; set; }

            ObjectType SuperInterfaceType { get; }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                var source = context.Instance as ISource;
                if (source != null)
                {
                    var superinterfaces = source.Superinterfaces;
                    var possibleSuperinterfaces = source.PossibleSuperinterfaces;
                    var dialog = new SuperinterfacesEditor(source.SuperInterfaceType, superinterfaces, possibleSuperinterfaces);
                    if (service.ShowDialog(dialog) == DialogResult.OK)
                    {
                        return dialog.Superinterfaces;
                    }
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}