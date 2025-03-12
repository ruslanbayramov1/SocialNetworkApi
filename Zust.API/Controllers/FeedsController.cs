using Microsoft.AspNetCore.Mvc;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class FeedsController : ControllerBase
{
    private readonly IPostService _postService;
    public FeedsController(IPostService postService)
    {
        _postService = postService;
    }
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Posts()
    {
        var data = await _postService.GetFeedPostsAsync();
        return Ok(data);
    }
}
