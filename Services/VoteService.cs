using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace NewSky.API.Services
{
    public class VoteService : IVoteService
    {
        private readonly HttpClient _httpClient;
        private readonly IRepository<UserNumberVote> _userNumberVoteRepository;
        private readonly IRepository<VoteReward> _voteRewardRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public VoteService(HttpClient httpClient,
                           IRepository<UserNumberVote> userNumberVoteRepository,
                           IUserService userService,
                           IMapper mapper,
                           IRepository<VoteReward> voteRewardRepository)
        {
            _httpClient = httpClient;
            _userNumberVoteRepository = userNumberVoteRepository;
            _userService = userService;
            _mapper = mapper;
            _voteRewardRepository = voteRewardRepository;
        }

        public async Task<VoteStatusDto> GetVoteStatusAsync(VoteWebSite voteWebsite, string username)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();
            string userIp = _userService.GetCurrentUserIp();

            var voteStatus = new VoteStatusDto();
            voteStatus.PlayerUsername = username;
            voteStatus.VoteWebSite = voteWebsite;

            switch (voteWebsite)
            {
                case VoteWebSite.Serveur_Prive:
                    response = await _httpClient.GetAsync($"https://serveur-prive.net/api/v1/servers/CHsmtiT2M8IJeg4/votes/{userIp}");
                    content = await response.Content.ReadAsStringAsync();
                    jsonContent = JObject.Parse(content);

                    if ((bool)jsonContent["success"] == true)
                    {
                        voteStatus.TimeLeft = TimeSpan.FromSeconds((int)jsonContent["data"]["next_vote_seconds"]);
                    }
                    else
                    {
                        voteStatus.TimeLeft = TimeSpan.Zero;
                    }

                    break;
                case VoteWebSite.ServeursMinecraft:
                    response = await _httpClient.GetAsync($"https://www.serveursminecraft.org/sm_api/peutVoter.php?id=6709&ip={userIp}");
                    content = await response.Content.ReadAsStringAsync();

                    if (content == "true")
                    {
                        voteStatus.TimeLeft = TimeSpan.Zero;
                    }
                    else
                    {
                        voteStatus.TimeLeft = TimeSpan.FromSeconds(int.Parse(content));
                    }

                    break;
                case VoteWebSite.Top_Serveurs:
                    response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/votes/check-ip?server_token=TTHSXBP2R3HT&ip={userIp}");

                    content = await response.Content.ReadAsStringAsync();
                    jsonContent = JObject.Parse(content);

                    if (Regex.IsMatch((string)jsonContent["code"], @"^4[0-9]{2}$"))
                    {
                        voteStatus.TimeLeft = TimeSpan.Zero;
                    }
                    else
                    {
                        voteStatus.TimeLeft = TimeSpan.FromMinutes((int)jsonContent["duration"]);
                    }

                    break;
                default:
                    throw new Exception($"This website isn't supported yet by our API");
            }

            return voteStatus;
        }

        public async Task<bool> TryVoteAsync(VoteWebSite voteWebsite, string username)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();
            string userIp = _userService.GetCurrentUserIp();
            bool success = false;
            var i = 0;

            switch (voteWebsite)
            {
                case VoteWebSite.Serveur_Prive:

                    while (success == false)
                    {
                        response = await _httpClient.GetAsync($"https://serveur-prive.net/api/v1/servers/CHsmtiT2M8IJeg4/votes/{userIp}");
                        content = await response.Content.ReadAsStringAsync();
                        jsonContent = JObject.Parse(content);

                        if ((bool)jsonContent["success"] != true)
                        {
                            if (i == 10)
                                return success;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    break;
                case VoteWebSite.ServeursMinecraft:
                    while (success == false)
                    {
                        response = await _httpClient.GetAsync($"https://www.serveursminecraft.org/sm_api/peutVoter.php?id=6709&ip={userIp}");
                        content = await response.Content.ReadAsStringAsync();

                        if (content == "true")
                        {
                            if (i == 10)
                                return success;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                        }
                    }

                    break;
                case VoteWebSite.Top_Serveurs:
                    while (success == false)
                    {
                        response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/votes/check-ip?server_token=TTHSXBP2R3HT&ip={userIp}");
                        content = await response.Content.ReadAsStringAsync();
                        jsonContent = JObject.Parse(content);

                        if (Regex.IsMatch((string)jsonContent["code"], @"^4[0-9]{2}$"))
                        {
                            if (i == 10)
                                return success;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                        }
                    }

                    break;
                default:
                    throw new Exception($"This website isn't supported yet by our API");

            }
            if (success)
            {
                var userNumberVote = await _userNumberVoteRepository.Query().FirstOrDefaultAsync(x => x.Username == username && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                if (userNumberVote == null)
                {
                    var result = await _userNumberVoteRepository.CreateAsync(new UserNumberVote
                    {
                        Username = username,
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
                        Votes = 1
                    });

                    if (result.IsSuccess)
                        success = true;
                }
                else
                {
                    userNumberVote.Votes++;
                    await _userNumberVoteRepository.UpdateAsync(userNumberVote.Id);
                }
            }

            return success;
        }

        public async Task<RankingResult> GetServerRankingAsync(int limit)
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
            return result;
        }

        public async Task<UserNumberVoteDto> GetUserRankingAsync(string username)
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


            return userRanking;
        }

        public async Task<List<VoteRewardDto>> GetVoteRewardsAsync()
        {
            var rewards = await _voteRewardRepository.Query().ToListAsync();
            var rewardsDto = _mapper.Map<List<VoteRewardDto>>(rewards);
            return rewardsDto;
        }

        public async Task<BaseResult> UpdateVoteRewardsAsync(List<VoteRewardDto> rewardsDto)
        {
            var rewards = await _voteRewardRepository.Query().ToListAsync();
            var result = new BaseResult();
            foreach (var reward in rewardsDto)
            {
                var rewardDB = rewards.FirstOrDefault(x => x.Position == reward.Position);
                if (rewardDB == null)
                {
                    var newReward = _mapper.Map<VoteReward>(reward);
                    var resultCreation = await _voteRewardRepository.CreateAsync(newReward);
                    if (!resultCreation.IsSuccess)
                        result.Errors.Add(resultCreation.Errors.First().Message);
                }
                else if (!rewardDB.Reward.Equals(reward.Reward))
                {
                    rewardDB.Reward = reward.Reward;
                    var resultUpdate = await _voteRewardRepository.UpdateAsync(rewardDB.Position);
                    if (!resultUpdate.IsSuccess)
                        result.Errors.Add(resultUpdate.Errors.First().Message);
                }
            }
            return result;
        }

        public async Task<PaginedResult<UserNumberVoteDto>> GetMonthVotesAsync(PaginationFilterParamsDto paginationParams)
        {

            var month = DateTime.Parse(paginationParams.Search).Month;
            var year = DateTime.Parse(paginationParams.Search).Year;
            var monthVotes = await _userNumberVoteRepository.Query().Where(x => x.Month == month && x.Year == year)
                                                            .Select(x => new UserNumberVoteDto
                                                            {
                                                                Username = x.Username,
                                                                MonthlyVotes = x.Votes,
                                                            })
                                                            .OrderBy(x => x.MonthlyVotes)
                                                            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                                            .Take(paginationParams.PageSize)
                                                            .ToListAsync();

            var totalCount = await _userNumberVoteRepository.Query()
                                                            .Where(x => x.Month == month && x.Year == year)
                                                            .CountAsync();

            return new PaginedResult<UserNumberVoteDto>
            {
                Items = monthVotes,
                TotalCount = totalCount,
                PageSize = paginationParams.PageSize,
                PageNumber = paginationParams.PageNumber
            };
        }

        public async Task<PaginedResult<UserNumberVoteDto>> GetTotalVotesAsync(PaginationFilterParamsDto paginationParams)
        {
            var userNumberVotePagined = new PaginedResult<UserNumberVoteDto>();
            var totalVotes = await _userNumberVoteRepository.Query().GroupBy(x => x.Username)
                                                            .Select(x => new UserNumberVoteDto
                                                            {
                                                                Username = x.Key,
                                                                TotalVotes = x.Sum(y => y.Votes)
                                                            })
                                                            .OrderBy(x => x.TotalVotes)
                                                            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                                            .Take(paginationParams.PageSize)
                                                            .ToListAsync();

            var totalCount = await _userNumberVoteRepository.Query()
                                                .GroupBy(x => x.Username)
                                                .CountAsync();

            return new PaginedResult<UserNumberVoteDto>
            {
                Items = totalVotes,
                TotalCount = totalCount,
                PageSize = paginationParams.PageSize,
                PageNumber = paginationParams.PageNumber
            };
        }
    }
}
