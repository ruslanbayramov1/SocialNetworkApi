using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Auth]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    public UsersController(IUserService userService, INotificationService notificationService)
    {
        _userService = userService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("[action]/{userId:guid}")]
    public async Task<IActionResult> GetProfileById(Guid userId)
    {
        var user = await _userService.GetUserProfileById(userId);
        return Ok(user);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetNotifications()
    {
        var data = await _notificationService.GetUserNotifications();
        return Ok(data);
    }
}
