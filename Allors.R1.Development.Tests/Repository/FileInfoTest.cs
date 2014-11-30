// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileInfoTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository.Storage
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class FileInfoTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void RelativeNameDifferentRoot()
        {
            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\1");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\1\1");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\");
                var fileInfo = new FileInfo(@"C:\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\1");
                var fileInfo = new FileInfo(@"C:\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"D:\");
                var fileInfo = new FileInfo(@"C:\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual(null, relativeFileName);
            }
        }

        [Test]
        public void RelativeNameSameRoot()
        {
            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("1/1/test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\1");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("1/test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\");
                var fileInfo = new FileInfo(@"C:\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("1/test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\1\1");
                var fileInfo = new FileInfo(@"C:\1\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\1");
                var fileInfo = new FileInfo(@"C:\1\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }

            {
                var baseDirectoryInfo = new DirectoryInfo(@"C:\");
                var fileInfo = new FileInfo(@"C:\test.txt");

                var allorsFileInfo = new AllorsFileInfo(fileInfo);
                var relativeFileName = allorsFileInfo.GetRelativeName(baseDirectoryInfo);

                Assert.AreEqual("test.txt".Replace('/', Path.DirectorySeparatorChar), relativeFileName);
            }
        }
    }
}