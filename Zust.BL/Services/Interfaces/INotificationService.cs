using Zust.BL.DTOs.Notifications;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationGetDto>> GetUserNotifications();
    Task<Notification> GetNotificationModelByIdAsync(Guid id);
    Task CratePostLikeNotification(PostLikeNotificationCreateDto dto);
    Task CreateCommentNotification(CommentNotificationCreateDto dto);
    Task CrateCommentLikeNotification(CommentLikeNotificationCreateDto dto);
    Task CreatePostNotificationForAllFollowers(PostNotificationCreateDto dto);
    Task CreateFriendRequestNotification(FriendRequestNotificationCreateDto dto);
    Task<Guid?> IsFollowRequestExistsAsync(FriendRequestNotificationCreateDto dto);
    Task UpdateNotificationHiddenInfo(Guid notificationId);
    Task DeleteNotificationAsync(Guid notificationId);
}
