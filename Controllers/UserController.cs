using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Extensions;
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
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                              IRepository<User> userRepository,
                              IRepository<Role> roleRepository,
                              IMapper mapper)
        {
            _userService = userService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            var userDto = new UserDto()
            {
                UserName = user.UserName,
                UUID = user.UUID,
                Email = user.Email,
                Roles = user.Roles.Select(x => x.Role.Name).ToList()
            };
            return Ok(userDto);
        }

        [HttpPost("update-email")]
        public async Task<IActionResult> UpdateUser(UpdateEmailDto updateModel)
        {
            var user = await _userService.GetCurrentUserAsync();
            var mappedUser = _mapper.Map(updateModel, user);
            var result = await _userRepository.UpdateAsync(mappedUser, mappedUser.Id);
            var baseResult = new BaseResult()
            {
                Success = result.IsSuccess,
                Errors = result.Errors.Select(x => x.Message).ToList()
            };
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationFilterParamsDto paginationFilterParams)
        {
            var usersQuery = _userRepository.Query().Where(x => x.UserName == paginationFilterParams.Search)
                                                    .Include(x => x.Roles).ThenInclude(x => x.Role)
                                                    .AsQueryable();

            switch (paginationFilterParams.Filter)
            {
                case "username":
                    if (paginationFilterParams.Direction == "asc")
                        usersQuery = usersQuery.OrderBy(x => x.UserName);
                    else
                        usersQuery = usersQuery.OrderByDescending(x => x.UserName);
                    break;

                case "uuid":
                    if (paginationFilterParams.Direction == "asc")
                        usersQuery = usersQuery.OrderBy(x => x.UUID);
                    else
                        usersQuery = usersQuery.OrderByDescending(x => x.UUID);
                    break;

                case "role":
                    var userByRole = new UserByRoleDto();
                    var roles = await _roleRepository.Query().ToListAsync();
                    var usersRole = _userRepository.Query().Include(x => x.Roles).ThenInclude(x => x.Role).GroupBy(x => x.Roles.Select(r => r.Role.Name)).ToList();
                    var x = "";
                    return Ok(usersRole);


            }
            usersQuery = usersQuery.Skip((paginationFilterParams.PageNumber - 1) * paginationFilterParams.PageSize).Take(paginationFilterParams.PageSize);
            var users = await usersQuery.ToListAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }
    }
}
