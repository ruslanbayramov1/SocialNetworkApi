using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zust.BL.DTOs.Notifications;
using Zust.BL.Enums;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Options;
using Zust.BL.Services.Interfaces;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Implements;

public class NotificationService : INotificationService
{
    private readonly IMongoDbService _mongoDbService;
    private readonly IUserClaimService _userClaimService;
    private readonly IFollowRepository _followRepo;
    private readonly ApiOption _opt;
    public NotificationService(IMongoDbService mongoDbService, IUserClaimService userClaimService, IFollowRepository followRepository, IOptions<ApiOption> opt)
    {
        _mongoDbService = mongoDbService;
        _userClaimService = userClaimService;
        _followRepo = followRepository;
        _opt = opt.Value;
    }
    public async Task CratePostLikeNotification(PostLikeNotificationCreateDto dto)
    {
        // if user is post owner himself, dont do anything
        if (dto.PostedUserId == dto.SenderUserId)
            return;

        Notification notification = new Notification
        {
            SenderId = dto.SenderUserId,
            SenderName = dto.SenderUserName,
            ReceiverId = dto.PostedUserId,
            RelatedEntityId = dto.PostId.ToString(),
            Type = NotificationTypes.Post,
            Action = NotificationActions.Like
        };
        await _mongoDbService.InsertToCollectionAsync(notification, MongoCollections.Notifications);
    }

    public async Task CreateCommentNotification(CommentNotificationCreateDto dto)
    {
        // notification in MongoDB
        List<Notification> notifications = new();
        // 1. Notify the post owner (if the post owner is not the commenter)
        if (dto.ParentCommentId == null && dto.PostedUserId != dto.SenderUserId)
        {
            notifications.Add(new Notification
            {
                SenderId = dto.SenderUserId,
                SenderName = dto.SenderUserName,
                ReceiverId = dto.PostedUserId,
                RelatedEntityId = dto.CommentId.ToString(),
                Type = NotificationTypes.Post,
                Action = NotificationActions.Comment,
            });
        }

        // 2. If it is a reply
        if (dto.ParentCommentId != null)
        {
            // 2.1. In every case, notify who somebody replying to his comment
            notifications.Add(new Notification
            {
                SenderId = dto.SenderUserId,
                SenderName= dto.SenderUserName,
                ReceiverId = dto.CommentedUserId,
                RelatedEntityId = dto.CommentId.ToString(),
                Type = NotificationTypes.Comment,
                Action = NotificationActions.Reply,
            });

            // 2.2. If replied comment is not by post owner, also notify him
            if (dto.PostedUserId != dto.SenderUserId)
            {
                notifications.Add(new Notification
                {
                    SenderId = dto.SenderUserId,
                    SenderName = dto.SenderUserName,
                    ReceiverId = dto.PostedUserId,
                    RelatedEntityId = dto.CommentId.ToString(),
                    Type = NotificationTypes.Post,
                    Action = NotificationActions.Comment,
                });
            }
        }

        if (notifications.Count > 0)
            await _mongoDbService.InsertManyToCollectionAsync(notifications, MongoCollections.Notifications);
    }

    public async Task CrateCommentLikeNotification(CommentLikeNotificationCreateDto dto)
    {
        // if the user is commentors himself, dont do anything
        if (dto.CommentedUserId == dto.SenderUserId)
            return;

        var notification = new Notification
        {
            ReceiverId = dto.CommentedUserId,
            SenderId = dto.SenderUserId,
            SenderName = dto.SenderUserName,
            RelatedEntityId = dto.CommentId.ToString(),
            Action = NotificationActions.Like,
            Type = NotificationTypes.Comment,
        };

        await _mongoDbService.InsertToCollectionAsync(notification, MongoCollections.Notifications);
    }

    public async Task CreatePostNotificationForAllFollowers(PostNotificationCreateDto dto)
    {
        if (dto.FollowerCount == 0)
            return;

        var notifications = await _followRepo.GetWhereAsync(x => x.FollowingId == _userClaimService.GetId(), x => new Notification
        {
            ReceiverId = x.FollowerId,
            SenderId = x.FollowingId,
            SenderName = _userClaimService.GetUserName(),
            RelatedEntityId = dto.PostId.ToString(),
            Type = NotificationTypes.Post,
            Action = NotificationActions.Interaction,
        });

        await _mongoDbService.InsertManyToCollectionAsync(notifications, MongoCollections.Notifications);
    }

    public async Task<List<NotificationGetDto>> GetUserNotifications()
    {
        var filter = Builders<Notification>.Filter.Eq(x => x.ReceiverId, _userClaimService.GetId());
        var notifications = await _mongoDbService.GetCollectionListWhere(filter, MongoCollections.Notifications);

        var notificationData = NotificationHelper.GenerateNotifications(_opt.BaseUrl, notifications);

        return notificationData;
    }
}
