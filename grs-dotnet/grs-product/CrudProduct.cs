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

// [START retail_crud_product]
// Create product in a catalog using Retail API

using Google.Cloud.Retail.V2;
using System;
using System.Linq;

namespace grs_product
{
    public static class CrudProduct
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultBranchName = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/default_branch";
        private const string Endpoint = "retail.googleapis.com";

        private const string ProductId = "crud_product_id";
        private static readonly string ProductName = $"{DefaultBranchName}/products/{ProductId}";

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

        // Generate product
        private static Product GenerateProduct()
        {
            var priceInfo = new PriceInfo
            {
                Price = 30.0f,
                OriginalPrice = 35.5f,
                CurrencyCode = "USD"
            };

            string[] brands = { "Google" };
            string[] categories = { "Speakers and displays" };

            var generatedProduct = new Product
            {
                Title = "Nest Mini",
                Type = Product.Types.Type.Primary,
                PriceInfo = priceInfo,
                Availability = Product.Types.Availability.InStock
            };

            generatedProduct.Categories.Add(categories);
            generatedProduct.Brands.Add(brands);

            return generatedProduct;
        }

        // Get product for update
        private static Product GenerateProductForUpdate()
        {
            var updatedPriceInfo = new PriceInfo
            {
                Price = 20.0f,
                OriginalPrice = 25.5f,
                CurrencyCode = "EUR"
            };

            string[] categories = { "Updated Speakers and displays" };
            string[] brands = { "Updated Google" };

            var generatedProduct = new Product
            {
                Id = ProductId,
                Name = ProductName,
                Title = "Updated Nest Mini",
                Type = Product.Types.Type.Primary,
                PriceInfo = updatedPriceInfo,
                Availability = Product.Types.Availability.OutOfStock
            };

            generatedProduct.Categories.Add(categories);
            generatedProduct.Brands.Add(brands);

            return generatedProduct;
        }

        // Call the Retail API to create a product
        private static Product CreateRetailProduct()
        {
            var generatedProduct = GenerateProduct();
            var createProductRequest = new CreateProductRequest
            {
                Product = generatedProduct,
                ProductId = ProductId,
                Parent = DefaultBranchName
            };

            Console.WriteLine("Create product. request: \n\n" + createProductRequest);

            var createdProduct = GetProductServiceClient().CreateProduct(createProductRequest);

            Console.WriteLine("\nCreated product: \n" + createdProduct);

            return createdProduct;
        }

        // Call the Retail API to get a product
        private static Product GetRetailProduct()
        {
            var getRequest = new GetProductRequest
            {
                Name = ProductName
            };

            Console.WriteLine("Get product. request: \n\n" + getRequest);

            var product = GetProductServiceClient().GetProduct(getRequest);

            Console.WriteLine("\nGet product. response: \n" + product);

            return product;
        }

        // Call the Retail API to update a product
        private static Product UpdateRetailProduct()
        {
            var generatedProductForUpdate = GenerateProductForUpdate();

            var updateRequest = new UpdateProductRequest
            {
                Product = generatedProductForUpdate,
                AllowMissing = true
            };

            Console.WriteLine("Update product. request: \n\n" + updateRequest);

            var updatedProduct = GetProductServiceClient().UpdateProduct(updateRequest);

            Console.WriteLine("\nUpdated product: " + updatedProduct);
            return updatedProduct;
        }

        // Call the Retail API to delete a product
        private static void DeleteRetailProduct()
        {
            var deleteRequest = new DeleteProductRequest
            {
                Name = ProductName
            };

            Console.WriteLine("\nDelete product. request: \n\n" + deleteRequest);

            GetProductServiceClient().DeleteProduct(deleteRequest);

            Console.WriteLine($"\nDeleting product:\nProduct {ProductName} was deleted");
        }

        // Perform CRUD Product Operations
        [Attributes.Example]
        public static void PerformCRUDProductOperations()
        {
            // Call the methods
            CreateRetailProduct();
            GetRetailProduct();
            UpdateRetailProduct();
            DeleteRetailProduct();
        }
    }
}
// [END retail_crud_product]