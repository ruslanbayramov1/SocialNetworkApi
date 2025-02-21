using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.Follows;
using Zust.BL.DTOs.Notifications;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Auth]
public class FollowsController : ControllerBase
{
    private readonly IFollowService _followService;
    private readonly IAccountCheckerService _accountCheckerService;
    private readonly INotificationService _notificationService;
    public FollowsController(IFollowService followService, IAccountCheckerService accountCheckerService, INotificationService notificationService)
    {
        _followService = followService;
        _accountCheckerService = accountCheckerService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("[action]/{userId:guid}")]
    public async Task<IActionResult> Followers(Guid userId)
    {
        var data = await _followService.GetAllFollowersAsync(userId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{userId:guid}")]
    public async Task<IActionResult> Followings(Guid userId)
    {
        var data = await _followService.GetAllFollowingsAsync(userId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Follow(FollowCreateDto dto)
    {
        string res = "";
        Guid? id = await _followService.IsFollowedBefore(dto);
        if (id.HasValue)
        {
            await _followService.DeleteAsync(id.Value);
            return NoContent();
        }
        else
        {
            bool isPrivate = await _accountCheckerService.IsPrivate(dto.FollowingId);
            if (!isPrivate)
            {
                var resp = await _followService.CreateAsync(dto);
                res = resp;
            }
            else
            {
                await _notificationService.CreateFriendRequestNotification(new FriendRequestNotificationCreateDto { FollowingId = dto.FollowingId });
                return StatusCode(StatusCodes.Status201Created, "Friend request sended.");
            }
        }

        return StatusCode(StatusCodes.Status201Created, res);
    }
}
