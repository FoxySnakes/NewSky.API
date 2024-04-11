using NewSky.API.Models.Db;

namespace NewSky.API.Models.Dto
{
    public class AppSettingsDto
    {
        // Social Medias
        public string DiscordUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string TikTokUrl { get; set; }

        // Key Tebex
        public string WebStoreIdentifier { get; set; }
        public string XTebexSecret { get; set; }

        // Server
        public string ServerIp { get; set; }
    }
}
