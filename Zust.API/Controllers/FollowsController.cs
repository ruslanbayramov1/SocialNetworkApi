using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.Follows;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Auth]
public class FollowsController : ControllerBase
{
    private readonly IFollowService _followService;
    public FollowsController(IFollowService followService)
    {
        _followService = followService;
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
            var resp = await _followService.CreateAsync(dto);
            res = resp;
        }

        return StatusCode(StatusCodes.Status201Created, res);
    }
}
