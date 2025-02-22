using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Zust.API.Constants;
using Zust.BL.Exceptions.Common;
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
                context.Request.EnableBuffering();
                string requestBody = "";
                dynamic requestData = "";

                if (!context.Request.ContentType?.ToLower().StartsWith("multipart/") ?? true)
                {
                    using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        requestBody = await reader.ReadToEndAsync();
                        if (!requestBody.IsNullOrEmpty() || !String.IsNullOrWhiteSpace(requestBody))
                        {
                            requestData = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody)!;
                        }
                    }
                }
                else
                { 
                    requestData = await context.Request.ReadFormAsync();
                }

                try
                {
                    if (currentAction != null && PermissionMiddlewareEndpointConstant.UserIdEndpointMethods.FirstOrDefault(x => x == currentAction) != null)
                    {
                        string? userIdStr = routeData["userId"]?.ToString() ?? requestData["userId"].ToString();
                        Guid.TryParse(userIdStr, out Guid userId);

                        await _accountCheckerService.HasPermission(userId);
                    }
                    else if (currentAction != null && PermissionMiddlewareEndpointConstant.PostIdEnpdointMethods.FirstOrDefault(x => x == currentAction) != null)
                    {
                        string? postIdStr = routeData["postId"]?.ToString() ?? requestData["postId"].ToString();
                        Guid.TryParse(postIdStr, out Guid postId);

                        Guid ownerId = await _accountCheckerService.GetPostOwnerIdAsync(postId);
                        await _accountCheckerService.HasPermission(ownerId);
                    }
                    else if (currentAction != null && PermissionMiddlewareEndpointConstant.CommentIdEndpointMethods.FirstOrDefault(x => x == currentAction) != null)
                    {
                        string? commentIdStr = routeData["commentId"]?.ToString() ?? requestData["commentId"].ToString();
                        Guid.TryParse(commentIdStr, out Guid commentId);

                        Guid ownerId = await _accountCheckerService.GetPostOwnerIdOnCommentAsync(commentId);
                        await _accountCheckerService.HasPermission(ownerId);
                    }

                    context.Request.Body.Position = 0;
                }
                catch (Exception ex)
                {
                    if (ex is IBaseException ibe)
                    {
                        context.Response.StatusCode = ibe.StatusCode;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            ibe.StatusCode,
                            ibe.ErrorMessage
                        });

                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = ex.Message
                        });

                        return;
                    }
                }
            }
        }
        // Proceed with the next middleware in the pipeline if permission check passed
        await next(context);
    }
}