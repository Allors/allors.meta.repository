// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexedObject.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Generation
{
    using System;
    using System.Collections.Generic;

    public sealed class IndexedObject
    {
        /// <summary>
        /// The accepted types are unit types and attributes.
        /// </summary>
        private static readonly HashSet<Type> AcceptedTypes = new HashSet<Type>
                                                                  {
                                                                      typeof(bool),
                                                                      typeof(long),
                                                                      typeof(string),
                                                                      typeof(IndexedObject)
                                                                  };

        /// <summary>
        /// The key value pairs.
        /// </summary>
        private KeyValuePair[] keyValuePairs;

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="name">
        /// The property name.
        /// </param>
        /// <returns>
        /// The value of the property.
        /// </returns>
        public object this[string name]
        {
            get
            {
                if (this.keyValuePairs == null)
                {
                    return null;
                }

                var key = name.ToLowerInvariant();

                if (key.StartsWith("exist") && key.Length > 5)
                {
                    return this[key.Substring(5)] != null;
                }
                
                foreach (var keyValuePair in this.keyValuePairs)
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        var o = keyValuePair.Value;
                        return o;
                    }
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    if (!AcceptedTypes.Contains(value.GetType()))
                    {
                        throw new ArgumentException("Non supported property type: " + value.GetType());
                    }
                }

                var key = name.ToLowerInvariant();
                
                if (value == null)
                {
                    this.Remove(key);
                }
                else
                {
                    this.Upsert(key, value);
                }
            }
        }

        /// <summary>
        /// Removes a key value pair.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        private void Remove(string key)
        {
            if (this.keyValuePairs != null)
            {
                foreach (var keyValuePair in this.keyValuePairs)
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        if (this.keyValuePairs.Length == 1)
                        {
                            this.keyValuePairs = null;
                        }
                        else
                        {
                            var list = new List<KeyValuePair>(this.keyValuePairs);
                            list.Remove(keyValuePair);
                            this.keyValuePairs = list.ToArray();
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Update or insert the key value pair.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void Upsert(string key, object value)
        {
            if (this.keyValuePairs == null)
            {
                this.keyValuePairs = new[] { new KeyValuePair(key, value) };
            }
            else
            {
                foreach (var keyValuePair in this.keyValuePairs)
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        keyValuePair.Value = value;
                        return;
                    }
                }

                var list = new List<KeyValuePair>(this.keyValuePairs.Length + 1);
                list.AddRange(this.keyValuePairs);
                list.Add(new KeyValuePair(key, value));
                this.keyValuePairs = list.ToArray();
            }
        }

        /// <summary>
        /// A key value pair, that is not a struct (no boxing) and
        /// where the value is updateable.
        /// </summary>
        private sealed class KeyValuePair
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyValuePair"/> class.
            /// </summary>
            /// <param name="key">
            /// The key.
            /// </param>
            /// <param name="value">
            /// The value.
            /// </param>
            internal KeyValuePair(string key, object value)
            {
                this.Key = key;
                this.Value = value;
            }

            /// <summary>
            /// Gets the key.
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            public object Value { get; set; }
        }
    }
}