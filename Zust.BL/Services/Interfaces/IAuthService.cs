using Zust.BL.DTOs.Auths;
using Zust.BL.Enums;
using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task<string> SendEmailConfirmationAsync();
    Task<string> SendNewPasswordEmailAsync(string oldCode);
    Task<string> SendForgotPasswordEmailAsync(string userEmail);
    Task VerifyEmailAsync(string code);
    Task SetNewPasswordAsync(string code, NewPasswordDto dto);
    Task SetNewPasswordForgotAsync(string code, string userEmail, NewPasswordDto dto);
    Task VerifyCode(User user, string code);
    Task<string> SendCodeToEmail(User user, int expTime, EmailTypes emailType);
}
