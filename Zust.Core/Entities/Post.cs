using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class Post : BaseEntity
{
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }

    public User PostedUser { get; set; } // the user who posts
    public Guid PostedUserId { get; set; }

    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>(); // likes on post
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>(); // comments on post
}
