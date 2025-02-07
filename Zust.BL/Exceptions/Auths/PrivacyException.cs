using Microsoft.AspNetCore.Http;
using Zust.BL.Exceptions.Common;

namespace Zust.BL.Exceptions.Auths;

internal class PrivacyException : Exception, IBaseException
{ 
    public int StatusCode => StatusCodes.Status400BadRequest;
    public string ErrorMessage { get; }

    public PrivacyException()
    {
        ErrorMessage = $"Privacy policy must be confirmed.";
    }

    public PrivacyException(string message)
    {
        ErrorMessage = message;
    }
}
