namespace EngineerChallengeGlazar
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //API-Football: Player ID 522, League Id 78, Season 2020-2022 (Free version of API only gives data from Seasons 2020-2022)
            //var ApiFootball = new APIFootballIntegration("522","78","2022");
            //var DataboxClient = new DataboxIntegration("7396e48d938f48c29764367c0e696fa3");
            //await DataboxClient.SendMetrics(ApiFootball);

            var GitHubApi = new APIGitHubIntegration("JasaGlazar", "Seminarska_naloga");
            var DataBoxClient1 = new DataboxIntegration("0542d6772a60454c9a99754089e3d3b4");

            await DataBoxClient1.SendMetrics(GitHubApi);
        }

    }
}
