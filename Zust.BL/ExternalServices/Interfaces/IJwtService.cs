using Zust.Core.Entities;

namespace Zust.BL.ExternalServices.Interfaces;

public interface IJwtService
{
    string CreateToken(User user, int hours = 36);
}
