using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerChallengeGlazar
{
    internal class APIFootballMetrics
    {
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Fouls_Committed { get; set; }
        public int Appearences { get; set; }
        public int Season {  get; set; }

        public APIFootballMetrics() { }
    }
}
