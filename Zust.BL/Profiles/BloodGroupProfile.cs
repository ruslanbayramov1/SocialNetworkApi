using AutoMapper;
using Zust.BL.DTOs.BloodGroups;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class BloodGroupProfile : Profile
{
    public BloodGroupProfile()
    {
        CreateMap<BloodGroupCreateDto, BloodGroup>();
        CreateMap<BloodGroupUpdateDto, BloodGroup>();
    }
}
