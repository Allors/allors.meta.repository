//------------------------------------------------------------------------------------------------- 
// <copyright file="AssemblyInfo.cs" company="Allors bvba">
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
// <summary>Defines the Default type.</summary>
//-------------------------------------------------------------------------------------------------
using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Allors bvba")]
[assembly: AssemblyProduct("Allors Platform")]
[assembly: AssemblyCopyright("(c) 2002-2010 Allors bvba")]
[assembly: AssemblyTrademark("Allors is a registered trademark")]
[assembly: AssemblyCulture("")] // don't specify "neutral" because some CLRs will balk on it

// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Allors.Development.Tests")]
[assembly: AssemblyDescription("The tests for the Allors Development library.")]
[assembly: AssemblyConfiguration("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]