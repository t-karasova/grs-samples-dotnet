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

// [START retail_create_product]
// Create product in a catalog using Retail API

using System;
using System.Linq;
using Google.Cloud.Retail.V2;

namespace grs_search.product
{
    public static class CreateProduct
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultBranchName = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/default_branch";
        private const string Endpoint = "retail.googleapis.com";

        private static readonly Random random = new();
        private static readonly string GeneratedProductId = RandomAlphanumericString(14);

        public static string RandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static ProductServiceClient GetProductServiceClient()
        {
            ProductServiceClientBuilder productServiceClientBuilder =
                new ProductServiceClientBuilder
                {
                    Endpoint = Endpoint
                };
            ProductServiceClient productServiceClient = productServiceClientBuilder.Build();
            return productServiceClient;
        }

        private static Product GenerateProduct()
        {
            var priceInfo = new PriceInfo
            {
                Price = 30.0f,
                OriginalPrice = 35.5f,
                CurrencyCode = "USD"
            };

            var generatedProduct = new Product
            {
                Title = "Nest Mini",
                Type = Product.Types.Type.Primary,
                PriceInfo = priceInfo,
                Availability = Product.Types.Availability.InStock
            };

            generatedProduct.Categories.Add("Speakers and displays");
            generatedProduct.Brands.Add("Google");

            return generatedProduct;
        }

        private static CreateProductRequest GetCreateProductRequest(Product productToCreate, string productId)
        {
            CreateProductRequest request = new CreateProductRequest
            {
                Product = productToCreate,
                ProductId = productId,
                Parent = DefaultBranchName
            };

            Console.WriteLine("Create product. request: \n" + request);

            return request;
        }

        public static Product CreateRetailProduct(string productId)
        {
            var generatedProduct = GenerateProduct();
            var createProductRequest = GetCreateProductRequest(generatedProduct, productId);

            var createdProduct = GetProductServiceClient().CreateProduct(createProductRequest);

            Console.WriteLine("Create product. response: \n" + createdProduct);

            
            return createdProduct;
        }

        [Attributes.Example]
        public static void PerformCreateProductOperation()
        {
            var createdProduct = CreateRetailProduct(GeneratedProductId);

            DeleteProduct.DeleteRetailProduct(createdProduct.Name);
        }
    }
}
// [END retail_create_product]