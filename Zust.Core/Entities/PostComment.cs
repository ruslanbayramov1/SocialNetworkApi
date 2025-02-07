using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class PostComment : BaseEntity
{
    public string Content { get; set; } = null!;

    public Post Post { get; set; } // the comment on this post (referance)
    public Guid PostId { get; set; }

    public Guid? ParentCommentId { get; set; } // self join reply (if not null, then its reply)
    public PostComment? ParentComment { get; set; }
    public ICollection<PostComment> Replies { get; set; } = new List<PostComment>();

    public User CommentedUser { get; set; } // the user who comments to post
    public Guid CommentedUserId { get; set; }

    public ICollection<PostCommentLike> Likes { get; set; } = new List<PostCommentLike>(); // likes on the comment
}
