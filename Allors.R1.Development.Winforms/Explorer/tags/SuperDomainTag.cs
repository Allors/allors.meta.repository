// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperDomainTag.cs" company="Allors bvba">
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

    public sealed class SuperDomainTag : Tag
    {
        public SuperDomainTag(Repository repository, Domain superDomain)
        {
            this.Repository = repository;
            this.SuperDomain = superDomain;
        }

        public Repository Repository { get; private set; }

        public Domain SuperDomain { get; private set; }

        public override bool Equals(object obj)
        {
            var that = obj as SuperDomainTag;
            return that != null && this.Repository.Equals(that.Repository) && this.SuperDomain.Equals(that.SuperDomain);
        }

        public override int GetHashCode()
        {
            return this.Repository.GetHashCode() ^ this.SuperDomain.GetHashCode();
        }
    }
}