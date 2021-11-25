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
    public static class SearchWithFiltering
    {
        private const string ProjectNumber = "945579214386";
        private const string Endpoint = "retail.googleapis.com";
                
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
        
        //[START get_search_request_with_filter]
        private static SearchRequest GetSearchRequest(string query, string filter)
        {
            const string defaultSearchPlacement =
                "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";

            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                Filter = filter,
                VisitorId = "123456"
            };
            Console.WriteLine("Search for products using filter. request: \n" + request);
            return request;
        }
        //[END get_search_request_with_filter]
        
        //[START search_for_products_using_filter]
        [Attributes.Example]
        public static void Search()
        {
            //TRY DIFFERENT FILTER EXPRESSIONS HERE:
            string filter = "(colorFamily: ANY(\"black\"))";
            
            SearchRequest request = GetSearchRequest("Tee", filter);
            
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using filter. response: \n" + item);
            }
        }
        //[END search_for_products_using_filter]
    }
}