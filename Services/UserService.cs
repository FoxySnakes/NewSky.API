using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Net;

namespace NewSky.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpContextAccessor httpContextAccessor,
                           IRepository<User> userRepository,
                           HttpClient httpClient,
                           IMapper mapper,
                           ILogger<UserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _httpClient = httpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResult> UpdateEmailAsync(string email)
        {
            var user = await GetCurrentUserAsync();
            user.Email = email;
            var result = await _userRepository.UpdateAsync(user.Id);
            var baseResult = new BaseResult()
            {
                Errors = result.Errors.Select(x => x.Message).ToList()
            };
            return baseResult;
        }

        public async Task<BaseResult> UpdateUserUsernameAsync(string userUuid, string userUsername)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.UUID == userUuid);
            if (user == null)
            {
                return new BaseResult() { Errors = new List<string>() { "Aucun Utilisateur trouvé" } };
            }
            var uuid = await GetUserUUIDAsync(userUsername);
            if (uuid == string.Empty)
            {
                return new BaseResult() { Errors = new List<string>() { "Ce nom n'est pas lié à un compte mojang" } };
            }

            user.UserName = userUsername;
            var resultUpdate = await _userRepository.UpdateAsync(user.Id);
            if (!resultUpdate.IsSuccess)
            {
                return new BaseResult() { Errors = resultUpdate.Errors.Select(x => x.Message).ToList() };
            }

            return new BaseResult();
        }

        public async Task<BaseResult> UpdateUserPunishmentAsync(string username, DateTime? banishmentEnd, DateTime? lockoutEnd)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return new BaseResult() { Errors = new List<string>() { "Aucun Utilisateur trouvé" } };
            }
            user.BanishmentEnd = (DateTime)(banishmentEnd == null ? DateTime.Now : banishmentEnd);
            user.LockoutEnd = (DateTime)(lockoutEnd == null ? DateTime.Now : lockoutEnd);
            var resultUpdate = await _userRepository.UpdateAsync(user.Id);

            return new BaseResult() { Errors = resultUpdate.Errors.Select(x => x.Message).ToList() };
        }

        public async Task<User> GetUserByUuidAsync(string uuid, bool includesPackages = false, bool includeRoles = false, bool includePermissions = false)
        {
            var userQuery = QueryUser(includesPackages, includeRoles, includePermissions);

            var user = await userQuery.FirstOrDefaultAsync(x => x.UUID == uuid);

            return user;
        }

        public async Task<User> GetCurrentUserAsync(bool includesPackages = false, bool includeRoles = false, bool includePermissions = false)
        {
            var userQuery = QueryUser(includesPackages, includeRoles, includePermissions);

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
            userIp = "37.171.193.207";
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
                if (response.StatusCode != HttpStatusCode.OK || response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return uuid;
                }

                await Task.Delay(1000);
            }
            while (response.StatusCode == HttpStatusCode.TooManyRequests);

            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);
            if (jsonContent["id"] != null)
            {
                uuid = jsonContent["id"].ToString();
                //uuid = uuid.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
            }
            return uuid;
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

            if(!hasAccess)
                _logger.LogWarning("User {UserName} try to make an action without permission {PermissionName}",user.UserName,permissionName);

            return hasAccess;
        }


        public async Task<PaginedResult<UsersFilterByCategory>> GetUsersFilteredAsync(PaginationFilterParamsDto paramsFilter)
        {
            var usersFilterByCategories = new List<UsersFilterByCategory>();
            var usersQueryable = _userRepository.Query().Include(x => x.Roles).ThenInclude(x => x.Role).AsQueryable();
            var users = new List<User>();
            var alphabetAndNumbers = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


            if (paramsFilter.Search != null)
            {
                usersQueryable = usersQueryable.Where(x => x.UserName.Contains(paramsFilter.Search));
            }

            switch (paramsFilter.Filter)
            {
                case "username":

                    if (paramsFilter.Direction == "asc")
                        users = usersQueryable.ToList().OrderBy(x =>
                                            char.IsLetter(x.UserName[0]) ? 0 :
                                            char.IsDigit(x.UserName[0]) ? 1 :
                                            2)
                                        .ThenBy(x => x.UserName)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize).ToList();

                    else
                    {
                        users = await usersQueryable.OrderByDescending(x => x.UserName)
                                    .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                    .Take(paramsFilter.PageSize)
                                    .ToListAsync();

                        alphabetAndNumbers.Reverse();
                    }

                    foreach (var character in alphabetAndNumbers)
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
                    if (paramsFilter.Direction == "asc")
                        users = usersQueryable.ToList().OrderBy(x =>
                                            char.IsLetter(x.UUID[0]) ? 0 :
                                            char.IsDigit(x.UUID[0]) ? 1 :
                                            2)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize).ToList();

                    else
                    {

                        users = await usersQueryable.OrderByDescending(x => x.UUID)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize)
                                        .ToListAsync();
                        alphabetAndNumbers.Reverse();
                    }
                    foreach (var character in alphabetAndNumbers)
                    {
                        var userOfCategory = users.Where(x => x.UUID.ToLower().StartsWith(character)).ToList();
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

                default:
                    if (paramsFilter.Direction == "asc")
                        users = usersQueryable.ToList().OrderBy(x =>
                                            char.IsLetter(x.UserName[0]) ? 0 :
                                            char.IsDigit(x.UserName[0]) ? 1 :
                                            2)
                                        .ThenBy(x => x.UserName)
                                        .Skip((paramsFilter.PageNumber - 1) * paramsFilter.PageSize)
                                        .Take(paramsFilter.PageSize).ToList();

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

            var totalCount = usersQueryable.ToListAsync().GetAwaiter().GetResult().Count;

            var result = new PaginedResult<UsersFilterByCategory>
            {
                Items = usersFilterByCategories,
                PageNumber = paramsFilter.PageNumber,
                PageSize = paramsFilter.PageSize,
                TotalCount = totalCount,
            };

            return result;
        }

        private IQueryable<User> QueryUser(bool includesPackages = false, bool includeRoles = false, bool includePermissions = false)
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

            return userQuery;
        }
    }
}
