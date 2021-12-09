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

namespace grs_search.Tests.product
{
    [TestClass]
    public class UpdateProductTest
    {
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
    }
}
