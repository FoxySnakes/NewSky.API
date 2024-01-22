namespace NewSky.API.Models.Db
{
    public class UserNumberVote : EntityBase
    {
        public string Username { get; set; }
        public int Votes { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
