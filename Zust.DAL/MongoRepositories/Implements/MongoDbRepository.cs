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
    private readonly MongoCollections _collectionName;
    public MongoDbRepository(IOptions<MongoOption> opt, MongoCollections collectionName)
    {
        var connectionString = opt.Value.Connection.Replace("<db_password>", opt.Value.Password);
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(MongoOption.DatabaseName);

        CreateNotificationIndexesAsync().Wait();
        _collectionName = collectionName;
    }

    public IMongoCollection<T> GetCollection()
        => _database.GetCollection<T>(_collectionName.ToString());

    public async Task<List<T>> GetCollectionList()
    {
        var data = await _database.GetCollection<T>(_collectionName.ToString()).FindAsync(_getGlobalFilter());
        var dataList = await data.ToListAsync();
        return dataList;
    }

    public async Task<List<T>> GetCollectionListWhere(FilterDefinition<T> filter)
    {
        var combinedFilter = Builders<T>.Filter.And(_getGlobalFilter(), filter);

        var data = await _database.GetCollection<T>(_collectionName.ToString()).FindAsync(combinedFilter);
        var dataList = await data.ToListAsync();
        return dataList;
    }

    public async Task<T> GetOneWhere(FilterDefinition<T> filter)
    {
        var combinedFilter = Builders<T>.Filter.And(_getGlobalFilter(), filter);

        var data = await _database.GetCollection<T>(_collectionName.ToString()).FindAsync(combinedFilter);
        var dataOne = await data.FirstOrDefaultAsync();
        return dataOne;
    }

    public async Task InsertManyToCollectionAsync(List<T> data)
    {
        var collection = GetCollection();
        await collection.InsertManyAsync(data);
    }

    public async Task InsertToCollectionAsync(T data)
    {
        var collection = GetCollection();
        await collection.InsertOneAsync(data);
    }

    public async Task UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> data)
    {
        var collection = GetCollection();
        var result = await collection.UpdateOneAsync(filter, data);
    }

    public async Task DeleteOneAsync(FilterDefinition<T> filter)
    {
        var collection = GetCollection();
        await collection.DeleteOneAsync(filter);
    }

    public async Task<bool> IsExistsAsync(FilterDefinition<T> filter)
    {
        var collection = await GetCollectionListWhere(filter);
        if (collection.Count == 0) return false;
        return true;
    }

    public async Task<T> GetOneById(Guid id)
    {
        var dataOne = await GetOneWhere(Builders<T>.Filter.Eq(x => x.Id, id));
        return dataOne;
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
            var notificationCollection = (IMongoCollection<Notification>)GetCollection();

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
