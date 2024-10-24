using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    public class APIFootballMetrics : ApiMetrics
    {
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Fouls_committed { get; set; }
        public int Appearences { get; set; }
        public int Season {  get; set; }
        public double Rating { get; set; }


        public APIFootballMetrics() { }
    }
}
