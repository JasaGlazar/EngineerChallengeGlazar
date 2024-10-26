using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Databox.Api;
using Databox.Client;
using Databox.Model;

namespace EngineerChallengeGlazar
{
    internal class APIFootballIntegration : IApiIntegration
    {
        public APIFootballResponse APIFootballResponse { get; set; }

        private HttpClient _httpClient;
        private readonly string apiUrl = "https://v3.football.api-sports.io/players";
        private readonly string apiKey = "548a580a913d6ff1d434bfd16128a28b";

        public string PlayerId { get; set; }
        public string LeagueId { get; set; }
        public string Season { get; set; }

        public APIFootballIntegration(string playerId, string leagueId, string season)
        {
            _httpClient = new HttpClient();
            PlayerId = playerId;
            LeagueId = leagueId;
            Season = season;
        }
        public async Task<string> GetData()
        {
            try
            {
                string fullUrl = BuildUrlQuery();

                using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("x-apisports-key", apiKey);
                request.Headers.Add("x-rapidapi-host", "v3.football.api-sports.io");

                using var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error reading the response content: " + ex.Message, ex);
                    }
                }
                else
                {
                    throw new Exception("Error: Request failed with status code " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error occurred while making the API request: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while making the API request: " + ex.Message, ex);
            }
        }
        public async Task<ApiMetrics> GetMetrics()
        {
            try
            {
                string jsonData = await GetData();

                try
                {
                    JObject jobject = JObject.Parse(jsonData);

                    var playerStats = jobject["response"]?[0]?["statistics"]?[0];
                    if (playerStats == null)
                    {
                        throw new Exception("Player statistics not found in the response.");
                    }

                    int? season = (int?)playerStats["league"]?["season"];
                    int? totalGoals = (int?)playerStats["goals"]?["total"];
                    int? totalAssists = (int?)playerStats["goals"]?["assists"];
                    int? totalAppearances = (int?)playerStats["games"]?["appearences"];
                    int? foulsCommitted = (int?)playerStats["fouls"]?["committed"];
                    double? rating = (double?)playerStats["games"]?["rating"];

                    APIFootballMetrics metrics = new APIFootballMetrics
                    {
                        Goals = totalGoals ?? 0,
                        Assists = totalAssists ?? 0,
                        Appearences = totalAppearances ?? 0,
                        Fouls_committed = foulsCommitted ?? 0,
                        Season = season ?? 0,
                        Rating = rating ?? 0
                    };

                    return metrics;
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    throw new Exception("Error parsing JSON response: " + ex.Message, ex);
                }
                catch (InvalidCastException ex)
                {
                    throw new Exception("Error converting data types from JSON response: " + ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching metrics: " + ex.Message, ex);
            }
        }

        public async Task<List<PushData>> CreatePushData()
        {
            var PlayerMetrics = (APIFootballMetrics)await GetMetrics();
            string Date = $"{PlayerMetrics.Season}-07-01T00:00:00Z";

            return new List<PushData>
            {
                new PushData()
                {
                    Key = "Goals",
                    Value = PlayerMetrics.Goals,
                    Date = Date,
                },
                new PushData()
                {
                    Key = "Assists",
                    Value = PlayerMetrics.Assists,
                    Date = Date
                },
                new PushData()
                {
                    Key = "Appearences",
                    Value = PlayerMetrics.Appearences,
                    Date = Date
                },
                new PushData()
                {
                    Key = "Fouls_committed",
                    Value = PlayerMetrics.Fouls_committed,
                    Date = Date
                },
                new PushData()
                {
                    Key = "Rating",
                    Value = (float)PlayerMetrics.Rating,
                    Date = Date
                }
            };
        }

        public Dictionary<string, string> CreateQueryParamsDict() => new Dictionary<string, string>
            {
                {"id", PlayerId},
                {"league", LeagueId},
                {"season", Season}
            };


        public string BuildUrlQuery()
        {
            var queryParams = CreateQueryParamsDict();
            var uriBuilder = new UriBuilder(apiUrl)
            {
                Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
            };

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
