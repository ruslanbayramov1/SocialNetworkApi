using Zust.BL.DTOs.PostComments;

namespace Zust.BL.DTOs.Posts;

public class FeedPostGetDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public string? MediaUrl { get; set; }

    public Guid PostedUserId { get; set; }

    public int LikeCount { get; set; }
    public ICollection<PostCommentGetDto> Comments { get; set; } = new List<PostCommentGetDto>();
}
