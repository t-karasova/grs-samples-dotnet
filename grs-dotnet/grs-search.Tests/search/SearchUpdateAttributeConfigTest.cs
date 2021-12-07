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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using grs_search.search;

namespace grs_search.Tests.search
{
    [TestClass]
    public class SearchUpdateAttributeConfigTest
    {
        [TestMethod]
        public void TestSearchAttributeConfig()
        {
            const string ExpectedProductTitle = "Sweater";
            const string ExpectedKey = "ecofriendly";

            var response = SearchAttributeConfig.Search();

            var actualProductTitle = response.ToArray()[0].Product.Title;
            var actualAttributeKeys = response.ToArray()[0].Product.Attributes.Keys;
            var actualResponseLength = response.ToArray().Length;

            Assert.IsTrue(actualProductTitle.Contains(ExpectedProductTitle));
            Assert.IsTrue(actualAttributeKeys.Contains(ExpectedKey));
            Assert.IsTrue(actualResponseLength == 1);
        }

        [TestMethod]
        public void TestUpdateAttributeConfig()
        {
            const string ExpectedProductId = "GGOEAAEC172013";
            const string ExpectedKey = "ecofriendly";

            var product = UpdateAttributeConfig.UpdateRetailProduct();

            Assert.AreEqual(ExpectedProductId, product.Id);
            Assert.IsTrue(product.Attributes.Keys.Contains(ExpectedKey));
        }
    }
}