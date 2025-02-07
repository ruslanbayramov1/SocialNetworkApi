using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class PostLike : BaseEntity
{
    public Post Post { get; set; } // post referance
    public Guid PostId { get; set; }

    public User LikedUser { get; set; } // the user who likes on post
    public Guid LikedUserId { get; set; }
}
