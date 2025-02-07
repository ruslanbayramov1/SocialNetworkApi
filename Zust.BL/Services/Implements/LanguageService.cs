using AutoMapper;
using Zust.BL.DTOs.Languages;
using Zust.BL.Exceptions.Common;
using Zust.BL.Services.Interfaces;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.Services.Implements;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _repo;
    private readonly IMapper _mapper;
    public LanguageService(ILanguageRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(LanguageCreateDto dto)
    {
        var entity = _mapper.Map<Language>(dto);
        await _repo.AddAsync(entity);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _getEntityByIdAsync(id);
        _repo.Remove(entity);
        await _repo.SaveAsync();
    }

    public async Task<List<LanguageGetDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync(x => new LanguageGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        return data;
    }

    public async Task<LanguageGetDto> GetByIdAsync(Guid id)
    {
        var data = await _getDtoByIdAsync(id);
        return data;
    }

    public async Task<int> GetCountAsync()
        => await _repo.GetAllCountAsync();

    public async Task UpdateAsync(Guid id, LanguageUpdateDto dto)
    {
        var entity = await _getEntityByIdAsync(id);
        _mapper.Map(dto, entity);
        _repo.Update(entity);
        await _repo.SaveAsync();
    }

    private async Task<LanguageGetDto> _getDtoByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new LanguageGetDto
        {
            Id = x.Id,
            Name = x.Name,
        });

        if (data == null) throw new NotFoundException<Language>();
        return data;
    }

    private async Task<Language> _getEntityByIdAsync(Guid id)
    {
        var data = await _repo.GetByIdAsync(id, x => new Language
        {
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            DeletedAt = x.DeletedAt,
            Id = x.Id,
            Name = x.Name,
            IsDeleted = x.IsDeleted
        });
        if (data == null) throw new NotFoundException<Language>();

        return data;
    }
}
