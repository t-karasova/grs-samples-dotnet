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

// [START retail_search_for_products_with_query_expansion_specification]
// Call Retail API to search for a products in a catalog,
// enabling the query expansion feature to let the Google Retail Search build an automatic query expansion.

using Google.Cloud.Retail.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace grs_search.search
{
    public static class SearchWithQueryExpansion
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultSearchPlacement = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/placements/default_search";
        private const string Endpoint = "retail.googleapis.com";

        // Get search service client
        private static SearchServiceClient GetSearchServiceClient()
        {
            var searchServiceClientBuilder = new SearchServiceClientBuilder
            {
                Endpoint = Endpoint
            };

            var searchServiceClient = searchServiceClientBuilder.Build();
            return searchServiceClient;
        }

        // Get search service request
        private static SearchRequest GetSearchRequest(string query, SearchRequest.Types.QueryExpansionSpec.Types.Condition condition)
        {
            var queryExpansionSpec = new SearchRequest.Types.QueryExpansionSpec()
            {
                Condition = condition
            };
            var searchRequest = new SearchRequest()
            {
                Placement = DefaultSearchPlacement, // Placement is used to identify the Serving Config name
                Query = query,
                QueryExpansionSpec = queryExpansionSpec,
                VisitorId = "123456", // A unique identifier to track visitors
                PageSize = 10
            };

            var jsonSerializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var searchRequestJson = JsonConvert.SerializeObject(searchRequest, jsonSerializeSettings);

            Console.WriteLine("\nSearch. request: \n\n" + searchRequestJson);
            return searchRequest;
        }

        // Call the Retail Search:
        [Attributes.Example]
        public static IEnumerable<SearchResponse> Search() // PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult>
        {
            // TRY DIFFERENT QUERY EXPANSION CONDITION HERE:
            var condition = SearchRequest.Types.QueryExpansionSpec.Types.Condition.Auto;
            string query = "Google Youth Hero Tee Grey";

            var searchRequest = GetSearchRequest(query, condition);

            var searchResponse = GetSearchServiceClient().Search(searchRequest).AsRawResponses(); //.AsRawResponses();

            Console.WriteLine("\nSearch. response: \n");

            var jsonSerializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var firstSearchResponse = searchResponse.FirstOrDefault();

            if (firstSearchResponse != null)
            {
                var objectToSerialize = new
                {
                    results = firstSearchResponse.Results,
                    totalSize = firstSearchResponse.TotalSize,
                    attributionToken = firstSearchResponse.AttributionToken,
                    nextPageToken = firstSearchResponse.NextPageToken,
                    facets = firstSearchResponse.Facets,
                    queryExpansionInfo = firstSearchResponse.QueryExpansionInfo
                };

                var serializedJson = JsonConvert.SerializeObject(objectToSerialize, jsonSerializeSettings);

                Console.WriteLine(serializedJson + "\n");
            }

            return searchResponse;
        }
    }
}
// [END retail_search_for_products_with_query_expansion_specification]