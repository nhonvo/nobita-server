using AutoMapper;
using HDBank.API.Models;
using HDBank.API.Models.Response;
using HDBank.Infrastructure.Models;

namespace HDBank.Core.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterModel, AppUser>()
                .ForMember(des => des.PhoneNumber, act => act.MapFrom(src => src.Phone));
            CreateMap<AppUser, UserInfoResponse>();
            CreateMap<Transaction, TransactionHistory>();
        }
    }
}