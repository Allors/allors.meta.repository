// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageTest.cs" company="Allors bvba">
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
    using System.Collections;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class StorageTest
    {
        private DirectoryInfo directoryInfo;

        [SetUp]
        public void SetUp()
        {
            this.directoryInfo = new DirectoryInfo("storage");
            this.directoryInfo.DeleteRecursive();

            this.directoryInfo.Create();
        }

        [Test]
        public void Save()
        {
            var contentsByFileNames = new Hashtable();

            for (var i = 0; i < 3; i++)
            {
                contentsByFileNames["file" + i] = "Contents " + i;
                for (var j = 0; j < 3; j++)
                {
                    contentsByFileNames["directory" + i + "/file" + i] = "Contents " + i + " " + j;
                    for (var k = 0; k < 3; k++)
                    {
                        contentsByFileNames["directory" + i + "/subdirectory" + j + "/file" + k] = "Contents " + i + " " + j + " " + k;
                    }
                }
            }

            // Initial Contents
            var location = new Location(this.directoryInfo);
            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = (string)contentsByFileName.Key;
                var fileContents = (string)contentsByFileName.Value;
                location.Save(fileName, fileContents);
            }

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = ((string)contentsByFileName.Key).Replace('/', Path.DirectorySeparatorChar);
                var fileContents = (string)contentsByFileName.Value;

                var fileInfo = new FileInfo(this.directoryInfo.FullName + Path.DirectorySeparatorChar + fileName);
                var readContents = File.ReadAllText(fileInfo.FullName);

                Assert.AreEqual(fileContents, readContents);
            }

            // MetaObjectChanged contents
            foreach (string fileName in new ArrayList(contentsByFileNames.Keys))
            {
                var newFileContents = (string)contentsByFileNames[fileName] + " (modified)";
                contentsByFileNames[fileName] = newFileContents;
            }

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = (string)contentsByFileName.Key;
                var fileContents = (string)contentsByFileName.Value;
                location.Save(fileName, fileContents);
            }

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = ((string)contentsByFileName.Key).Replace('/', Path.DirectorySeparatorChar);
                var fileContents = (string)contentsByFileName.Value;

                var fileInfo = new FileInfo(this.directoryInfo.FullName + Path.DirectorySeparatorChar + fileName);
                var readContents = File.ReadAllText(fileInfo.FullName);

                Assert.AreEqual(fileContents, readContents);
            }

            // Same contents
            var timeStampByFileName = new Hashtable();

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = ((string)contentsByFileName.Key).Replace('/', Path.DirectorySeparatorChar);
                var fileInfo = new FileInfo(this.directoryInfo.FullName + Path.DirectorySeparatorChar + fileName);
                timeStampByFileName[fileName] = fileInfo.CreationTime;
            }

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = (string)contentsByFileName.Key;
                var fileContents = (string)contentsByFileName.Value;
                location.Save(fileName, fileContents);
            }

            foreach (DictionaryEntry contentsByFileName in contentsByFileNames)
            {
                var fileName = ((string)contentsByFileName.Key).Replace('/', Path.DirectorySeparatorChar);
                var fileContents = (string)contentsByFileName.Value;

                var fileInfo = new FileInfo(this.directoryInfo.FullName + Path.DirectorySeparatorChar + fileName);
                fileInfo.Refresh();

                Assert.AreEqual(timeStampByFileName[fileName], fileInfo.CreationTime);

                var readContents = File.ReadAllText(fileInfo.FullName);
                Assert.AreEqual(fileContents, readContents);
            }
        }
    }
}