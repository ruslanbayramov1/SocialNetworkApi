namespace Zust.BL.DTOs.Notifications;

public class PostLikeNotificationCreateDto
{
    public Guid PostedUserId { get; set; }
    public string SenderUserName { get; set; } = null!;
    public Guid SenderUserId { get; set; }
    public Guid PostId { get; set; }
}
