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

// [START retail_search_for_products_with_query_parameter]
// Call Retail API to search for a products in a catalog using only search query.

using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search.search
{
    public static class SearchSimpleQuery
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultSearchPlacement = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/placements/default_search";
        private const string Endpoint = "retail.googleapis.com";
        
        private static SearchServiceClient GetSearchServiceClient()
        {
            var searchServiceClientBuilder = new SearchServiceClientBuilder
            {
                Endpoint = Endpoint
            };

            var searchServiceClient = searchServiceClientBuilder.Build();
            return searchServiceClient;
        }

        private static SearchRequest GetSearchRequest(string query)
        {
            var searchRequest = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Query = query,
                VisitorId = "123456", // A unique identifier to track visitors
                PageSize = 10
            };

            Console.WriteLine("Search for products by query. request: \n" + searchRequest);
            return searchRequest;
        }

        [Attributes.Example]
        public static PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> Search()
        {
            var query = "Hoodie";
            
            var searchRequest = GetSearchRequest(query);
            var searchResponse = GetSearchServiceClient().Search(searchRequest);

            foreach (var item in searchResponse)
            {
                Console.WriteLine("Search. response : \n" + item);
            }

            return searchResponse;
        }
    }
}
// [END retail_search_for_products_with_query_parameter]