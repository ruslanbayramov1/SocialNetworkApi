using Microsoft.AspNetCore.Http;

namespace Zust.BL.Exceptions.Common;

public class LengthException<T> : Exception, IBaseException where T : class, new()
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public LengthException()
    {
        ErrorMessage = $"{typeof(T).Name.ToLower()} is validating character length limit.";
    }

    public LengthException(string message)
    {
        ErrorMessage = message;
    }
}

public class LengthException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public LengthException(string key)
    {
        ErrorMessage = $"{key} is validating character length limit.";
    }

    public LengthException(string key, int size)
    {
        ErrorMessage = $"{key} must be less than {size} characters.";
    }
}
