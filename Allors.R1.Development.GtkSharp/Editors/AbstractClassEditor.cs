// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractClassEditor.cs" company="Allors bvba">
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

    public class AbstractClassEditor : PropertyEditorCell
    {
        private readonly Repository repository;

        public AbstractClassEditor(Repository repository)
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
            private AbstractClassComboBox abstractClassComboBox;

            public Editor(Repository repository)
            {
                this.repository = repository;
            }

            public event EventHandler ValueChanged;

            public object Value
            {
                get
                {
                    return this.abstractClassComboBox.ActiveItem;
                }

                set
                {
                    this.abstractClassComboBox.ActiveItem = (ObjectType)value;
                }
            }

            public void Initialize(EditSession session)
            {
                var abstractClass = (ObjectType)session.Property.GetValue(session.Instance);
                this.abstractClassComboBox = new AbstractClassComboBox(this.repository) { ActiveItem = abstractClass };
                this.abstractClassComboBox.Changed += this.AbstractClassComboBoxChanged;
                this.Add(this.abstractClassComboBox);
                this.abstractClassComboBox.ShowAll();
                this.ShowAll();
            }

            private void AbstractClassComboBoxChanged(object o, EventArgs args)
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