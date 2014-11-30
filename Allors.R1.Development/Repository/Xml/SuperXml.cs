// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuperXml.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Xml
{
    using System;
    using System.Xml.Serialization;

    public class SuperXml : IComparable
    {
        [XmlAttribute("id")]
        public Guid id;

        public string location;

        internal SuperXml(Guid superId, string path)
        {
            this.id = superId;
            this.location = path;
        }

        protected SuperXml()
        {
        }

        public int CompareTo(object obj)
        {
            var that = obj as SuperXml;
            if (that != null)
            {
                return this.id.CompareTo(that.id);
            }

            return -1;
        }
    }
}