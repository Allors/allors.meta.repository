namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalMetaObject :  AllorsInternal
	{
		global::System.Guid Id
		{
			get;
			set;
		}

		void RemoveId();

		bool ExistId
		{
			get;
		}


	}

	public interface AllorsInterfaceMetaObject :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassMetaObject :  AllorsInternalMetaObject , AllorsEmbeddedObject
	{
		protected System.Object _MetaObjectId;




		/// <summary>
		/// Do not use, this is an Allors internal field.
		/// </summary>
		protected bool isDeleted;

		/// <summary>
		/// Do not use, this is an Allors internal field.
		/// </summary>
		protected AllorsEmbeddedSession session;

		/// <summary>
		/// Do not use, this is an Allors internal field.
		/// </summary>
		protected System.Int32 id;

		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassMetaObject"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassMetaObject ( AllorsEmbeddedSession session, System.Int32 id ) 
		{
			this.session = session;
			this.id = id;
		}

		public AllorsEmbeddedSession AllorsSession
		{ 
			get
			{
				return session;
			}
		}

		public int AllorsObjectId
		{ 
			get
			{
				return id;
			}
		}

		public bool IsDeleted
		{ 
			get
			{
				return isDeleted;
			}
		}

		/// <summary>
		/// Asserts that this Object is in the right state.
		/// </summary>
		protected void AllorsAssert()
		{
			if( isDeleted ) 
			{
				throw new System.Exception("Object of class "+ GetType().ToString() +" with id " + id + " has been deleted");
			}
		}

		/// <summary>
		/// Asserts that this Object and the related Object are in the right state.
		/// </summary>
		/// <param name="relatedObject">The related object.</param>
		protected void AllorsAssert(AllorsEmbeddedObject relatedObject)
		{
			AllorsAssert();
			if( relatedObject!=null && session!=relatedObject.AllorsSession )
			{
				throw new System.ArgumentException("Objects are from different populations");
			}
		}

		/// <summary>
		/// Asserts that this Object and the related Objects are in the right state.
		/// </summary>
		/// <param name="relatedObjects">The related objects.</param>
		protected void AllorsAssert(System.Collections.ICollection relatedObjects)
		{
			AllorsAssert();
			if(relatedObjects!=null)
			{
				foreach(AllorsEmbeddedObject obj in relatedObjects)
				{
					if( obj!=null && session!=obj.AllorsSession )
					{
						throw new System.ArgumentException("Objects are from different populations");
					}
				}
			}
		}

		public abstract System.Guid AllorsObjectTypeId {get;}

		public abstract void Delete();

		object AllorsInternal.GetRole(AllorsEmbeddedRelationType relation)
		{
			throw new System.NotImplementedException();
		}

		void AllorsInternal.SetRole(AllorsEmbeddedRelationType relation, object role)
		{
			throw new System.NotImplementedException();
		}


		public virtual global::System.Guid Id
		{
			get
			{ 
				AllorsAssert();
				return (global::System.Guid)_MetaObjectId;
			}

			set
			{
				AllorsAssert();
				RoleSetMetaObjectId(value);
			}
		}

		protected void RoleSetMetaObjectId(global::System.Guid role)
		{
			_MetaObjectId = role;
		}

		public virtual bool ExistId
		{
			get
			{
				return _MetaObjectId != null;
			}
		}

		public virtual void RemoveId()
		{
			_MetaObjectId = null;
		}

}
}