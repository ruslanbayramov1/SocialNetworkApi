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
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        return Ok(user);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetProfileById(Guid userId)
    {
        var user = await _userService.GetUserProfileById(userId);
        return Ok(user);
    }
}
