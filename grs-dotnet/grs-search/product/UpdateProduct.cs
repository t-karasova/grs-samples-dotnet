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

using System;
using Google.Cloud.Retail.V2;

namespace grs_search.product
{
    public static class UpdateProduct
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private const string Endpoint = "retail.googleapis.com";
        private const string GeneratedProductId = "GGCOGAAA101259";

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

        private static UpdateProductRequest GetUpdateProductRequest(Product productToUpdate)
        {
            UpdateProductRequest request = new UpdateProductRequest
            {
                Product = productToUpdate,
                AllowMissing = true
            };

            Console.WriteLine("Update product. request: \n" + request);
            return request;
        }

        public static Product UpdateRetailProduct(Product originalProduct)
        {
            var productForUpdate = GenerateProductForUpdate(originalProduct.Id);
            var updateProductRequest = GetUpdateProductRequest(productForUpdate);
            var updatedProduct = GetProductServiceClient().UpdateProduct(updateProductRequest);

            Console.WriteLine("Update product. response: \n" + updatedProduct);
            return updatedProduct;
        }

        [Attributes.Example]
        public static void PerformUpdateProductOperation()
        {
            var originalProduct = CreateProduct.CreateRetailProduct(GeneratedProductId);

            var updatedProduct = UpdateRetailProduct(originalProduct);

            DeleteProduct.DeleteRetailProduct(updatedProduct.Name);
        }
    }
}
// [END retail_update_product]