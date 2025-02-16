namespace Zust.BL.DTOs.Notifications;

public class NotificationGetDto
{
    public string RelatedLink { get; set; }
    public string UserProfileLink { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
