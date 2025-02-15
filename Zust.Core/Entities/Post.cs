using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

/// <summary>
/// Represents a post created by a user, including likes and comments.
/// </summary>
public class Post : BaseEntity
{
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The user who created the post.
    /// </summary>
    public User PostedUser { get; set; }
    /// <summary>
    /// The ID of the user who created the post.
    /// </summary>
    public Guid PostedUserId { get; set; }

    /// <summary>
    /// The collection of likes received on the post.
    /// </summary>
    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    /// <summary>
    /// The collection of comments made on the post.
    /// </summary>
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
}
