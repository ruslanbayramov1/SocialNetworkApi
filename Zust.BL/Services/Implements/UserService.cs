using Zust.BL.DTOs.Users;
using Zust.BL.Services.Interfaces;

namespace Zust.BL.Services.Implements;

public class UserService : IUserService
{
    public Task<UserGetDto> GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }
}
