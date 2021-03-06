// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractClassPropertyDescriptor.cs" company="Allors bvba">
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

    using Allors.R1.Development.Repository.GtkSharp.Editors;
    using Allors.R1.Development.Repository;

    using MonoDevelop.Components.PropertyGrid;

    public class AbstractClassPropertyDescriptor : PropertyDescriptorBase
    {
        private readonly Repository repository;

        public AbstractClassPropertyDescriptor(Repository repository, PropertyDescriptor subject)
            : base(subject)
        {
            this.repository = repository;
        }

        public override object GetEditor(Type editorBaseType)
        {
            if (editorBaseType == typeof(PropertyEditorCell))
            {
                return new AbstractClassEditor(this.repository);
            }

            return base.GetEditor(editorBaseType);
        }
    }
}