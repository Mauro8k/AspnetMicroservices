using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalagContext
    {
        IMongoCollection<Product> Products { get; } 
    }
}
