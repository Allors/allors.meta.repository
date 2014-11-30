// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDescriptorBase.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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

namespace Allors.R1.Development.Repository.GtkSharp.PropertyDescriptors
{
    using System;
    using System.ComponentModel;

    public abstract class PropertyDescriptorBase : PropertyDescriptor
    {
        private readonly PropertyDescriptor subject;

        protected PropertyDescriptorBase(PropertyDescriptor subject)
            : base(subject)
        {
            this.subject = subject;
        }

        public override Type ComponentType
        {
            get
            {
                return this.subject.ComponentType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this.subject.IsReadOnly;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.subject.PropertyType;
            }
        }

        public override bool CanResetValue(object component)
        {
            return this.subject.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return this.subject.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            this.subject.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            this.subject.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.subject.ShouldSerializeValue(component);
        }
    }
}