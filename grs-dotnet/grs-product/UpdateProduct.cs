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

// [START retail_update_product]
// Update product from a catalog using Retail API

using Google.Cloud.Retail.V2;
using System;
using System.Linq;

namespace grs_product
{
    public static class UpdateProduct
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private const string Endpoint = "retail.googleapis.com";

        private static readonly Random random = new();
        private static readonly string GeneratedProductId = RandomAlphanumericString(14);

        public static string RandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Get product service client
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

        private static Product GenerateProductForUpdate(string productId)
        {
            var updatedPriceInfo = new PriceInfo
            {
                Price = 20.0f,
                OriginalPrice = 25.5f,
                CurrencyCode = "EUR"
            };

            var generatedProduct = new Product
            {
                Id = productId,
                Name = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/default_branch/products/{productId}",
                Title = "Updated Nest Mini",
                Type = Product.Types.Type.Primary,
                PriceInfo = updatedPriceInfo,
                Availability = Product.Types.Availability.OutOfStock
            };

            generatedProduct.Categories.Add("Updated Speakers and displays");
            generatedProduct.Brands.Add("Updated Google");

            return generatedProduct;
        }

        // Get update product request
        private static UpdateProductRequest GetUpdateProductRequest(Product productToUpdate)
        {
            UpdateProductRequest request = new UpdateProductRequest
            {
                Product = productToUpdate,
                AllowMissing = true
            };

            Console.WriteLine("Update product. request: \n\n" + request);
            return request;
        }

        // Call the Retail API to update a product
        public static Product UpdateRetailProduct(Product originalProduct)
        {
            var productForUpdate = GenerateProductForUpdate(originalProduct.Id);
            var updateProductRequest = GetUpdateProductRequest(productForUpdate);
            var updatedProduct = GetProductServiceClient().UpdateProduct(updateProductRequest);

            Console.WriteLine("\nUpdated product: " + updatedProduct);
            return updatedProduct;
        }

        // Perform product update
        [Attributes.Example]
        public static Product PerformUpdateProductOperation()
        {
            // Create product
            var originalProduct = CreateProduct.CreateRetailProduct(GeneratedProductId);

            // Update created product
            var updatedProduct = UpdateRetailProduct(originalProduct);

            // Delete updated product
            DeleteProduct.DeleteRetailProduct(updatedProduct.Name);

            return updatedProduct;
        }
    }
}
// [END retail_update_product]