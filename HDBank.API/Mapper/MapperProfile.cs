using AutoMapper;
using HDBank.API.Models;
using HDBank.Infrastructure.Models;

namespace HDBank.Core.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterModel, AppUser>()
                .ForMember(des => des.PhoneNumber, act => act.MapFrom(src => src.Phone));
        }
    }
}