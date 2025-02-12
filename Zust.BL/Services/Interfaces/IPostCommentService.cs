using Zust.BL.DTOs.PostComments;

namespace Zust.BL.Services.Interfaces;

public interface IPostCommentService
{
    Task CreateCommentAsync(PostCommentCreateDto dto);
    Task<PostCommentGetDto> GetCommentAsync(Guid commentId);
    Task<List<PostCommentGetDto>> GetCommentsAsync(Guid postId);
    Task<List<PostCommentGetDto>> GetRepliesAsync(Guid commentId);
}
