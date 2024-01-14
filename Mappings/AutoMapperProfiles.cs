using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Db;
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
            CreateMap<TebexPackageDto, Package>()
                .ForMember(dest => dest.TebexId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Package, TebexPackageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TebexId));
        }
    }
}
