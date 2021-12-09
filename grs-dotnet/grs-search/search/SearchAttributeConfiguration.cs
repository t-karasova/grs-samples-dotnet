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

// [START retail_search_with_filter_by_attribute]
// Call Retail API to search for a products in a catalog, filter the results by the "product.attribute" field.

using Google.Api.Gax;
using Google.Cloud.Retail.V2;
using System;

namespace grs_search.search
{
    public static class SearchAttributeConfiguration
    {
        private static readonly string ProjectNumber = Environment.GetEnvironmentVariable("PROJECT_NUMBER");
        private static readonly string DefaultSearchPlacement = $"projects/{ProjectNumber}/locations/global/catalogs/default_catalog/placements/default_search";
        private const string Endpoint = "retail.googleapis.com";
        private const string QueryFilter = "(attributes.ecofriendly: ANY(\"recycled packaging\"))";

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
                Filter = QueryFilter,
                PageSize = 10,
                VisitorId = "123456" // A unique identifier to track visitors
            };

            Console.WriteLine("Search. request: \n" + searchRequest);
            return searchRequest;
        }

        [Attributes.Example]
        public static PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> Search()
        {
            string query = "sweater";
            var request = GetSearchRequest(query);

            var searchResponse = GetSearchServiceClient().Search(request);

            foreach (var item in searchResponse)
            {
                Console.WriteLine("Search. response: \n" + item);
            }

            return searchResponse;
        }
    }
}
// [END retail_search_with_filter_by_attribute]