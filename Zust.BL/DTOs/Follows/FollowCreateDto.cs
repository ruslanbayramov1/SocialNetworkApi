namespace Zust.BL.DTOs.Follows;

public class FollowCreateDto
{
    /// <summary>
    /// ID of the user who is being followed.
    /// </summary>
    public Guid FollowingId { get; set; }
}
