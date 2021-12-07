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

// [START set_inventory]

using System;
using System.Threading;
using Google.Cloud.Retail.V2;
using Google.Protobuf.WellKnownTypes;

namespace grs_search.product
{
    public static class SetInventory
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");

        private const string Endpoint = "retail.googleapis.com";
        private const string ProductId = "fulfillment_test_product_id";
        private static readonly string ProductName = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/default_branch/products/{ProductId}";

        // The request timestamp
        private static DateTime RequestTimeStamp = DateTime.Now.ToUniversalTime();
        // The outdated request timestamp
        // request_time = datetime.datetime.now() - datetime.timedelta(days=1)

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

        private static Product GetProductWithInventoryInfo(string productName)
        {
            var priceInfo = new PriceInfo
            {
                Price = 15.0f,
                OriginalPrice = 20.0f,
                Cost = 8.0f,
                CurrencyCode = "USD"
            };

            var fulfillmentInfo = new FulfillmentInfo
            {
                Type = "pickup-in-store"
            };

            string[] placeIds = { "store1", "store2" };

            fulfillmentInfo.PlaceIds.Add(placeIds);

            var product = new Product
            {
                Name = productName,
                PriceInfo = priceInfo,
                Availability = Product.Types.Availability.InStock
            };

            product.FulfillmentInfo.Add(fulfillmentInfo);

            return product;
        }

        private static SetInventoryRequest GetSetInventoryRequest(string productName)
        {
            var setInventoryRequest = new SetInventoryRequest
            {
                Inventory = GetProductWithInventoryInfo(productName),
                SetTime = Timestamp.FromDateTime(RequestTimeStamp),
                AllowMissing = true
            };

            Console.WriteLine("Set Inventory. request: \n" + setInventoryRequest);
            return setInventoryRequest;
        }

        private static void SetProductInventory(string productName)
        {
            var setInventoryRequest = GetSetInventoryRequest(productName);
            GetProductServiceClient().SetInventory(setInventoryRequest);

            //This is a long running operation and its result is not immediately present with get operations,
            // thus we simulate wait with sleep method.
            Console.WriteLine("Set inventory. Wait 10 seconds:");
            Thread.Sleep(10000);
        }

        [Attributes.Example]
        public static void PerformSetInventoryOperation()
        {
            CreateProduct.CreateRetailProduct(ProductId);
            SetProductInventory(ProductName);
            GetProduct.GetRetailProduct(ProductName);
            DeleteProduct.DeleteRetailProduct(ProductName);
        }
    }
}
// [END set_inventory]