using Zust.BL.DTOs.Auths;

namespace Zust.BL.Services.Interfaces;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
}
