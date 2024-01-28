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

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _roleService.GetRolesAsync();
            var rolesNames = roles.Select(x => x.Name).ToList();  
            return Ok(rolesNames);
        }

        [HttpPut("create")]
        [Permission(PermissionName.CreateRole)]
        public async Task<IActionResult> CreateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var resultCreation = await _roleService.CreateRoleAsync(role, roleDto.Permissions);
            return Ok(resultCreation);
        }

        [HttpPatch("update")]
        [Permission(PermissionName.UpdateRole)]
        public async Task<IActionResult> UpdateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var resultUpdate = await _roleService.UpdateRoleAsync(role, roleDto.Permissions);
            return Ok(resultUpdate);
        }

        [HttpDelete("delete")]
        [Permission(PermissionName.DeleteRole)]
        public async Task<IActionResult> DeleteRoleAsync(string roleName)
        {
            var resultDelete = await _roleService.DeleteRoleAsync(roleName);
            return Ok(resultDelete);
        }
    }
}
