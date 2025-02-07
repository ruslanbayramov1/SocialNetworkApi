using Zust.BL.DTOs.Genders;

namespace Zust.BL.Services.Interfaces;

public interface IGenderService
{
    Task<List<GenderGetDto>> GetAllAsync();
    Task<GenderGetDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, GenderUpdateDto dto);
    Task CreateAsync(GenderCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<int> GetCountAsync();
}
