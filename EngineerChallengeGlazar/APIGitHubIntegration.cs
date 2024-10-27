using Databox.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    public class APIGitHubIntegration : IApiIntegration
    {
        private readonly string serviceProvider = "GitHub API";
        public string ServiceProvider { get { return serviceProvider; } }

        private static readonly string ClientId = "Ov23liKdLzVJARntR9Yc";
        private static readonly string ClientSecret = "5881050ef5e19c874690710869b9d4992a15a62e";

        private static readonly string redirectUri = "http://localhost";

        private static readonly string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        private static readonly string TokenEndpoint = "https://github.com/login/oauth/access_token";

        public HttpClient _httpClient;
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }

        public APIGitHubIntegration(string repositoryOwner, string repositoryName)
        {
            _httpClient = new HttpClient();
            RepositoryOwner = repositoryOwner;
            RepositoryName = repositoryName;
        }

        public async Task<List<PushData>> CreatePushData()
        {
            var GitHubMetrics = (APIGithubMetrics)await GetMetrics();

            return new List<PushData>
            {
                new PushData()
                {
                    Key = "Open_Issues",
                    Value = Convert.ToInt32(GitHubMetrics.Open_Issues),
                    Date = DateTime.Now.ToString("o")
                },
                new PushData()
                {
                    Key = "Watchers",
                    Value = Convert.ToInt32(GitHubMetrics.Watchers),
                    Date = DateTime.Now.ToString("o")

                },
                new PushData()
                {
                    Key = "Stargazers",
                    Value = Convert.ToInt32(GitHubMetrics.Stargazers),
                    Date = DateTime.Now.ToString("o")
                }
            };

        }

        public async Task<string> GetData()
        {
            string AccessToken = await GetAccessToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));


            var response = await _httpClient.GetAsync($"https://api.github.com/repos/{RepositoryOwner}/{RepositoryName}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var repoData = JObject.Parse(json);

                int OpenIssues = (int)repoData["open_issues_count"];
                int Watchers = (int)repoData["watchers_count"];
                int Stargazers = (int)repoData["stargazers_count"];

                var metrics = new
                {
                    Open_Issues = OpenIssues,
                    Watchers = Watchers,
                    Stargazers = Stargazers,
                };

                string jsonString = JsonConvert.SerializeObject(metrics);
                return jsonString;
            }

            return string.Empty;
        }

        public async Task<ApiMetrics> GetMetrics()
        {
            string jsonString = await GetData();

            if (jsonString.Contains("error"))
            {
                throw new Exception("Failed to fetch GitHub metrics.");
            }

            var metricsData = JsonConvert.DeserializeObject<APIGithubMetrics>(jsonString);

            APIGithubMetrics metrics = new APIGithubMetrics
            {
                Open_Issues = metricsData.Open_Issues,
                Watchers = metricsData.Watchers,
                Stargazers = metricsData.Stargazers
            };

            return metrics;
        }

        public static async Task<string> RequestAuthorization()
        {
            string AuthorizationUrl = $"{AuthorizationEndpoint}?client_id={ClientId}&redirect_uri={redirectUri}&scope=repo";

            Console.WriteLine("Open the following URL in your browser and authorize the application:");
            Console.WriteLine(AuthorizationUrl);
            Console.WriteLine("Enter the code from the URL after authorization and redirection (Example-http://localhost/?code=920xxxxxxxxxxxxxxxxx):");

            string AuthorizationCode = Console.ReadLine();

            if (!string.IsNullOrEmpty(AuthorizationCode))
            {
                return AuthorizationCode;
            }
            else
            {
                return "";
            }
        }

        public async Task<string> GetAccessToken()
        {
            string AuthorizationCode = await RequestAuthorization();

            var AccessTokenRequestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("code", AuthorizationCode),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            ]);

            var tokenResponse = await _httpClient.PostAsync(TokenEndpoint, AccessTokenRequestData);
            var responseString = await tokenResponse.Content.ReadAsStringAsync();

            var queryParams = System.Web.HttpUtility.ParseQueryString(responseString);

            string accessToken = queryParams["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }
            else
            {
                Console.WriteLine("Error obtaining access token");
                return "";
            }
        }
    }
}
