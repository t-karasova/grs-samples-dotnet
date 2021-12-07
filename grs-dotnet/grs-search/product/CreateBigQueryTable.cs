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

using System;
using System.Diagnostics;

namespace grs_search.product
{
    public static class CreateBigQueryTable
    {
        private static readonly string ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");

        private const string DataSetId = "products";
        private const string TableId = "products";

        private static void CreateBQDataSet(string dataSetName)
        {
            var listDataSets = ListBQDataSets();
            if (!listDataSets.Contains(dataSetName))
            {
                var createDataSetCommand = $"bq --location=US mk -d --default_table_expiration 3600 --description \"This is my dataset.\" {ProjectId}:{dataSetName}";

                var procStartInfo = new ProcessStartInfo("cmd", "/c " + createDataSetCommand);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;

                using (Process process = new Process())
                {
                    process.StartInfo = procStartInfo;
                    process.Start();

                    process.WaitForExit();

                    string result = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine($"Dataset {dataSetName} already exists.");
            }
        }

        private static string ListBQDataSets()
        {
            var listDataSetCommand = $"bq ls --project_id {ProjectId}";
            var arrayDataSetCommands = listDataSetCommand.Split(" ");

            var procStartInfo = new ProcessStartInfo("cmd", "/c " + listDataSetCommand);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);

                return result;
            }
        }

        private static void CreateBQTable(string dataSet, string tableName)
        {
            var listBQTables = ListBQTables(dataSet);
            if (!listBQTables.Contains(dataSet))
            {
                var createTableCommand = $"bq mk --table {ProjectId}:{dataSet}.{tableName} product/product_schema.json";

                var procStartInfo = new ProcessStartInfo("cmd", "/c " + createTableCommand);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;

                // wrap IDisposable into using (in order to release hProcess) 
                using (Process process = new Process())
                {
                    process.StartInfo = procStartInfo;
                    process.Start();

                    // Add this: wait until process does its work
                    process.WaitForExit();

                    // and only then read the result
                    string result = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine($"Table {tableName} already exists.");
            }

        }

        private static string ListBQTables(string dataSet)
        {
            var listTablesCommand = $"bq ls {ProjectId}:{dataSet}";
            var tables = listTablesCommand.Split(" ");

            var procStartInfo = new ProcessStartInfo("cmd", "/c " + listTablesCommand);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Tables: \n" + result);

                return result;
            }
        }

        private static void UploadDataToBQTable(string dataSet, string tableName, string source)
        {
            var uploadDataCommand = $"bq load --source_format=NEWLINE_DELIMITED_JSON {ProjectId}:{dataSet}.{tableName} {source} product/product_schema.json";
            var arrayUploadData = uploadDataCommand.Split(" ");

            var procStartInfo = new ProcessStartInfo("cmd", "/c " + uploadDataCommand);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        [Attributes.Example]
        public static void PerformCreationOfBigQueryTable()
        {
            CreateBQDataSet(DataSetId);
            CreateBQTable(DataSetId, TableId);
            UploadDataToBQTable(DataSetId, TableId, "product/products.json");
            CreateBQTable(DataSetId, "products_some_invalid");
            UploadDataToBQTable(DataSetId, "products_some_invalid", "product/products_some_invalid.json");
        }
    }
}
