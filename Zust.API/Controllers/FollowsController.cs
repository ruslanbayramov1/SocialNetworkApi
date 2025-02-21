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
        await _accountCheckerService.HasPermission(userId);

        var data = await _followService.GetAllFollowersAsync(userId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{userId:guid}")]
    public async Task<IActionResult> Followings(Guid userId)
    {
        await _accountCheckerService.HasPermission(userId);

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
                var notificationData = new FriendRequestNotificationCreateDto { FollowingId = dto.FollowingId };
                Guid? notificationId = await _notificationService.IsFollowRequestExistsAsync(notificationData);

                if (!notificationId.HasValue)
                {
                    await _notificationService.CreateFriendRequestNotification(notificationData);
                    return StatusCode(StatusCodes.Status201Created, "Friend request sended.");
                }
                else
                { 
                    await _notificationService.UpdateNotificationHiddenInfo(notificationId.Value);
                    return StatusCode(StatusCodes.Status204NoContent, "Friend request removed.");
                }
            }
        }

        return StatusCode(StatusCodes.Status201Created, res);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Respond(FollowRespondDto dto)
    {
        if (!dto.IsApproved)
        {
            await _notificationService.UpdateNotificationHiddenInfo(dto.NotificationId);
            return NoContent();
        }

        var res = await _followService.ApproveAndCreate(dto.NotificationId);
        await _notificationService.UpdateNotificationHiddenInfo(dto.NotificationId);
        return StatusCode(StatusCodes.Status202Accepted, res);
    }
}
