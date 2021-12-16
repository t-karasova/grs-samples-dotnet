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

using Google.Cloud.Retail.V2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace grs_product.Tests
{
    [TestClass]
    public class UpdateProductTest
    {
        private const string ProductFolderName = "grs-product";
        private static readonly string WorkingDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, ProductFolderName);

        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        const string CMDFileName = "cmd.exe";
        const string CommandLineArguments = "/c " + "dotnet run -- UpdateProduct"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestUpdateProduct()
        {
            const string ExpectedProductTitle = "Updated Nest Mini";
            const string ExpectedCurrencyCode = "EUR";
            const float ExpectedProductPrice = 20.0f;
            const float ExpectedProductOriginalPrice = 25.5f;
            const Product.Types.Availability ExpectedProductAvailability = Product.Types.Availability.OutOfStock;

            var updatedProduct = UpdateProduct.PerformUpdateProductOperation();

            Assert.AreEqual(ExpectedProductTitle, updatedProduct.Title);
            Assert.AreEqual(ExpectedCurrencyCode, updatedProduct.PriceInfo.CurrencyCode);
            Assert.AreEqual(ExpectedProductPrice, updatedProduct.PriceInfo.Price);
            Assert.AreEqual(ExpectedProductOriginalPrice, updatedProduct.PriceInfo.OriginalPrice);
            Assert.AreEqual(ExpectedProductAvailability, updatedProduct.Availability);
        }

        [TestMethod]
        public void TestOutputUpdateProduct()
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

            Assert.IsTrue(consoleOutput.Contains("Created product:"));
            Assert.IsTrue(consoleOutput.Contains("Updated product:"));
            Assert.IsTrue(consoleOutput.Contains("\"title\": \"Updated Nest Mini\""));
            Assert.IsTrue(consoleOutput.Contains("\"brands\": [ \"Updated Google\" ]"));
            Assert.IsTrue(consoleOutput.Contains("\"price\": 20"));
            Assert.IsTrue(consoleOutput.Contains($"Product projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/0/products/"));
            Assert.IsTrue(consoleOutput.Contains("was deleted"));
        }
    }
}
