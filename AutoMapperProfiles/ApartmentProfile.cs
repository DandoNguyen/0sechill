using _0sechill.Dto.Apartment.Request;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<EditApartmentDto, Apartment>();
            CreateMap<Apartment, Apartment>()
                .ForMember(x => x.blockId, opt => opt.Ignore())
                .ForMember(x => x.apartmentId, opt => opt.Ignore());
        }
    }
}
