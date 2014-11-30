//------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectId.cs" company="Allors bvba">
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
// <summary>Defines the ObjectId type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Every <see cref="IObject"/> has an Id.
    /// </summary>
    [Serializable]
    public abstract class ObjectId : IComparable
    {
        /// <summary>
        /// An empty array of object ids.
        /// </summary>
        public static readonly ObjectId[] EmptyObjectIds = { };

        /// <summary>
        ///  Gets the value of the Id.
        /// </summary>
        /// <value>The value.</value>
        public abstract object Value { get; }

        /// <summary>
        /// Gets a value that can be used as a properties key 
        /// in a <see cref="IPopulation"/> or <see cref="ISession"/>.
        /// </summary>
        /// <value>The value.</value>
        public string Key
        {
            get
            {
                return string.Format("Allors[{0}]", this);
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance. </exception>
        public abstract int CompareTo(object obj);
    }
}