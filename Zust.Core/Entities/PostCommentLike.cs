using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

/// <summary>
/// Represents a like on a post comment, including the user who liked it.
/// </summary>
public class PostCommentLike : BaseEntity
{
    /// <summary>
    /// The comment that is being liked.
    /// </summary>
    public PostComment? PostComment { get; set; }
    /// <summary>
    /// The ID of the comment that is being liked.
    /// </summary>
    public Guid? PostCommentId { get; set; }

    /// <summary>
    /// The user who liked the comment.
    /// </summary>
    public User? LikedUser { get; set; }
    /// <summary>
    /// The ID of the user who liked the comment.
    /// </summary>
    public Guid? LikedUserId { get; set; }
}
