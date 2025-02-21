using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zust.Core.Enums;
using Zust.Core.Interfaces.MongoRepositories;
using Zust.Core.MongoEntities;
using Zust.Core.MongoEntities.Common;
using Zust.DAL.Options;

namespace Zust.DAL.MongoRepositories.Implements;

public class MongoDbRepository<T> : IMongoDbRepository<T> where T : BaseMongoEntity, new()
{
    private readonly IMongoDatabase _database;
    public MongoDbRepository(IOptions<MongoOption> opt)
    {
        var connectionString = opt.Value.Connection.Replace("<db_password>", opt.Value.Password);
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(MongoOption.DatabaseName);

        CreateNotificationIndexesAsync().Wait();
    }

    public IMongoCollection<T> GetCollection(MongoCollections collectionName)
        => _database.GetCollection<T>(collectionName.ToString());

    public async Task<List<T>> GetCollectionList(MongoCollections collectionName)
    {
        var data = await _database.GetCollection<T>(collectionName.ToString()).FindAsync(_getGlobalFilter());
        var dataList = await data.ToListAsync();
        return dataList;
    }

    public async Task<List<T>> GetCollectionListWhere(FilterDefinition<T> filter, MongoCollections collectionName)
    {
        var combinedFilter = Builders<T>.Filter.And(_getGlobalFilter(), filter);

        var data = await _database.GetCollection<T>(collectionName.ToString()).FindAsync(combinedFilter);
        var dataList = await data.ToListAsync();
        return dataList;
    }

    public async Task InsertManyToCollectionAsync(List<T> data, MongoCollections collectionName)
    {
        var collection = GetCollection(collectionName);
        await collection.InsertManyAsync(data);
    }

    public async Task InsertToCollectionAsync(T data, MongoCollections collectionName)
    {
        var collection = GetCollection(collectionName);
        await collection.InsertOneAsync(data);
    }

    public async Task DeleteOneAsync(FilterDefinition<T> filter, MongoCollections collectionName)
    {
        var collection = GetCollection(collectionName);
        await collection.DeleteOneAsync(filter);
    }

    public async Task<bool> IsExistsAsync(FilterDefinition<T> filter, MongoCollections collectionName)
    {
        var collection = await GetCollectionListWhere(filter, collectionName);
        if (collection.Count == 0) return false;
        return true;
    }

    // global filter
    private FilterDefinition<T> _getGlobalFilter()
    {
        return Builders<T>.Filter.Ne(x => x.IsHidden, true);
    }

    // index creator
    private async Task CreateNotificationIndexesAsync()
    {
        if (typeof(T).Name.ToString().ToLower() is "notification")
        {
            var notificationCollection = (IMongoCollection<Notification>)GetCollection(MongoCollections.Notifications);

            var existingIndexes = await notificationCollection.Indexes.ListAsync();
            var indexNames = await existingIndexes.ToListAsync(); // listing index names

            if (indexNames.Any(ix => ix["name"] == "ReceiverId_Index")) // check if index name exists, then return
                return;

            var indexKeys = Builders<Notification>.IndexKeys
                .Ascending(n => n.ReceiverId);

            var indexModel = new CreateIndexModel<Notification>(indexKeys, new CreateIndexOptions { Name = "ReceiverId_Index" });

            await notificationCollection.Indexes.CreateOneAsync(indexModel);
        }
    }
}
