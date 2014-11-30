// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Resources
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;

    using Microsoft.Build.Evaluation;

    using NUnit.Framework;

    [TestFixture]
    public class MergeTest
    {
        private readonly DirectoryInfo outpuDirectory = new DirectoryInfo("../../Allors.R1.Development.Tests.Resources");
        private readonly DirectoryInfo inputDirectory1 = new DirectoryInfo("../../Allors.R1.Development.Tests.Resources/1");
        private readonly DirectoryInfo inputDirectory2 = new DirectoryInfo("../../Allors.R1.Development.Tests.Resources/2");
        private readonly DirectoryInfo inputDirectory3 = new DirectoryInfo("../../Allors.R1.Development.Tests.Resources/3");
        private readonly DirectoryInfo inputDirectory4 = new DirectoryInfo("../../Allors.R1.Development.Tests.Resources/4");

        private readonly FileInfo fileA = new FileInfo("../../Allors.R1.Development.Tests.Resources/a.resx");
        private readonly FileInfo fileAnl = new FileInfo("../../Allors.R1.Development.Tests.Resources/a-nl.resx");

        private readonly FileInfo mergeProjectFile = new FileInfo("../../Allors.R1.Development.Tests.Resources/Merge.proj");

        [SetUp]
        public void SetUp()
        {
            this.fileA.Delete();
            this.fileAnl.Delete();
        }

        [Test]
        public void All()
        {
            var inputDirectories = new[] { this.inputDirectory1, this.inputDirectory2, this.inputDirectory3, this.inputDirectory4 };

            var resources = new Resources(inputDirectories, this.outpuDirectory);
            resources.Merge();

            var a = this.GetResources(this.fileA);
            Assert.AreEqual("4", a["value"]);

            var anl = this.GetResources(this.fileAnl);
            Assert.AreEqual("3-nl", anl["value"]);
        }

        [Test]
        public void MsBuild()
        {
            var projectCollection = new ProjectCollection();
            var project = projectCollection.LoadProject(this.mergeProjectFile.FullName);
            project.Build();

            var a = this.GetResources(this.fileA);
            Assert.AreEqual("4", a["value"]);

            var anl = this.GetResources(this.fileAnl);
            Assert.AreEqual("3-nl", anl["value"]);
        }

        private Dictionary<string,object> GetResources(FileInfo fileInfo)
        {
            var dictionary = new Dictionary<string, object>();

            using (var resxReader = new ResXResourceReader(fileInfo.FullName))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    dictionary.Add((string)entry.Key, entry.Value);
                }
            }

            return dictionary;
        }
    }
}