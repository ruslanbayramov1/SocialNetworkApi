using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Zust.Core.Enums;
using Zust.Core.MongoEntities.Common;

namespace Zust.Core.MongoEntities;

/// <summary>
/// All notification handling
/// </summary>
public class Notification : BaseMongoEntity
{
    public Notification()
    {
        
    }

    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    [BsonRepresentation(BsonType.String)]
    public Guid ReceiverId { get; set; }
    /// <summary>
    /// Entity id for notification types
    /// </summary>
    public string? RelatedEntityId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public NotificationTypes? Type { get; set; }
    [BsonRepresentation(BsonType.String)]
    public NotificationActions Action { get; set; }
}
