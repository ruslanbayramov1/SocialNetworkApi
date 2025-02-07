using Zust.BL.DTOs.Languages;

namespace Zust.BL.Services.Interfaces;

public interface ILanguageService
{
    Task<List<LanguageGetDto>> GetAllAsync();
    Task<LanguageGetDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, LanguageUpdateDto dto);
    Task CreateAsync(LanguageCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<int> GetCountAsync();
}
