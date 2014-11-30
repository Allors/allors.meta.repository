//------------------------------------------------------------------------------------------------- 
// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;

    using AllorsGenerated;

    /// <summary>
    /// An <see cref="AssociationType"/> defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public sealed partial class AssociationType : IComparable
    {
        /// <summary>
        /// Used to form names to navigate from <see cref="RoleType"/> To <see cref="AssociationType"/>;
        /// </summary>
        private const string Where = "Where";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public override string Name
        {
            get
            {
                return this.FullName;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public string FullName
        {
            get
            {
                if (this.IsMany)
                {
                    return this.WherePluralName;
                }

                return this.WhereSingularName;
            }
        }

        /// <summary>
        /// Gets or sets the name of the assigned plural.
        /// </summary>
        /// <value>The name of the assigned plural.</value>
        public override string AssignedPluralName
        {
            get
            {
                return base.AssignedPluralName;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.AssignedPluralName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the assigned singular.
        /// </summary>
        /// <value>The name of the assigned singular.</value>
        public override string AssignedSingularName
        {
            get
            {
                return base.AssignedSingularName;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.AssignedSingularName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exist root <see cref="ObjectType"/>s.
        /// </summary>
        /// <value><c>true</c> if there exists root <see cref="ObjectType"/>s; otherwise, <c>false</c>.</value>
        public bool ExistRootTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return ExistDerivedRootObjectTypes;
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Gets the the full plural name.
        /// </summary>
        /// <value>The full name of the plural.</value>
        public string FullPluralName
        {
            get { return this.RelationTypeWhereAssociationType.RoleType.SingularName + this.PluralName; }
        }

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full name of the singular.</value>
        public string FullSingularName
        {
            get { return this.RelationTypeWhereAssociationType.RoleType.SingularName + this.SingularName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="AssignedPluralName"/> is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's <see cref="AssignedPluralName"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedPluralNameDefault
        {
            get { return !ExistAssignedPluralName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="AssignedSingularName"/> is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's <see cref="AssignedPluralName"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedSingularNameDefault
        {
            get { return !ExistAssignedSingularName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="IsMany"/> is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's <see cref="IsMany"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsIsManyDefault
        {
            get { return this.IsMany == false; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a multiplicity of many.
        /// </summary>
        /// <value><c>true</c> if this instance is many; otherwise, <c>false</c>.</value>
        public override bool IsMany
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.IsMany;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.IsMany = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="ObjectType"/> is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's <see cref="ObjectType"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsObjectTypeDefault
        {
            get { return !ExistObjectType; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this instance is one; otherwise, <c>false</c>.</value>
        public bool IsOne
        {
            get { return !this.IsMany; }
            set { this.IsMany = !value; }
        }

        /// <summary>
        /// Gets or sets ObjectType.
        /// </summary>
        /// <value>The ObjectType.</value>
        public override ObjectType ObjectType
        {
            get
            {
                return base.ObjectType;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.ObjectType = value;
            }
        }

        /// <summary>
        /// Gets the plural name.
        /// </summary>
        /// <value>The plural name.</value>
        public string PluralName
        {
            get
            {
                if (ExistAssignedPluralName)
                {
                    return this.AssignedPluralName;
                }

                if (ExistObjectType && ObjectType.ExistPluralName)
                {
                    return ObjectType.PluralName;
                }

                return this.SingularName;
            }
        }

        /// <summary>
        /// Gets the relation type.
        /// </summary>
        /// <value>The type of the relation.</value>
        public RelationType RelationType
        {
            get { return RelationTypeWhereAssociationType; }
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>The role .</value>
        public RoleType RoleType
        {
            get { return RelationTypeWhereAssociationType.RoleType; }
        }

        /// <summary>
        /// Gets the root name.
        /// </summary>
        /// <value>The name of the root.</value>
        public string RootName
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return DerivedRootName;
            }
        }

        /// <summary>
        /// Gets the root types.
        /// </summary>
        /// <value>The root types.</value>
        public ObjectType[] RootTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return DerivedRootObjectTypes;
            }
        }

        /// <summary>
        /// Gets the singular name.
        /// </summary>
        /// <value>The name of the singular.</value>
        public string SingularName
        {
            get
            {
                if (ExistAssignedSingularName)
                {
                    return this.AssignedSingularName;
                }

                if (ExistObjectType)
                {
                    if (ObjectType.ExistSingularName)
                    {
                        return ObjectType.SingularName;
                    }

                    return ObjectType.ToString();
                }

                if (ExistRelationTypeWhereAssociationType)
                {
                    return RelationTypeWhereAssociationType.SafeName;
                }

                return this.Id.ToString();
            }
        }
        
        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The name of the validation.</value>
        protected override string ValidationName
        {
            get
            {
                if (ExistRelationTypeWhereAssociationType)
                {
                    return "association type " + RelationTypeWhereAssociationType.Name;
                }

                return "unknown association type";
            }
        }

        /// <summary>
        /// Gets the plural name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The plural name when using <see cref="Where"/>.</value>
        private string WherePluralName
        {
            get { return this.PluralName + Where + this.RelationTypeWhereAssociationType.RoleType.SingularName; }
        }

        /// <summary>
        /// Gets the singular name when using <see cref="Where"/>.
        /// </summary>
        /// <value>The singular name when using <see cref="Where"/>.</value>
        private string WhereSingularName
        {
            get { return this.SingularName + Where + this.RelationTypeWhereAssociationType.RoleType.SingularName; }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            var that = obj as AssociationType;
            if (that != null)
            {
                return string.CompareOrdinal(this.SingularName, that.SingularName);
            }

            return -1;
        }

        /// <summary>
        /// Send a changed event.
        /// </summary>
        public override void SendChangedEvent()
        {
            if (this.ExistRelationTypeWhereAssociationType)
            {
                this.RelationTypeWhereAssociationType.SendChangedEvent();
            }
        }

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="ObjectType"/>.
        /// </returns>
        public override ObjectType GetObjectType()
        {
            return this.ObjectType;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// It is not possible to directly delete an association.
        /// </exception>
        public override void Delete()
        {
            throw new NotSupportedException();
        }
        
        /// <summary>
        /// Removes the AssignedPluralName.
        /// </summary>
        public override void RemoveAssignedPluralName()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveAssignedPluralName();
        }

        /// <summary>
        /// Removes the AssignedSingularName.
        /// </summary>
        public override void RemoveAssignedSingularName()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveAssignedSingularName();
        }

        /// <summary>
        /// Removes the IsMany.
        /// </summary>
        public override void RemoveIsMany()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveIsMany();
        }

        /// <summary>
        /// Removes the ObjectType.
        /// </summary>
        public override void RemoveObjectType()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveObjectType();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.IsMany = false;
            this.RemoveAssignedPluralName();
            this.RemoveAssignedSingularName();
            this.RemoveObjectType();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            try
            {
                return this.RelationType.ToString();
            }
            catch
            {
                return base.ToString();
            }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>A new instance</returns>
        internal static AssociationType Create(AllorsEmbeddedSession session)
        {
            var association = (AssociationType)session.Create(AllorsEmbeddedDomain.AssociationType);
            association.Reset();
            return association;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        internal void InternalDelete()
        {
            base.Delete();
        }

        /// <summary>
        /// Copy form source.
        /// </summary>
        /// <param name="source">The source.</param>
        internal void Copy(AssociationType source)
        {
            if (!this.ExistId)
            {
                this.Id = source.Id;
            }
            else if (!this.Id.Equals(source.Id))
            {
                throw new Exception("imported object has a different id (" + this.ValidationName + ")");
            }

            this.IsMany = source.IsMany;
            this.AssignedPluralName = source.AssignedPluralName;
            this.AssignedSingularName = source.AssignedSingularName;
            if (source.ExistObjectType)
            {
                ObjectType = (ObjectType)this.Domain.Domain.Find(source.ObjectType.Id);
            }
        }

        /// <summary>
        /// Purges the derivations.
        /// </summary>
        internal void PurgeDerivations()
        {
            RemoveDerivedRootName();
            RemoveDerivedRootObjectTypes();
        }

        /// <summary>
        /// Derive the multiplicity.
        /// </summary>
        internal void DeriveMultiplicity()
        {
            if (this.RoleType != null && this.RoleType.ExistObjectType && this.RoleType.ObjectType.IsUnit)
            {
                if (!this.ExistIsMany || this.IsMany)
                {
                    base.IsMany = false;
                }
            }
        }

        /// <summary>
        /// Derives the root name.
        /// </summary>
        internal void DeriveRootName()
        {
            DerivedRootName = null;

            if (ExistObjectType && ExistRelationTypeWhereAssociationType && RelationTypeWhereAssociationType.ExistRoleType &&
                RelationTypeWhereAssociationType.RoleType.ExistObjectType)
            {
                DerivedRootName = this.FullSingularName;

                if (ExistDerivedRootObjectTypes)
                {
                    DerivedRootName = this.SingularName;

                    foreach (var bundledType in this.RootTypes)
                    {
                        foreach (var otherRole in bundledType.RolesTypesWhereRootType)
                        {
                            if (otherRole.ExistObjectType)
                            {
                                if (otherRole.SingularName.Equals(this.SingularName))
                                {
                                    DerivedRootName = this.FullSingularName;
                                    return;
                                }
                            }
                        }

                        foreach (var otherAssociation in bundledType.AssociationTypesWhereRootType)
                        {
                            if (!this.Equals(otherAssociation))
                            {
                                if (otherAssociation.ExistObjectType)
                                {
                                    if (otherAssociation.SingularName.Equals(this.SingularName))
                                    {
                                        DerivedRootName = this.FullSingularName;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Derives the root type.
        /// </summary>
        internal void DeriveRootType()
        {
            var relationType = RelationTypeWhereAssociationType;
            var role = relationType.RoleType;

            DerivedRootObjectTypes = null;

            if (!relationType.IsManyToMany &&
                relationType.ExistExclusiveRootClasses &&
                role.IsMany)
            {
                DerivedRootObjectTypes = role.ObjectType.RootClasses;
            }
        }

        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <param name="validationLog">The validation information.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (!ExistObjectType)
            {
                var message = this.ValidationName + " has no object type";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.AssociationTypeObjectType);
            }

            if (!ExistRelationTypeWhereAssociationType)
            {
                var message = this.ValidationName + " has no relation type";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RelationTypeAssociationType);
            }

            if (ExistAssignedSingularName && !ExistAssignedPluralName)
            {
                var message = this.ValidationName + " has a singular but no plural name";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.AssociationTypeAssignedPluralName);
            }

            if (this.ExistAssignedSingularName && this.AssignedSingularName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.AssociationTypeAssignedSingularName);
            }

            if (this.ExistAssignedPluralName && this.AssignedPluralName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned plural name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.AssociationTypeAssignedPluralName);
            }
        }

        /// <summary>
        /// Ensures that the relation type derivations are up to date.
        /// </summary>
        private void EnsureRelationTypeDerivations()
        {
            if (ExistRelationTypeWhereAssociationType)
            {
                RelationTypeWhereAssociationType.EnsureRelationTypeDerivations();
            }
            else if (ExistObjectType)
            {
                ObjectType.EnsureRelationTypeDerivations();
            }
            else
            {
                this.Domain.EnsureRelationTypeDerivations();
            }
        }

        /// <summary>
        /// Make the relation type derivations stale.
        /// </summary>
        private void StaleRelationTypeDerivations()
        {
            if (this.ExistRelationTypeWhereAssociationType)
            {
                this.RelationTypeWhereAssociationType.Domain.StaleRelationTypeDerivations();
            }
            else if (this.ExistObjectType)
            {
                this.ObjectType.StaleRelationTypeDerivations();
            }
            else
            {
                this.Domain.StaleRelationTypeDerivations();
            }
        }
    }
}