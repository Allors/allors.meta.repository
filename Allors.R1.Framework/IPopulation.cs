// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPopulation.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1
{
    using System;
    using System.Xml;

    using Allors.R1.Meta;

    /// <summary>
    /// A population is the container for objects and relations.
    /// </summary>
    public interface IPopulation
    {
        /// <summary>
        /// Occurs when a session is created.
        /// </summary>
        event SessionCreatedEventHandler SessionCreated;

        /// <summary>
        ///  Gets
        /// <ul>
        /// <li>the id of this database</li>
        /// <li>the id of the database from this workspace</li>
        /// </ul>
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the object factory.
        /// </summary>
        /// <value>The object factory.</value>
        IObjectFactory ObjectFactory { get; }

        /// <summary>
        /// Gets a value indicating whether this population is a database.
        /// </summary>
        /// <value>
        /// <c>true</c> if this population is a database; otherwise, <c>false</c>.
        /// </value>
        bool IsDatabase { get; }

        /// <summary>
        /// Gets a value indicating whether this population is a workspace.
        /// </summary>
        /// <value>
        /// <c>true</c> if this population is a workspace; otherwise, <c>false</c>.
        /// </value>
        bool IsWorkspace { get; }

        /// <summary>
        /// Gets the meta domain of this population.
        /// </summary>
        Domain Domain { get; }

        /// <summary>
        /// Population properties are simple key/value pairs.
        /// Because Allors objects can not hold instance variables, this is the only way
        /// for Allors objects to hold references to Non Allors objects or Allors objects
        /// from a different population.
        /// </summary>
        /// <param name="name">The key by which the session object is retrieved.</param>
        /// <returns>The properties.</returns>
        object this[string name]
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new AllorsSession.
        /// </summary>
        /// <returns>a newly created AllorsSession</returns>
        ISession CreateSession();

        /// <summary>
        /// Loads the population from the <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        void Load(XmlReader reader);

        /// <summary>
        /// Saves the population to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void Save(XmlWriter writer);
    }
}