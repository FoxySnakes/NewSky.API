namespace NewSky.API.Models.Dto
{
    public class UserNumberVoteDto
    {
        public string Username { get; set; }
        public int MonthlyVotes { get; set; }
        public int TotalVotes { get; set; }
        public int? MonthlyPosition { get; set; } = null;
        public int? TotalPosition { get; set; } = null;
    }
}
