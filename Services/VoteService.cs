using NewSky.API.Models;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Enums;
using NewSky.API.Services.Interface;
using NewSky.API.Helpers;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<VoteStatusDto> GetVoteStatus(VoteWebSite voteWebsite)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();
            string userIp = _userService.GetCurrentUserIp();

            var voteStatus = new VoteStatusDto();
            var username = _userService.GetCurrentUserName();
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
                    response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/votes/check?server_token=TTHSXBP2R3HT&playername={username}");

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

        public async Task<ServerStats> GetServerStats(VoteWebSite voteWebSite)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();

            var serverStats = new ServerStats();
            serverStats.VoteWebSite = voteWebSite;

            switch (voteWebSite)
            {
                case VoteWebSite.Serveur_Prive:
                    response = await _httpClient.GetAsync($"https://serveur-prive.net/api/v1/servers/CHsmtiT2M8IJeg4/statistics");
                    content = await response.Content.ReadAsStringAsync();
                    jsonContent = JObject.Parse(content);

                    serverStats.Position = (int)jsonContent["data"]["position"];
                    serverStats.TotalVotes = (int)jsonContent["data"]["votes_count"];
                    serverStats.Rating = (int)jsonContent["data"]["rating"];
                    serverStats.ServerHaveStats = true;

                    return serverStats;

                case VoteWebSite.ServeursMinecraft:
                    serverStats.ServerHaveStats = false;

                    return serverStats;

                case VoteWebSite.Top_Serveurs:
                    response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/servers/TTHSXBP2R3HT/full");
                    content = await response.Content.ReadAsStringAsync();
                    jsonContent = JObject.Parse(content);

                    var currentMonth = DateTime.Today.ToString("MMMM", CultureInfo.InvariantCulture).ToLower().RemoveDiacritics();
                    serverStats.TotalVotes = int.Parse(jsonContent["server"]["last_monthly_stat"].First()[$"{currentMonth}_votes"].ToString());
                    
                    response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/servers/TTHSXBP2R3HT/advices");
                    content = await response.Content.ReadAsStringAsync();
                    jsonContent = JObject.Parse(content);

                    serverStats.Rating = (double)jsonContent["advices"].First()["global"];
                    serverStats.ServerHaveStats = true;

                    return serverStats;

                default:
                    serverStats.ServerHaveStats = false;
                    return serverStats;
            }
        }

        public async Task<bool> TryVoteAsync(VoteWebSite voteWebsite)
        {
            var response = new HttpResponseMessage();
            var content = "";
            var jsonContent = new JObject();
            string userIp = _userService.GetCurrentUserIp();
            var currentUserName = _userService.GetCurrentUserName();
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
                                return false;

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
                                return false;

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
                        response = await _httpClient.GetAsync($"https://api.top-serveurs.net/v1/votes/check?server_token=TTHSXBP2R3HT&playername={currentUserName}");
                        content = await response.Content.ReadAsStringAsync();
                        jsonContent = JObject.Parse(content);


                        if (Regex.IsMatch((string)jsonContent["code"], @"^4[0-9]{2}$"))
                        {
                            if (i == 10)
                                return false;

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
            var line = await _userNumberVoteRepository.Query().FirstOrDefaultAsync(x => x.Username == currentUserName);
            if(line == null)
            {
                var result = await _userNumberVoteRepository.CreateAsync(new UserNumberVote { Username = currentUserName });
                line = result.Entity;
            }

            line.MonthlyVotes += 1;
            line.TotalVotes += 1;
            await _userNumberVoteRepository.UpdateAsync(line, line.Id);

            return true;
        }
    }
}
