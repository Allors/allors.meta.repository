using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Allors.R1.Meta.AllorsGenerated
{
	/// <summary>
	/// AllorsObjects are objects that are held in an AllorsPopulation
	/// and their lifecycle is managed through an AllorsSession.
	/// </summary>
	public interface AllorsEmbeddedObject
	{
		AllorsEmbeddedSession AllorsSession { get; }

		int AllorsObjectId { get; }

		System.Guid AllorsObjectTypeId { get; }

		void Delete();

		bool IsDeleted { get; }
	}

	/// <summary>
	/// An AllorsSession is a transactional session that
	/// provides lifecycle management to objects in the population.
	/// Transactions are 'rolling'. After committing or rolling back
	/// a transaction an new transaction is automatically started,
	/// there is no need to manually 'begin' a transaction.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	public class AllorsEmbeddedSession 
	{
		/// <summary>
		/// The <see cref="currentId"/> field holds the highest id issued.
		/// </summary>
		System.Int32 currentId = 0;

		/// <summary>
		/// The <see cref="allorsObjects"/> field holds all of <see cref="AllorsEmbeddedObject"/>s in this poulation keyed by their ids.
		/// </summary>
		System.Collections.Hashtable allorsObjects = new System.Collections.Hashtable();

		/// <summary>
		/// The <see cref="state"/> field holds a Dictionary of user supplied key/value pairs.
		/// </summary>
		System.Collections.Hashtable state = new System.Collections.Hashtable();

		/// <summary>
		/// Session state are simple key/value pairs.
		/// Because AllorsObjects can not hold instance variables, this is the only way
		/// for AllorsObjects to hold references to Non-Allors objects or Allors objects
		/// from a different population.
		/// </summary>
		/// <value>The properties.</value>
		public object this[string name]
		{
		    get { return state[name]; }
		    set { state[name] = value; }
		}

		/// <summary>
		/// Commits all changes that where made during this transaction.
		/// Because transactions are rolling, a new transaction is automatically created.
		/// </summary>
		public void Commit()
		{
		}

		/// <summary>
		/// Rolls back all changes that where made during this transaction.
		/// Because transactions are rolling, a new transaction is automatically created.
		/// </summary>
		public void Rollback()
		{
			throw new System.Exception("Rollback not supported on Memory Profile");
		}

		internal void Delete( AllorsEmbeddedObject allorsObject ) 
		{
			allorsObjects.Remove(allorsObject.AllorsObjectId);
		}

		/// <summary>
		/// Creates an Extent for the specified ObjectType.
		/// </summary>
		/// <param name="objectType">The ObjectType.</param>
		/// <returns></returns>
		public AllorsEmbeddedObject[] Extent(AllorsEmbeddedObjectType objectType)
		{
			System.Collections.ArrayList extent = new System.Collections.ArrayList();
			foreach( System.Object allorsObject in allorsObjects.Values)
			{
				if( objectType.ConcreteClasses.Contains(allorsObject.GetType()) )
				{
					extent.Add(allorsObject);
				}
			}
			return extent.ToArray(objectType.Type) as AllorsEmbeddedObject[];
		}

		/// <summary>
		/// Creates an Extent for the whole population.
		/// </summary>
		/// <returns></returns>
		public AllorsEmbeddedObject[] Extent()
	    {
            ArrayList extent = new ArrayList(allorsObjects.Values);
            return extent.ToArray(typeof(AllorsEmbeddedObject)) as AllorsEmbeddedObject[];
        }

		/// <summary>
		/// Instantiates an Allors Object.
		/// </summary>
		/// <param name="objectId">The object id.</param>
		/// <returns></returns>
		public AllorsEmbeddedObject Instantiate( int objectId ) 
		{
			return (AllorsEmbeddedObject)allorsObjects[objectId];
		}

		/// <summary>
		/// Creates an Allors Object.
		/// </summary>
		/// <param name="objectType">The ObjectType.</param>
		/// <returns>a new <see cref="AllorsEmbeddedObject"/></returns>
		public AllorsEmbeddedObject Create(AllorsEmbeddedObjectType objectType) 
		{
			return Create(objectType, ++currentId );
		}

		/// <summary>
		/// Inserts an Allors Object with the specified id.
		/// </summary>
		/// <param name="objectType">The <see cref="AllorsEmbeddedObjectType"/>.</param>
		/// <param name="objectId">The object id.</param>
		/// <returns></returns>
		public AllorsEmbeddedObject Insert(AllorsEmbeddedObjectType objectType, int objectId) 
		{
		    if(allorsObjects.ContainsKey(objectId))
		    {
			throw new Exception("Object with id " + objectId + " already exists");
		    }
		    return Create(objectType, objectId);
		}

		/// <param name="type">The ObjectType.</param>
		/// <param name="id">The Object Id.</param>
		/// <returns>a new <see cref="AllorsEmbeddedObject"/></returns>
		AllorsEmbeddedObject Create(AllorsEmbeddedObjectType type, int id) 
		{
			if( id > currentId ) {
				currentId = id;
			}

			AllorsEmbeddedObject allorsObject = null;
			switch(type.Tag)
			{
				case AllorsTypeTags.AssociationType:
					allorsObject = new global::Allors.R1.Meta.AssociationType(this,id);
				break;
				case AllorsTypeTags.Domain:
					allorsObject = new global::Allors.R1.Meta.Domain(this,id);
				break;
				case AllorsTypeTags.RoleType:
					allorsObject = new global::Allors.R1.Meta.RoleType(this,id);
				break;
				case AllorsTypeTags.RelationType:
					allorsObject = new global::Allors.R1.Meta.RelationType(this,id);
				break;
				case AllorsTypeTags.Inheritance:
					allorsObject = new global::Allors.R1.Meta.Inheritance(this,id);
				break;
				case AllorsTypeTags.ObjectType:
					allorsObject = new global::Allors.R1.Meta.ObjectType(this,id);
				break;
				case AllorsTypeTags.MethodType:
					allorsObject = new global::Allors.R1.Meta.MethodType(this,id);
				break;

				default:
					throw new System.ArgumentException("Unkown type");
			}

			allorsObjects[allorsObject.AllorsObjectId] = allorsObject;
			return allorsObject;
		}

		/// <summary>
		/// Saves the population to the <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Save(XmlWriter writer)
		{
		    bool writeDocument = false;
		    if (writer.WriteState == WriteState.Start)
		    {
			writer.WriteStartDocument();
			writeDocument = true;
		    }
		    writer.WriteStartElement(AllorsEmbeddedXml.ALLORS);
		    writer.WriteAttributeString(AllorsEmbeddedXml.VERSION, AllorsEmbeddedXml.VERSION_CURRENT);

		    SavePopulation(writer);

		    writer.WriteEndElement();
		    if (writeDocument)
		    {
			writer.WriteEndDocument();
		    }
		}

		/// <param name="writer">The writer.</param>
		private void SavePopulation(XmlWriter writer)
		{
		    writer.WriteStartElement(AllorsEmbeddedXml.POPULATION);

		    Hashtable embeddedObjectsByObjectTypeId = new Hashtable();
		    foreach (AllorsEmbeddedObject embeddedObject in allorsObjects.Values)
		    {
			ArrayList strategies = (ArrayList)embeddedObjectsByObjectTypeId[embeddedObject.AllorsObjectTypeId];
			if (strategies == null)
			{
			    strategies = new ArrayList();
			}
			strategies.Add(embeddedObject);
			embeddedObjectsByObjectTypeId[embeddedObject.AllorsObjectTypeId] = strategies;
		    }

		    writer.WriteStartElement(AllorsEmbeddedXml.OBJECTS);
		    SaveObjects(writer, embeddedObjectsByObjectTypeId);
		    writer.WriteEndElement();

		    writer.WriteStartElement(AllorsEmbeddedXml.RELATIONS);
		    SaveRelations(writer, embeddedObjectsByObjectTypeId);
		    writer.WriteEndElement();

		    writer.WriteEndElement();
		}

		/// <param name="writer">The writer.</param>
		/// <param name="embeddedObjectsByObjectTypeId">The embedded objects by object type id.</param>
		private void SaveObjects(XmlWriter writer, Hashtable embeddedObjectsByObjectTypeId)
		{
		    foreach (System.Guid type in embeddedObjectsByObjectTypeId.Keys)
		    {
			ArrayList strategies = (ArrayList) embeddedObjectsByObjectTypeId[type];

			if(strategies.Count>0)
			{
			    writer.WriteStartElement(AllorsEmbeddedXml.OBJECT_TYPE);
			    writer.WriteAttributeString(AllorsEmbeddedXml.ID, type.ToString());

			    for (int i = 0; i < strategies.Count; i++)
			    {
				AllorsEmbeddedObject embeddedObject = (AllorsEmbeddedObject)strategies[i];
				if (i > 0)
				{
				    writer.WriteString(AllorsEmbeddedXml.OBJECTS_SPLITTER);
				}
				writer.WriteString(embeddedObject.AllorsObjectId.ToString());
			    }
			    writer.WriteEndElement();
			}
		    }
		}

		/// <param name="writer">The writer.</param>
		/// <param name="strategiesByObjectType">The strategies by object type.</param>
		private void SaveRelations(XmlWriter writer, Hashtable strategiesByObjectType)
		{
		    foreach (AllorsEmbeddedRelationType relationType in AllorsEmbeddedDomain.RelationByTag.Values)
		    {
			bool relationsExist = false;

			foreach (AllorsEmbeddedObjectType objectType in relationType.ConcreteClassHierarchy)
			{
			    ArrayList embeddedObjects = (ArrayList)strategiesByObjectType[objectType.Id];
			    if (embeddedObjects != null)
			    {
				foreach (AllorsEmbeddedObject embeddedObject in embeddedObjects)
				{
					object role = ((AllorsInternal)embeddedObject).GetRole(relationType);
					if(role!=null)
					{
						AllorsEmbeddedObject[] roles = role as AllorsEmbeddedObject[];
						if (roles != null && roles.Length == 0)
						{
							continue;
						}

						if(!relationsExist)
						{
							if(relationType.IsUnit)
							{
								writer.WriteStartElement(AllorsEmbeddedXml.RELATION_TYPE_UNIT);
							}
							else
							{
								writer.WriteStartElement(AllorsEmbeddedXml.RELATION_TYPE_COMPOSITE);
							}
							writer.WriteAttributeString(AllorsEmbeddedXml.ID, relationType.Id.ToString());
							relationsExist = true;
						}

						writer.WriteStartElement(AllorsEmbeddedXml.RELATION);
						writer.WriteAttributeString(AllorsEmbeddedXml.ASSOCIATION, XmlConvert.ToString(embeddedObject.AllorsObjectId));

						if (relationType.IsUnit)
						{
							switch (relationType.Type.Tag)
							{
								case AllorsTypeTags.AllorsString:
									writer.WriteString((string)role);
									break;
								case AllorsTypeTags.AllorsInteger:
								    writer.WriteString(XmlConvert.ToString((int)role));
								    break;
								case AllorsTypeTags.AllorsLong:
								    writer.WriteString(XmlConvert.ToString((long)role));
								    break;
								case AllorsTypeTags.AllorsDecimal:
								    writer.WriteString(XmlConvert.ToString((decimal)role));
								    break;
								case AllorsTypeTags.AllorsDouble:
								    writer.WriteString(XmlConvert.ToString((double)role));
								    break;
								case AllorsTypeTags.AllorsBoolean:
								    writer.WriteString(XmlConvert.ToString((bool)role));
								    break;
								case AllorsTypeTags.AllorsDate:
								    writer.WriteString(XmlConvert.ToString((System.DateTime)role, XmlDateTimeSerializationMode.Utc));
								    break;
								case AllorsTypeTags.AllorsDateTime:
								    writer.WriteString(XmlConvert.ToString((System.DateTime)role,XmlDateTimeSerializationMode.Utc));
								    break;
								case AllorsTypeTags.AllorsUnique:
								    writer.WriteString(XmlConvert.ToString((System.Guid)role));
								    break;
								case AllorsTypeTags.AllorsBinary:
								    writer.WriteString(Convert.ToBase64String((byte[])role));
								    break;
								default:
									throw new System.ArgumentException("Unknown Unit Type: " + relationType.Id.ToString());
							}
						}
						else
						{
						    if (relationType.IsMany)
						    {
							AllorsEmbeddedObject[] roleObjects = (AllorsEmbeddedObject[])role;

							bool first = true;
							foreach (AllorsEmbeddedObject roleObject in roleObjects)
							{
							    if (first)
							    {
								first = false;
							    }
							    else
							    {
								writer.WriteString(AllorsEmbeddedXml.XML_OBJECTS_SPLITTER_STRING);
							    }
							    writer.WriteString(roleObject.AllorsObjectId.ToString());
							}
						    }
						    else
						    {
							AllorsEmbeddedObject roleObject = (AllorsEmbeddedObject)role;
							writer.WriteString(roleObject.AllorsObjectId.ToString());
						    }
						}
						writer.WriteEndElement();
					}
				}
			    }
			}

			if (relationsExist)
			{
			    writer.WriteEndElement();
			}
		    }
		}

		/// <summary>
		/// Loads the population from the <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public void Load(XmlReader reader)
		{
				currentId = 0;
				allorsObjects = new System.Collections.Hashtable();
				state = new System.Collections.Hashtable();

		    while (reader.Read())
		    {
			if (reader.IsStartElement())
			{
			    if (reader.Name.Equals(AllorsEmbeddedXml.POPULATION))
			    {
				LoadRepository(reader);
			    }
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		private void LoadRepository(XmlReader reader)
		{
		    while (reader.Read())
		    {
			switch (reader.NodeType)
			{
			    case XmlNodeType.Element:
				if (reader.Name.Equals(AllorsEmbeddedXml.OBJECTS))
				{
				    if (!reader.IsEmptyElement)
				    {
					LoadObjects(reader);
				    }
				}
				else if (reader.Name.Equals(AllorsEmbeddedXml.RELATIONS))
				{
				    if (!reader.IsEmptyElement)
				    {
					LoadRelationTypes(reader);
				    }
				}
				break;
			    case XmlNodeType.EndElement:
				return;
			    default:
				// eat everything but elements
				break;
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		private void LoadObjects(XmlReader reader)
		{
		    while (reader.Read())
		    {
			switch (reader.NodeType)
			{
			    case XmlNodeType.Element:
				if (reader.Name.Equals(AllorsEmbeddedXml.OBJECT_TYPE))
				{
				    if (!reader.IsEmptyElement)
				    {
					LoadObjectType(reader);
				    }
				}
				break;
			    case XmlNodeType.EndElement:
				return;
			    default:
				// eat everything but elements
				break;
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		private void LoadObjectType(XmlReader reader)
		{
		    var objectTypeId = new System.Guid(reader.GetAttribute(AllorsEmbeddedXml.ID));
		    AllorsEmbeddedObjectType type = (AllorsEmbeddedObjectType) AllorsEmbeddedDomain.TypeById[objectTypeId];

		    string objectIdsString = reader.ReadString();
		    string[] objectIds = objectIdsString.Split(AllorsEmbeddedXml.OBJECTS_SPLITTER_CHAR_ARRAY);

		    for (int i = 0; i < objectIds.Length; i++)
		    {
			int objectId = Int32.Parse(objectIds[i]);

			if (type == null || !type.IsConcreteComposite)
			{
			    throw new System.ArgumentException("ObjectType not compatible" + objectTypeId.ToString());
			}
			else
			{
			    Create(type, objectId);
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		private void LoadRelationTypes(XmlReader reader)
		{
		    while (reader.Read())
		    {
			switch (reader.NodeType)
			{
			    case XmlNodeType.Element:
				if (reader.Name.Equals(AllorsEmbeddedXml.RELATION_TYPE_UNIT))
				{
				    if (!reader.IsEmptyElement)
				    {
					LoadRelations(reader,true);
				    }
				}
				else if (reader.Name.Equals(AllorsEmbeddedXml.RELATION_TYPE_COMPOSITE))
				{
				    if (!reader.IsEmptyElement)
				    {
					LoadRelations(reader,false);
				    }
				}
				break;
			    case XmlNodeType.EndElement:
				return;
			    default:
				// eat everything but elements
				break;
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		/// <param name="isUnit">if set to <c>true</c> the relation's Role has an Unit Object Type.</param>
		private void LoadRelations(XmlReader reader, bool isUnit)
		{
		    var relationTypeId = new System.Guid(reader.GetAttribute(AllorsEmbeddedXml.ID));
		    AllorsEmbeddedRelationType relation = (AllorsEmbeddedRelationType)AllorsEmbeddedDomain.RelationById[relationTypeId];

		    if (!reader.IsEmptyElement)
		    {
			if (relation == null ||
			    relation.IsUnit != isUnit)
			{
			    throw new System.ArgumentException("RelationType not compatible" + relationTypeId.ToString());
			}
			else
			{
			    if (relation.IsUnit)
			    {
				LoadUnitRelations(reader, relation);
			    }
			    else
			    {
				LoadCompositeRelations(reader, relation);
			    }
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		/// <param name="relation">The relation.</param>
		private void LoadUnitRelations(XmlReader reader, AllorsEmbeddedRelationType relation)
		{
		    while (reader.Read())
		    {
			switch (reader.NodeType)
			{
			    case XmlNodeType.Element:
				if (reader.Name.Equals(AllorsEmbeddedXml.RELATION))
				{
				    Int32 a = Int32.Parse(reader.GetAttribute(AllorsEmbeddedXml.ASSOCIATION));
				    AllorsEmbeddedObject strategy = (AllorsEmbeddedObject) allorsObjects[a];
				    string value = String.Empty;
				    if (!reader.IsEmptyElement)
				    {
					value = reader.ReadString();
				    }

				    if (strategy == null)
				    {
					throw new System.ArgumentException("Object not found" + a.ToString());
				    }
				    else
				    {
									AllorsInternal internalStrategy = (AllorsInternal)strategy;
					if (reader.IsEmptyElement)
					{
					    if (relation.Type.Tag == AllorsTypeTags.AllorsString)
					    {
						internalStrategy.SetRole(relation, String.Empty);
					    }
					}
					else
					{
					    try
					    {
						switch (relation.Type.Tag)
						{
						    case AllorsTypeTags.AllorsString:
							string parameter = value;
							internalStrategy.SetRole(relation, parameter);
							break;
						    case AllorsTypeTags.AllorsInteger:
							internalStrategy.SetRole(relation, XmlConvert.ToInt32(value));
							break;
						    case AllorsTypeTags.AllorsLong:
							internalStrategy.SetRole(relation, XmlConvert.ToInt64(value));
							break;
						    case AllorsTypeTags.AllorsDecimal:
							internalStrategy.SetRole(relation, XmlConvert.ToDecimal(value));
							break;
						    case AllorsTypeTags.AllorsDouble:
							internalStrategy.SetRole(relation, XmlConvert.ToDouble(value));
							break;
						    case AllorsTypeTags.AllorsBoolean:
							internalStrategy.SetRole(relation, XmlConvert.ToBoolean(value));
							break;
						    case AllorsTypeTags.AllorsDate:
							internalStrategy.SetRole(relation, XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc));
							break;
						    case AllorsTypeTags.AllorsDateTime:
							internalStrategy.SetRole(relation, XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc));
							break;
						    case AllorsTypeTags.AllorsUnique:
							internalStrategy.SetRole(relation, XmlConvert.ToGuid(value));
							break;
						    case AllorsTypeTags.AllorsBinary:
							internalStrategy.SetRole(relation, Convert.FromBase64String(value));
							break;
						    default:
							throw new System.ArgumentException("Unknown Unit Type: " + relation.Type.Id.ToString());
						}
					    }
					    catch
					    {
						throw new System.ArgumentException("Unknown Unit Type: " + relation.Type.Id.ToString());
					    }
					}
				    }
				}
				break;
			    case XmlNodeType.EndElement:
				return;
			    default:
				// eat everything but elements
				break;
			}
		    }
		}

		/// <param name="reader">The reader.</param>
		/// <param name="relation">The relation.</param>
		private void LoadCompositeRelations(XmlReader reader, AllorsEmbeddedRelationType relation)
		{
		    while (reader.Read())
		    {
			switch (reader.NodeType)
			{
			    case XmlNodeType.Element:
				if (reader.Name.Equals(AllorsEmbeddedXml.RELATION))
				{
				    Int32 a = Int32.Parse(reader.GetAttribute(AllorsEmbeddedXml.ASSOCIATION));
				    AllorsEmbeddedObject strategy = (AllorsEmbeddedObject) allorsObjects[a];

				    string value = String.Empty;
				    if (!reader.IsEmptyElement)
				    {
					value = reader.ReadString();
				    }

				    if (!reader.IsEmptyElement)
				    {
					string roleIdsString = value;
					string[] roleIds = roleIdsString.Split(AllorsEmbeddedXml.XML_OBJECTS_SPLITTER);

					if (strategy == null ||
					    (relation.IsOne && roleIds.Length != 1))
					{
					    throw new System.ArgumentException("Too many roles for relation " + relation.Type.Id.ToString());
					}
					else
					{
						AllorsInternal internalStrategy = (AllorsInternal)strategy;
					    if (relation.IsOne)
					    {
							int r = Int32.Parse(roleIds[0]);
							AllorsEmbeddedObject roleObject = (AllorsEmbeddedObject) allorsObjects[r];
							if (roleObject == null)
							{
								throw new System.ArgumentException("Object not found" + r.ToString());
							}
							else
							{
								internalStrategy.SetRole(relation, roleObject);
							}
							}
							else
							{
							AllorsEmbeddedObject[] roleObjects = (AllorsEmbeddedObject[]) Array.CreateInstance(relation.Type.Type,roleIds.Length);
							for (int i = 0; i < roleIds.Length; i++)
							{
													int r = Int32.Parse(roleIds[i]);
								AllorsEmbeddedObject roleObject = (AllorsEmbeddedObject) allorsObjects[r];
								if (roleObject == null)
								{
								throw new System.ArgumentException("Object not found" + r.ToString());
								}
								else
								{
								roleObjects[i] = roleObject;
								}
							}
							internalStrategy.SetRole(relation, roleObjects);
							}
						}
				    }
				}
				break;
			    case XmlNodeType.EndElement:
				return;
			    default:
				// eat everything but elements
				break;
			}
		    }
		}
	}

	/// <summary>
	/// An <see cref="AllorsEmbeddedObjectType"/> defines the state and behavior for
	/// a Set of <see cref="AllorsEmbeddedObject"/>s.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	public class AllorsEmbeddedObjectType
	{
		internal readonly Guid Id;

		internal readonly bool IsConcreteComposite;

		internal readonly int Tag;	

		internal readonly Type Type;

	    internal readonly System.Collections.Generic.List<Type> ConcreteClasses;

		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsEmbeddedObjectType"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="type">The type.</param>
		/// <param name="isConcreteComposite">if set to <c>true</c> then this type is a Concrete Composite Type.</param>
		internal AllorsEmbeddedObjectType(Guid id, Type type, bool isConcreteComposite) : this(id,type,isConcreteComposite,-1){}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsEmbeddedObjectType"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="type">The type.</param>
		/// <param name="isConcreteComposite">if set to <c>true</c> then this type is a Concrete Composite Type.</param>
		/// <param name="tag">The tag.</param>
		internal AllorsEmbeddedObjectType(Guid id, Type type, bool isConcreteComposite, int tag)
		{
			this.Id = id;
			this.IsConcreteComposite = isConcreteComposite;
			this.Tag = tag;
			this.Type = type;

			this.ConcreteClasses = new System.Collections.Generic.List<Type>();
		}
	}

	/// <summary>
	/// An <see cref="AllorsEmbeddedRelationType"/> defines the state and behavior for
	/// a Set of relations.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	public class AllorsEmbeddedRelationType
	{
		private static AllorsEmbeddedObjectType[] EmptyEmbeddedObjectTypeArray = new AllorsEmbeddedObjectType[0];

		internal readonly Guid Id;

		internal readonly int Tag;

		internal readonly string Name;

		internal readonly AllorsEmbeddedObjectType Type;

		internal readonly bool IsUnit;

		internal readonly bool IsMany;

		internal readonly ArrayList ConcreteClassHierarchy;

		/// <summary>
		/// Initializes a new instance of the <see cref="AllorsEmbeddedRelationType"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="tag">The tag.</param>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <param name="IsUnit">if set to <c>true</c> the Role's Object Type is a Unit.</param>
		/// <param name="isMany">if set to <c>true</c> the Role's multiplicity is Many.</param>
		internal AllorsEmbeddedRelationType(Guid id, int tag, string name, AllorsEmbeddedObjectType type, bool IsUnit, bool isMany)
		{
			this.Id = id;
			this.Tag = tag;
			this.Name = name;
			this.Type = type;
			this.IsUnit = IsUnit;
			this.IsMany = isMany;
			this.ConcreteClassHierarchy = new ArrayList(1);
		}

		/// <summary>
		/// Gets a value indicating whether this relation has a Role with multiplicity one.
		/// </summary>
		/// <value><c>true</c> if this instance has a Role with multiplicity one; otherwise, <c>false</c>.</value>
		internal bool IsOne
		{
			get
			{
				return !IsMany;
			}
		}

	}

	/// <summary>
	/// An AllorsEmbeddedDomain groups related <see cref="AllorsEmbeddedRelationType"/>s and
	/// <see cref="AllorsEmbeddedObjectType"/>s
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	public class AllorsEmbeddedDomain
	{
		internal static readonly Hashtable RelationByTag;

		internal static readonly Hashtable RelationById;

		internal static readonly Hashtable TypeById;

		/// <summary>
		/// Lookups the relation by tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		public static AllorsEmbeddedRelationType LookupRelationByTag(string tag)
		{
		    return (AllorsEmbeddedRelationType)RelationByTag[tag];
		}

		/// <summary>
		/// Lookups the relation by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static AllorsEmbeddedRelationType LookupRelationById(Guid id)
		{
		    return (AllorsEmbeddedRelationType)RelationById[id];
		}

		/// <summary>
		/// Lookups the type by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static AllorsEmbeddedObjectType LookupTypeById(Guid id)
		{
		    return (AllorsEmbeddedObjectType) TypeById[id];
		}

		#region Types
		/// <summary>
		/// The PropertyType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType PropertyType = new AllorsEmbeddedObjectType(new Guid("47bb2aad-ad9e-4789-82b4-c9594585b3da"), typeof(global::Allors.R1.Meta.PropertyType), false , AllorsTypeTags.PropertyType );

		/// <summary>
		/// The AssociationType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AssociationType = new AllorsEmbeddedObjectType(new Guid("51df648e-4870-403a-8b2e-50f6e63d4749"), typeof(global::Allors.R1.Meta.AssociationType), true , AllorsTypeTags.AssociationType );

		/// <summary>
		/// The MetaObject Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType MetaObject = new AllorsEmbeddedObjectType(new Guid("7794a50c-a2ba-45cb-a8a7-9e8091d313cb"), typeof(global::Allors.R1.Meta.MetaObject), false , AllorsTypeTags.MetaObject );

		/// <summary>
		/// The Domain Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType Domain = new AllorsEmbeddedObjectType(new Guid("804929af-0208-4384-a74d-17353963d105"), typeof(global::Allors.R1.Meta.Domain), true , AllorsTypeTags.Domain );

		/// <summary>
		/// The RoleType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType RoleType = new AllorsEmbeddedObjectType(new Guid("903d3eb5-dc70-4cb4-93b8-5a1d0899c949"), typeof(global::Allors.R1.Meta.RoleType), true , AllorsTypeTags.RoleType );

		/// <summary>
		/// The OperandType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType OperandType = new AllorsEmbeddedObjectType(new Guid("97cd4cf4-1945-4b60-aa3b-f8629e6074da"), typeof(global::Allors.R1.Meta.OperandType), false , AllorsTypeTags.OperandType );

		/// <summary>
		/// The RelationType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType RelationType = new AllorsEmbeddedObjectType(new Guid("c03575fa-2d84-4096-9c4d-93cf05d3c1de"), typeof(global::Allors.R1.Meta.RelationType), true , AllorsTypeTags.RelationType );

		/// <summary>
		/// The Inheritance Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType Inheritance = new AllorsEmbeddedObjectType(new Guid("ceb95f73-a297-48cc-85bc-92efa5954efc"), typeof(global::Allors.R1.Meta.Inheritance), true , AllorsTypeTags.Inheritance );

		/// <summary>
		/// The ObjectType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType ObjectType = new AllorsEmbeddedObjectType(new Guid("e6270568-a164-40ef-bf2b-cfdff59ee1fa"), typeof(global::Allors.R1.Meta.ObjectType), true , AllorsTypeTags.ObjectType );

		/// <summary>
		/// The MethodType Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType MethodType = new AllorsEmbeddedObjectType(new Guid("edf4fdb9-9fc9-4914-bd67-c781f9199f98"), typeof(global::Allors.R1.Meta.MethodType), true , AllorsTypeTags.MethodType );

		/// <summary>
		/// The AllorsString Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsString = new AllorsEmbeddedObjectType(new Guid("ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9"), typeof(global::System.String), false , AllorsTypeTags.AllorsString );

		/// <summary>
		/// The AllorsInteger Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsInteger = new AllorsEmbeddedObjectType(new Guid("ccd6f134-26de-4103-bff9-a37ec3e997a3"), typeof(global::System.Int32), false , AllorsTypeTags.AllorsInteger );

		/// <summary>
		/// The AllorsLong Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsLong = new AllorsEmbeddedObjectType(new Guid("e8989069-024b-4389-ac77-a98c4dfff25a"), typeof(global::System.Int64), false , AllorsTypeTags.AllorsLong );

		/// <summary>
		/// The AllorsDecimal Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsDecimal = new AllorsEmbeddedObjectType(new Guid("da866d8e-2c40-41a8-ae5b-5f6dae0b89c8"), typeof(global::System.Decimal), false , AllorsTypeTags.AllorsDecimal );

		/// <summary>
		/// The AllorsDouble Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsDouble = new AllorsEmbeddedObjectType(new Guid("ffcabd07-f35f-4083-bef6-f6c47970ca5d"), typeof(global::System.Double), false , AllorsTypeTags.AllorsDouble );

		/// <summary>
		/// The AllorsBoolean Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsBoolean = new AllorsEmbeddedObjectType(new Guid("b5ee6cea-4e2b-498e-a5dd-24671d896477"), typeof(global::System.Boolean), false , AllorsTypeTags.AllorsBoolean );

		/// <summary>
		/// The AllorsDateTime Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsDateTime = new AllorsEmbeddedObjectType(new Guid("c4c09343-61d3-418c-ade2-fe6fd588f128"), typeof(global::Allors.R1.Meta.AllorsDateTime), false , AllorsTypeTags.AllorsDateTime );

		/// <summary>
		/// The AllorsUnique Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsUnique = new AllorsEmbeddedObjectType(new Guid("6dc0a1a8-88a4-4614-adb4-92dd3d017c0e"), typeof(global::System.Guid), false , AllorsTypeTags.AllorsUnique );

		/// <summary>
		/// The AllorsBinary Object Type.
		/// </summary>
		public static AllorsEmbeddedObjectType AllorsBinary = new AllorsEmbeddedObjectType(new Guid("c28e515b-cae8-4d6b-95bf-062aec8042fc"), typeof(global::System.Byte[]), false , AllorsTypeTags.AllorsBinary );


		#endregion

		#region Relations
		/// <summary>
		/// The ObjectTypeDerivedExclusiveAssociationType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedExclusiveAssociationType = new AllorsEmbeddedRelationType(new Guid("002d425c-697b-4da4-84b1-533f16b8bcf5"),AllorsRelationTags.ObjectTypeDerivedExclusiveAssociationType,"DerivedExclusiveAssociationType", AssociationType, false, true);

		/// <summary>
		/// The DomainDeclaredObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDeclaredObjectType = new AllorsEmbeddedRelationType(new Guid("01186644-8f73-4056-9931-e1326788b4f1"),AllorsRelationTags.DomainDeclaredObjectType,"DeclaredObjectType", ObjectType, false, true);

		/// <summary>
		/// The InheritanceSubtype Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType InheritanceSubtype = new AllorsEmbeddedRelationType(new Guid("032fea21-491c-428a-adb1-0fbb2c067db7"),AllorsRelationTags.InheritanceSubtype,"Subtype", ObjectType, false, false);

		/// <summary>
		/// The DomainName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainName = new AllorsEmbeddedRelationType(new Guid("0415b007-0f3b-4593-9e4d-13fa8f9c7e67"),AllorsRelationTags.DomainName,"Name", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeDerivedExclusiveSuperinterface Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedExclusiveSuperinterface = new AllorsEmbeddedRelationType(new Guid("072f9860-40ff-4287-b39e-f8b13c143a80"),AllorsRelationTags.ObjectTypeDerivedExclusiveSuperinterface,"DerivedExclusiveSuperinterface", ObjectType, false, true);

		/// <summary>
		/// The AssociationTypeAssignedPluralName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeAssignedPluralName = new AllorsEmbeddedRelationType(new Guid("0c2e3f09-716b-4aec-b6bc-b8a46422f2f3"),AllorsRelationTags.AssociationTypeAssignedPluralName,"AssignedPluralName", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeDerivedSubclass Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedSubclass = new AllorsEmbeddedRelationType(new Guid("0fc8ba5e-47aa-4ed0-85cc-6bba1c4a82f1"),AllorsRelationTags.ObjectTypeDerivedSubclass,"DerivedSubclass", ObjectType, false, true);

		/// <summary>
		/// The DomainDeclaredMethodType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDeclaredMethodType = new AllorsEmbeddedRelationType(new Guid("1645bace-7a7c-4e62-965d-ec4c2b870fc1"),AllorsRelationTags.DomainDeclaredMethodType,"DeclaredMethodType", MethodType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedAssociationType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedAssociationType = new AllorsEmbeddedRelationType(new Guid("16ce019a-3301-420c-934c-b63de44045dc"),AllorsRelationTags.ObjectTypeDerivedAssociationType,"DerivedAssociationType", AssociationType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedDirectSupertype Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedDirectSupertype = new AllorsEmbeddedRelationType(new Guid("1c188575-35f1-4942-b97c-3a3d8b0b5730"),AllorsRelationTags.ObjectTypeDerivedDirectSupertype,"DerivedDirectSupertype", ObjectType, false, true);

		/// <summary>
		/// The AssociationTypeObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeObjectType = new AllorsEmbeddedRelationType(new Guid("1cffe950-1066-432d-8782-64d40838dab2"),AllorsRelationTags.AssociationTypeObjectType,"ObjectType", ObjectType, false, false);

		/// <summary>
		/// The MetaObjectId Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType MetaObjectId = new AllorsEmbeddedRelationType(new Guid("206900d8-c8a9-4b97-8577-a5f6e2e6a2d4"),AllorsRelationTags.MetaObjectId,"Id", AllorsUnique, true, false);

		/// <summary>
		/// The DomainDerivedSuperDomain Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedSuperDomain = new AllorsEmbeddedRelationType(new Guid("24a5b0cc-30aa-4124-a907-6bc382987ee3"),AllorsRelationTags.DomainDerivedSuperDomain,"DerivedSuperDomain", Domain, false, true);

		/// <summary>
		/// The ObjectTypeDerivedDirectSuperclass Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedDirectSuperclass = new AllorsEmbeddedRelationType(new Guid("26fd946a-1442-4330-9305-2498f7f14d86"),AllorsRelationTags.ObjectTypeDerivedDirectSuperclass,"DerivedDirectSuperclass", ObjectType, false, false);

		/// <summary>
		/// The ObjectTypePluralName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypePluralName = new AllorsEmbeddedRelationType(new Guid("2fdc2297-0c58-4adf-ba8f-20a67dc774ee"),AllorsRelationTags.ObjectTypePluralName,"PluralName", AllorsString, true, false);

		/// <summary>
		/// The RoleTypeObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeObjectType = new AllorsEmbeddedRelationType(new Guid("2fe22b85-c763-4367-9a56-7b67c73b6415"),AllorsRelationTags.RoleTypeObjectType,"ObjectType", ObjectType, false, false);

		/// <summary>
		/// The RoleTypeDerivedRootType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeDerivedRootType = new AllorsEmbeddedRelationType(new Guid("380a9a6d-22e1-4751-98bb-c8aa002391c9"),AllorsRelationTags.RoleTypeDerivedRootType,"DerivedRootType", ObjectType, false, true);

		/// <summary>
		/// The RelationTypeIsDerived Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RelationTypeIsDerived = new AllorsEmbeddedRelationType(new Guid("3fe64969-00d4-45a5-bc50-e833a8eb0f4f"),AllorsRelationTags.RelationTypeIsDerived,"IsDerived", AllorsBoolean, true, false);

		/// <summary>
		/// The AssociationTypeIsMany Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeIsMany = new AllorsEmbeddedRelationType(new Guid("41435c99-5531-411c-b2f1-aadf5ebe07e2"),AllorsRelationTags.AssociationTypeIsMany,"IsMany", AllorsBoolean, true, false);

		/// <summary>
		/// The RoleTypeScale Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeScale = new AllorsEmbeddedRelationType(new Guid("4562f0c5-d6fd-44c4-83f4-50b322d6be86"),AllorsRelationTags.RoleTypeScale,"Scale", AllorsInteger, true, false);

		/// <summary>
		/// The ObjectTypeDerivedMethodType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedMethodType = new AllorsEmbeddedRelationType(new Guid("4883b1bb-ce26-4417-b895-1c924cc5beb6"),AllorsRelationTags.ObjectTypeDerivedMethodType,"DerivedMethodType", MethodType, false, true);

		/// <summary>
		/// The RoleTypePrecision Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypePrecision = new AllorsEmbeddedRelationType(new Guid("492bf4cc-6f36-4d40-888f-f231746c56a2"),AllorsRelationTags.RoleTypePrecision,"Precision", AllorsInteger, true, false);

		/// <summary>
		/// The DomainDeclaredInheritance Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDeclaredInheritance = new AllorsEmbeddedRelationType(new Guid("535ee922-1c44-4d24-969c-439dd4c2501c"),AllorsRelationTags.DomainDeclaredInheritance,"DeclaredInheritance", Inheritance, false, true);

		/// <summary>
		/// The DomainDirectSuperDomain Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDirectSuperDomain = new AllorsEmbeddedRelationType(new Guid("58a4747e-c80f-4034-984a-e633ccf74436"),AllorsRelationTags.DomainDirectSuperDomain,"DirectSuperDomain", Domain, false, true);

		/// <summary>
		/// The RoleTypeSize Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeSize = new AllorsEmbeddedRelationType(new Guid("59b7d108-99d4-4c58-b921-e39e8fafb5d3"),AllorsRelationTags.RoleTypeSize,"Size", AllorsInteger, true, false);

		/// <summary>
		/// The DomainUnitDomain Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainUnitDomain = new AllorsEmbeddedRelationType(new Guid("5b037e20-b168-4fd1-8b93-4faeb487b4d9"),AllorsRelationTags.DomainUnitDomain,"UnitDomain", Domain, false, false);

		/// <summary>
		/// The DomainDerivedUnitObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedUnitObjectType = new AllorsEmbeddedRelationType(new Guid("6aadc304-2860-452b-8dd9-7ac298240e53"),AllorsRelationTags.DomainDerivedUnitObjectType,"DerivedUnitObjectType", ObjectType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedSuperinterface Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedSuperinterface = new AllorsEmbeddedRelationType(new Guid("6c3036b1-8719-4d8d-876e-fcd2af160c1c"),AllorsRelationTags.ObjectTypeDerivedSuperinterface,"DerivedSuperinterface", ObjectType, false, true);

		/// <summary>
		/// The DomainDerivedCompositeObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedCompositeObjectType = new AllorsEmbeddedRelationType(new Guid("6f9a6958-0cff-40ff-b0cc-441f4ceec07b"),AllorsRelationTags.DomainDerivedCompositeObjectType,"DerivedCompositeObjectType", ObjectType, false, true);

		/// <summary>
		/// The RoleTypeDerivedHierarchyPluralName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeDerivedHierarchyPluralName = new AllorsEmbeddedRelationType(new Guid("7a1cb848-b2ce-49ff-b4b1-56c484026a50"),AllorsRelationTags.RoleTypeDerivedHierarchyPluralName,"DerivedHierarchyPluralName", AllorsString, true, false);

		/// <summary>
		/// The RoleTypeDerivedHierarchySingularName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeDerivedHierarchySingularName = new AllorsEmbeddedRelationType(new Guid("85463ed8-8337-404e-9b30-bbccfe4f17d0"),AllorsRelationTags.RoleTypeDerivedHierarchySingularName,"DerivedHierarchySingularName", AllorsString, true, false);

		/// <summary>
		/// The RoleTypeAssignedPluralName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeAssignedPluralName = new AllorsEmbeddedRelationType(new Guid("86f8b95c-3754-4f06-83c5-da190986f180"),AllorsRelationTags.RoleTypeAssignedPluralName,"AssignedPluralName", AllorsString, true, false);

		/// <summary>
		/// The RoleTypeDerivedRootName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeDerivedRootName = new AllorsEmbeddedRelationType(new Guid("8bbf6411-3063-4434-b2d0-c7c3f586915f"),AllorsRelationTags.RoleTypeDerivedRootName,"DerivedRootName", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeIsInterface Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeIsInterface = new AllorsEmbeddedRelationType(new Guid("8e55c4f5-61f6-448b-b351-942ae4a695a6"),AllorsRelationTags.ObjectTypeIsInterface,"IsInterface", AllorsBoolean, true, false);

		/// <summary>
		/// The ObjectTypeDerivedSubinterface Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedSubinterface = new AllorsEmbeddedRelationType(new Guid("9a6ea1b5-e49c-4bb3-b3b7-044b5508de6d"),AllorsRelationTags.ObjectTypeDerivedSubinterface,"DerivedSubinterface", ObjectType, false, true);

		/// <summary>
		/// The ObjectTypeIsUnit Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeIsUnit = new AllorsEmbeddedRelationType(new Guid("a02f806c-248d-47f6-a963-9022ba2b6543"),AllorsRelationTags.ObjectTypeIsUnit,"IsUnit", AllorsBoolean, true, false);

		/// <summary>
		/// The ObjectTypeDerivedExclusiveConcreteLeafClass Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedExclusiveConcreteLeafClass = new AllorsEmbeddedRelationType(new Guid("a03c2b32-1ce5-4534-be3e-7914c2a69a7f"),AllorsRelationTags.ObjectTypeDerivedExclusiveConcreteLeafClass,"DerivedExclusiveConcreteLeafClass", ObjectType, false, false);

		/// <summary>
		/// The AssociationTypeDerivedRootObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeDerivedRootObjectType = new AllorsEmbeddedRelationType(new Guid("a4dd730f-adea-4fff-b0c4-41879b1523ab"),AllorsRelationTags.AssociationTypeDerivedRootObjectType,"DerivedRootObjectType", ObjectType, false, true);

		/// <summary>
		/// The AssociationTypeDerivedRootName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeDerivedRootName = new AllorsEmbeddedRelationType(new Guid("a5bbc0a7-cfb0-45bf-b7aa-ef1e8d66eb39"),AllorsRelationTags.AssociationTypeDerivedRootName,"DerivedRootName", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeDerivedDirectSuperinterface Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedDirectSuperinterface = new AllorsEmbeddedRelationType(new Guid("a6e4c5b3-deef-41bb-8de1-1842cbe2bd94"),AllorsRelationTags.ObjectTypeDerivedDirectSuperinterface,"DerivedDirectSuperinterface", ObjectType, false, true);

		/// <summary>
		/// The MethodTypeObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType MethodTypeObjectType = new AllorsEmbeddedRelationType(new Guid("abe24993-88b8-482d-ba31-650554089e96"),AllorsRelationTags.MethodTypeObjectType,"ObjectType", ObjectType, false, false);

		/// <summary>
		/// The MethodTypeName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType MethodTypeName = new AllorsEmbeddedRelationType(new Guid("b0cd2e7f-748c-4cdd-a6ac-22515950d212"),AllorsRelationTags.MethodTypeName,"Name", AllorsString, true, false);

		/// <summary>
		/// The RelationTypeIsIndexed Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RelationTypeIsIndexed = new AllorsEmbeddedRelationType(new Guid("b8def1db-3539-4973-a896-389725af87e3"),AllorsRelationTags.RelationTypeIsIndexed,"IsIndexed", AllorsBoolean, true, false);

		/// <summary>
		/// The DomainDeclaredRelationType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDeclaredRelationType = new AllorsEmbeddedRelationType(new Guid("bdb941ff-f2f9-4718-ad81-2df914c9d471"),AllorsRelationTags.DomainDeclaredRelationType,"DeclaredRelationType", RelationType, false, true);

		/// <summary>
		/// The RoleTypeIsMany Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeIsMany = new AllorsEmbeddedRelationType(new Guid("bf7466a1-f0c0-429a-9a23-e9eccd89abfa"),AllorsRelationTags.RoleTypeIsMany,"IsMany", AllorsBoolean, true, false);

		/// <summary>
		/// The ObjectTypeDerivedUnitRoleType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedUnitRoleType = new AllorsEmbeddedRelationType(new Guid("c48c14a5-cd3e-4c80-8e6b-14334b630504"),AllorsRelationTags.ObjectTypeDerivedUnitRoleType,"DerivedUnitRoleType", RoleType, false, true);

		/// <summary>
		/// The DomainDerivedRelationType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedRelationType = new AllorsEmbeddedRelationType(new Guid("c918dd43-da9e-4f99-8fbb-413c7982ed9c"),AllorsRelationTags.DomainDerivedRelationType,"DerivedRelationType", RelationType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedRootClass Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedRootClass = new AllorsEmbeddedRelationType(new Guid("ccc7763e-bc22-41e4-869f-b9fab9a86c6c"),AllorsRelationTags.ObjectTypeDerivedRootClass,"DerivedRootClass", ObjectType, false, true);

		/// <summary>
		/// The DomainDerivedMethodType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedMethodType = new AllorsEmbeddedRelationType(new Guid("cefb06c8-cf2d-4ffd-96bf-da0401b8ac26"),AllorsRelationTags.DomainDerivedMethodType,"DerivedMethodType", MethodType, false, true);

		/// <summary>
		/// The DomainDerivedInheritance Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedInheritance = new AllorsEmbeddedRelationType(new Guid("d29ab381-6c53-439d-9c1d-a9d15a07cd3b"),AllorsRelationTags.DomainDerivedInheritance,"DerivedInheritance", Inheritance, false, true);

		/// <summary>
		/// The RoleTypeAssignedSingularName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RoleTypeAssignedSingularName = new AllorsEmbeddedRelationType(new Guid("d65f89e9-0eda-462f-b0f8-50e95419f2bb"),AllorsRelationTags.RoleTypeAssignedSingularName,"AssignedSingularName", AllorsString, true, false);

		/// <summary>
		/// The RelationTypeRoleType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RelationTypeRoleType = new AllorsEmbeddedRelationType(new Guid("d69428eb-0df9-414f-a453-d1568b39d7f3"),AllorsRelationTags.RelationTypeRoleType,"RoleType", RoleType, false, false);

		/// <summary>
		/// The ObjectTypeUnitTag Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeUnitTag = new AllorsEmbeddedRelationType(new Guid("d855e865-0a24-447f-8808-4608c013034f"),AllorsRelationTags.ObjectTypeUnitTag,"UnitTag", AllorsInteger, true, false);

		/// <summary>
		/// The ObjectTypeDerivedCompositeRoleType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedCompositeRoleType = new AllorsEmbeddedRelationType(new Guid("d9ad42bf-af1d-467a-b10e-aee692421fec"),AllorsRelationTags.ObjectTypeDerivedCompositeRoleType,"DerivedCompositeRoleType", RoleType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedSuperclass Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedSuperclass = new AllorsEmbeddedRelationType(new Guid("dad9ec8c-b723-4b6e-9972-15c14c340e52"),AllorsRelationTags.ObjectTypeDerivedSuperclass,"DerivedSuperclass", ObjectType, false, true);

		/// <summary>
		/// The ObjectTypeIsAbstract Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeIsAbstract = new AllorsEmbeddedRelationType(new Guid("dc85d5e7-7071-4380-a597-3a16c554f653"),AllorsRelationTags.ObjectTypeIsAbstract,"IsAbstract", AllorsBoolean, true, false);

		/// <summary>
		/// The AssociationTypeAssignedSingularName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType AssociationTypeAssignedSingularName = new AllorsEmbeddedRelationType(new Guid("de038faa-da98-4550-bfff-e281f7955591"),AllorsRelationTags.AssociationTypeAssignedSingularName,"AssignedSingularName", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeDerivedRoleType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedRoleType = new AllorsEmbeddedRelationType(new Guid("e0f76c24-20d3-41b2-9671-341aeb87c1dd"),AllorsRelationTags.ObjectTypeDerivedRoleType,"DerivedRoleType", RoleType, false, true);

		/// <summary>
		/// The DomainDerivedObjectType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType DomainDerivedObjectType = new AllorsEmbeddedRelationType(new Guid("e16b5b92-eaaf-47d2-8eb5-4343b55f3eca"),AllorsRelationTags.DomainDerivedObjectType,"DerivedObjectType", ObjectType, false, true);

		/// <summary>
		/// The InheritanceSupertype Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType InheritanceSupertype = new AllorsEmbeddedRelationType(new Guid("e171ff1e-5b6b-468f-88c5-87fca1b1e898"),AllorsRelationTags.InheritanceSupertype,"Supertype", ObjectType, false, false);

		/// <summary>
		/// The ObjectTypeSingularName Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeSingularName = new AllorsEmbeddedRelationType(new Guid("e242bb45-ad6a-4542-832c-704050561268"),AllorsRelationTags.ObjectTypeSingularName,"SingularName", AllorsString, true, false);

		/// <summary>
		/// The ObjectTypeDerivedExclusiveRoleType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedExclusiveRoleType = new AllorsEmbeddedRelationType(new Guid("e9505b5e-ae67-48e1-a5b7-b8490a0bb2ca"),AllorsRelationTags.ObjectTypeDerivedExclusiveRoleType,"DerivedExclusiveRoleType", RoleType, false, true);

		/// <summary>
		/// The ObjectTypeDerivedSupertype Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType ObjectTypeDerivedSupertype = new AllorsEmbeddedRelationType(new Guid("fbe0443b-db7c-4fdd-b442-155b8ebfe00b"),AllorsRelationTags.ObjectTypeDerivedSupertype,"DerivedSupertype", ObjectType, false, true);

		/// <summary>
		/// The RelationTypeAssociationType Relation Type.
		/// </summary>
		public static AllorsEmbeddedRelationType RelationTypeAssociationType = new AllorsEmbeddedRelationType(new Guid("ff912320-76f6-4e5c-a18d-cd54ebb59b71"),AllorsRelationTags.RelationTypeAssociationType,"AssociationType", AssociationType, false, false);


		#endregion

		/// <summary>
		/// Initializes the <see cref="AllorsEmbeddedDomain"/> class.
		/// </summary>
		static AllorsEmbeddedDomain()
		{
			RelationByTag = new Hashtable();
			RelationById = new Hashtable();
			TypeById = new Hashtable();
			#region Types
			TypeById[PropertyType.Id] = PropertyType;
			TypeById[AssociationType.Id] = AssociationType;
			TypeById[MetaObject.Id] = MetaObject;
			TypeById[Domain.Id] = Domain;
			TypeById[RoleType.Id] = RoleType;
			TypeById[OperandType.Id] = OperandType;
			TypeById[RelationType.Id] = RelationType;
			TypeById[Inheritance.Id] = Inheritance;
			TypeById[ObjectType.Id] = ObjectType;
			TypeById[MethodType.Id] = MethodType;
			TypeById[AllorsString.Id] = AllorsString;
			TypeById[AllorsInteger.Id] = AllorsInteger;
			TypeById[AllorsLong.Id] = AllorsLong;
			TypeById[AllorsDecimal.Id] = AllorsDecimal;
			TypeById[AllorsDouble.Id] = AllorsDouble;
			TypeById[AllorsBoolean.Id] = AllorsBoolean;
			TypeById[AllorsDateTime.Id] = AllorsDateTime;
			TypeById[AllorsUnique.Id] = AllorsUnique;
			TypeById[AllorsBinary.Id] = AllorsBinary;


		PropertyType.ConcreteClasses.Add(AssociationType.Type);
		PropertyType.ConcreteClasses.Add(RoleType.Type);

		AssociationType.ConcreteClasses.Add(AssociationType.Type);

		MetaObject.ConcreteClasses.Add(AssociationType.Type);
		MetaObject.ConcreteClasses.Add(Domain.Type);
		MetaObject.ConcreteClasses.Add(RoleType.Type);
		MetaObject.ConcreteClasses.Add(RelationType.Type);
		MetaObject.ConcreteClasses.Add(Inheritance.Type);
		MetaObject.ConcreteClasses.Add(ObjectType.Type);
		MetaObject.ConcreteClasses.Add(MethodType.Type);

		Domain.ConcreteClasses.Add(Domain.Type);

		RoleType.ConcreteClasses.Add(RoleType.Type);

		OperandType.ConcreteClasses.Add(AssociationType.Type);
		OperandType.ConcreteClasses.Add(RoleType.Type);
		OperandType.ConcreteClasses.Add(MethodType.Type);

		RelationType.ConcreteClasses.Add(RelationType.Type);

		Inheritance.ConcreteClasses.Add(Inheritance.Type);

		ObjectType.ConcreteClasses.Add(ObjectType.Type);

		MethodType.ConcreteClasses.Add(MethodType.Type);

			#endregion

			#region Relations
			RelationByTag[ObjectTypeDerivedExclusiveAssociationType.Tag] = ObjectTypeDerivedExclusiveAssociationType;
						RelationById[ObjectTypeDerivedExclusiveAssociationType.Id] = ObjectTypeDerivedExclusiveAssociationType;
			RelationByTag[DomainDeclaredObjectType.Tag] = DomainDeclaredObjectType;
						RelationById[DomainDeclaredObjectType.Id] = DomainDeclaredObjectType;
			RelationByTag[InheritanceSubtype.Tag] = InheritanceSubtype;
						RelationById[InheritanceSubtype.Id] = InheritanceSubtype;
			RelationByTag[DomainName.Tag] = DomainName;
						RelationById[DomainName.Id] = DomainName;
			RelationByTag[ObjectTypeDerivedExclusiveSuperinterface.Tag] = ObjectTypeDerivedExclusiveSuperinterface;
						RelationById[ObjectTypeDerivedExclusiveSuperinterface.Id] = ObjectTypeDerivedExclusiveSuperinterface;
			RelationByTag[AssociationTypeAssignedPluralName.Tag] = AssociationTypeAssignedPluralName;
						RelationById[AssociationTypeAssignedPluralName.Id] = AssociationTypeAssignedPluralName;
			RelationByTag[ObjectTypeDerivedSubclass.Tag] = ObjectTypeDerivedSubclass;
						RelationById[ObjectTypeDerivedSubclass.Id] = ObjectTypeDerivedSubclass;
			RelationByTag[DomainDeclaredMethodType.Tag] = DomainDeclaredMethodType;
						RelationById[DomainDeclaredMethodType.Id] = DomainDeclaredMethodType;
			RelationByTag[ObjectTypeDerivedAssociationType.Tag] = ObjectTypeDerivedAssociationType;
						RelationById[ObjectTypeDerivedAssociationType.Id] = ObjectTypeDerivedAssociationType;
			RelationByTag[ObjectTypeDerivedDirectSupertype.Tag] = ObjectTypeDerivedDirectSupertype;
						RelationById[ObjectTypeDerivedDirectSupertype.Id] = ObjectTypeDerivedDirectSupertype;
			RelationByTag[AssociationTypeObjectType.Tag] = AssociationTypeObjectType;
						RelationById[AssociationTypeObjectType.Id] = AssociationTypeObjectType;
			RelationByTag[MetaObjectId.Tag] = MetaObjectId;
						RelationById[MetaObjectId.Id] = MetaObjectId;
			RelationByTag[DomainDerivedSuperDomain.Tag] = DomainDerivedSuperDomain;
						RelationById[DomainDerivedSuperDomain.Id] = DomainDerivedSuperDomain;
			RelationByTag[ObjectTypeDerivedDirectSuperclass.Tag] = ObjectTypeDerivedDirectSuperclass;
						RelationById[ObjectTypeDerivedDirectSuperclass.Id] = ObjectTypeDerivedDirectSuperclass;
			RelationByTag[ObjectTypePluralName.Tag] = ObjectTypePluralName;
						RelationById[ObjectTypePluralName.Id] = ObjectTypePluralName;
			RelationByTag[RoleTypeObjectType.Tag] = RoleTypeObjectType;
						RelationById[RoleTypeObjectType.Id] = RoleTypeObjectType;
			RelationByTag[RoleTypeDerivedRootType.Tag] = RoleTypeDerivedRootType;
						RelationById[RoleTypeDerivedRootType.Id] = RoleTypeDerivedRootType;
			RelationByTag[RelationTypeIsDerived.Tag] = RelationTypeIsDerived;
						RelationById[RelationTypeIsDerived.Id] = RelationTypeIsDerived;
			RelationByTag[AssociationTypeIsMany.Tag] = AssociationTypeIsMany;
						RelationById[AssociationTypeIsMany.Id] = AssociationTypeIsMany;
			RelationByTag[RoleTypeScale.Tag] = RoleTypeScale;
						RelationById[RoleTypeScale.Id] = RoleTypeScale;
			RelationByTag[ObjectTypeDerivedMethodType.Tag] = ObjectTypeDerivedMethodType;
						RelationById[ObjectTypeDerivedMethodType.Id] = ObjectTypeDerivedMethodType;
			RelationByTag[RoleTypePrecision.Tag] = RoleTypePrecision;
						RelationById[RoleTypePrecision.Id] = RoleTypePrecision;
			RelationByTag[DomainDeclaredInheritance.Tag] = DomainDeclaredInheritance;
						RelationById[DomainDeclaredInheritance.Id] = DomainDeclaredInheritance;
			RelationByTag[DomainDirectSuperDomain.Tag] = DomainDirectSuperDomain;
						RelationById[DomainDirectSuperDomain.Id] = DomainDirectSuperDomain;
			RelationByTag[RoleTypeSize.Tag] = RoleTypeSize;
						RelationById[RoleTypeSize.Id] = RoleTypeSize;
			RelationByTag[DomainUnitDomain.Tag] = DomainUnitDomain;
						RelationById[DomainUnitDomain.Id] = DomainUnitDomain;
			RelationByTag[DomainDerivedUnitObjectType.Tag] = DomainDerivedUnitObjectType;
						RelationById[DomainDerivedUnitObjectType.Id] = DomainDerivedUnitObjectType;
			RelationByTag[ObjectTypeDerivedSuperinterface.Tag] = ObjectTypeDerivedSuperinterface;
						RelationById[ObjectTypeDerivedSuperinterface.Id] = ObjectTypeDerivedSuperinterface;
			RelationByTag[DomainDerivedCompositeObjectType.Tag] = DomainDerivedCompositeObjectType;
						RelationById[DomainDerivedCompositeObjectType.Id] = DomainDerivedCompositeObjectType;
			RelationByTag[RoleTypeDerivedHierarchyPluralName.Tag] = RoleTypeDerivedHierarchyPluralName;
						RelationById[RoleTypeDerivedHierarchyPluralName.Id] = RoleTypeDerivedHierarchyPluralName;
			RelationByTag[RoleTypeDerivedHierarchySingularName.Tag] = RoleTypeDerivedHierarchySingularName;
						RelationById[RoleTypeDerivedHierarchySingularName.Id] = RoleTypeDerivedHierarchySingularName;
			RelationByTag[RoleTypeAssignedPluralName.Tag] = RoleTypeAssignedPluralName;
						RelationById[RoleTypeAssignedPluralName.Id] = RoleTypeAssignedPluralName;
			RelationByTag[RoleTypeDerivedRootName.Tag] = RoleTypeDerivedRootName;
						RelationById[RoleTypeDerivedRootName.Id] = RoleTypeDerivedRootName;
			RelationByTag[ObjectTypeIsInterface.Tag] = ObjectTypeIsInterface;
						RelationById[ObjectTypeIsInterface.Id] = ObjectTypeIsInterface;
			RelationByTag[ObjectTypeDerivedSubinterface.Tag] = ObjectTypeDerivedSubinterface;
						RelationById[ObjectTypeDerivedSubinterface.Id] = ObjectTypeDerivedSubinterface;
			RelationByTag[ObjectTypeIsUnit.Tag] = ObjectTypeIsUnit;
						RelationById[ObjectTypeIsUnit.Id] = ObjectTypeIsUnit;
			RelationByTag[ObjectTypeDerivedExclusiveConcreteLeafClass.Tag] = ObjectTypeDerivedExclusiveConcreteLeafClass;
						RelationById[ObjectTypeDerivedExclusiveConcreteLeafClass.Id] = ObjectTypeDerivedExclusiveConcreteLeafClass;
			RelationByTag[AssociationTypeDerivedRootObjectType.Tag] = AssociationTypeDerivedRootObjectType;
						RelationById[AssociationTypeDerivedRootObjectType.Id] = AssociationTypeDerivedRootObjectType;
			RelationByTag[AssociationTypeDerivedRootName.Tag] = AssociationTypeDerivedRootName;
						RelationById[AssociationTypeDerivedRootName.Id] = AssociationTypeDerivedRootName;
			RelationByTag[ObjectTypeDerivedDirectSuperinterface.Tag] = ObjectTypeDerivedDirectSuperinterface;
						RelationById[ObjectTypeDerivedDirectSuperinterface.Id] = ObjectTypeDerivedDirectSuperinterface;
			RelationByTag[MethodTypeObjectType.Tag] = MethodTypeObjectType;
						RelationById[MethodTypeObjectType.Id] = MethodTypeObjectType;
			RelationByTag[MethodTypeName.Tag] = MethodTypeName;
						RelationById[MethodTypeName.Id] = MethodTypeName;
			RelationByTag[RelationTypeIsIndexed.Tag] = RelationTypeIsIndexed;
						RelationById[RelationTypeIsIndexed.Id] = RelationTypeIsIndexed;
			RelationByTag[DomainDeclaredRelationType.Tag] = DomainDeclaredRelationType;
						RelationById[DomainDeclaredRelationType.Id] = DomainDeclaredRelationType;
			RelationByTag[RoleTypeIsMany.Tag] = RoleTypeIsMany;
						RelationById[RoleTypeIsMany.Id] = RoleTypeIsMany;
			RelationByTag[ObjectTypeDerivedUnitRoleType.Tag] = ObjectTypeDerivedUnitRoleType;
						RelationById[ObjectTypeDerivedUnitRoleType.Id] = ObjectTypeDerivedUnitRoleType;
			RelationByTag[DomainDerivedRelationType.Tag] = DomainDerivedRelationType;
						RelationById[DomainDerivedRelationType.Id] = DomainDerivedRelationType;
			RelationByTag[ObjectTypeDerivedRootClass.Tag] = ObjectTypeDerivedRootClass;
						RelationById[ObjectTypeDerivedRootClass.Id] = ObjectTypeDerivedRootClass;
			RelationByTag[DomainDerivedMethodType.Tag] = DomainDerivedMethodType;
						RelationById[DomainDerivedMethodType.Id] = DomainDerivedMethodType;
			RelationByTag[DomainDerivedInheritance.Tag] = DomainDerivedInheritance;
						RelationById[DomainDerivedInheritance.Id] = DomainDerivedInheritance;
			RelationByTag[RoleTypeAssignedSingularName.Tag] = RoleTypeAssignedSingularName;
						RelationById[RoleTypeAssignedSingularName.Id] = RoleTypeAssignedSingularName;
			RelationByTag[RelationTypeRoleType.Tag] = RelationTypeRoleType;
						RelationById[RelationTypeRoleType.Id] = RelationTypeRoleType;
			RelationByTag[ObjectTypeUnitTag.Tag] = ObjectTypeUnitTag;
						RelationById[ObjectTypeUnitTag.Id] = ObjectTypeUnitTag;
			RelationByTag[ObjectTypeDerivedCompositeRoleType.Tag] = ObjectTypeDerivedCompositeRoleType;
						RelationById[ObjectTypeDerivedCompositeRoleType.Id] = ObjectTypeDerivedCompositeRoleType;
			RelationByTag[ObjectTypeDerivedSuperclass.Tag] = ObjectTypeDerivedSuperclass;
						RelationById[ObjectTypeDerivedSuperclass.Id] = ObjectTypeDerivedSuperclass;
			RelationByTag[ObjectTypeIsAbstract.Tag] = ObjectTypeIsAbstract;
						RelationById[ObjectTypeIsAbstract.Id] = ObjectTypeIsAbstract;
			RelationByTag[AssociationTypeAssignedSingularName.Tag] = AssociationTypeAssignedSingularName;
						RelationById[AssociationTypeAssignedSingularName.Id] = AssociationTypeAssignedSingularName;
			RelationByTag[ObjectTypeDerivedRoleType.Tag] = ObjectTypeDerivedRoleType;
						RelationById[ObjectTypeDerivedRoleType.Id] = ObjectTypeDerivedRoleType;
			RelationByTag[DomainDerivedObjectType.Tag] = DomainDerivedObjectType;
						RelationById[DomainDerivedObjectType.Id] = DomainDerivedObjectType;
			RelationByTag[InheritanceSupertype.Tag] = InheritanceSupertype;
						RelationById[InheritanceSupertype.Id] = InheritanceSupertype;
			RelationByTag[ObjectTypeSingularName.Tag] = ObjectTypeSingularName;
						RelationById[ObjectTypeSingularName.Id] = ObjectTypeSingularName;
			RelationByTag[ObjectTypeDerivedExclusiveRoleType.Tag] = ObjectTypeDerivedExclusiveRoleType;
						RelationById[ObjectTypeDerivedExclusiveRoleType.Id] = ObjectTypeDerivedExclusiveRoleType;
			RelationByTag[ObjectTypeDerivedSupertype.Tag] = ObjectTypeDerivedSupertype;
						RelationById[ObjectTypeDerivedSupertype.Id] = ObjectTypeDerivedSupertype;
			RelationByTag[RelationTypeAssociationType.Tag] = RelationTypeAssociationType;
						RelationById[RelationTypeAssociationType.Id] = RelationTypeAssociationType;

			#endregion

			#region Relations CompositeHierarchy
			ArrayList types = new ArrayList();
			ObjectTypeDerivedExclusiveAssociationType.ConcreteClassHierarchy.Add(ObjectType);
			DomainDeclaredObjectType.ConcreteClassHierarchy.Add(Domain);
			InheritanceSubtype.ConcreteClassHierarchy.Add(Inheritance);
			DomainName.ConcreteClassHierarchy.Add(Domain);
			ObjectTypeDerivedExclusiveSuperinterface.ConcreteClassHierarchy.Add(ObjectType);
			AssociationTypeAssignedPluralName.ConcreteClassHierarchy.Add(AssociationType);
			ObjectTypeDerivedSubclass.ConcreteClassHierarchy.Add(ObjectType);
			DomainDeclaredMethodType.ConcreteClassHierarchy.Add(Domain);
			ObjectTypeDerivedAssociationType.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedDirectSupertype.ConcreteClassHierarchy.Add(ObjectType);
			AssociationTypeObjectType.ConcreteClassHierarchy.Add(AssociationType);
			MetaObjectId.ConcreteClassHierarchy.Add(AssociationType);
			MetaObjectId.ConcreteClassHierarchy.Add(Domain);
			MetaObjectId.ConcreteClassHierarchy.Add(RoleType);
			MetaObjectId.ConcreteClassHierarchy.Add(RelationType);
			MetaObjectId.ConcreteClassHierarchy.Add(Inheritance);
			MetaObjectId.ConcreteClassHierarchy.Add(ObjectType);
			MetaObjectId.ConcreteClassHierarchy.Add(MethodType);
			DomainDerivedSuperDomain.ConcreteClassHierarchy.Add(Domain);
			ObjectTypeDerivedDirectSuperclass.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypePluralName.ConcreteClassHierarchy.Add(ObjectType);
			RoleTypeObjectType.ConcreteClassHierarchy.Add(RoleType);
			RoleTypeDerivedRootType.ConcreteClassHierarchy.Add(RoleType);
			RelationTypeIsDerived.ConcreteClassHierarchy.Add(RelationType);
			AssociationTypeIsMany.ConcreteClassHierarchy.Add(AssociationType);
			RoleTypeScale.ConcreteClassHierarchy.Add(RoleType);
			ObjectTypeDerivedMethodType.ConcreteClassHierarchy.Add(ObjectType);
			RoleTypePrecision.ConcreteClassHierarchy.Add(RoleType);
			DomainDeclaredInheritance.ConcreteClassHierarchy.Add(Domain);
			DomainDirectSuperDomain.ConcreteClassHierarchy.Add(Domain);
			RoleTypeSize.ConcreteClassHierarchy.Add(RoleType);
			DomainUnitDomain.ConcreteClassHierarchy.Add(Domain);
			DomainDerivedUnitObjectType.ConcreteClassHierarchy.Add(Domain);
			ObjectTypeDerivedSuperinterface.ConcreteClassHierarchy.Add(ObjectType);
			DomainDerivedCompositeObjectType.ConcreteClassHierarchy.Add(Domain);
			RoleTypeDerivedHierarchyPluralName.ConcreteClassHierarchy.Add(RoleType);
			RoleTypeDerivedHierarchySingularName.ConcreteClassHierarchy.Add(RoleType);
			RoleTypeAssignedPluralName.ConcreteClassHierarchy.Add(RoleType);
			RoleTypeDerivedRootName.ConcreteClassHierarchy.Add(RoleType);
			ObjectTypeIsInterface.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedSubinterface.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeIsUnit.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedExclusiveConcreteLeafClass.ConcreteClassHierarchy.Add(ObjectType);
			AssociationTypeDerivedRootObjectType.ConcreteClassHierarchy.Add(AssociationType);
			AssociationTypeDerivedRootName.ConcreteClassHierarchy.Add(AssociationType);
			ObjectTypeDerivedDirectSuperinterface.ConcreteClassHierarchy.Add(ObjectType);
			MethodTypeObjectType.ConcreteClassHierarchy.Add(MethodType);
			MethodTypeName.ConcreteClassHierarchy.Add(MethodType);
			RelationTypeIsIndexed.ConcreteClassHierarchy.Add(RelationType);
			DomainDeclaredRelationType.ConcreteClassHierarchy.Add(Domain);
			RoleTypeIsMany.ConcreteClassHierarchy.Add(RoleType);
			ObjectTypeDerivedUnitRoleType.ConcreteClassHierarchy.Add(ObjectType);
			DomainDerivedRelationType.ConcreteClassHierarchy.Add(Domain);
			ObjectTypeDerivedRootClass.ConcreteClassHierarchy.Add(ObjectType);
			DomainDerivedMethodType.ConcreteClassHierarchy.Add(Domain);
			DomainDerivedInheritance.ConcreteClassHierarchy.Add(Domain);
			RoleTypeAssignedSingularName.ConcreteClassHierarchy.Add(RoleType);
			RelationTypeRoleType.ConcreteClassHierarchy.Add(RelationType);
			ObjectTypeUnitTag.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedCompositeRoleType.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedSuperclass.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeIsAbstract.ConcreteClassHierarchy.Add(ObjectType);
			AssociationTypeAssignedSingularName.ConcreteClassHierarchy.Add(AssociationType);
			ObjectTypeDerivedRoleType.ConcreteClassHierarchy.Add(ObjectType);
			DomainDerivedObjectType.ConcreteClassHierarchy.Add(Domain);
			InheritanceSupertype.ConcreteClassHierarchy.Add(Inheritance);
			ObjectTypeSingularName.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedExclusiveRoleType.ConcreteClassHierarchy.Add(ObjectType);
			ObjectTypeDerivedSupertype.ConcreteClassHierarchy.Add(ObjectType);
			RelationTypeAssociationType.ConcreteClassHierarchy.Add(RelationType);

			#endregion
		}
	}

	/// <summary>
	/// The <see cref="AllorsInternal"/> interface needs to be implemented by
	/// <see cref="AllorsEmbeddedObject"/>s. It's is required for managing the
	/// Relations.
	/// </summary>
	internal interface AllorsInternal
	{
		/// <param name="relation">The relation.</param>
		/// <returns></returns>
		object GetRole(AllorsEmbeddedRelationType relation);

		/// <param name="relation">The relation.</param>
	    /// <param name="role">The role. </param>
		/// <returns></returns>
		void SetRole(AllorsEmbeddedRelationType relation, object role);
	}

	/// <summary>
	/// The <see cref="AllorsEmbeddedArrays"/> class is a Utility class 
	/// for maniupulating <see cref="Array"/>'s.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	internal class AllorsEmbeddedArrays
	{
		/// <summary>
		/// Checks if the object exists in this Array.
		/// </summary>
		/// <param name="objectArray">The object array.</param>
		/// <param name="value">The value.</param>
		/// <returns>true if the object exists in this Array</returns>
		internal static bool Exist(AllorsEmbeddedObject[] objectArray, AllorsEmbeddedObject value)
		{
			foreach( AllorsEmbeddedObject objectArrayValue in objectArray )
			{
				if( objectArrayValue.Equals(value) )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds the specified object to the Array.
		/// </summary>
		/// <param name="objectArray">The object array.</param>
		/// <param name="value">The object.</param>
		/// <returns>The new object Array</returns>
		internal static AllorsEmbeddedObject[] Add(AllorsEmbeddedObject[] objectArray, AllorsEmbeddedObject value)
		{
			AllorsEmbeddedObject[] newObjectArray = (AllorsEmbeddedObject[]) Array.CreateInstance(objectArray.GetType().GetElementType(), objectArray.Length+1);
			System.Array.Copy(objectArray,0,newObjectArray,0,objectArray.Length);
			newObjectArray[objectArray.Length] = value;
			return newObjectArray;
		}

		/// <summary>
		/// Removes the specified object array.
		/// </summary>
		/// <param name="objectArray">The object array.</param>
		/// <param name="value">The object.</param>
		/// <returns>The new object Array</returns>
		internal static AllorsEmbeddedObject[] Remove(AllorsEmbeddedObject[] objectArray, AllorsEmbeddedObject value)
		{
			System.Collections.ArrayList newObjectArrayList = new System.Collections.ArrayList(objectArray);
			newObjectArrayList.Remove(value);
			return newObjectArrayList.ToArray(objectArray.GetType().GetElementType()) as AllorsEmbeddedObject[];
		}

		/// <summary>
		/// An empty array of PropertyTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.PropertyType[] EMPTY_PropertyType_ARRAY = {};
		/// <summary>
		/// An empty array of AssociationTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.AssociationType[] EMPTY_AssociationType_ARRAY = {};
		/// <summary>
		/// An empty array of MetaObjects.
		/// </summary>
		internal static global::Allors.R1.Meta.MetaObject[] EMPTY_MetaObject_ARRAY = {};
		/// <summary>
		/// An empty array of Domains.
		/// </summary>
		internal static global::Allors.R1.Meta.Domain[] EMPTY_Domain_ARRAY = {};
		/// <summary>
		/// An empty array of RoleTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.RoleType[] EMPTY_RoleType_ARRAY = {};
		/// <summary>
		/// An empty array of OperandTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.OperandType[] EMPTY_OperandType_ARRAY = {};
		/// <summary>
		/// An empty array of RelationTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.RelationType[] EMPTY_RelationType_ARRAY = {};
		/// <summary>
		/// An empty array of Inheritances.
		/// </summary>
		internal static global::Allors.R1.Meta.Inheritance[] EMPTY_Inheritance_ARRAY = {};
		/// <summary>
		/// An empty array of ObjectTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.ObjectType[] EMPTY_ObjectType_ARRAY = {};
		/// <summary>
		/// An empty array of MethodTypes.
		/// </summary>
		internal static global::Allors.R1.Meta.MethodType[] EMPTY_MethodType_ARRAY = {};

	}

	/// <summary>
	/// The <see cref="AllorsEmbeddedXml"/> class defines the tags and
	/// constants necessary for saving and loading populations to Xml files.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	public class AllorsEmbeddedXml
	{
		internal static readonly string ALLORS = "allors";

		internal static readonly string VERSION = "version";

		internal static readonly string VERSION_CURRENT = "1";

		internal static readonly string POPULATION = "population";

		internal static readonly string OBJECTS = "objects";

		internal static readonly string RELATIONS = "relations";

		internal static readonly string OBJECT_TYPE = "ot";

		internal static readonly string OBJECTS_SPLITTER = ",";

		internal static readonly char[] OBJECTS_SPLITTER_CHAR_ARRAY = { OBJECTS_SPLITTER[0] };

		internal static readonly string ID = "i";

		internal static readonly string RELATION_TYPE_COMPOSITE = "rtc";

		internal static readonly string RELATION_TYPE_UNIT = "rtu";

		internal static readonly string RELATION = "r";

		internal static readonly string ASSOCIATION = "a";

		internal static readonly string DOMAIN_ID = "id";

		internal static readonly string NAME = "name";

		internal static readonly char XML_OBJECTS_SPLITTER = ',';

		internal static readonly string XML_OBJECTS_SPLITTER_STRING = XML_OBJECTS_SPLITTER.ToString();


		internal static readonly char XML_OBJECT_SPLITTER = ':';

		internal static readonly string XML_OBJECT_SPLITTER_STRING = XML_OBJECT_SPLITTER.ToString();

		internal static readonly char XML_OBJECT_PART_SPLITTER = '@';

		internal static readonly string XML_OBJECT_PART_SPLITTER_STRING = XML_OBJECT_PART_SPLITTER.ToString();
	}

	/// <summary>
	/// The <see cref="AllorsTypeTags"/> hold the tags for all <see cref="AllorsEmbeddedObject"/>s
	/// in this <see cref="AllorsEmbeddedDomain"/>.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	internal class AllorsTypeTags
	{
			internal const int PropertyType = 1;
			internal const int AssociationType = 2;
			internal const int MetaObject = 3;
			internal const int Domain = 4;
			internal const int RoleType = 5;
			internal const int OperandType = 6;
			internal const int RelationType = 7;
			internal const int Inheritance = 8;
			internal const int ObjectType = 9;
			internal const int MethodType = 10;
			internal const int AllorsString = 11;
			internal const int AllorsInteger = 12;
			internal const int AllorsLong = 13;
			internal const int AllorsDecimal = 14;
			internal const int AllorsDouble = 15;
			internal const int AllorsBoolean = 16;
			internal const int AllorsDateTime = 17;
			internal const int AllorsUnique = 18;
			internal const int AllorsBinary = 19;

	}

	/// <summary>
	/// The <see cref="AllorsRelationTags"/> hold the tags for all relations
	/// in this <see cref="AllorsEmbeddedDomain"/>.
	/// </summary>
	[System.Diagnostics.DebuggerNonUserCode]
	internal class AllorsRelationTags
	{
		internal const int ObjectTypeDerivedExclusiveAssociationType = 1;
		internal const int DomainDeclaredObjectType = 2;
		internal const int InheritanceSubtype = 3;
		internal const int DomainName = 4;
		internal const int ObjectTypeDerivedExclusiveSuperinterface = 5;
		internal const int AssociationTypeAssignedPluralName = 6;
		internal const int ObjectTypeDerivedSubclass = 7;
		internal const int DomainDeclaredMethodType = 8;
		internal const int ObjectTypeDerivedAssociationType = 9;
		internal const int ObjectTypeDerivedDirectSupertype = 10;
		internal const int AssociationTypeObjectType = 11;
		internal const int MetaObjectId = 12;
		internal const int DomainDerivedSuperDomain = 13;
		internal const int ObjectTypeDerivedDirectSuperclass = 14;
		internal const int ObjectTypePluralName = 15;
		internal const int RoleTypeObjectType = 16;
		internal const int RoleTypeDerivedRootType = 17;
		internal const int RelationTypeIsDerived = 18;
		internal const int AssociationTypeIsMany = 19;
		internal const int RoleTypeScale = 20;
		internal const int ObjectTypeDerivedMethodType = 21;
		internal const int RoleTypePrecision = 22;
		internal const int DomainDeclaredInheritance = 23;
		internal const int DomainDirectSuperDomain = 24;
		internal const int RoleTypeSize = 25;
		internal const int DomainUnitDomain = 26;
		internal const int DomainDerivedUnitObjectType = 27;
		internal const int ObjectTypeDerivedSuperinterface = 28;
		internal const int DomainDerivedCompositeObjectType = 29;
		internal const int RoleTypeDerivedHierarchyPluralName = 30;
		internal const int RoleTypeDerivedHierarchySingularName = 31;
		internal const int RoleTypeAssignedPluralName = 32;
		internal const int RoleTypeDerivedRootName = 33;
		internal const int ObjectTypeIsInterface = 34;
		internal const int ObjectTypeDerivedSubinterface = 35;
		internal const int ObjectTypeIsUnit = 36;
		internal const int ObjectTypeDerivedExclusiveConcreteLeafClass = 37;
		internal const int AssociationTypeDerivedRootObjectType = 38;
		internal const int AssociationTypeDerivedRootName = 39;
		internal const int ObjectTypeDerivedDirectSuperinterface = 40;
		internal const int MethodTypeObjectType = 41;
		internal const int MethodTypeName = 42;
		internal const int RelationTypeIsIndexed = 43;
		internal const int DomainDeclaredRelationType = 44;
		internal const int RoleTypeIsMany = 45;
		internal const int ObjectTypeDerivedUnitRoleType = 46;
		internal const int DomainDerivedRelationType = 47;
		internal const int ObjectTypeDerivedRootClass = 48;
		internal const int DomainDerivedMethodType = 49;
		internal const int DomainDerivedInheritance = 50;
		internal const int RoleTypeAssignedSingularName = 51;
		internal const int RelationTypeRoleType = 52;
		internal const int ObjectTypeUnitTag = 53;
		internal const int ObjectTypeDerivedCompositeRoleType = 54;
		internal const int ObjectTypeDerivedSuperclass = 55;
		internal const int ObjectTypeIsAbstract = 56;
		internal const int AssociationTypeAssignedSingularName = 57;
		internal const int ObjectTypeDerivedRoleType = 58;
		internal const int DomainDerivedObjectType = 59;
		internal const int InheritanceSupertype = 60;
		internal const int ObjectTypeSingularName = 61;
		internal const int ObjectTypeDerivedExclusiveRoleType = 62;
		internal const int ObjectTypeDerivedSupertype = 63;
		internal const int RelationTypeAssociationType = 64;

	}
}