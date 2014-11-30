//------------------------------------------------------------------------------------------------- 
// <copyright file="PullUp.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    /// <summary>
    /// Pulls up a meta object from a domain to a super domain.
    /// </summary>
    public abstract class PullUp : Command
    {
        private readonly HashSet<Domain> superDomains;

        private readonly HashSet<ObjectType> objectTypesToPull;
        private readonly HashSet<Inheritance> inheritancesToPull;
        private readonly HashSet<RelationType> relationTypesToPull;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PullUp" /> class.
        /// </summary>
        /// <param name="pullFromRepository">Pull from repository.</param>
        /// <param name="pullToSuperDomain">Pull up to super domain.</param>
        /// <exception cref="System.ArgumentException">Domain and super domain are equal</exception>
        internal PullUp(Repository pullFromRepository, Domain pullToSuperDomain)
        {
            this.PullFromRepository = pullFromRepository;
            this.PullToSuperDomain = pullToSuperDomain;
            
            if (this.PullFromRepository.Domain.Equals(pullToSuperDomain))
            {
                throw new ArgumentException("Domain and super domain are equal", "pullToSuperDomain");
            }

            this.superDomains = new HashSet<Domain> { pullToSuperDomain };
            this.superDomains.UnionWith(pullToSuperDomain.SuperDomains);

            this.objectTypesToPull = new HashSet<ObjectType>();
            this.inheritancesToPull = new HashSet<Inheritance>();
            this.relationTypesToPull = new HashSet<RelationType>();
        }

        /// <summary>
        /// Gets the repository to pull from.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public Repository PullFromRepository { get; private set; }

        /// <summary>
        /// Gets the super domain to pull to.
        /// </summary>
        /// <value>
        /// The super domain.
        /// </value>
        public Domain PullToSuperDomain { get; private set; }

        /// <summary>
        /// Gets the object types to pull.
        /// </summary>
        /// <value>
        /// The object types to pull.
        /// </value>
        public ObjectType[] ObjectTypesToPull
        {
            get
            {
                return this.objectTypesToPull.ToArray();
            }
        }

        /// <summary>
        /// Gets the inheritances to pull.
        /// </summary>
        /// <value>
        /// The inheritances to pull.
        /// </value>
        public Inheritance[] InheritancesToPull
        {
            get
            {
                return this.inheritancesToPull.ToArray();
            }
        }

        /// <summary>
        /// Gets the relation types to pull.
        /// </summary>
        /// <value>
        /// The relation types to pull.
        /// </value>
        public RelationType[] RelationTypesToPull
        {
            get
            {
                return this.relationTypesToPull.ToArray();
            }
        }

        /// <summary>
        /// Gets all meta objects to pull.
        /// </summary>
        /// <value>
        /// All meta objects to pull.
        /// </value>
        public MetaObject[] MetaObjectsToPull
        {
            get
            {
                var metaObjectsToPull = new HashSet<MetaObject>();
                metaObjectsToPull.UnionWith(this.ObjectTypesToPull);
                metaObjectsToPull.UnionWith(this.InheritancesToPull);
                metaObjectsToPull.UnionWith(this.RelationTypesToPull);
                return metaObjectsToPull.ToArray();
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute()
        {
            foreach (var objectTypeToPull in this.objectTypesToPull)
            {
                this.PullToSuperDomain.AddDeclaredObjectType(objectTypeToPull);
                objectTypeToPull.SendChangedEvent();
            }

            foreach (var inheritanceToPull in this.inheritancesToPull)
            {
                this.PullToSuperDomain.AddDeclaredInheritance(inheritanceToPull);
                inheritanceToPull.SendChangedEvent();
            }

            foreach (var relationTypeToPull in this.relationTypesToPull)
            {
                this.PullToSuperDomain.AddDeclaredRelationType(relationTypeToPull);
                relationTypeToPull.SendChangedEvent();
            }
        }

        /// <summary>
        /// Resolves all dependent meta objects for this object type.
        /// </summary>
        /// <param name="objectType">The object type to resolve dependencies for.</param>
        protected void ResolveDependencies(ObjectType objectType)
        {
            if (this.objectTypesToPull.Contains(objectType) || this.superDomains.Contains(objectType.DomainWhereDeclaredObjectType))
            {
                return;
            }

            this.objectTypesToPull.Add(objectType);
        }

        /// <summary>
        /// Resolves all dependent meta objects for this object type.
        /// </summary>
        /// <param name="inheritance">The inheritance to resolve dependencies for.</param>
        protected void ResolveDependencies(Inheritance inheritance)
        {
            if (this.inheritancesToPull.Contains(inheritance) || this.superDomains.Contains(inheritance.DomainWhereDeclaredInheritance))
            {
                return;
            }

            this.inheritancesToPull.Add(inheritance);

            this.ResolveDependencies(inheritance.Subtype);
            this.ResolveDependencies(inheritance.Supertype);
        }

        /// <summary>
        /// Resolves all dependent meta objects for this object type.
        /// </summary>
        /// <param name="relationType">The relation type to resolve dependencies for.</param>
        protected void ResolveDependencies(RelationType relationType)
        {
            if (this.relationTypesToPull.Contains(relationType) || this.superDomains.Contains(relationType.DomainWhereDeclaredRelationType))
            {
                return;
            }

            this.relationTypesToPull.Add(relationType);

            this.ResolveDependencies(relationType.AssociationType.ObjectType);
            this.ResolveDependencies(relationType.RoleType.ObjectType);
        }
    }
}