using Zust.BL.DTOs.Notifications;

namespace Zust.BL.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationGetDto>> GetUserNotifications();
    Task CratePostLikeNotification(PostLikeNotificationCreateDto dto);
    Task CreateCommentNotification(CommentNotificationCreateDto dto);
    Task CrateCommentLikeNotification(CommentLikeNotificationCreateDto dto);
    Task CreatePostNotificationForAllFollowers(PostNotificationCreateDto dto);
    Task CreateFriendRequestNotification(FriendRequestNotificationCreateDto dto);
    //Task DeleteNotifications<T>(List<FilterDefinition<T>> filters);
}
