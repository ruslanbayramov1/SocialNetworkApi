using Zust.BL.DTOs.Notifications;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Responses.Posts;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class PostCommentService : IPostCommentService
{
    private readonly IPostRepository _postRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly IPostCommentRepository _postCommentRepo;
    private readonly IPostService _postService;
    private readonly IUserRepository _userRepo;
    public PostCommentService(IPostRepository postRepository, IUserClaimService userClaimService, IPostCommentRepository postCommentRepo, IUserService userService, IPostService postService, IUserRepository userRepo)
    {
        _postRepo = postRepository;
        _userClaimService = userClaimService;
        _postCommentRepo = postCommentRepo;
        _postService = postService;
        _userRepo = userRepo;
    }

    public async Task<CommentCreateResponse> CreateCommentAsync(PostCommentCreateDto dto)
    {
        var post = await _postService.GetPostModelByIdAsync(dto.PostId);
        if (post == null) throw new NotFoundException<Post>();

        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        PostComment? parentComment = new();
        if (dto.ParentCommentId != null)
        { 
            parentComment = await _postCommentRepo.GetByIdAsync(dto.ParentCommentId.Value);
            if (parentComment == null) throw new NotFoundException("Comment");
        }

        var comment = new PostComment
        {
            ParentCommentId = dto.ParentCommentId,
            CommentedUserId = _userClaimService.GetId(),
            Content = dto.Content,
            PostId = dto.PostId,
        };

        await _postCommentRepo.AddAsync(comment);
        await _postCommentRepo.SaveAsync();

        var res = new CommentCreateResponse
        {
            NotificationData = new CommentNotificationCreateDto
            {
                SenderUserId = user.Id,
                SenderUserName = user.UserName,
                PostedUserId = post.PostedUserId.Value,
                CommentedUserId = parentComment.CommentedUserId.HasValue ? parentComment.CommentedUserId.Value : null,
                CommentId = comment.Id,
                ParentCommentId = comment.ParentCommentId,
            }
        };

        return res;
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var res = await _postCommentRepo.GetByExpressionAsync(x => x.Id == commentId && x.CommentedUserId == _userClaimService.GetId());
        if (res == null)
            throw new NotFoundException("Comment");

        await _postCommentRepo.RemoveAsync(commentId);
        await _postCommentRepo.SaveAsync();
    }

    public async Task<PostCommentGetDto> GetCommentAsync(Guid commentId)
    {
        bool isCommentExists = await _postCommentRepo.IsExistsAsync(commentId);
        if (!isCommentExists) throw new NotFoundException("Comment");

        var comment = await _postCommentRepo.GetByIdAsync(commentId, x => new PostCommentGetDto
        {
            Id = x.Id,
            PostId = x.PostId.Value,
            Content = x.Content,
            ParentCommentId = x.ParentCommentId,
            CommentedUser = new UserCommentGetDto
            {
                Id = x.CommentedUser.Id,
                FirstName = x.CommentedUser.FirstName,
                LastName = x.CommentedUser.LastName,
                CoverImageUrl = x.CommentedUser.CoverImageUrl,
                ProfileImageUrl = x.CommentedUser.ProfileImageUrl,
                UserName = x.CommentedUser.UserName,
            },
            LikeCount = x.Likes.Count(),
            ReplyCount = x.Replies.Count(),
        });
        return comment!;
    }

    public async Task<List<PostCommentGetDto>> GetCommentsAsync(Guid postId)
    {
        bool isPostExists = await _postRepo.IsExistsAsync(postId);
        if (!isPostExists) throw new NotFoundException<Post>();

        var comments = await _postCommentRepo.GetWhereAsync(x => x.PostId == postId && x.ParentCommentId == null, x => new PostCommentGetDto
        {
            Id = x.Id,
            PostId = x.PostId.Value,
            Content = x.Content,
            ParentCommentId = x.ParentCommentId,
            CommentedUser = new UserCommentGetDto
            {
                Id = x.CommentedUser.Id,
                FirstName = x.CommentedUser.FirstName,
                LastName = x.CommentedUser.LastName,
                CoverImageUrl = x.CommentedUser.CoverImageUrl,
                ProfileImageUrl = x.CommentedUser.ProfileImageUrl,
                UserName = x.CommentedUser.UserName,
            },
            LikeCount = x.Likes.Count(),
            ReplyCount = x.Replies.Count(),
        });
        return comments;
    }

    public async Task<List<PostCommentGetDto>> GetRepliesAsync(Guid commentId)
    {
        bool isCommentExists = await _postCommentRepo.IsExistsAsync(commentId);
        if (!isCommentExists) throw new NotFoundException("Comment");

        var comment = await _postCommentRepo.GetByIdAsync(commentId, x => new PostComment { 
            Id = x.Id,
            CommentedUser = x.CommentedUser,
            ParentCommentId = x.ParentCommentId,
            CommentedUserId = x.CommentedUserId,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            DeletedAt = x.DeletedAt,
            IsDeleted = x.IsDeleted,
            Likes = x.Likes,
            ParentComment = x.ParentComment,
            Post = x.Post,
            PostId = x.PostId,
            Replies = x.Replies.Select(y => new PostComment
            {
                Id = y.Id,
                CommentedUser = y.CommentedUser,
                ParentCommentId = y.ParentCommentId,
                CommentedUserId = y.CommentedUserId,
                Content = y.Content,
                CreatedAt = y.CreatedAt,
                DeletedAt = y.DeletedAt,
                IsDeleted = y.IsDeleted,
                Likes = y.Likes,
                ParentComment = y.ParentComment,
                Post = y.Post,
                PostId = y.PostId,
            }).ToList(),
            UpdatedAt = x.UpdatedAt,
        });

        var replies = comment!.Replies.Select(x => new PostCommentGetDto
        {
            Id = x.Id,
            PostId = x.PostId.Value,
            Content = x.Content,
            ParentCommentId = x.ParentCommentId,
            CommentedUser = new UserCommentGetDto
            {
                Id = x.CommentedUser.Id,
                FirstName = x.CommentedUser.FirstName,
                LastName = x.CommentedUser.LastName,
                CoverImageUrl = x.CommentedUser.CoverImageUrl,
                ProfileImageUrl = x.CommentedUser.ProfileImageUrl,
                UserName = x.CommentedUser.UserName,
            },
            LikeCount = x.Likes.Count(),
            ReplyCount = x.Replies.Count(),
        }).ToList();

        return replies;
    }
}
