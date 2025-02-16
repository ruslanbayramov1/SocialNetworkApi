namespace Zust.BL.DTOs.Notifications;

public class CommentLikeNotification
{
    public Guid UserId { get; set; }
    public Guid CommentedUserId { get; set; }
    public Guid CommentId { get; set; }
}
