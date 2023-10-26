using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class RankingResult
    {
        public List<UserNumberVoteDto> MonthlyTop { get; set; }
        public List<UserNumberVoteDto> TotalTop { get; set; }
    }
}
