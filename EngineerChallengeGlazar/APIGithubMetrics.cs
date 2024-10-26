using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    class APIGithubMetrics : ApiMetrics
    {
        public int Open_Issues { get; set; }
        public int Watchers { get; set; }
        public int Stargazers { get; set; }

        public APIGithubMetrics() { }


    }
}
