using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using System.Data;
using System.Text.RegularExpressions;

namespace NewSky.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IRepository<User> userRepository, IMapper mapper, ISecurityService securityService, IUserService userService, IRoleService roleService, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _securityService = securityService;
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }
        public async Task<LoginResult> TryLoginAsync(string userNameOrEmail, string password)
        {
            var regex = new Regex("^.+@.+\\..+$");
            var user = new User();
            var result = new LoginResult();

            if (regex.IsMatch(userNameOrEmail))
                user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Email == userNameOrEmail);
            else
                user = await _userRepository.Query().FirstOrDefaultAsync(x => x.UserName == userNameOrEmail);

            if (user != null)
            {
                if (user.IsBanned || user.IsLocked)
                {
                    result.IsBanned = user.IsBanned;
                    result.LockoutEnd = user.LockoutEnd;
                    result.IsLocked = user.IsLocked;
                    result.BanishmentEnd = user.BanishmentEnd;
                }
                else if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    result.IsSuccess = true;
                    result.User = _mapper.Map<UserDto>(user);
                    result.Token = _securityService.GenerateToken(user);
                }
                else
                {
                    result.IsSuccess = false;
                    user.AccessFailedCount++;
                    var updateResult = await _userRepository.UpdateAsync(user.Id);
                    if (updateResult.IsSuccess)
                    {
                        if (user.AccessFailedCount == 3)
                        {
                            user.LockoutEnd = DateTime.Now.AddHours(1);
                        }
                        else if (user.AccessFailedCount == 4)
                        {
                            user.LockoutEnd = DateTime.Now.AddDays(1);
                        }
                        else if (user.AccessFailedCount >= 5)
                        {
                            user.LockoutEnd = DateTime.MaxValue;
                            _logger.LogWarning("A user try to access too many times on account '{UserName}'. Account blocked", user.UserName);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<RegisterResult> RegisterAccountAsync(RegisterDto model)
        {
            var newUser = _mapper.Map<User>(model);
            var result = new RegisterResult();
            newUser.UUID = await _userService.GetUserUUIDAsync(model.UserName);
            if (newUser.UUID == string.Empty)
            {
                result.Errors.Add("Compte Mojang Introuvable");
                return result;
            }
            newUser.PasswordHash = _securityService.HashPassword(model.Password);
            var resultCreation = await _userRepository.CreateAsync(newUser);


            if (resultCreation.IsSuccess)
            {
                await _roleService.AddRoleOnUserAsync(newUser, DefaultRole.Player);
                result.User = new UserDto()
                {
                    UserName = newUser.UserName,
                    UUID = newUser.UUID,
                    Email = newUser.Email,
                };
                result.Token = _securityService.GenerateToken(newUser);
            }

            else
            {
                result.Errors = resultCreation.Errors.Select(x => x.Message).ToList();
            }

            return result;
        }

        public async Task<AccountManageResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            var passwordMatch = BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
            if (passwordMatch)
            {
                user.PasswordHash = _securityService.HashPassword(newPassword);
                user.AccessFailedCount = 0;
                var changeResult = await _userRepository.UpdateAsync(user.Id);
                return new AccountManageResult() { Error = null};
            }
            return new AccountManageResult() { Error = "Mot de passe incorrect" };
        }

        public async Task<AccountManageResult> DisableAccountAsync(User user, PasswordDto passwordDto)
        {
            var result = new AccountManageResult();

            if (user != null)
            {
                var resultLogin = await TryLoginAsync(user.UserName, passwordDto.OldPassword);

                if (resultLogin.IsSuccess)
                {
                    user.LockoutEnd = DateTime.MaxValue;
                    var updateResult = await _userRepository.UpdateAsync(user.Id);

                    if (!updateResult.IsSuccess)
                    {
                        result.Error = updateResult.Errors.First().ToString();
                    }
                }
                else
                {
                    if (resultLogin.IsLocked || resultLogin.IsBanned)
                    {
                        result.NeedDisconnect = true;
                    }
                    else
                    {
                        result.Error = "Mot de passe incorrect";
                    }
                }
            }
            return result;
        }

        public async Task<AccountManageResult> DeleteAccountAsync(User user, PasswordDto passwordDto)
        {
            var result = new AccountManageResult();

            if (user != null)
            {
                var resultLogin = await TryLoginAsync(user.UserName, passwordDto.OldPassword);

                if (resultLogin.IsSuccess)
                {
                    var deleteResult = await _userRepository.DeleteAsync(user.Id);

                    if (!deleteResult.IsSuccess)
                    {
                        result.Error = deleteResult.Errors.First().ToString();
                    }
                }
                else
                {
                    if (resultLogin.IsLocked || resultLogin.IsBanned)
                    {
                        result.NeedDisconnect = true;
                        result.Error = $"Ce compte est {(resultLogin.IsBanned ? "banni" : "verouillé")}";
                    }
                    else
                    {
                        result.Error = "Mot de passe incorrect";
                    }
                }
            }
            return result;
        }
    }
}
