using AutoMapper;
using Zust.BL.DTOs.Auths;
using Zust.Core.Entities;

namespace Zust.BL.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}
