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
using grs_search.product;
using Google.Cloud.Retail.V2;
using System.Diagnostics;
using System;

namespace grs_search.Tests.product
{
    [TestClass]
    public class SetInventoryTest
    {
        private static readonly string WorkingDirectory = Environment.GetEnvironmentVariable("GRS_TEST_PATH");
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        const string CMDFileName = "cmd.exe";
        const string CommandLineArguments = "/c " + "dotnet run -- SetInventory"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestSearchAttributeConfig()
        {
            const string ExpectedCurrencyCode = "USD";
            const float ExpectedProductPrice = 30.0f;
            const float ExpectedProductOriginalPrice = 35.5f;
            const Product.Types.Availability ExpectedProductAvailability = Product.Types.Availability.InStock;

            var inventoryProduct = SetInventory.PerformSetInventoryOperation();

            Assert.AreEqual(ExpectedCurrencyCode, inventoryProduct.PriceInfo.CurrencyCode);
            Assert.AreEqual(ExpectedProductPrice, inventoryProduct.PriceInfo.Price);
            Assert.AreEqual(ExpectedProductOriginalPrice, inventoryProduct.PriceInfo.OriginalPrice);
            Assert.AreEqual(ExpectedProductAvailability, inventoryProduct.Availability);
        }

        [TestMethod]
        public void TestOutputSetInventory()
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

            Assert.IsTrue(consoleOutput.Contains("Created product:"));
            Assert.IsTrue(consoleOutput.Contains($"\"name\": \"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/0/products/inventory_test_product_id\""));
        }
    }
}
