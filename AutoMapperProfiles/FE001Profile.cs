using _0sechill.Dto.FE001.Model;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class FE001Profile : Profile
    {
        public FE001Profile()
        {
            CreateMap<ApplicationUser, FE001UserModel>()
                .ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.lastName + src.firstName));
        }
    }
}
