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

        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            List<string> userNames = new List<string> { "LilKiss", "yvue", "coliy", "lyg", "SpoconyBogdzio", "stupkara", "fruitchii", "pwinkcat", "valwie", "ProrabVasa", "FlxMNS", "MmeBonnie", "Nimie", "SunieFlower", "SYUNG_", "str0b", "shoyuuki", "Lunalabest", "NinkoMS", "Amistosa", "HE4RT__", "MrFr0sty_", "kubix13ksavi", "natalka2005", "ARTINEY", "Nevelis", "I_Love_M_", "WavyLOVE", "whezz", "aeebo🇺🇦", "Sereign", "papierka3", "Nxyt", "svitxhblvde", "zywool", "Porzekson", "Pqinite_", "Torrence", "NACr1ng0", "Xenan", "Kusiciel", "Latosz3k", "elicetrem", "Luvonos", "ziolo_", "ihateazentify", "MegLovesMen", "50kp", "strwngled", "j_juliaxlz", "Atypowaa", "ifyoucanthang", "BabyYoda25", "kox9", "sillyella", "_p0cisz01", "Encerrada", "kittysnowie", "sus1duck", "BialyTravis", "Gabsonzabson", "LanaDeelRey", "DroppedByBabcia", "FaZZZow", "supervillaincat", "Omgitsh3cz", "g0r3magala", "Katyx7", "rwsario", "Neereene", "HYDHyper", "irrevocableBorn", "bwored", "emilsia", "PoppedBy1Stif_", "_vDelicja_", "klaeew🇺🇦", "vumt", "Proroczek", "piwnybrzuch", "Traumado", "Joqnny", "bubbLyPrototype", "Rozprowacz22", "BaIenciaga", "parlaementt", "Enzo2thiccc", "meilia", "Pin_girlnwn", "shaniayanofc", "saikin7", "KAPABAH", "BabiVenom", "habilitei", "bobiik__", "cqrlaaa", "Bri4n44", "Rehired", "Bambi_official", "KittenCrystals", "Abruwult", "KaiCenat_Skibidi", "harlota", "Nataila123", "zayvunn", "YuRey_", "rtwz", "Canecoca", "20420", "arinacuki26", "murdlali", "eziara", "Wshanaoo_", "sayoniaa", "Grqved", "012012012", "Tabbeh", "Kat0Key", "PRINCESAvIOlETTA", "Gh0stieTheSilly", "Nyrvie0", "lemonstagistaken", "asythj", "MOMMYSADIE", "lenofaq_", "yskzzz", "ekitten_tamer", "Sxtanic" };

            var success = 0;
            for (int i = 0; i < userNames.Count; i++)
            {
                var result = await _authService.RegisterAccountAsync(new RegisterDto
                {
                    Email = $"{userNames[i]}@test.fr",
                    Password = "Test1234",
                    UserName = userNames[i]
                });
                if (result.IsSuccess)
                    success++;
            }
            return Ok($"{success} / {userNames.Count()}");
        }
    }
}
