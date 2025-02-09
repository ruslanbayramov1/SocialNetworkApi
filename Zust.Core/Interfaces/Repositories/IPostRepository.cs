using Zust.Core.Entities;

namespace Zust.Core.Interfaces.Repositories;

public interface IPostRepository : IGenericRepository<Post>
{
    /// <summary>
    /// Gets how many likes user have on posts.
    /// </summary>
    Task<int> GetUserLikes(Guid userId);
}
