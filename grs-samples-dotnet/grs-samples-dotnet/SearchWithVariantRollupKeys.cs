using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;
using Google.Protobuf.Collections;

namespace grs_samples_dotnet
{
    public static class SearchWithVariantRollupKeys
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

        //[START search for products with variant rollup keys]
        private static void SearchProductWithVariantRollupKeys(string query, RepeatedField<string> variantRollupKeys)
        {
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                VariantRollupKeys = {variantRollupKeys},
                VisitorId = VisitorId
            };
            Console.WriteLine("Search for products with variant rollup keys. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse.Types.SearchResult item in response)
            {
                Console.WriteLine("Search for products with variant rollup keys. response: \n" + item);
            }
        }

        //[END search for products with variant rollup keys]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            // Search for products with variant rollup keys
            RepeatedField<string> variantRollupKeys = new RepeatedField<string>() {"shipToStore.stare2"};
            SearchProductWithVariantRollupKeys(Query, variantRollupKeys);

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}