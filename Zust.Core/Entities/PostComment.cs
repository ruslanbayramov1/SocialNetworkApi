using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

/// <summary>
/// Represents a comment on a post, including replies, likes, and the user who commented.
/// </summary>
public class PostComment : BaseEntity
{
    public string Content { get; set; } = null!;

    /// <summary>
    /// The post that the comment belongs to.
    /// </summary>
    public Post Post { get; set; }
    /// <summary>
    /// The ID of the post that the comment belongs to.
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Self join and the ID of the parent comment if this is a reply.
    /// If null, this comment is a top-level comment.
    /// </summary>
    public Guid? ParentCommentId { get; set; }
    /// <summary>
    /// Navigation property for the parent comment in case this is a reply.
    /// The case is parent comment Id is not null.
    /// </summary>
    public PostComment? ParentComment { get; set; }
    /// <summary>
    /// The collection of replies to this comment.
    /// </summary>
    public ICollection<PostComment> Replies { get; set; } = new List<PostComment>();

    /// <summary>
    /// The user who wrote the comment.
    /// </summary>
    public User CommentedUser { get; set; }
    /// <summary>
    /// The ID of the user who wrote the comment.
    /// </summary>
    public Guid CommentedUserId { get; set; }

    /// <summary>
    /// The collection of likes received on the comment.
    /// </summary>
    public ICollection<PostCommentLike> Likes { get; set; } = new List<PostCommentLike>(); // likes on the comment
}
