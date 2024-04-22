using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        private readonly IAppSettingService _appSettingService;
        public AppSettingController(IAppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetAppSettingsPublic()
        {
            var appSettingsPublic = await _appSettingService.GetAppSettingsAsync(true);
            return Ok(appSettingsPublic);
        }

        [HttpGet]
        [Permission(PermissionName.AccessToGeneralSettingsOnAdminPanel)]
        public async Task<IActionResult> GetAppSettings()
        {
            var appSettings = await _appSettingService.GetAppSettingsAsync();
            return Ok(appSettings);
        }

        [HttpPost("update")]
        [Permission(PermissionName.UpdateGeneralSettings)]
        public async Task<IActionResult> UpdateAppSettings([FromBody] AppSettingsDto appSettingsDto)
        {
            var result = await _appSettingService.UpdateAppSettingsAsync(appSettingsDto);
            return Ok(result);
        }
    }
}
