using AutoMapper;
using Zust.BL.DTOs.RelationStatuses;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class RelationStatusProfile : Profile
{
    public RelationStatusProfile()
    {
        CreateMap<RelationStatusCreateDto, RelationStatus>();
        CreateMap<RelationStatusUpdateDto, RelationStatus>();
    }
}
