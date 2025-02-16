using Zust.BL.DTOs.Notifications;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class PostLikeService : IPostLikeService
{
    private readonly IPostService _postService;
    private readonly IPostLikeRepository _postLikeRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly INotificationService _notificationService;
    public PostLikeService(IPostService postService, IPostLikeRepository postLikeRepository, IUserRepository userRepository, IUserClaimService userClaimService, INotificationService notificationService)
    {
        _postService = postService;
        _postLikeRepo = postLikeRepository;
        _userRepo = userRepository;
        _userClaimService = userClaimService;
        _notificationService = notificationService;
    }

    public async Task<List<PostLikeGetDto>> GetPostLikes(Guid postId)
    { 
        var post = await _postService.GetPostModelByIdAsync(postId);

        var postLikes = await _postLikeRepo.GetWhereAsync(x => x.PostId == postId, x => new PostLikeGetDto
        {
            PostId = post.Id,
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
        return postLikes;
    }

    public async Task CreatePostLikeAsync(PostLikeCreateDto dto)
    {
        var post = await _postService.GetPostModelByIdAsync(dto.PostId);

        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        var postLike = new PostLike
        {
            PostId = dto.PostId,
            LikedUserId = user.Id
        };

        await _postLikeRepo.AddAsync(postLike);
        await _postLikeRepo.SaveAsync();

        // if a like is creating and liked user is not posted users himself, then store notification in MongoDB
        await _notificationService.CratePostLikeNotification(new PostLikeNotificationDto 
        { 
            PostedUserId = post.PostedUserId, 
            PostId = post.Id, 
            UserId = user.Id
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        var likedPost = await _postLikeRepo.GetByIdAsync(id);
        if (likedPost == null) throw new NotFoundException<User>();

        await _postLikeRepo.RemoveAsync(id);
        await _postLikeRepo.SaveAsync();
    }

    public async Task<Guid?> IsLikedBefore(PostLikeCreateDto dto)
    {
        var res = await _postLikeRepo.GetByExpressionAsync(x => x.PostId == dto.PostId && x.LikedUserId == _userClaimService.GetId());
        if (res != null)
        { 
            return res.Id;
        }

        return null;
    }
}
