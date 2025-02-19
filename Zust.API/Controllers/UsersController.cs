using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.Users;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Auth]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly IUserClaimService _userClaimService;
    private readonly IAccountCheckerService _accountCheckerService;
    public UsersController(IUserService userService, INotificationService notificationService, IUserClaimService userClaimService, IAccountCheckerService accountCheckerService)
    {
        _userService = userService;
        _notificationService = notificationService;
        _userClaimService = userClaimService;
        _accountCheckerService = accountCheckerService;
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
        // checking if current user have permission to see account of owner's
        await _accountCheckerService.HasPermission(userName);

        var user = await _userService.GetUserAccountByName(userName);
        return Ok(user);
    }

    [HttpPost]
    [Route("Update/Account")]
    public async Task<IActionResult> UpdateAccount(UserProfileUpdateDto dto)
    { 
        await _userService.UpdateProfile(dto);
        return Created();
    }

    [HttpPost]
    [Route("Update/ProfileImage")]
    public async Task<IActionResult> UpdateProfileImage(IFormFile image)
    {
        await _userService.UpdateProfileImage(image);
        return Created();
    }

    [HttpPost]
    [Route("Update/ProfileBanner")]
    public async Task<IActionResult> UpdateProfileBanner(IFormFile banner)
    {
        await _userService.UpdateProfileBanner(banner);
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
