using _0sechill.Dto.Block.Response;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class BlockProfile : Profile
    {
        public BlockProfile()
        {
            CreateMap<BlockDto, Block>();
        }
    }
}
