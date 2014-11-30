// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateTest.cs" company="Allors bvba">
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

namespace Allors.R1.Development.Repository
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class DuplicateTest
    {
        private DirectoryInfo directoryInfo;
        private Repository repository;

        private static FileInfo TemplateSourceFileInfo
        {
            get { return new FileInfo(Path.Combine(TemplatesSourceDirectoryInfo.FullName, "allors.repository.stg")); }
        }

        private static DirectoryInfo TemplatesSourceDirectoryInfo
        {
            get
            {
                var allorsRoot = new DirectoryInfo(".");
                return new DirectoryInfo(Path.Combine(allorsRoot.FullName, @"repository\templates"));
            }
        }

        [SetUp]
        public void SetUp()
        {
            this.directoryInfo = new DirectoryInfo("template");
            this.directoryInfo.DeleteRecursive();

            this.directoryInfo.Create();

            this.repository = new Repository(this.directoryInfo, true);
        }

        [Test]
        public void GenerateFromFile()
        {
            var domain = this.repository.Domain;
            domain.Name = "MyDomain";
            domain.SendChangedEvent();

            var thisType = domain.AddDeclaredObjectType(Guid.NewGuid());
            thisType.SingularName = "ThisType";
            thisType.PluralName = "TheseTypes";

            thisType.IsAbstract = true;
            thisType.SendChangedEvent();

            var thatType = domain.AddDeclaredObjectType(Guid.NewGuid());
            thatType.SingularName = "ThatType";
            thatType.PluralName = "ThatTypes";
            thatType.SendChangedEvent();

            var superType = domain.AddDeclaredObjectType(Guid.NewGuid());
            superType.SingularName = "SuperType";
            superType.PluralName = "Supertypes";
            superType.IsInterface = true;
            superType.SendChangedEvent();

            var inheritance = thisType.AddDirectSupertype(superType);
            inheritance.SendChangedEvent();

            var relationTypeWhereAssociation = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereAssociation.AssociationType.ObjectType = thisType;
            relationTypeWhereAssociation.RoleType.ObjectType = thatType;
            relationTypeWhereAssociation.SendChangedEvent();

            var relationTypeWhereRole = domain.AddDeclaredRelationType(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            relationTypeWhereRole.AssociationType.ObjectType = thatType;
            relationTypeWhereRole.AssociationType.AssignedSingularName = "from";
            relationTypeWhereRole.AssociationType.AssignedPluralName = "froms";
            relationTypeWhereRole.RoleType.ObjectType = thisType;
            relationTypeWhereRole.RoleType.AssignedSingularName = "to";
            relationTypeWhereRole.RoleType.AssignedPluralName = "tos";
            relationTypeWhereRole.SendChangedEvent();

            Assert.IsTrue(domain.IsValid);

            var template = this.repository.AddTemplate();
            template.Name = "repository";
            template.Source = new Uri(TemplateSourceFileInfo.FullName);

            template.Generate(new DummyLog());

            // TODO: Checks
            var duplicateRepository = new Repository(directoryInfo);
        }

        private class DummyLog : Log
        {
            public override void Error(object sender, string message)
            {
            }
        }
    }
}