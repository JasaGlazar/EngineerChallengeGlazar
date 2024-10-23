using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    internal interface IApiIntegration
    {
        public Task<string> GetData();
        //public ApiMetrics GetMetrics();

    }
}
