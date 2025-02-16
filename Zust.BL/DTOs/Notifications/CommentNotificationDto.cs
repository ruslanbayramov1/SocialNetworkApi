namespace Zust.BL.DTOs.Notifications;

public class CommentNotificationDto
{
    public Guid? ParentCommentId { get; set; }
    public Guid PostedUserId { get; set; }
    public Guid UserId { get; set; }
    public Guid CommentedUserId { get; set; }
    public Guid CommentId { get; set; } 
}
