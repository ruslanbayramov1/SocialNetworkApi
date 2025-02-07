using AutoMapper;
using Zust.BL.DTOs.Occupations;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class OccupationService : IOccupationService
{
    private readonly IOccupationRepository _repo;
    private readonly IMapper _mapper;
    public OccupationService(IOccupationRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(OccupationCreateDto dto)
    {
        var entity = _mapper.Map<Occupation>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _getEntityByIdAsync(id);
        _repo.Remove(entity);
        await _repo.SaveAsync();
    }

    public async Task<List<OccupationGetDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync(x => new OccupationGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        return data;
    }

    public async Task<OccupationGetDto> GetByIdAsync(Guid id)
    {
        var data = await _getDtoByIdAsync(id);
        return data;
    }

    public async Task<int> GetCountAsync()
        => await _repo.GetAllCountAsync();

    public async Task UpdateAsync(Guid id, OccupationUpdateDto dto)
    {
        var entity = await _getEntityByIdAsync(id);
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveAsync();
    }

    private async Task<OccupationGetDto> _getDtoByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new OccupationGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        if (data == null) throw new NotFoundException<Occupation>();
        return data;
    }

    private async Task<Occupation> _getEntityByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new Occupation
        {
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            Name = x.Name,
            IsDeleted = x.IsDeleted
        });
        if (data == null) throw new NotFoundException<Occupation>();

        return data;
    }
}
