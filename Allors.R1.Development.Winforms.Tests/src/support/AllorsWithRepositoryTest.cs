// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsWithRepositoryTest.cs" company="Allors bvba">
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

namespace AllorsTestWindowsTests
{
    using System.IO;
    using Allors.R1.Development.Repository;

    public abstract class AllorsWithRepositoryTest : AllorsWithoutRepositoryTest
    {
        public override void SetUp()
        {
            base.SetUp();
            this.Repository = new Repository(RepositoryDirectory, true);
        }

        protected FileInfo SaveTemplate(string fileName, string template)
        {
            var fileInfo = new FileInfo(fileName);
            File.WriteAllText(fileInfo.FullName, template);
            return fileInfo;
        }
    }
}