using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetAllVoteStatus([FromQuery] string username)
        {
            var voteStatus1 = await _voteService.GetVoteStatusAsync(VoteWebSite.Serveur_Prive, username);
            var voteStatus2 = await _voteService.GetVoteStatusAsync(VoteWebSite.ServeursMinecraft, username);
            var voteStatus3 = await _voteService.GetVoteStatusAsync(VoteWebSite.Top_Serveurs, username);

            var voteStatusList = new List<VoteStatusDto> { voteStatus1, voteStatus2, voteStatus3 };

            return Ok(voteStatusList);
        }

        [HttpGet("server-ranking")]
        public async Task<IActionResult> GetServerRanking([FromQuery] int limit = 10)
        {
            var serverRanking = await _voteService.GetServerRankingAsync(limit);
            return Ok(serverRanking);
        }

        [HttpGet("user-ranking")]
        public async Task<IActionResult> GetUserRanking([FromQuery] string username)
        {
            var userRanking = await _voteService.GetUserRankingAsync(username);
            return Ok(userRanking);
        }

        [HttpGet("rewards")]
        public async Task<IActionResult> GetVoteRewards()
        {
            var voteRewards = await _voteService.GetVoteRewardsAsync();
            return Ok(voteRewards);
        }

        [HttpPost("update-rewards")]
        public async Task<IActionResult> UpdateRewards([FromBody] List<VoteRewardDto> rewardsDto)
        {
            var result = await _voteService.UpdateVoteRewardsAsync(rewardsDto);
            return Ok(result);
        }

        [HttpGet("{voteWebsite}")]
        public async Task<IActionResult> TryVoteAsync(VoteWebSite voteWebsite, [FromQuery] string username)
        {
            var result = await _voteService.TryVoteAsync(voteWebsite, username);
            return Ok(result);
        }

        [HttpGet("month")]
        [Permission(PermissionName.AccessToVotesOnAdminPanel)]
        public async Task<IActionResult> GetMonthVotes([FromQuery] PaginationFilterParamsDto paginationParams)
        {
            var rankingPagined = await _voteService.GetMonthVotesAsync(paginationParams);
            return Ok(rankingPagined);
        }

        [HttpGet("total")]
        [Permission(PermissionName.AccessToVotesOnAdminPanel)]
        public async Task<IActionResult> GetTotalVotes([FromQuery] PaginationFilterParamsDto paginationParams)
        {
            var rankingPagined = await _voteService.GetTotalVotesAsync(paginationParams);
            return Ok(rankingPagined);
        }
    }
}
