namespace Zust.BL.DTOs.Notifications;

public class CommentNotificationCreateDto
{
    public Guid? ParentCommentId { get; set; }
    public Guid PostedUserId { get; set; }
    public Guid SenderUserId { get; set; }
    public string SenderUserName { get; set; }
    public Guid? CommentedUserId { get; set; }
    public Guid CommentId { get; set; } 
}
