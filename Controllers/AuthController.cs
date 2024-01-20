using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService,
                              IAuthService authService,
                              IMapper mapper)
        {
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterAccountAsync(model);
            
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.TryLoginAsync(model.UsernameOrEmail, model.Password);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _authService.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

            return Ok(result);
        }

        [HttpPost("disable-account")]
        public async Task<IActionResult> DisableAccount([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _authService.DisableAccountAsync(user,passwordDto);
            return Ok(result);
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] PasswordDto passwordDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _authService.DeleteAccountAsync(user, passwordDto);
            return Ok(result);
        }

        [HttpGet("access-admin-panel")]
        public async Task<IActionResult> HaveAccessToAdminPanel()
        {
            var user = await _userService.GetCurrentUserAsync(includePermissions: true);
            var hasAccess = _userService.HasPermission(user, PermissionName.AccessToAdminPanel);
            return Ok(hasAccess);
        }
    }
}
