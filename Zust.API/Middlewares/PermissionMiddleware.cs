using Zust.API.Constants;
using Zust.BL.Services.Interfaces;

namespace Zust.API.Middlewares;

public class PermissionMiddleware : IMiddleware
{
    IAccountCheckerService _accountCheckerService;
    public PermissionMiddleware(IAccountCheckerService accountCheckerService)
    {
        _accountCheckerService = accountCheckerService;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Check if request's controller and action matches for the conditions and proceed accordingly
        var routeData = context.Request.RouteValues;

        if (context.Request.Path.HasValue)
        {
            string? currentController = routeData["controller"]?.ToString();
            string? currentAction = routeData["action"]?.ToString();
            var isCurrentController = PermissionMiddlewareEndpointConstant.RequiredControllers.FirstOrDefault(x => x == currentController);

            if (currentController != null && isCurrentController != null)
            {
                if (currentAction != null && PermissionMiddlewareEndpointConstant.UserIdEndpointMethods.FirstOrDefault(x => x == currentAction) != null)
                {
                    string? userIdStr = routeData["userId"]?.ToString();
                    Guid userId = Guid.Parse(userIdStr!);

                    await _accountCheckerService.HasPermission(userId);
                }
                else if (currentAction != null && PermissionMiddlewareEndpointConstant.PostIdEnpdointMethods.FirstOrDefault(x => x == currentAction) != null)
                {
                    string? postIdStr = routeData["postId"]?.ToString();
                    Guid postId = Guid.Parse(postIdStr!);

                    Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
                    await _accountCheckerService.HasPermission(ownerId);
                }
                else if (currentAction != null && PermissionMiddlewareEndpointConstant.CommentIdEndpointMethods.FirstOrDefault(x => x == currentAction) != null)
                {
                    string? commentIdStr = routeData["commentId"]?.ToString();
                    Guid commentId = Guid.Parse(commentIdStr!);

                    Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
                    await _accountCheckerService.HasPermission(ownerId);
                }
            }
        }

        // Proceed with the next middleware in the pipeline if permission check passed
        await next(context);
    }
}
