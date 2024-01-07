using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewSky.API.Models;
using NewSky.API.Models.Dto;

namespace NewSky.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<UserNumberVote, UserNumberVoteDto>().ReverseMap();
            CreateMap<VoteReward, VoteRewardDto>().ReverseMap();
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UpdateEmail, User>();
        }
    }
}
