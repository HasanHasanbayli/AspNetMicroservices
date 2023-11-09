using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Context;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        DatabaseSettings databaseSettings = configuration.GetSection(key: "DatabaseSettings").Get<DatabaseSettings>()!;

        MongoClient client = new(databaseSettings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(name: databaseSettings.DatabaseName);
        Products = database.GetCollection<Product>(name: databaseSettings.CollectionName);

        CatalogSeedData.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}