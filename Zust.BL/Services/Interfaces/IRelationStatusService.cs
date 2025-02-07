using Zust.BL.DTOs.RelationStatuses;

namespace Zust.BL.Services.Interfaces;

public interface IRelationStatusService
{
    Task<List<RelationStatusGetDto>> GetAllAsync();
    Task<RelationStatusGetDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, RelationStatusUpdateDto dto);
    Task CreateAsync(RelationStatusCreateDto dto);
    Task DeleteAsync(Guid id);
    Task<int> GetCountAsync();
}
