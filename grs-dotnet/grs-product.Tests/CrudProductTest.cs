// Copyright 2021 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace grs_product.Tests
{
    [TestClass]
    public class CrudProductTest
    {
        private const string ProductFolderName = "grs-product";

        private const string DotNetCommand = "dotnet run -- CrudProduct";

        private const string WindowsTerminalName = "cmd.exe";
        private const string UnixTerminalName = "/bin/bash";
        private const string WindowsTerminalPrefix = "/c ";
        private const string UnixTerminalPrefix = "-c ";
        private const string WindowsTerminalQuotes = "";
        private const string UnixTerminalQuotes = "\"";

        private static readonly string WorkingDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, ProductFolderName);

        private static readonly bool CurrentOSIsWindows = Environment.OSVersion.VersionString.Contains("Windows");
        private static readonly string CurrentTerminalPrefix = CurrentOSIsWindows ? WindowsTerminalPrefix : UnixTerminalPrefix;
        private static readonly string CurrentTerminalFile = CurrentOSIsWindows ? WindowsTerminalName : UnixTerminalName;
        private static readonly string CurrentTerminalQuotes = CurrentOSIsWindows ? WindowsTerminalQuotes : UnixTerminalQuotes;

        private static readonly string CommandLineArguments = CurrentTerminalPrefix + CurrentTerminalQuotes + DotNetCommand + CurrentTerminalQuotes;

        [TestMethod]
        public void TestOutputCrudProduct()
        {
            string consoleOutput = string.Empty;

            var processStartInfo = new ProcessStartInfo(CurrentTerminalFile, CommandLineArguments)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = WorkingDirectory
            };

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;

                process.Start();

                consoleOutput = process.StandardOutput.ReadToEnd();
            }

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Create product. request:(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Create product. request:(.*)\"parent\": \"projects/(.*)/locations/global/catalogs/default_catalog/branches/default_branch\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Create product. request:(.*)\"title\": \"Nest Mini\"(.*)", RegexOptions.Singleline).Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Created product:(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Created product:(.*)\"id\": \"crud_product_id\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Created product:(.*)\"title\": \"Nest Mini\"(.*)", RegexOptions.Singleline).Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Get product. request:(.*)\"name\": \"projects/(.*)/locations/global/catalogs/default_catalog/branches/default_branch/products/crud_product_id\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Get product. request:(.*)\"name\": \"projects/(.*)/locations/global/catalogs/default_catalog/branches/default_branch/products/crud_product_id\"(.*)", RegexOptions.Singleline).Success);


            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Updated product:(.*)\"price\":(.*)20(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Product (.*) was deleted(.*)").Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Update product. request:(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Update product. request:(.*)\"name\": \"projects/(.*)/locations/global/catalogs/default_catalog/branches/default_branch/products/crud_product_id\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Update product. request:(.*)\"title\": \"Updated Nest Mini\"(.*)", RegexOptions.Singleline).Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Delete product. request:(.*)\"name\": \"projects/(.*)/locations/global/catalogs/default_catalog/branches/default_branch/products/crud_product_id\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Product (.*) was deleted(.*)").Success);
        }
    }
}