using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<UserProfileGetDto> GetUserProfileById(Guid id)
    {
        bool res = await _userRepo.IsExistsAsync(id);
        if (!res) throw new NotFoundException<User>();

        UserProfileGetDto? userProfile = await _userRepo.GetByIdAsync(id,x => new UserProfileGetDto {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            CoverImageUrl = x.CoverImageUrl!,
            ProfileImageUrl = x.ProfileImageUrl!,
            Role = ((Roles)x.Role).ToString(),
            UserName = x.UserName,
            FollowerCount = x.Followers.Count(),
            FollowingCount = x.Followings.Count(),
            LikeCount = x.Posts.SelectMany(x => x.Likes).Count()
        });

        return userProfile;
    }

    public Task UpdateProfile()
    {
        throw new NotImplementedException();
    }
}
