﻿using _0sechill.Dto.UserDto.Request;
using _0sechill.Models;
using AutoMapper;

namespace _0sechill.AutoMapperProfiles
{
    public class userProfile : Profile
    {
        public userProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();
        }
    }
}
