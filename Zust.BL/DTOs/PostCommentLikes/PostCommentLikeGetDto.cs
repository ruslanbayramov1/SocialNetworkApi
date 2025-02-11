using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.PostCommentLikes;

public class PostCommentLikeGetDto
{
    /// <summary>
    /// The unique identifier of the comment being liked.
    /// </summary>
    public Guid CommentId { get; set; }
    /// <summary>
    /// The user liking the comment.
    /// </summary>
    public UserLikeGetDto LikedUser { get; set; }
}
