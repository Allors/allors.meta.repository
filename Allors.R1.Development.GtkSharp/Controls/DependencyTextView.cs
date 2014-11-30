//------------------------------------------------------------------------------------------------- 
// <copyright file="DependencyTextView.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
//-------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.GtkSharp
{
    using System;
    using System.Text;

    using Allors.R1.Development.Repository.Commands;

    using Gtk;

    public class DependencyTextView : TextView
    {
        public void Update(PullUp pullUp)
        {
            this.Buffer.Clear();

            var text = new StringBuilder();
            foreach (var metaObject in pullUp.MetaObjectsToPull)
            {
                text.Append(metaObject + Environment.NewLine);
            }

            this.Buffer.Text = text.ToString();
        }

        public void Update(PushDown pushDown)
        {
            this.Buffer.Clear();

            foreach (var metaObject in pushDown.MetaObjectsToPush)
            {
                this.Buffer.Text += metaObject + Environment.NewLine;
            }
        }
    }
}