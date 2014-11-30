//------------------------------------------------------------------------------------------------- 
// <copyright file="MainWindow.cs" company="Allors bvba">
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
    using System.IO;

    using Allors.R1.Development.Repository.GtkSharp.Wizards;

    using GLib;
    using Gtk;

    using MonoDevelop.Components.PropertyGrid;

    using Window = Gtk.Window;
    using WindowType = Gtk.WindowType;

    public partial class MainWindow : Window
    {
        private readonly Explorer.Tree explorer;
        private readonly PropertyGrid propertyGrid;
        private readonly Validation.Grid grid;

        public MainWindow(string[] args) : base(WindowType.Toplevel)
        {
            ExceptionManager.UnhandledException += this.ExceptionManagerUnhandledException;
            this.Build();
            
            try
            {
                this.explorer = new Explorer.Tree(this);
                this.explorer.Selected += this.ExplorerSelected;
                var scrolledWindow = new ScrolledWindow { this.explorer };
                this.mainHPaned.Add1(scrolledWindow);

                this.propertyGrid = new PropertyGrid();
                this.propertyGrid.SetSizeRequest(500, 400);
                this.mainHPaned.Add2(this.propertyGrid);

                this.propertyGrid.ShowAll();

                this.grid = new Validation.Grid(this.explorer);
                this.grid.SetSizeRequest(500, 100);
                this.vbox1.Add(this.grid);

                this.ShowAll();
            }
            catch
            {
            }

            if (args.Length > 0)
            {
                var repository = new Repository(new DirectoryInfo(args[0]));
                this.explorer.Open(repository);
            }
        }

        protected void OnNewActionActivated(object sender, EventArgs e)
        {
            var addRepositoryWizard = new AddRepositoryWizard();
            try
            {
                if (addRepositoryWizard.Run() == (int)ResponseType.Ok)
                {
                    this.explorer.Open(addRepositoryWizard.Repository);
                }
            }
            finally
            {
                addRepositoryWizard.Destroy();
            }
        }
        
        protected void OnOpenActionActivated(object sender, EventArgs e)
        {
            var fileChooser = new FileChooserDialog(
                "Select Repository",
                this, 
                FileChooserAction.Open,
                "Cancel",
                ResponseType.Cancel,
                "Select",
                ResponseType.Accept);

            var filter = new FileFilter { Name = "Allors repository (*.repository)" };
            filter.AddPattern("*.repository");
            fileChooser.AddFilter(filter);

            try
            {
                if (fileChooser.Run() == (int)ResponseType.Accept)
                {
                    var directoryInfo = new FileInfo(fileChooser.Filename).Directory;
                    var repository = new Repository(directoryInfo);
                    this.explorer.Open(repository);
                }
            }
            finally
            {
                fileChooser.Destroy();
            }
        }

        protected void OnQuitActionActivated(object sender, EventArgs e)
        {
            Application.Quit();
        }

        protected void OnAdvancedActionToggled(object sender, EventArgs e)
        {
            this.explorer.AdvancedView = this.AdvancedAction.Active;
        }

        protected void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            Application.Quit();
            args.RetVal = true;
        }

        private void ExceptionManagerUnhandledException(UnhandledExceptionArgs args)
        {
            args.ExitApplication = false;

            try
            {
                string message;
                var exception = args.ExceptionObject as Exception;
                if (exception != null)
                {
                    if (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }

                    message = exception.Message + "\n" + exception.StackTrace;
                }
                else
                {
                    message = args.ExceptionObject.ToString();
                }

                message = Markup.EscapeText(message);

                var dialog = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Close,
                    message);
                dialog.Run();
                dialog.Destroy();
            }
            catch
            {
            }
        }

        private void ExplorerSelected(object sender, Explorer.SelectedEventArgs args)
        {
            this.propertyGrid.CurrentObject = args.SelectionForPropertyGrid;
        }
    }
}