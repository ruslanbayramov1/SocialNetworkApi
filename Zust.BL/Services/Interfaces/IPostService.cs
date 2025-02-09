using Zust.BL.DTOs.Posts;

namespace Zust.BL.Services.Interfaces;

public interface IPostService
{
    Task CreatePostAsync(PostCreateDto vm);
}
