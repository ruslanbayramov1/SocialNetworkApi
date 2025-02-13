using MongoDB.Driver;
using Zust.BL.Enums;

namespace Zust.BL.Services.Interfaces;

public interface IMongoDbService
{
    IMongoCollection<T> GetCollection<T>(MongoCollections collectionName);
    Task InsertToCollectionAsync<T>(T data, MongoCollections collectionName);
    Task InsertManyToCollectionAsync<T>(List<T> data, MongoCollections collectionName);
}
