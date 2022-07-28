using _0sechill.Dto.Issues.Response;
using _0sechill.Models.IssueManagement;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class issueDtoProfiles : Profile
    {
        public issueDtoProfiles()
        {
            CreateMap<Issues, IssueDto>()
                .ForMember(dest => dest.assignIssueModelId, opt => opt.MapFrom(src => src.assignIssue.ID.ToString()));
        }
    }
}
