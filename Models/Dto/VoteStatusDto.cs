using NewSky.API.Models.Enums;

namespace NewSky.API.Models.Dto
{
    public class VoteStatusDto
    {
        public string PlayerUsername { get; set; }
        public VoteWebSite VoteWebSite { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
