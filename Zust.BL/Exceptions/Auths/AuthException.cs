using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

public class AuthException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status401Unauthorized;
    public string ErrorMessage { get; }

    public AuthException()
    {
        ErrorMessage = "Unauthorized.";
    }

    public AuthException(string message)
    {
        ErrorMessage = message;
    }
}
