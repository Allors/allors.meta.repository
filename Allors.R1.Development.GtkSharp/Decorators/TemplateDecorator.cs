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

namespace Allors.R1.Development.Repository.GtkSharp.Decorators
{
    using System;
    using System.ComponentModel;

    using Allors.R1.Development.Repository.Generation;

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
        public Guid Id
        {
            get { return this.template.Id; }
        }

        [Category("\u200BGeneral")]
        public string Name
        {
            get { return this.template.Name; }
            set { this.template.Name = value; }
        }

        [Category("\u200BGeneral")]
        public string Extension
        {
            get { return this.template.Extension; }
            set { this.template.Extension = value; }
        }

        [Category("\u200BGeneral")]
        public string Output
        {
            get { return this.template.Output; }
            set { this.template.Output = value; }
        }


        [Category("StringTemplate")]
        [DisplayName("Source")]
        public Uri Source
        {
            get { return this.template.Source; }
            set { this.template.Source = value; }
        }

        [Category("StringTemplate")]
        [DisplayName("Name")]
        public string StringTemplateName
        {
            get { return this.template.StringTemplate.Name; }
        }

        [Category("StringTemplate")]
        [DisplayName("Version")]
        public string StringTemplateVersion
        {
            get { return this.template.StringTemplate.Version; }
        }

        public override string ToString()
        {
            return this.template.Name;
        }
    }
}