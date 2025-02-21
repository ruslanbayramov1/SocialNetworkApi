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
        var data = await _database.GetCollection<T>(collectionName.ToString()).FindAsync(null);
        var dataList = await data.ToListAsync();
        return dataList;
    }

    public async Task<List<T>> GetCollectionListWhere(FilterDefinition<T> filter, MongoCollections collectionName)
    {
        var data = await _database.GetCollection<T>(collectionName.ToString()).FindAsync(filter);
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

    public async Task DeleteManyAsync(FilterDefinition<T> filter, MongoCollections collectionName)
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

    private async Task<IMongoCollection<T>> _applyGlobalFilters(IMongoCollection<T> collection)
    {
        var filter = Builders<T>.Filter.Ne(x => x.IsHidden, true);
        await collection.FindAsync(filter);
        return collection;
    }

    // index creator
    private async Task CreateNotificationIndexesAsync()
    {
        if (typeof(T) is Notification)
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
