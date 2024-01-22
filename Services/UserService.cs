using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Attributs;
using NewSky.API.Extensions;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;

namespace NewSky.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IRepository<Role> _roleRepository;

        public UserService(IHttpContextAccessor httpContextAccessor,
                           IRepository<User> userRepository,
                           HttpClient httpClient,
                           IMapper mapper,
                           IRepository<Role> roleRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _httpClient = httpClient;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResult> UpdateEmailAsync(string email)
        {
            var user = await GetCurrentUserAsync();
            var newUser = _mapper.Map<User>(user);
            var result = await _userRepository.UpdateAsync(newUser, newUser.Id);
            var baseResult = new BaseResult()
            {
                Errors = result.Errors.Select(x => x.Message).ToList()
            };
            return baseResult;
        }

        public async Task<User> GetCurrentUserAsync(bool includesPackages = false, bool includeRoles = false, bool includePermissions = false)
        {
            var userQuery = _userRepository.Query();

            if (includesPackages)
            {
                userQuery = userQuery.Include(x => x.Packages).ThenInclude(x => x.Package);
            }
            if (includeRoles)
            {
                userQuery = userQuery.Include(x => x.Roles).ThenInclude(x => x.Role);
            }
            if (includePermissions)
            {
                userQuery = userQuery.Include(x => x.Roles).ThenInclude(x => x.Role).ThenInclude(x => x.Permissions).ThenInclude(x => x.Permission);
            }

            var user = await userQuery.FirstOrDefaultAsync(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

            return user;
        }

        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public string GetCurrentUserIp()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userIp = httpContext.Connection.RemoteIpAddress?.ToString();
#if DEBUG
            userIp = "91.173.177.105";
#endif
            return userIp;
        }

        public async Task<string> GetUserUUIDAsync(string username)
        {
            var uuid = string.Empty;
            HttpResponseMessage response;
            do
            {
                response = await _httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
                if( response.StatusCode != HttpStatusCode.OK || response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return uuid;
                }

                await Task.Delay(1000);
            }
            while(response.StatusCode == HttpStatusCode.TooManyRequests);

            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);
            if (jsonContent["id"] != null)
            {
                uuid = jsonContent["id"].ToString();
                //uuid = uuid.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
            }
            return uuid;
        }

        public async Task<AdminPanelPermissionDto> GetCurrentUserAdminPanelPermissionsAsync()
        {
            var user = await GetCurrentUserAsync(includePermissions: true);
            var result = new AdminPanelPermissionDto()
            {
                // Access
                AccessToSalesOnAdminPanel = HasPermission(user, PermissionName.AccessToSalesOnAdminPanel),
                AccessToUsersOnAdminPanel = HasPermission(user, PermissionName.AccessToUsersOnAdminPanel),
                AccessToVotesOnAdminPanel = HasPermission(user, PermissionName.AccessToVotesOnAdminPanel),
                AccessToGeneralSettingsOnAdminPanel = HasPermission(user, PermissionName.AccessToGeneralSettingsOnAdminPanel),

                // Create
                CreateRole = HasPermission(user, PermissionName.CreateRole),

                // Update
                UpdateUserPermissions = HasPermission(user, PermissionName.UpdateUserPermissions),
                UpdateUserUserName = HasPermission(user, PermissionName.UpdateUserUserName),
                UpdateUserStatus = HasPermission(user, PermissionName.UpdateUserStatus),
                ManageUserCart = HasPermission(user, PermissionName.ManageUserCart),
                UpdateGeneralSettings = HasPermission(user, PermissionName.UpdateGeneralSettings),
                UpdateUserRole = HasPermission(user, PermissionName.UpdateUserRole),
                UpdateRole = HasPermission(user, PermissionName.UpdateRole),

                // Delete
                DeleteRole = HasPermission(user, PermissionName.DeleteRole),
            };


            return result;
        }

        public bool HasPermission(User user, string permissionName)
        {
            var hasAccess = false;
            var userPermissions = new List<PermissionDto>();

            userPermissions.AddRange(_mapper.Map<List<PermissionDto>>(user.Roles.SelectMany(role => role.Role.Permissions.Where(x => x.Permission.Name == permissionName))));

            var hasPermissionRefused = false; // On vérifie d'abord si la permission est réfusé car le refus l'emporte sur l'acceptation
            foreach (var permission in userPermissions)
            {
                if ((bool)!permission.HasPermission)
                    hasPermissionRefused = true;
            }

            if (!hasPermissionRefused) // Si il n'a pas de refus, on vérifie si il une acceptation et sinon on lui refuse l'accès par défaut
            {
                foreach (var permission in userPermissions)
                {
                    if ((bool)permission.HasPermission)
                        hasAccess = true;
                }
            }

            return hasAccess;
        }


        public async Task<PaginedResult<UsersFilterByCategory>> GetUsersFilteredAsync(PaginationFilterParamsDto paramsFilter)
        {
            var usersFilterByCategories = new List<UsersFilterByCategory>();
            var usersQueryable = _userRepository.Query().Include(x => x.Roles).ThenInclude(x => x.Role).AsQueryable();

            if (paramsFilter.Search != null)
            {
                usersQueryable = usersQueryable.Where(x => x.UserName.Contains(paramsFilter.Search));
            }

            switch (paramsFilter.Filter)
            {
                case "username":
                    var users = new List<User>();
                    if (paramsFilter.Direction == "asc")
                        users = usersQueryable.ToList().OrderBy(x => x.UserName, new CustomStringComparer())
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize).ToList();

                    else
                        users = await usersQueryable.OrderByDescending(x => x.UserName)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize)
                                        .ToListAsync();

                    var x = users.Select(x => x.UserName).ToList(); 
                    var firstChars = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                    foreach (var character in firstChars)
                    {
                        var userOfCategory = users.Where(x => x.UserName.ToLower().StartsWith(character)).ToList();
                        if (userOfCategory.Any())
                        {
                            usersFilterByCategories.Add(new UsersFilterByCategory
                            {
                                CategoryName = character,
                                Users = _mapper.Map<List<UserDto>>(userOfCategory)
                            });
                        }
                        users.RemoveAll(x => userOfCategory.Contains(x));
                    }
                    if (users.Any())
                    {
                        usersFilterByCategories.Add(new UsersFilterByCategory
                        {
                            CategoryName = "*",
                            Users = _mapper.Map<List<UserDto>>(users)
                        });
                    }

                    break;

                case "uuid":








                    break;

                case "role":
                    var roles = await _roleRepository.Query().ToListAsync();

                    foreach (var role in roles)
                    {
                        var userByRole = new UsersFilterByCategory();
                        var userInRole = await _userRepository.Query().Include(x => x.Roles).ThenInclude(x => x.Role)
                                                                      .Where(x => x.Roles.Select(x => x.Role.Name).Contains(role.Name))
                                                                      .ToListAsync();

                    }


                    break;

                default:
                    if (paramsFilter.Direction == "asc")
                        users = await usersQueryable.OrderBy(x => x.UserName)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize)
                                        .ToListAsync();

                    else
                        users = await usersQueryable.OrderByDescending(x => x.UserName)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize)
                                        .ToListAsync();

                    usersFilterByCategories.Add(new UsersFilterByCategory
                    {
                        CategoryName = "Utilisateurs",
                        Users = _mapper.Map<List<UserDto>>(users)
                    });

                    break;

            }

            var result = new PaginedResult<UsersFilterByCategory>
            {
                Items = usersFilterByCategories,
                PageNumber = paramsFilter.PageNumber,
                PageSize = paramsFilter.PageSize,
                TotalCount = await _userRepository.Query().CountAsync()
            };

            return result;
        }
    }
}
