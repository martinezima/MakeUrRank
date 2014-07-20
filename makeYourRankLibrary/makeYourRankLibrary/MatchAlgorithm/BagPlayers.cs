using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makeYourRankLibrary.MatchAlgorithm
{
    public class BagPlayers
    {
        public string OwnerKey { get; set; }
        public string PlayerId { get; set; }
        public List<Match> Contenders { get; set; }

        public BagPlayers()
        {
            this.Contenders = new List<Match>();
        }
    }

    public struct Match
    {
        public int Week { get; set; }
        public string Contender1 { get; set; }
        public string Contender2 { get; set; }
    }

    public struct ByeWeek
    {
        public int Week { get; set; }
        public string Contender { get; set; }
    }

}
