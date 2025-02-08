using Zust.BL.DTOs.Auths;
using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task<string> SendEmailConfirmationAsync();
    Task<string> SendNewPasswordEmailAsync(string oldCode);
    Task VerifyCode(User user, string code);
    Task VerifyEmail(string code);
    Task SetNewPassword(string code, NewPasswordDto dto);
}
