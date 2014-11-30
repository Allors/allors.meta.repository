// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritanceTag.cs" company="Allors bvba">
// Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.R1.Development.Repository.WinForms
{
    using Allors.R1.Development.Repository;
    using Allors.R1.Meta;

    public sealed class InheritanceTag : Tag
    {
        public InheritanceTag(Repository repository, ObjectType objectType, Inheritance inheritance)
        {
            this.Repository = repository;
            this.ObjectType = objectType;
            this.Inheritance = inheritance;
        }

        public Repository Repository { get; private set; }

        public ObjectType ObjectType { get; private set; }

        public Inheritance Inheritance { get; private set; }

        public override bool Equals(object obj)
        {
            var that = obj as InheritanceTag;
            return that != null && this.Repository.Equals(that.Repository) && this.ObjectType.Equals(that.ObjectType) && this.Inheritance.Equals(that.Inheritance);
        }

        public override int GetHashCode()
        {
            return this.Repository.GetHashCode() ^ this.ObjectType.GetHashCode() ^ this.Inheritance.GetHashCode();
        }
    }
}