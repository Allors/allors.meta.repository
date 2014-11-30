//------------------------------------------------------------------------------------------------- 
// <copyright file="Icons.cs" company="Allors bvba">
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

    using Gdk;

    using Gtk;

    using GC = Gdk.GC;

    public static class Icons
    {
        private static bool init;

        static Icons()
        {
            init = false;
        }

        public static void Init()
        {
            if (!init)
            {
                init = true;

                var iconFactory = new IconFactory();
                iconFactory.Add("allors-program", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.allors.ico")));
                iconFactory.Add("allors-association", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.association.ico")));
                iconFactory.Add("allors-class", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.class.ico")));
                iconFactory.Add("allors-class-locked", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.class-locked.ico")));
                iconFactory.Add("allors-cluster", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.cluster.ico")));
                iconFactory.Add("allors-domain", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.domain.ico")));
                iconFactory.Add("allors-domain-environment", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.domainenvironment.ico")));
                iconFactory.Add("allors-except", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.except.ico")));
                iconFactory.Add("allors-extension", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.extension.ico")));
                iconFactory.Add("allors-filter", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.filter.ico")));
                iconFactory.Add("allors-interface", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.interface.ico")));
                iconFactory.Add("allors-interface-locked", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.interface-locked.ico")));
                iconFactory.Add("allors-intersect", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.intersect.ico")));
                iconFactory.Add("allors-method", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.method.ico")));
                iconFactory.Add("allors-method-locked", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.method-locked.ico")));
                iconFactory.Add("allors-namespace-locked", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.namespace-locked.ico")));
                iconFactory.Add("allors-predicate", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.predicate.ico")));
                iconFactory.Add("allors-role", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.role.ico")));
                iconFactory.Add("allors-union", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.union.ico")));
                iconFactory.Add("allors-namespace", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.namespace.ico")));
                iconFactory.Add("allors-role-locked", new IconSet(Pixbuf.LoadFromResource("Allors.R1.Development.GtkSharp.Icons.role-locked.ico")));
                iconFactory.AddDefault();            
            }
        }

        public static Pixbuf LoadIcon(Widget widget, string name, IconSize size)
        {
            var res = widget.RenderIcon(name, size, null);
            if (res != null)
            {
                return res;
            }

            int sz;
            int sy;
            Icon.SizeLookup(size, out sz, out sy);
            try
            {
                return IconTheme.Default.LoadIcon(name, sz, 0);
            }
            catch (Exception)
            {
                if (name != "gtk-missing-image")
                {
                    return LoadIcon(widget, "gtk-missing-image", size);
                }

                var pmap = new Pixmap(Screen.Default.RootWindow, sz, sz);
                var gc = new GC(pmap)
                {
                    RgbFgColor = new Color(255, 255, 255)
                };
                pmap.DrawRectangle(gc, true, 0, 0, sz, sz);
                gc.RgbFgColor = new Color(0, 0, 0);
                pmap.DrawRectangle(gc, false, 0, 0, sz - 1, sz - 1);
                gc.SetLineAttributes(3, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
                gc.RgbFgColor = new Color(255, 0, 0);
                pmap.DrawLine(gc, sz / 4, sz / 4, (sz - 1) - (sz / 4), (sz - 1) - (sz / 4));
                pmap.DrawLine(gc, (sz - 1) - (sz / 4), sz / 4, sz / 4, (sz - 1) - (sz / 4));
                return Pixbuf.FromDrawable(pmap, pmap.Colormap, 0, 0, 0, 0, sz, sz);
            }
        }
    }
}

