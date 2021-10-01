using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_samples_dotnet
{
    public static class SearchWithNumericalFacet
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

        //[START search for products and return numerical facet]
        private static void SearchProductNumericalFacet(string query, string key,
            Google.Cloud.Retail.V2.Interval interval)
        {
            SearchRequest.Types.FacetSpec facetSpec = new SearchRequest.Types.FacetSpec()
            {
                FacetKey = new SearchRequest.Types.FacetSpec.Types.FacetKey()
                {
                    Key = key,
                    Intervals = {interval}
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
            Console.WriteLine("Search for products and return numerical facet. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            foreach (SearchResponse item in response.AsRawResponses())
            {
                Console.WriteLine("Search for products and return numerical facet. response: \n" + item.Facets);
            }
        }

        //[END search for products and return numerical facet]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            Interval interval = new Interval()
            {
                Minimum = 10.0f,
                Maximum = 20.0f
            };
            SearchProductNumericalFacet(Query, "price", interval);

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}