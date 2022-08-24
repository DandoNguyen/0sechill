using _0sechill.Hubs.Dto.Response;
using _0sechill.Hubs.Model;

namespace _0sechill.AutoMapperProfiles
{
    public class MessageProfile: AutoMapper.Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageResponseDto>().ForMember(x => x.username, opt => opt.MapFrom(x => x.User.UserName));
        }
    }
}
