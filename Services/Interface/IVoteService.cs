using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IVoteService
    {
        Task<VoteStatusDto> GetVoteStatusAsync(VoteWebSite voteWebSite, string username);
        Task<bool> TryVoteAsync(VoteWebSite voteWebsite, string username);

        Task<RankingResult> GetServerRankingAsync(int limit);
        Task<UserNumberVoteDto> GetUserRankingAsync(string username);
        Task<List<VoteRewardDto>> GetVoteRewardsAsync();

        Task<BaseResult> UpdateVoteRewardsAsync(List<VoteRewardDto> rewardsDto);

        Task<PaginedResult<UserNumberVoteDto>> GetMonthVotesAsync(PaginationFilterParamsDto paginationParams);
        Task<PaginedResult<UserNumberVoteDto>> GetTotalVotesAsync(PaginationFilterParamsDto paginationParams);
    }
}
