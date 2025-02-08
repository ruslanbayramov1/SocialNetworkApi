using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zust.BL.DTOs.Posts;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]PostCreateDto dto)
    {
        await _postService.CreatePostAsync(dto);
        return Created();
    }
}
