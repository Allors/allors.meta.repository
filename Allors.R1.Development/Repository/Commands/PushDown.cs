//------------------------------------------------------------------------------------------------- 
// <copyright file="PushDown.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    /// <summary>
    /// Push down a meta object from a super domain to a domain.
    /// </summary>
    public abstract class PushDown : Command
    {
        private readonly HashSet<ObjectType> domainDeclaredObjectTypes;
        private readonly HashSet<Inheritance> domainDeclaredInheritances;
        private readonly HashSet<RelationType> domainDeclaredRelationTypes;

        private readonly HashSet<ObjectType> objectTypesToPush;
        private readonly HashSet<Inheritance> inheritancesToPush;
        private readonly HashSet<RelationType> relationTypesToPush;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushDown" /> class.
        /// </summary>
        /// <param name="repository">Push down to this repository.</param>
        internal PushDown(Repository repository)
        {
            this.Domain = repository.Domain;

            this.domainDeclaredObjectTypes = new HashSet<ObjectType>(this.Domain.DeclaredObjectTypes);
            this.domainDeclaredInheritances = new HashSet<Inheritance>(this.Domain.DeclaredInheritances);
            this.domainDeclaredRelationTypes = new HashSet<RelationType>(this.Domain.DeclaredRelationTypes);

            this.objectTypesToPush = new HashSet<ObjectType>();
            this.inheritancesToPush = new HashSet<Inheritance>();
            this.relationTypesToPush = new HashSet<RelationType>();
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public Domain Domain { get; private set; }

        /// <summary>
        /// Gets the object types to push.
        /// </summary>
        /// <value>
        /// The object types to push.
        /// </value>
        public ObjectType[] ObjectTypesToPush
        {
            get
            {
                return this.objectTypesToPush.ToArray();
            }
        }

        /// <summary>
        /// Gets the inheritances to push.
        /// </summary>
        /// <value>
        /// The inheritances to push.
        /// </value>
        public Inheritance[] InheritancesToPush
        {
            get
            {
                return this.inheritancesToPush.ToArray();
            }
        }

        /// <summary>
        /// Gets the relation types to push.
        /// </summary>
        /// <value>
        /// The relation types to push.
        /// </value>
        public RelationType[] RelationTypesToPush
        {
            get
            {
                return this.relationTypesToPush.ToArray();
            }
        }

        /// <summary>
        /// Gets all meta objects to pull.
        /// </summary>
        /// <value>
        /// All meta objects to pull.
        /// </value>
        public MetaObject[] MetaObjectsToPush
        {
            get
            {
                var metaObjectsToPull = new HashSet<MetaObject>();
                metaObjectsToPull.UnionWith(this.ObjectTypesToPush);
                metaObjectsToPull.UnionWith(this.InheritancesToPush);
                metaObjectsToPull.UnionWith(this.RelationTypesToPush);
                return metaObjectsToPull.ToArray();
            }
        }
        
        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            foreach (var relationTypeToPush in this.RelationTypesToPush)
            {
                this.Domain.AddDeclaredRelationType(relationTypeToPush);
                relationTypeToPush.SendChangedEvent();
            }

            foreach (var inheritanceToPush in this.InheritancesToPush)
            {
                this.Domain.AddDeclaredInheritance(inheritanceToPush);
                inheritanceToPush.SendChangedEvent();
            }

            foreach (var objectTypeToPush in this.ObjectTypesToPush)
            {
                this.Domain.AddDeclaredObjectType(objectTypeToPush);
                objectTypeToPush.SendChangedEvent();
            }
        }

        protected void ResolveDependencies(ObjectType objectType)
        {
            if (this.domainDeclaredObjectTypes.Contains(objectType) || this.ObjectTypesToPush.Contains(objectType))
            {
                return;
            }

            this.objectTypesToPush.Add(objectType);

            foreach (var inheritance in objectType.InheritancesWhereSupertype)
            {
                this.ResolveDependencies(inheritance);
            }

            foreach (var inheritance in objectType.InheritancesWhereSubtype)
            {
                this.ResolveDependencies(inheritance);
            }

            foreach (var roleType in objectType.RoleTypesWhereObjectType)
            {
                this.ResolveDependencies(roleType.RelationType);
            }

            foreach (var associationType in objectType.AssociationTypesWhereObjectType)
            {
                this.ResolveDependencies(associationType.RelationType);
            }
        }

        protected void ResolveDependencies(Inheritance inheritance)
        {
            if (this.domainDeclaredInheritances.Contains(inheritance) || this.InheritancesToPush.Contains(inheritance))
            {
                return;
            }

            this.inheritancesToPush.Add(inheritance);
        }

        protected void ResolveDependencies(RelationType relationType)
        {
            if (this.domainDeclaredRelationTypes.Contains(relationType) || this.RelationTypesToPush.Contains(relationType))
            {
                return;
            }

            this.relationTypesToPush.Add(relationType);
        }
    }
}