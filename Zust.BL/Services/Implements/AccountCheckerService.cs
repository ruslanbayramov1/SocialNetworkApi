using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class AccountCheckerService : IAccountCheckerService
{
    private readonly IUserService _userService;
    private readonly IPostService _postService;
    private readonly IPostCommentService _postCommentService;
    private readonly IFollowRepository _followRepo;
    private readonly IUserClaimService _userClaimService;
    public AccountCheckerService(IUserService userService, IPostService postService, IPostCommentService postCommentService, IFollowRepository followRepository, IUserClaimService userClaimService)
    {
        _userService = userService;
        _postService = postService;
        _postCommentService = postCommentService;
        _userService = userService;
        _followRepo = followRepository;
        _userClaimService = userClaimService;
    }

    public async Task HasPermission(Guid ownerUserId)
    {
        var curUserId = _userClaimService.GetId();
        bool isSelf = curUserId == ownerUserId;

        if (!isSelf)
        {
            var isPrivate = await IsPrivate(ownerUserId);
            if (isPrivate)
            {
                var isFriend = await IsFriend(ownerUserId);
                if (!isFriend) throw new Exception("Bu camaatin priveyt hesabidi, agilli ol!");
            }
        }
    }

    public async Task HasPermission(string ownerUserName)
    {
        var curUserName = _userClaimService.GetUserName();
        bool isSelf = curUserName == ownerUserName;
        if (!isSelf)
        {
            var isPrivate = await IsPrivate(ownerUserName);
            if (isPrivate)
            {
                var isFriend = await IsFriend(ownerUserName);
                if (!isFriend) throw new Exception("Bu camaatin priveyt hesabidi, agilli ol!");
            }
        }
    }

    public async Task<bool> IsPrivate(Guid ownerUserId)
    {
        var user = await _userService.GetById(ownerUserId);
        return user.IsPrivate;
    }

    public async Task<bool> IsFriend(Guid ownerUserId)
    {
        var follow = await _followRepo.GetByExpressionAsync(x => x.FollowerId == _userClaimService.GetId() && x.FollowingId == ownerUserId);
        bool res = follow != null;

        return res;
    }

    public async Task<bool> IsPrivate(string ownerUserName)
    {
        var user = await _userService.GetByName(ownerUserName);
        return user.IsPrivate;
    }

    public async Task<bool> IsFriend(string ownerUserName)
    {
        var user = await _userService.GetByName(ownerUserName);
        var res = await IsFriend(user.Id);
        return res;
    }

    public async Task<Guid> GetPostOwnerIdAsync(Guid postId)
    {
        var post = await _postService.GetPostModelByIdAsync(postId);
        return post.PostedUserId;
    }

    public async Task<Guid> GetPostOwnerIdOnCommentAsync(Guid commentId)
    {
        var comment = await _postCommentService.GetCommentAsync(commentId);
        var postOwnerId = await GetPostOwnerIdAsync(comment.PostId);
        return postOwnerId;
    }
}
