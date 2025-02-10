using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.Posts;

namespace Zust.BL.Services.Interfaces;

public interface IPostService
{
    Task<List<PostGetDto>> GetUserPostAsync(Guid userId);
    Task<PostGetDto> GetPostByIdAsync(Guid postId);
    Task CreatePostAsync(PostCreateDto dto);
    Task CreateCommentAsync(PostCommentCreateDto dto);
}
