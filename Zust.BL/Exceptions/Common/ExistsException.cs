using Microsoft.AspNetCore.Http;

namespace Zust.BL.Exceptions.Common;

public class ExistsException<T> : Exception, IBaseException where T : class, new()
{
    public int StatusCode => StatusCodes.Status409Conflict;

    public string ErrorMessage { get; }

    public ExistsException()
    {
        ErrorMessage = $"{typeof(T).Name.ToLower()} already exists.";
    }

    public ExistsException(string message)
    {
        ErrorMessage = message;
    }
}

public class ExistsException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status409Conflict;

    public string ErrorMessage { get; }

    public ExistsException(string key)
    {
        ErrorMessage = $"{key} already exists.";
    }
}

