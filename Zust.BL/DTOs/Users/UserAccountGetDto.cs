namespace Zust.BL.DTOs.Users;

public class UserAccountGetDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public int LikeCount { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string? Gender { get; set; }
    public string? RelationStatus { get; set; }
    public string? BloodGroup { get; set; }
    public string? Language { get; set; }
    public string? Website { get; set; }
    public bool IsPrivate { get; set; }
}
