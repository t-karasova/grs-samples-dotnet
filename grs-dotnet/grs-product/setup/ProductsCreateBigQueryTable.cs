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

namespace grs_product
{
    public static class ProductsCreateBigQueryTable
    {
        private const string DataSetId = "products";
        private const string TableId = "products";
        private const string InvalidTableId = "products_some_invalid";

        private static readonly string ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");

        private const string WindowsTerminalName = "cmd.exe";
        private const string UnixTerminalName = "/bin/bash";
        private const string WindowsTerminalPrefix = "/c ";
        private const string UnixTerminalPrefix = "-c ";
        private const string WindowsTerminalQuotes = "";
        private const string UnixTerminalQuotes = "\"";

        private static readonly bool CurrentOSIsWindows = Environment.OSVersion.VersionString.Contains("Windows");
        private static readonly string CurrentTerminalPrefix = CurrentOSIsWindows ? WindowsTerminalPrefix : UnixTerminalPrefix;
        private static readonly string CurrentTerminalFile = CurrentOSIsWindows ? WindowsTerminalName : UnixTerminalName;
        private static readonly string CurrentTerminalQuotes = CurrentOSIsWindows ? WindowsTerminalQuotes : UnixTerminalQuotes;

        private static void CreateBQDataSet(string dataSetName)
        {
            var listDataSets = ListBQDataSets();
            if (!listDataSets.Contains(dataSetName))
            {
                string createDataSetCommand = CurrentTerminalPrefix + CurrentTerminalQuotes + $"bq --location=US mk -d --default_table_expiration 3600 --description \"This is my dataset.\" {ProjectId}:{dataSetName}" + CurrentTerminalQuotes;
                string consoleOutput = string.Empty;

                var processStartInfo = new ProcessStartInfo(CurrentTerminalFile, createDataSetCommand)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process())
                {
                    process.StartInfo = processStartInfo;

                    process.Start();

                    consoleOutput = process.StandardOutput.ReadToEnd();
                }
            }
            else
            {
                Console.WriteLine($"Dataset {dataSetName} already exists.");
            }
        }

        private static string ListBQDataSets()
        {
            string dataSets = string.Empty;

            string listDataSetCommand = CurrentTerminalPrefix + CurrentTerminalQuotes + $"bq ls --project_id {ProjectId}" + CurrentTerminalQuotes;

            var processStartInfo = new ProcessStartInfo(CurrentTerminalFile, listDataSetCommand)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;

                process.Start();

                dataSets = process.StandardOutput.ReadToEnd();

                Console.WriteLine(dataSets);
            }

            return dataSets;
        }

        private static void CreateBQTable(string dataSet, string tableName)
        {
            var listBQTables = ListBQTables(dataSet);
            if (!listBQTables.Contains(dataSet))
            {
                string consoleOutput = string.Empty;

                var createTableCommand = CurrentTerminalPrefix + CurrentTerminalQuotes + $"bq mk --table {ProjectId}:{dataSet}.{tableName} product/resources/product_schema.json" + CurrentTerminalQuotes;

                var procStartInfo = new ProcessStartInfo(CurrentTerminalFile, createTableCommand)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = procStartInfo;
                    process.Start();

                    consoleOutput = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(consoleOutput);
                }
            }
            else
            {
                Console.WriteLine($"Table {tableName} already exists.");
            }

        }

        private static string ListBQTables(string dataSet)
        {
            string tables = string.Empty;
            var listTablesCommand = CurrentTerminalPrefix + CurrentTerminalQuotes + $"bq ls {ProjectId}:{dataSet}" + CurrentTerminalQuotes;

            var procStartInfo = new ProcessStartInfo(CurrentTerminalFile, listTablesCommand)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                tables = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Tables: \n" + tables);

                return tables;
            }
        }

        private static void UploadDataToBQTable(string dataSet, string tableName, string source)
        {
            string consoleOutput = string.Empty;

            var uploadDataCommand = CurrentTerminalPrefix + CurrentTerminalQuotes + $"bq load --source_format=NEWLINE_DELIMITED_JSON {ProjectId}:{dataSet}.{tableName} {source} product/resources/product_schema.json" + CurrentTerminalQuotes;

            var procStartInfo = new ProcessStartInfo(CurrentTerminalFile, uploadDataCommand)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                consoleOutput = process.StandardOutput.ReadToEnd();
                Console.WriteLine(consoleOutput);
            }
        }

        [Attributes.Example]
        public static void PerformCreationOfBigQueryTable()
        {
            CreateBQDataSet(DataSetId);
            CreateBQTable(DataSetId, TableId);
            UploadDataToBQTable(DataSetId, TableId, "product/resources/products.json");
            CreateBQTable(DataSetId, InvalidTableId);
            UploadDataToBQTable(DataSetId, InvalidTableId, "product/resources/products_some_invalid.json");
        }
    }
}
