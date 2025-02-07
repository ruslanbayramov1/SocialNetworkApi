using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class PostCommentLike : BaseEntity
{
    public PostComment PostComment { get; set; } // liking which comment (referance)
    public Guid PostCommentId { get; set; }

    public User LikedUser { get; set; } // who likes the comment
    public Guid LikedUserId { get; set; }
}
