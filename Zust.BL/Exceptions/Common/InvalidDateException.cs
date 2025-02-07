using Microsoft.AspNetCore.Http;

namespace Zust.BL.Exceptions.Common;

public class InvalidDateException<T> : Exception, IBaseException where T : class, new()
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public InvalidDateException()
    {
        ErrorMessage = $"This is not valid date.";
    }

    public InvalidDateException(string message)
    {
        ErrorMessage = message;
    }
}

public class InvalidDateException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public InvalidDateException(string message)
    {
        ErrorMessage = message;
    }
}
