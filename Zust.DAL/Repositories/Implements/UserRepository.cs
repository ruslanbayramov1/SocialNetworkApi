using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly IPostRepository _postRepo;
    public UserRepository(AppDbContext context, IPostRepository postRepo) : base(context)
    {
        _postRepo = postRepo;
    }
    public async Task<int> GetUserLikes(Guid userId)
    { 
        int postLike = await _postRepo.GetUserLikes(userId);
        return postLike;
    }
}
