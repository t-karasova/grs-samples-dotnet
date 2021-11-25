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

#region retail_search_product_with_boost_spec
// Call Retail API to search for a products in a catalog, rerank the
// results boosting or burying the products that match defined condition.

using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithBoostSpec
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

        private static SearchRequest GetSearchRequest(string query, string condition, float boostStrength)
        {
            const string defaultSearchPlacement =
                "projects/" + ProjectNumber + "/locations/global/catalogs/default_catalog/placements/default_search";

            SearchRequest.Types.BoostSpec.Types.ConditionBoostSpec conditionBoostSpec =
                new SearchRequest.Types.BoostSpec.Types.ConditionBoostSpec()
                {
                    Condition = condition,
                    Boost = boostStrength
                };
            SearchRequest request = new SearchRequest()
            {
                Placement = defaultSearchPlacement,
                Query = query,
                BoostSpec = new SearchRequest.Types.BoostSpec()
                {
                    ConditionBoostSpecs = {conditionBoostSpec}
                },
                VisitorId = "123456" // A unique identifier to track visitors
            };
            Console.WriteLine("Search for products using boost specification. request: \n" + request);
            return request;
        }

        [Attributes.Example]
        public static void Search()
        {
            // TRY DIFFERENT CONDITIONS HERE:
            string condition = "colorFamily: ANY(\"blue\")";
            float boost = 1f;

            SearchRequest request = GetSearchRequest("Tee", condition, boost);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using boost specification. response: \n" + item);
            }
        }
    }
}
#endregion