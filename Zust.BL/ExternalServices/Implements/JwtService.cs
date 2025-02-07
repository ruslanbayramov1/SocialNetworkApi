using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zust.BL.Enums;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Options;
using Zust.Core.Entities;

namespace Zust.BL.ExternalServices.Implements;

public class JwtService : IJwtService
{
    private readonly JwtOption _opt;
    public JwtService(IOptions<JwtOption> opt)
    {
        _opt = opt.Value;
    }

    public string CreateToken(User user, int hours = 36)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(nameof(UserClaimTypes.UserName), user.UserName),
            new Claim(nameof(UserClaimTypes.Id), Convert.ToString(user.Id.ToString())),
            new Claim(nameof(UserClaimTypes.Email), user.Email),
            new Claim(nameof(UserClaimTypes.Role), Convert.ToString(user.Role)),
            new Claim(nameof(UserClaimTypes.FirstName), user.FirstName),
            new Claim(nameof(UserClaimTypes.LastName), user.LastName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SecretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwt = new(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(hours),
            signingCredentials: credentials
            );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwt);

        return token;
    }
}
