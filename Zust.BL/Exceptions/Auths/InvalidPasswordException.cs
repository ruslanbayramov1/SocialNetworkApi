using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

public class InvalidPasswordException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public InvalidPasswordException()
    {
        ErrorMessage = "The passoword is invalid.";
    }

    public InvalidPasswordException(string message)
    {
        ErrorMessage = message;
    }
}
