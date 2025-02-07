using AutoMapper;
using Zust.BL.DTOs.Languages;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class LanguageProfile : Profile
{
    public LanguageProfile()
    {
        CreateMap<LanguageCreateDto, Language>();
        CreateMap<LanguageUpdateDto, Language>();
    }
}
