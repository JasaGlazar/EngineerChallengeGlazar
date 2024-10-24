using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    internal class APIFootballResponse : ApiResponse
    {
        public readonly string Service_Provider = "API-Football";
        public APIFootballResponse(DateTime fetchedAt, bool isSuccess, string? errorMessage, int numberKPI) : base(fetchedAt, isSuccess, errorMessage, numberKPI)
        {
        }
    }
}
