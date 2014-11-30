//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectType.cs" company="Allors bvba">
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
// <summary>Defines the ObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using AllorsGenerated;

    /// <summary>
    /// An <see cref="ObjectType"/> defines the state and behavior for
    /// a set of <see cref="IObject"/>s.
    /// </summary>
    public partial class ObjectType : IComparable
    {
        /// <summary>
        /// An empty array of object types.
        /// </summary>
        private static readonly ObjectType[] EmptyArray = new ObjectType[0];

        /// <summary>
        /// A cache for the ids of the <see cref="AssociationTypes"/>.
        /// </summary>
        private Dictionary<Guid, object> associationIdsCache;
        
        /// <summary>
        /// A cache for the ids of the <see cref="RoleTypes"/>.
        /// </summary>
        private Dictionary<Guid, object> roleIdsCache;

        /// <summary>
        /// A cache for the ids of the <see cref="RoleTypes"/>.
        /// </summary>
        private HashSet<ObjectType> concreteClassesCache;

        /// <summary>
        /// Gets or sets the user defined object that will act as an extension.
        /// </summary>
        /// <value>The user defined extension object.</value>
        public IObjectTypeExtension Extension { get; set; }

        /// <summary>
        /// Gets the association count.
        /// </summary>
        /// <value>The association count.</value>
        public int AssociationTypesCount
        {
            get { return this.AssociationTypes.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the association count is greater than 32.
        /// </summary>
        /// <value>
        /// <c>true</c> if association count is greater than 32; otherwise, <c>false</c>.
        /// </value>
        public bool AssociationTypesCountGreaterThan32
        {
            get { return this.AssociationTypes.Length > 32; }
        }

        /// <summary>
        /// Gets the associations.
        /// </summary>
        /// <value>The associations.</value>
        public AssociationType[] AssociationTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedAssociationTypes;
            }
        }

        /// <summary>
        /// Gets the associations where this instance is the root type.
        /// </summary>
        /// <value>The associations where this instance is the root type.</value>
        public AssociationType[] AssociationTypesWhereRootType
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.AssociationTypesWhereDerivedRootObjectType;
            }
        }

        /// <summary>
        /// Gets the binary roles.
        /// </summary>
        /// <value>The binary roles.</value>
        public RoleType[] BinaryRoles
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsBinary); }
        }

        /// <summary>
        /// Gets the boolean roles.
        /// </summary>
        /// <value>The boolean roles.</value>
        public RoleType[] BooleanRoles
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsBoolean); }
        }

        /// <summary>
        /// Gets the double roles.
        /// </summary>
        /// <value>The double roles.</value>
        public RoleType[] DoubleRoleTypes
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsDouble); }
        }

        /// <summary>
        /// Gets the date time role types.
        /// </summary>
        /// <value>The date time role types.</value>
        public RoleType[] DateTimeRoleTypes
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsDateTime); }
        }

        /// <summary>
        /// Gets the decimal role types.
        /// </summary>
        /// <value>The decimal role types.</value>
        public RoleType[] DecimalRoleTypes
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsDecimal); }
        }

        /// <summary>
        /// Gets the integer32 roles.
        /// </summary>
        /// <value>The integer32 roles.</value>
        public RoleType[] Integer32Roles
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsInteger); }
        }

        /// <summary>
        /// Gets the integer64 roles.
        /// </summary>
        /// <value>The integer64 roles.</value>
        public RoleType[] Integer64Roles
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsLong); }
        }

        /// <summary>
        /// Gets the string roles.
        /// </summary>
        /// <value>The string roles.</value>
        public RoleType[] StringRoleTypes
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsString); }
        }

        /// <summary>
        /// Gets the composite role count.
        /// </summary>
        /// <value>The composite role count.</value>
        public int CompositeRoleTypeCount
        {
            get { return this.CompositeRoleTypes.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the composite role count is greater than 32.
        /// </summary>
        /// <value>
        /// <c>true</c> if the composite role count is greater than32; otherwise, <c>false</c>.
        /// </value>
        public bool CompositeRoleTypeCountGreaterThan32
        {
            get { return this.CompositeRoleTypes.Length > 32; }
        }

        /// <summary>
        /// Gets the composite roles.
        /// </summary>
        /// <value>The composite roles.</value>
        public RoleType[] CompositeRoleTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedCompositeRoleTypes;
            }
        }

        /// <summary>
        /// Gets the concrete sub classes or
        /// self if this is a concrete class.
        /// </summary>
        /// <value>The concrete classes.</value>
        public ObjectType[] ConcreteClasses
        {
            get
            {
                if (this.IsConcreteComposite)
                {
                    ObjectType[] selfArray = { this };
                    return selfArray;
                }

                return this.Subclasses.Length == 0 ? EmptyArray : this.ConcreteSubClasses;
            }
        }

        /// <summary>
        /// Gets the direct subtypes.
        /// </summary>
        /// <value>The direct subtypes.</value>
        public ObjectType[] DirectSubtypes
        {
            get { return this.ObjectTypesWhereDirectSupertype; }
        }

        /// <summary>
        /// Gets the direct superclass.
        /// </summary>
        /// <value>The direct superclass.</value>
        public ObjectType DirectSuperclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedDirectSuperclass;
            }
        }

        /// <summary>
        /// Gets the direct super interfaces.
        /// </summary>
        /// <value>The direct super interfaces.</value>
        public ObjectType[] DirectSuperinterfaces
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedDirectSuperinterfaces;
            }
        }

        /// <summary>
        /// Gets the direct super types.
        /// </summary>
        /// <value>The direct super types.</value>
        public ObjectType[] DirectSupertypes
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedDirectSupertypes;
            }
        }

        /// <summary>
        /// Gets the exclusive associations.
        /// </summary>
        /// <value>The exclusive associations.</value>
        public AssociationType[] ExclusiveAssociationTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedExclusiveAssociationTypes;
            }
        }

        /// <summary>
        /// Gets the exclusive concrete subclass.
        /// </summary>
        /// <value>The exclusive concrete subclass.</value>
        public ObjectType ExclusiveConcreteSubclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedExclusiveConcreteLeafClass;
            }
        }

        /// <summary>
        /// Gets the exclusive roles.
        /// </summary>
        /// <value>The exclusive roles.</value>
        public RoleType[] ExclusiveRoleTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedExclusiveRoleTypes;
            }
        }

        /// <summary>
        /// Gets the exclusive root class.
        /// </summary>
        /// <value>The exclusive root class.</value>
        public ObjectType ExclusiveRootClass
        {
            get
            {
                if (this.ExistExclusiveRootClass)
                {
                    return this.RootClasses[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the exclusive super interfaces.
        /// </summary>
        /// <value>The exclusive super interfaces.</value>
        public ObjectType[] ExclusiveSuperinterfaces
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedExclusiveSuperinterfaces;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exist concrete classes.
        /// </summary>
        /// <value>
        ///  <c>true</c> if there exist concrete classes; otherwise, <c>false</c>.
        /// </value>
        public bool ExistConcreteClasses
        {
            get { return this.ConcreteClasses.Length > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether there exists a direct superclass.
        /// </summary>
        /// <value>
        ///  <c>true</c> if there exists a direct superclass; otherwise, <c>false</c>.
        /// </value>
        public bool ExistDirectSuperclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedDirectSuperclass;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exists direct super interfaces.
        /// </summary>
        /// <value>
        ///  <c>true</c> if there exists direct super interfaces; otherwise, <c>false</c>.
        /// </value>
        public bool ExistDirectSuperinterfaces
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedDirectSuperinterfaces;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exists direct super types.
        /// </summary>
        /// <value>
        /// <c>true</c> if there exists direct super types; otherwise, <c>false</c>.
        /// </value>
        public bool ExistDirectSupertypes
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedDirectSupertypes;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exists an exclusive concrete subclass.
        /// </summary>
        /// <value>
        /// <c>true</c> if there exists an exclusive concrete subclass; otherwise, <c>false</c>.
        /// </value>
        public bool ExistExclusiveConcreteSubclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedExclusiveConcreteLeafClass;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [exist exclusive root class].
        /// </summary>
        /// <value>
        /// <c>true</c> if [exist exclusive root class]; otherwise, <c>false</c>.
        /// </value>
        public bool ExistExclusiveRootClass
        {
            get { return this.RootClasses.Length == 1; }
        }

        /// <summary>
        /// Gets a value indicating whether there exist root classes.
        /// </summary>
        /// <value><c>true</c> if there exist root classes; otherwise, <c>false</c>.</value>
        public bool ExistRootClasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedRootClasses;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exist subclasses.
        /// </summary>
        /// <value><c>true</c> if there exist subclasses; otherwise, <c>false</c>.</value>
        public bool ExistSubclasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedSubclasses;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there exists super classes.
        /// </summary>
        /// <value><c>true</c> if there exist super classes; otherwise, <c>false</c>.</value>
        public bool ExistSuperclasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistDerivedSuperclasses;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an abstract composite.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is an abstract composite; otherwise, <c>false</c>.
        /// </value>
        public bool IsAbstractComposite
        {
            get { return !this.IsUnit && this.IsAbstract; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a binary.
        /// </summary>
        /// <value><c>true</c> if this instance is a binary; otherwise, <c>false</c>.</value>
        public bool IsBinary
        {
            get { return this.Id.Equals(UnitTypeIds.BinaryId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a boolean.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean
        {
            get { return this.Id.Equals(UnitTypeIds.BooleanId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a class.
        /// </summary>
        /// <value><c>true</c> if this instance is a class; otherwise, <c>false</c>.</value>
        public bool IsClass
        {
            get { return !this.IsInterface; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a composite.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a composite; otherwise, <c>false</c>.
        /// </value>
        public bool IsComposite
        {
            get { return !this.IsUnit; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is concrete.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is concrete; otherwise, <c>false</c>.
        /// </value>
        public bool IsConcrete
        {
            get { return !this.IsInterface && !this.IsAbstract; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a concrete composite.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a concrete composite; otherwise, <c>false</c>.
        /// </value>
        public bool IsConcreteComposite
        {
            get { return !this.IsUnit && this.IsConcrete; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a date time.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a date time; otherwise, <c>false</c>.
        /// </value>
        public bool IsDateTime
        {
            get { return this.Id.Equals(UnitTypeIds.DatetimeId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a decimal.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a decimal; otherwise, <c>false</c>.
        /// </value>
        public bool IsDecimal
        {
            get { return this.Id.Equals(UnitTypeIds.DecimalId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's assigned plural name is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's assigned plural name is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedPluralNameDefault
        {
            get { return !this.ExistPluralName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's assigned singular name is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's assigned singular name is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedSingularNameDefault
        {
            get { return !this.ExistSingularName; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's is abstract is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's is abstract is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsIsAbstractDefault
        {
            get { return this.IsAbstract.Equals(false); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is interface is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is is interface default; otherwise, <c>false</c>.
        /// </value>
        public bool IsIsInterfaceDefault
        {
            get { return this.IsInterface.Equals(false); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is unit is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is unit is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsIsUnitDefault
        {
            get { return this.IsUnit.Equals(false); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance unit tag is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance unit tag is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnitTagDefault
        {
            get { return !this.ExistUnitTag; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a double.
        /// </summary>
        /// <value><c>true</c> if this instance is a double; otherwise, <c>false</c>.</value>
        public bool IsDouble
        {
            get { return this.Id.Equals(UnitTypeIds.DoubleId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an integer.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is an integer; otherwise, <c>false</c>.
        /// </value>
        public bool IsInteger
        {
            get { return this.Id.Equals(UnitTypeIds.IntegerId); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an interface.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is an interface; otherwise, <c>false</c>.
        /// </value>
        public override bool IsInterface
        {
            get
            {
                return base.IsInterface;
            }

            set
            {
                base.IsInterface = value;

                this.Domain.StaleObjectTypeDerivations();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is long.
        /// </summary>
        /// <value><c>true</c> if this instance is a long; otherwise, <c>false</c>.</value>
        public bool IsLong
        {
            get { return this.Id.Equals(UnitTypeIds.LongId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance requires precision.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance requires precision; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrecisionRequired
        {
            get
            {
                if (this.IsDecimal)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a root class.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a root class; otherwise, <c>false</c>.
        /// </value>
        public bool IsRootClass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ExistObjectTypesWhereDerivedRootClass;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance requires a scale.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance requires a scale; otherwise, <c>false</c>.
        /// </value>
        public bool IsScaleRequired
        {
            get
            {
                if (this.IsDecimal)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance requires a size.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance requires a size; otherwise, <c>false</c>.
        /// </value>
        public bool IsSizeRequired
        {
            get
            {
                if (this.IsString || this.IsBinary)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a string.
        /// </summary>
        /// <value><c>true</c> if this instance is a string; otherwise, <c>false</c>.</value>
        public bool IsString
        {
            get { return this.Id.Equals(UnitTypeIds.StringId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a unique.
        /// </summary>
        /// <value><c>true</c> if this instance is a unique; otherwise, <c>false</c>.</value>
        public bool IsUnique
        {
            get { return this.Id.Equals(UnitTypeIds.Unique); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name .</value>
        public string Name
        {
            get
            {
                if (this.ExistSingularName)
                {
                    return this.SingularName;
                }

                return this.IdAsString;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is a direct superclass.
        /// </summary>
        /// <value>The object types where this instance is a direct superclass.</value>
        public ObjectType[] ObjectTypesWhereDirectSuperclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedDirectSuperclass;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is a direct super interface.
        /// </summary>
        /// <value>The object types where this instance is a direct super interface.</value>
        public ObjectType[] ObjectTypesWhereDirectSuperinterface
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedDirectSuperinterface;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is an exclusive super interface.
        /// </summary>
        /// <value>The object types where this instance is an exclusive super interface.</value>
        public ObjectType[] ObjectTypesWhereExclusiveSuperinterface
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedExclusiveSuperinterface;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is the root class.
        /// </summary>
        /// <value>The object types where this instance is the root class.</value>
        public ObjectType[] ObjectTypesWhereRootClass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedRootClass;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is the super class.
        /// </summary>
        /// <value>The object types where this instance is the super class.</value>
        public ObjectType[] ObjectTypesWhereSuperclass
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedSuperclass;
            }
        }

        /// <summary>
        /// Gets the object types where this instance is the super interface.
        /// </summary>
        /// <value>The object types where this instance is the super interface.</value>
        public ObjectType[] ObjectTypesWhereSuperinterface
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedSuperinterface;
            }
        }

        /// <summary>
        /// Gets or sets the plural name.
        /// </summary>
        /// <value>The plural name.</value>
        public override string PluralName
        {
            get
            {
                return base.PluralName;
            }

            set
            {
                base.PluralName = value;

                this.Domain.StaleObjectTypeDerivations();
            }
        }

        /// <summary>
        /// Gets the method types.
        /// </summary>
        /// <value>The method types.</value>
        public MethodType[] MethodTypes
        {
            get
            {
                this.EnsureMethodTypeDerivations();
                return this.DerivedMethodTypes;
            }
        }

        /// <summary>
        /// Gets the role count.
        /// </summary>
        /// <value>The role count.</value>
        public int RoleTypeCount
        {
            get { return this.RoleTypes.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the role count is greater than 32.
        /// </summary>
        /// <value>
        ///  <c>true</c> if the role count is greater than32; otherwise, <c>false</c>.
        /// </value>
        public bool RoleTypeCountGreaterThan32
        {
            get { return this.RoleTypes.Length > 32; }
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public RoleType[] RoleTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedRoleTypes;
            }
        }

        /// <summary>
        /// Gets the roles where this instance is the root type.
        /// </summary>
        /// <value>The roles where this instance is the root type.</value>
        public RoleType[] RolesTypesWhereRootType
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.RoleTypesWhereDerivedRootType;
            }
        }

        /// <summary>
        /// Gets the root classes.
        /// </summary>
        /// <value>The root classes.</value>
        public ObjectType[] RootClasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedRootClasses;
            }
        }

        /// <summary>
        /// Gets or sets the singular name.
        /// </summary>
        /// <value>The singular name.</value>
        public override string SingularName
        {
            get
            {
                return base.SingularName;
            }

            set
            {
                base.SingularName = value;

                this.Domain.StaleObjectTypeDerivations();
            }
        }

        /// <summary>
        /// Gets the subclasses.
        /// </summary>
        /// <value>The subclasses.</value>
        public ObjectType[] Subclasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedSubclasses;
            }
        }

        /// <summary>
        /// Gets the sub interfaces.
        /// </summary>
        /// <value>The sub interfaces.</value>
        public ObjectType[] Subinterfaces
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedSubinterfaces;
            }
        }

        /// <summary>
        /// Gets the subtypes.
        /// </summary>
        /// <value>The subtypes.</value>
        public ObjectType[] Subtypes
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedSupertype;
            }
        }

        /// <summary>
        /// Gets the super classes.
        /// </summary>
        /// <value>The super classes.</value>
        public ObjectType[] Superclasses
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedSuperclasses;
            }
        }

        /// <summary>
        /// Gets the super interfaces.
        /// </summary>
        /// <value>The super interfaces.</value>
        public ObjectType[] Superinterfaces
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedSuperinterfaces;
            }
        }

        /// <summary>
        /// Gets the super types.
        /// </summary>
        /// <value>The super types.</value>
        public ObjectType[] Supertypes
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.DerivedSupertypes;
            }
        }

        /// <summary>
        /// Gets the unique roles.
        /// </summary>
        /// <value>The unique roles.</value>
        public RoleType[] UniqueRoleTypes
        {
            get { return this.GetUnitRoleTypes(UnitTypeTags.AllorsUnique); }
        }

        /// <summary>
        /// Gets the unit role count.
        /// </summary>
        /// <value>The unit role count.</value>
        public int UnitRoleTypesCount
        {
            get { return this.UnitRoleTypes.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the unit role count is greater than32.
        /// </summary>
        /// <value>
        ///  <c>true</c> if the unit role count is greater than 32; otherwise, <c>false</c>.
        /// </value>
        public bool UnitRoleTypesCountGreaterThan32
        {
            get { return this.UnitRoleTypes.Length > 32; }
        }

        /// <summary>
        /// Gets the unit roles.
        /// </summary>
        /// <value>The unit roles.</value>
        public RoleType[] UnitRoleTypes
        {
            get
            {
                this.EnsureRelationTypeDerivations();
                return this.DerivedUnitRoleTypes;
            }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected override string ValidationName
        {
            get
            {
                if (this.ExistSingularName)
                {
                    return "object type " + this.SingularName;
                }
                
                if (this.ExistId)
                {
                    return "object type " + this.Id;
                }

                return "unknown object type";
            }
        }

        /// <summary>
        /// Gets the concrete sub classes.
        /// </summary>
        /// <value>The concrete sub classes.</value>
        private ObjectType[] ConcreteSubClasses
        {
            get
            {
                var concreteSubClasses = new List<ObjectType>();
                foreach (var subClass in this.Subclasses)
                {
                    if (subClass.IsConcrete)
                    {
                        concreteSubClasses.Add(subClass);
                    }
                }

                return concreteSubClasses.ToArray();
            }
        }

        /// <summary>
        /// Gets the object types where this instance is a direct supertype.
        /// </summary>
        /// <value>The object types where this instance is a direct supertype.</value>
        private ObjectType[] ObjectTypesWhereDirectSupertype
        {
            get
            {
                this.EnsureObjectTypeDerivations();
                return this.ObjectTypesWhereDerivedDirectSupertype;
            }
        }

        /// <summary>
        /// Adds the direct supertype.
        /// </summary>
        /// <param name="supertype">The supertype.</param>
        /// <returns>The inheritance.</returns> 
        public Inheritance AddDirectSupertype(ObjectType supertype)
        {
            if (supertype == null)
            {
                return null;
            }

            if (!this.IsValidSupertype(supertype))
            {
                throw new ArgumentException(supertype + " is not a valid supertype for " + this);
            }

            var inheritance = this.FindInheritanceWhereDirectSubtype(supertype);
            if (inheritance == null)
            {
                if (supertype.IsConcreteComposite)
                {
                    throw new ArgumentException("The inheritance " + this + "::" + supertype + " can not have a concrete superclass");
                }

                if (supertype.IsAbstractComposite && this.IsInterface)
                {
                    throw new ArgumentException("The inheritance " + this + "::" + supertype + " can not have an abstract");
                }

                if (supertype.IsClass && this.ExistDirectSuperclass)
                {
                    this.FindInheritanceWhereDirectSubtype(this.DirectSuperclass).Delete();
                }

                inheritance = this.Domain.AddDeclaredInheritance(Guid.NewGuid());
                inheritance.Subtype = this;
                inheritance.Supertype = supertype;
            }

            return inheritance;
        }

        /// <summary>
        /// Adds a <see cref="MethodType"/> to this object.
        /// </summary>
        /// <param name="methodId">The method id</param>
        /// <param name="methodName">The method name</param>
        /// <returns>The method type</returns>
        public MethodType AddMethodType(Guid methodId, string methodName)
        {
            var methodType = this.Domain.AddDeclaredMethodType(methodId);
            methodType.ObjectType = this;
            methodType.Name = methodName;
            return methodType;
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
            var that = obj as ObjectType;
            if (that != null)
            {
                return string.CompareOrdinal(this.Name, that.Name);
            }

            return -1;
        }

        /// <summary>
        /// Copy form source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public void Copy(ObjectType source)
        {
            this.CopyMetaObject(source);

            this.PluralName = source.PluralName;
            this.SingularName = source.SingularName;
            this.IsAbstract = source.IsAbstract;
            this.IsInterface = source.IsInterface;
            this.IsUnit = source.IsUnit;

            if (this.ExistUnitTag)
            {
                this.UnitTag = source.UnitTag;
            }
        }

        /// <summary>
        /// Determines whether this instance contains the specified association.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <returns>
        ///  <c>true</c> if this instance contains the specified association; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsAssociationType(AssociationType association)
        {
            this.EnsureRelationTypeDerivations();
            return this.associationIdsCache.ContainsKey(association.RelationTypeWhereAssociationType.Id);
        }

        /// <summary>
        /// Determines whether this instance contains the specified role.
        /// </summary>
        /// <param name="role">The role .</param>
        /// <returns>
        ///  <c>true</c> if this instance contains the specified role; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsRoleType(RoleType role)
        {
            this.EnsureRelationTypeDerivations();
            return this.roleIdsCache.ContainsKey(role.RelationTypeWhereRoleType.Id);
        }

        /// <summary>
        /// Contains this concrete class.
        /// </summary>
        /// <param name="objectType">
        /// The concrete class.
        /// </param>
        /// <returns>
        /// True if this contains the concrete class.
        /// </returns>
        public bool ContainsConcreteClass(ObjectType objectType)
        {
            this.EnsureObjectTypeDerivations();
            return this.concreteClassesCache.Contains(objectType);
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            var domain = this.Domain;
            var deleteId = this.Id;

            this.Reset();
            base.Delete();

            domain.StaleObjectTypeDerivations();
            domain.SendDeletedEvent(deleteId);
        }

        /// <summary>
        /// Delete this instance and its associations and inheritances.
        /// </summary>
        public void DeleteRecursive()
        {
            var inheritances = new List<Inheritance>();
            inheritances.AddRange(this.InheritancesWhereSubtype);
            inheritances.AddRange(this.InheritancesWhereSupertype);

            var associations = this.AssociationTypesWhereObjectType;

            foreach (var association in associations)
            {
                association.RelationType.Delete();
            }

            this.Delete();

            foreach (var inheritance in inheritances)
            {
                inheritance.Delete();
            }
        }

        /// <summary>
        /// Finds the inheritance where this instance is the direct subtype.
        /// </summary>
        /// <param name="supertype">The supertype.</param>
        /// <returns>The inheritance.</returns>
        public Inheritance FindInheritanceWhereDirectSubtype(ObjectType supertype)
        {
            foreach (var inheritance in this.InheritancesWhereSubtype)
            {
                if (supertype.Equals(inheritance.Supertype))
                {
                    return inheritance;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified super type is a valid super type.
        /// </summary>
        /// <param name="supertype">The super type.</param>
        /// <returns>
        ///  <c>true</c> if the specified super type is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidSupertype(ObjectType supertype)
        {
            if (!this.IsCyclicInheritance(supertype))
            {
                return true;
            }

            return false;
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
        /// Removes the plural name.
        /// </summary>
        public override void RemovePluralName()
        {
            base.RemovePluralName();
            this.Domain.StaleObjectTypeDerivations();
        }

        /// <summary>
        /// Removes the singular name.
        /// </summary>
        public override void RemoveSingularName()
        {
            base.RemoveSingularName();
            this.Domain.StaleObjectTypeDerivations();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.RemovePluralName();
            this.RemoveSingularName();
            this.IsAbstract = false;
            this.IsInterface = false;
            this.IsUnit = false;
            this.RemoveUnitTag();
        }

        /// <summary>
        /// Sets the direct super interfaces.
        /// </summary>
        /// <param name="superInterfaces">The super interfaces.</param>
        public void SetDirectSuperinterfaces(ObjectType[] superInterfaces)
        {
            foreach (var superType in superInterfaces)
            {
                this.AddDirectSupertype(superType);
            }

            foreach (var inheritance in this.InheritancesWhereSubtype)
            {
                var superType = inheritance.Supertype;

                if (superType == null || 
                    (superType.IsInterface && Array.IndexOf(superInterfaces, superType) < 0))
                {
                    if (!inheritance.DomainWhereDeclaredInheritance.IsSuperDomain)
                    {
                        inheritance.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            if (this.ExistSingularName)
            {
                return this.SingularName;
            }

            if (this.ExistId)
            {
                return this.IdAsString;
            }

            return this.AllorsObjectId.ToString(CultureInfo.InvariantCulture);
        }
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>The new instance.</returns>
        internal static ObjectType Create(AllorsEmbeddedSession session)
        {
            var type = (ObjectType)session.Create(AllorsEmbeddedDomain.ObjectType);
            type.Reset();
            return type;
        }

        /// <summary>
        /// Ensures that relation type derivations are up to date.
        /// </summary>
        internal void EnsureRelationTypeDerivations()
        {
            this.Domain.EnsureRelationTypeDerivations();
        }

        /// <summary>
        /// Determines whether adding the specified super type will result in a cycle.
        /// </summary>
        /// <param name="superType">The super type.</param>
        /// <returns>
        /// <c>true</c> if adding the specified super type will result in a cycle; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsCyclicInheritance(ObjectType superType)
        {
            if (this.Equals(superType))
            {
                return true;
            }

            foreach (var directSub in this.ObjectTypesWhereDirectSupertype)
            {
                if (directSub.IsCyclicInheritance(superType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Purges the derivations.
        /// </summary>
        internal void PurgeDerivations()
        {
            this.RemoveDerivedAssociationTypes();
            this.RemoveDerivedCompositeRoleTypes();
            this.RemoveDerivedDirectSuperclass();
            this.RemoveDerivedDirectSuperinterfaces();
            this.RemoveDerivedExclusiveAssociationTypes();
            this.RemoveDerivedExclusiveConcreteLeafClass();
            this.RemoveDerivedExclusiveRoleTypes();
            this.RemoveDerivedExclusiveSuperinterfaces();
            this.RemoveDerivedRoleTypes();
            this.RemoveDerivedRootClasses();
            this.RemoveDerivedSubclasses();
            this.RemoveDerivedSubinterfaces();
            this.RemoveDerivedSuperclasses();
            this.RemoveDerivedSuperinterfaces();
            this.RemoveDerivedSupertypes();
            this.RemoveDerivedUnitRoleTypes();
        }

        /// <summary>
        /// Stales the relation type derivations.
        /// </summary>
        internal void StaleRelationTypeDerivations()
        {
            this.Domain.StaleRelationTypeDerivations();
        }

        /// <summary>
        /// Derive association types.
        /// </summary>
        /// <param name="associations">The associations.</param>
        internal void DeriveAssociationTypes(HashSet<AssociationType> associations)
        {
            associations.Clear();
            foreach (var role in this.RoleTypesWhereObjectType)
            {
                associations.Add(role.RelationTypeWhereRoleType.AssociationType);
            }

            foreach (var superType in this.Supertypes)
            {
                foreach (var role in superType.RoleTypesWhereObjectType)
                {
                    associations.Add(role.RelationTypeWhereRoleType.AssociationType);
                }
            }

            var associationArray = new AssociationType[associations.Count];
            associations.CopyTo(associationArray);

            this.DerivedAssociationTypes = associationArray;
        }

        /// <summary>
        /// Derive composite role types.
        /// </summary>
        /// <param name="roles">The roles.</param>
        internal void DeriveCompositeRoleTypes(HashSet<RoleType> roles)
        {
            roles.Clear();
            foreach (var role in this.DerivedRoleTypes)
            {
                // TODO: Test
                if (role.ExistObjectType && role.ObjectType.IsComposite)
                {
                    roles.Add(role);
                }
            }

            var roleArray = new RoleType[roles.Count];
            roles.CopyTo(roleArray);

            this.DerivedCompositeRoleTypes = roleArray;
        }

        /// <summary>
        /// Derive association ids cache.
        /// </summary>
        internal void DeriveAssociationIdsCache()
        {
            this.associationIdsCache = new Dictionary<Guid, object>();
            foreach (var containsAssociation in this.AssociationTypes)
            {
                this.associationIdsCache[containsAssociation.RelationTypeWhereAssociationType.Id] = null;
            }
        }

        /// <summary>
        /// Derive role ids cache.
        /// </summary>
        internal void DeriveRoleTypeIdsCache()
        {
            this.roleIdsCache = new Dictionary<Guid, object>();
            foreach (var containsRole in this.DerivedRoleTypes)
            {
                this.roleIdsCache[containsRole.RelationTypeWhereRoleType.Id] = null;
            }
        }

        /// <summary>
        /// Derive concrete classes cache.
        /// </summary>
        internal void DeriveConcreteClassesCache()
        {
            this.concreteClassesCache = new HashSet<ObjectType>(this.ConcreteClasses);
        }

        /// <summary>
        /// Derive direct superclass.
        /// </summary>
        internal void DeriveDirectSuperclass()
        {
            this.DerivedDirectSuperclass = null;
            foreach (var directSupertype in this.DerivedDirectSupertypes)
            {
                if (!directSupertype.IsInterface)
                {
                    this.DerivedDirectSuperclass = directSupertype;
                }
            }
        }

        /// <summary>
        /// Derive direct super interface.
        /// </summary>
        /// <param name="directSuperinterfaces">The direct super interfaces.</param>
        internal void DeriveDirectSuperinterface(HashSet<ObjectType> directSuperinterfaces)
        {
            directSuperinterfaces.Clear();
            foreach (var directSupertype in this.DerivedDirectSupertypes)
            {
                if (directSupertype.IsInterface)
                {
                    directSuperinterfaces.Add(directSupertype);
                }
            }

            var directSuperinterfaceArray = new ObjectType[directSuperinterfaces.Count];
            directSuperinterfaces.CopyTo(directSuperinterfaceArray);

            this.DerivedDirectSuperinterfaces = directSuperinterfaceArray;
        }

        /// <summary>
        /// Derive direct super type derivations.
        /// </summary>
        /// <param name="directSupertypes">The direct super types.</param>
        internal void DeriveDirectSupertypes(HashSet<ObjectType> directSupertypes)
        {
            directSupertypes.Clear();
            foreach (var inheritance in this.InheritancesWhereSubtype)
            {
                directSupertypes.Add(inheritance.Supertype);
            }

            var directSupertypeArray = new ObjectType[directSupertypes.Count];
            directSupertypes.CopyTo(directSupertypeArray);

            this.DerivedDirectSupertypes = directSupertypeArray;
        }

        /// <summary>
        /// Derive exclusive association types.
        /// </summary>
        /// <param name="exclusiveAssociationTypes">The exclusive association types.</param>
        internal void DeriveExclusiveAssociationTypes(HashSet<AssociationType> exclusiveAssociationTypes)
        {
            exclusiveAssociationTypes.Clear();
            foreach (var role in this.RoleTypesWhereObjectType)
            {
                exclusiveAssociationTypes.Add(role.RelationTypeWhereRoleType.AssociationType);
            }

            foreach (var superType in this.ExclusiveSuperinterfaces)
            {
                foreach (var role in superType.RoleTypesWhereObjectType)
                {
                    exclusiveAssociationTypes.Add(role.RelationTypeWhereRoleType.AssociationType);
                }
            }

            var exclusiveAssociationTypeArray = new AssociationType[exclusiveAssociationTypes.Count];
            exclusiveAssociationTypes.CopyTo(exclusiveAssociationTypeArray);

            this.DerivedExclusiveAssociationTypes = exclusiveAssociationTypeArray;
        }

        /// <summary>
        /// Derive exclusive concrete leaf classes.
        /// </summary>
        /// <param name="concreteLeafClasses">The concrete leaf classes.</param>
        internal void DeriveExclusiveConcreteLeafClass(HashSet<ObjectType> concreteLeafClasses)
        {
            concreteLeafClasses.Clear();

            this.DerivedExclusiveConcreteLeafClass = null;
            if (this.IsConcrete && !this.ExistSubclasses)
            {
                concreteLeafClasses.Add(this);
            }

            foreach (var rootClass in this.RootClasses)
            {
                foreach (var rootSubClass in rootClass.Subclasses)
                {
                    if (rootSubClass.IsConcrete && !rootSubClass.ExistSubclasses)
                    {
                        if (!concreteLeafClasses.Contains(rootSubClass))
                        {
                            concreteLeafClasses.Add(rootSubClass);
                        }
                    }
                }
            }

            if (concreteLeafClasses.Count == 1)
            {
                var concreteLeafClassArray = new ObjectType[concreteLeafClasses.Count];
                concreteLeafClasses.CopyTo(concreteLeafClassArray);

                this.DerivedExclusiveConcreteLeafClass = concreteLeafClassArray[0];
            }
        }

        /// <summary>
        /// Derive exclusive roles.
        /// </summary>
        /// <param name="exclusiveRoles">The exclusive roles.</param>
        internal void DeriveExclusiveRoleTypes(HashSet<RoleType> exclusiveRoles)
        {
            exclusiveRoles.Clear();
            foreach (var association in this.AssociationTypesWhereObjectType)
            {
                exclusiveRoles.Add(association.RelationTypeWhereAssociationType.RoleType);
            }

            foreach (var superType in this.ExclusiveSuperinterfaces)
            {
                foreach (var association in superType.AssociationTypesWhereObjectType)
                {
                    exclusiveRoles.Add(association.RelationTypeWhereAssociationType.RoleType);
                }
            }

            var exclusiveRoleArray = new RoleType[exclusiveRoles.Count];
            exclusiveRoles.CopyTo(exclusiveRoleArray);

            this.DerivedExclusiveRoleTypes = exclusiveRoleArray;
        }

        /// <summary>
        /// Derive exclusive super interfaces.
        /// </summary>
        /// <param name="superInterfaces">The super interfaces.</param>
        internal void DeriveExclusiveSuperinterfaces(HashSet<ObjectType> superInterfaces)
        {
            superInterfaces.Clear();
            foreach (var superType in this.DerivedSupertypes)
            {
                if (superType.IsInterface && !superType.IsImplementedByAnyOf(this.Superclasses))
                {
                    superInterfaces.Add(superType);
                }
            }

            var superInterfaceArray = new ObjectType[superInterfaces.Count];
            superInterfaces.CopyTo(superInterfaceArray);

            this.DerivedExclusiveSuperinterfaces = superInterfaceArray;
        }

        /// <summary>
        /// Derive method types.
        /// </summary>
        /// <param name="methodTypes">
        /// The method types.
        /// </param>
        internal void DeriveMethodTypes(HashSet<MethodType> methodTypes)
        {
            methodTypes.Clear();
            foreach (var methodType in this.MethodTypesWhereObjectType)
            {
                methodTypes.Add(methodType);
            }

            foreach (var superType in this.Supertypes)
            {
                foreach (var methodType in superType.MethodTypesWhereObjectType)
                {
                    methodTypes.Add(methodType);
                }
            }

            var methodTypeArray = new MethodType[methodTypes.Count];
            methodTypes.CopyTo(methodTypeArray);

            this.DerivedMethodTypes = methodTypeArray;
        }

        /// <summary>
        /// Derive role types.
        /// </summary>
        /// <param name="roleTypes">The role types.</param>
        internal void DeriveRoleTypes(HashSet<RoleType> roleTypes)
        {
            roleTypes.Clear();
            foreach (var association in this.AssociationTypesWhereObjectType)
            {
                roleTypes.Add(association.RelationTypeWhereAssociationType.RoleType);
            }

            foreach (var superType in this.Supertypes)
            {
                foreach (var association in superType.AssociationTypesWhereObjectType)
                {
                    roleTypes.Add(association.RelationTypeWhereAssociationType.RoleType);
                }
            }

            var roleTypeArray = new RoleType[roleTypes.Count];
            roleTypes.CopyTo(roleTypeArray);

            this.DerivedRoleTypes = roleTypeArray;
        }

        /// <summary>
        /// Derive root class for classes.
        /// </summary>
        internal void DeriveRootClassForClasses()
        {
            this.DerivedRootClasses = null;
            if (!this.IsInterface)
            {
                this.DeriveRootClassForClassRecursively(this);
            }
        }

        /// <summary>
        /// Derive root class for interfaces.
        /// </summary>
        /// <param name="rootClasses">The root classes.</param>
        internal void DeriveRootClassForInterfaces(HashSet<ObjectType> rootClasses)
        {
            // TODO: Extra Tests required.
            rootClasses.Clear();
            if (this.IsInterface)
            {
                foreach (var subClass in this.DerivedSubclasses)
                {
                    foreach (var rootClass in subClass.DerivedRootClasses)
                    {
                        if (!rootClasses.Contains(rootClass))
                        {
                            rootClasses.Add(rootClass);
                        }
                    }
                }
            }

            var rootClassArray = new ObjectType[rootClasses.Count];
            rootClasses.CopyTo(rootClassArray);

            this.DerivedRootClasses = rootClassArray;
        }

        /// <summary>
        /// Derive subclasses.
        /// </summary>
        /// <param name="subClasses">The sub classes.</param>
        internal void DeriveSubclasses(HashSet<ObjectType> subClasses)
        {
            subClasses.Clear();
            foreach (var subType in this.ObjectTypesWhereDerivedSupertype)
            {
                if (!subType.IsInterface)
                {
                    subClasses.Add(subType);
                }
            }

            var subClassArray = new ObjectType[subClasses.Count];
            subClasses.CopyTo(subClassArray);

            this.DerivedSubclasses = subClassArray;
        }

        /// <summary>
        /// Derive sub interfaces.
        /// </summary>
        /// <param name="subInterfaces">The sub interfaces.</param>
        internal void DeriveSubinterfaces(HashSet<ObjectType> subInterfaces)
        {
            subInterfaces.Clear();
            foreach (var subType in this.ObjectTypesWhereDerivedSupertype)
            {
                if (subType.IsInterface)
                {
                    subInterfaces.Add(subType);
                }
            }

            var subInterfaceArray = new ObjectType[subInterfaces.Count];
            subInterfaces.CopyTo(subInterfaceArray);

            this.DerivedSubinterfaces = subInterfaceArray;
        }

        /// <summary>
        /// Derive super classes.
        /// </summary>
        /// <param name="superClasses">The super classes.</param>
        internal void DeriveSuperclasses(HashSet<ObjectType> superClasses)
        {
            superClasses.Clear();
            foreach (var superTypes in this.DerivedSupertypes)
            {
                if (!superTypes.IsInterface)
                {
                    superClasses.Add(superTypes);
                }
            }

            var superClassArray = new ObjectType[superClasses.Count];
            superClasses.CopyTo(superClassArray);

            this.DerivedSuperclasses = superClassArray;
        }

        /// <summary>
        /// Derive super interface.
        /// </summary>
        /// <param name="superInterfaces">The super interfaces.</param>
        internal void DeriveSuperinterfaces(HashSet<ObjectType> superInterfaces)
        {
            superInterfaces.Clear();
            foreach (var superType in this.DerivedSupertypes)
            {
                if (superType.IsInterface)
                {
                    superInterfaces.Add(superType);
                }
            }

            var superInterfaceArray = new ObjectType[superInterfaces.Count];
            superInterfaces.CopyTo(superInterfaceArray);

            this.DerivedSuperinterfaces = superInterfaceArray;
        }

        /// <summary>
        /// Derive super types.
        /// </summary>
        /// <param name="superTypes">The super types.</param>
        internal void DeriveSupertypes(HashSet<ObjectType> superTypes)
        {
            superTypes.Clear();
            this.DeriveSupertypesRecursively(this, superTypes);

            var superTypeArray = new ObjectType[superTypes.Count];
            superTypes.CopyTo(superTypeArray);

            this.DerivedSupertypes = superTypeArray;
        }

        /// <summary>
        /// Derive unit role types.
        /// </summary>
        /// <param name="roles">The roles.</param>
        internal void DeriveUnitRoleTypes(HashSet<RoleType> roles)
        {
            roles.Clear();
            foreach (var role in this.DerivedRoleTypes)
            {
                // TODO: Test
                if (role.ExistObjectType && role.ObjectType.IsUnit)
                {
                    roles.Add(role);
                }
            }

            var roleArray = new RoleType[roles.Count];
            roles.CopyTo(roleArray);

            this.DerivedUnitRoleTypes = roleArray;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (this.ExistSingularName)
            {
                if (this.SingularName.Length < 2)
                {
                    var message = this.ValidationName + " should have a singular name with at least 2 characters";
                    validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.ObjectTypeSingularName);
                }
                else
                {
                    if (!char.IsLetter(this.SingularName[0]))
                    {
                        var message = this.ValidationName + "'s singular name should start with an alfabetical character";
                        validationLog.AddError(message, this, ValidationKind.Format, AllorsEmbeddedDomain.ObjectTypeSingularName);
                    }

                    for (var i = 1; i < this.SingularName.Length; i++)
                    {
                        if (!char.IsLetter(this.SingularName[i]) && !char.IsDigit(this.SingularName[i]))
                        {
                            var message = this.ValidationName + "'s singular name should only contain alfanumerical characters";
                            validationLog.AddError(message, this, ValidationKind.Format, AllorsEmbeddedDomain.ObjectTypeSingularName);
                            break;
                        }
                    }
                }

                if (validationLog.ExistObjectTypeName(this.SingularName))
                {
                    var message = "The singular name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.ObjectTypeSingularName);
                }
                else
                {
                    validationLog.AddObjectTypeName(this.SingularName);
                }
            }
            else
            {
                validationLog.AddError(this.ValidationName + " has no singular name", this, ValidationKind.Required, AllorsEmbeddedDomain.ObjectTypeSingularName);
            }

            if (this.ExistPluralName)
            {
                if (this.PluralName.Length < 2)
                {
                    var message = this.ValidationName + " should have a plural name with at least 2 characters";
                    validationLog.AddError(message, this, ValidationKind.MinimumLength, AllorsEmbeddedDomain.ObjectTypePluralName);
                }
                else
                {
                    if (!char.IsLetter(this.PluralName[0]))
                    {
                        var message = this.ValidationName + "'s plural name should start with an alfabetical character";
                        validationLog.AddError(message, this, ValidationKind.Format, AllorsEmbeddedDomain.ObjectTypePluralName);
                    }

                    for (var i = 1; i < this.PluralName.Length; i++)
                    {
                        if (!char.IsLetter(this.PluralName[i]) && !char.IsDigit(this.PluralName[i]))
                        {
                            var message = this.ValidationName + "'s plural name should only contain alfanumerical characters";
                            validationLog.AddError(message, this, ValidationKind.Format, AllorsEmbeddedDomain.ObjectTypePluralName);
                            break;
                        }
                    }
                }

                if (validationLog.ExistObjectTypeName(this.PluralName))
                {
                    var message = "The plural name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.ObjectTypePluralName);
                }
                else
                {
                    validationLog.AddObjectTypeName(this.PluralName);
                }
            }
            else
            {
                validationLog.AddError(this.ValidationName + " has no plural name", this, ValidationKind.Required, AllorsEmbeddedDomain.ObjectTypePluralName);
            }
        }

        /// <summary>
        /// Ensures that method type derivations are up to date.
        /// </summary>
        private void EnsureMethodTypeDerivations()
        {
            this.Domain.EnsureMethodTypeDerivations();
        }

        /// <summary>
        /// Ensures that object type derivations are up to date.
        /// </summary>
        private void EnsureObjectTypeDerivations()
        {
            this.Domain.EnsureObjectTypeDerivations();
        }

        /// <summary>
        /// Gets the unit roles.
        /// </summary>
        /// <param name="unitTypeTags">The unit type tag.</param>
        /// <returns>The roles.</returns>
        private RoleType[] GetUnitRoleTypes(UnitTypeTags unitTypeTags)
        {
            var roles = new List<RoleType>(this.UnitRoleTypes.Length);
            foreach (var role in this.UnitRoleTypes)
            {
                if (role.ObjectType.UnitTag == (int)unitTypeTags)
                {
                    roles.Add(role);
                }
            }

            return roles.ToArray();
        }

        /// <summary>
        /// Determines whether this instance is implemented by any of the specified object types.
        /// </summary>
        /// <param name="objectTypes">The object types.</param>
        /// <returns>
        ///  <c>true</c> if this instance is implemented by any of the specified object types; otherwise, <c>false</c>.
        /// </returns>
        private bool IsImplementedByAnyOf(IEnumerable<ObjectType> objectTypes)
        {
            foreach (var domainType in objectTypes)
            {
                foreach (var superType in domainType.DerivedSupertypes)
                {
                    if (this.Equals(superType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Derive root class for class recursively.
        /// </summary>
        /// <param name="type">The type .</param>
        private void DeriveRootClassForClassRecursively(ObjectType type)
        {
            if (this.ExistDerivedDirectSuperclass && !Equals(this.DerivedDirectSuperclass, type))
            {
                this.DerivedDirectSuperclass.DeriveRootClassForClassRecursively(type);
            }
            else
            {
                type.AddDerivedRootClass(this);
            }
        }

        /// <summary>
        /// Derive super types recursively.
        /// </summary>
        /// <param name="type">The type .</param>
        /// <param name="superTypes">The super types.</param>
        private void DeriveSupertypesRecursively(ObjectType type, HashSet<ObjectType> superTypes)
        {
            foreach (var directSupertype in this.DerivedDirectSupertypes)
            {
                if (!Equals(directSupertype, type))
                {
                    superTypes.Add(directSupertype);
                    directSupertype.DeriveSupertypesRecursively(type, superTypes);
                }
            }
        }
    }
}