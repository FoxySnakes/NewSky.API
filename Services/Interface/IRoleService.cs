using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();

        Task<PaginedResult<RoleDto>> GetRolesPaginedAsync(PaginationFilterParamsDto paginationFilterParamsDto);
        Task<BaseResult> CreateRoleAsync(Role role, List<PermissionDto> permissions);
        Task<BaseResult> UpdateRoleAsync(Role role, List<PermissionDto> permissions);
        Task<BaseResult> DeleteRoleAsync(string roleName);

        Task<BaseResult> AddRoleOnUserAsync(User user,  string roleName);
        Task<BaseResult> RemoveRoleOnUserAsync(User user, string roleName);
    }
}
