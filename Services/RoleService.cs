using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> roleRepository,
                           IRepository<UserRole> userRoleRepository,
                           IRepository<RolePermission> rolePermissionRepository,
                           IRepository<Permission> permissionRepository,
                           IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            var roles = await _roleRepository.Query().ToListAsync();
            return roles;
        }

        public async Task<PaginedResult<RoleDto>> GetRolesPaginedAsync(PaginationFilterParamsDto paramsFilter)
        {
            var rolesQueryable = _roleRepository.Query().Include(x => x.Permissions).ThenInclude(x => x.Permission).AsQueryable();
            var roles = new List<Role>();

            if (paramsFilter.Search != null)
            {
                rolesQueryable = rolesQueryable.Where(x => x.Name.Contains(paramsFilter.Search));
            }

            if (paramsFilter.Direction == "asc")
                roles = rolesQueryable.ToList().OrderBy(x =>
                                    char.IsLetter(x.Name[0]) ? 0 :
                                    char.IsDigit(x.Name[0]) ? 1 :
                                    2)
                                .ThenBy(x => x.Name)
                                .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                .Take(paramsFilter.PageSize).ToList();

            else
            {
                roles = await rolesQueryable.OrderByDescending(x => x.Name)
                            .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                            .Take(paramsFilter.PageSize)
                            .ToListAsync();
            }

            var totalCount = await _roleRepository.Query().CountAsync();

            var result = new PaginedResult<RoleDto>
            {
                Items = _mapper.Map<List<RoleDto>>(roles),
                PageNumber = paramsFilter.PageNumber,
                PageSize = paramsFilter.PageSize,
                TotalCount = totalCount,
            };

            return result;
        }

        /// <summary>
        /// Create a Role asynchornously
        /// </summary>
        /// <param name="role">Role to create</param>
        /// <param name="permissions">Permission of the role</param>
        /// <author>Matthieu Delannoy</author>
        /// <seealso cref="Role"/>
        /// <seealso cref="PermissionDto"/>
        /// <returns>The result of the creation of the role</returns>
        public async Task<BaseResult> CreateRoleAsync(Role role, List<PermissionDto> permissions)
        {
            var result = new BaseResult();
            var createResult = await _roleRepository.CreateAsync(role);

            if (createResult.IsSuccess)
            {
                foreach(var permission in permissions)
                {
                    var permissionToAdd = await _permissionRepository.Query().FirstOrDefaultAsync(x => x.Name == permission.Name);
                    if(permissionToAdd != null)
                    {
                        if(permission.HasPermission != null)
                        {
                            var rolePermission = new RolePermission
                            {
                                PermissionId = permissionToAdd.Id,
                                RoleId = role.Id,
                                HasPermission = (bool)permission.HasPermission
                            };
                            var creationPermissionRoleResult = await _rolePermissionRepository.CreateAsync(rolePermission);

                            if (!creationPermissionRoleResult.IsSuccess)
                            {
                                result.Errors.Add(creationPermissionRoleResult.Errors.Select(x => x.Message).First());
                            }
                        }
                    }
                    else
                    {
                        result.Errors.Add("No Permission found with this name");
                    }

                }
            }
            else
            {
                result.Errors.Add(createResult.Errors.Select(x => x.Message).First());
            }
            return result;
        }

        public async Task<BaseResult> UpdateRoleAsync(Role roleToUpdate, List<PermissionDto> permissions)
        {
            var result = new BaseResult();
            var role = await _roleRepository.Query().FirstOrDefaultAsync(x => x.Name == roleToUpdate.Name);
            if(role != null)
            {
                var updateResult = new DbOperationResult<Role>();
                if (!role.IsDefault)
                {
                    role.Description = roleToUpdate.Description;
                    role.Name = roleToUpdate.Name;
                    role.Color = roleToUpdate.Color;
                    updateResult = await _roleRepository.UpdateAsync(role.Id);
                }
                else
                {
                    role.Color = roleToUpdate.Color;
                    updateResult = await _roleRepository.UpdateAsync(role.Id);
                }

                if (updateResult.IsSuccess)
                {
                    foreach (var permission in permissions)
                    {
                        var rolePermission = await _rolePermissionRepository.Query()
                            .Include(x => x.Permission)
                            .FirstOrDefaultAsync(x => x.Permission.Name == permission.Name && x.RoleId == role.Id);

                        if (rolePermission == null && permission.HasPermission != null)
                        {
                            var permissionToAdd = await _permissionRepository.Query().FirstOrDefaultAsync(x => x.Name == permission.Name);
                            var rolePermissionToCreate = new RolePermission { PermissionId = permissionToAdd.Id, RoleId = role.Id, HasPermission = (bool)permission.HasPermission };
                            var createResult = await _rolePermissionRepository.CreateAsync(rolePermissionToCreate);

                            if (!createResult.IsSuccess)
                            {
                                result.Errors.Add(createResult.Errors.Select(x => x.Message).First());
                            }
                        }
                        else if (rolePermission != null && rolePermission.HasPermission != permission.HasPermission && rolePermission.IsEditable)
                        {
                            if (permission.HasPermission != null)
                            {
                            rolePermission.HasPermission = (bool)permission.HasPermission;
                                var updateRolePermissionResult = await _rolePermissionRepository.UpdateAsync(rolePermission.Id);

                                if (!updateRolePermissionResult.IsSuccess)
                                {
                                    result.Errors.Add(updateRolePermissionResult.Errors.Select(x => x.Message).First());
                                }
                            }
                            else
                            {
                                var deleteResult = await _rolePermissionRepository.DeleteAsync(rolePermission.Id);

                                if (!deleteResult.IsSuccess)
                                {
                                    result.Errors.Add(deleteResult.Errors.Select(x => x.Message).First());
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Errors.Add(updateResult.Errors.Select(x => x.Message).First());
                }
            }
            else
            {
                result.Errors.Add("This role don't exist");
            }

            return result;
        }

        public async Task<BaseResult> DeleteRoleAsync(string roleName)
        {
            var result = new BaseResult();
            var role = await _roleRepository.Query().FirstOrDefaultAsync(x => x.Name == roleName);
            if(role != null)
            {
                if (!role.IsDefault)
                {
                    var deleteResult = await _roleRepository.DeleteAsync(role.Id);
                    if (!deleteResult.IsSuccess)
                    {
                        result.Errors.Add(deleteResult.Errors.Select(x => x.Message).First());
                    }
                }
                else
                {
                    result.Errors.Add("You can't delete this role");
                }
            }
            else
            {
                result.Errors.Add("No Role found with this Id");
            }
            return result;
        }

        public async Task<BaseResult> AddRoleOnUserAsync(User user, string roleName)
        {
            var result = new BaseResult();
            var role = await _roleRepository.Query().FirstOrDefaultAsync(x => x.Name == roleName);
            if (role == null)
            {
                result.Errors.Add("Role Not found");
                return result;
            }

            var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
            var createResult = await _userRoleRepository.CreateAsync(userRole);

            if(!createResult.IsSuccess)
            {
                result.Errors.Add(createResult.Errors.Select(x => x.Message).First());
            }

            return result;
        }

        public async Task<BaseResult> RemoveRoleOnUserAsync(User user, string roleName)
        {
            var result = new BaseResult();
            var userRole = await _userRoleRepository.Query().Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == user.Id && x.Role.Name == roleName);

            if (userRole == null)
            {
                result.Errors.Add("This user don't have this role");
                return result;
            }

            var deleteResult = await _userRoleRepository.DeleteAsync(userRole.Id);

            if (!deleteResult.IsSuccess)
            {
                result.Errors.Add(deleteResult.Errors.Select(x => x.Message).First());
            }

            return result;
        }
    }
}
