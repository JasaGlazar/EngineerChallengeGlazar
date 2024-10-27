using EngineerChallengeGlazar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    [TestClass]
    public class APIGitHubIntegrationTests
    {

        [TestInitialize]
        private static APIGitHubIntegration CreateAPIGitHubIntegrationInstance()
        {
            return new APIGitHubIntegration("JasaGlazar", "Seminarska_naloga");
        }

        [TestMethod]
        public async Task GetMetrics_ShouldReturnValidMetrics_WhenResponseIsValid()
        {
            var integration = CreateAPIGitHubIntegrationInstance();
            var metrics = (APIGithubMetrics) await integration.GetMetrics();

            Assert.AreEqual(2, metrics.Open_Issues);
            Assert.AreEqual(1, metrics.Stargazers);
            Assert.AreEqual(1, metrics.Watchers);

        }

        [TestMethod]
        public async Task CreatePushData_ShouldReturnCorrectPushData()
        {
            var integration = CreateAPIGitHubIntegrationInstance();
            var pushData = await integration.CreatePushData();

            Assert.AreEqual(3, pushData.Count);
            Assert.IsTrue(pushData.Exists(p => p.Key == "Open_Issues" && (int)p.Value == 2));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Stargazers" && (int)p.Value == 1));
            Assert.IsTrue(pushData.Exists(p => p.Key == "Watchers" && (int)p.Value == 1));

        }
    }
}
