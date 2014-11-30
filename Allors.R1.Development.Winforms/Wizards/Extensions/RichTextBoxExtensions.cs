// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxExtensions.cs" company="Allors bvba">
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
namespace Allors.R1.Development.Repository.WinForms.Wizards
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Allors.R1.Development.Repository.Commands;

    /// <summary>
    /// The pull up wizard.
    /// </summary>
    public static class RichTextBoxExtensions
    {
        public static void UpdateDependencies(this RichTextBox richTextBox, PullUp pullUp)
        {
            richTextBox.Text = string.Empty;

            foreach (var metaObject in pullUp.MetaObjectsToPull)
            {
                richTextBox.SelectionColor = Color.Red;
                richTextBox.SelectedText = metaObject + Environment.NewLine;
            }
        }

        public static void UpdateDependencies(this RichTextBox richTextBox, PushDown pushDown)
        {
            richTextBox.Text = string.Empty;

            foreach (var metaObject in pushDown.MetaObjectsToPush)
            {
                richTextBox.SelectionColor = Color.Red;
                richTextBox.SelectedText = metaObject + Environment.NewLine;
            }
        }
    }
}