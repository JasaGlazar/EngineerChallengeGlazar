namespace EngineerChallengeGlazar
{
    internal class Program
    {
        private static HttpClient _httpClient = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //API-Football: Player ID 522, League Id 78, Season 2020-2022
            var ApiFootball = new APIFootballIntegration(_httpClient, "522","78","2020");

            //Task<string> task = ApiFootball.GetData();
            //Console.WriteLine(task.Result);

            var footballmetrics = ApiFootball.GetMetrics();
        }
    }
}
