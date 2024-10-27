using EngineerChallengeGlazar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace TestProject1
{
    [TestClass]
    public class APIFootballIntegrationTests
    {
        private HttpClient _httpClient;

        [TestInitialize]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        private APIFootballIntegration CreateAPIFootballIntegrationInstance()
        {
            return new APIFootballIntegration("522", "78", "2020")
            {
                _httpClient = _httpClient // Assign the mocked HttpClient
            };
        }

        [TestMethod]
        public void BuildUrlQuery_ShouldReturnCorrectUrl()
        {
            var integration = CreateAPIFootballIntegrationInstance();
            var expectedUrl = "https://v3.football.api-sports.io/players?id=522&league=78&season=2020";

            var actualUrl = integration.BuildUrlQuery();

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [TestMethod]
        public async Task GetMetrics_ShouldReturnValidMetrics_WhenResponseIsValid()
        {
            var integration = CreateAPIFootballIntegrationInstance();
            var metrics = (APIFootballMetrics)await integration.GetMetrics();

            Assert.AreEqual(11, metrics.Goals);
            Assert.AreEqual(18, metrics.Assists);
            Assert.AreEqual(32, metrics.Appearences);
            Assert.AreEqual(22, metrics.Fouls_committed);
            Assert.AreEqual(7.38, Math.Round(metrics.Rating,2));
        }

        [TestMethod]
        public async Task CreatePushData_ShouldReturnCorrectPushData()
        {
            var integration = CreateAPIFootballIntegrationInstance();
            var pushData = await integration.CreatePushData();

            Assert.AreEqual(5, pushData.Count);
            Assert.IsTrue(pushData.Exists(p => p.Key == "Goals" && (int)p.Value == 11));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Assists" && (int)p.Value == 18));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Appearences" && (int)p.Value == 32));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Fouls_committed" && (int)p.Value == 22));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Rating" && Math.Round((float)p.Value,2) == 7.38));
        }
    }
}