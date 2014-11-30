//------------------------------------------------------------------------------------------------- 
// <copyright file="AddRelationTypeWizard.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.GtkSharp.Wizards
{
    using System;
    using System.Collections.Generic;

    using Allors.R1.Development.Repository;
    using Allors.R1.Development.Repository.GtkSharp.Decorators;
    using Allors.R1.Development.Repository.GtkSharp.Extensions;
    using Allors.R1.Meta;
    using Allors.R1.Meta.AllorsGenerated;

    using Gtk;

    public class AddRelationTypeWizard : Dialog
    {
        private readonly ErrorMessage associationObjectTypeErrorMessage;
        private readonly ErrorMessage associationSingularNameErrorMessage;
        private readonly ErrorMessage associationPluralNameErrorMessage;
        private readonly ErrorMessage roleObjectTypeErrorMessage;
        private readonly ErrorMessage roleSingularNameErrorMessage;
        private readonly ErrorMessage rolePluralNameErrorMessage;

        private readonly Entry associationSingularNameEntry;
        private readonly Entry associationPluralNameEntry;
        private readonly ObjectTypeComboBox associationObjectTypeComboBox;
        private readonly Entry roleSingularNameEntry;
        private readonly Entry rolePluralNameEntry;
        private readonly ObjectTypeComboBox roleObjectTypeComboBox;
        private readonly MultiplicityComboBox multiplicityComboBox;
        private readonly Entry explanationEntry;

        private readonly Repository repository;

        public AddRelationTypeWizard(Repository repository, ObjectType associationObjectType)
        {
            this.repository = repository;

            this.Title = Mono.Unix.Catalog.GetString("Allors Add RelationType Wizard");
            this.Icon = Gdk.Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico");
            this.DefaultWidth = 800;
            this.DefaultHeight = 1;

            var headerBox = new VBox
                                 {
                                     Spacing = 10, 
                                     BorderWidth = 10
                                 };
            this.VBox.PackStart(headerBox, false, false, 0);

            headerBox.PackStart(new HtmlLabel("<span size=\"large\"><b>Welcome to the Allors Add RelationType Wizard</b></span>", 0.5f));
            headerBox.PackStart(new HtmlLabel("This wizard adds a new relation type to your domain.", 0.5f));

            var form = new Form();
            this.VBox.PackStart(form);
            
            // Association Type
            var associationLabel = new HtmlLabel("<b>Association</b>");

            var associationObjectTypeLabel = new HtmlLabel("Object Type");
            this.associationObjectTypeComboBox = new ObjectTypeComboBox(repository);
            this.associationObjectTypeComboBox.Changed += this.OnAssociationObjectTypeComboBoxOnChanged; 
            this.associationObjectTypeErrorMessage = new ErrorMessage();

            var associationSingularNameLabel = new HtmlLabel("Singular name");
            this.associationSingularNameEntry = new Entry();
            this.associationSingularNameErrorMessage = new ErrorMessage();

            var associationPluralNameLabel = new HtmlLabel("Plural name");
            this.associationPluralNameEntry = new Entry();
            this.associationPluralNameErrorMessage = new ErrorMessage();

            // Role Type
            var roleLabel = new HtmlLabel("<b>Role</b>");

            var roleObjectTypeLabel = new HtmlLabel("Object Type");
            this.roleObjectTypeComboBox = new ObjectTypeComboBox(repository);
            this.roleObjectTypeComboBox.Changed += this.OnRoleObjectTypeComboBoxOnChanged; 
            this.roleObjectTypeErrorMessage = new ErrorMessage();

            var roleSingularNameLabel = new HtmlLabel("Singular name");
            this.roleSingularNameEntry = new Entry();
            this.roleSingularNameErrorMessage = new ErrorMessage();
            
            var rolePluralNameLabel = new HtmlLabel("Plural name");
            this.rolePluralNameEntry = new Entry();
            this.rolePluralNameErrorMessage = new ErrorMessage();

            // Relation Type
            var relationTypeLabel = new HtmlLabel("<b>Relation</b>");

            var multiplicityLabel = new HtmlLabel("Multiplicity");
            this.multiplicityComboBox = new MultiplicityComboBox();
            this.multiplicityComboBox.Changed += this.OnMultiplicityComboBoxOnChanged;

            var explanationLabel = new HtmlLabel("Explanation");
            this.explanationEntry = new Entry { Sensitive = false };

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
                                    UseStock = true, 
                                    UseUnderline = true, 
                                    Label = "gtk-ok"
                                };
            buttonOk.Clicked += this.OnButtonOkClicked;
            this.ActionArea.PackStart(buttonOk);

            // Table Layout
            // Association Type
            form.Attach(associationLabel, 0, 3, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            
            form.Attach(associationObjectTypeLabel, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationObjectTypeComboBox, 0, 1, 2, 3, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationObjectTypeErrorMessage, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(associationSingularNameLabel, 1, 2, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationSingularNameEntry, 1, 2, 2, 3, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationSingularNameErrorMessage, 1, 2, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(associationPluralNameLabel, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationPluralNameEntry, 2, 3, 2, 3, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.associationPluralNameErrorMessage, 2, 3, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            // Role Type
            form.Attach(roleLabel, 0, 3, 4, 5, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(roleObjectTypeLabel, 0, 1, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.roleObjectTypeComboBox, 0, 1, 6, 7, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.roleObjectTypeErrorMessage, 0, 1, 7, 8, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);

            form.Attach(roleSingularNameLabel, 1, 2, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.roleSingularNameEntry, 1, 2, 6, 7, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.roleSingularNameErrorMessage, 1, 2, 7, 8, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(rolePluralNameLabel, 2, 3, 5, 6, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.rolePluralNameEntry, 2, 3, 6, 7, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);
            form.Attach(this.rolePluralNameErrorMessage, 2, 3, 7, 8, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            // Relation Type
            form.Attach(relationTypeLabel, 0, 3, 8, 9, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

            form.Attach(multiplicityLabel, 0, 1, 9, 10, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.multiplicityComboBox, 0, 1, 10, 11, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);

            form.Attach(explanationLabel, 1, 3, 9, 10, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
            form.Attach(this.explanationEntry, 1, 3, 10, 11, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Fill, 0, 0);

            this.ShowAll();

            this.ResetErrorMessages();

            this.associationObjectTypeComboBox.ActiveItem = associationObjectType;
            this.multiplicityComboBox.ActiveItem = Multiplicity.ManyToOne;
        }
        
        public RelationType RelationType { get; private set; }

        private void OnAssociationObjectTypeComboBoxOnChanged(object sender, EventArgs args)
        {
            var objectType = this.associationObjectTypeComboBox.ActiveItem;
            if (objectType != null)
            {
                this.associationSingularNameEntry.Text = objectType.SingularName;
                this.associationPluralNameEntry.Text = objectType.PluralName;
            }

            this.UpdateMultiplicityExplanation();
        }

        private void OnRoleObjectTypeComboBoxOnChanged(object sender, EventArgs args)
        {
            var objectType = this.roleObjectTypeComboBox.ActiveItem;
            if (objectType != null)
            {
                this.multiplicityComboBox.ActiveItem = objectType.IsUnit ? Multiplicity.OneToOne : Multiplicity.ManyToOne;

                this.roleSingularNameEntry.Text = objectType.SingularName;
                this.rolePluralNameEntry.Text = objectType.PluralName;
            }

            this.UpdateMultiplicityExplanation();
        }

        private void OnMultiplicityComboBoxOnChanged(object sender, EventArgs args)
        {
            this.UpdateMultiplicityExplanation();
        }

        private void OnButtonOkClicked(object sender, EventArgs e)
        {
            var error = false;
            this.ResetErrorMessages();

            var roleObjectType = this.roleObjectTypeComboBox.ActiveItem;
            if (roleObjectType == null)
            {
                error = true;
                this.roleObjectTypeErrorMessage.Text = "Object Type is mandatory";
            }

            if (!error)
            {
                var domain = this.repository.Domain;
                this.RelationType = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

                var associationObjectType = this.associationObjectTypeComboBox.ActiveItem;
                var associationSingularName = this.associationSingularNameEntry.Text.TrimToNull();
                var associationPluralName = this.associationPluralNameEntry.Text.TrimToNull();

                this.RelationType.AssociationType.ObjectType = associationObjectType;
                this.RelationType.AssociationType.AssignedSingularName = associationObjectType.SingularName.Equals(associationSingularName) ? null : associationSingularName;
                this.RelationType.AssociationType.AssignedPluralName = associationObjectType.PluralName.Equals(associationPluralName) ? null : associationPluralName;

                var roleSingularName = this.roleSingularNameEntry.Text.TrimToNull();
                var rolePluralName = this.rolePluralNameEntry.Text.TrimToNull();

                this.RelationType.RoleType.ObjectType = roleObjectType;
                this.RelationType.RoleType.AssignedSingularName = roleObjectType.SingularName.Equals(roleSingularName) ? null : roleSingularName;
                this.RelationType.RoleType.AssignedPluralName = roleObjectType.PluralName.Equals(rolePluralName) ? null : rolePluralName;

                var activeItem = this.multiplicityComboBox.ActiveItem;
                if (activeItem != null)
                {
                    var multiplicity = activeItem.Value;
                    switch (multiplicity) 
                    {
                        case Multiplicity.OneToOne:
                            this.RelationType.AssociationType.IsOne = true;
                            this.RelationType.RoleType.IsOne = true;
                            break;
                        case Multiplicity.OneToMany:
                            this.RelationType.AssociationType.IsOne = true;
                            this.RelationType.RoleType.IsMany = true;
                            break;
                        case Multiplicity.ManyToOne:
                            this.RelationType.AssociationType.IsMany = true;
                            this.RelationType.RoleType.IsOne = true;
                            break;
                        case Multiplicity.ManyToMany:
                            this.RelationType.AssociationType.IsMany = true;
                            this.RelationType.RoleType.IsMany = true;
                            break;
                    }
                }

                this.RelationType.IsIndexed = roleObjectType.IsComposite;

                var validationReport = domain.Validate();
                if (!validationReport.ContainsErrors)
                {
                    this.RelationType.SendChangedEvent();
                    domain.SendChangedEvent();
                    this.Respond(ResponseType.Ok);
                }
                else
                {
                    this.RelationType.Delete();
                    domain.SendChangedEvent();

                    var unhandledErrors = new List<ValidationError>();
                    foreach (var validationError in validationReport.Errors)
                    {
                        foreach (var member in validationError.Members)
                        {
                            if (member.Equals(AllorsEmbeddedDomain.AssociationTypeAssignedSingularName))
                            {
                                this.associationSingularNameErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.AssociationTypeAssignedPluralName))
                            {
                                this.associationPluralNameErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.AssociationTypeObjectType))
                            {
                                this.associationObjectTypeErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.RoleTypeAssignedSingularName))
                            {
                                this.roleSingularNameErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.RoleTypeAssignedPluralName))
                            {
                                this.rolePluralNameErrorMessage.Text = validationError.Message;
                            }
                            else if (member.Equals(AllorsEmbeddedDomain.RoleTypeObjectType))
                            {
                                this.roleObjectTypeErrorMessage.Text = validationError.Message;
                            }
                            else
                            {
                                unhandledErrors.Add(validationError);
                            }
                        }
                    }

                    if (unhandledErrors.Count > 0)
                    {
                        var message = string.Join("\n", unhandledErrors);
                        var md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, message);
                        md.Run();
                        md.Destroy();
                    }
                }
            }

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }

        private void UpdateMultiplicityExplanation()
        {
            this.explanationEntry.Text = string.Empty;
            var roleType = this.roleObjectTypeComboBox.ActiveItem;
            if (roleType != null)
            {
                var activeItem = this.multiplicityComboBox.ActiveItem;
                if (activeItem != null)
                {
                    var multiplicity = activeItem.Value;

                    var explanation = this.associationSingularNameEntry.Text;
                    if (multiplicity.Equals(Multiplicity.OneToOne) || multiplicity.Equals(Multiplicity.ManyToOne))
                    {
                        explanation += " has one " + this.roleSingularNameEntry.Text;
                    }
                    else
                    {
                        explanation += " has many " + this.rolePluralNameEntry.Text;
                    }

                    if (roleType.IsComposite)
                    {
                        explanation += ", " + this.roleSingularNameEntry.Text;
                        if (multiplicity.Equals(Multiplicity.OneToOne) || multiplicity.Equals(Multiplicity.OneToMany))
                        {
                            explanation += " has one " + this.associationSingularNameEntry.Text;
                        }
                        else
                        {
                            explanation += " has many " + this.associationPluralNameEntry.Text;
                        }
                    }

                    this.explanationEntry.Text = explanation;
                }
            }
        }

        private void ResetErrorMessages()
        {
            this.associationSingularNameErrorMessage.Text = null;
            this.associationPluralNameErrorMessage.Text = null;
            this.associationObjectTypeErrorMessage.Text = null;

            this.roleSingularNameErrorMessage.Text = null;
            this.rolePluralNameErrorMessage.Text = null;
            this.roleObjectTypeErrorMessage.Text = null;

            this.Resize(this.DefaultWidth, this.DefaultHeight);
        }
    }
}