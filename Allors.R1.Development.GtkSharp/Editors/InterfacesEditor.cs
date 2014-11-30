// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterfacesEditor.cs" company="Allors bvba">
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
    using Allors.R1.Development.Repository.GtkSharp.Converters;
    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Meta;

    using Gtk;

    using MonoDevelop.Components.PropertyGrid;

    public class InterfacesEditor : PropertyEditorCell
    {
        private readonly Repository repository;

        public InterfacesEditor(Repository repository)
        {
            this.repository = repository;
        }

        protected override IPropertyEditor CreateEditor(Gdk.Rectangle cellArea, StateType state)
        {
            return new Editor(this.repository);
        }

        private class Editor : HBox, IPropertyEditor
        {
            private readonly Repository repository;
            private ObjectType subtype;
            private ObjectType[] supertypes;

            private Label label;

            public Editor(Repository repository)
            {
                this.repository = repository;
            }

            public event EventHandler ValueChanged;

            public object Value
            {
                get
                {
                    return this.supertypes;
                }

                set
                {
                    this.supertypes = (ObjectType[])value;
                    this.label.Text = SuperinterfacesConverter.ToString(this.supertypes);
                }
            }

            public void Initialize(EditSession session)
            {
                this.subtype = ((ObjectTypeDecorator)session.Instance).ObjectType;
                var currentSupertypes = session.Property.GetValue(session.Instance);

                var text = SuperinterfacesConverter.ToString((ObjectType[])currentSupertypes);
                this.label = new Label(text);
                this.PackStart(this.label);
                var labelLayout = (BoxChild)this[this.label];
                labelLayout.Expand = false;

                var button = new Button("...");
                this.PackEnd(button);
                var buttonLayout = (BoxChild)this[button];
                buttonLayout.Expand = false;

                button.Clicked += delegate
                {
                    var dialog = new EditorDialog(this.repository, this.subtype);
                    dialog.Response += (o, args) =>
                    {
                        if (args.ResponseId == ResponseType.Ok)
                        {
                            this.Value = dialog.Interfaces;
                            
                            var valueChanged = this.ValueChanged;
                            if (valueChanged != null)
                            {
                                valueChanged(this, EventArgs.Empty);
                            }
                        }
                    };
                    dialog.Run();
                    dialog.Destroy();
                };

                this.ShowAll();
            }

            private class EditorDialog : Dialog
            {
                private readonly Repository repository;

                private readonly InterfaceList interfaceList;

                public EditorDialog(Repository repository, ObjectType subType)
                {
                    this.Title = Mono.Unix.Catalog.GetString("Edit Interfaces");
                    this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
                    this.DefaultWidth = 600;
                    this.DefaultHeight = 400;

                    var form = new Form();
                    this.VBox.PackStart(form);

                    var scrolledWindow = new ScrolledWindow();
                    this.interfaceList = new InterfaceList(repository);
                    scrolledWindow.Add(this.interfaceList);

                    this.interfaceList.ActiveItems = subType.DirectSuperinterfaces;
                    
                    var buttonCancel = new Button
                    {
                        CanDefault = true,
                        UseStock = true,
                        UseUnderline = true,
                        Label = "gtk-cancel"
                    };
                    this.AddActionWidget(buttonCancel, -6);

                    var buttonOk = new Button
                    {
                        CanDefault = true,
                        Name = "buttonOk",
                        UseStock = true,
                        UseUnderline = true,
                        Label = "gtk-ok"
                    };
                    buttonOk.Clicked += this.OnButtonOkClicked;
                    this.ActionArea.PackStart(buttonOk);

                    // Layout
                    form.Attach(scrolledWindow, 0, 1, 0, 1, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill | AttachOptions.Expand, 0, 0);
                    
                    this.ShowAll();

                    this.repository = repository;

                    this.Resize(this.DefaultWidth, this.DefaultHeight);
                }

                public ObjectType[] Interfaces
                {
                    get
                    {
                        return this.interfaceList.ActiveItems;
                    }
                }

                private void OnButtonOkClicked(object sender, EventArgs eventArgs)
                {
                    this.Respond(ResponseType.Ok);
                }
            }
        }
    }
}