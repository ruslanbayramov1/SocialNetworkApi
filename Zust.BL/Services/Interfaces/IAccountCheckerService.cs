using Zust.Core.Entities;

namespace Zust.BL.Services.Interfaces;

public interface IAccountCheckerService
{
    Task HasPermission(Guid ownerUserId);
    Task HasPermission(string ownerUserName);
    Task<bool> IsPrivate(Guid ownerUserId);
    Task<bool> IsFriend(Guid ownerUserId);
    Task<bool> IsPrivate(string ownerUserName);
    Task<bool> IsFriend(string ownerUserName);
    Task<Guid> GetPostOwnerIdAsync(Guid postId);
    Task<Guid> GetPostOwnerIdOnCommentAsync(Guid commentId);
}
