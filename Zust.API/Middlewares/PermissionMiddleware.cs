using Zust.BL.Constants;
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
        // Check if route contains postId or commentId, as that is where permission checks need to happen
        var routeData = context.Request.RouteValues;

        if (context.Request.Path.HasValue && context.Request.Path.Value == "/api/Posts/Post")
        {
            await next(context);
            return;
        }

        if (context.Request.Path.HasValue)
        {

            // SABAH ENDPOINT ADLARINI (method) UNIQUE QOYURAM VE REQUSESTDEKI METHOD ADINA GORE FILTERLEME ELIYIREM. EKS HALDA ALINMIR.
            string? reqPath = context.Request.Path.Value;
            string[] pathArr = [];

            if (!reqPath.EndsWith("/"))
            {
                string manipulatedReqPath = reqPath + "/";
                pathArr = manipulatedReqPath.Split('/');
            }
            else
            { 
                pathArr = context.Request.Path.Value.Split('/');
            }
            Array.Resize(ref pathArr, pathArr.Length - 1);


            string path = String.Join("/", pathArr);

            if (PermissionMiddlewareEndpointConstant.UserIdEnpoints.FirstOrDefault(x => x == path) != null)
            { 
                string? userIdStr = routeData["userId"]?.ToString();

                Guid userId = Guid.Parse(userIdStr!);
            
                await _accountCheckerService.HasPermission(userId);
            }
            else if (PermissionMiddlewareEndpointConstant.PostIdEnpoints.FirstOrDefault(x => x == path) != null)
            {
                string? postIdStr = routeData["postId"]?.ToString();
                Guid postId = Guid.Parse(postIdStr!);

                Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
                await _accountCheckerService.HasPermission(ownerId);
            }
            else if (PermissionMiddlewareEndpointConstant.CommentIdEnpoints.FirstOrDefault(x => x == path) != null)
            {
                string? commentIdStr = routeData["commentId"]?.ToString();
                Guid commentId = Guid.Parse(commentIdStr!);

                Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
                await _accountCheckerService.HasPermission(ownerId);
            }
        }
       

        // Proceed with the next middleware in the pipeline if permission check passed
        await next(context);
    }
}
