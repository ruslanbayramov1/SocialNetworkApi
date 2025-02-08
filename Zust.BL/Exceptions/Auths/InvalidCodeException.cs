using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

internal class InvalidCodeException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public InvalidCodeException()
    {
        ErrorMessage = "The code is invalid.";
    }

    public InvalidCodeException(string message)
    {
        ErrorMessage = message;
    }
}
