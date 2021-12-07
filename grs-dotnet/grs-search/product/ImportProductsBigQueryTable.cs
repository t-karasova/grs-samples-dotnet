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

// [START retail_import_products_from_big_query]
// Import products into a catalog from big query table using Retail API


using System;
using System.Threading;
using Google.Cloud.Retail.V2;

namespace grs_search.product
{
    public static class ImportProductsBigQueryTable
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");

        private static readonly string DefaultCatalog = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/1";
        private const string Endpoint = "retail.googleapis.com";
        private const string DataSetId = "products";
        private const string TableId = "products";
        private const string DataSchema = "product";
        // TO CHECK ERROR HANDLING USE THE TABLE OF INVALID PRODUCTS:
        // TableId = "products_for_import_invalid"

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

        private static ImportProductsRequest GetImportProductsBigQueryRequest(ImportProductsRequest.Types.ReconciliationMode reconciliationMode)
        {
            // TO CHECK ERROR HANDLING PASTE THE INVALID CATALOG NAME HERE:
            // DefaultCatalog = "invalid_catalog_name"
            var bigQuerySource = new BigQuerySource
            {
                ProjectId = ProjectId,
                DatasetId = DataSetId,
                TableId = TableId,
                DataSchema = DataSchema
            };

            var inputConfig = new ProductInputConfig
            {
                BigQuerySource = bigQuerySource
            };

            var importRequest = new ImportProductsRequest
            {
                Parent = DefaultCatalog,
                ReconciliationMode = reconciliationMode,
                InputConfig = inputConfig
            };

            Console.WriteLine("Import products from big query table. request: \n" + importRequest);
            return importRequest;
        }

        [Attributes.Example]
        public static void ImportProductsFromBigQuery()
        {
            // TRY THE FULL RECONCILIATION MODE HERE:
            var recoinciliationMode = ImportProductsRequest.Types.ReconciliationMode.Full;
            var importBigQueryRequest = GetImportProductsBigQueryRequest(recoinciliationMode);
            var bigQueryOperation = GetProductServiceClient().ImportProducts(importBigQueryRequest);

            Console.WriteLine("The operation was started: Operation\n" + bigQueryOperation.Name);

            while (!bigQueryOperation.IsCompleted)
            {
                Console.WriteLine("Please wait till opeartion is done");
                Thread.Sleep(5000);
            }

            Console.WriteLine("Import products operation is done");
            Console.WriteLine("Number of successfully imported products: " + bigQueryOperation.Metadata.SuccessCount);
            Console.WriteLine("Number of failures during the importing: " + bigQueryOperation.Metadata.FailureCount);
            Console.WriteLine("Operation result: \n" + bigQueryOperation.Result);
        }
    }
}
// [END retail_import_products_from_big_query]