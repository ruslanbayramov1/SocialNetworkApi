using Microsoft.AspNetCore.Http;
using Zust.BL.DTOs.Users;
using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Gets general profile stats about user
    /// </summary>
    Task<UserProfileGetDto> GetUserProfileById(Guid id);
    Task<List<UserProfileGetDto>> GetUserProfileByName(string userName);
    Task<UserAccountGetDto> GetUserAccountByName(string userName);
    Task UpdateProfile(UserProfileUpdateDto dto);
    Task UpdateProfileImage(IFormFile image);
    Task UpdateProfileBanner(IFormFile banner);
    Task<bool> IsPrivate(Guid ownerUserId);
    Task<bool> IsFriend(Guid ownerUserId);
    Task<bool> IsPrivate(string ownerUserName);
    Task<bool> IsFriend(string ownerUserName);
    Task<User> GetById(Guid userId);
    Task<User> GetByName(string userName);
}
