﻿using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // CreateMap<UserDTO,UserDomain>()
            //   .ForMember(x => x.Name,opt => opt.MapFrom(x => x.FullName))
            // .ReverseMap();
            CreateMap<Region, RegionDtoV1>().ReverseMap();
            CreateMap<Region, RegionDtoV2>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
            

        }
    }
}
