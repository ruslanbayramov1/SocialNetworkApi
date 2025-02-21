using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class StoryViewRepository : GenericRepository<StoryView>, IStoryViewRepository
{
    public StoryViewRepository(AppDbContext context) : base(context)
    {
    }
}
