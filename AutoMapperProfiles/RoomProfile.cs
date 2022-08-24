using _0sechill.Hubs.Dto.Response;
using _0sechill.Hubs.Model;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>()
                .ForMember(x => x.roomId, opt => opt.MapFrom(x => x.ID.ToString()));
        }
    }
}
