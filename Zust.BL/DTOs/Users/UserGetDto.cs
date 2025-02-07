using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.Core.Entities;
using Zust.Core.Enums;

namespace Zust.BL.DTOs.Users;

public class UserGetDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
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

    public ICollection<PostGetDto> Posts { get; set; } = new List<PostGetDto>(); // users himself Posts
    public ICollection<PostCommentGetDto> PostComments { get; set; } = new List<PostCommentGetDto>(); // users commented posts (comments on someones post)
    public ICollection<PostLikeGetDto> PostLikes { get; set; } = new List<PostLikeGetDto>(); // users liked posts (likes on someones post)
    public ICollection<PostCommentLikeGetDto> PostCommentLikes { get; set; } = new List<PostCommentLikeGetDto>(); // users liked comments (likes on someones comment)
}
