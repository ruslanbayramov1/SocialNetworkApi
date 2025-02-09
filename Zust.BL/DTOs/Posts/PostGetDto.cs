using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.Users;

namespace Zust.BL.DTOs.Posts;

public class PostGetDto
{
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }

    public UserProfileGetDto PostedUser { get; set; }

    public int LikeCount { get; set; }
    public ICollection<PostCommentGetDto> Comments { get; set; }
}
