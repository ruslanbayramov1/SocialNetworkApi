using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.Posts;

public class PostGetDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public string? MediaUrl { get; set; }

    public UserProfileGetDto PostedUser { get; set; }

    public int LikeCount { get; set; }
    public ICollection<PostCommentGetDto> Comments { get; set; } = new List<PostCommentGetDto>();
}
