using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResult> TryLoginAsync(string userNameOrEmail, string password);
        Task<AccountManageResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<RegisterResult> RegisterAccountAsync(RegisterDto model);

        Task<AccountManageResult> DisableAccountAsync(User user, PasswordDto passwordDto);
        Task<AccountManageResult> DeleteAccountAsync(User user, PasswordDto passwordDto);

    }
}
