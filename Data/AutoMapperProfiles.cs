using AutoMapper;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;

namespace NewSky.API.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<VoteReward, VoteRewardDto>().ReverseMap();

            CreateMap<TebexPackageDto, Package>()
                .ForMember(dest => dest.TebexId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Package, TebexPackageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TebexId));

            CreateMap<RegisterDto, User>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Roles.SelectMany(role =>
                    role.Role.Permissions.Select(permission => new PermissionDto
                    {
                        Name = permission.Permission.Name,
                        HasPermission = permission.HasPermission
                    })).GroupBy(x => x.Name).Select(g => g.OrderBy(p => p.HasPermission).First()).ToList()
                ))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Role.Name)))
                .ForMember(dest => dest.Packages, opt => opt.Ignore())
                .ForMember(dest => dest.Packages, opt => opt.MapFrom(src => src.Packages.Select(userpackage => new PackageCartDto
                {
                    TebexPackage = new TebexPackageDto
                    {
                        Id = userpackage.Package.TebexId,
                        Name = userpackage.Package.Name,
                        ImageUrl = userpackage.Package.ImageUrl,
                        PriceHt = userpackage.Package.PriceHt,
                        PriceTtc = userpackage.Package.PriceTtc,
                        CreationDate = userpackage.Package.CreationDate,
                        ExpirationDate = userpackage.Package.ExpirationDate,
                    },
                    Quantity = userpackage.Quantity,
                })))
                .ForMember(dest => dest.BanishmentEnd, opt => opt.MapFrom(src => src.BanishmentEnd > DateTime.Now ? src.BanishmentEnd : (DateTime?)null))
                .ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd > DateTime.Now ? src.LockoutEnd : (DateTime?)null));

            CreateMap<UserDto, User>();

            CreateMap<RoleDto, Role>()
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions.Select(x => new PermissionDto { Name = x.Permission.Name, HasPermission = x.HasPermission})));

            CreateMap<RolePermission, PermissionDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Permission.Name))
                .ForMember(dest => dest.HasPermission, opt => opt.MapFrom(src => src.HasPermission));
        }
    }
}
