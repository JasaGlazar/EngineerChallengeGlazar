using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databox.Api;
using Databox.Client;
using Databox.Model;
using Microsoft.Extensions.Logging;

namespace EngineerChallengeGlazar
{
    internal class DataboxIntegration
    {
        private HttpClient _httpClient;
        private Configuration _config;
        private readonly ILogger<DataboxIntegration> _logger;

        public string APIToken { get; set; }

        public DataboxIntegration(string aPIToken, ILogger<DataboxIntegration> logger)
        {
            _httpClient = new HttpClient();
            APIToken = aPIToken;

            _config = new Configuration();
            _config.BasePath = "https://push.databox.com";
            _config.Username = APIToken;
            _config.DefaultHeaders.Add("Accept", "application/vnd.databox.v2+json");

            _logger = logger;


        }

        public async Task SendMetrics(IApiIntegration apiIntegration)
        {
            HttpClientHandler handler = new HttpClientHandler();

            var apiInstance = new DefaultApi(_httpClient, _config, handler);
            var dataPostRequest = apiIntegration.CreatePushData();

            var serviceProviderName = apiIntegration.ServiceProvider;
            int numberOfMetrics = dataPostRequest.Result.Count;
            string timeOfSending = DateTime.Now.ToString();

            string metricsInfo = string.Format("Sending metrics for provider: {0} at {1}", serviceProviderName, timeOfSending);
            _logger.LogInformation(metricsInfo);
            WriteLogToFile(metricsInfo);


            foreach (var data in dataPostRequest.Result)
            {
                string metricsLog = string.Format("Metric Sent - Key: {0}, Value: {1}, Date: {2}", data.Key, data.Value, data.Date);
                _logger.LogDebug(metricsLog);
                WriteLogToFile(metricsLog);
            }

            try
            {
                var response = await apiInstance.DataPostWithHttpInfoAsync(dataPostRequest.Result);
                string successMessage = string.Format("Successfully sent {0} metrics for provider: {1} at {2}", 
                numberOfMetrics, serviceProviderName, timeOfSending);
                _logger.LogInformation(successMessage);
                WriteLogToFile(successMessage);
            }
            catch (ApiException ex)
            {
                //Console.WriteLine("Exception when calling DefaultApi.DataPostWithHttpInfo: " + ex.Message);
                //Console.WriteLine("Status Code: " + ex.ErrorCode);
                //Console.WriteLine(ex.StackTrace);
                //Console.WriteLine(ex.Source);
                //Console.WriteLine(ex.ErrorContent);
                string errorMessage = string.Format("Failed to send metrics for provider: {0} at {1}. Error Code: {2}, Message: {3}, Content: {4}",
                    serviceProviderName, timeOfSending, ex.ErrorCode, ex.Message, ex.ErrorContent);
                _logger.LogError(errorMessage);
                WriteLogToFile(errorMessage);
            }
        }

        public void WriteLogToFile(string logMessage, string filePath = @".\SendMetricsLog.txt")
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.UtcNow}: {logMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log to file: {ex.Message}");
            }
        }
    }
}
