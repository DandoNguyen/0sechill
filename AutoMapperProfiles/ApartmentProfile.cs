using _0sechill.Dto.Apartment.Request;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<AddApartmentDto, Apartment>();
        }
    }
}
