using Zust.BL.DTOs.BloodGroups;

namespace Zust.BL.Services.Interfaces;

public interface IBloodGroupService
{
    Task<List<BloodGroupGetDto>> GetAllAsync();
    Task<BloodGroupGetDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, BloodGroupUpdateDto dto);
    Task CreateAsync(BloodGroupCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<int> GetCountAsync();
}
