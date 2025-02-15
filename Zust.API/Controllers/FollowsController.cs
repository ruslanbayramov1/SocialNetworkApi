using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.Follows;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class FollowsController : ControllerBase
{
    private readonly IFollowService _followService;
    public FollowsController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Followers()
    {
        var data = await _followService.GetAllFollowersAsync();
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Followings()
    {
        var data = await _followService.GetAllFollowingsAsync();
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
            res = "User unfollowed.";
        }
        else
        {
            var resp = await _followService.CreateAsync(dto);
            res = resp;
        }

        return Ok(res);
    }
}
