using _0sechill.Models;
using _0sechill.Models.Dto.UserDto.Request;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegistrationDto, ApplicationUser>();
        }
    }
}
