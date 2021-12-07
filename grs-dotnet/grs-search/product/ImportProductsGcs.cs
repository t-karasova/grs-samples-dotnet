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

// [START retail_import_products_from_gcs]
// Import products into a catalog from gcs using Retail API


using System;
using System.Threading;
using Google.Cloud.Retail.V2;

namespace grs_search.product
{
    public static class ImportProductsGcs
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");

        private static readonly string DefaultCatalog = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/branches/1";
        private const string Endpoint = "retail.googleapis.com";
        private const string gcsBucket = "gs://products_catalog";
        private const string gcsErrorsBucket = "gs://products_catalog/error";
        private const string gcsProductsObject = "products.json";
        // TO CHECK ERROR HANDLING USE THE JSON WITH INVALID PRODUCT
        // gcs_products_object = "products_for_import_some_invalid.json"

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

        private static ImportProductsRequest GetImportProductsGcsRequest(string gcsObjectName)
        {
            // TO CHECK ERROR HANDLING PASTE THE INVALID CATALOG NAME HERE:
            // DefaultCatalog = "invalid_catalog_name"
            var gcsSource = new GcsSource();
            gcsSource.InputUris.Add($"{gcsBucket}/{gcsObjectName}");

            var inputConfig = new ProductInputConfig
            {
                GcsSource = gcsSource
            };

            Console.WriteLine("GCS source: \n" + gcsSource.InputUris);

            var errorsConfig = new ImportErrorsConfig
            {
                GcsPrefix = gcsErrorsBucket
            };

            var importRequest = new ImportProductsRequest
            {
                Parent = DefaultCatalog,
                ReconciliationMode = ImportProductsRequest.Types.ReconciliationMode.Incremental,
                InputConfig = inputConfig,
                ErrorsConfig = errorsConfig
            };

            Console.WriteLine("Import products from google cloud source. request: \n" + importRequest);
            return importRequest;
        }

        [Attributes.Example]
        public static void ImportProductsFromGcs()
        {
            var importGcsRequest = GetImportProductsGcsRequest(gcsProductsObject);
            var gcsOperation = GetProductServiceClient().ImportProducts(importGcsRequest);

            Console.WriteLine("The operation was started: Operation\n" + gcsOperation.Name);

            while (!gcsOperation.IsCompleted)
            {
                Console.WriteLine("Please wait till opeartion is done");
                Thread.Sleep(5000);
            }

            Console.WriteLine("Import products operation is done");
            Console.WriteLine("Number of successfully imported products: " + gcsOperation.Metadata.SuccessCount);
            Console.WriteLine("Number of failures during the importing: " + gcsOperation.Metadata.FailureCount);
            Console.WriteLine("Operation result: \n" + gcsOperation.Result);

            // The imported products needs to be indexed in the catalog before they become available for search.
            Console.WriteLine("Wait till products become indexed in the catalog, after that they will be available for search");
            Thread.Sleep(120000);
        }
    }
}
// [END retail_import_products_from_gcs]