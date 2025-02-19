using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.Responses.Posts;

namespace Zust.BL.Services.Interfaces;

public interface ICommentLikeService
{
    Task<List<PostCommentLikeGetDto>> GetCommentLikes(Guid commentId);
    Task<CommentLikeCreateResponse> CreateCommentLikeAsync(PostCommentLikeCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<Guid?> IsLikedBefore(PostCommentLikeCreateDto dto);
}
