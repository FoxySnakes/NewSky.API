using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;

namespace NewSky.API.Services.Interface
{
    public interface IVoteService
    {
        Task<VoteStatusDto> GetVoteStatus(VoteWebSite voteWebSite);
        Task<ServerStats> GetServerStats(VoteWebSite voteWebSite);
        Task<bool> TryVoteAsync(VoteWebSite voteWebsite);
    }
}
