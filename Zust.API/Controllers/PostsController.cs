using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Auth]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IPostLikeService _postLikeService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly IPostCommentService _postCommentService;
    public PostsController(IPostService postService, IPostLikeService postLikeService, ICommentLikeService commentLikeService, IPostCommentService postCommentService)
    {
        _postService = postService;
        _postLikeService = postLikeService;
        _commentLikeService = commentLikeService;
        _postCommentService = postCommentService;
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> GetById(Guid postId)
    {
        return Ok(await _postService.GetPostByIdAsync(postId));
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Post([FromForm] PostCreateDto dto)
    {
        await _postService.CreatePostAsync(dto);
        return Created();
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> Comments(Guid postId)
    {
        var data = await _postCommentService.GetCommentsAsync(postId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> Comment(Guid commentId)
    {
        var data =await _postCommentService.GetCommentAsync(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Comment(PostCommentCreateDto dto)
    {
        await _postCommentService.CreateCommentAsync(dto);
        return Created();
    }

    [HttpGet]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> Replies(Guid commentId)
    {
        var data = await _postCommentService.GetRepliesAsync(commentId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> Likes(Guid postId)
    {
        var data = await _postLikeService.GetPostLikes(postId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Like(PostLikeCreateDto dto)
    {
        Guid? id = await _postLikeService.IsLikedBefore(dto);

        if (id.HasValue)
        {
            await _postLikeService.DeleteAsync(id.Value);
            return NoContent();
        }
        else
        {
            await _postLikeService.CreatePostLikeAsync(dto);
        }
        return Created();
    }

    [HttpGet]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> CommentLikes(Guid commentId)
    {
        var data = await _commentLikeService.GetCommentLikes(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> CommentLike(Guid commentId)
    {
        await _commentLikeService.CreateCommentLikeAsync(commentId);
        return Created();
    }
}
