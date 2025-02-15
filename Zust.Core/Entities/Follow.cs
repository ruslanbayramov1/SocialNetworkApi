using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

/// <summary>
/// Represents a follow relationship between users.
/// This entity defines the connection where one user (Follower) follows another user (Following).
/// </summary>
public class Follow : BaseEntity
{
    /// <summary>
    /// ID of the user who is following another user.
    /// </summary>
    public Guid FollowerId { get; set; }
    /// <summary>
    /// ID of the user who is being followed.
    /// </summary>
    public Guid FollowingId { get; set; }

    /// <summary>
    /// Navigation property for the user who is following another user.
    /// </summary>
    public User FollowerUser { get; set; }
    /// <summary>
    /// Navigation property for the user who is being followed.
    /// </summary>
    public User FollowingUser { get; set; }
}
