using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makeYourRankLibrary.MatchAlgorithm
{
    public class BasicAlgorithm : MatchAlgorithm
    {

        
        #region Constructors
        
        /// <summary>
        /// BasicAlgorithm constructor
        /// </summary>
        /// <param name="_players">List<BagPlayers></param>
        /// <param name="_isRoundTrip">bool</param>
        public BasicAlgorithm(List<BagPlayers> _players, bool _isRoundTrip)
        {
            base.PlayerCalendar = _players;
            base.IsRoundTrip = _isRoundTrip;
            this.StartProcess();
        }

        /// <summary>
        /// BasicAlgorithm constructor
        /// </summary>
        /// <param name="_players">Dictionary<string, string></param>
        /// <param name="_isRoundTrip">bool</param>
        public BasicAlgorithm(Dictionary<string, string> _players, bool _isRoundTrip)
        {
            base.FillFromDictionary(_players);
            base.IsRoundTrip = _isRoundTrip;
            this.StartProcess();
        }
        
        #endregion

        #region Main Process

        public override void StartProcess()
        {
            var players = base.PlayerCalendar.Select(player => player.PlayerId).ToList();

            foreach (var player in base.PlayerCalendar)
            {
                var oponents = makeYourRankLibrary.Utilities.CloneListExtension.Clone(players);
                oponents.Remove(player.PlayerId);

                //The Core of this algorith, a random number will be generated in order to get the oponent
                var rand = new System.Random(DateTime.Now.Millisecond);
                var iteration = this.Iterations;
                for (int i = 0; i < this.Iterations; i++)
                {
                    iteration--;
                    var choice = rand.Next(iteration);
                    var match = new Match { Contender1 = player.PlayerId, Contender2 = oponents[choice]};
                    //player.Oponents.Add(match);
                    if (!IsExistingMatch(match))
                        base.Matches.Add(match);

                    //Asking Lexnav how change the order of items with .net framework or custom routine
                    oponents.RemoveAt(choice);
                    oponents.Reverse();
                }
            }

            base.MakeCalendar();
        }

        #endregion

        #region Methods
        


        #endregion



    }
}
