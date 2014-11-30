// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTypeEditor.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Editors
{
    using System;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    using Gtk;

    using MonoDevelop.Components.PropertyGrid;

    public class ObjectTypeEditor : PropertyEditorCell
    {
        private readonly Repository repository;

        public ObjectTypeEditor(Repository repository)
        {
            this.repository = repository;
        }

        protected override IPropertyEditor CreateEditor(Gdk.Rectangle cellArea, StateType state)
        {
            return new Editor(this.repository);
        }

        private class Editor : EventBox, IPropertyEditor
        {
            private readonly Repository repository;
            private ObjectTypeComboBox objectTypeComboBox;

            public Editor(Repository repository)
            {
                this.repository = repository;
            }

            public event EventHandler ValueChanged;

            public object Value
            {
                get
                {
                    return this.objectTypeComboBox.ActiveItem;
                }

                set
                {
                    this.objectTypeComboBox.ActiveItem = (ObjectType)value;
                }
            }

            public void Initialize(EditSession session)
            {
                this.objectTypeComboBox = new ObjectTypeComboBox(this.repository) { ActiveItem = (ObjectType)session.Property.GetValue(session.Instance) };
                this.objectTypeComboBox.Changed += this.ObjectTypeComboBoxChanged;
                this.Add(this.objectTypeComboBox);
                this.objectTypeComboBox.ShowAll();
                this.ShowAll();
            }

            private void ObjectTypeComboBoxChanged(object o, EventArgs args)
            {
                var valueChanged = this.ValueChanged;
                if (valueChanged != null)
                {
                    valueChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}