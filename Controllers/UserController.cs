using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                              IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userService.GetCurrentUserAsync(includePackages:true, includeRoles:true, includePermissions:true);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost("update-email")]
        public async Task<IActionResult> UpdateUserEmail(string email)
        {
            var result = await _userService.UpdateEmailAsync(email);
            return Ok(result);
        }

        [HttpGet]
        [Permission(PermissionName.AccessToUsersOnAdminPanel)]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationFilterParamsDto paginationFilterParams)
        {
            var result = await _userService.GetUsersFilteredAsync(paginationFilterParams);
            return Ok(result);
        }

        [Permission(PermissionName.AccessToAdminPanel)]
        [HttpGet("permissions-admin-panel")]
        public async Task<IActionResult> GetCurrentUserAdminPanelPermissions()
        {
            var userPermissions = await _userService.GetCurrentUserAdminPanelPermissionsAsync();
            return Ok(userPermissions);
        }
    }
}
