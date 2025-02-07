using AutoMapper;
using Zust.BL.DTOs.Occupations;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class OccupationProfile : Profile
{
    public OccupationProfile()
    {
        CreateMap<OccupationCreateDto, Occupation>();
        CreateMap<OccupationUpdateDto, Occupation>();
    }
}
