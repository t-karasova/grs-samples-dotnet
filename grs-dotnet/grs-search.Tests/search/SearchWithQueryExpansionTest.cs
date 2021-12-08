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
using System.Linq;
using grs_search.search;

namespace grs_search.Tests.search
{
    [TestClass]
    public class SearchWithQueryExpansionTest
    {
        [TestMethod]
         public void TestSearchWithQueryExpansion()
        {
            const string ExpectedProductTitle = "Google Youth Hero Tee Grey";
            const int ExpectedResponseLength = 29;

            var response = SearchWithQueryExpansion.Search();

            var actualFirstProductTitle = response.ToArray()[0].Product.Title;
            var actualThirdProductTitle = response.ToArray()[2].Product.Title;
            var actualResponseLength = response.ToArray().Length;

            Assert.IsTrue(actualFirstProductTitle.Equals(ExpectedProductTitle));
            Assert.IsTrue(!actualThirdProductTitle.Equals(ExpectedProductTitle));
            Assert.IsTrue(actualResponseLength == ExpectedResponseLength);
        }
    }
}
