namespace Zust.BL.DTOs.Notifications;

public class PostLikeNotificationDto
{
    public Guid PostedUserId { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}
