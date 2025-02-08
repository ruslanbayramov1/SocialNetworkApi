using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

internal class EmailConfirmedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public EmailConfirmedException()
    {
        ErrorMessage = "Email already confirmed.";
    }

    public EmailConfirmedException(string message)
    {
        ErrorMessage = message;
    }
}
