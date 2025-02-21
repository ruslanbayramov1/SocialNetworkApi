using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class StoryLikeRepository : GenericRepository<StoryLike>, IStoryLikeRepository
{
    public StoryLikeRepository(AppDbContext context) : base(context)
    {
    }
}
