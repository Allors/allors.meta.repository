namespace Uml
{
	public interface PropertyType  : OperandType 
	{
	}
	public interface AssociationType  : PropertyType 
	{
		string AssignedPluralName{set;}

		ObjectType ObjectType{set;}

		bool IsMany{set;}

		ObjectType DerivedRootObjectTypes{set;}

		string DerivedRootName{set;}

		string AssignedSingularName{set;}

	}
	public interface MetaObject 
	{
		global::System.Guid? Id{set;}

	}
	public interface Domain  : MetaObject 
	{
		ObjectType DeclaredObjectTypes{set;}

		string Name{set;}

		MethodType DeclaredMethodTypes{set;}

		Domain DerivedSuperDomains{set;}

		Inheritance DeclaredInheritances{set;}

		Domain DirectSuperDomains{set;}

		Domain UnitDomain{set;}

		ObjectType DerivedUnitObjectTypes{set;}

		ObjectType DerivedCompositeObjectTypes{set;}

		RelationType DeclaredRelationTypes{set;}

		RelationType DerivedRelationTypes{set;}

		MethodType DerivedMethodTypes{set;}

		Inheritance DerivedInheritances{set;}

		ObjectType DerivedObjectTypes{set;}

	}
	public interface RoleType  : PropertyType 
	{
		ObjectType ObjectType{set;}

		ObjectType DerivedRootTypes{set;}

		int Scale{set;}

		int Precision{set;}

		int Size{set;}

		string DerivedHierarchyPluralName{set;}

		string DerivedHierarchySingularName{set;}

		string AssignedPluralName{set;}

		string DerivedRootName{set;}

		bool IsMany{set;}

		string AssignedSingularName{set;}

	}
	public interface OperandType  : MetaObject 
	{
	}
	public interface RelationType  : MetaObject 
	{
		bool IsDerived{set;}

		bool IsIndexed{set;}

		RoleType RoleType{set;}

		AssociationType AssociationType{set;}

	}
	public interface Inheritance  : MetaObject 
	{
		ObjectType Subtype{set;}

		ObjectType Supertype{set;}

	}
	public interface ObjectType  : MetaObject 
	{
		AssociationType DerivedExclusiveAssociationTypes{set;}

		ObjectType DerivedExclusiveSuperinterfaces{set;}

		ObjectType DerivedSubclasses{set;}

		AssociationType DerivedAssociationTypes{set;}

		ObjectType DerivedDirectSupertypes{set;}

		ObjectType DerivedDirectSuperclass{set;}

		string PluralName{set;}

		MethodType DerivedMethodTypes{set;}

		ObjectType DerivedSuperinterfaces{set;}

		bool IsInterface{set;}

		ObjectType DerivedSubinterfaces{set;}

		bool IsUnit{set;}

		ObjectType DerivedExclusiveConcreteLeafClass{set;}

		ObjectType DerivedDirectSuperinterfaces{set;}

		RoleType DerivedUnitRoleTypes{set;}

		ObjectType DerivedRootClasses{set;}

		int UnitTag{set;}

		RoleType DerivedCompositeRoleTypes{set;}

		ObjectType DerivedSuperclasses{set;}

		bool IsAbstract{set;}

		RoleType DerivedRoleTypes{set;}

		string SingularName{set;}

		RoleType DerivedExclusiveRoleTypes{set;}

		ObjectType DerivedSupertypes{set;}

	}
	public interface MethodType  : OperandType 
	{
		ObjectType ObjectType{set;}

		string Name{set;}

	}
}