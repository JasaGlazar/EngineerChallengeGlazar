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
        public string APIToken { get; set; }

        public DataboxIntegration(HttpClient httpClient, string aPIToken)
        {
            _httpClient = httpClient;
            APIToken = aPIToken;
        }

        public async Task SendMetrics(IApiIntegration apiIntegration)
        {
            Configuration config = new Configuration();
            config.BasePath = "https://push.databox.com";
            config.Username = APIToken;
            config.DefaultHeaders.Add("Accept", "application/vnd.databox.v2+json");

            HttpClientHandler handler = new HttpClientHandler();

            var apiInstance = new DefaultApi(_httpClient, config, handler);
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
            }
        }
    }
}
