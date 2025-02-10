using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

public class AccessDeniedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status403Forbidden;
    public string ErrorMessage { get; }

    public AccessDeniedException()
    {
        ErrorMessage = "Access denied.";
    }

    public AccessDeniedException(string message)
    {
        ErrorMessage = message;
    }
}
