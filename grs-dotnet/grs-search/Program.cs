using System;

namespace grs_search
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("Please run the application again and pass one of the file names to run. ");
            }
            if (args[0] == "SearchSimpleQuery")
            {
                SearchSimpleQuery.Search();
            }
            if (args[0] == "SearchWithBoostSpec")
            {
                SearchWithBoostSpec.Search();
            }
            if (args[0] == "SearchWithFiltering")
            {
                SearchWithFiltering.Search();
            }
            if (args[0] == "SearchWithNumericalFacet")
            {
                SearchWithNumericalFacet.Search();
            }
            if (args[0] == "SearchWithOrdering")
            {
                SearchWithOrdering.Search();
            }
            if (args[0] == "SearchWithPaginationNextPageToken")
            {
                SearchWithPaginationNextPageToken.Search();
            }
            if (args[0] == "SearchWithPaginationPageSize")
            {
                SearchWithPaginationPageSize.Search();
            }
            if (args[0] == "SearchWithQueryExpansion")
            {
                SearchWithQueryExpansion.Search();
            }
            if (args[0] == "SearchWithTextualFacetExcludingFilterKey")
            {
                SearchWithTextualFacetExcludingFilterKey.Search();
            }
            if (args[0] == "SearchWithTextualFacet")
            {
                SearchWithTextualFacet.Search();
            }
            if (args[0] == "SearchWithTextualFacetRestrictingValues")
            {
                SearchWithTextualFacetRestrictingValues.Search();
            }
            if (args[0] == "SearchWithVariantRollupKeys")
            {
                SearchWithVariantRollupKeys.Search();
            }
        }
    }
}
