using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithBoostSpec
    {
        private const string Endpoint = "test-retail.sandbox.googleapis.com";

        private const string BranchName =
            "projects/1038874412926/locations/global/catalogs/default_catalog/branches/default_branch";

        private const string DefaultSearchPlacement =
            "projects/1038874412926/locations/global/catalogs/default_catalog/placements/default_search";

        private const string VisitorId = "visitor1";
        private const string Query = "test_query";

        //[START get Search client]
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
        //[END get Search client]

        //[START search for products using boost specification]
        private static void SearchProductWithBoostSpec(string query, string condition, float boostScore)
        {
            SearchRequest.Types.BoostSpec.Types.ConditionBoostSpec conditionBoostSpec =
                new SearchRequest.Types.BoostSpec.Types.ConditionBoostSpec()
                {
                    Condition = condition,
                    Boost = boostScore
                };
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                BoostSpec = new SearchRequest.Types.BoostSpec()
                {
                    ConditionBoostSpecs = {conditionBoostSpec}
                },
                VisitorId = VisitorId
            };
            Console.WriteLine("Search for products using boost specification. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using boost specification. response: \n" + item);
            }
        }
        //[END search for products using boost specification]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            // Search for products using boost specification
            SearchProductWithBoostSpec(Query, "colorFamily: ANY(\"black\")", 0.5f);

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}