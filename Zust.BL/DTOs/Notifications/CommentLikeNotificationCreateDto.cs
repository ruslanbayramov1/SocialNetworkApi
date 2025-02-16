namespace Zust.BL.DTOs.Notifications;

public class CommentLikeNotificationCreateDto
{
    public Guid SenderUserId { get; set; }
    public string SenderUserName { get; set; }
    public Guid CommentedUserId { get; set; }
    public Guid CommentId { get; set; }
}
