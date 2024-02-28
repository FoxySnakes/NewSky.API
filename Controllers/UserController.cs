using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using System.Data;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService,
                              IMapper mapper,
                              IRoleService roleService,
                              ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Permission(PermissionName.AccessToUsersOnAdminPanel)]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationFilterParamsDto paginationFilterParams)
        {
            var result = await _userService.GetUsersFilteredAsync(paginationFilterParams);
            return Ok(result);
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

            if (!result.Success)
            {
                var user = await _userService.GetCurrentUserAsync();
                _logger.LogError("Failed updating email from '{LastEmail}' to '{NewEmail}' for the user {UserName}", user.Email, email, user.UserName);
            }

            return Ok(result);
        }

        [HttpPost("update-informations")]
        [Permission(PermissionName.UpdateUserInformations)]
        public async Task<IActionResult> UpdateUserInformations([FromBody] UpdateUserInformationsDto model)
        {
            var result = new BaseResult();

            var resultUpdateUsername = await _userService.UpdateUserUsernameAsync(model.Uuid, model.UserName);
            if(!resultUpdateUsername.Success)
            {
                result.Errors = resultUpdateUsername.Errors;
                _logger.LogError("Failed changing username from {LastUserName} to {NewUserName}", _userService.GetCurrentUserName(), model.UserName);
                return Ok(result);
            }

            var user = await _userService.GetUserByUuid(model.Uuid, includeRoles: true);

            foreach (var role in model.Roles)
            {
                if(!user.Roles.Select(x => x.Role.Name).Contains(role))
                {
                    var resultAddRole = await _roleService.AddRoleOnUserAsync(user, role);
                    if (!resultAddRole.Success)
                    {
                        result.Errors = resultAddRole.Errors;
                        _logger.LogError("Failed adding role {RoleName} on user {UserName}", role, user.UserName);
                    }
                }
            }

            var userRolesName = user.Roles.Select(x => x.Role.Name).ToList();

            foreach (var role in userRolesName)
            {
                if (!model.Roles.Contains(role))
                {
                    var resultDeleteRole = await _roleService.RemoveRoleOnUserAsync(user, role);
                    if (!resultDeleteRole.Success)
                    {
                        result.Errors = resultDeleteRole.Errors;
                        _logger.LogError("Failed removing role {RoleName} on user {UserName}", role, user.UserName);
                    }
                }
            }


            return Ok(result);
        }

        [HttpPost("update-punishments")]
        [Permission(PermissionName.UpdateUserPunishment)]
        public async Task<IActionResult> UpdateUserPunishments([FromBody] UpdateUserPunishmentDto model)
        {
            var result = await _userService.UpdateUserPunishmentAsync(model.Username, model.BanishmentEnd, model.LockoutEnd);

            if(!result.Success)
                _logger.LogError("Failed updating punishments for user {UserName}", model.Username);

            return Ok(result);
        }
    }
}
