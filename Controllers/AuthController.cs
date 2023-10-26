using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using System.Text.RegularExpressions;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<User> userManager,
                              IMapper mapper,
                              IUserService userService,
                              SignInManager<User> signInManager,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var newUser = _mapper.Map<User>(model);
            newUser.UUID = await _userService.GetUserUUIDAsync(model.UserName);

            var resultCreation = await _userManager.CreateAsync(newUser);

            var result = new RegisterResult();

            if (resultCreation.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Role.Player);
                await _userManager.AddPasswordAsync(newUser, model.Password);
                await _signInManager.PasswordSignInAsync(newUser, model.Password, false, true);
                result.User = new UserDto()
                {
                    UserName = newUser.UserName,
                    UUID = newUser.UUID,
                    Email = newUser.Email,
                    EmailConfirmed = newUser.EmailConfirmed,
                    Role = Role.Player
                };
                result.Token = _tokenService.GenerateToken(newUser);
            }

            else
            {
                result.Errors = resultCreation.Errors.ToList();
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
                user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if(user is not null)
            {
                var resultLogin = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                var userRoles = await _userManager.GetRolesAsync(user);
                result = new LoginResult()
                {
                    IsBanned = resultLogin.IsNotAllowed,
                    IsLocked = resultLogin.IsLockedOut,
                    IsSuccess = resultLogin.Succeeded,
                    RequiresTwoFactor = resultLogin.RequiresTwoFactor,
                    User = new UserDto()
                    {
                        UserName = user.UserName,
                        UUID = user.UUID,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        Role = userRoles.FirstOrDefault()
                    },
                    Token = _tokenService.GenerateToken(user)
                };
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        //[HttpPost("verify-email")]
        //public async Task<IActionResult> VerifyEmail([FromQuery] string c, string u)
        //{
        //    var user = await _userManager.FindByNameAsync(u);
        //    var resultConfirm = await _userManager.ConfirmEmailAsync(user, c);

        //    var result = new VerifyEmailResult()
        //    {
        //        IsSucceded = resultConfirm.Succeeded,
        //        Errors = resultConfirm.Errors.ToList()
        //    };

        //    return Ok(result);
        //}
    }
}
