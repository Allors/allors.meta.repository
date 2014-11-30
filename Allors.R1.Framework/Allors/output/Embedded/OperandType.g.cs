namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalOperandType : AllorsInternalMetaObject, AllorsInternal
	{
	}

	public interface AllorsInterfaceOperandType :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassOperandType :  global::Allors.R1.Meta.MetaObject,  AllorsInternalOperandType , AllorsEmbeddedObject
	{


		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassOperandType"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassOperandType(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

}
}