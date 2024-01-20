using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IUserService
    {
        Task<BaseResult> UpdateEmailAsync(string email);
        Task<User> GetCurrentUserAsync(bool includePackages = false, bool includeRoles = false, bool includePermissions = false);
        string GetCurrentUserIp();
        string GetCurrentUserName();
        Task<string> GetUserUUIDAsync(string username);

        Task<AdminPanelPermissionDto> GetCurrentUserAdminPanelPermissionsAsync();
        bool HasPermission(User user, string permissionName);

        Task<PaginedResult<UsersFilterByCategory>> GetUsersFilteredAsync(PaginationFilterParamsDto paramsFilter);

    }
}
