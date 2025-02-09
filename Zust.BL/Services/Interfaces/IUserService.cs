using Zust.BL.DTOs.Users;

namespace Zust.BL.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Gets all information about user
    /// </summary>
    /// Task<UserProfileGetDto> GetUserProfileById(Guid id);
    Task<UserGetDto> GetUserById(Guid id);
    /// <summary>
    /// Gets general profile stats about user
    /// </summary>
    Task<UserProfileGetDto> GetUserProfileById(Guid id);
}
