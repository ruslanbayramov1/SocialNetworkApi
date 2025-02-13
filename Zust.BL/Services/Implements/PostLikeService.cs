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

    public async Task CreatePostLikeAsync(Guid postId)
    {
        var post = await _postService.GetPostModelByIdAsync(postId);

        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        var postLikeBefore = post.Likes.FirstOrDefault(x => x.LikedUserId == user.Id);

        if (postLikeBefore != null) // if user alreadt liked then remove, else - add like
        {
            _postLikeRepo.Remove(postLikeBefore);
        }
        else
        {
            var postLike = new PostLike
            {
                PostId = postId,
                LikedUserId = user.Id
            };
            await _postLikeRepo.AddAsync(postLike);
        }

        await _postLikeRepo.SaveAsync();

        // if a like is creating and liked user is not posted users himself, then store notification in MongoDB
        await _notificationService.CratePostLikeNotification(user, post, postLikeBefore != null);
    }
}
