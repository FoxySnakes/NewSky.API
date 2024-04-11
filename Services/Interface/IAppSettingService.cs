using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IAppSettingService
    {
        Task<dynamic> GetAppSettingsAsync(bool onlyPublic = false);

        Task<BaseResult> UpdateAppSettingsAsync(AppSettingsDto appSettingsDto);
    }
}
