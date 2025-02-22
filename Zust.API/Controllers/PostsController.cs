using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.BL.Responses.Posts;
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
    private readonly INotificationService _notificationService;
    public PostsController(IPostService postService, IPostLikeService postLikeService, ICommentLikeService commentLikeService, IPostCommentService postCommentService, INotificationService notificationService)
    {
        _postService = postService;
        _postLikeService = postLikeService;
        _commentLikeService = commentLikeService;
        _postCommentService = postCommentService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("User/{userId:guid}")]
    public async Task<IActionResult> GetUserPosts(Guid userId)
    {
        var data = await _postService.GetUserPostsAsync(userId);
        return Ok(data);
    }

    [HttpGet]
    [Route("Post/{postId:guid}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var res = await _postService.GetPostByIdAsync(postId);
        return Ok(res);
    }

    [HttpPost]
    [Route("Post")]
    public async Task<IActionResult> CreatePost([FromForm] PostCreateDto dto)
    {
        PostCreateResponse res = await _postService.CreatePostAsync(dto);
        await _notificationService.CreatePostNotificationForAllFollowers(res.NotificationData);

        return Created();
    }

    [HttpDelete]
    [Route("Post/{postId:guid}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    { 
        await _postService.DeleteAsync(postId);
        return NoContent();
    }

    [HttpGet]
    [Route("Comments/{postId:guid}")]
    public async Task<IActionResult> GetPostComments(Guid postId)
    {
        var data = await _postCommentService.GetCommentsAsync(postId);
        return Ok(data);
    }

    [HttpGet]
    [Route("Comment/{commentId:guid}")]
    public async Task<IActionResult> GetComment(Guid commentId)
    {
        var data =await _postCommentService.GetCommentAsync(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("Comment")]
    public async Task<IActionResult> CreateComment(PostCommentCreateDto dto)
    {
        CommentCreateResponse res = await _postCommentService.CreateCommentAsync(dto);
        await _notificationService.CreateCommentNotification(res.NotificationData);

        return Created();
    }

    [HttpDelete]
    [Route("Comment/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await _postCommentService.DeleteAsync(commentId);
        return NoContent();
    }

    [HttpGet]
    [Route("Replies/{commentId:guid}")]
    public async Task<IActionResult> GetCommentReplies(Guid commentId)
    {
        var data = await _postCommentService.GetRepliesAsync(commentId);
        return Ok(data);
    }

    [HttpGet]
    [Route("Likes/{postId:guid}")]
    public async Task<IActionResult> GetPostLikes(Guid postId)
    {
        var data = await _postLikeService.GetPostLikes(postId);
        return Ok(data);
    }

    [HttpPost]
    [Route("Like")]
    public async Task<IActionResult> CreatePostLike(PostLikeCreateDto dto)
    {
        Guid? id = await _postLikeService.IsLikedBefore(dto);

        if (id.HasValue)
        {
            await _postLikeService.DeleteAsync(id.Value);
            return NoContent();
        }
        else
        {
            PostLikeCreateResponse res = await _postLikeService.CreatePostLikeAsync(dto);
            await _notificationService.CratePostLikeNotification(res.NotificationData);
        }
        return Created();
    }

    [HttpGet]
    [Route("Comment/Likes/{commentId:guid}")]
    public async Task<IActionResult> GetCommentLikes(Guid commentId)
    {
        var data = await _commentLikeService.GetCommentLikes(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("Comment/Like")]
    public async Task<IActionResult> CreateCommentLike(PostCommentLikeCreateDto dto)
    {
        Guid? id = await _commentLikeService.IsLikedBefore(dto);

        if (id.HasValue)
        {
            await _commentLikeService.DeleteAsync(id.Value);
            return NoContent();
        }
        else
        {
            CommentLikeCreateResponse res = await _commentLikeService.CreateCommentLikeAsync(dto);
            await _notificationService.CrateCommentLikeNotification(res.NotificationData);
        }
        return Created();
    }
}
