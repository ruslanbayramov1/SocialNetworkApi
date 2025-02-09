using Microsoft.EntityFrameworkCore;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<int> GetUserLikes(Guid userId)
    {
        var posts = await Table.Where(x => x.PostedUserId == userId).ToListAsync();
        int count = posts.Sum(x => x.Likes.Count);
        return count;
    }
}
