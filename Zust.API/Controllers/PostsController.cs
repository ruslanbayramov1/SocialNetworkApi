using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.Posts;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Auth]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create([FromForm]PostCreateDto dto)
    {
        await _postService.CreatePostAsync(dto);
        return Created();
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> GetById(Guid postId)
    { 
        return Ok(await _postService.GetPostByIdAsync(postId));
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateComment(PostCommentCreateDto dto)
    {
        await _postService.CreateCommentAsync(dto);
        return Created();
    }
}
