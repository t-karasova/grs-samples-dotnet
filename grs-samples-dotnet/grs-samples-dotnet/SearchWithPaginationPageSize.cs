using System;
using Google.Api.Gax;
using Google.Cloud.Retail.V2;

namespace grs_samples_dotnet
{
    public static class SearchWithPaginationPageSize
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

        //[START search for products defining page size]
        private static string SearchProductWithPageSize(string query, int pageSize)
        {
            SearchRequest request = new SearchRequest()
            {
                Placement = DefaultSearchPlacement,
                Branch = BranchName,
                Query = query,
                PageSize = pageSize,
                VisitorId = VisitorId
            };
            Console.WriteLine("search for products defining page size. request: \n" + request);
            PagedEnumerable<SearchResponse, SearchResponse.Types.SearchResult> response =
                GetSearchServiceClient().Search(request);
            Page<SearchResponse.Types.SearchResult> page = response.ReadPage(pageSize);
            foreach (SearchResponse.Types.SearchResult item in page)
            {
                Console.WriteLine("search for products defining page size. response: \n" + item);
            }

            return page.NextPageToken;
        }
        //[END search for products defining page size]

        public static void Search()
        {
            SetupCatalog.IngestProducts();

            //Search for products defining page size
            SearchProductWithPageSize(Query, 2);

            SetupCatalog.DeleteIngestedProducts();
        }
    }
}