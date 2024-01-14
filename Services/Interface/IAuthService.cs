using NewSky.API.Models.Db;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResult> TryLoginAsync(User user, string password);
        Task<AccountManageResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    }
}
