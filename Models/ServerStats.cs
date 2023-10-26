using NewSky.API.Models.Enums;

namespace NewSky.API.Models
{
    public class ServerStats
    {
        public VoteWebSite VoteWebSite { get; set; }
        public int TotalVotes { get; set; } = -1;
        public int Position { get; set; } = -1;
        public double Rating { get; set; } = -1;
        public bool ServerHaveStats { get; set; }
    }
}
