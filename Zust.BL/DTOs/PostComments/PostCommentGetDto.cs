using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.PostComments;

public class PostCommentGetDto
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string Content { get; set; }
    public Guid? ParentCommentId { get; set; }
    public UserCommentGetDto CommentedUser { get; set; }
    public int LikeCount { get; set; }
    public int ReplyCount { get; set; }
}
