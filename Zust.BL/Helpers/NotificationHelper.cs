using Zust.BL.Constants;
using Zust.BL.DTOs.Notifications;
using Zust.Core.Enums;
using Zust.Core.MongoEntities;

namespace Zust.BL.Helpers;

public class NotificationHelper
{
    public static string GenerateNotificationMessage(string senderUserName, NotificationTypes? type ,NotificationActions action)
    {
        string message = action switch
        {
            NotificationActions.Comment => $"{senderUserName} commented on your",
            NotificationActions.Like => $"{senderUserName} liked your",
            NotificationActions.Reply => $"{senderUserName} replied to your",
            _ => $"{senderUserName} sended new"
        };

        string fullMessage = type switch
        {
            NotificationTypes.Comment => $"{message} {type?.ToString().ToLower()}.",
            NotificationTypes.Post => $"{message} {type?.ToString().ToLower()}.",
            NotificationTypes.Story => $"{message} {type?.ToString().ToLower()}.",
            NotificationTypes.Friendship => $"{message} {type?.ToString().ToLower()} request.",
            _ => " request."
        };

        return fullMessage;
    }

    public static string GenerateRelatedLink(string baseUrl, NotificationTypes? type)
    {
        string message = type switch
        {
            NotificationTypes.Comment => $"{baseUrl}/{EndpointConstant.CommentGet}",
            NotificationTypes.Post => $"{baseUrl}/{EndpointConstant.PostGet}",
            NotificationTypes.Story => $"{baseUrl}/{EndpointConstant.StoryGet}",
            NotificationTypes.Friendship => $"{baseUrl}/{EndpointConstant.UserProfileGet}",
            _ => baseUrl
        };

        return message;
    }

    public static List<NotificationGetDto> GenerateNotifications(string baseUrl, List<Notification> notifications)
    {
        var notificationData = notifications.Select(x => new NotificationGetDto
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            RelatedLink = $"{NotificationHelper.GenerateRelatedLink(baseUrl, x.Type)}/{x.RelatedEntityId}",
            UserAccountLink = $"{baseUrl}/{EndpointConstant.UserAccountGet}/{x.SenderName}",
            Message = NotificationHelper.GenerateNotificationMessage(x.SenderName, x.Type, x.Action),
        }).ToList();

        return notificationData;
    }
}
