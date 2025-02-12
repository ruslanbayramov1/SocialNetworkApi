using Zust.BL.DTOs.PostCommentLikes;

namespace Zust.BL.Services.Interfaces;

public interface ICommentLikeService
{
    Task<List<PostCommentLikeGetDto>> GetCommentLikes(Guid commentId);
    Task CreateCommentLikeAsync(Guid commentId);
}
