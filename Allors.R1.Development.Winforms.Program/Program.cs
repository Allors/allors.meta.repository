// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
//   // 
//   // Dual Licensed under
//   //   a) the Lesser General Public Licence v3 (LGPL)
//   //   b) the Allors License
//   // 
//   // The LGPL License is included in the file lgpl.txt.
//   // The Allors License is an addendum to your contract.
//   // 
//   // Allors Platform is distributed in the hope that it will be useful,
//   // but WITHOUT ANY WARRANTY; without even the implied warranty of
//   // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   // GNU General Public License for more details.
//   // 
//   // For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.R1.Development.Repository.Winforms.Program
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The Allors Development Winforms Program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>
        /// The status code.
        /// </returns>
        [STAThread]
        public static int Main(string[] args)
        {
            MainForm mainForm;
            if (args.Length == 0)
            {
                mainForm = new MainForm();
            }
            else
            {
                var path = args[0];
                mainForm = new MainForm(path, false);
            }

            Application.Run(mainForm);
            return 0;
        }
    }
}