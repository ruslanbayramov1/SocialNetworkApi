using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.Core.Entities;

namespace Zust.BL.DTOs.Users;

public class UserGetDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public string? BackupEmail { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string? Gender { get; set; }
    public string? RelationStatus { get; set; }
    public string? BloodGroup { get; set; }
    public string? Website { get; set; }
    public string? Language { get; set; }
    public string? Address { get; set; }
    public string ProfileImageUrl { get; set; }
    public string CoverImageUrl { get; set; }
    public bool IsEmailConfirmed { get; set; } = false;

    public ICollection<PostGetDto> Posts { get; set; } = new List<PostGetDto>(); // users himself Posts
    public ICollection<PostCommentGetDto> PostComments { get; set; } = new List<PostCommentGetDto>(); // users commented posts (comments on someones post)
    public ICollection<PostLikeGetDto> PostLikes { get; set; } = new List<PostLikeGetDto>(); // users liked posts (likes on someones post)
    public ICollection<PostCommentLikeGetDto> PostCommentLikes { get; set; } = new List<PostCommentLikeGetDto>(); // users liked comments (likes on someones comment)
}
