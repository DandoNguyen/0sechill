using _0sechill.Dto.Apartment.Request;
using _0sechill.Dto.Apartment.Response;
using _0sechill.Dto.Block.Response;
using _0sechill.Dto.Comments.Request;
using _0sechill.Dto.Comments.Response;
using _0sechill.Dto.Contract.Request;
using _0sechill.Dto.Contract.Response;
using _0sechill.Dto.FE001.Model;
using _0sechill.Dto.FE001.Response;
using _0sechill.Dto.FE002.Request;
using _0sechill.Dto.FE003.Request;
using _0sechill.Dto.FE003.Response;
using _0sechill.Dto.FE004.Response;
using _0sechill.Dto.FE006.Response;
using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Hubs.Dto.Response;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Models.Dto.UserDto.Request;
using _0sechill.Models.IssueManagement;
using AutoMapper;

namespace _0sechill
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Apartment
            CreateMap<EditApartmentDto, Apartment>();
            CreateMap<Apartment, Apartment>()
                .ForMember(x => x.blockId, opt => opt.Ignore())
                .ForMember(x => x.apartmentId, opt => opt.Ignore());

            //Auth
            CreateMap<RegistrationDto, ApplicationUser>();

            //Block
            CreateMap<BlockDto, Block>();

            //FE001
            CreateMap<ApplicationUser, FE001UserModel>()
                .ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.lastName + src.firstName));
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.lastName + src.firstName));

            //FE002
            CreateMap<EmployeeInfoDto, ApplicationUser>();

            //FE003
            CreateMap<CreateIssueDto, Issues>();
            CreateMap<Issues, IssueDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID.ToString()))
                .ForMember(dest => dest.authorName, opt => opt.MapFrom(src => src.author.lastName + src.author.firstName));

            //FE004
            CreateMap<BookingTask, BookingTaskDto>();

            //FE006
            CreateMap<AssignIssue, IssueStaffDto>()
                .ForMember(dest => dest.AssignIssueID, opt => opt.MapFrom(src => src.ID.ToString()))
                .ForMember(dest => dest.staffId, opt => opt.MapFrom(src => src.staff.Id));
            CreateMap<ApplicationUser, IssueDto>();

            //Message
            CreateMap<Message, MessageResponseDto>().ForMember(x => x.username, opt => opt.MapFrom(x => x.User.UserName));

            //Room
            CreateMap<Room, RoomDto>().ForMember(x => x.roomId, opt => opt.MapFrom(x => x.ID.ToString()));

            //UserHistory
            CreateMap<Block, BlockDto>();
            CreateMap<Apartment, ApartmentDto>();

            //User
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, userProfileDto>();
            CreateMap<ApplicationUser, userProfileDto>().ReverseMap();

            //Contract 
            CreateMap<AddNewContractDto, UserHistory>();
            CreateMap<UserHistory, ContractDto>()
                .ForMember(X => X.ID, opt => opt.MapFrom(src => src.userHistoryId));

            //CommentDto
            CreateMap<CreateCommentDto, Comments>();
            CreateMap<Comments, CommentDto>();
        }
    }
}
