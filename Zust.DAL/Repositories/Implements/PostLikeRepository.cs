using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
{
    public PostLikeRepository(AppDbContext context) : base(context)
    {
    }
}
