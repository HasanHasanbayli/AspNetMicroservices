using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Context;

public class CatalogSeedData
{
    public static void SeedData(IMongoCollection<Product> products)
    {
        bool existProduct = products.Find(filter: product => true).Any();

        if (!existProduct)
        {
            products.InsertMany(documents: GetPreconfiguredProducts());
        }
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>
        {
            new()
            {
                Name = "IPhone X",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-1.png",
                Price = 950.00M,
                Category = "Smart Phone"
            },
            new()
            {
                Name = "Samsung 10",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-2.png",
                Price = 840.00M,
                Category = "Smart Phone"
            },
            new()
            {
                Name = "Huawei Plus",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-3.png",
                Price = 650.00M,
                Category = "White Appliances"
            },
            new()
            {
                Name = "Xiaomi Mi 9",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-4.png",
                Price = 470.00M,
                Category = "White Appliances"
            },
            new()
            {
                Name = "Samsung Galaxy Watch 4",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-5.png",
                Price = 350.00M,
                Category = "Smart Watch"
            },
            new()
            {
                Name = "Samsung Galaxy Watch 3",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-6.png",
                Price = 250.00M,
                Category = "Smart Watch"
            },
            new()
            {
                Name = "Samsung Galaxy Buds Pro",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-7.png",
                Price = 170.00M,
                Category = "Accessories"
            },
            new()
            {
                Name = "Samsung Galaxy Buds Live",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-8.png",
                Price = 150.00M,
                Category = "Accessories"
            },
            new()
            {
                Name = "Samsung Galaxy Buds",
                Summary = "Summary",
                Description = "Description",
                ImageFile = "product-9.png",
                Price = 130.00M,
                Category = "Access"
            }
        };
    }
}