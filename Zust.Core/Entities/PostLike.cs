using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class PostLike : BaseEntity
{
    /// <summary>
    /// The post that is being liked.
    /// </summary>
    public Post Post { get; set; }
    /// <summary>
    /// The ID of the post that is being liked.
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// The user who liked the post.
    /// </summary>
    public User LikedUser { get; set; }
    /// <summary>
    /// The ID of the user who liked the post.
    /// </summary>
    public Guid LikedUserId { get; set; }
}
