using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace NewSky.API.Models.Db
{
    public class AppSetting : EntityBaseWithId
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public static class AppSettingDefault
    {
        // Social Medias
        public static string DiscordUrl = "";
        public static string InstagramUrl = "";
        public static string YoutubeUrl = "";
        public static string TwitterUrl = "";
        public static string TikTokUrl = "";

        // Key Tebex
        public static string WebStoreIdentifier = "q9p1-a1dc8b36df104edf3ec62d84d3851a78ef5dcf51";
        public static string XTebexSecret = "3421979d0b799fe1733ce64ad591b28d3bc86b77";

        // Server
        public static string ServerIp = "";
    }
}
