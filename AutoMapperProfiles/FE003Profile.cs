using _0sechill.Dto.FE003.Request;
using _0sechill.Dto.FE003.Response;
using _0sechill.Models.IssueManagement;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class FE003Profile : Profile
    {
        public FE003Profile()
        {
            CreateMap<CreateIssueDto, Issues>();
            CreateMap<Issues, IssueDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID.ToString()))
                .ForMember(dest => dest.authorName, opt => opt.MapFrom(src => src.author.lastName + src.author.firstName));
            
        }
    }
}
