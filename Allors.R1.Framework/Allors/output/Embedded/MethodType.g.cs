namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalMethodType : AllorsInternalOperandType, AllorsInternal
	{
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

		void AllorsRemoveMethodTypeObjectType();


		global::System.String Name
		{
			get;
			set;
		}

		void RemoveName();

		bool ExistName
		{
			get;
		}



		global::Allors.R1.Meta.Domain DomainWhereDeclaredMethodType
		{
			get;
		}

		bool ExistDomainWhereDeclaredMethodType
		{
			get;
		}


		void AllorsRoleSyncSetDeclaredMethodTypeDomain(global::Allors.R1.Meta.Domain association );


		void AllorsRoleReleaseDeclaredMethodTypeDomain();


		global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedMethodType
		{
			get;
		}

		bool ExistObjectTypesWhereDerivedMethodType
		{
			get;
		}

		void AllorsRoleSyncAddDerivedMethodTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleSyncRemoveDerivedMethodTypeObjectType(global::Allors.R1.Meta.ObjectType association );

		void AllorsRoleReleaseDerivedMethodTypeObjectType();


		global::Allors.R1.Meta.Domain[] DomainsWhereDerivedMethodType
		{
			get;
		}

		bool ExistDomainsWhereDerivedMethodType
		{
			get;
		}

		void AllorsRoleSyncAddDerivedMethodTypeDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleSyncRemoveDerivedMethodTypeDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleReleaseDerivedMethodTypeDomain();


	}

	public interface AllorsInterfaceMethodType :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassMethodType :  global::Allors.R1.Meta.OperandType,  AllorsInternalMethodType , AllorsEmbeddedObject
	{
		protected global::Allors.R1.Meta.ObjectType _MethodTypeObjectType;


		protected System.Object _MethodTypeName;


		protected global::Allors.R1.Meta.Domain _DeclaredMethodTypeDomain;


		protected global::Allors.R1.Meta.ObjectType[] _DerivedMethodTypeObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _DerivedMethodTypeDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;




		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassMethodType"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassMethodType(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

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
				return new System.Guid("edf4fdb9-9fc9-4914-bd67-c781f9199f98");
			}
		}

		public override void Delete()
		{
			AllorsAssert();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRemoveMethodTypeObjectType();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRoleReleaseDeclaredMethodTypeDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRoleReleaseDerivedMethodTypeObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRoleReleaseDerivedMethodTypeDomain();


			session.Delete(this);
			isDeleted = true;
		}

		object AllorsInternal.GetRole(AllorsEmbeddedRelationType relation)
		{
			AllorsAssert();
			switch(relation.Tag)
			{
				case AllorsRelationTags.MethodTypeObjectType:
					return _MethodTypeObjectType;
				case AllorsRelationTags.MethodTypeName:
					return _MethodTypeName;
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
				case AllorsRelationTags.MethodTypeObjectType:
					RoleSetMethodTypeObjectType((global::Allors.R1.Meta.ObjectType)role);
					break;
				case AllorsRelationTags.MethodTypeName:
					RoleSetMethodTypeName((global::System.String)role);
					break;
				case AllorsRelationTags.MetaObjectId:
					RoleSetMetaObjectId((global::System.Guid)role);
					break;

		default:
				throw new System.ArgumentException("Illegal relation " + relation.Name);
			}
		}


		public virtual global::Allors.R1.Meta.ObjectType ObjectType
		{
			get
			{
				AllorsAssert();
				return _MethodTypeObjectType;
			}

			set
			{
				AllorsAssert(value);
				RoleSetMethodTypeObjectType(value);
			}
		}

		protected void RoleSetMethodTypeObjectType(global::Allors.R1.Meta.ObjectType value)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRemoveMethodTypeObjectType();
			if( value != null ) 
			{
				_MethodTypeObjectType = value;
				_MethodTypeObjectType = value;
				// role side
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)_MethodTypeObjectType).AllorsRoleSyncAddObjectTypeMethodType((global::Allors.R1.Meta.MethodType) this ); 
			}
		}

		public virtual void RemoveObjectType()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)this).AllorsRemoveMethodTypeObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRemoveMethodTypeObjectType()
		{
			AllorsAssert();
			if( _MethodTypeObjectType != null) 
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)_MethodTypeObjectType).AllorsRoleSyncRemoveObjectTypeMethodType( (global::Allors.R1.Meta.MethodType) this ); 
				_MethodTypeObjectType = null;
				_MethodTypeObjectType = null;
			}
		}

		public virtual bool ExistObjectType
		{
			get
			{
				return _MethodTypeObjectType != null;
			}
		}

		public virtual global::System.String Name
		{
			get
			{ 
				AllorsAssert();
				return (global::System.String)_MethodTypeName;
			}

			set
			{
				AllorsAssert();
				RoleSetMethodTypeName(value);
			}
		}

		protected void RoleSetMethodTypeName(global::System.String role)
		{
			_MethodTypeName = role;
		}

		public virtual bool ExistName
		{
			get
			{
				return _MethodTypeName != null;
			}
		}

		public virtual void RemoveName()
		{
			_MethodTypeName = null;
		}


		public virtual global::Allors.R1.Meta.Domain DomainWhereDeclaredMethodType
		{
			get
			{
				AllorsAssert();
				return _DeclaredMethodTypeDomain;
			}
		}

		public virtual bool ExistDomainWhereDeclaredMethodType
		{
			get
			{
				return _DeclaredMethodTypeDomain != null;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleSyncSetDeclaredMethodTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DeclaredMethodTypeDomain = association;
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleReleaseDeclaredMethodTypeDomain()
		{
			if( _DeclaredMethodTypeDomain != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)_DeclaredMethodTypeDomain).AllorsRemoveDomainDeclaredMethodType( (global::Allors.R1.Meta.MethodType) this);
			}
		}


		public virtual global::Allors.R1.Meta.ObjectType[] ObjectTypesWhereDerivedMethodType
		{
			get
			{
				AllorsAssert();
				return _DerivedMethodTypeObjectType;
			}
		}

		public virtual bool ExistObjectTypesWhereDerivedMethodType
		{
			get
			{
				return _DerivedMethodTypeObjectType.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleSyncAddDerivedMethodTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedMethodTypeObjectType, association ) ) 
			{
				_DerivedMethodTypeObjectType = (global::Allors.R1.Meta.ObjectType[])AllorsEmbeddedArrays.Add(_DerivedMethodTypeObjectType,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleSyncRemoveDerivedMethodTypeObjectType(global::Allors.R1.Meta.ObjectType association)
		{
			AllorsAssert();
			_DerivedMethodTypeObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove(_DerivedMethodTypeObjectType,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleReleaseDerivedMethodTypeObjectType()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.ObjectType association in _DerivedMethodTypeObjectType )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)association).AllorsRemoveObjectTypeDerivedMethodType((global::Allors.R1.Meta.MethodType) this);
			}
		}

		public virtual global::Allors.R1.Meta.Domain[] DomainsWhereDerivedMethodType
		{
			get
			{
				AllorsAssert();
				return _DerivedMethodTypeDomain;
			}
		}

		public virtual bool ExistDomainsWhereDerivedMethodType
		{
			get
			{
				return _DerivedMethodTypeDomain.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleSyncAddDerivedMethodTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedMethodTypeDomain, association ) ) 
			{
				_DerivedMethodTypeDomain = (global::Allors.R1.Meta.Domain[])AllorsEmbeddedArrays.Add(_DerivedMethodTypeDomain,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleSyncRemoveDerivedMethodTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DerivedMethodTypeDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove(_DerivedMethodTypeDomain,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType.AllorsRoleReleaseDerivedMethodTypeDomain()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.Domain association in _DerivedMethodTypeDomain )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)association).AllorsRemoveDomainDerivedMethodType((global::Allors.R1.Meta.MethodType) this);
			}
		}

}
}