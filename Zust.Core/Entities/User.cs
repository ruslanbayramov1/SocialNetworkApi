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
    public int Role = (int)Roles.Member;
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

    public ICollection<Post> Posts { get; set; } = new List<Post>(); // users himself Posts
    public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>(); // users commented posts (comments on someones post)
    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>(); // users liked posts (likes on someones post)
    public ICollection<PostCommentLike> PostCommentLikes { get; set; } = new List<PostCommentLike>(); // users liked comments (likes on someones comment)
}
