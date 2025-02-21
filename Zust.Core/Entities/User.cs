using Zust.Core.Entities.Common;
using Zust.Core.Enums;

namespace Zust.Core.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Role { get; set; } = (int)Roles.Member;
    public string? BackupEmail { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Occupation? Occupation { get; set; }
    public Guid? OccupationId { get; set; }
    public Gender? Gender { get; set; }
    public Guid? GenderId { get; set; }
    public RelationStatus? RelationStatus { get; set; }
    public Guid? RelationStatusId { get; set; }
    public BloodGroup? BloodGroup { get; set; }
    public Guid? BloodGroupId { get; set; }
    public string? Website { get; set; }
    public Language? Language { get; set; }
    public Guid? LanguageId { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; } = "https://finalprojectolympus.blob.core.windows.net/images/defaultuserimage.jpg";
    public string? CoverImageUrl { get; set; } = "https://finalprojectolympus.blob.core.windows.net/images/defaultusercover.jpg";
    public bool IsEmailConfirmed { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsBanned { get; set; }

    /// <summary>
    /// The collection of posts created by the user.
    /// </summary>
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    /// <summary>
    /// The collection of comments made by the user on various posts.
    /// </summary>
    public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    /// <summary>
    /// The collection of posts that the user has liked.
    /// </summary>
    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    /// <summary>
    /// The collection of comments that the user has liked.
    /// </summary>
    public ICollection<PostCommentLike> PostCommentLikes { get; set; } = new List<PostCommentLike>();

    /// <summary>
    /// The collection of users who follow this user.
    /// </summary>
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();

    /// <summary>
    /// The collection of users who the user follows.
    /// </summary>
    public ICollection<Follow> Followings { get; set; } = new List<Follow>();

    /// <summary>
    /// The collection of posts created by user.
    /// </summary>
    public ICollection<Story> Stories { get; set; } = new List<Story>();

    /// <summary>
    /// The collection of likes made by user to various stories.
    /// </summary>
    public ICollection<StoryLike> StoryLikes { get; set; } = new List<StoryLike>();

    /// <summary>
    /// The collection of views made by user to various stories.
    /// </summary>
    public ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();
}
