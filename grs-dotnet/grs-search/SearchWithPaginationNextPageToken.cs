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
    public static class SearchWithPaginationNextPageToken
    {
        // TODO Define the project number here:
        private const string ProjectNumber = "";
        private const string Endpoint = "test-retail.sandbox.googleapis.com";

        private const string defaultSearchPlacement =
            "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";


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

        //[START get_search_request_with_for_first_page]
        private static SearchRequest GetSearchRequestFirstPage(string query, int pageSize)
        {
            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                PageSize = pageSize,
                VisitorId = "123456"
            };
            Console.WriteLine("search for products defining page size. request: \n" + request);
            return request;
        }
        //[END get_search_request_with_for_first_page]

        //[START get_search_request_with_for_next_page]
        private static SearchRequest GetSearchRequestNextPage(string query,
            int pageSize, string nextPageToken)
        {
            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                PageSize = pageSize,
                PageToken = nextPageToken,
                VisitorId = "123456"
            };
            Console.WriteLine("search for products defining page size and next page token. request: \n" + request);
            return request;
        }
        //[END get_search_request_with_for_next_page]

        //[START search_for_products_defining_page_size_and_next_page_token]
        [Attributes.Example]
        public static void Search()
        {
            int pageSize = 6;

            // Get the first page of the search response
            SearchRequest firstPageRequest = GetSearchRequestFirstPage("Tee", pageSize);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> firstPageResponse =
                GetSearchServiceClient().Search(firstPageRequest);
            Page<SearchResponse.Types.SearchResult> firstPage = firstPageResponse.ReadPage(pageSize);
            string nextPageToken = firstPage.NextPageToken;

            // Get the next page of the search response using nextPageToken from the previous request
            SearchRequest nextPageRequest = GetSearchRequestNextPage("Tee", pageSize, nextPageToken);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> nextPageResponse =
                GetSearchServiceClient().Search(nextPageRequest);
            Page<SearchResponse.Types.SearchResult> nextPage = nextPageResponse.ReadPage(pageSize);
            foreach (SearchResponse.Types.SearchResult item in nextPage)
            {
                Console.WriteLine("search for products defining page size and next page token. response: \n" + item);
            }
        }
        //[END search_for_products_defining_page_size_and_next_page_token]
    }
}