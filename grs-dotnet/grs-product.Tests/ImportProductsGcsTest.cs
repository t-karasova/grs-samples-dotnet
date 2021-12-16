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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace grs_product.Tests
{
    [TestClass]
    public class ImportProductsGcsTest
    {
        private const string ProductFolderName = "grs-product";
        private static readonly string WorkingDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, ProductFolderName);

        private const string CMDFileName = "cmd.exe";
        private const string CommandLineArguments = "/c " + "dotnet run -- ImportProductsGcs"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestOutputImportProductsGcs()
        {
            string consoleOutput = string.Empty;

            var processStartInfo = new ProcessStartInfo(CMDFileName, CommandLineArguments)
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

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Import products from google cloud source. request:(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)\"inputUris\":(.*)\"gs://.*/products.json\"(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)The operation was started:(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)projects/(.*)/locations/global/catalogs/default_catalog/branches/0/operations/import-products(.*)").Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Number of successfully imported products:(.*)316(.*)").Success);
        }
    }
}
