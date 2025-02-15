using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class CommentLikeService : ICommentLikeService
{
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly IPostCommentLikeRepository _postCommentLikeRepo;
    private readonly IPostCommentRepository _postCommentRepo;
    private readonly INotificationService _notificationService;
    public CommentLikeService(IUserRepository userRepository, IUserClaimService userClaimService, IPostCommentLikeRepository postCommentLikeRepository, IPostCommentRepository postCommentRepo, INotificationService notificationService)
    {
        _userRepo = userRepository;
        _userClaimService = userClaimService;
        _postCommentLikeRepo = postCommentLikeRepository;
        _postCommentRepo = postCommentRepo;
        _notificationService = notificationService;
    }

    public async Task CreateCommentLikeAsync(PostCommentLikeCreateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        PostComment? postComment = await _postCommentRepo.GetByIdAsync(dto.CommentId);
        if (postComment == null) throw new NotFoundException("Post comment");

        var commentLike = new PostCommentLike
        {
            PostCommentId = dto.CommentId,
            LikedUserId = user.Id,
        };

        await _postCommentLikeRepo.AddAsync(commentLike);
        await _postCommentLikeRepo.SaveAsync();

        // notification on comment like
        var isLikedBefore = await IsLikedBefore(dto);
        await _notificationService.CrateCommentLikeNotification(user, postComment);
    }

    public async Task<List<PostCommentLikeGetDto>> GetCommentLikes(Guid commentId)
    {
        bool res = await _postCommentRepo.IsExistsAsync(commentId);
        if (!res) throw new NotFoundException("Comment");

        var commentLike = await _postCommentLikeRepo.GetWhereAsync(x => x.PostCommentId == commentId, x => new PostCommentLikeGetDto
        {
            CommentId = commentId,
            LikedUser = new UserLikeGetDto
            {
                Id = x.LikedUser.Id,
                CoverImageUrl = x.LikedUser.CoverImageUrl,
                ProfileImageUrl = x.LikedUser.ProfileImageUrl,
                FirstName = x.LikedUser.FirstName,
                LastName = x.LikedUser.LastName,
                UserName = x.LikedUser.UserName
            }
        });

        return commentLike;
    }

    public async Task DeleteAsync(Guid id)
    {
        var likedPost = await _postCommentLikeRepo.GetByIdAsync(id);
        if (likedPost == null) throw new NotFoundException<User>();

        await _postCommentLikeRepo.RemoveAsync(id);
        await _postCommentLikeRepo.SaveAsync();
    }

    public async Task<Guid?> IsLikedBefore(PostCommentLikeCreateDto dto)
    {
        var res = await _postCommentLikeRepo.GetByExpressionAsync(x => x.PostCommentId == dto.CommentId && x.LikedUserId == _userClaimService.GetId());
        if (res != null)
        {
            return res.Id;
        }

        return null;
    }
}
