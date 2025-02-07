using Zust.BL.DTOs.Users;

namespace Zust.BL.Services.Interfaces;

public interface IUserService
{
    Task<UserGetDto> GetUserById(Guid id);
}
