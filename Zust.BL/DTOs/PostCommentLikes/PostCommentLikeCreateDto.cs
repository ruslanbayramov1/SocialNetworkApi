namespace Zust.BL.DTOs.PostCommentLikes;

public class PostCommentLikeCreateDto
{
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
}
