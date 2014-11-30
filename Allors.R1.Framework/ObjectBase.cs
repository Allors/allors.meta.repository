//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectBase.cs" company="Allors bvba">
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
// <summary>Defines the ObjectBase type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1
{
    using System.Diagnostics;

    /// <summary>
    /// A base implementation for a static <see cref="IObject"/>.
    /// </summary>
    public abstract class ObjectBase : IObject
    {
        /// <summary>
        /// The <see cref="Strategy"/>.
        /// </summary>
        private readonly IStrategy strategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBase"/> class.
        /// </summary>
        /// <param name="allors">The allors.</param>
        protected ObjectBase(IStrategy allors)
        {
            this.strategy = allors;
        }
        
        /// <summary>
        /// Gets the <see cref="Strategy"/>.
        /// </summary>
        /// <value>The strategy.</value>
        public IStrategy Strategy
        {
            [DebuggerStepThrough]
            get { return this.strategy; }
        }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public ObjectId Id
        {
            [DebuggerStepThrough]
            get 
            {
                return this.strategy.ObjectId;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            var typedObj = obj as IObject;
            return typedObj != null &&
                   typedObj.Strategy.ObjectId.Equals(this.Strategy.ObjectId) &&
                   typedObj.Strategy.Session.Population.Id.Equals(this.Strategy.Session.Population.Id);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// After syncing a new workspace object, the database object
        /// will receive a new id (when using integer based ids).
        /// Since the hash code is based on the object's id, this
        /// means that you can not use Allors Objects as keys during
        /// a Sync() on a Workspace.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Strategy.ObjectId.GetHashCode();
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public virtual void Delete()
        {
            this.strategy.Delete();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.GetType().Name + "[" + this.Strategy.ObjectId + "]";
        }
    }
}