using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.PostComments;

public class PostCommentGetDto
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public Guid? ParentCommentId { get; set; }
    public List<PostCommentLikeGetDto>? PostCommentLikes { get; set; } = new();
    public List<PostCommentGetDto>? Replies { get; set; } = new();
}
