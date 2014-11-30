//------------------------------------------------------------------------------------------------- 
// <copyright file="SessionCreatedEventArgs.cs" company="Allors bvba">
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
// <summary>Defines the SessionCreatedEventArgs type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.R1
{
    /// <summary>
    /// The <see cref="IDatabase"/> raises an <see cref="SessionCreatedEventArgs"/> event 
    /// a new <see cref="ISession"/> is created.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="SessionCreatedEventArgs"/>.</param>
    public delegate void SessionCreatedEventHandler(object sender, SessionCreatedEventArgs args);

    /// <summary>
    /// The session created event arguments.
    /// </summary>
    public class SessionCreatedEventArgs
    {
        /// <summary>
        /// The <see cref="ISession"/> that was created.
        /// </summary>
        private readonly ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCreatedEventArgs"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SessionCreatedEventArgs(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Gets the session that was created.
        /// </summary>
        /// <value>The session that was created.</value>
        public ISession Session
        {
            get { return this.session; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Session.ToString();
        }
    }
}