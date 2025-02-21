using Zust.BL.DTOs.Follows;

namespace Zust.BL.Services.Interfaces;

public interface IFollowService
{
    Task<List<FollowGetDto>> GetAllFollowersAsync(Guid userId);
    Task<List<FollowGetDto>> GetAllFollowingsAsync(Guid userId);
    Task<string> CreateAsync(FollowCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<Guid?> IsFollowedBefore(FollowCreateDto dto);
    Task<string> ApproveAndCreate(Guid notificationId);
}
