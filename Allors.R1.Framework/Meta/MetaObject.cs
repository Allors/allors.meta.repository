// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaObject.cs" company="Allors bvba">
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

namespace Allors.R1.Meta
{
    using System;
    using System.Collections.Generic;

    using Allors.R1.Meta.AllorsGenerated;

    /// <summary>
    /// Base class for Meta objects.
    /// </summary>
    public abstract partial class MetaObject
    {
        /// <summary>
        /// Gets the id comparer for meta objects.
        /// </summary>
        public static readonly IComparer<MetaObject> IdComparer = new PrivateIdComparer();

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The meta object id.</value>
        public override sealed Guid Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                if (this.ExistId)
                {
                    throw new ArgumentException("Id " + this.Id.ToString() + " is write once");
                }

                if (value.Equals(Guid.Empty))
                {
                    this.RemoveId();
                }
                else
                {
                    var existing = this.Domain.Find(value);

                    if (existing != null)
                    {
                        throw new ArgumentException("Id " + value.ToString() + " is already in use");
                    }
                    
                    base.Id = value;
                    this.Domain.MetaObjectById[this.Id] = this;
                }
            }
        }

        /// <summary>
        /// Gets the id as a number only string.
        /// </summary>
        /// <value>The id as a number only string.</value>
        public string IdAsNumberString
        {
            get { return this.Id.ToString("N").ToLower(); }
        }

        /// <summary>
        /// Gets the id as a string.
        /// </summary>
        /// <value>The id as a string.</value>
        public string IdAsString
        {
            get { return this.Id.ToString("D").ToLower(); }
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public Domain Domain
        {
            get { return Domain.GetDomain(this.AllorsSession); }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        protected abstract string ValidationName { get; }

        /// <summary>
        /// Removes the Id.
        /// </summary>
        public override void RemoveId()
        {
            if (this.ExistId)
            {
                throw new ArgumentException("Id is write once");
            }

            base.RemoveId();
        }

        /// <summary>
        /// Send a changed event.
        /// </summary>
        public virtual void SendChangedEvent()
        {
            this.Domain.SendChangedEvent(this);
        }

        /// <summary>
        /// Validate this object.
        /// </summary>
        /// <param name="validationLog">
        /// The validation log.
        /// </param>
        protected internal virtual void Validate(ValidationLog validationLog)
        {
            if (!this.ExistId)
            {
                var message = "id on " + this.ValidationName + " is required";
                validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.MetaObjectId);
            }
            else
            {
                if (validationLog.ExistId(this.Id))
                {
                    var message = "id " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.MetaObjectId);
                }
                else
                {
                    validationLog.AddId(this.Id);
                }
            }
        }

        /// <summary>
        /// Copy from the source meta object.
        /// </summary>
        /// <param name="source">
        /// The source meta object.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when this object already has an id that is different from the source object.
        /// </exception>
        protected void CopyMetaObject(MetaObject source)
        {
            if (!this.ExistId)
            {
                this.Id = source.Id;
            }
            else if (!this.Id.Equals(source.Id))
            {
                throw new ArgumentException("imported object has a different id (" + this.ValidationName + ")");
            }
        }

        /// <summary>
        /// The id comparer.
        /// </summary>
        private class PrivateIdComparer : IComparer<MetaObject>
        {
            /// <summary>
            /// Compares two relationTypes by id.
            /// </summary>
            /// <returns>
            /// The result of the comparison.
            /// </returns>
            /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
            public int Compare(MetaObject x, MetaObject y)
            {
                return x.Id.CompareTo(y.Id);
            }
        }
    }
}