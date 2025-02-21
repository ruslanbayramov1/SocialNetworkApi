using MongoDB.Driver;
using Zust.Core.MongoEntities.Common;

namespace Zust.Core.Interfaces.MongoRepositories;

public interface IMongoDbRepository<T> where T : BaseMongoEntity, new()
{
    IMongoCollection<T> GetCollection();
    Task<List<T>> GetCollectionList();
    Task<List<T>> GetCollectionListWhere(FilterDefinition<T> filter);
    Task<T> GetOneById(Guid id);
    Task UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> data);
    Task<T> GetOneWhere(FilterDefinition<T> filter);
    Task InsertToCollectionAsync(T data);
    Task InsertManyToCollectionAsync(List<T> data);
    Task DeleteOneAsync(FilterDefinition<T> filter);
    Task<bool> IsExistsAsync(FilterDefinition<T> filter);
}
