using AutoMapper;
using BCrypt.Net;
using Zust.BL.DTOs.Auths;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Auths;
using Zust.BL.Exceptions.Common;
using Zust.BL.Extensions;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IUserClaimService _userClaimService;
    private readonly ICacheService _cacheService;
    public AuthService(IUserRepository repo, IMapper mapper, IJwtService jwtService, IEmailService emailService, IUserClaimService userClaimService, ICacheService cacheService)
    {
        _repo = repo;
        _mapper = mapper;
        _jwtService = jwtService;
        _emailService = emailService;
        _userClaimService = userClaimService;
        _cacheService = cacheService;
    }

    public async Task<Guid> RegisterAsync(RegisterDto dto)
    {
        bool resUsername = await _repo.IsExistsAsync(x => x.UserName == dto.UserName);
        bool resEmail = await _repo.IsExistsAsync(x => x.Email == dto.Email);

        if (!dto.ConfirmPrivacyPolicy)
            throw new PrivacyException();

        if (resEmail || resUsername)
            throw new ExistsException<User>();

        var entity = _mapper.Map<User>(dto);
        entity.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
        if (await _repo.GetAllCountAsync() == 0)
        { 
            entity.Role = (int)Roles.Admin;
        }
        else
        {
            entity.Role = (int)Roles.Member;
        }

        await _repo.AddAsync(entity);
        await _repo.SaveAsync();

        return entity.Id;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        User? user = null;
        if (dto.UserNameOrEmail.Contains('@'))
        {
            user = await _repo.GetByExpressionAsync(x => x.Email == dto.UserNameOrEmail);
        }
        else
        {
            user = await _repo.GetByExpressionAsync(x => x.UserName == dto.UserNameOrEmail);
        }

        if (user == null) throw new NotFoundException<User>("Username or password is wrong");

        bool res = BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, user.PasswordHash);
        if (!res) throw new NotFoundException<User>("Username or password is wrong");

        string token = _jwtService.CreateToken(user, 36);

        return token;
    }

    public async Task<string> SendEmailConfirmationAsync()
    {
        var user = await _repo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        if (user.IsEmailConfirmed)
            throw new EmailConfirmedException();

        string code = CodeHelper.GenerateCode();
        string res = "";
        int exp = 300;
        string userEmail = user.BackupEmail ?? user.Email;

        bool exists = await _cacheService.IsExists<string>(user.UserName);
        if (exists)
            throw new ExistsException("Confirmation code");

        await _cacheService.Set(user.UserName, code, exp);

        await _emailService.SendCodeToEmailAsync(user.UserName, code, userEmail, EmailTypes.Confirmation);
        res = $"Code successfully sended to your email {userEmail.HideEmailInfo()}, and will exipre after {exp / 60} minutes.";

        return res;
    }

    public async Task<string> SendNewPasswordEmailAsync(string oldCode)
    {
        var user = await _repo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        var passwordRes = BCrypt.Net.BCrypt.EnhancedVerify(oldCode, user.PasswordHash);
        if (!passwordRes)
            throw new InvalidPasswordException();

        string code = CodeHelper.GenerateCode();
        string res = "";
        int exp = 300;
        string userEmail = user.BackupEmail ?? user.Email;

        bool exists = await _cacheService.IsExists<string>(user.UserName);
        if (exists)
            throw new ExistsException("Code");  

        await _cacheService.Set(user.UserName, code, exp);

        await _emailService.SendCodeToEmailAsync(user.UserName, code, userEmail, EmailTypes.NewPassword);
        res = $"Code successfully sended to your email {userEmail.HideEmailInfo()}, and will exipre after {exp / 60} minutes.";

        return res;
    }

    public async Task VerifyEmail(string code)
    {
        var user = await _repo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        await VerifyCode(user, code);

        user.IsEmailConfirmed = true;
        _repo.Update(user);
        await _repo.SaveAsync();
        _cacheService.Delete(user.UserName);
    }

    public async Task SetNewPassword(string code, NewPasswordDto dto)
    {
        var user = await _repo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        await VerifyCode(user, code);

        var newPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password);
        user.PasswordHash = newPassword;

        _repo.Update(user);
        await _repo.SaveAsync();
        _cacheService.Delete(user.UserName);
    }

    public async Task VerifyCode(User user, string code)
    {
        string? cacheCode = await _cacheService.Get<string>(user.UserName);
        if (String.IsNullOrEmpty(cacheCode) || String.IsNullOrWhiteSpace(cacheCode))
            throw new NotFoundException("Code");

        if (cacheCode != code)
            throw new InvalidCodeException();
    }
}
