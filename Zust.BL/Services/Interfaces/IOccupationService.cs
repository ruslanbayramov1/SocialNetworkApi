using Zust.BL.DTOs.Occupations;

namespace Zust.BL.Services.Interfaces;

public interface IOccupationService
{
    Task<List<OccupationGetDto>> GetAllAsync();
    Task<OccupationGetDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, OccupationUpdateDto dto);
    Task CreateAsync(OccupationCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<int> GetCountAsync();
}
