using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;
using Google.Protobuf.Collections;

namespace grs_search
{
    public static class SearchWithTextualFacetExcludingFilterKey
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

        //[START search for products and return textual facet excluding filter key]
        private static void SearchProductTextualFacetExcludingFilterKey(string query, string key,
            RepeatedField<string> excludedFilterKeys, string filter)
        {
            SearchRequest.Types.FacetSpec facetSpec = new SearchRequest.Types.FacetSpec()
            {
                FacetKey = new SearchRequest.Types.FacetSpec.Types.FacetKey()
                {
                    Key = key
                },
                ExcludedFilterKeys = {excludedFilterKeys}
            };
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                Filter = filter,
                FacetSpecs = {facetSpec},
                VisitorId = VisitorId
            };
            Console.WriteLine(
                "Search for products and return textual facet excluding filter key. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse item in response.AsRawResponses())
            {
                Console.WriteLine("Search for products and return textual facet excluding filter key. response: \n" +
                                  item.Facets);
            }
        }
        //[END search for products and return textual facet excluding filter key]


        public static void Search()
        {
            SetupCatalog.IngestProducts();

            // Search for products and return textual facets excluding filter key
            RepeatedField<string> excludedFilterKeys = new RepeatedField<string>() {"colorFamily"};
            SearchProductTextualFacetExcludingFilterKey(Query, "colorFamily", excludedFilterKeys,
                "colorFamily: ANY(\"black\")");

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}