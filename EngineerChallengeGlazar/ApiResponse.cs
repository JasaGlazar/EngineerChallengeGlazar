using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    internal class ApiResponse
    {
        public DateTime FetchedAt { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int NumberKPI { get; set; }

        public ApiResponse(DateTime fetchedAt, bool isSuccess, string? errorMessage, int numberKPI)
        {
            FetchedAt = fetchedAt;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            NumberKPI = numberKPI;
        }
    }
}
