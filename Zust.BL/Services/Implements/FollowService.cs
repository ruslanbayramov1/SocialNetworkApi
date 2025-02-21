using Zust.BL.DTOs.Follows;
using Zust.BL.DTOs.Users;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.Core.Interfaces.Repositories;
using Zust.Core.MongoEntities;

namespace Zust.BL.Services.Implements;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _followRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUserClaimService _userClaimService;
    private readonly INotificationService _notificationService;
    public FollowService(IFollowRepository followRepo, IUserRepository userRepo, IUserClaimService userClaimService, INotificationService notificationService)
    {
        _followRepo = followRepo;
        _userRepo = userRepo;
        _userClaimService = userClaimService;
        _notificationService = notificationService;
    }

    public async Task<string> ApproveAndCreate(Guid notificationId)
    {
        Notification notification = await _notificationService.GetNotificationModelByIdAsync(notificationId);

        if (notification.ReceiverId != _userClaimService.GetId())
            throw new Exception("Get oz notificationlariva bax ;)");

        var follow = new Follow
        {
            FollowerId = notification.SenderId,
            FollowingId = _userClaimService.GetId(),
        };

        await _followRepo.AddAsync(follow);
        await _followRepo.SaveAsync();
        return $"User {notification.SenderName}'s friendship request accepted.";
    }

    public async Task<string> CreateAsync(FollowCreateDto dto)
    {
        var followingUser = await _userRepo.GetByIdAsync(dto.FollowingId);
        if (followingUser == null) throw new NotFoundException<User>();

        var follow = new Follow
        {
            FollowerId = _userClaimService.GetId(),
            FollowingId = dto.FollowingId,
        };

        await _followRepo.AddAsync(follow);
        await _followRepo.SaveAsync();
        return $"User {followingUser.UserName} followed.";
    }

    public async Task DeleteAsync(Guid id)
    {
        var res = await _followRepo.IsExistsAsync(id);
        if (!res) throw new NotFoundException("Follow action");

        await _followRepo.RemoveAsync(id);
        await _followRepo.SaveAsync();
    }

    public async Task<List<FollowGetDto>> GetAllFollowersAsync(Guid userId)
    {
        bool res = await _userRepo.IsExistsAsync(userId);
        if (!res) throw new NotFoundException<User>();

        var data = await _followRepo.GetWhereAsync(x => x.FollowingId == userId,x => new FollowGetDto
        { 
            User = new UserProfileGetDto
            { 
                Email = x.FollowerUser.Email,
                FirstName = x.FollowerUser.FirstName,
                LastName = x.FollowerUser.LastName,
                UserName = x.FollowerUser.UserName,
                Role = ((Roles)x.FollowerUser.Role).ToString(),
                CoverImageUrl = x.FollowerUser.CoverImageUrl,
                ProfileImageUrl = x.FollowerUser.ProfileImageUrl,
                FollowerCount = x.FollowerUser.Followers.Count(),
                FollowingCount = x.FollowerUser.Followings.Count(),
                LikeCount = x.FollowerUser.Posts.SelectMany(y => y.Likes).Count(),
            }
        });

        return data;
    }

    public async Task<List<FollowGetDto>> GetAllFollowingsAsync(Guid userId)
    {
        bool res = await _userRepo.IsExistsAsync(userId);
        if (!res) throw new NotFoundException<User>();

        var data = await _followRepo.GetWhereAsync(x => x.FollowerId == userId, x => new FollowGetDto
        {
            User = new UserProfileGetDto
            {
                Email = x.FollowingUser.Email,
                FirstName = x.FollowingUser.FirstName,
                LastName = x.FollowingUser.LastName,
                UserName = x.FollowingUser.UserName,
                Role = ((Roles)x.FollowingUser.Role).ToString(),
                CoverImageUrl = x.FollowingUser.CoverImageUrl,
                ProfileImageUrl = x.FollowingUser.ProfileImageUrl,
                FollowerCount = x.FollowingUser.Followers.Count(),
                FollowingCount = x.FollowingUser.Followings.Count(),
                LikeCount = x.FollowingUser.Posts.SelectMany(y => y.Likes).Count(),
            }
        });

        return data;
    }

    public async Task<Guid?> IsFollowedBefore(FollowCreateDto dto)
    {
        var followingUser = await _userRepo.GetByIdAsync(dto.FollowingId);
        if (followingUser == null) throw new NotFoundException<User>();

        var isFollowedData = await _followRepo.GetByExpressionAsync(x => x.FollowerId == _userClaimService.GetId() && x.FollowingId == dto.FollowingId);
        if (isFollowedData != null)
        {
            return isFollowedData.Id;
        }

        return null;
    }
}
