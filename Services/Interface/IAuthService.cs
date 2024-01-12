using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResult> TryLoginAsync(User user, string password);
        Task<AccountManageDto> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    }
}
