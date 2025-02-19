using Zust.BL.DTOs.Posts;
using Zust.BL.Responses.Posts;
using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface IPostService
{
    Task<List<PostGetDto>> GetUserPostsAsync(Guid userId);
    Task<PostGetDto> GetPostByIdAsync(Guid postId);
    Task<PostCreateResponse> CreatePostAsync(PostCreateDto dto);

    //helpers
    Task<Post> GetPostModelByIdAsync(Guid postId);
}
