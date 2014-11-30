//------------------------------------------------------------------------------------------------- 
// <copyright file="RoleType.cs" company="Allors bvba">
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
// <summary>Defines the RoleType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using AllorsGenerated;

    /// <summary>
    /// A <see cref="RoleType"/> defines the role side of a relation.
    /// This is also called the 'passive' side.
    /// RoleTypes can have composite and unit <see cref="ObjectType"/>s.
    /// </summary>
    public partial class RoleType : IComparable
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

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
        /// Gets or sets the assigned singular name.
        /// </summary>
        /// <value>The assigned plural name.</value>
        public override string AssignedSingularName
        {
            get
            {
                return base.AssignedSingularName;
            }

            set
            {
                // TODO: Test
                this.StaleRelationTypeDerivations();
                base.AssignedSingularName = value;
            }
        }

        /// <summary>
        /// Gets or sets the assigned plural name.
        /// </summary>
        /// <value>The assigned plural name.</value>
        public override string AssignedPluralName
        {
            get
            {
                return base.AssignedPluralName;
            }

            set
            {
                // TODO: Test
                this.StaleRelationTypeDerivations();
                base.AssignedPluralName = value;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public override string Name
        {
            get
            {
                if (this.IsMany)
                {
                    if (this.HierarchyPluralName != null)
                    {
                        return this.HierarchyPluralName;
                    }

                    return this.PluralName;
                }

                if (this.HierarchySingularName != null)
                {
                    return this.HierarchySingularName;
                }

                return this.SingularName;
            }
        }

        /// <summary>
        /// Gets the singular name.
        /// </summary>
        /// <value>The singular name.</value>
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
                    return ObjectType.ExistSingularName ? ObjectType.SingularName : ObjectType.ToString();
                }

                if (ExistRelationTypeWhereRoleType)
                {
                    return RelationTypeWhereRoleType.SafeName;
                }

                return AllorsObjectId.ToString(CultureInfo.InvariantCulture);
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
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public string FullName
        {
            get
            {
                if (this.IsMany)
                {
                    return this.FullPluralName;
                }

                return this.FullSingularName;
            }
        }

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full singular name.</value>
        public string FullSingularName
        {
            get { return this.RelationTypeWhereRoleType.AssociationType.SingularName + this.SingularName; }
        }

        /// <summary>
        /// Gets the full plural name.
        /// </summary>
        /// <value>The full plural name.</value>
        public string FullPluralName
        {
            get { return this.RelationTypeWhereRoleType.AssociationType.SingularName + this.PluralName; }
        }

        /// <summary>
        /// Gets the name of the root.
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
        /// Gets the hierarchy singular name.
        /// </summary>
        /// <value>The name of the hierarchy singular.</value>
        public string HierarchySingularName
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return DerivedHierarchySingularName;
            }
        }

        /// <summary>
        /// Gets the hierarchy plural name.
        /// </summary>
        /// <value>The name of the hierarchy plural.</value>
        public string HierarchyPluralName
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return DerivedHierarchyPluralName;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has multiplicity one.
        /// </summary>
        /// <value><c>true</c> if this instance's multiplicity is one; otherwise, <c>false</c>.</value>
        public bool IsOne
        {
            get { return !this.IsMany; }
            set { this.IsMany = !value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has multiplicity is many.
        /// </summary>
        /// <value><c>true</c> if this instance has multiplicity many; otherwise, <c>false</c>.</value>
        public override bool IsMany
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.IsMany;
            }

            set
            {
                // TODO: Test
                this.StaleRelationTypeDerivations();
                base.IsMany = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether size exists.
        /// </summary>
        public override bool ExistSize
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.ExistSize;
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public override int Size
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.Size;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.Size = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether scale exists.
        /// </summary>
        public override bool ExistScale
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.ExistScale;
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public override int Scale
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.Scale;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.Scale = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether precision exists.
        /// </summary>
        public override bool ExistPrecision
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.ExistPrecision;
            }
        }

        /// <summary>
        /// Gets or sets the precision.
        /// </summary>
        public override int Precision
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return base.Precision;
            }

            set
            {
                this.StaleRelationTypeDerivations();
                base.Precision = value;
            }
        }

        /// <summary>
        /// Gets the type of the relation.
        /// </summary>
        /// <value>The type of the relation.</value>
        public RelationType RelationType
        {
            get { return RelationTypeWhereRoleType; }
        }

        /// <summary>
        /// Gets the association.
        /// </summary>
        /// <value>The association.</value>
        public AssociationType AssociationType
        {
            get { return RelationTypeWhereRoleType.AssociationType; }
        }

        /// <summary>
        /// Gets or sets <see cref="ObjectType"/>.
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
        /// Gets the root objectTypes.
        /// </summary>
        /// <value>The root objectTypes.</value>
        public ObjectType[] RootTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return DerivedRootTypes;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exist root objectTypes.
        /// </summary>
        /// <value><c>true</c> if there exist root objectTypes; otherwise, <c>false</c>.</value>
        public bool ExistRootTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return ExistDerivedRootTypes;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's assigned singular name is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's assigned singular name is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedSingularNameDefault
        {
            get { return !ExistAssignedSingularName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's assigned plural name is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's assigned plural name is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedPluralNameDefault
        {
            get { return !ExistAssignedPluralName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's is many is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's is many is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsIsManyDefault
        {
            get { return this.IsMany.Equals(false); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's object type is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance object type is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsObjectTypeDefault
        {
            get { return !ExistObjectType; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's precision is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's precision is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrecisionDefault
        {
            get { return !this.ExistPrecision; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's scale is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's is default scale; otherwise, <c>false</c>.
        /// </value>
        public bool IsScaleDefault
        {
            get { return !this.ExistScale; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's size is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's size is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsSizeDefault
        {
            get { return !this.ExistSize; }
        }
        
        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected override string ValidationName
        {
            get
            {
                if (ExistRelationTypeWhereRoleType)
                {
                    return "role type " + RelationTypeWhereRoleType.Name;
                }

                return "unknown role type";
            }
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
            var that = obj as RoleType;
            if (that != null)
            {
                return string.CompareOrdinal(this.SingularName, that.SingularName);
            }

            return -1;
        }

        /// <summary>
        /// Copy from source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Copy(RoleType source)
        {
            this.CopyMetaObject(source);

            this.AssignedPluralName = source.PluralName;
            this.AssignedSingularName = source.SingularName;
            this.IsMany = source.IsMany;
            if (source.ExistSize)
            {
                this.Size = source.Size;
            }

            if (source.ExistPrecision)
            {
                this.Precision = source.Precision;
            }

            if (source.ExistScale)
            {
                this.Scale = source.Scale;
            }

            if (source.ExistObjectType)
            {
                ObjectType = (ObjectType)this.Domain.Domain.Find(source.ObjectType.Id);
            }
        }

        /// <summary>
        /// Send the changed event.
        /// </summary>
        public override void SendChangedEvent()
        {
            if (this.ExistRelationTypeWhereRoleType)
            {
                this.RelationTypeWhereRoleType.SendChangedEvent();
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            throw new NotSupportedException();
        }
        
        /// <summary>
        /// Removes the Id.
        /// </summary>
        public override void RemoveId()
        {
            if (ExistId)
            {
                throw new ArgumentException("Id is write once");
            }

            base.RemoveId();
        }

        /// <summary>
        /// Removes the assigned plural name.
        /// </summary>
        public override void RemoveAssignedPluralName()
        {
            // TODO: Test
            this.StaleRelationTypeDerivations();
            base.RemoveAssignedPluralName();
        }

        /// <summary>
        /// Removes the assigned singular name.
        /// </summary>
        public override void RemoveAssignedSingularName()
        {
            // TODO: Test
            this.StaleRelationTypeDerivations();
            base.RemoveAssignedSingularName();
        }

        /// <summary>
        /// Removes the is many.
        /// </summary>
        public override void RemoveIsMany()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveIsMany();
        }

        /// <summary>
        /// Removes the object type.
        /// </summary>
        public override void RemoveObjectType()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveObjectType();
        }

        /// <summary>
        /// Remove size.
        /// </summary>
        public override void RemoveSize()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveSize();
        }

        /// <summary>
        /// Remove scale.
        /// </summary>
        public override void RemoveScale()
        {
            this.StaleRelationTypeDerivations();
            base.RemoveScale();
        }

        /// <summary>
        /// Remove precision.
        /// </summary>
        public override void RemovePrecision()
        {
            this.StaleRelationTypeDerivations();
            base.RemovePrecision();
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
        /// Get the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The role value.
        /// </returns>
        public override object Get(IStrategy strategy)
        {
            return strategy.GetRole(this);
        }

        /// <summary>
        /// Set the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <param name="value">
        /// The role value.
        /// </param>
        public void Set(IStrategy strategy, object value)
        {
            strategy.SetRole(this, value);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.RemoveAssignedPluralName();
            this.RemoveAssignedSingularName();
            this.IsMany = false;
            this.RemoveScale();
            this.RemoveSize();
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
        /// <returns>The new instance.</returns>
        internal static RoleType Create(AllorsEmbeddedSession session)
        {
            var role = (RoleType)session.Create(AllorsEmbeddedDomain.RoleType);
            role.Reset();
            return role;
        }

        /// <summary>
        /// Delete for internal purpose.
        /// </summary>
        internal void InternalDelete()
        {
            base.Delete();
        }

        /// <summary>
        /// Purges the derivations.
        /// </summary>
        internal void PurgeDerivations()
        {
            RemoveDerivedHierarchyPluralName();
            RemoveDerivedHierarchySingularName();
            RemoveDerivedRootName();
            RemoveDerivedRootTypes();
        }

        /// <summary>
        /// Derive multiplicity, scale and size.
        /// </summary>
        internal void DeriveMultiplicityScaleAndSize()
        {
            if (this.ExistObjectType && this.ObjectType.IsUnit)
            {
                if (!this.ExistIsMany || this.IsMany)
                {
                    base.IsMany = false;
                }

                switch ((UnitTypeTags)this.ObjectType.UnitTag)
                {
                    case UnitTypeTags.AllorsString:
                        if (!this.ExistSize)
                        {
                            this.Size = 256;

                            base.RemoveScale();
                            base.RemovePrecision();
                        }

                        break;
                    case UnitTypeTags.AllorsBinary:
                        if (!this.ExistSize)
                        {
                            this.Size = MaximumSize;

                            base.RemoveScale();
                            base.RemovePrecision();
                        }

                        break;
                    case UnitTypeTags.AllorsDecimal:
                        if (!this.ExistPrecision)
                        {
                            this.Precision = 19;
                        }

                        if (!this.ExistScale)
                        {
                            this.Scale = 2;
                        }

                        base.RemoveSize();
                        break;

                    default:
                        base.RemoveSize();
                        base.RemoveScale();
                        base.RemovePrecision();
                        break;
                }
            }
            else
            {
                base.RemoveSize();
                base.RemoveScale();
                base.RemovePrecision();
            }
        }

        /// <summary>
        /// Derive root objectTypes.
        /// </summary>
        internal void DeriveRootTypes()
        {
            var relationType = RelationTypeWhereRoleType;
            var association = relationType.AssociationType;
            var role = this;

            DerivedRootTypes = null;

            // TODO: Test
            if (association.ExistObjectType)
            {
                if (ObjectType.IsUnit)
                {
                    DerivedRootTypes = association.ObjectType.RootClasses;
                }
                else
                {
                    if (!relationType.IsManyToMany &&
                        relationType.ExistExclusiveRootClasses &&
                        !role.IsMany)
                    {
                        DerivedRootTypes = association.ObjectType.RootClasses;
                    }
                }
            }
        }

        /// <summary>
        /// Derive hierarchy plural name.
        /// </summary>
        /// <param name="objectTypes">The object Types.</param>
        internal void DeriveHierarchyPluralName(HashSet<ObjectType> objectTypes)
        {
            objectTypes.Clear();
            DerivedHierarchyPluralName = null;

            if (ExistObjectType && ExistRelationTypeWhereRoleType && RelationTypeWhereRoleType.ExistAssociationType &&
                RelationTypeWhereRoleType.AssociationType.ExistObjectType)
            {
                var myAssociation = RelationTypeWhereRoleType.AssociationType;
                var myAssociationType = myAssociation.ObjectType;

                objectTypes.Add(myAssociationType);
                objectTypes.UnionWith(myAssociationType.Supertypes);
                objectTypes.UnionWith(myAssociationType.Subtypes);
                foreach (var subType in myAssociationType.Subtypes)
                {
                    foreach (var superType in subType.Supertypes)
                    {
                        if (!objectTypes.Contains(superType))
                        {
                            objectTypes.Add(superType);
                        }
                    }
                }

                DerivedHierarchyPluralName = this.PluralName;

                foreach (var type in objectTypes)
                {
                    foreach (var otherAsssociation in type.AssociationTypesWhereObjectType)
                    {
                        if (!myAssociation.Equals(otherAsssociation))
                        {
                            if (otherAsssociation.ExistRelationTypeWhereAssociationType)
                            {
                                var otherRelationType = otherAsssociation.RelationTypeWhereAssociationType;
                                if (otherRelationType.ExistRoleType)
                                {
                                    var otherRole = otherRelationType.RoleType;
                                    if (otherRole.ExistObjectType)
                                    {
                                        // TODO: Test for PluralName == null
                                        if (otherRole.PluralName.Equals(this.PluralName))
                                        {
                                            DerivedHierarchyPluralName = this.FullPluralName;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Derive hierarchy singular name.
        /// </summary>
        /// <param name="objectTypes">The object types.</param>
        internal void DeriveHierarchySingularName(HashSet<ObjectType> objectTypes)
        {
            objectTypes.Clear();
            DerivedHierarchySingularName = null;

            if (ExistObjectType && ExistRelationTypeWhereRoleType && RelationTypeWhereRoleType.ExistAssociationType &&
                RelationTypeWhereRoleType.AssociationType.ExistObjectType)
            {
                var myAssociation = RelationTypeWhereRoleType.AssociationType;
                var myAssociationType = myAssociation.ObjectType;

                objectTypes.Add(myAssociationType);
                objectTypes.UnionWith(myAssociationType.Supertypes);
                objectTypes.UnionWith(myAssociationType.Subtypes);
                foreach (var subType in myAssociationType.Subtypes)
                {
                    foreach (var superType in subType.Supertypes)
                    {
                        if (!objectTypes.Contains(superType))
                        {
                            objectTypes.Add(superType);
                        }
                    }
                }

                DerivedHierarchySingularName = this.SingularName;

                foreach (var type in objectTypes)
                {
                    foreach (var otherAsssociation in type.AssociationTypesWhereObjectType)
                    {
                        if (!myAssociation.Equals(otherAsssociation))
                        {
                            if (otherAsssociation.ExistRelationTypeWhereAssociationType)
                            {
                                var otherRelationType = otherAsssociation.RelationTypeWhereAssociationType;
                                if (otherRelationType.ExistRoleType)
                                {
                                    var otherRole = otherRelationType.RoleType;
                                    if (otherRole.ExistObjectType)
                                    {
                                        if (otherRole.SingularName.Equals(this.SingularName))
                                        {
                                            DerivedHierarchySingularName = this.FullSingularName;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Derive root name.
        /// </summary>
        internal void DeriveRootName()
        {
            DerivedRootName = null;

            if (ExistObjectType && ExistRelationTypeWhereRoleType && RelationTypeWhereRoleType.ExistAssociationType &&
                RelationTypeWhereRoleType.AssociationType.ExistObjectType)
            {
                DerivedRootName = this.FullSingularName;

                if (ExistDerivedRootTypes)
                {
                    DerivedRootName = this.SingularName;

                    foreach (var rootType in DerivedRootTypes)
                    {
                        foreach (var otherRole in rootType.RolesTypesWhereRootType)
                        {
                            if (!Equals(otherRole))
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
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (!ExistObjectType)
            {
                var message = this.ValidationName + " has no ObjectType";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypeObjectType);
            }
            else
            {
                if (this.ObjectType.IsUnit)
                {
                    switch ((UnitTypeTags)this.ObjectType.UnitTag)
                    {
                        case UnitTypeTags.AllorsString:
                            if (!this.ExistSize)
                            {
                                var message = this.ValidationName + " should have a size.";
                                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypeSize);
                            }

                            break;
                        case UnitTypeTags.AllorsBinary:
                            if (!this.ExistSize)
                            {
                                var message = this.ValidationName + " should have a size.";
                                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypeSize);
                            }

                            break;
                        case UnitTypeTags.AllorsDecimal:
                            if (!this.ExistPrecision)
                            {
                                var message = this.ValidationName + " should have a precision.";
                                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypePrecision);
                            }

                            if (!this.ExistScale)
                            {
                                var message = this.ValidationName + " should have a scale.";
                                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypeScale);
                            }

                            break;
                    }
                }
            }

            if (!ExistRelationTypeWhereRoleType)
            {
                var message = this.ValidationName + " has no relation type";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RelationTypeRoleType);
            }

            if (ExistAssignedSingularName && !ExistAssignedPluralName)
            {
                var message = this.ValidationName + " has a singular but no plural name";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.RoleTypeAssignedPluralName);
            }

            if (this.ExistAssignedSingularName && this.AssignedSingularName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.RoleTypeAssignedSingularName);
            }

            if (this.ExistAssignedPluralName && this.AssignedPluralName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.RoleTypeAssignedPluralName);
            }
        }

        /// <summary>
        /// Ensures that the relation type derivations are up to date.
        /// </summary>
        private void EnsureRelationTypeDerivations()
        {
            if (ExistRelationTypeWhereRoleType)
            {
                RelationTypeWhereRoleType.EnsureRelationTypeDerivations();
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
        /// Stales the relation type derivations.
        /// </summary>
        private void StaleRelationTypeDerivations()
        {
            if (this.ExistRelationTypeWhereRoleType)
            {
                this.RelationTypeWhereRoleType.Domain.StaleRelationTypeDerivations();
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