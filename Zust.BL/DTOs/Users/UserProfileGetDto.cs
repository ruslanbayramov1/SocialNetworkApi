namespace Zust.BL.DTOs.Users;

public class UserProfileGetDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public int LikeCount { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public string ProfileImageUrl { get; set; }
    public string CoverImageUrl { get; set; }
}
