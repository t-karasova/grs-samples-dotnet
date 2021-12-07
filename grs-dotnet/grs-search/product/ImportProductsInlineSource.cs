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

// [START retail_import_products_from_inline_source]
// Import products into a catalog from inline source using Retail API


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Cloud.Retail.V2;
using Google.Protobuf.WellKnownTypes;

namespace grs_search.product
{
    public static class ImportProductsInlineSource
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");

        private static readonly string DefaultCatalog = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/1";
        private const string Endpoint = "retail.googleapis.com";

        private static readonly Random random = new();
        private static readonly string GeneratedProductId1 = RandomAlphanumericString(14);
        private static readonly string GeneratedProductId2 = RandomAlphanumericString(14);

        public static string RandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static List<Product> GetProducts()
        {
            var products = new List<Product>();
            var priceInfo1 = new PriceInfo
            {
                Price = 16.0f,
                OriginalPrice = 45.0f,
                Cost = 12.0f,
                CurrencyCode = "USD"
            };

            string[] colors1 = { "Light blue", "Blue", "Dark blue"};
            var colorInfo1 = new ColorInfo();
            colorInfo1.ColorFamilies.Add("Blue");
            colorInfo1.Colors.AddRange(colors1);

            var fulfillmentInfo1 = new FulfillmentInfo();
            fulfillmentInfo1.Type = "pickup-in-store";
            string[] placeIds1 = { "store1", "store2" };
            fulfillmentInfo1.PlaceIds.AddRange(placeIds1);

            string[] paths1 = { "title", "categories", "price_info", "color_info" };
            var fieldMask1 = new FieldMask();
            fieldMask1.Paths.AddRange(paths1);

            // TO CHECK ERROR HANDLING COMMENT OUT THE PRODUCT TITLE HERE:
            var product1 = new Product
            {
                Title = "#IamRemarkable Pen",
                Id = GeneratedProductId1,
                Uri = "https://shop.googlemerchandisestore.com/Google+Redesign/Office/IamRemarkable+Pen",
                PriceInfo = priceInfo1,
                ColorInfo = colorInfo1,
                RetrievableFields = fieldMask1,
            };

            product1.Categories.Add("Office");
            product1.Brands.Add("#IamRemarkable");
            product1.FulfillmentInfo.Add(fulfillmentInfo1);



            var priceInfo2 = new PriceInfo
            {
                Price = 35.0f,
                OriginalPrice = 45.0f,
                Cost = 12.0f,
                CurrencyCode = "USD"
            };

            string[] colors2 = { "Sky blue" };
            var colorInfo2 = new ColorInfo();
            colorInfo2.ColorFamilies.Add("Blue");
            colorInfo2.Colors.AddRange(colors2);

            var fulfillmentInfo2 = new FulfillmentInfo();
            fulfillmentInfo2.Type = "pickup-in-store";
            string[] placeIds2 = { "store2", "store3" };
            fulfillmentInfo2.PlaceIds.AddRange(placeIds2);

            string[] paths2 = { "title", "categories", "price_info", "color_info" };
            var fieldMask2 = new FieldMask();
            fieldMask2.Paths.AddRange(paths2);

            // TO CHECK ERROR HANDLING COMMENT OUT THE PRODUCT TITLE HERE:
            var product2 = new Product
            {
                Title = "Android Embroidered Crewneck Sweater",
                Id = GeneratedProductId2,
                Uri = "https://shop.googlemerchandisestore.com/Google+Redesign/Apparel/Android+Embroidered+Crewneck+Sweater",
                PriceInfo = priceInfo2,
                ColorInfo = colorInfo2,
                RetrievableFields = fieldMask2,
            };

            product2.Categories.Add("Apparel");
            product2.Brands.Add("Android");
            product2.FulfillmentInfo.Add(fulfillmentInfo2);

            products.Add(product1);
            products.Add(product2);

            return products;
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

        private static ImportProductsRequest GetImportProductsInlineRequest(List<Product> productsToImport)
        {
            // TO CHECK ERROR HANDLING PASTE THE INVALID CATALOG NAME HERE:
            // DefaultCatalog = "invalid_catalog_name"
            var inlineSource = new ProductInlineSource();
            inlineSource.Products.AddRange(productsToImport);

            var inputConfig = new ProductInputConfig
            {
                ProductInlineSource = inlineSource
            };

            var importRequest = new ImportProductsRequest
            {
                Parent = DefaultCatalog,
                InputConfig = inputConfig,
            };

            Console.WriteLine("Import products from google cloud source. request: \n" + importRequest);
            return importRequest;
        }

        [Attributes.Example]
        public static void ImportProductsFromInlineSource()
        {
            var products = GetProducts();
            var importRequest = GetImportProductsInlineRequest(products);
            var importOperation = GetProductServiceClient().ImportProducts(importRequest);

            Console.WriteLine("The operation was started: Operation\n" + importOperation.Name);

            while (!importOperation.IsCompleted)
            {
                Console.WriteLine("Please wait till opeartion is done");
                Thread.Sleep(5000);
            }

            Console.WriteLine("Import products operation is done");
            Console.WriteLine("Number of successfully imported products: " + importOperation.Metadata.SuccessCount);
            Console.WriteLine("Number of failures during the importing: " + importOperation.Metadata.FailureCount);
            Console.WriteLine("Operation result: \n" + importOperation.Result);
        }
    }
}
