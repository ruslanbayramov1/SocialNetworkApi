using Zust.BL.Enums;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Implements;

public class NotificationService : INotificationService
{
    private readonly IMongoDbService _mongoDbService;
    public NotificationService(IMongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }
    public async Task CratePostLikeNotification(User user, Post post, bool isAlreadyLiked)
    {
        // if post is already liked, or liking user is post owner himself, dont do anything
        if (isAlreadyLiked || post.PostedUser.Id == user.Id)
            return;

        Notification notification = new Notification
        {
            SenderId = user.Id,
            ReceiverId = post.PostedUser.Id,
            RelatedEntityId = post.Id.ToString(),
            Type = NotificationTypes.Post,
            Action = NotificationActions.Like
        };
        await _mongoDbService.InsertToCollectionAsync(notification, MongoCollections.Notifications);
    }

    public async Task CreateCommentNotification(User user, PostComment comment, Post post, PostComment parentComment)
    {
        // notification in MongoDB
        List<Notification> notifications = new();
        // 1. Notify the post owner (if the post owner is not the commenter)
        if (comment.ParentCommentId == null && post.PostedUserId != user.Id)
        {
            notifications.Add(new Notification
            {
                SenderId = user.Id,
                ReceiverId = post.PostedUserId,
                RelatedEntityId = comment.Id.ToString(),
                Type = NotificationTypes.Post,
                Action = NotificationActions.Comment,
            });
        }

        // 2. If it is a reply
        if (comment.ParentCommentId != null)
        {
            // 2.1. In every case, notify who somebody replying to his comment
            notifications.Add(new Notification
            {
                SenderId = user.Id,
                ReceiverId = parentComment.CommentedUserId,
                RelatedEntityId = comment.Id.ToString(),
                Type = NotificationTypes.Comment,
                Action = NotificationActions.Reply,
            });

            // 2.2. If replied comment is not by post owner, also notify him
            if (post.PostedUserId != user.Id)
            {
                notifications.Add(new Notification
                {
                    SenderId = user.Id,
                    ReceiverId = post.PostedUserId,
                    RelatedEntityId = comment.Id.ToString(),
                    Type = NotificationTypes.Post,
                    Action = NotificationActions.Comment,
                });
            }
        }

        if (notifications.Count > 0)
            await _mongoDbService.InsertManyToCollectionAsync(notifications, MongoCollections.Notifications);
    }

    public async Task CrateCommentLikeNotification(User user, PostComment postComment, bool isAlreadyLiked)
    {
        // if comment is already liked, or liking user is commentors himself, dont do anything
        if (isAlreadyLiked || postComment.CommentedUserId == user.Id)
            return;

        var notification = new Notification
        {
            ReceiverId = postComment.CommentedUserId,
            SenderId = user.Id,
            RelatedEntityId = postComment.Id.ToString(),
            Action = NotificationActions.Like,
            Type = NotificationTypes.Comment,
        };

        await _mongoDbService.InsertToCollectionAsync(notification, MongoCollections.Notifications);
    }
}
