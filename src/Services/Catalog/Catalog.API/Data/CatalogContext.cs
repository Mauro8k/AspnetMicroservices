using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalagContext
    {
        private readonly IConfiguration _configuration;

        public CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var dataBase = client.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DataBaseName"));

            Products = dataBase.GetCollection<Product>(_configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products  {get;}
    }
}
