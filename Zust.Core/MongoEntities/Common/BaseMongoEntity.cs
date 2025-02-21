using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Zust.Core.MongoEntities.Common;

public class BaseMongoEntity
{

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsHidden { get; set; } = false;
}
