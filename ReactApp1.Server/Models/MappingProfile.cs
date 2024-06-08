using AutoMapper;
using System;
using TestApp.Server.Models;

namespace ReactApp1.Server.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.First().Role.Name)); ;
        }
    }
}
