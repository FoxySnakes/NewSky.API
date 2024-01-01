using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                              UserManager<User> userManager,
                              IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto()
            {
                UserName = user.UserName,
                UUID = user.UUID,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Role = userRoles.FirstOrDefault()
            };
            return Ok(userDto);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UserPrivateInfoDto userModel)
        {
            var user = await _userService.GetCurrentUserAsync();
            var mappedUser = _mapper.Map(userModel, user);
            var result = await _userManager.UpdateAsync(mappedUser);
            return Ok(result);
        }

        [HttpGet("personnal-information")]
        public async Task<IActionResult> GetCurrentUserPersonnalInformationAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            var userPrivateInfoDto = _mapper.Map<UserPrivateInfoDto>(user);
            return Ok(userPrivateInfoDto);
        }
    }
}
