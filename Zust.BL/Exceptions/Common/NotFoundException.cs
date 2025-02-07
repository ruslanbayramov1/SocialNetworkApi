using Microsoft.AspNetCore.Http;

namespace Zust.BL.Exceptions.Common;

public class NotFoundException<T> : Exception, IBaseException where T : class, new()
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public NotFoundException()
    {
        ErrorMessage = $"{typeof(T).Name} not found.";
    }

    public NotFoundException(string message)
    {
        ErrorMessage = message;
    }
}

public class NotFoundException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status404NotFound;

    public string ErrorMessage { get; }

    public NotFoundException(string key)
    {
        ErrorMessage = $"{key} not found.";
    }
}
