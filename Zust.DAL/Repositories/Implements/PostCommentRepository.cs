using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class PostCommentRepository : GenericRepository<PostComment>, IPostCommentRepository
{
    public PostCommentRepository(AppDbContext context) : base(context)
    {
    }
}
