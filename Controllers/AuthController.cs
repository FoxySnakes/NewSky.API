using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using System.Text;
using System.Text.RegularExpressions;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper,
                              IUserService userService,
                              ISecurityService securityService,
                              IRepository<User> userRepository,
                              IAuthService authService)
        {
            _mapper = mapper;
            _userService = userService;
            _securityService = securityService;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = new RegisterResult();
            var newUser = _mapper.Map<User>(model);
            newUser.UUID = await _userService.GetUserUUIDAsync(model.UserName);
            if(newUser.UUID == string.Empty)
            {
                result.Errors.Add("Compte Mojang Introuvable");
                return Ok(result);
            }
            newUser.PasswordHash = _securityService.HashPassword(model.Password);
            var resultCreation = await _userRepository.CreateAsync(newUser);



            if (resultCreation.IsSuccess)
            {
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

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var regex = new Regex("^.+@.+\\..+$");
            var user = new User();
            var result = new LoginResult();

            if (regex.IsMatch(model.UsernameOrEmail))
                user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Email == model.UsernameOrEmail);
            else
                user = await _userRepository.Query().FirstOrDefaultAsync(x => x.UserName == model.UsernameOrEmail);

            if (user != null)
            {
                result = await _authService.TryLoginAsync(user, model.Password);
            }
            else
            {
                result = new LoginResult()
                {
                    IsSuccess = false,
                };
            }

            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var changeResult = await _authService.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

            var result = new AccountManageResult(changeResult.Success, null);
            if(!changeResult.Success)
            {
                result.Error = changeResult.Error;
            }
            return Ok(result);
        }

        [HttpPost("disable-account")]
        public async Task<IActionResult> DisableAccount([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                var resultLogin = await _authService.TryLoginAsync(user, passwordDto.OldPassword);

                var result = new AccountManageResult(resultLogin.IsSuccess, null);
                if (resultLogin.IsSuccess)
                {
                    user.LockoutEnd = TimeSpan.MaxValue;
                    var updateResult = await _userRepository.UpdateAsync(user, user.Id);

                    if (updateResult.IsSuccess)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = updateResult.Errors.First().ToString();
                        return Ok(result);
                    }
                }
                else
                {
                    result.Success = false;
                    if (resultLogin.IsLocked || resultLogin.IsBanned)
                    {
                        result.NeedDisconnect = true;
                    }
                    else
                    {
                        result.Error = "Mot de passe incorrect";
                    }
                    return Ok(result);
                }
            }
            return NotFound();
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                var resultLogin = await _authService.TryLoginAsync(user, passwordDto.OldPassword);

                var result = new AccountManageResult(resultLogin.IsSuccess, null);
                if (resultLogin.IsSuccess)
                {
                    var deleteResult = await _userRepository.DeleteAsync(user.Id);

                    if (deleteResult.IsSuccess)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = deleteResult.Errors.First().ToString();
                        return Ok(result);
                    }
                }
                else
                {
                    result.Success = false;
                    if (resultLogin.IsLocked || resultLogin.IsBanned)
                    {
                        result.NeedDisconnect = true;
                    }
                    else
                    {
                        result.Error = "Mot de passe incorrect";
                    }
                    return Ok(result);
                }
            }
            return NotFound();
        }

        [HttpGet("admin")]
        public async Task<IActionResult> IsAdmin()
        {
            var user = await _userService.GetCurrentUserAsync();
            if(user.Permissions.Select(x => x.Permission.Name).Contains(PermissionName.AccessToAdminPanel))
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
