namespace Zust.BL.DTOs.Notifications;

public class NotificationGetDto
{
    public Guid Id { get; set; }
    public string? RelatedLink { get; set; }
    public string UserAccountLink { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}
