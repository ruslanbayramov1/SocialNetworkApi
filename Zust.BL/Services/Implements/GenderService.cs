using AutoMapper;
using Zust.BL.DTOs.Genders;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class GenderService : IGenderService
{
    private readonly IGenderRepository _repo;
    private readonly IMapper _mapper;
    public GenderService(IGenderRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(GenderCreateDto dto)
    {
        var entity = _mapper.Map<Gender>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _getEntityByIdAsync(id);
        _repo.Remove(entity);
        await _repo.SaveAsync();
    }

    public async Task<List<GenderGetDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync(x => new GenderGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        return data;
    }

    public async Task<GenderGetDto> GetByIdAsync(Guid id)
    {
        var data = await _getDtoByIdAsync(id);
        return data;
    }

    public async Task<int> GetCountAsync()
        => await _repo.GetAllCountAsync();

    public async Task UpdateAsync(Guid id, GenderUpdateDto dto)
    {
        Gender entity = await _getEntityByIdAsync(id);
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveAsync();
    }

    private async Task<GenderGetDto> _getDtoByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new GenderGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        if (data == null) throw new NotFoundException<Gender>();
        return data;
    }

    private async Task<Gender> _getEntityByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new Gender
        {
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            Name = x.Name,
            IsDeleted = x.IsDeleted
        });
        if (data == null) throw new NotFoundException<Gender>();

        return data;
    }
}
