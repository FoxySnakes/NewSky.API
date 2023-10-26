using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Models.Result;
using NewSky.API.Services;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IRepository<UserNumberVote> _userNumberVoteRepository;
        private readonly IRepository<VoteReward> _voteRewardRepository;
        private readonly IMapper _mapper;

        public VoteController(IVoteService voteService,
                              IRepository<UserNumberVote> userNumberVoteRepository,
                              IRepository<VoteReward> voteRewardRepository,
                              IMapper mapper)
        {
            _voteService = voteService;
            _userNumberVoteRepository = userNumberVoteRepository;
            _voteRewardRepository = voteRewardRepository;
            _mapper = mapper;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetAllVoteStatus()
        {
            var voteStatus1 = await _voteService.GetVoteStatus(VoteWebSite.Serveur_Prive);
            var voteStatus2 = await _voteService.GetVoteStatus(VoteWebSite.ServeursMinecraft);
            var voteStatus3 = await _voteService.GetVoteStatus(VoteWebSite.Top_Serveurs);

            var voteStatusList = new List<VoteStatusDto> { voteStatus1, voteStatus2, voteStatus3 };

            return Ok(voteStatusList);
        }

        [HttpGet("status/{voteWebsite}")]
        public async Task<IActionResult> GetVoteStatusByWebsite(VoteWebSite voteWebsite)
        {
            var voteStatus = await _voteService.GetVoteStatus(voteWebsite);
            return Ok(voteStatus);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetServerStats(VoteWebSite voteWebSite)
        {
            var serverStats = await _voteService.GetServerStats(voteWebSite);

            return Ok(serverStats);
        }

        [HttpGet("ranking")]
        public async Task<IActionResult> GetServerRanking([FromQuery] int limit = 10)
        {
            var monthlyTop = await _userNumberVoteRepository.Query().OrderByDescending(x => x.MonthlyVotes).Take(limit).ToListAsync();
            var totalTop = await _userNumberVoteRepository.Query().OrderByDescending(x => x.TotalVotes).Take(limit).ToListAsync();
            var result = new RankingResult()
            {
                MonthlyTop = _mapper.Map<List<UserNumberVoteDto>>(monthlyTop),
                TotalTop = _mapper.Map<List<UserNumberVoteDto>>(totalTop)
            };
            return Ok(result);
        }

        [HttpGet("ranking/{username}")]
        public async Task<IActionResult> GetUserRanking(string username)
        {
            var ranking = await _userNumberVoteRepository.Query().ToListAsync();
            var userVotesNumber = ranking.Find(x => x.Username == username);
            var userRanking = new UserNumberVoteDto()
            {
                Username = username,
                MonthlyVotes = userVotesNumber.MonthlyVotes,
                TotalVotes = userVotesNumber.TotalVotes,
                MonthlyPosition = ranking.OrderByDescending(x => x.MonthlyVotes).ToList().IndexOf(userVotesNumber) + 1,
                TotalPosition = ranking.OrderByDescending(x => x.TotalVotes).ToList().IndexOf(userVotesNumber) + 1,
            };

            return Ok(userRanking);
        }

        [HttpGet("rewards")]
        public async Task<IActionResult> GetVoteRewards()
        {
            var rewards = await _voteRewardRepository.Query().ToListAsync();
            var rewardsDto = _mapper.Map<List<VoteRewardDto>>(rewards);
            return Ok(rewardsDto);
        }

        [HttpGet("{voteWebsite}")]
        public async Task<IActionResult> TryVoteAsync(VoteWebSite voteWebsite)
        {
            var result = await _voteService.TryVoteAsync(voteWebsite);

            return Ok(result);
        }
    }
}
