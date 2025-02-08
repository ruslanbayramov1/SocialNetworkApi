using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Files;

public class InvalidFileSizeException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public InvalidFileSizeException()
    {
        ErrorMessage = $"The file size is invalid.";
    }

    public InvalidFileSizeException(string message)
    {
        ErrorMessage = message;
    }
}
