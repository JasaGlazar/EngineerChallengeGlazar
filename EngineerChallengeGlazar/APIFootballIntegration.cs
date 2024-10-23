using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    internal class APIFootballIntegration
    {
        public APIFootballResponse APIFootballResponse { get; set; }

        private HttpClient _httpClient;
        private readonly string apiUrl = "https://v3.football.api-sports.io/players";
        private readonly string apiKey = "548a580a913d6ff1d434bfd16128a28b";

        public string playerId { get; set; }
        public string leagueId { get; set; }
        public string season { get; set; }

        public APIFootballIntegration(HttpClient httpClient, string playerId, string leagueId, string season)
        {
            _httpClient = httpClient;
            this.playerId = playerId;
            this.leagueId = leagueId;
            this.season = season;
        }
        public async Task<string> GetData()
        {
            string fullUrl = BuildUrlQuery();
            using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Add("x-apisports-key", apiKey);
            request.Headers.Add("x-rapidapi-host", "v3.football.api-sports.io");

            using var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                throw new Exception("Error: " + response.StatusCode);
            }
        }
        public async Task<APIFootballMetrics> GetMetrics()
        {
            string jsonData = await GetData();

            JObject jobject = JObject.Parse(jsonData);

            var playerStats = jobject["response"]?[0]?["statistics"]?[0];

            int? totalGoals = (int)playerStats?["goals"]?["total"];
            int? totalAssists = (int)playerStats?["goals"]?["assists"];
            int? totalAppearances = (int)playerStats?["games"]?["appearances"];
            int? foulsCommitted = (int)playerStats?["fouls"]?["committed"];
            int? season = (int)playerStats?["league"]?["season"];

            APIFootballMetrics metrics = new APIFootballMetrics
            {
                Goals = totalGoals ?? 0,
                Assists = totalAssists ?? 0,
                Appearences = totalAppearances ?? 0,
                Fouls_Committed = foulsCommitted ?? 0,
                Season = season ?? 0
            };

        }

        public Dictionary<string, string> createQueryParamsDict() => new Dictionary<string, string>
            {
                {"id", playerId},
                {"league", leagueId},
                {"season", season}
            };


        public string BuildUrlQuery()
        {
            var queryParams = createQueryParamsDict();
            var uriBuilder = new UriBuilder(apiUrl)
            {
                Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
            };

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
