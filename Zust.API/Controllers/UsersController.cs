using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.Users;
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
    public async Task<IActionResult> Profile(Guid userId)
    {
        var user = await _userService.GetUserProfileById(userId);
        return Ok(user);
    }

    [HttpGet]
    [Route("[action]/{userName}")]
    public async Task<IActionResult> Profile(string userName)
    {
        var user = await _userService.GetUserProfileByName(userName);
        return Ok(user);
    }

    [HttpGet]
    [Route("[action]/{userName}")]
    public async Task<IActionResult> Account(string userName)
    {
        var user = await _userService.GetUserAccountByName(userName);
        return Ok(user);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> UpdateAccount(UserProfileUpdateDto dto)
    { 
        await _userService.UpdateProfile(dto);
        return Created();
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Notifications()
    {
        var data = await _notificationService.GetUserNotifications();
        return Ok(data);
    }
}
