using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Files;

public class InvalidFileTypeException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public InvalidFileTypeException()
    {
        ErrorMessage = $"The file type is invalid.";
    }

    public InvalidFileTypeException(string message)
    {
        ErrorMessage = message;
    }
}
