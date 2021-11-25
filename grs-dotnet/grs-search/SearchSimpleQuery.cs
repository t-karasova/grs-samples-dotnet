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

# region retail_search_for_products_with_query_parameter
// Call Retail API to search for a products in a catalog using only search query.

using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchSimpleQuery
    {
        private const string ProjectNumber = "945579214386";
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

        private static SearchRequest GetSearchRequest(string query)
        {
            const string defaultSearchPlacement =
                "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";
            
            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                VisitorId = "123456" // A unique identifier to track visitors
            };
            Console.WriteLine("Search for products by query. request: \n" + request);
            return request;
        }

        [Attributes.Example]
        public static void Search()
        {
            SearchRequest request = GetSearchRequest("Hoodie");
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("search for products by query. response : \n" + item);
            }
        }
    }
}
#endregion