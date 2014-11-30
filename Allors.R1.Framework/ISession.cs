//------------------------------------------------------------------------------------------------- 
// <copyright file="ISession.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the ISession type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1
{
    using System;

    using Allors.R1.Meta;

    /// <summary>
    /// <para>
    /// An AllorsSession is a transactional session that provides lifecycle management to objects in the population.
    /// </para>
    /// <para>
    /// Transactions are 'rolling'. After committing or rolling back a transaction an new transaction is automatically started, there is no need to manually 'begin' a transaction.
    /// </para>
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Occurs when the Commit() method is left.
        /// Changes to the database are allowed but they occur in a new transaction. 
        /// </summary>
        event SessionCommittedEventHandler Committed;

        /// <summary>
        /// Occurs when the Commit() method is entered.
        /// Changes to the database are allowed and are within the current transaction. 
        /// Calls to Commit() and Rollback() during this event are ignored.
        /// </summary>
        event SessionCommittingEventHandler Committing;

        /// <summary>
        /// Occurs when the Rollback() method is entered.
        /// Changes to the database are allowed but will have no effect because the transaction will be rolled back. 
        /// Calls to Commit() and Rollback() during this event are ignored.
        /// </summary>
        event SessionRollingBackEventHandler RollingBack;

        /// <summary>
        /// Occurs when the Rollback() method is left.
        /// Changes to the database are allowed but they occur in a new transaction. 
        /// </summary>
        event SessionRolledBackEventHandler RolledBack;

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        IPopulation Population { get; }
        
        /// <summary>
        /// Session properties are simple key/value pairs.
        /// Because AllorsObjects can not hold instance variables, this is the only way
        /// for AllorsObjects to hold references to non Allors objects or Allors objects
        /// from a different population.
        /// </summary>
        /// <param name="name">The key by which the session object is retrieved.</param>
        /// <returns>The property.</returns>
        object this[string name]
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a change set of all changes up to this checkpoint,
        /// starting from either the beginning of the transaction or
        /// from a previous checkpoint.
        /// </summary>
        /// <returns>The change set.</returns>
        IChangeSet Checkpoint();
        
        /// <summary>
        /// Creates an Extent for the specified ObjectType.
        /// Only works for static domains.
        /// </summary>
        /// <typeparam name="T">The type for the extent.</typeparam>
        /// <returns>The extent.</returns>
        Extent<T> Extent<T>() where T : IObject;

        /// <summary>
        /// Creates an Extent for the specified <see cref="ObjectType"/>.
        /// </summary>
        /// <param name="objectType">The objectType.</param>
        /// <returns>The extent.</returns>
        Extent Extent(ObjectType objectType);

        /// <summary>
        /// Creates an Extent that is the exception of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The except extent.</returns>
        Extent Except(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Creates an Extent that is the Intersect of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The intersect extent.</returns>
        Extent Intersect(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Creates an Extent that is the Union of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The union extent.</returns>
        Extent Union(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Commits all changes that where made during this transaction.
        /// Because transactions are rolling, a new transaction is automatically created.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back all changes that where made during this transaction.
        /// Because transactions are rolling, a new transaction is automatically created.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Creates an Allors Object.
        /// Only works for static domains.
        /// </summary>
        /// <typeparam name="T">The ObjectType</typeparam>
        /// <returns>a new <see cref="IObject"/></returns>
        T Create<T>() where T : IObject;

        /// <summary>
        /// Creates an Allors Object.
        /// </summary>
        /// <param name="objectType">The ObjectType.</param>
        /// <returns>a new <see cref="IObject"/></returns>
        IObject Create(ObjectType objectType);

        /// <summary>
        /// Creates a specified amount of AllorsObjects.
        /// </summary>
        /// <param name="objectType">The ObjectType.</param>
        /// <param name="count">The count.</param>
        /// <returns>The created objects.</returns>
        IObject[] Create(ObjectType objectType, int count);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(IObject obj);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(string objectId);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(ObjectId objectId);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(IObject[] objects);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objectIds">The object ids.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(string[] objectIds);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objectIds">The object ids.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(ObjectId[] objectIds);
    }
}