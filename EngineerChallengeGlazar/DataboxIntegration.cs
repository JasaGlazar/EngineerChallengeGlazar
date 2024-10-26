using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databox.Api;
using Databox.Client;
using Databox.Model;

namespace EngineerChallengeGlazar
{
    internal class DataboxIntegration
    {
        private HttpClient _httpClient;
        private Configuration _config;
        public string APIToken { get; set; }

        public DataboxIntegration(string aPIToken)
        {
            _httpClient = new HttpClient();
            APIToken = aPIToken;

            _config = new Configuration();
            _config.BasePath = "https://push.databox.com";
            _config.Username = APIToken;
            _config.DefaultHeaders.Add("Accept", "application/vnd.databox.v2+json");

        }

        public async Task SendMetrics(IApiIntegration apiIntegration)
        {
            HttpClientHandler handler = new HttpClientHandler();

            var apiInstance = new DefaultApi(_httpClient, _config, handler);
            var dataPostRequest = apiIntegration.CreatePushData();

            try
            {
                var response = await apiInstance.DataPostWithHttpInfoAsync(dataPostRequest.Result);
                Console.WriteLine(response.Data.ToString());
            }
            catch (ApiException ex)
            {
                Console.WriteLine("Exception when calling DefaultApi.DataPostWithHttpInfo: " + ex.Message);
                Console.WriteLine("Status Code: " + ex.ErrorCode);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.ErrorContent);
            }
        }
    }
}
