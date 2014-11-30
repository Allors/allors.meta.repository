namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalRelationType : AllorsInternalMetaObject, AllorsInternal
	{
		global::System.Boolean IsDerived
		{
			get;
			set;
		}

		void RemoveIsDerived();

		bool ExistIsDerived
		{
			get;
		}


		global::System.Boolean IsIndexed
		{
			get;
			set;
		}

		void RemoveIsIndexed();

		bool ExistIsIndexed
		{
			get;
		}


		global::Allors.R1.Meta.RoleType RoleType
		{
			get;
			set;
		}

		void RemoveRoleType();

		bool ExistRoleType
		{
			get;
		}

		void AllorsRemoveRelationTypeRoleType();


		global::Allors.R1.Meta.AssociationType AssociationType
		{
			get;
			set;
		}

		void RemoveAssociationType();

		bool ExistAssociationType
		{
			get;
		}

		void AllorsRemoveRelationTypeAssociationType();



		global::Allors.R1.Meta.Domain DomainWhereDeclaredRelationType
		{
			get;
		}

		bool ExistDomainWhereDeclaredRelationType
		{
			get;
		}


		void AllorsRoleSyncSetDeclaredRelationTypeDomain(global::Allors.R1.Meta.Domain association );


		void AllorsRoleReleaseDeclaredRelationTypeDomain();


		global::Allors.R1.Meta.Domain[] DomainsWhereDerivedRelationType
		{
			get;
		}

		bool ExistDomainsWhereDerivedRelationType
		{
			get;
		}

		void AllorsRoleSyncAddDerivedRelationTypeDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleSyncRemoveDerivedRelationTypeDomain(global::Allors.R1.Meta.Domain association );

		void AllorsRoleReleaseDerivedRelationTypeDomain();


	}

	public interface AllorsInterfaceRelationType :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassRelationType :  global::Allors.R1.Meta.MetaObject,  AllorsInternalRelationType , AllorsEmbeddedObject
	{
		protected System.Object _RelationTypeIsDerived;


		protected System.Object _RelationTypeIsIndexed;


		protected global::Allors.R1.Meta.RoleType _RelationTypeRoleType;


		protected global::Allors.R1.Meta.AssociationType _RelationTypeAssociationType;


		protected global::Allors.R1.Meta.Domain _DeclaredRelationTypeDomain;


		protected global::Allors.R1.Meta.Domain[] _DerivedRelationTypeDomain = AllorsEmbeddedArrays.EMPTY_Domain_ARRAY;




		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassRelationType"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassRelationType(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

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
				return new System.Guid("c03575fa-2d84-4096-9c4d-93cf05d3c1de");
			}
		}

		public override void Delete()
		{
			AllorsAssert();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeRoleType();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeAssociationType();

			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRoleReleaseDeclaredRelationTypeDomain();
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRoleReleaseDerivedRelationTypeDomain();


			session.Delete(this);
			isDeleted = true;
		}

		object AllorsInternal.GetRole(AllorsEmbeddedRelationType relation)
		{
			AllorsAssert();
			switch(relation.Tag)
			{
				case AllorsRelationTags.RelationTypeIsDerived:
					return _RelationTypeIsDerived;
				case AllorsRelationTags.RelationTypeIsIndexed:
					return _RelationTypeIsIndexed;
				case AllorsRelationTags.RelationTypeRoleType:
					return _RelationTypeRoleType;
				case AllorsRelationTags.RelationTypeAssociationType:
					return _RelationTypeAssociationType;
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
				case AllorsRelationTags.RelationTypeIsDerived:
					RoleSetRelationTypeIsDerived((global::System.Boolean)role);
					break;
				case AllorsRelationTags.RelationTypeIsIndexed:
					RoleSetRelationTypeIsIndexed((global::System.Boolean)role);
					break;
				case AllorsRelationTags.RelationTypeRoleType:
					RoleSetRelationTypeRoleType((global::Allors.R1.Meta.RoleType)role);
					break;
				case AllorsRelationTags.RelationTypeAssociationType:
					RoleSetRelationTypeAssociationType((global::Allors.R1.Meta.AssociationType)role);
					break;
				case AllorsRelationTags.MetaObjectId:
					RoleSetMetaObjectId((global::System.Guid)role);
					break;

		default:
				throw new System.ArgumentException("Illegal relation " + relation.Name);
			}
		}


		public virtual global::System.Boolean IsDerived
		{
			get
			{ 
				AllorsAssert();
				return (global::System.Boolean)_RelationTypeIsDerived;
			}

			set
			{
				AllorsAssert();
				RoleSetRelationTypeIsDerived(value);
			}
		}

		protected void RoleSetRelationTypeIsDerived(global::System.Boolean role)
		{
			_RelationTypeIsDerived = role;
		}

		public virtual bool ExistIsDerived
		{
			get
			{
				return _RelationTypeIsDerived != null;
			}
		}

		public virtual void RemoveIsDerived()
		{
			_RelationTypeIsDerived = null;
		}

		public virtual global::System.Boolean IsIndexed
		{
			get
			{ 
				AllorsAssert();
				return (global::System.Boolean)_RelationTypeIsIndexed;
			}

			set
			{
				AllorsAssert();
				RoleSetRelationTypeIsIndexed(value);
			}
		}

		protected void RoleSetRelationTypeIsIndexed(global::System.Boolean role)
		{
			_RelationTypeIsIndexed = role;
		}

		public virtual bool ExistIsIndexed
		{
			get
			{
				return _RelationTypeIsIndexed != null;
			}
		}

		public virtual void RemoveIsIndexed()
		{
			_RelationTypeIsIndexed = null;
		}

		public virtual global::Allors.R1.Meta.RoleType RoleType
		{
			get
			{
				AllorsAssert();
				return _RelationTypeRoleType;
			}

			set
			{
				AllorsAssert(value);
				RoleSetRelationTypeRoleType(value);
			}
		}

		protected void RoleSetRelationTypeRoleType(global::Allors.R1.Meta.RoleType value)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeRoleType();
			if( value != null ) 
			{
				_RelationTypeRoleType = value;
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalRoleType)_RelationTypeRoleType).AllorsRoleReleaseRoleTypeRelationType();
				_RelationTypeRoleType = value;
				// role side
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalRoleType)_RelationTypeRoleType).AllorsRoleSyncSetRoleTypeRelationType((global::Allors.R1.Meta.RelationType) this ); 
			}
		}

		public virtual void RemoveRoleType()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeRoleType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRemoveRelationTypeRoleType()
		{
			AllorsAssert();
			if( _RelationTypeRoleType != null) 
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalRoleType)_RelationTypeRoleType).AllorsRoleSyncSetRoleTypeRelationType( null ); 
				_RelationTypeRoleType = null;
				_RelationTypeRoleType = null;
			}
		}

		public virtual bool ExistRoleType
		{
			get
			{
				return _RelationTypeRoleType != null;
			}
		}

		public virtual global::Allors.R1.Meta.AssociationType AssociationType
		{
			get
			{
				AllorsAssert();
				return _RelationTypeAssociationType;
			}

			set
			{
				AllorsAssert(value);
				RoleSetRelationTypeAssociationType(value);
			}
		}

		protected void RoleSetRelationTypeAssociationType(global::Allors.R1.Meta.AssociationType value)
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeAssociationType();
			if( value != null ) 
			{
				_RelationTypeAssociationType = value;
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)_RelationTypeAssociationType).AllorsRoleReleaseAssociationTypeRelationType();
				_RelationTypeAssociationType = value;
				// role side
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)_RelationTypeAssociationType).AllorsRoleSyncSetAssociationTypeRelationType((global::Allors.R1.Meta.RelationType) this ); 
			}
		}

		public virtual void RemoveAssociationType()
		{
			((Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType)this).AllorsRemoveRelationTypeAssociationType();
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRemoveRelationTypeAssociationType()
		{
			AllorsAssert();
			if( _RelationTypeAssociationType != null) 
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalAssociationType)_RelationTypeAssociationType).AllorsRoleSyncSetAssociationTypeRelationType( null ); 
				_RelationTypeAssociationType = null;
				_RelationTypeAssociationType = null;
			}
		}

		public virtual bool ExistAssociationType
		{
			get
			{
				return _RelationTypeAssociationType != null;
			}
		}


		public virtual global::Allors.R1.Meta.Domain DomainWhereDeclaredRelationType
		{
			get
			{
				AllorsAssert();
				return _DeclaredRelationTypeDomain;
			}
		}

		public virtual bool ExistDomainWhereDeclaredRelationType
		{
			get
			{
				return _DeclaredRelationTypeDomain != null;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRoleSyncSetDeclaredRelationTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DeclaredRelationTypeDomain = association;
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRoleReleaseDeclaredRelationTypeDomain()
		{
			if( _DeclaredRelationTypeDomain != null )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)_DeclaredRelationTypeDomain).AllorsRemoveDomainDeclaredRelationType( (global::Allors.R1.Meta.RelationType) this);
			}
		}


		public virtual global::Allors.R1.Meta.Domain[] DomainsWhereDerivedRelationType
		{
			get
			{
				AllorsAssert();
				return _DerivedRelationTypeDomain;
			}
		}

		public virtual bool ExistDomainsWhereDerivedRelationType
		{
			get
			{
				return _DerivedRelationTypeDomain.Length > 0;
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRoleSyncAddDerivedRelationTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			if( !AllorsEmbeddedArrays.Exist( _DerivedRelationTypeDomain, association ) ) 
			{
				_DerivedRelationTypeDomain = (global::Allors.R1.Meta.Domain[])AllorsEmbeddedArrays.Add(_DerivedRelationTypeDomain,association);
			}
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRoleSyncRemoveDerivedRelationTypeDomain(global::Allors.R1.Meta.Domain association)
		{
			AllorsAssert();
			_DerivedRelationTypeDomain = (global::Allors.R1.Meta.Domain[]) AllorsEmbeddedArrays.Remove(_DerivedRelationTypeDomain,association);
		}

		void Allors.R1.Meta.AllorsGenerated.AllorsInternalRelationType.AllorsRoleReleaseDerivedRelationTypeDomain()
		{
			AllorsAssert();
			foreach( global::Allors.R1.Meta.Domain association in _DerivedRelationTypeDomain )
			{
				((Allors.R1.Meta.AllorsGenerated.AllorsInternalDomain)association).AllorsRemoveDomainDerivedRelationType((global::Allors.R1.Meta.RelationType) this);
			}
		}

}
}