using Microsoft.AspNetCore.Http;
using Zust.BL.Enums;
using Zust.BL.ExternalServices.Interfaces;

namespace Zust.BL.ExternalServices.Implements;

public class UserClaimService : IUserClaimService
{
    private readonly HttpContext _httpContext;
    public UserClaimService(IHttpContextAccessor http)
    {
        _httpContext = http.HttpContext!;
    }

    public string GetUserName()
        => _httpContext.User.FindFirst(nameof(UserClaimTypes.UserName))?.Value!;

    public string GetRole()
        => _httpContext.User.FindFirst(nameof(UserClaimTypes.Role))?.Value!;

    public string GetEmail()
        => _httpContext.User.FindFirst(nameof(UserClaimTypes.Email))?.Value!;

    public Guid GetId()
        => Guid.Parse(_httpContext.User.FindFirst(nameof(UserClaimTypes.Id))?.Value!);

    public string GetFirstName()
        => _httpContext.User.FindFirst(nameof(UserClaimTypes.FirstName))?.Value!;

    public string GetLastName()
    => _httpContext.User.FindFirst(nameof(UserClaimTypes.LastName))?.Value!;

    public string GetFullName()
    => GetFirstName() + " " + GetLastName();
}
