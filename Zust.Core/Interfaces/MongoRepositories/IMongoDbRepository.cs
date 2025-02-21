using MongoDB.Driver;
using Zust.Core.Enums;
using Zust.Core.MongoEntities.Common;

namespace Zust.Core.Interfaces.MongoRepositories;

public interface IMongoDbRepository<T> where T : BaseMongoEntity, new()
{
    IMongoCollection<T> GetCollection(MongoCollections collectionName);
    Task<List<T>> GetCollectionList(MongoCollections collectionName);
    Task<List<T>> GetCollectionListWhere(FilterDefinition<T> filter, MongoCollections collectionName);
    Task InsertToCollectionAsync(T data, MongoCollections collectionName);
    Task InsertManyToCollectionAsync(List<T> data, MongoCollections collectionName);
    Task DeleteOneAsync(FilterDefinition<T> filter, MongoCollections collectionName);
    Task<bool> IsExistsAsync(FilterDefinition<T> filter, MongoCollections collectionName);
}
