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

// [START retail_delete_product]
// Delete product from a catalog using Retail API

using System;
using Google.Cloud.Retail.V2;

namespace grs_search.product
{
    public static class DeleteProduct
    {
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

        private static DeleteProductRequest GetDeleteProductRequest(string productName)
        {
            DeleteProductRequest request = new DeleteProductRequest
            {
                Name = productName
            };

            Console.WriteLine("Delete product. request: \n" + request);

            return request;
        }

        public static void DeleteRetailProduct(string productName)
        {
            var deleteProductRequest = GetDeleteProductRequest(productName);

            GetProductServiceClient().DeleteProduct(deleteProductRequest);

            Console.WriteLine($"Deleting product {productName}. response: \nProduct was deleted.");
        }

        [Attributes.Example]
        public static Product PerformDeleteProductOperation()
        {
            var productToDelete = CreateProduct.CreateRetailProduct(GeneratedProductId);

            DeleteRetailProduct(productToDelete.Name);

            return productToDelete;
        }
    }
}
// [END retail_delete_product]