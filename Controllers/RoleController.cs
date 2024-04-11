using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, IMapper mapper, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _roleService.GetRolesAsync();
            var rolesNames = roles.Select(x => x.Name).ToList();  
            return Ok(rolesNames);
        }

        [HttpGet("pagined")]
        public async Task<IActionResult> GetRolesPaginedAsync([FromQuery] PaginationFilterParamsDto paginationFilterParams)
        {
            var rolesPagined = await _roleService.GetRolesPaginedAsync(paginationFilterParams);
            return Ok(rolesPagined);
        }

        [HttpPost("create")]
        [Permission(PermissionName.CreateRole)]
        public async Task<IActionResult> CreateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var resultCreation = await _roleService.CreateRoleAsync(role, roleDto.Permissions);

            if (!resultCreation.Success)
                _logger.LogWarning("Failed creating role '{RoleName}'", roleDto.Name);

            return Ok(resultCreation);
        }

        [HttpPost("update")]
        [Permission(PermissionName.UpdateRole)]
        public async Task<IActionResult> UpdateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var resultUpdate = await _roleService.UpdateRoleAsync(role, roleDto.Permissions);

            if (!resultUpdate.Success)
                _logger.LogError("Failed upadting role '{RoleName}'", roleDto.Name);

            return Ok(resultUpdate);
        }

        [HttpPost("delete")]
        [Permission(PermissionName.DeleteRole)]
        public async Task<IActionResult> DeleteRoleAsync(string roleName)
        {
            var resultDelete = await _roleService.DeleteRoleAsync(roleName);

            if (!resultDelete.Success)
                _logger.LogError("Failed deleting role '{RoleName}'", roleName);

            return Ok(resultDelete);
        }
    }
}
