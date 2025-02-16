using Zust.BL.DTOs.Notifications;

namespace Zust.BL.Services.Interfaces;

public interface INotificationService
{
    Task CratePostLikeNotification(PostLikeNotificationDto dto);
    Task CreateCommentNotification(CommentNotificationDto dto);
    Task CrateCommentLikeNotification(CommentLikeNotification dto);
    Task CreatePostNotificationForAllFollowers(PostNotificationDto dto);
}
