using Zust.BL.DTOs.PostLikes;

namespace Zust.BL.Services.Interfaces;

public interface IPostLikeService
{
    Task<List<PostLikeGetDto>> GetPostLikes(Guid postId);
    Task CreatePostLikeAsync(PostLikeCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<Guid?> IsLikedBefore(PostLikeCreateDto dto);
}
