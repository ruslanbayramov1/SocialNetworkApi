using Zust.BL.DTOs.Users;

namespace Zust.BL.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Gets general profile stats about user
    /// </summary>
    Task<UserProfileGetDto> GetUserProfileById(Guid id);
}
