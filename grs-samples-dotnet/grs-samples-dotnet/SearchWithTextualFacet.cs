using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_samples_dotnet
{
    public static class SearchWithTextualFacet
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

        //[START search for products and return textual facet]
        private static void SearchProductTextualFacet(string query, string key)
        {
            SearchRequest.Types.FacetSpec facetSpec = new SearchRequest.Types.FacetSpec()
            {
                FacetKey = new SearchRequest.Types.FacetSpec.Types.FacetKey()
                {
                    Key = key
                }
            };
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                FacetSpecs = {facetSpec},
                VisitorId = VisitorId
            };
            Console.WriteLine("Search for products and return textual facet. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse item in response.AsRawResponses())
            {
                Console.WriteLine("Search for products and return textual facet. response: \n" + item.Facets);
            }
        }
        //[END search for products and return textual facet]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            // Search for products and return textual facets
            SearchProductTextualFacet(Query, "colorFamily");

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}