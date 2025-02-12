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
    private readonly IPostService _postService;
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly IPostCommentLikeRepository _postCommentLikeRepo;
    private readonly IPostCommentRepository _postCommentRepo;
    public CommentLikeService(IPostService postService, IUserRepository userRepository, IUserClaimService userClaimService, IPostCommentLikeRepository postCommentLikeRepository, IPostCommentRepository postCommentRepo)
    {
        _postService = postService;
        _userRepo = userRepository;
        _userClaimService = userClaimService;
        _postCommentLikeRepo = postCommentLikeRepository;
        _postCommentRepo = postCommentRepo;
    }

    public async Task CreateCommentLikeAsync(Guid commentId)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        PostComment? postComment = await _postCommentRepo.GetByIdAsync(commentId, x => new PostComment
        {
            CommentedUser = x.CommentedUser,
            CommentedUserId = x.CommentedUserId,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            IsDeleted = x.IsDeleted,
            Likes = x.Likes,
            ParentComment = x.ParentComment,
            ParentCommentId = x.ParentCommentId,
            Post = x.Post,
            PostId = x.PostId,
            Replies = x.Replies,
            UpdatedAt = x.UpdatedAt
        });

        if (postComment == null) throw new NotFoundException("Post comment");

        var postCommentLike = await _postCommentLikeRepo.GetByExpressionAsync(x => x.LikedUserId == user.Id && x.PostCommentId == postComment.Id);

        if (postCommentLike == null)
        {
            var commentLike = new PostCommentLike
            {
                PostCommentId = commentId,
                LikedUserId = user.Id,
            };
            await _postCommentLikeRepo.AddAsync(commentLike);
        }
        else
        {
            _postCommentLikeRepo.Remove(postCommentLike);
        }

        await _postCommentLikeRepo.SaveAsync();
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
}
