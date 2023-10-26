namespace NewSky.API.Models
{
    public class UserNumberVote : EntityBase
    {
        public string Username { get; set; }
        public int MonthlyVotes { get; set; }
        public int TotalVotes { get; set; }
    }
}
