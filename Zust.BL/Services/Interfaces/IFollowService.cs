using Zust.BL.DTOs.Follows;

namespace Zust.BL.Services.Interfaces;

public interface IFollowService
{
    Task<List<FollowGetDto>> GetAllFollowersAsync();
    Task<List<FollowGetDto>> GetAllFollowingsAsync();
    Task<string> CreateAsync(FollowCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<Guid?> IsFollowedBefore(FollowCreateDto dto);
}
