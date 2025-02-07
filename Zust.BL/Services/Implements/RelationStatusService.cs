using AutoMapper;
using Zust.BL.DTOs.RelationStatuses;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class RelationStatusService : IRelationStatusService
{
    private readonly IRelationStatusRepository _repo;
    private readonly IMapper _mapper;
    public RelationStatusService(IRelationStatusRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(RelationStatusCreateDto dto)
    {
        var entity = _mapper.Map<RelationStatus>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _getEntityByIdAsync(id);
        _repo.Remove(entity);
        await _repo.SaveAsync();
    }

    public async Task<List<RelationStatusGetDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync(x => new RelationStatusGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        return data;
    }

    public async Task<RelationStatusGetDto> GetByIdAsync(Guid id)
    {
        var data = await _getDtoByIdAsync(id);
        return data;
    }

    public async Task<int> GetCountAsync()
        => await _repo.GetAllCountAsync();

    public async Task UpdateAsync(Guid id, RelationStatusUpdateDto dto)
    {
        var entity = await _getEntityByIdAsync(id);
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveAsync();
    }

    private async Task<RelationStatusGetDto> _getDtoByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new RelationStatusGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        if (data == null) throw new NotFoundException("Relation status");
        return data;
    }

    private async Task<RelationStatus> _getEntityByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new RelationStatus
        {
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            Name = x.Name,
            IsDeleted = x.IsDeleted
        });
        if (data == null) throw new NotFoundException("Relation status");

        return data;
    }
}
