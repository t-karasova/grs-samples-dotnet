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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace grs_events.Tests
{
    [TestClass]
    public class ImportUserEventsGcsTest
    {
        private const string EventsFolderName = "grs-events";
        private static readonly string WorkingDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, EventsFolderName);

        private const string WindowsTerminalVarName = "ComSpec";
        private const string UnixTerminalVarName = "SHELL";
        private static readonly string CurrentTerminalVarName = Environment.OSVersion.VersionString.Contains("Windows") ? WindowsTerminalVarName : UnixTerminalVarName;
        private static readonly string CurrentTerminalFile = Environment.GetEnvironmentVariable(CurrentTerminalVarName);
        private const string CommandLineArguments = "/c " + "dotnet run -- ImportUserEventsGcs"; // The "/c" tells cmd to execute the command that follows, and then exit.

        [TestMethod]
        public void TestOutputImportUserEventsGcs()
        {
            string consoleOutput = string.Empty;

            var processStartInfo = new ProcessStartInfo(CurrentTerminalFile, CommandLineArguments)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = WorkingDirectory
            };

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;

                process.Start();

                consoleOutput = process.StandardOutput.ReadToEnd();
            }

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Import user events from google cloud source. request: (.*)\"parent\": \"projects/(.*)/locations/global/catalogs/default_catalog(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Import user events from google cloud source. request: (.*)\"inputConfig\": (.*)\"gcsSource\"(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)The operation was started:(.*)projects/(.*)/locations/global/catalogs/default_catalog(.*)/operations/import-user-events(.*)", RegexOptions.Singleline).Success);

            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Import user events operation is done(.*)").Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Number of successfully imported events:(.*)4(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Number of failures during the importing: (.*)0(.*)", RegexOptions.Singleline).Success);
            Assert.IsTrue(Regex.Match(consoleOutput, "(.*)Operation result: (.*)\"errorsConfig\"(.*)", RegexOptions.Singleline).Success);
        }
    }
}