using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class StoryRepository : GenericRepository<Story>, IStoryRepository
{
    public StoryRepository(AppDbContext context) : base(context)
    {
    }
}
