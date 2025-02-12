using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class PostCommentLikeRepository : GenericRepository<PostCommentLike>, IPostCommentLikeRepository
{
    public PostCommentLikeRepository(AppDbContext context) : base(context)
    {
    }
}
