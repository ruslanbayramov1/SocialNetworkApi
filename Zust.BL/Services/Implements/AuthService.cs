using AutoMapper;
using Zust.BL.DTOs.Auths;
using Zust.BL.Exceptions.Auths;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
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
    public AuthService(IUserRepository repo, IMapper mapper, IJwtService jwtService)
    {
        _repo = repo;
        _mapper = mapper;
        _jwtService = jwtService;
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
        entity.Role = (int)Roles.Member;

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
}
