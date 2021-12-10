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

// [START retail_update_attribute_configuration]
// Update product in a catalog using Retail API to change the product attribute searchability and indexability.

using Google.Cloud.Retail.V2;
using System;

namespace grs_search.search
{
    public static class UpdateAttributeConfiguration
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private const string Endpoint = "retail.googleapis.com";

        private static readonly string ProductId = "GGOEAAEC172013";

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

        // Get product
        private static Product GetProduct(string productId)
        {
            var defaultSearchPlacement = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/default_branch/products/{productId}";
            var getProductRequest = new GetProductRequest
            {
                Name = defaultSearchPlacement // Placement is used to identify the Serving Config name
            };

            var product = GetProductServiceClient().GetProduct(getProductRequest);

            return product;
        }

        // Get update product request
        private static UpdateProductRequest GetUpdateProductRequest(Product productToUpdate)
        {
            var updateRequest = new UpdateProductRequest
            {
                Product = productToUpdate,
            };

            Console.WriteLine("Update product. request: \n\n" + updateRequest);
            return updateRequest;
        }

        // Update the product attribute:
        [Attributes.Example]
        public static Product UpdateRetailProduct()
        {
            // Get a product from catalog
            var productToUpdate = GetProduct(ProductId);

            // Prepare the product attribute to be updated
            var attribute = new CustomAttribute
            {
                Indexable = false,
                Searchable = false
            };

            string[] attributeText = { "recycled fabrics", "recycled packaging", "plastic-free packaging", "ethically made" };
            attribute.Text.AddRange(attributeText);

            // Set the attribute to the original product
            productToUpdate.Attributes.Remove("ecofriendly");
            productToUpdate.Attributes.Add("ecofriendly", attribute);

            // Update product
            var updateProductRequest = GetUpdateProductRequest(productToUpdate);
            var updatedProduct = GetProductServiceClient().UpdateProduct(updateProductRequest);

            Console.WriteLine("\nUpdated product: \n\n" + updatedProduct);

            return updatedProduct;
        }
    }
}
// [END retail_update_attribute_configuration]