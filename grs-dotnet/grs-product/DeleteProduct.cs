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

// [START retail_delete_product]
// Delete product from a catalog using Retail API

using Google.Cloud.Retail.V2;
using System;
using System.Linq;

namespace grs_product
{
    public static class DeleteProduct
    {
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
            var productServiceClientBuilder = new ProductServiceClientBuilder
            {
                Endpoint = Endpoint
            };

            var productServiceClient = productServiceClientBuilder.Build();
            return productServiceClient;
        }

        // Get delete product request
        private static DeleteProductRequest GetDeleteProductRequest(string productName)
        {
            var deleteRequest = new DeleteProductRequest
            {
                Name = productName
            };

            Console.WriteLine("\nDelete product. request: \n\n" + deleteRequest);

            return deleteRequest;
        }

        // Call the Retail API to delete a product
        public static void DeleteRetailProduct(string productName)
        {
            var deleteProductRequest = GetDeleteProductRequest(productName);

            GetProductServiceClient().DeleteProduct(deleteProductRequest);

            Console.WriteLine($"\nDeleting product:\nProduct {productName} was deleted");
        }

        // Perform product deletion
        [Attributes.Example]
        public static void PerformDeleteProductOperation()
        {
            // Create product
            var createdProduct = CreateProduct.CreateRetailProduct(GeneratedProductId);

            // Delete created product
            DeleteRetailProduct(createdProduct.Name);
        }
    }
}
// [END retail_delete_product]