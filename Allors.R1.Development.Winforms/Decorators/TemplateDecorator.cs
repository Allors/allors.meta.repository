// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateDecorator.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.WinForms.Decorators
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using Allors.R1.Development.Repository.Generation;
    using Allors.R1.Development.Repository.WinForms.Converters;
    using Allors.R1.Development.Repository.WinForms.Objects;

    [TypeConverter(typeof(PropertySorter))]
    public class TemplateDecorator
    {
        private readonly Template template;

        public TemplateDecorator(Template template)
        {
            this.template = template;
        }

        [Browsable(false)]
        public Template Template
        {
            get { return this.template; }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(1)]
        public Guid Id
        {
            get { return this.template.Id; }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(2)]
        public string Name
        {
            get
            {
                return this.template.Name;
            }

            set
            {
                this.template.Name = value;
                this.SendChangedEvent();
            }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(3)]
        public string Extension
        {
            get
            {
                return this.template.Extension;
            }

            set
            {
                this.template.Extension = value;
                this.SendChangedEvent();
            }
        }

        [Category("\u200BGeneral")]
        [PropertyOrder(4)]
        public string Output
        {
            get
            {
                return this.template.Output;
            }

            set
            {
                this.template.Output = value;
                this.SendChangedEvent();
            }
        }

        [TypeConverter(typeof(SettingsConverter))]
        [Category("\u200BGeneral")]
        [PropertyOrder(5)]
        public Setting[] Settings
        {
            get
            {
                return this.template.Settings.Dictionary.Select(keyValuePair => new Setting { Key = keyValuePair.Key, Value = keyValuePair.Value }).ToArray();
            }

            set
            {
                this.template.Settings.Dictionary.Clear();

                if (value != null)
                {
                    foreach (var setting in value)
                    {
                        if (!string.IsNullOrWhiteSpace(setting.Key) && !string.IsNullOrWhiteSpace(setting.Value))
                        {
                            this.template.Settings.Dictionary[setting.Key] = setting.Value;
                        }
                    }
                }

                this.SendChangedEvent();
            }
        }
        
        [Category("StringTemplate")]
        [DisplayName("Source")]
        [PropertyOrder(1)]
        public Uri Source
        {
            get
            {
                return this.template.Source;
            }

            set
            {
                this.template.Source = value;
                this.SendChangedEvent();
            }
        }

        [Category("StringTemplate")]
        [DisplayName("Name")]
        [PropertyOrder(2)]
        public string StringTemplateName
        {
            get { return this.template.StringTemplate.Name; }
        }

        [Category("StringTemplate")]
        [DisplayName("Version")]
        [PropertyOrder(3)]
        public string StringTemplateVersion
        {
            get { return this.template.StringTemplate.Version; }
        }

        public override string ToString()
        {
            return this.template.Name;
        }

        private void SendChangedEvent()
        {
            this.template.SendChangedEvent();
        }
    }
}