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

# region retail_search_for_products_with_page_size
// Call Retail API to search for a products in a catalog,
// limit the number of the products per page and go to the next page using "next_page_token"
// or jump to chosen page using "offset".

using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithPagination
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

        private static SearchRequest GetSearchRequest(string query, int pageSize, int offset, string nextPageToken)
        {
            const string defaultSearchPlacement =
                "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";

            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                VisitorId = "123456", //A unique identifier to track visitors
                Query = query,
                PageSize = pageSize,
                Offset = offset,
                PageToken = nextPageToken
            };
            Console.WriteLine("search for products defining page size. request: \n" + request);

            return request;
        }

        [Attributes.Example]
        public static void Search()
        {
            //TRY DIFFERENT PAGINATION PARAMETERS HERE:
            int pageSize = 6;
            int offset = 0;
            string nextPageToken = "";

            SearchRequest request = GetSearchRequest("Tee", pageSize, offset, nextPageToken);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            Page<SearchResponse.Types.SearchResult> page = response.ReadPage(pageSize);
            foreach (SearchResponse.Types.SearchResult item in page)
            {
                Console.WriteLine("search for products defining page size. response: \n" + item);
            }
        }
    }
}
#endregion