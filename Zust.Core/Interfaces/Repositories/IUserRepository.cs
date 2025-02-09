using Zust.Core.Entities;

namespace Zust.Core.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Gets how many likes user have on total.
    /// </summary>
    Task<int> GetUserLikes(Guid userId);
}
