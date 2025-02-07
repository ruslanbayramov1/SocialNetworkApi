using AutoMapper;
using Zust.BL.DTOs.Genders;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class GenderProfile : Profile
{
    public GenderProfile()
    {
        CreateMap<GenderCreateDto, Gender>();
        CreateMap<GenderUpdateDto, Gender>();
    }
}
