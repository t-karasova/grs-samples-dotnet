using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_search
{
    public static class SearchWithQueryExpansion
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

        //[START search for products using query expansion specification]
        private static void SearchProductWithQueryExpansion(string query,
            SearchRequest.Types.QueryExpansionSpec.Types.Condition condition)
        {
            SearchRequest.Types.QueryExpansionSpec queryExpansionSpec = new SearchRequest.Types.QueryExpansionSpec()
            {
                Condition = condition
            };
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                QueryExpansionSpec = queryExpansionSpec,
                VisitorId = VisitorId
            };
            Console.WriteLine("Search for products using query expansion specification. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products using query expansion specification. response: \n" + item);
            }
        }
        //[END search for products using query expansion specification]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            // Search for products using query expansion specification
            SearchProductWithQueryExpansion(Query, SearchRequest.Types.QueryExpansionSpec.Types.Condition.Auto);

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}