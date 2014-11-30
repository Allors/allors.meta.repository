//------------------------------------------------------------------------------------------------- 
// <copyright file="Inheritance.cs" company="Allors bvba">
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
// <summary>Defines the Inheritance type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;
    using AllorsGenerated;

    /// <summary>
    /// Defines a subtype/supertype relation between two <see cref="ObjectType"/>s.
    /// </summary>
    public sealed partial class Inheritance
    {
        /// <summary>
        /// Gets a value indicating whether this subtype is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this subtype is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsSubtypeDefault
        {
            get { return !ExistSubtype; }
        }

        /// <summary>
        /// Gets a value indicating whether the supertype is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if the supertype is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupertypeDefault
        {
            get { return !ExistSupertype; }
        }

        /// <summary>
        /// Gets or sets the subtype.
        /// </summary>
        /// <value>The Subtype.</value>
        public override ObjectType Subtype
        {
            get
            {
                return base.Subtype;
            }

            set
            {
                if (value != null && value.Equals(this.Supertype))
                {
                    throw new ArgumentException("Can not inherit from itself.");
                }

                base.Subtype = value;
                this.Domain.StaleInheritanceDerivations();
            }
        }

        /// <summary>
        /// Gets or sets the supertype.
        /// </summary>
        /// <value>The Supertype.</value>
        public override ObjectType Supertype
        {
            get
            {
                return base.Supertype;
            }

            set
            {
                if (value != null && value.Equals(this.Subtype))
                {
                    throw new ArgumentException("Can not inherit from itself.");
                }

                base.Supertype = value;
                this.Domain.StaleInheritanceDerivations();
            }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        protected override string ValidationName
        {
            get
            {
                if (ExistSupertype && ExistSubtype)
                {
                    return "inheritance " + this.Subtype + "::" + this.Supertype;
                }

                if (ExistId)
                {
                    return "inheritance " + this.Id;
                }

                return "unknown inheritance";
            }
        }

        /// <summary>
        /// Copy from source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Copy(Inheritance source)
        {
            this.CopyMetaObject(source);

            if (source.ExistSubtype)
            {
                this.Subtype = (ObjectType)this.Domain.Domain.Find(source.Subtype.Id);
            }

            if (source.ExistSupertype)
            {
                this.Supertype = (ObjectType)this.Domain.Domain.Find(source.Supertype.Id);
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            var domain = this.Domain;
            var deleteId = this.Id;

            this.Reset();
            base.Delete();
            
            domain.StaleInheritanceDerivations();
            domain.SendDeletedEvent(deleteId);
        }

        /// <summary>
        /// Removes the Id.
        /// </summary>
        public override void RemoveId()
        {
            if (ExistId)
            {
                throw new ArgumentException("Id is write once");
            }

            base.RemoveId();
        }

        /// <summary>
        /// Removes the Subtype.
        /// </summary>
        public override void RemoveSubtype()
        {
            base.RemoveSubtype();
            this.Domain.StaleInheritanceDerivations();
        }

        /// <summary>
        /// Removes the Supertype.
        /// </summary>
        public override void RemoveSupertype()
        {
            base.RemoveSupertype();
            this.Domain.StaleInheritanceDerivations();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.RemoveSubtype();
            this.RemoveSupertype();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return (ExistSubtype ? this.Subtype.Name : string.Empty) + "::" + (ExistSupertype ? this.Supertype.Name : string.Empty);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>The new inheritance.</returns>
        internal static Inheritance Create(AllorsEmbeddedSession session)
        {
            var inheritance = (Inheritance)session.Create(AllorsEmbeddedDomain.Inheritance);
            return inheritance;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (ExistSubtype && ExistSupertype)
            {
                if (this.Subtype.IsCyclicInheritance(this.Supertype))
                {
                    var message = this.ValidationName + " has a cycle in its inheritance hierarchy";
                    validationLog.AddError(message, this, ValidationKind.Cyclic, AllorsEmbeddedDomain.InheritanceSubtype);
                }

                var inheritance = this.Subtype.FindInheritanceWhereDirectSubtype(this.Supertype);
                if (inheritance != null && !this.Equals(inheritance))
                {
                    var message = "name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.InheritanceSupertype);
                }

                if (this.Supertype.IsClass && this.Subtype.ExistDirectSuperclass)
                {
                    var existingInheritance = this.Subtype.FindInheritanceWhereDirectSubtype(this.Subtype.DirectSuperclass);
                    if (!this.Equals(existingInheritance))
                    {
                        var message = "multiple class inheritance is not supported (" + this.ValidationName + ")";
                        validationLog.AddError(message, this, ValidationKind.Exclusive, AllorsEmbeddedDomain.InheritanceSupertype);
                    }
                }

                if (this.Supertype.IsConcreteComposite)
                {
                    var message = this.ValidationName + " can not have a concrete superclass";
                    validationLog.AddError(message, this, ValidationKind.Hierarchy, AllorsEmbeddedDomain.InheritanceSupertype);
                }

                if (this.Supertype.IsAbstractComposite && this.Subtype.IsInterface)
                {
                    var message = this.ValidationName + " can not have an abstract superclass";
                    validationLog.AddError(message, this, ValidationKind.Hierarchy, AllorsEmbeddedDomain.InheritanceSupertype);
                }
            }
            else
            {
                if (!ExistSubtype)
                {
                    var message = this.ValidationName + " has a missing Supertype";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.InheritanceSupertype);
                }
                else
                {
                    var message = this.ValidationName + " has a missing Subtype";
                    validationLog.AddError(message, this, ValidationKind.Unique, AllorsEmbeddedDomain.InheritanceSupertype);
                }
            }
        }
    }
}