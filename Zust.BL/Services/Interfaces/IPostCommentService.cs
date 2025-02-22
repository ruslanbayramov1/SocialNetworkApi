using Zust.BL.DTOs.PostComments;
using Zust.BL.Responses.Posts;

namespace Zust.BL.Services.Interfaces;

public interface IPostCommentService
{
    Task<CommentCreateResponse> CreateCommentAsync(PostCommentCreateDto dto);
    Task<PostCommentGetDto> GetCommentAsync(Guid commentId);
    Task<List<PostCommentGetDto>> GetCommentsAsync(Guid postId);
    Task<List<PostCommentGetDto>> GetRepliesAsync(Guid commentId);
    Task DeleteAsync(Guid commentId);
}
