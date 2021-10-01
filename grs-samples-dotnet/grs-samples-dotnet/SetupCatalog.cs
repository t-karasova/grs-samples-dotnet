using System;
using System.IO;
using System.Threading;
using Google.Cloud.Retail.V2;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;

namespace grs_samples_dotnet
{
    public class SetupCatalog
    {
        private const string Endpoint = "test-retail.sandbox.googleapis.com";

        private const string BranchName =
            "projects/1038874412926/locations/global/catalogs/default_catalog/branches/default_branch";

        private const string Query = "test_query";

        private static FieldMask _fieldMask = new FieldMask()
        {
            Paths = {"name", "title", "price_info", "color_info", "brands"}
        };

        private static string _createdPrimaryProductName = null;
        private static string _createdVariantProductName = null;

        private static string GetRandomName()
        {
            return Path.GetRandomFileName().Replace(".", "").Substring(0, 8);
        }

        //[START get product client]
        private static ProductServiceClient GetProductServiceClient()
        {
            ProductServiceClientBuilder productServiceClientBuilder =
                new ProductServiceClientBuilder
                {
                    Endpoint = Endpoint
                };
            ProductServiceClient productServiceClient = productServiceClientBuilder.Build();
            return productServiceClient;
        }
        //[END get product client]

        //[START prepare Primary product sample]
        private static Product GetPrimaryProductSample()
        {
            Product product =
                new Product()
                {
                    Name = BranchName + "/" + GetRandomName(),
                    Title = Query,
                    Type = Product.Types.Type.Primary,
                    Categories = {"Nest > speakers and displays"},
                    Uri = "https://www.test-uri.com",
                    Brands = {"Google"},
                    ColorInfo = new ColorInfo()
                    {
                        ColorFamilies = {"black"},
                        Colors = {"carbon"}
                    },
                    PriceInfo = new PriceInfo()
                    {
                        Price = (float) 20.0,
                        OriginalPrice = (float) 25.0,
                        Cost = (float) 10.0,
                        CurrencyCode = "USD"
                    },
                    FulfillmentInfo =
                    {
                        new RepeatedField<FulfillmentInfo>()
                        {
                            new FulfillmentInfo()
                            {
                                Type = "ship-to-store",
                                PlaceIds = {"store1", "store2"}
                            }
                        }
                    },
                    RetrievableFields = _fieldMask
                };
            return product;
        }
        //[END prepare Primary product sample]

        //[START prepare Variant product sample]
        private static Product GetVariantProductSample()
        {
            return
                new Product()
                {
                    Name = BranchName + "/" + GetRandomName(),
                    Title = Query,
                    Type = Product.Types.Type.Variant,
                    Categories = {"Nest > speakers and displays"},
                    Uri = "https://www.test-uri.com",
                    Brands = {"Google"},
                    FulfillmentInfo =
                    {
                        new RepeatedField<FulfillmentInfo>()
                        {
                            new FulfillmentInfo()
                            {
                                Type = "ship-to-store",
                                PlaceIds = {"store2"}
                            }
                        }
                    },
                    RetrievableFields = _fieldMask
                };
        }

        //[END prepare Variant product sample]

        //[START delete product]
        private static void DeleteProduct(string productName)
        {
            GetProductServiceClient().DeleteProduct(productName);
            Console.WriteLine("product " + productName + " deleted");
        }
        //[END delete product]

        //[START create primary product with related variant product]
        private static void CreatePrimaryAndRelatedVariantProduct(Product primaryProduct, Product variantProduct)
        {
            Product primaryCreated =
                GetProductServiceClient().CreateProduct(BranchName, primaryProduct, GetRandomName());
            _createdPrimaryProductName = primaryCreated.Name;
            variantProduct.PrimaryProductId = primaryCreated.Id;
            Product variantCreated =
                GetProductServiceClient().CreateProduct(BranchName, variantProduct, GetRandomName());
            _createdVariantProductName = variantCreated.Name;
            Console.WriteLine("Created Primary products' names: \n" + _createdPrimaryProductName);
            Console.WriteLine("Created Variant products' names: \n" + _createdVariantProductName);
            Console.WriteLine("Wait for the products to become indexed");
            Thread.Sleep(3000);
        }
        //[END create primary product with related variant product]

        public static void IngestProducts()
        {
            //Create products for search
            Product primaryProduct = GetPrimaryProductSample();
            Product variantProduct = GetVariantProductSample();
            CreatePrimaryAndRelatedVariantProduct(primaryProduct, variantProduct);
        }

        public static void DeleteIngestedProducts()
        {
            //Delete Variant product
            DeleteProduct(_createdVariantProductName);
            //Delete Primary product
            DeleteProduct(_createdPrimaryProductName);
        }
    }
}