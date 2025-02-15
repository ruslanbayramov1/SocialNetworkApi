using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface INotificationService
{
    Task CratePostLikeNotification(User user, Post post);
    Task CreateCommentNotification(User user, PostComment comment, Post post, PostComment parentComment);
    Task CrateCommentLikeNotification(User user, PostComment postComment);
}
