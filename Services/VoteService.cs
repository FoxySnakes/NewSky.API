using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Services.Interface;
using NewSky.API.Helpers;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;

namespace NewSky.API.Services
{
    public class VoteService : IVoteService
    {
        private readonly HttpClient _httpClient;
        private readonly IRepository<UserNumberVote> _userNumberVoteRepository;
        private readonly IUserService _userService;

        public VoteService(HttpClient httpClient,
                           IRepository<UserNumberVote> userNumberVoteRepository,
                           IUserService userService)
        {
            _httpClient = httpClient;
            _userNumberVoteRepository = userNumberVoteRepository;
            _userService = userService;
        }

        public async Task<VoteStatusDto> GetVoteStatus(VoteWebSite voteWebsite, string username)
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

        public async Task<TimeSpan> TryVoteAsync(VoteWebSite voteWebsite, string username)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();
            string userIp = _userService.GetCurrentUserIp();
            bool success = false;
            var i = 0;
            TimeSpan timeLeft = TimeSpan.Zero;

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
                                return timeLeft;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                            timeLeft = TimeSpan.FromSeconds(int.Parse((string)jsonContent["data"]["next_vote_seconds"]));
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
                                return timeLeft;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                            timeLeft = TimeSpan.FromSeconds(int.Parse(content));
                        }
                    }

                    break;
                case VoteWebSite.Top_Serveurs:
                    while (success == false)
                    {
                        response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/votes/check?server_token=TTHSXBP2R3HT&playername={username}");
                        content = await response.Content.ReadAsStringAsync();
                        jsonContent = JObject.Parse(content);


                        if (Regex.IsMatch((string)jsonContent["code"], @"^4[0-9]{2}$"))
                        {
                            if (i == 10)
                                return timeLeft;

                            i++;
                            await Task.Delay(6000);
                        }
                        else
                        {
                            success = true;
                            timeLeft = TimeSpan.FromMinutes(int.Parse((string)jsonContent["duration"]));
                        }
                    }

                    break;
                default:
                    throw new Exception($"This website isn't supported yet by our API");

            }
            var line = await _userNumberVoteRepository.Query().FirstOrDefaultAsync(x => x.Username == username);
            if(line == null)
            {
                var result = await _userNumberVoteRepository.CreateAsync(new UserNumberVote { Username = username });
                line = result.Entity;
            }

            line.MonthlyVotes += 1;
            line.TotalVotes += 1;
            await _userNumberVoteRepository.UpdateAsync(line, line.Id);

            return timeLeft;
        }
    }
}
