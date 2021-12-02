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

using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithQueryExpansion
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultSearchPlacement = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/placements/default_search";
        private const string Endpoint = "retail.googleapis.com";

        private static SearchServiceClient GetSearchServiceClient()
        {
            SearchServiceClientBuilder searchServiceClientBuilder =
                new SearchServiceClientBuilder
                {
                    Endpoint = Endpoint
                };
            SearchServiceClient searchServiceClient = searchServiceClientBuilder.Build();
            return searchServiceClient;
        }

        private static SearchRequest GetSearchRequest(string query, SearchRequest.Types.QueryExpansionSpec.Types.Condition condition)
        {
            SearchRequest.Types.QueryExpansionSpec queryExpansionSpec = new SearchRequest.Types.QueryExpansionSpec()
            {
                Condition = condition
            };
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Query = query,
                QueryExpansionSpec = queryExpansionSpec,
                VisitorId = "123456" // A unique identifier to track visitors
            };
            Console.WriteLine("Search for products using query expansion specification. request: \n" + request);
            return request;
        }

        [Attributes.Example]
        public static void Search()
        {
            // TRY DIFFERENT QUERY EXPANSION CONDITION HERE:
            var condition = SearchRequest.Types.QueryExpansionSpec.Types.Condition.Auto;
            
            SearchRequest request = GetSearchRequest("Google Youth Hero Tee Grey", condition);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using query expansion specification. response: \n" + item);
            }
        }
    }
}
// [END retail_search_for_products_with_query_expansion_specification]