using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using System.Collections.Immutable;

namespace NewSky.API.Services
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IRepository<AppSetting> _appSettingRepository;
        private ImmutableDictionary<string, string> _appSettings;

        public AppSettingService(IRepository<AppSetting> appSettingRepository)
        {
            _appSettingRepository = appSettingRepository;
            LoadSettings();
        }

        public async Task<dynamic> GetAppSettingsAsync(bool onlyPublic = false)
        {
            var appSettings = await _appSettingRepository.Query().ToListAsync();
            if (onlyPublic)
                return new AppSettingsPublicDto()
                {
                    DiscordUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.DiscordUrl))?.Value,
                    InstagramUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.InstagramUrl))?.Value,
                    YoutubeUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.YoutubeUrl))?.Value,
                    TwitterUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.TwitterUrl))?.Value,
                    TikTokUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.TikTokUrl))?.Value,
                    ServerIp = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.ServerIp))?.Value
                };

            else
                return new AppSettingsDto()
                {
                    DiscordUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.DiscordUrl))?.Value,
                    InstagramUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.InstagramUrl))?.Value,
                    YoutubeUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.YoutubeUrl))?.Value,
                    TwitterUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.TwitterUrl))?.Value,
                    TikTokUrl = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.TikTokUrl))?.Value,
                    WebStoreIdentifier = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.WebStoreIdentifier))?.Value,
                    XTebexSecret = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.XTebexSecret))?.Value,
                    ServerIp = appSettings.FirstOrDefault(x => x.Name == nameof(AppSettingsDto.ServerIp))?.Value
                };
        }

        public async Task<BaseResult> UpdateAppSettingsAsync(AppSettingsDto appSettingsDto)
        {
            var result = new BaseResult();

            foreach (var property in typeof(AppSettingsDto).GetProperties())
            {
                var currentValue = _appSettings.FirstOrDefault(x => x.Key == property.Name).Value;
                var newValue = property.GetValue(appSettingsDto) as string;
                if (currentValue != newValue)
                {
                    var appSettingDb = _appSettingRepository.Query().FirstOrDefault(x => x.Name == property.Name);
                    appSettingDb.Value = newValue;
                    var resultUpdate = await _appSettingRepository.UpdateAsync(appSettingDb.Id);
                    if (!resultUpdate.IsSuccess)
                        result.Errors.Add(resultUpdate.Errors.First().Message);
                }
            }

            return result;
        }

        private void LoadSettings()
        {
            _appSettings = _appSettingRepository.Query().ToImmutableDictionary(setting => setting.Name, setting => setting.Value);
        }
    }
}
