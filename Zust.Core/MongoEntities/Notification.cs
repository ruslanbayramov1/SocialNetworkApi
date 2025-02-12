using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Zust.Core.Enums;

namespace Zust.Core.MongoEntities;

/// <summary>
/// All notification handling
/// </summary>
public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
