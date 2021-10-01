using System;

namespace grs_samples_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            // if (args.Length == 0)
            // {
            //     Console.Write("Please pass one of the file names to run");
            // }
            // if (args[0] == "SearchSimpleQuery")
            // {
            //     SearchSimpleQuery.Search();
            // }
            // if (args[0] == "SearchWithFiltering")
            // {
            //     SearchWithFiltering.Search();
            // }
            SearchSimpleQuery.Search();
            SearchWithBoostSpec.Search();
            SearchWithFiltering.Search();
            SearchWithNumericalFacet.Search();
            SearchWithOrdering.Search();
            SearchWithPaginationNextPageToken.Search();
            SearchWithPaginationPageSize.Search();
            SearchWithQueryExpansion.Search();
            SearchWithTextualFacetExcludingFilterKey.Search();
            SearchWithTextualFacet.Search();
            SearchWithTextualFacetRestrictingValues.Search();
            SearchWithVariantRollupKeys.Search();
        }
    }
}