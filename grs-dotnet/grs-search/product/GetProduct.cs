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

// [START retail_get_product]
// Get product from a catalog using Retail API

using Google.Cloud.Retail.V2;
using System;

namespace grs_search.product
{
    public static class GetProduct
    {
        private const string Endpoint = "retail.googleapis.com";
        private const string GeneratedProductId = "GGCOGAAA101259";

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

        // Get create product request
        private static GetProductRequest GetProductRequest(string productName)
        {
            GetProductRequest request = new GetProductRequest
            {
                Name = productName
            };

            Console.WriteLine("Get product. request: \n\n" + request);
            return request;
        }

        // Call the Retail API to get a product
        public static Product GetRetailProduct(string productName)
        {
            var getProductRequest = GetProductRequest(productName);

            var product = GetProductServiceClient().GetProduct(getProductRequest);

            Console.WriteLine("\nGet product. response: \n" + product);
            
            return product;
        }

        // Perform product retrieval
        [Attributes.Example]
        public static Product PerformGetProductOperation()
        {
            // Create product
            var createdProduct = CreateProduct.CreateRetailProduct(GeneratedProductId);

            // Get created product
            var retrievedProduct = GetRetailProduct(createdProduct.Name);

            // Delete created product
            DeleteProduct.DeleteRetailProduct(retrievedProduct.Name);

            return retrievedProduct;
        }
    }
}
// [END retail_get_product]