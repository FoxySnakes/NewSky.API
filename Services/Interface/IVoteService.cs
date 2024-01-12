using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;

namespace NewSky.API.Services.Interface
{
    public interface IVoteService
    {
        Task<VoteStatusDto> GetVoteStatus(VoteWebSite voteWebSite, string username);
        Task<TimeSpan> TryVoteAsync(VoteWebSite voteWebsite, string username);
    }
}
