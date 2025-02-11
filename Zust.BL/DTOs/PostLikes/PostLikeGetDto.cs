using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.PostLikes;

public class PostLikeGetDto
{
    /// <summary>
    /// The unique identifier of the post being liked.
    /// </summary>
    public Guid PostId { get; set; }
    /// <summary>
    /// The user liking the post.
    /// </summary>
    public UserLikeGetDto LikedUser { get; set; }
}
