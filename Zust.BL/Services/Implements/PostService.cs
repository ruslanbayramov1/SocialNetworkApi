using Zust.BL.Constants;
using Zust.BL.DTOs.Notifications;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.Posts;
using Zust.BL.DTOs.Users;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Common;
using Zust.BL.Exceptions.Files;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Helpers;
using Zust.BL.Responses.Posts;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserClaimService _userClaimService;
    private readonly IUserRepository _userRepo;
    private readonly IAzureCloudBlobService _azureCloudBlobService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    public PostService(IPostRepository postRepository, IUserClaimService userClaimService, IUserRepository userRepo, IAzureCloudBlobService azureCloudBlobService, IUserService userService, INotificationService notificationService)
    {
        _postRepository = postRepository;
        _userClaimService = userClaimService;
        _userRepo = userRepo;
        _azureCloudBlobService = azureCloudBlobService;
        _userService = userService;
        _notificationService = notificationService;
    }

    public async Task<List<PostGetDto>> GetUserPostsAsync(Guid userId)
    {
        var user = await _userRepo.IsExistsAsync(userId);
        if (!user)
            throw new NotFoundException<User>();

        UserProfileGetDto userDto = await _userService.GetUserProfileById(userId);

        List<PostGetDto>? posts = await _postRepository.GetWhereAsync(x => x.PostedUserId == userId, x => new PostGetDto
        {
            Id = x.Id,
            Content = x.Content,
            ImageUrl = x.ImageUrl,
            LikeCount = x.Likes.Count(),
            PostedUser = userDto,
            Comments = x.Comments
            .Where(y => y.ParentCommentId == null)
            .Select(y => new PostCommentGetDto
            {
                Id = y.Id,
                PostId = y.PostId,
                Content = y.Content,
                ParentCommentId = y.ParentCommentId,
                LikeCount = y.Likes.Count(),
                ReplyCount = y.Replies.Count(),
                CommentedUser = new UserCommentGetDto
                {
                    Id = y.CommentedUser.Id,
                    FirstName = y.CommentedUser.FirstName,
                    LastName = y.CommentedUser.LastName,
                    CoverImageUrl = y.CommentedUser.CoverImageUrl,
                    ProfileImageUrl = y.CommentedUser.ProfileImageUrl,
                    UserName = y.CommentedUser.UserName,
                },
            }).ToList(),
        });

        return posts;
    }

    public async Task<PostGetDto> GetPostByIdAsync(Guid postId)
    {
        bool res = await _postRepository.IsExistsAsync(postId);
        if (!res)
            throw new NotFoundException<Post>();

        User? user = await _userRepo.GetByExpressionAsync(x => x.Posts.Select(y => y.Id).Contains(postId));
        if (user == null) throw new NotFoundException<User>();

        UserProfileGetDto userDto = await _userService.GetUserProfileById(user.Id);

        PostGetDto? post = await _postRepository.GetByIdAsync(postId, x => new PostGetDto
        {
            Id = x.Id,
            Content = x.Content,
            ImageUrl = x.ImageUrl,
            LikeCount = x.Likes.Count(),
            PostedUser = userDto,
            Comments = x.Comments
            .Where(y => y.ParentCommentId == null)
            .Select(y => new PostCommentGetDto
            {
                Id = y.Id,
                PostId = y.PostId,
                Content = y.Content,
                ParentCommentId = y.ParentCommentId,
                LikeCount = y.Likes.Count(),
                ReplyCount = y.Replies.Count(),
                CommentedUser = new UserCommentGetDto
                {
                    Id = y.CommentedUser.Id,
                    FirstName = y.CommentedUser.FirstName,
                    LastName = y.CommentedUser.LastName,
                    CoverImageUrl = y.CommentedUser.CoverImageUrl,
                    ProfileImageUrl = y.CommentedUser.ProfileImageUrl,
                    UserName = y.CommentedUser.UserName,
                },
            }).ToList(),
        });

        return post;
    }

    public async Task<PostCreateResponse> CreatePostAsync(PostCreateDto dto)
    {
        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId(), x => new User
        { 
            Id = x.Id,
            Followers = x.Followers,
            Followings = x.Followings,
            UserName = x.UserName,
        });
        if (user == null) throw new NotFoundException<User>();

        string? imageUrl = null;
        if (dto.Image != null)
        {
            if (!dto.Image.IsValidSize())
            {
                throw new InvalidFileSizeException($"The image size is invalid. Maximum allowed size is {FileConstant.ImageSize / 1024} mb");
            }
            else if (!dto.Image.IsValidType())
            {
                throw new InvalidFileTypeException($"The image type is invalid. Allowed ones are any types of images.");
            }
            imageUrl = await _azureCloudBlobService.UploadImageAsync(dto.Image, AzureFolderDestinations.Posts);
        }

        var model = new Post
        {
            Content = dto.Content,
            PostedUserId = user.Id,
            ImageUrl = imageUrl,
        };
        await _postRepository.AddAsync(model);
        await _postRepository.SaveAsync();

        var response = new PostCreateResponse
        {
            NotificationData = new PostNotificationCreateDto
            {
                FollowerCount = user.Followers.Count(),
                PostId = model.Id,
            }
        };
        return response;
    }

    // helper
    public async Task<Post> GetPostModelByIdAsync(Guid postId)
    {
        var post = await _postRepository.GetByIdAsync(postId, x => new Post
        {
            Id = x.Id,
            Comments = x.Comments,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            DeletedAt = x.DeletedAt,
            ImageUrl = x.ImageUrl,
            IsDeleted = x.IsDeleted,
            Likes = x.Likes,
            PostedUser = x.PostedUser,
            PostedUserId = x.PostedUserId,
            UpdatedAt = x.UpdatedAt,
        });

        if (post == null)
            throw new NotFoundException<Post>();

        return post;
    }
}