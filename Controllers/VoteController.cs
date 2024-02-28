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
        public async Task<IActionResult> GetAllVoteStatus([FromQuery] string username)
        {
            var voteStatus1 = await _voteService.GetVoteStatus(VoteWebSite.Serveur_Prive, username);
            var voteStatus2 = await _voteService.GetVoteStatus(VoteWebSite.ServeursMinecraft, username);
            var voteStatus3 = await _voteService.GetVoteStatus(VoteWebSite.Top_Serveurs, username);

            var voteStatusList = new List<VoteStatusDto> { voteStatus1, voteStatus2, voteStatus3 };

            return Ok(voteStatusList);
        }

        [HttpGet("server-ranking")]
        public async Task<IActionResult> GetServerRanking([FromQuery] int limit = 10)
        {
            var monthlyTop = await _userNumberVoteRepository.Query().Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year)
                                                                    .OrderByDescending(x => x.Votes)
                                                                    .Take(limit)
                                                                    .Select(x => new UserNumberVoteDto
                                                                    {
                                                                        Username = x.Username,
                                                                        MonthlyVotes = x.Votes
                                                                    })
                                                                    .ToListAsync();

            var totalTop = await _userNumberVoteRepository.Query().GroupBy(x => x.Username)
                                                          .Select(group => new UserNumberVoteDto
                                                          {
                                                              Username = group.Key,
                                                              TotalVotes = group.Sum(x => x.Votes),
                                                          })
                                                         .ToListAsync();

            var result = new RankingResult()
            {
                MonthlyTop = _mapper.Map<List<UserNumberVoteDto>>(monthlyTop),
                TotalTop = totalTop
            };
            return Ok(result);
        }

        [HttpGet("user-ranking")]
        public async Task<IActionResult> GetUserRanking([FromQuery] string username)
        {
            var ranking = await _userNumberVoteRepository.Query().ToListAsync();
            var userVotesNumber = ranking.Where(x => x.Username == username);
            var userRanking = new UserNumberVoteDto() { Username = username };
            if (userVotesNumber.Any())
            {
                var monthlyVotes = userVotesNumber.FirstOrDefault(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                var monthlyTop = ranking.Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year)
                                                     .OrderByDescending(x => x.Votes)
                                                     .ToList();
                var totalTop = ranking.GroupBy(x => x.Username)
                                      .OrderByDescending(group => group.Sum(x => x.Votes))
                                      .ToList();

                userRanking.MonthlyVotes = monthlyVotes == null ? 0 : monthlyVotes.Votes;
                userRanking.TotalVotes = userVotesNumber.Sum(x => x.Votes);
                userRanking.MonthlyPosition = monthlyTop.FindIndex(x => x.Username == username) == -1 ? monthlyTop.Count() + 1 : monthlyTop.FindIndex(x => x.Username == username) + 1;
                userRanking.TotalPosition = totalTop.FindIndex(x => x.Key == username) == -1 ? totalTop.Count() + 1 : totalTop.FindIndex(group => group.Key == username) + 1;
            }
            else
            {
                userRanking.MonthlyVotes = 0;
                userRanking.TotalVotes = 0;
                userRanking.MonthlyPosition = ranking.Where(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).OrderByDescending(x => x.Votes).ToList().Count + 1;
                userRanking.TotalPosition = ranking.GroupBy(x => x.Username).Count() + 1;
            }


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
        public async Task<IActionResult> TryVoteAsync(VoteWebSite voteWebsite, [FromQuery] string username)
        {
            var result = await _voteService.TryVoteAsync(voteWebsite, username);

            return Ok(result);
        }
    }
}
