using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Auths;
using Zust.Core.Enums;

namespace Zust.BL.Attributes;

/// <summary>
/// Authentication attribute for protecting endpoints
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthAttribute : Attribute, IAsyncActionFilter
{
    private int access = 0;
    public AuthAttribute()
    {
        
    }
    public AuthAttribute(Roles role)
    {
        access = (int)role;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<NoAuthAttribute>() != null)
        { 
            await next();
            return;
        }

        var value = context.HttpContext.User.FindFirst(nameof(UserClaimTypes.Role))?.Value;
        if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            throw new AuthException();

        int role = Convert.ToInt32(value);

        if (access != 0 && (role & access) == 0)
            throw new AccessDeniedException();

        await next();
    }
}
