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
using grs_search.product;
using Google.Cloud.Retail.V2;

namespace grs_search.Tests.product
{
    [TestClass]
    public class CreateProductTest
    {
        [TestMethod]
        public void TestCreateProduct()
        {
            const string ExpectedProductTitle = "Nest Mini";
            const string ExpectedCurrencyCode = "USD";
            const float ExpectedProductPrice = 30.0f;
            const float ExpectedProductOriginalPrice = 35.5f;
            const Product.Types.Availability ExpectedProductAvailability = Product.Types.Availability.InStock;

            var createdProduct = CreateProduct.PerformCreateProductOperation();

            Assert.AreEqual(ExpectedProductTitle, createdProduct.Title);
            Assert.AreEqual(ExpectedCurrencyCode, createdProduct.PriceInfo.CurrencyCode);
            Assert.AreEqual(ExpectedProductPrice, createdProduct.PriceInfo.Price);
            Assert.AreEqual(ExpectedProductOriginalPrice, createdProduct.PriceInfo.OriginalPrice);
            Assert.AreEqual(ExpectedProductAvailability, createdProduct.Availability);
        }
    }
}
