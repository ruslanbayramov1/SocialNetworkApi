using MongoDB.Bson.Serialization.Attributes;
using Zust.Core.Enums;

namespace Zust.Core.MongoEntities;

/// <summary>
/// 
/// </summary>
public class PostLikeNotification
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? TypeId { get; set; }
    public NotificationTypes? Type { get; set; }
    public NotificationActions Action { get; set; }
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
