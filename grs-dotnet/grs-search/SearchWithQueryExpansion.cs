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
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithQueryExpansion
    {
        private const string Endpoint = "test-retail.sandbox.googleapis.com";

        // TODO Define the project number here:
        private const string ProjectNumber = "";

        //[START get_search_client]
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
        //[END get_search_client]

        //[START get_search_request_with_query_expansion]
        private static SearchRequest GetSearchRequest(string query,
            SearchRequest.Types.QueryExpansionSpec.Types.Condition condition)
        {
            const string defaultSearchPlacement =
                "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";


            SearchRequest.Types.QueryExpansionSpec queryExpansionSpec = new SearchRequest.Types.QueryExpansionSpec()
            {
                Condition = condition
            };
            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                QueryExpansionSpec = queryExpansionSpec,
                VisitorId = "123456"
            };
            Console.WriteLine("Search for products using query expansion specification. request: \n" + request);
            return request;
        }
        //[END get_search_request_with_query_expansion]

        // [START search_for_products_using_query_expansion_specification]
        [Attributes.Example]
        public static void Search()
        {
            // TRY DIFFERENT QUERY EXPANSION CONDITION HERE:
            SearchRequest request = GetSearchRequest("Google Youth Hero Tee Grey",
                SearchRequest.Types.QueryExpansionSpec.Types.Condition.Auto);

            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using query expansion specification. response: \n" + item);
            }
        }
        // [END search_for_products_using_query_expansion_specification]
    }
}