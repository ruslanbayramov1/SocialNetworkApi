using AutoMapper;
using Zust.BL.DTOs.BloodGroups;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class BloodGroupService : IBloodGroupService
{
    private readonly IBloodGroupRepository _repo;
    private readonly IMapper _mapper;
    public BloodGroupService(IBloodGroupRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(BloodGroupCreateDto dto)
    {
        var entity = _mapper.Map<BloodGroup>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _getEntityByIdAsync(id);
        _repo.Remove(entity);
        await _repo.SaveAsync();
    }

    public async Task<List<BloodGroupGetDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync(x => new BloodGroupGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        return data;
    }

    public async Task<BloodGroupGetDto> GetByIdAsync(Guid id)
    {
        var data = await _getDtoByIdAsync(id);
        return data;
    }

    public async Task<int> GetCountAsync()
        => await _repo.GetAllCountAsync();

    public async Task UpdateAsync(Guid id, BloodGroupUpdateDto dto)
    {
        var entity = await _getEntityByIdAsync(id);
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveAsync();
    }

    private async Task<BloodGroupGetDto> _getDtoByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new BloodGroupGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        if (data == null) throw new NotFoundException("Blood group");
        return data;
    }

    private async Task<BloodGroup> _getEntityByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new BloodGroup
        {
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            Name = x.Name,
            IsDeleted = x.IsDeleted
        });
        if (data == null) throw new NotFoundException("Blood group");

        return data;
    }
}
