﻿// Copyright 2021 Google Inc. All Rights Reserved.
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

using grs_search.search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace grs_search.Tests.search
{
    [TestClass]
    public class UpdateAttributeConfigurationTest
    {
        private const string SearchFolderName = "grs-search";
        private static readonly string WorkingDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, SearchFolderName);

        private const string CMDFileName = "cmd.exe";
        private const string CommandLineArguments = "/c " + "dotnet run -- UpdateAttributeConfiguration"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestUpdateAttributeConfiguration()
        {
            const string ExpectedProductId = "GGOEAAEC172013";
            const string ExpectedKey = "ecofriendly";

            var product = UpdateAttributeConfiguration.UpdateRetailProduct();

            Assert.AreEqual(ExpectedProductId, product.Id);
            Assert.IsTrue(product.Attributes.Keys.Contains(ExpectedKey));
        }

        [TestMethod]
        public void TestOutputUpdateAttributeConfiguration()
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

            Assert.IsTrue(consoleOutput.Contains("Update product. request:"));
            Assert.IsTrue(consoleOutput.Contains("Updated product:"));
            // Check the response contains some products
            Assert.IsTrue(consoleOutput.Contains("\"id\":"));
            Assert.IsTrue(consoleOutput.Contains("\"product\":"));
        }
    }
}