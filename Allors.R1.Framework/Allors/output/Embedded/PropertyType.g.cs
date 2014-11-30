namespace Allors.R1.Meta.AllorsGenerated
{
	internal interface AllorsInternalPropertyType : AllorsInternalOperandType, AllorsInternal
	{
	}

	public interface AllorsInterfacePropertyType :  AllorsEmbeddedObject 
	{
	}

	[System.Diagnostics.DebuggerNonUserCode]
	public abstract class AllorsClassPropertyType :  global::Allors.R1.Meta.OperandType,  AllorsInternalPropertyType , AllorsEmbeddedObject
	{


		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsClassPropertyType"/> class.
		/// </summary>
		/// <param name="session">The Allors Session.</param>
		/// <param name="id">The Allors Object Id.</param>
		protected AllorsClassPropertyType(AllorsEmbeddedSession session, System.Int32 id) : base(session,id){}

}
}