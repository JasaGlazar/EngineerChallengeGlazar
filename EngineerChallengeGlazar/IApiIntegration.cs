using Databox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    public interface IApiIntegration
    {
        public Task<string> GetData();
        public Task<ApiMetrics> GetMetrics();
        public Task<List<PushData>> CreatePushData();

    }
}
