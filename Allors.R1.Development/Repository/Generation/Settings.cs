// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Linq;

    using Allors.R1.Development.Repository.Generation.Xml;

    public sealed class Settings
    {
        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                var key = name.Trim().ToLowerInvariant();

                string value;
                this.Dictionary.TryGetValue(key, out value);
                return value;
            }

            set
            {
                var key = name.Trim().ToLowerInvariant();
                this.Dictionary[key] = value;
            }
        }

        public Dictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        /// <summary>
        /// Creates an IndexedObject for this Configuration.
        /// </summary>
        /// <returns>the indexed object</returns>
        public IndexedObject CreateIndexedObject()
        {
            var root = new IndexedObject();

            foreach (var keyValuePair in this.dictionary)
            {
                object value;

                var stringValue = keyValuePair.Value;

                bool boolValue;
                if (bool.TryParse(stringValue, out boolValue))
                {
                    value = boolValue;
                }
                else
                {
                    long longValue;
                    if (long.TryParse(stringValue, out longValue))
                    {
                        value = longValue;
                    }
                    else
                    {
                        value = stringValue;
                    }
                }

                var keys = keyValuePair.Key.Split('.');

                var current = root;
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];

                    if (i == keys.Length - 1)
                    {
                        current[key] = value;
                    }
                    else
                    {
                        var next = (IndexedObject)current[key];
                        if (next == null)
                        {
                            next = new IndexedObject();
                            current[key] = next;
                        }

                        current = next;
                    }
                }
            }

            return root;
        }

        internal void Load(TemplateXml xml)
        {
            this.dictionary.Clear();
            foreach (var setting in xml.settings)
            {
                this.dictionary[setting.key] = setting.value;
            }
        }

        internal void Save(TemplateXml xml)
        {
            xml.settings = new List<SettingsXml>(this.dictionary.Select(keyValuePair => new SettingsXml(keyValuePair.Key, keyValuePair.Value)));
        }
    }
}