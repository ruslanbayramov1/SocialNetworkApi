using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class FollowRepository : GenericRepository<Follow>, IFollowRepository
{
    public FollowRepository(AppDbContext context) : base(context)
    {
    }
}
