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
        public APIFootballResponse(string data, DateTime fetchedAt, bool isSuccess, string? errorMessage, int numberKPI) : base(data, fetchedAt, isSuccess, errorMessage, numberKPI)
        {
        }
    }
}
