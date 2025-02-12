using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zust.BL.Enums;
using Zust.BL.Options;
using Zust.BL.Services.Interfaces;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Implements;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;
    public MongoDbService(IOptions<MongoOption> opt)
    {
        var connectionString = opt.Value.Connection.Replace("<db_password>", opt.Value.Password);
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(MongoOption.DatabaseName);

        CreatePostNotificationIndexesAsync().Wait();
    }

    public IMongoCollection<T> GetCollection<T>(MongoCollections collectionName)
        => _database.GetCollection<T>(collectionName.ToString());

    public async Task InsertToCollectionAsync<T>(T data, MongoCollections collectionName)
    {
        var collection = GetCollection<T>(collectionName);
        await collection.InsertOneAsync(data);
    }

    private async Task CreatePostNotificationIndexesAsync()
    {
        var notificationCollection = GetCollection<PostLikeNotification>(MongoCollections.PostNotifications);

        var existingIndexes = await notificationCollection.Indexes.ListAsync();
        var indexNames = await existingIndexes.ToListAsync(); // listing index names

        if (indexNames.Any(ix => ix["name"] == "ReceiverId_Index")) // check if index name exists, then return
            return;

        var indexKeys = Builders<PostLikeNotification>.IndexKeys
            .Ascending(n => n.ReceiverId);

        var indexModel = new CreateIndexModel<PostLikeNotification>(indexKeys, new CreateIndexOptions { Name = "ReceiverId_Index" });

        await notificationCollection.Indexes.CreateOneAsync(indexModel);
    }
}
