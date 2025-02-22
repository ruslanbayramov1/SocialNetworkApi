using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zust.BL.DTOs.Notifications;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Options;
using Zust.BL.Services.Interfaces;
using Zust.Core.Enums;
using Zust.Core.Interfaces.MongoRepositories;
using Zust.Core.Interfaces.Repositories;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Implements;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly IFollowRepository _followRepo;
    private readonly ApiOption _opt;
    public NotificationService(INotificationRepository notificationRepository, IUserClaimService userClaimService, IFollowRepository followRepository, IOptions<ApiOption> opt)
    {
        _notificationRepo = notificationRepository;
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
        await _notificationRepo.InsertToCollectionAsync(notification);
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
                ReceiverId = dto.CommentedUserId.Value,
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
            await _notificationRepo.InsertManyToCollectionAsync(notifications);
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

        await _notificationRepo.InsertToCollectionAsync(notification);
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

        await _notificationRepo.InsertManyToCollectionAsync(notifications);
    }

    public async Task<List<NotificationGetDto>> GetUserNotifications()
    {
        var filter = Builders<Notification>.Filter.Eq(x => x.ReceiverId, _userClaimService.GetId());
        var notifications = await _notificationRepo.GetCollectionListWhere(filter);

        var notificationData = NotificationHelper.GenerateNotifications(_opt.BaseUrl, notifications);

        return notificationData;
    }

    public async Task<Notification> GetNotificationModelByIdAsync(Guid id)
    {
        Notification notification = await _notificationRepo.GetOneById(id);
        return notification;
    }

    public async Task CreateFriendRequestNotification(FriendRequestNotificationCreateDto dto)
    {
        var notification = new Notification
        {
            ReceiverId = dto.FollowingId,
            SenderId = _userClaimService.GetId(),
            Action = NotificationActions.Interaction,
            Type = NotificationTypes.Friendship,
            RelatedEntityId = _userClaimService.GetId().ToString(),
            SenderName= _userClaimService.GetUserName(),
        };

        await _notificationRepo.InsertToCollectionAsync(notification);
    }

    public async Task<Guid?> IsFollowRequestExistsAsync(FriendRequestNotificationCreateDto dto)
    {

        FilterDefinition<Notification> filters = Builders<Notification>.Filter.And(
                Builders<Notification>.Filter.Eq(x => x.ReceiverId, dto.FollowingId),
                Builders<Notification>.Filter.Eq(x => x.SenderId, _userClaimService.GetId()),
                Builders<Notification>.Filter.Eq(x => x.Type, NotificationTypes.Friendship),
                Builders<Notification>.Filter.Eq(x => x.Action, NotificationActions.Interaction)
            );
        var res = await _notificationRepo.GetOneWhere(filters);

        if (res != null)
        {
            return res.Id;
        }

        return null;
    }

    public async Task DeleteNotificationAsync(Guid notificationId)
    {
        FilterDefinition<Notification> filters = Builders<Notification>.Filter.Eq(x => x.Id, notificationId);
        await _notificationRepo.DeleteOneAsync(filters);
    }

    public async Task UpdateNotificationHiddenInfo(Guid notificationId)
    {
        var notification = await GetNotificationModelByIdAsync(notificationId);
        FilterDefinition<Notification> filter = Builders<Notification>.Filter.Eq(x => x.Id, notificationId);
        UpdateDefinition<Notification> updateDefinition = Builders<Notification>.Update.Set(x => x.IsHidden, true);

        await _notificationRepo.UpdateOneAsync(filter, updateDefinition);
    }
}
