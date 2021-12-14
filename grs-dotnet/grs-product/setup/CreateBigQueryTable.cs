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
    public static class CreateBigQueryTable
    {
        private static readonly string ProjectId = Environment.GetEnvironmentVariable("PROJECT_ID");

        private const string DataSetId = "products";
        private const string TableId = "products";
        private const string InvalidTableId = "products_some_invalid";

        const string CMDFileName = "cmd.exe";

        private static void CreateBQDataSet(string dataSetName)
        {
            var listDataSets = ListBQDataSets();
            if (!listDataSets.Contains(dataSetName))
            {
                string createDataSetCommand = "/c " + $"bq --location=US mk -d --default_table_expiration 3600 --description \"This is my dataset.\" {ProjectId}:{dataSetName}";
                string consoleOutput = string.Empty;

                var processStartInfo = new ProcessStartInfo(CMDFileName, createDataSetCommand);

                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                // processStartInfo.WorkingDirectory = WorkingDirectory;

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

            string listDataSetCommand = $"c/ bq ls --project_id {ProjectId}";

            var processStartInfo = new ProcessStartInfo(CMDFileName, listDataSetCommand);

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            // processStartInfo.WorkingDirectory = WorkingDirectory;

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

                var createTableCommand = $"c/ bq mk --table {ProjectId}:{dataSet}.{tableName} product/resources/product_schema.json";

                var procStartInfo = new ProcessStartInfo(CMDFileName, createTableCommand);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
 
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
            var listTablesCommand = $"c/ bq ls {ProjectId}:{dataSet}";
            
            var procStartInfo = new ProcessStartInfo(CMDFileName, listTablesCommand);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

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

            var uploadDataCommand = $"c/ bq load --source_format=NEWLINE_DELIMITED_JSON {ProjectId}:{dataSet}.{tableName} {source} product/resources/product_schema.json";

            var procStartInfo = new ProcessStartInfo(CMDFileName, uploadDataCommand);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

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
