namespace EngineerChallengeGlazar
{
    internal class Program
    {
        private static HttpClient _httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //API-Football: Player ID 522, League Id 78, Season 2020-2022 (Free version of API only gives data from Seasons 2020-2022)
            var ApiFootball = new APIFootballIntegration(_httpClient, "522","78","2022");
            var DataboxClient = new DataboxIntegration(_httpClient, "7396e48d938f48c29764367c0e696fa3");
            await DataboxClient.SendMetrics(ApiFootball);
        }

    }
}
