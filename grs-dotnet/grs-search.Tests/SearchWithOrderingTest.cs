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

using grs_search.search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace grs_search.Tests.search
{
    [TestClass]
    public class SearchWithOrderingTest
    {
        private static readonly string WorkingDirectory = Environment.GetEnvironmentVariable("GRS_SEARCH_TEST_PATH");
        const string CMDFileName = "cmd.exe";
        const string CommandLineArguments = "/c " + "dotnet run -- SearchWithOrdering"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestSearchWithOrdering()
        {
            const int ExpectedProductPrice = 39;

            var response = SearchWithOrdering.Search();

            var actualProductPrice = response.ToArray()[0].Product.PriceInfo.Price;
            var actualResponseLength = response.ToArray().Length;

            Assert.IsTrue(actualProductPrice == ExpectedProductPrice);
        }

        [TestMethod]
        public void TestOutputSearchWithOrdering()
        {
            string consoleOutput = string.Empty;

            var processStartInfo = new ProcessStartInfo(CMDFileName, CommandLineArguments);

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = WorkingDirectory;

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;

                process.Start();

                consoleOutput = process.StandardOutput.ReadToEnd();
            }

            Assert.IsTrue(consoleOutput.Contains("Search. request:"));
            Assert.IsTrue(consoleOutput.Contains("Search. response:"));
            // Check the response contains some products
            Assert.IsTrue(consoleOutput.Contains("\"id\":"));
            Assert.IsTrue(consoleOutput.Contains("\"product\":"));
        }
    }
}
