namespace Zust.BL.Helpers;

public class NotificationHelper
{
    public static string GetPostLikeNotificationMessage(string senderUserName)
    {
        return $"{senderUserName} liked your post.";
    }

    public static string GetCommentLikeNotificationMessage(string senderUserName)
    {
        return $"{senderUserName} liked your comment.";
    }

    public static string GetCommentReplyNotificationMessage(string senderUserName)
    {
        return $"{senderUserName} replied to your comment";
    }
}
