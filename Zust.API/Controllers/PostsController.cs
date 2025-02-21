using Microsoft.AspNetCore.Mvc;
using Zust.BL.Attributes;
using Zust.BL.DTOs.PostCommentLikes;
using Zust.BL.DTOs.PostComments;
using Zust.BL.DTOs.PostLikes;
using Zust.BL.DTOs.Posts;
using Zust.BL.ExternalServices.Interfaces;
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
    private readonly IUserClaimService _userClaimService;
    private readonly IUserService _userService;
    private readonly IAccountCheckerService _accountCheckerService;
    private readonly INotificationService _notificationService;
    public PostsController(IPostService postService, IPostLikeService postLikeService, ICommentLikeService commentLikeService, IPostCommentService postCommentService, IUserClaimService userClaimService, IUserService userService, IAccountCheckerService accountCheckerService, INotificationService notificationService)
    {
        _postService = postService;
        _postLikeService = postLikeService;
        _commentLikeService = commentLikeService;
        _postCommentService = postCommentService;
        _userClaimService = userClaimService;
        _userService = userService;
        _accountCheckerService = accountCheckerService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("[action]/{userId:guid}")]
    public async Task<IActionResult> User(Guid userId)
    {
        // checking if current user have permission to see all posts of owner's
        await _accountCheckerService.HasPermission(userId);

        var data = await _postService.GetUserPostsAsync(userId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> Post(Guid postId)
    {
        // checking if current user have permission to see post of owner's
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
        await _accountCheckerService.HasPermission(ownerId);

        var res = await _postService.GetPostByIdAsync(postId);
        return Ok(res);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Post([FromForm] PostCreateDto dto)
    {
        PostCreateResponse res = await _postService.CreatePostAsync(dto);
        await _notificationService.CreatePostNotificationForAllFollowers(res.NotificationData);

        return Created();
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> Comments(Guid postId)
    {
        // checking if current user have permission to see the comments at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
        await _accountCheckerService.HasPermission(ownerId);

        var data = await _postCommentService.GetCommentsAsync(postId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> Comment(Guid commentId)
    {
        // checking if current user have permission to see comment at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
        await _accountCheckerService.HasPermission(ownerId);

        var data =await _postCommentService.GetCommentAsync(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Comment(PostCommentCreateDto dto)
    {
        // checking if current user have permission to make comment at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(dto.PostId);
        await _accountCheckerService.HasPermission(ownerId);

        CommentCreateResponse res = await _postCommentService.CreateCommentAsync(dto);
        await _notificationService.CreateCommentNotification(res.NotificationData);

        return Created();
    }

    [HttpGet]
    [Route("[action]/{commentId:guid}")]
    public async Task<IActionResult> Replies(Guid commentId)
    {
        // checking if current user have permission to see the replies on comment at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
        await _accountCheckerService.HasPermission(ownerId);

        var data = await _postCommentService.GetRepliesAsync(commentId);
        return Ok(data);
    }

    [HttpGet]
    [Route("[action]/{postId:guid}")]
    public async Task<IActionResult> Likes(Guid postId)
    {
        // checking if current user have permission to see the likes at the owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
        await _accountCheckerService.HasPermission(ownerId);

        var data = await _postLikeService.GetPostLikes(postId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Like(PostLikeCreateDto dto)
    {
        // checking if current user have permission to make like at the owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(dto.PostId);
        await _accountCheckerService.HasPermission(ownerId);

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
    public async Task<IActionResult> CommentLikes(Guid commentId)
    {
        // checking if current user have permission to see the likes on comment at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
        await _accountCheckerService.HasPermission(ownerId);

        var data = await _commentLikeService.GetCommentLikes(commentId);
        return Ok(data);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CommentLike(PostCommentLikeCreateDto dto)
    {
        // checking if current user have permission to make like on comment at owner's post
        Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(dto.CommentId);
        await _accountCheckerService.HasPermission(ownerId);

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
