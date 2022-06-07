using _0sechill.Dto.UserHistory;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class userHistory : Profile
    {
        public userHistory()
        {
            CreateMap<Block, BlockDto>();
            CreateMap<Apartment, ApartmentDto>();
        }
    }
}
