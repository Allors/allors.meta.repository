namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalAssociationType : AllorsInternalPropertyType, AllorsInternal
	{
		global::System.String AssignedPluralName
		{
			get;
			set;
		}

		void RemoveAssignedPluralName();

		bool ExistAssignedPluralName
		{
			get;
		}


		global::Allors.R1.Meta.ObjectType ObjectType
		{
			get;
			set;
		}

		void RemoveObjectType();

		bool ExistObjectType
		{
			get;
		}

		void AllorsRemoveAssociationTypeObjectType();


		global::System.Boolean IsMany
		{
			get;
			set;
		}

		void RemoveIsMany();

		bool ExistIsMany
		{
			get;
		}


		global::Allors.R1.Meta.ObjectType[] DerivedRootObjectTypes
		{
			get;
			set;
		}

		void AddDerivedRootObjectType( global::Allors.R1.Meta.ObjectType addRole );

		void RemoveDerivedRootObjectType( global::Allors.R1.Meta.ObjectType removeRole );

		void RemoveDerivedRootObjectTypes();

		bool ExistDerivedRootObjectTypes
		{
			get;
		}
		void AllorsRemoveAssociationTypeDerivedRootObjectType();

		void AllorsRemoveAssociationTypeDerivedRootObjectType( global::Allors.R1.Meta.ObjectType role );


		global::System.String DerivedRootName
		{
			get;
			set;
		}

		void RemoveDerivedRootName();

		bool ExistDerivedRootName
		{
			get;
		}


		global::System.String AssignedSingularName
		{
			get;
			set;
		}

		void RemoveAssignedSingularName();

		bool ExistAssignedSingularName
		{
			get;
		}


		global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedExclusiveAssociationType
		{
			get;
		}

		bool ExistObjectTypesWhereDerivedExclusiveAssociationType
		{
			get;
		}

		void AllorsRoleSyncAddDerivedExclusiveAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleSyncRemoveDerivedExclusiveAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleReleaseDerivedExclusiveAssociationTypeObjectType();


		global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedAssociationType
		{
			get;
		}

		bool ExistObjectTypesWhereDerivedAssociationType
		{
			get;
		}

		void AllorsRoleSyncAddDerivedAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleSyncRemoveDerivedAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleReleaseDerivedAssociationTypeObjectType();



		global::Allors.R1.Meta.RelationType RelationTypeWhereAssociationType
		{
			get;
		}

		bool ExistRelationTypeWhereAssociationType
		{
			get;
		}


		void AllorsRoleSyncSetAssociationTypeRelationType(global::Allors.R1.Meta.RelationType association );


		void AllorsRoleReleaseAssociationTypeRelationType();


	}

	public interface AllorsInterfaceAssociationType :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassAssociationType :  global::Allors.R1.Meta.PropertyType,  AllorsInternalAssociationType , AllorsEmbeddedObject
	{
		protected System.Object _AssociationTypeAssignedPluralName;


		protected global::Allors.R1.Meta.ObjectType _AssociationTypeObjectType;


		protected System.Object _AssociationTypeIsMany;


		protected global::Allors.R1.Meta.ObjectType[] _AssociationTypeDerivedRootObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected System.Object _AssociationTypeDerivedRootName;


		protected System.Object _AssociationTypeAssignedSingularName;


		protected global::Allors.R1.Meta.ObjectType[] _DerivedExclusiveAssociationTypeObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.ObjectType[] _DerivedAssociationTypeObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.RelationType _AssociationTypeRelationType;




		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassAssociationType"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassAssociationType(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override System.String ToString() {
			return this.GetType().Name + " (" + this.AllorsObjectId + ")";
		}

		/// <summary>
		/// Gets the Object Type Id.
		/// </summary>
		/// <value>The Object Type Id.</value>
		public override System.Guid AllorsObjectTypeId
		{ 
			get
			{
				//TODO: make static
				return new System.Guid("51df648e-4870-403a-8b2e-50f6e63d4749");
			}
		}

		public override void Delete()
		{
			AllorsAssert();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeDerivedRootObjectType();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRoleReleaseDerivedExclusiveAssociationTypeObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRoleReleaseDerivedAssociationTypeObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRoleReleaseAssociationTypeRelationType();


			session.Delete(this);
			isDeleted = true;
		}

		object AllorsInternal.GetRole(AllorsEmbeddedRelationType relation)
		{
			AllorsAssert();
			switch(relation.Tag)
			{
				case AllorsRelationTags.AssociationTypeAssignedPluralName:
					return _AssociationTypeAssignedPluralName;
				case AllorsRelationTags.AssociationTypeObjectType:
					return _AssociationTypeObjectType;
				case AllorsRelationTags.AssociationTypeIsMany:
					return _AssociationTypeIsMany;
				case AllorsRelationTags.AssociationTypeDerivedRootObjectType:
					return _AssociationTypeDerivedRootObjectType;
				case AllorsRelationTags.AssociationTypeDerivedRootName:
					return _AssociationTypeDerivedRootName;
				case AllorsRelationTags.AssociationTypeAssignedSingularName:
					return _AssociationTypeAssignedSingularName;
				case AllorsRelationTags.MetaObjectId:
					return _MetaObjectId;

		default:
				throw new System.ArgumentException("Illegal relation " + relation.Name);
			}
		}

		void AllorsInternal.SetRole(AllorsEmbeddedRelationType relation, object role)
		{
			switch(relation.Tag)
			{
				case AllorsRelationTags.AssociationTypeAssignedPluralName:
					RoleSetAssociationTypeAssignedPluralName((global::System.String)role);
					break;
				case AllorsRelationTags.AssociationTypeObjectType:
					RoleSetAssociationTypeObjectType((global::Allors.R1.Meta.ObjectType)role);
					break;
				case AllorsRelationTags.AssociationTypeIsMany:
					RoleSetAssociationTypeIsMany((global::System.Boolean)role);
					break;
				case AllorsRelationTags.AssociationTypeDerivedRootObjectType:
					RoleSetAssociationTypeDerivedRootObjectType((global::Allors.R1.Meta.ObjectType[])role);
					break;
				case AllorsRelationTags.AssociationTypeDerivedRootName:
					RoleSetAssociationTypeDerivedRootName((global::System.String)role);
					break;
				case AllorsRelationTags.AssociationTypeAssignedSingularName:
					RoleSetAssociationTypeAssignedSingularName((global::System.String)role);
					break;
				case AllorsRelationTags.MetaObjectId:
					RoleSetMetaObjectId((global::System.Guid)role);
					break;

		default:
				throw new System.ArgumentException("Illegal relation " + relation.Name);
			}
		}


		public virtual global::System.String AssignedPluralName
		{
			get
			{ 
				AllorsAssert();
				return (global::System.String)_AssociationTypeAssignedPluralName;
			}

			set
			{
				AllorsAssert();
				RoleSetAssociationTypeAssignedPluralName(value);
			}
		}

		protected void RoleSetAssociationTypeAssignedPluralName(global::System.String role)
		{
			_AssociationTypeAssignedPluralName = role;
		}

		public virtual bool ExistAssignedPluralName
		{
			get
			{
				return _AssociationTypeAssignedPluralName != null;
			}
		}

		public virtual void RemoveAssignedPluralName()
		{
			_AssociationTypeAssignedPluralName = null;
		}

		public virtual global::Allors.R1.Meta.ObjectType ObjectType
		{
			get
			{
				AllorsAssert();
				return _AssociationTypeObjectType;
			}

			set
			{
				AllorsAssert(value);
				RoleSetAssociationTypeObjectType(value);
			}
		}

		protected void RoleSetAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType value)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeObjectType();
			if( value != null ) 
			{
				_AssociationTypeObjectType = value;
				_AssociationTypeObjectType = value;
				// role side
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)_AssociationTypeObjectType).AllorsRoleSyncAddObjectTypeAssociationType((global::Allors.R1.Meta.AssociationType) this ); 
			}
		}

		public virtual void RemoveObjectType()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRemoveAssociationTypeObjectType()
		{
			AllorsAssert();
			if( _AssociationTypeObjectType != null) 
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)_AssociationTypeObjectType).AllorsRoleSyncRemoveObjectTypeAssociationType( (global::Allors.R1.Meta.AssociationType) this ); 
				_AssociationTypeObjectType = null;
				_AssociationTypeObjectType = null;
			}
		}

		public virtual bool ExistObjectType
		{
			get
			{
				return _AssociationTypeObjectType != null;
			}
		}

		public virtual global::System.Boolean IsMany
		{
			get
			{ 
				AllorsAssert();
				return (global::System.Boolean)_AssociationTypeIsMany;
			}

			set
			{
				AllorsAssert();
				RoleSetAssociationTypeIsMany(value);
			}
		}

		protected void RoleSetAssociationTypeIsMany(global::System.Boolean role)
		{
			_AssociationTypeIsMany = role;
		}

		public virtual bool ExistIsMany
		{
			get
			{
				return _AssociationTypeIsMany != null;
			}
		}

		public virtual void RemoveIsMany()
		{
			_AssociationTypeIsMany = null;
		}

		public virtual global::Allors.R1.Meta.ObjectType[] DerivedRootObjectTypes
		{
			get
			{ 
				AllorsAssert();
				return _AssociationTypeDerivedRootObjectType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetAssociationTypeDerivedRootObjectType(value);
			}
		}

		protected void RoleSetAssociationTypeDerivedRootObjectType(global::Allors.R1.Meta.ObjectType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeDerivedRootObjectType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in roles )
				{
					RoleAddDerivedRootObjectType(role);
				}
			}
		}

		public virtual void AddDerivedRootObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			RoleAddDerivedRootObjectType( addRole );
		}

		void RoleAddDerivedRootObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _AssociationTypeDerivedRootObjectType, addRole ) ) 
				{
					// association side
					_AssociationTypeDerivedRootObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Add( _AssociationTypeDerivedRootObjectType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleSyncAddDerivedRootObjectTypeAssociationType( (global::Allors.R1.Meta.AssociationType) this ); 
				}
			}
		}

		public virtual void RemoveDerivedRootObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeDerivedRootObjectType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRemoveAssociationTypeDerivedRootObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _AssociationTypeDerivedRootObjectType, removeRole ) ) 
				{
					_AssociationTypeDerivedRootObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove( _AssociationTypeDerivedRootObjectType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)removeRole).AllorsRoleSyncRemoveDerivedRootObjectTypeAssociationType( (global::Allors.R1.Meta.AssociationType) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedRootObjectTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)this).AllorsRemoveAssociationTypeDerivedRootObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRemoveAssociationTypeDerivedRootObjectType()
		{
			AllorsAssert();

			if( _AssociationTypeDerivedRootObjectType!=null )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in _AssociationTypeDerivedRootObjectType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)role).AllorsRoleSyncRemoveDerivedRootObjectTypeAssociationType( (global::Allors.R1.Meta.AssociationType) this ); 
				}
			}
			_AssociationTypeDerivedRootObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;
		}

		public virtual bool ExistDerivedRootObjectTypes
		{
			get
			{
				return _AssociationTypeDerivedRootObjectType.Length > 0;
			}
		}


		public virtual global::System.String DerivedRootName
		{
			get
			{ 
				AllorsAssert();
				return (global::System.String)_AssociationTypeDerivedRootName;
			}

			set
			{
				AllorsAssert();
				RoleSetAssociationTypeDerivedRootName(value);
			}
		}

		protected void RoleSetAssociationTypeDerivedRootName(global::System.String role)
		{
			_AssociationTypeDerivedRootName = role;
		}

		public virtual bool ExistDerivedRootName
		{
			get
			{
				return _AssociationTypeDerivedRootName != null;
			}
		}

		public virtual void RemoveDerivedRootName()
		{
			_AssociationTypeDerivedRootName = null;
		}

		public virtual global::System.String AssignedSingularName
		{
			get
			{ 
				AllorsAssert();
				return (global::System.String)_AssociationTypeAssignedSingularName;
			}

			set
			{
				AllorsAssert();
				RoleSetAssociationTypeAssignedSingularName(value);
			}
		}

		protected void RoleSetAssociationTypeAssignedSingularName(global::System.String role)
		{
			_AssociationTypeAssignedSingularName = role;
		}

		public virtual bool ExistAssignedSingularName
		{
			get
			{
				return _AssociationTypeAssignedSingularName != null;
			}
		}

		public virtual void RemoveAssignedSingularName()
		{
			_AssociationTypeAssignedSingularName = null;
		}

		public virtual global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedExclusiveAssociationType
		{
			get
			{
				AllorsAssert();
				return _DerivedExclusiveAssociationTypeObjectType;
			}
		}

		public virtual bool ExistObjectTypesWhereDerivedExclusiveAssociationType
		{
			get
			{
				return _DerivedExclusiveAssociationTypeObjectType.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleSyncAddDerivedExclusiveAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedExclusiveAssociationTypeObjectType, association ) ) 
			{
				_DerivedExclusiveAssociationTypeObjectType = (global::Allors.R1.Meta.ObjectType[])AllorsEmbeddedArrays.Add(_DerivedExclusiveAssociationTypeObjectType,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleSyncRemoveDerivedExclusiveAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			_DerivedExclusiveAssociationTypeObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove(_DerivedExclusiveAssociationTypeObjectType,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleReleaseDerivedExclusiveAssociationTypeObjectType()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.ObjectType association in _DerivedExclusiveAssociationTypeObjectType )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)association).AllorsRemoveObjectTypeDerivedExclusiveAssociationType((global::Allors.R1.Meta.AssociationType) this);
			}
		}

		public virtual global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedAssociationType
		{
			get
			{
				AllorsAssert();
				return _DerivedAssociationTypeObjectType;
			}
		}

		public virtual bool ExistObjectTypesWhereDerivedAssociationType
		{
			get
			{
				return _DerivedAssociationTypeObjectType.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleSyncAddDerivedAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedAssociationTypeObjectType, association ) ) 
			{
				_DerivedAssociationTypeObjectType = (global::Allors.R1.Meta.ObjectType[])AllorsEmbeddedArrays.Add(_DerivedAssociationTypeObjectType,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleSyncRemoveDerivedAssociationTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			_DerivedAssociationTypeObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove(_DerivedAssociationTypeObjectType,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleReleaseDerivedAssociationTypeObjectType()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.ObjectType association in _DerivedAssociationTypeObjectType )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)association).AllorsRemoveObjectTypeDerivedAssociationType((global::Allors.R1.Meta.AssociationType) this);
			}
		}


		public virtual global::Allors.R1.Meta.RelationType RelationTypeWhereAssociationType
		{
			get
			{
				AllorsAssert();
				return _AssociationTypeRelationType;
			}
		}

		public virtual bool ExistRelationTypeWhereAssociationType
		{
			get
			{
				return _AssociationTypeRelationType != null;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleSyncSetAssociationTypeRelationType(global::Allors.R1.Meta.RelationType association)
		{
			AllorsAssert();
			_AssociationTypeRelationType = association;
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType.AllorsRoleReleaseAssociationTypeRelationType()
		{
			if( _AssociationTypeRelationType != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)_AssociationTypeRelationType).AllorsRemoveRelationTypeAssociationType();
			}
		}


}
}