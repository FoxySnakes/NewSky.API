namespace NewSky.API.Models.Db
{
    public class UserNumberVote : EntityBaseWithId
    {
        public string Username { get; set; }
        public int Votes { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
