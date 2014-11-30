//------------------------------------------------------------------------------------------------- 
// <copyright file="MethodType.cs" company="Allors bvba">
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
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Meta
{
    using System;
    using System.Reflection;

    using AllorsGenerated;

    public partial class MethodType
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                this.StaleMethodTypeDerivations();
                base.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the ObjectType.
        /// </summary>
        /// <value>The ObjectType.</value>
        public override ObjectType ObjectType
        {
            get
            {
                return base.ObjectType;
            }

            set
            {
                this.StaleMethodTypeDerivations();
                base.ObjectType = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="ObjectType"/> is default.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance's <see cref="ObjectType"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsObjectTypeDefault
        {
            get { return !ExistObjectType; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance's <see cref="Name"/> is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's <see cref="Name"/> is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsNameDefault
        {
            get { return !ExistName; }
        }

        /// <summary>
        /// Gets the validation name.
        /// </summary>
        /// <value>The validation name.</value>
        protected override string ValidationName
        {
            get
            {
                if (ExistName)
                {
                    return "method type " + this.Name;
                }

                return "unknown method type";
            }
        }

        /// <summary>
        /// Copy from source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void Copy(MethodType source)
        {
            this.CopyMetaObject(source);

            this.Name = source.Name;
            if (source.ExistObjectType)
            {
                ObjectType = (ObjectType)this.Domain.Domain.Find(source.ObjectType.Id);
            }
        }

        /// <summary>
        /// Removes the Name.
        /// </summary>
        public override void RemoveName()
        {
            this.StaleMethodTypeDerivations();
            base.RemoveName();
        }

        /// <summary>
        /// Removes the ObjectType.
        /// </summary>
        public override void RemoveObjectType()
        {
            this.StaleMethodTypeDerivations();
            base.RemoveObjectType();
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
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.RemoveName();
            this.RemoveObjectType();
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            var deleteId = this.Id;
            var domain = this.Domain;

            this.Reset();
            base.Delete();

            domain.SendDeletedEvent(deleteId);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>The new instance.</returns>
        internal static MethodType Create(AllorsEmbeddedSession session)
        {
            var methodType = (MethodType)session.Create(AllorsEmbeddedDomain.MethodType);
            methodType.Reset();
            return methodType;
        }
        
        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        protected internal override void Validate(ValidationLog validationLog)
        {
            base.Validate(validationLog);

            if (!ExistName)
            {
                var message = this.ValidationName + " has no name";
                validationLog.AddError(message, this, ValidationKind.Required, AllorsEmbeddedDomain.MethodTypeName);
            }
        }

        /// <summary>
        /// Stales the relation type derivations.
        /// </summary>
        private void StaleMethodTypeDerivations()
        {
            this.Domain.StaleMethodTypeDerivations();
        }
    }
}