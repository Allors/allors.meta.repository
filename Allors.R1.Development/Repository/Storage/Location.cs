// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Allors bvba">
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
    using System;
    using System.IO;

    public class Location
    {
        private readonly DirectoryInfo directory;

        public Location(DirectoryInfo directoryInfo)
        {
            directory = directoryInfo;
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return directory; }
        }

        public void Save(string fileName, string fileContents)
        {
            FileInfo fileInfo = new FileInfo(directory.FullName + Path.DirectorySeparatorChar + fileName.Replace('\\', Path.DirectorySeparatorChar));
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            if (fileInfo.Exists)
            {
                string existingFileContents = File.ReadAllText(fileInfo.FullName);
                if (!fileContents.Equals(existingFileContents))
                {
                    File.WriteAllText(fileInfo.FullName, fileContents);
                    fileInfo.CreationTime = DateTime.Now;
                }
            }
            else
            {
                File.WriteAllText(fileInfo.FullName, fileContents);
                fileInfo.CreationTime = DateTime.Now;
            }
        }
    }
}