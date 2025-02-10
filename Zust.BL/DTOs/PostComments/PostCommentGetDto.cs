using Zust.BL.DTOs.PostCommentLikes;

namespace Zust.BL.DTOs.PostComments;

public class PostCommentGetDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string CommentedUserName { get; set; }
    public Guid? ParentCommentId { get; set; }
    public List<PostCommentLikeGetDto>? PostCommentLikes { get; set; } = new();
    public List<PostCommentGetDto>? Replies { get; set; } = new();
}
