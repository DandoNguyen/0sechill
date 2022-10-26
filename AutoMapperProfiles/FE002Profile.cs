using _0sechill.Dto.FE002.Request;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class FE002Profile : Profile
    {
        public FE002Profile()
        {
            CreateMap<EmployeeInfoDto, ApplicationUser>();
        }
    }
}
