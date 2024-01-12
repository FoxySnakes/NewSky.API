using AutoMapper;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ISecurityService _securityService;

        public AuthService(IRepository<User> userRepository, IMapper mapper, ISecurityService securityService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _securityService = securityService;
        }
        public async Task<LoginResult> TryLoginAsync(User user, string password)
        {
            var result = new LoginResult();
            var passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (passwordMatch)
            {
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                user.AccessFailedCount++;
                var updateResult = await _userRepository.UpdateAsync(user, user.Id);
                if (updateResult.IsSuccess)
                {
                    if (user.AccessFailedCount == 3)
                    {
                        user.LockoutEnd = TimeSpan.FromHours(1);
                    }
                    else if (user.AccessFailedCount == 4)
                    {
                        user.LockoutEnd = TimeSpan.FromDays(1);
                    }
                    else if (user.AccessFailedCount >= 5)
                    {
                        user.LockoutEnd = TimeSpan.MaxValue;
                    }
                }
            }

            result.User = _mapper.Map<UserDto>(user);
            result.IsBanned = user.IsBanned;
            result.IsLocked = user.IsLocked;

            return result;
        }

        public async Task<AccountManageDto> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            var passwordMatch = BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
            if (passwordMatch)
            {
                user.PasswordHash = _securityService.HashPassword(newPassword);
                user.AccessFailedCount = 0;
                var changeResult = await _userRepository.UpdateAsync(user, user.Id);
                return new AccountManageDto(changeResult.IsSuccess, null);
            }
            return new AccountManageDto(false, "Mot de passe incorrect");
        }
    }
}
