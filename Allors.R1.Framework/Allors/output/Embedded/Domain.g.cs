namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalDomain : AllorsInternalMetaObject, AllorsInternal
	{
		global::Allors.R1.Meta.ObjectType[] DeclaredObjectTypes
		{
			get;
			set;
		}

		void AddDeclaredObjectType( global::Allors.R1.Meta.ObjectType addRole );

		void RemoveDeclaredObjectType( global::Allors.R1.Meta.ObjectType removeRole );

		void RemoveDeclaredObjectTypes();

		bool ExistDeclaredObjectTypes
		{
			get;
		}
		void AllorsRemoveDomainDeclaredObjectType();

		void AllorsRemoveDomainDeclaredObjectType( global::Allors.R1.Meta.ObjectType role );


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


		global::Allors.R1.Meta.MethodType[] DeclaredMethodTypes
		{
			get;
			set;
		}

		void AddDeclaredMethodType( global::Allors.R1.Meta.MethodType addRole );

		void RemoveDeclaredMethodType( global::Allors.R1.Meta.MethodType removeRole );

		void RemoveDeclaredMethodTypes();

		bool ExistDeclaredMethodTypes
		{
			get;
		}
		void AllorsRemoveDomainDeclaredMethodType();

		void AllorsRemoveDomainDeclaredMethodType( global::Allors.R1.Meta.MethodType role );


		global::Allors.R1.Meta.Domain[] DerivedSuperDomains
		{
			get;
			set;
		}

		void AddDerivedSuperDomain( global::Allors.R1.Meta.Domain addRole );

		void RemoveDerivedSuperDomain( global::Allors.R1.Meta.Domain removeRole );

		void RemoveDerivedSuperDomains();

		bool ExistDerivedSuperDomains
		{
			get;
		}
		void AllorsRemoveDomainDerivedSuperDomain();

		void AllorsRemoveDomainDerivedSuperDomain( global::Allors.R1.Meta.Domain role );


		global::Allors.R1.Meta.Inheritance[] DeclaredInheritances
		{
			get;
			set;
		}

		void AddDeclaredInheritance( global::Allors.R1.Meta.Inheritance addRole );

		void RemoveDeclaredInheritance( global::Allors.R1.Meta.Inheritance removeRole );

		void RemoveDeclaredInheritances();

		bool ExistDeclaredInheritances
		{
			get;
		}
		void AllorsRemoveDomainDeclaredInheritance();

		void AllorsRemoveDomainDeclaredInheritance( global::Allors.R1.Meta.Inheritance role );


		global::Allors.R1.Meta.Domain[] DirectSuperDomains
		{
			get;
			set;
		}

		void AddDirectSuperDomain( global::Allors.R1.Meta.Domain addRole );

		void RemoveDirectSuperDomain( global::Allors.R1.Meta.Domain removeRole );

		void RemoveDirectSuperDomains();

		bool ExistDirectSuperDomains
		{
			get;
		}
		void AllorsRemoveDomainDirectSuperDomain();

		void AllorsRemoveDomainDirectSuperDomain( global::Allors.R1.Meta.Domain role );


		global::Allors.R1.Meta.Domain UnitDomain
		{
			get;
			set;
		}

		void RemoveUnitDomain();

		bool ExistUnitDomain
		{
			get;
		}

		void AllorsRemoveDomainUnitDomain();


		global::Allors.R1.Meta.ObjectType[] DerivedUnitObjectTypes
		{
			get;
			set;
		}

		void AddDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType addRole );

		void RemoveDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType removeRole );

		void RemoveDerivedUnitObjectTypes();

		bool ExistDerivedUnitObjectTypes
		{
			get;
		}
		void AllorsRemoveDomainDerivedUnitObjectType();

		void AllorsRemoveDomainDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType role );


		global::Allors.R1.Meta.ObjectType[] DerivedCompositeObjectTypes
		{
			get;
			set;
		}

		void AddDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType addRole );

		void RemoveDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType removeRole );

		void RemoveDerivedCompositeObjectTypes();

		bool ExistDerivedCompositeObjectTypes
		{
			get;
		}
		void AllorsRemoveDomainDerivedCompositeObjectType();

		void AllorsRemoveDomainDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType role );


		global::Allors.R1.Meta.RelationType[] DeclaredRelationTypes
		{
			get;
			set;
		}

		void AddDeclaredRelationType( global::Allors.R1.Meta.RelationType addRole );

		void RemoveDeclaredRelationType( global::Allors.R1.Meta.RelationType removeRole );

		void RemoveDeclaredRelationTypes();

		bool ExistDeclaredRelationTypes
		{
			get;
		}
		void AllorsRemoveDomainDeclaredRelationType();

		void AllorsRemoveDomainDeclaredRelationType( global::Allors.R1.Meta.RelationType role );


		global::Allors.R1.Meta.RelationType[] DerivedRelationTypes
		{
			get;
			set;
		}

		void AddDerivedRelationType( global::Allors.R1.Meta.RelationType addRole );

		void RemoveDerivedRelationType( global::Allors.R1.Meta.RelationType removeRole );

		void RemoveDerivedRelationTypes();

		bool ExistDerivedRelationTypes
		{
			get;
		}
		void AllorsRemoveDomainDerivedRelationType();

		void AllorsRemoveDomainDerivedRelationType( global::Allors.R1.Meta.RelationType role );


		global::Allors.R1.Meta.MethodType[] DerivedMethodTypes
		{
			get;
			set;
		}

		void AddDerivedMethodType( global::Allors.R1.Meta.MethodType addRole );

		void RemoveDerivedMethodType( global::Allors.R1.Meta.MethodType removeRole );

		void RemoveDerivedMethodTypes();

		bool ExistDerivedMethodTypes
		{
			get;
		}
		void AllorsRemoveDomainDerivedMethodType();

		void AllorsRemoveDomainDerivedMethodType( global::Allors.R1.Meta.MethodType role );


		global::Allors.R1.Meta.Inheritance[] DerivedInheritances
		{
			get;
			set;
		}

		void AddDerivedInheritance( global::Allors.R1.Meta.Inheritance addRole );

		void RemoveDerivedInheritance( global::Allors.R1.Meta.Inheritance removeRole );

		void RemoveDerivedInheritances();

		bool ExistDerivedInheritances
		{
			get;
		}
		void AllorsRemoveDomainDerivedInheritance();

		void AllorsRemoveDomainDerivedInheritance( global::Allors.R1.Meta.Inheritance role );


		global::Allors.R1.Meta.ObjectType[] DerivedObjectTypes
		{
			get;
			set;
		}

		void AddDerivedObjectType( global::Allors.R1.Meta.ObjectType addRole );

		void RemoveDerivedObjectType( global::Allors.R1.Meta.ObjectType removeRole );

		void RemoveDerivedObjectTypes();

		bool ExistDerivedObjectTypes
		{
			get;
		}
		void AllorsRemoveDomainDerivedObjectType();

		void AllorsRemoveDomainDerivedObjectType( global::Allors.R1.Meta.ObjectType role );


		global::Allors.R1.Meta.Domain[] DomainsWhereDerivedSuperDomain
		{
			get;
		}

		bool ExistDomainsWhereDerivedSuperDomain
		{
			get;
		}

		void AllorsRoleSyncAddDerivedSuperDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleSyncRemoveDerivedSuperDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleReleaseDerivedSuperDomainDomain();


		global::Allors.R1.Meta.Domain[] DomainsWhereDirectSuperDomain
		{
			get;
		}

		bool ExistDomainsWhereDirectSuperDomain
		{
			get;
		}

		void AllorsRoleSyncAddDirectSuperDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleSyncRemoveDirectSuperDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleReleaseDirectSuperDomainDomain();


		global::Allors.R1.Meta.Domain[] DomainsWhereUnitDomain
		{
			get;
		}

		bool ExistDomainsWhereUnitDomain
		{
			get;
		}

		void AllorsRoleSyncAddUnitDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleSyncRemoveUnitDomainDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleReleaseUnitDomainDomain();


	}

	public interface AllorsInterfaceDomain :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassDomain :  global::Allors.R1.Meta.MetaObject,  AllorsInternalDomain , AllorsEmbeddedObject
	{
		protected global::Allors.R1.Meta.ObjectType[] _DomainDeclaredObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected System.Object _DomainName;


		protected global::Allors.R1.Meta.MethodType[] _DomainDeclaredMethodType = AllorsEmbeddedArrays.EMPTY_MethodType_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _DomainDerivedSuperDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;


		protected global::Allors.R1.Meta.Inheritance[] _DomainDeclaredInheritance = AllorsEmbeddedArrays.EMPTY_Inheritance_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _DomainDirectSuperDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;


		protected global::Allors.R1.Meta.Domain _DomainUnitDomain;


		protected global::Allors.R1.Meta.ObjectType[] _DomainDerivedUnitObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.ObjectType[] _DomainDerivedCompositeObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.RelationType[] _DomainDeclaredRelationType = AllorsEmbeddedArrays.EMPTY_RelationType_ARRAY;


		protected global::Allors.R1.Meta.RelationType[] _DomainDerivedRelationType = AllorsEmbeddedArrays.EMPTY_RelationType_ARRAY;


		protected global::Allors.R1.Meta.MethodType[] _DomainDerivedMethodType = AllorsEmbeddedArrays.EMPTY_MethodType_ARRAY;


		protected global::Allors.R1.Meta.Inheritance[] _DomainDerivedInheritance = AllorsEmbeddedArrays.EMPTY_Inheritance_ARRAY;


		protected global::Allors.R1.Meta.ObjectType[] _DomainDerivedObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _DerivedSuperDomainDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _DirectSuperDomainDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;


		protected global::Allors.R1.Meta.Domain[] _UnitDomainDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;




		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassDomain"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassDomain(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

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
				return new System.Guid("804929af-0208-4384-a74d-17353963d105");
			}
		}

		public override void Delete()
		{
			AllorsAssert();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredMethodType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedSuperDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredInheritance();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDirectSuperDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainUnitDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedUnitObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedCompositeObjectType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredRelationType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedRelationType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedMethodType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedInheritance();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedObjectType();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRoleReleaseDerivedSuperDomainDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRoleReleaseDirectSuperDomainDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRoleReleaseUnitDomainDomain();


			session.Delete(this);
			isDeleted = true;
		}

		object AllorsInternal.GetRole(AllorsEmbeddedRelationType relation)
		{
			AllorsAssert();
			switch(relation.Tag)
			{
				case AllorsRelationTags.DomainDeclaredObjectType:
					return _DomainDeclaredObjectType;
				case AllorsRelationTags.DomainName:
					return _DomainName;
				case AllorsRelationTags.DomainDeclaredMethodType:
					return _DomainDeclaredMethodType;
				case AllorsRelationTags.DomainDerivedSuperDomain:
					return _DomainDerivedSuperDomain;
				case AllorsRelationTags.DomainDeclaredInheritance:
					return _DomainDeclaredInheritance;
				case AllorsRelationTags.DomainDirectSuperDomain:
					return _DomainDirectSuperDomain;
				case AllorsRelationTags.DomainUnitDomain:
					return _DomainUnitDomain;
				case AllorsRelationTags.DomainDerivedUnitObjectType:
					return _DomainDerivedUnitObjectType;
				case AllorsRelationTags.DomainDerivedCompositeObjectType:
					return _DomainDerivedCompositeObjectType;
				case AllorsRelationTags.DomainDeclaredRelationType:
					return _DomainDeclaredRelationType;
				case AllorsRelationTags.DomainDerivedRelationType:
					return _DomainDerivedRelationType;
				case AllorsRelationTags.DomainDerivedMethodType:
					return _DomainDerivedMethodType;
				case AllorsRelationTags.DomainDerivedInheritance:
					return _DomainDerivedInheritance;
				case AllorsRelationTags.DomainDerivedObjectType:
					return _DomainDerivedObjectType;
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
				case AllorsRelationTags.DomainDeclaredObjectType:
					RoleSetDomainDeclaredObjectType((global::Allors.R1.Meta.ObjectType[])role);
					break;
				case AllorsRelationTags.DomainName:
					RoleSetDomainName((global::System.String)role);
					break;
				case AllorsRelationTags.DomainDeclaredMethodType:
					RoleSetDomainDeclaredMethodType((global::Allors.R1.Meta.MethodType[])role);
					break;
				case AllorsRelationTags.DomainDerivedSuperDomain:
					RoleSetDomainDerivedSuperDomain((global::Allors.R1.Meta.Domain[])role);
					break;
				case AllorsRelationTags.DomainDeclaredInheritance:
					RoleSetDomainDeclaredInheritance((global::Allors.R1.Meta.Inheritance[])role);
					break;
				case AllorsRelationTags.DomainDirectSuperDomain:
					RoleSetDomainDirectSuperDomain((global::Allors.R1.Meta.Domain[])role);
					break;
				case AllorsRelationTags.DomainUnitDomain:
					RoleSetDomainUnitDomain((global::Allors.R1.Meta.Domain)role);
					break;
				case AllorsRelationTags.DomainDerivedUnitObjectType:
					RoleSetDomainDerivedUnitObjectType((global::Allors.R1.Meta.ObjectType[])role);
					break;
				case AllorsRelationTags.DomainDerivedCompositeObjectType:
					RoleSetDomainDerivedCompositeObjectType((global::Allors.R1.Meta.ObjectType[])role);
					break;
				case AllorsRelationTags.DomainDeclaredRelationType:
					RoleSetDomainDeclaredRelationType((global::Allors.R1.Meta.RelationType[])role);
					break;
				case AllorsRelationTags.DomainDerivedRelationType:
					RoleSetDomainDerivedRelationType((global::Allors.R1.Meta.RelationType[])role);
					break;
				case AllorsRelationTags.DomainDerivedMethodType:
					RoleSetDomainDerivedMethodType((global::Allors.R1.Meta.MethodType[])role);
					break;
				case AllorsRelationTags.DomainDerivedInheritance:
					RoleSetDomainDerivedInheritance((global::Allors.R1.Meta.Inheritance[])role);
					break;
				case AllorsRelationTags.DomainDerivedObjectType:
					RoleSetDomainDerivedObjectType((global::Allors.R1.Meta.ObjectType[])role);
					break;
				case AllorsRelationTags.MetaObjectId:
					RoleSetMetaObjectId((global::System.Guid)role);
					break;

		default:
				throw new System.ArgumentException("Illegal relation " + relation.Name);
			}
		}


		public virtual global::Allors.R1.Meta.ObjectType[] DeclaredObjectTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDeclaredObjectType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDeclaredObjectType(value);
			}
		}

		protected void RoleSetDomainDeclaredObjectType(global::Allors.R1.Meta.ObjectType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredObjectType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in roles )
				{
					RoleAddDeclaredObjectType(role);
				}
			}
		}

		public virtual void AddDeclaredObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			RoleAddDeclaredObjectType( addRole );
		}

		void RoleAddDeclaredObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleReleaseDeclaredObjectTypeDomain();
				if( !AllorsEmbeddedArrays.Exist( _DomainDeclaredObjectType, addRole ) ) 
				{
					// association side
					_DomainDeclaredObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Add( _DomainDeclaredObjectType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleSyncSetDeclaredObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDeclaredObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredObjectType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDeclaredObjectType, removeRole ) ) 
				{
					_DomainDeclaredObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove( _DomainDeclaredObjectType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)removeRole).AllorsRoleSyncSetDeclaredObjectTypeDomain( null ); 
			
				}
			}
		}

		public virtual void RemoveDeclaredObjectTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredObjectType()
		{
			AllorsAssert();

			if( _DomainDeclaredObjectType!=null )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in _DomainDeclaredObjectType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)role).AllorsRoleSyncSetDeclaredObjectTypeDomain( null ); 
				}
			}
			_DomainDeclaredObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;
		}

		public virtual bool ExistDeclaredObjectTypes
		{
			get
			{
				return _DomainDeclaredObjectType.Length > 0;
			}
		}


		public virtual global::System.String Name
		{
			get
			{ 
				AllorsAssert();
				return (global::System.String)_DomainName;
			}

			set
			{
				AllorsAssert();
				RoleSetDomainName(value);
			}
		}

		protected void RoleSetDomainName(global::System.String role)
		{
			_DomainName = role;
		}

		public virtual bool ExistName
		{
			get
			{
				return _DomainName != null;
			}
		}

		public virtual void RemoveName()
		{
			_DomainName = null;
		}

		public virtual global::Allors.R1.Meta.MethodType[] DeclaredMethodTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDeclaredMethodType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDeclaredMethodType(value);
			}
		}

		protected void RoleSetDomainDeclaredMethodType(global::Allors.R1.Meta.MethodType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredMethodType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.MethodType role in roles )
				{
					RoleAddDeclaredMethodType(role);
				}
			}
		}

		public virtual void AddDeclaredMethodType( global::Allors.R1.Meta.MethodType addRole )
		{
			RoleAddDeclaredMethodType( addRole );
		}

		void RoleAddDeclaredMethodType( global::Allors.R1.Meta.MethodType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)addRole).AllorsRoleReleaseDeclaredMethodTypeDomain();
				if( !AllorsEmbeddedArrays.Exist( _DomainDeclaredMethodType, addRole ) ) 
				{
					// association side
					_DomainDeclaredMethodType = (global::Allors.R1.Meta.MethodType[]) AllorsEmbeddedArrays.Add( _DomainDeclaredMethodType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)addRole).AllorsRoleSyncSetDeclaredMethodTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDeclaredMethodType( global::Allors.R1.Meta.MethodType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredMethodType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredMethodType( global::Allors.R1.Meta.MethodType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDeclaredMethodType, removeRole ) ) 
				{
					_DomainDeclaredMethodType = (global::Allors.R1.Meta.MethodType[]) AllorsEmbeddedArrays.Remove( _DomainDeclaredMethodType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)removeRole).AllorsRoleSyncSetDeclaredMethodTypeDomain( null ); 
			
				}
			}
		}

		public virtual void RemoveDeclaredMethodTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredMethodType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredMethodType()
		{
			AllorsAssert();

			if( _DomainDeclaredMethodType!=null )
			{
				foreach( global::Allors.R1.Meta.MethodType role in _DomainDeclaredMethodType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)role).AllorsRoleSyncSetDeclaredMethodTypeDomain( null ); 
				}
			}
			_DomainDeclaredMethodType = AllorsEmbeddedArrays.EMPTY_MethodType_ARRAY;
		}

		public virtual bool ExistDeclaredMethodTypes
		{
			get
			{
				return _DomainDeclaredMethodType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Domain[] DerivedSuperDomains
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedSuperDomain;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedSuperDomain(value);
			}
		}

		protected void RoleSetDomainDerivedSuperDomain(global::Allors.R1.Meta.Domain[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedSuperDomain();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.Domain role in roles )
				{
					RoleAddDerivedSuperDomain(role);
				}
			}
		}

		public virtual void AddDerivedSuperDomain( global::Allors.R1.Meta.Domain addRole )
		{
			RoleAddDerivedSuperDomain( addRole );
		}

		void RoleAddDerivedSuperDomain( global::Allors.R1.Meta.Domain addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedSuperDomain, addRole ) ) 
				{
					// association side
					_DomainDerivedSuperDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Add( _DomainDerivedSuperDomain, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)addRole).AllorsRoleSyncAddDerivedSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedSuperDomain( global::Allors.R1.Meta.Domain removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedSuperDomain( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedSuperDomain( global::Allors.R1.Meta.Domain removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedSuperDomain, removeRole ) ) 
				{
					_DomainDerivedSuperDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove( _DomainDerivedSuperDomain, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)removeRole).AllorsRoleSyncRemoveDerivedSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedSuperDomains()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedSuperDomain();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedSuperDomain()
		{
			AllorsAssert();

			if( _DomainDerivedSuperDomain!=null )
			{
				foreach( global::Allors.R1.Meta.Domain role in _DomainDerivedSuperDomain ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)role).AllorsRoleSyncRemoveDerivedSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedSuperDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;
		}

		public virtual bool ExistDerivedSuperDomains
		{
			get
			{
				return _DomainDerivedSuperDomain.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Inheritance[] DeclaredInheritances
		{
			get
			{ 
				AllorsAssert();
				return _DomainDeclaredInheritance;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDeclaredInheritance(value);
			}
		}

		protected void RoleSetDomainDeclaredInheritance(global::Allors.R1.Meta.Inheritance[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredInheritance();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.Inheritance role in roles )
				{
					RoleAddDeclaredInheritance(role);
				}
			}
		}

		public virtual void AddDeclaredInheritance( global::Allors.R1.Meta.Inheritance addRole )
		{
			RoleAddDeclaredInheritance( addRole );
		}

		void RoleAddDeclaredInheritance( global::Allors.R1.Meta.Inheritance addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)addRole).AllorsRoleReleaseDeclaredInheritanceDomain();
				if( !AllorsEmbeddedArrays.Exist( _DomainDeclaredInheritance, addRole ) ) 
				{
					// association side
					_DomainDeclaredInheritance = (global::Allors.R1.Meta.Inheritance[]) AllorsEmbeddedArrays.Add( _DomainDeclaredInheritance, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)addRole).AllorsRoleSyncSetDeclaredInheritanceDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDeclaredInheritance( global::Allors.R1.Meta.Inheritance removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredInheritance( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredInheritance( global::Allors.R1.Meta.Inheritance removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDeclaredInheritance, removeRole ) ) 
				{
					_DomainDeclaredInheritance = (global::Allors.R1.Meta.Inheritance[]) AllorsEmbeddedArrays.Remove( _DomainDeclaredInheritance, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)removeRole).AllorsRoleSyncSetDeclaredInheritanceDomain( null ); 
			
				}
			}
		}

		public virtual void RemoveDeclaredInheritances()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredInheritance();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredInheritance()
		{
			AllorsAssert();

			if( _DomainDeclaredInheritance!=null )
			{
				foreach( global::Allors.R1.Meta.Inheritance role in _DomainDeclaredInheritance ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)role).AllorsRoleSyncSetDeclaredInheritanceDomain( null ); 
				}
			}
			_DomainDeclaredInheritance = AllorsEmbeddedArrays.EMPTY_Inheritance_ARRAY;
		}

		public virtual bool ExistDeclaredInheritances
		{
			get
			{
				return _DomainDeclaredInheritance.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Domain[] DirectSuperDomains
		{
			get
			{ 
				AllorsAssert();
				return _DomainDirectSuperDomain;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDirectSuperDomain(value);
			}
		}

		protected void RoleSetDomainDirectSuperDomain(global::Allors.R1.Meta.Domain[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDirectSuperDomain();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.Domain role in roles )
				{
					RoleAddDirectSuperDomain(role);
				}
			}
		}

		public virtual void AddDirectSuperDomain( global::Allors.R1.Meta.Domain addRole )
		{
			RoleAddDirectSuperDomain( addRole );
		}

		void RoleAddDirectSuperDomain( global::Allors.R1.Meta.Domain addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDirectSuperDomain, addRole ) ) 
				{
					// association side
					_DomainDirectSuperDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Add( _DomainDirectSuperDomain, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)addRole).AllorsRoleSyncAddDirectSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDirectSuperDomain( global::Allors.R1.Meta.Domain removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDirectSuperDomain( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDirectSuperDomain( global::Allors.R1.Meta.Domain removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDirectSuperDomain, removeRole ) ) 
				{
					_DomainDirectSuperDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove( _DomainDirectSuperDomain, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)removeRole).AllorsRoleSyncRemoveDirectSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDirectSuperDomains()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDirectSuperDomain();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDirectSuperDomain()
		{
			AllorsAssert();

			if( _DomainDirectSuperDomain!=null )
			{
				foreach( global::Allors.R1.Meta.Domain role in _DomainDirectSuperDomain ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)role).AllorsRoleSyncRemoveDirectSuperDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDirectSuperDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;
		}

		public virtual bool ExistDirectSuperDomains
		{
			get
			{
				return _DomainDirectSuperDomain.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Domain UnitDomain
		{
			get
			{
				AllorsAssert();
				return _DomainUnitDomain;
			}

			set
			{
				AllorsAssert(value);
				RoleSetDomainUnitDomain(value);
			}
		}

		protected void RoleSetDomainUnitDomain(global::Allors.R1.Meta.Domain value)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainUnitDomain();
			if( value != null ) 
			{
				_DomainUnitDomain = value;
				_DomainUnitDomain = value;
				// role side
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)_DomainUnitDomain).AllorsRoleSyncAddUnitDomainDomain((global::Allors.R1.Meta.Domain) this ); 
			}
		}

		public virtual void RemoveUnitDomain()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainUnitDomain();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainUnitDomain()
		{
			AllorsAssert();
			if( _DomainUnitDomain != null) 
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)_DomainUnitDomain).AllorsRoleSyncRemoveUnitDomainDomain( (global::Allors.R1.Meta.Domain) this ); 
				_DomainUnitDomain = null;
				_DomainUnitDomain = null;
			}
		}

		public virtual bool ExistUnitDomain
		{
			get
			{
				return _DomainUnitDomain != null;
			}
		}

		public virtual global::Allors.R1.Meta.ObjectType[] DerivedUnitObjectTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedUnitObjectType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedUnitObjectType(value);
			}
		}

		protected void RoleSetDomainDerivedUnitObjectType(global::Allors.R1.Meta.ObjectType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedUnitObjectType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in roles )
				{
					RoleAddDerivedUnitObjectType(role);
				}
			}
		}

		public virtual void AddDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			RoleAddDerivedUnitObjectType( addRole );
		}

		void RoleAddDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedUnitObjectType, addRole ) ) 
				{
					// association side
					_DomainDerivedUnitObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Add( _DomainDerivedUnitObjectType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleSyncAddDerivedUnitObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedUnitObjectType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedUnitObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedUnitObjectType, removeRole ) ) 
				{
					_DomainDerivedUnitObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove( _DomainDerivedUnitObjectType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)removeRole).AllorsRoleSyncRemoveDerivedUnitObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedUnitObjectTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedUnitObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedUnitObjectType()
		{
			AllorsAssert();

			if( _DomainDerivedUnitObjectType!=null )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in _DomainDerivedUnitObjectType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)role).AllorsRoleSyncRemoveDerivedUnitObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedUnitObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;
		}

		public virtual bool ExistDerivedUnitObjectTypes
		{
			get
			{
				return _DomainDerivedUnitObjectType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.ObjectType[] DerivedCompositeObjectTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedCompositeObjectType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedCompositeObjectType(value);
			}
		}

		protected void RoleSetDomainDerivedCompositeObjectType(global::Allors.R1.Meta.ObjectType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedCompositeObjectType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in roles )
				{
					RoleAddDerivedCompositeObjectType(role);
				}
			}
		}

		public virtual void AddDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			RoleAddDerivedCompositeObjectType( addRole );
		}

		void RoleAddDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedCompositeObjectType, addRole ) ) 
				{
					// association side
					_DomainDerivedCompositeObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Add( _DomainDerivedCompositeObjectType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleSyncAddDerivedCompositeObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedCompositeObjectType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedCompositeObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedCompositeObjectType, removeRole ) ) 
				{
					_DomainDerivedCompositeObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove( _DomainDerivedCompositeObjectType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)removeRole).AllorsRoleSyncRemoveDerivedCompositeObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedCompositeObjectTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedCompositeObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedCompositeObjectType()
		{
			AllorsAssert();

			if( _DomainDerivedCompositeObjectType!=null )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in _DomainDerivedCompositeObjectType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)role).AllorsRoleSyncRemoveDerivedCompositeObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedCompositeObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;
		}

		public virtual bool ExistDerivedCompositeObjectTypes
		{
			get
			{
				return _DomainDerivedCompositeObjectType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.RelationType[] DeclaredRelationTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDeclaredRelationType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDeclaredRelationType(value);
			}
		}

		protected void RoleSetDomainDeclaredRelationType(global::Allors.R1.Meta.RelationType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredRelationType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.RelationType role in roles )
				{
					RoleAddDeclaredRelationType(role);
				}
			}
		}

		public virtual void AddDeclaredRelationType( global::Allors.R1.Meta.RelationType addRole )
		{
			RoleAddDeclaredRelationType( addRole );
		}

		void RoleAddDeclaredRelationType( global::Allors.R1.Meta.RelationType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)addRole).AllorsRoleReleaseDeclaredRelationTypeDomain();
				if( !AllorsEmbeddedArrays.Exist( _DomainDeclaredRelationType, addRole ) ) 
				{
					// association side
					_DomainDeclaredRelationType = (global::Allors.R1.Meta.RelationType[]) AllorsEmbeddedArrays.Add( _DomainDeclaredRelationType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)addRole).AllorsRoleSyncSetDeclaredRelationTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDeclaredRelationType( global::Allors.R1.Meta.RelationType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredRelationType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredRelationType( global::Allors.R1.Meta.RelationType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDeclaredRelationType, removeRole ) ) 
				{
					_DomainDeclaredRelationType = (global::Allors.R1.Meta.RelationType[]) AllorsEmbeddedArrays.Remove( _DomainDeclaredRelationType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)removeRole).AllorsRoleSyncSetDeclaredRelationTypeDomain( null ); 
			
				}
			}
		}

		public virtual void RemoveDeclaredRelationTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDeclaredRelationType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDeclaredRelationType()
		{
			AllorsAssert();

			if( _DomainDeclaredRelationType!=null )
			{
				foreach( global::Allors.R1.Meta.RelationType role in _DomainDeclaredRelationType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)role).AllorsRoleSyncSetDeclaredRelationTypeDomain( null ); 
				}
			}
			_DomainDeclaredRelationType = AllorsEmbeddedArrays.EMPTY_RelationType_ARRAY;
		}

		public virtual bool ExistDeclaredRelationTypes
		{
			get
			{
				return _DomainDeclaredRelationType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.RelationType[] DerivedRelationTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedRelationType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedRelationType(value);
			}
		}

		protected void RoleSetDomainDerivedRelationType(global::Allors.R1.Meta.RelationType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedRelationType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.RelationType role in roles )
				{
					RoleAddDerivedRelationType(role);
				}
			}
		}

		public virtual void AddDerivedRelationType( global::Allors.R1.Meta.RelationType addRole )
		{
			RoleAddDerivedRelationType( addRole );
		}

		void RoleAddDerivedRelationType( global::Allors.R1.Meta.RelationType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedRelationType, addRole ) ) 
				{
					// association side
					_DomainDerivedRelationType = (global::Allors.R1.Meta.RelationType[]) AllorsEmbeddedArrays.Add( _DomainDerivedRelationType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)addRole).AllorsRoleSyncAddDerivedRelationTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedRelationType( global::Allors.R1.Meta.RelationType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedRelationType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedRelationType( global::Allors.R1.Meta.RelationType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedRelationType, removeRole ) ) 
				{
					_DomainDerivedRelationType = (global::Allors.R1.Meta.RelationType[]) AllorsEmbeddedArrays.Remove( _DomainDerivedRelationType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)removeRole).AllorsRoleSyncRemoveDerivedRelationTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedRelationTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedRelationType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedRelationType()
		{
			AllorsAssert();

			if( _DomainDerivedRelationType!=null )
			{
				foreach( global::Allors.R1.Meta.RelationType role in _DomainDerivedRelationType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)role).AllorsRoleSyncRemoveDerivedRelationTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedRelationType = AllorsEmbeddedArrays.EMPTY_RelationType_ARRAY;
		}

		public virtual bool ExistDerivedRelationTypes
		{
			get
			{
				return _DomainDerivedRelationType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.MethodType[] DerivedMethodTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedMethodType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedMethodType(value);
			}
		}

		protected void RoleSetDomainDerivedMethodType(global::Allors.R1.Meta.MethodType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedMethodType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.MethodType role in roles )
				{
					RoleAddDerivedMethodType(role);
				}
			}
		}

		public virtual void AddDerivedMethodType( global::Allors.R1.Meta.MethodType addRole )
		{
			RoleAddDerivedMethodType( addRole );
		}

		void RoleAddDerivedMethodType( global::Allors.R1.Meta.MethodType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedMethodType, addRole ) ) 
				{
					// association side
					_DomainDerivedMethodType = (global::Allors.R1.Meta.MethodType[]) AllorsEmbeddedArrays.Add( _DomainDerivedMethodType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)addRole).AllorsRoleSyncAddDerivedMethodTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedMethodType( global::Allors.R1.Meta.MethodType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedMethodType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedMethodType( global::Allors.R1.Meta.MethodType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedMethodType, removeRole ) ) 
				{
					_DomainDerivedMethodType = (global::Allors.R1.Meta.MethodType[]) AllorsEmbeddedArrays.Remove( _DomainDerivedMethodType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)removeRole).AllorsRoleSyncRemoveDerivedMethodTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedMethodTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedMethodType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedMethodType()
		{
			AllorsAssert();

			if( _DomainDerivedMethodType!=null )
			{
				foreach( global::Allors.R1.Meta.MethodType role in _DomainDerivedMethodType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalMethodType)role).AllorsRoleSyncRemoveDerivedMethodTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedMethodType = AllorsEmbeddedArrays.EMPTY_MethodType_ARRAY;
		}

		public virtual bool ExistDerivedMethodTypes
		{
			get
			{
				return _DomainDerivedMethodType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Inheritance[] DerivedInheritances
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedInheritance;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedInheritance(value);
			}
		}

		protected void RoleSetDomainDerivedInheritance(global::Allors.R1.Meta.Inheritance[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedInheritance();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.Inheritance role in roles )
				{
					RoleAddDerivedInheritance(role);
				}
			}
		}

		public virtual void AddDerivedInheritance( global::Allors.R1.Meta.Inheritance addRole )
		{
			RoleAddDerivedInheritance( addRole );
		}

		void RoleAddDerivedInheritance( global::Allors.R1.Meta.Inheritance addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedInheritance, addRole ) ) 
				{
					// association side
					_DomainDerivedInheritance = (global::Allors.R1.Meta.Inheritance[]) AllorsEmbeddedArrays.Add( _DomainDerivedInheritance, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)addRole).AllorsRoleSyncAddDerivedInheritanceDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedInheritance( global::Allors.R1.Meta.Inheritance removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedInheritance( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedInheritance( global::Allors.R1.Meta.Inheritance removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedInheritance, removeRole ) ) 
				{
					_DomainDerivedInheritance = (global::Allors.R1.Meta.Inheritance[]) AllorsEmbeddedArrays.Remove( _DomainDerivedInheritance, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)removeRole).AllorsRoleSyncRemoveDerivedInheritanceDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedInheritances()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedInheritance();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedInheritance()
		{
			AllorsAssert();

			if( _DomainDerivedInheritance!=null )
			{
				foreach( global::Allors.R1.Meta.Inheritance role in _DomainDerivedInheritance ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalInheritance)role).AllorsRoleSyncRemoveDerivedInheritanceDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedInheritance = AllorsEmbeddedArrays.EMPTY_Inheritance_ARRAY;
		}

		public virtual bool ExistDerivedInheritances
		{
			get
			{
				return _DomainDerivedInheritance.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.ObjectType[] DerivedObjectTypes
		{
			get
			{ 
				AllorsAssert();
				return _DomainDerivedObjectType;
			}

			set
			{ 
				AllorsAssert(value);
				RoleSetDomainDerivedObjectType(value);
			}
		}

		protected void RoleSetDomainDerivedObjectType(global::Allors.R1.Meta.ObjectType[] roles)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedObjectType();
			if( roles != null && roles.Length > 0 )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in roles )
				{
					RoleAddDerivedObjectType(role);
				}
			}
		}

		public virtual void AddDerivedObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			RoleAddDerivedObjectType( addRole );
		}

		void RoleAddDerivedObjectType( global::Allors.R1.Meta.ObjectType addRole )
		{
			AllorsAssert(addRole);
			if( addRole != null )
			{
				if( !AllorsEmbeddedArrays.Exist( _DomainDerivedObjectType, addRole ) ) 
				{
					// association side
					_DomainDerivedObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Add( _DomainDerivedObjectType, addRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)addRole).AllorsRoleSyncAddDerivedObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
		}

		public virtual void RemoveDerivedObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedObjectType( removeRole );
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedObjectType( global::Allors.R1.Meta.ObjectType removeRole ) 
		{
			AllorsAssert(removeRole);
			if( removeRole != null )
			{
				if(AllorsEmbeddedArrays.Exist( _DomainDerivedObjectType, removeRole ) ) 
				{
					_DomainDerivedObjectType = (global::Allors.R1.Meta.ObjectType[]) AllorsEmbeddedArrays.Remove( _DomainDerivedObjectType, removeRole );
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)removeRole).AllorsRoleSyncRemoveDerivedObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
			
				}
			}
		}

		public virtual void RemoveDerivedObjectTypes()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)this).AllorsRemoveDomainDerivedObjectType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRemoveDomainDerivedObjectType()
		{
			AllorsAssert();

			if( _DomainDerivedObjectType!=null )
			{
				foreach( global::Allors.R1.Meta.ObjectType role in _DomainDerivedObjectType ) 
				{
					// role side
					((Allors.R1.Meta.AllorsGenerated.AllorsInternalObjectType)role).AllorsRoleSyncRemoveDerivedObjectTypeDomain( (global::Allors.R1.Meta.Domain) this ); 
				}
			}
			_DomainDerivedObjectType = AllorsEmbeddedArrays.EMPTY_ObjectType_ARRAY;
		}

		public virtual bool ExistDerivedObjectTypes
		{
			get
			{
				return _DomainDerivedObjectType.Length > 0;
			}
		}


		public virtual global::Allors.R1.Meta.Domain[] DomainsWhereDerivedSuperDomain
		{
			get
			{
				AllorsAssert();
				return _DerivedSuperDomainDomain;
			}
		}

		public virtual bool ExistDomainsWhereDerivedSuperDomain
		{
			get
			{
				return _DerivedSuperDomainDomain.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncAddDerivedSuperDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedSuperDomainDomain, association ) ) 
			{
				_DerivedSuperDomainDomain = (global::Allors.R1.Meta.Domain[])AllorsEmbeddedArrays.Add(_DerivedSuperDomainDomain,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncRemoveDerivedSuperDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DerivedSuperDomainDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove(_DerivedSuperDomainDomain,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleReleaseDerivedSuperDomainDomain()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.Domain association in _DerivedSuperDomainDomain )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)association).AllorsRemoveDomainDerivedSuperDomain((global::Allors.R1.Meta.Domain) this);
			}
		}

		public virtual global::Allors.R1.Meta.Domain[] DomainsWhereDirectSuperDomain
		{
			get
			{
				AllorsAssert();
				return _DirectSuperDomainDomain;
			}
		}

		public virtual bool ExistDomainsWhereDirectSuperDomain
		{
			get
			{
				return _DirectSuperDomainDomain.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncAddDirectSuperDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DirectSuperDomainDomain, association ) ) 
			{
				_DirectSuperDomainDomain = (global::Allors.R1.Meta.Domain[])AllorsEmbeddedArrays.Add(_DirectSuperDomainDomain,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncRemoveDirectSuperDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DirectSuperDomainDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove(_DirectSuperDomainDomain,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleReleaseDirectSuperDomainDomain()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.Domain association in _DirectSuperDomainDomain )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)association).AllorsRemoveDomainDirectSuperDomain((global::Allors.R1.Meta.Domain) this);
			}
		}

		public virtual global::Allors.R1.Meta.Domain[] DomainsWhereUnitDomain
		{
			get
			{
				AllorsAssert();
				return _UnitDomainDomain;
			}
		}

		public virtual bool ExistDomainsWhereUnitDomain
		{
			get
			{
				return _UnitDomainDomain.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncAddUnitDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _UnitDomainDomain, association ) ) 
			{
				_UnitDomainDomain = (global::Allors.R1.Meta.Domain[])AllorsEmbeddedArrays.Add(_UnitDomainDomain,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleSyncRemoveUnitDomainDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_UnitDomainDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove(_UnitDomainDomain,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain.AllorsRoleReleaseUnitDomainDomain()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.Domain association in _UnitDomainDomain )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)association).AllorsRemoveDomainUnitDomain();
			}
		}

}
}