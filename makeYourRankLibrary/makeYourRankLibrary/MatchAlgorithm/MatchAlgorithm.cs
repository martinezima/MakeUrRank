using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYourRankLibrary.MatchAlgorithm
{
    public abstract class MatchAlgorithm
    {
        #region Properties & Fields

        public List<BagPlayers> PlayerCalendar { get; set; }
        public List<Match> Matches { get; set; }
        public bool IsRoundTrip { get; set; }
        public int _iterations;
        private bool IsLocal { get; set; }
        private bool ThrowCoin { get; set; }
        private List<ByeWeek> byeWeekContenders;

        public List<ByeWeek> ByeWeekContenders 
        { 
            get
            {
                if (this.byeWeekContenders == null)
                    this.byeWeekContenders = new List<ByeWeek>();

                return this.byeWeekContenders;
            } 
        }
        public int MatchCounter { get; set; }
        public int Iterations
        {
            get
            {
                if (_iterations == 0)
                {
                    _iterations = this.IsRoundTrip ? (this.PlayerCalendar.Count - 1) * 2 : (this.PlayerCalendar.Count - 1);
                }
                return _iterations;
            }
        }

        public int NumOfPlayers 
        {
            get
            {
                return this.PlayerCalendar == null ? 0 : this.PlayerCalendar.Count;
            }
        }

        public int GamesPerWeek
        {
            get
            {
                return this.PlayerCalendar.Count/2;
            }
        }

        public bool DoesNeedByeWeek
        {
            get
            {
                return Convert.ToBoolean(this.PlayerCalendar.Count % 2);
            }
        }

        #endregion

        public MatchAlgorithm()
        {
            this.Matches = new List<Match>();
        }

        public abstract void StartProcess();

        #region Methods

        /// <summary>
        /// Fill PlayerCalendar object From Dictionary
        /// </summary>
        /// <param name="_gameCalendar">Dictionary<string, string></param>
        public virtual void FillFromDictionary(Dictionary<string, string> _gameCalendar)
        {
            if (_gameCalendar != null)
            {
                this.PlayerCalendar = new List<BagPlayers>();
                foreach (var item in _gameCalendar)
                {
                    var player = new BagPlayers();

                    player.OwnerKey = item.Key;
                    player.PlayerId = item.Value;
                    this.PlayerCalendar.Add(player);
                }
            }
        }

        /// <summary>
        /// Review if the Match is valid or does not exist yet, in case it does, return the match!
        /// </summary>
        /// <param name="match">Match match</param>
        /// <returns>Match</returns>
        public virtual bool IsExistingMatch(Match match)
        {
            return this.Matches.Where(m => m.Contender1 == match.Contender2 && 
                        m.Contender2 == match.Contender1).Count() == 0 ? false : true;
        }

        /// <summary>
        /// Set local or visitor
        /// </summary>
        public virtual void SetLocalOrVisitor(Match match, int week)
        {

            var o1WasLocalPrevious = this.Matches.Exists(m => m.Contender1 == match.Contender1 && 
                m.Week == week);
            var o2WasLocalPrevious = this.Matches.Exists(m => m.Contender1 == match.Contender2 &&
                m.Week == week);
            var contenders = new List<string>() { match.Contender1, match.Contender2 };

            if (o1WasLocalPrevious == o2WasLocalPrevious)
            {
                var rand = new System.Random(DateTime.Now.Millisecond);
                var choice = rand.Next(2);
                match.Contender1 = contenders[choice];
                contenders.RemoveAt(choice);
                match.Contender2 = contenders[0];
            }
            else if(o1WasLocalPrevious)
            {
                match.Contender1 = contenders[1];
                match.Contender2 = contenders[0];
            }
        }

        /// <summary>
        /// Make the calendar
        /// </summary>
        public virtual void MakeCalendar()
        {
            var tempMatches = new List<Match>(this.Matches);               
            var numberOfMatches = this.Matches.Count;
            var weeks = this.Matches.Count / this.GamesPerWeek;

            //Clear matches to put them organized
            this.Matches.Clear();  
            var byeWeekBag = this.PlayerCalendar.Select(player => player.PlayerId).ToList();
            

            var rand = new System.Random(DateTime.Now.Millisecond);
            var choice = 0;
            var byeContender = string.Empty;
            for (int week = 1; week <= weeks; week++)
            {
                var remainingBag = this.PlayerCalendar.Select(player => player.PlayerId).ToList();
                if (this.DoesNeedByeWeek)
                {
                    //Set bye week
                    choice = rand.Next(byeWeekBag.Count - 1);
                    byeContender = byeWeekBag[choice];
                    this.ByeWeekContenders.Add(new ByeWeek() { Contender = byeContender, Week = week });
                    byeWeekBag.RemoveAt(choice);
                    remainingBag.RemoveAt(choice);
                }


                var matchesToEvaluate = new List<Match>(tempMatches);

                matchesToEvaluate.RemoveAll(m => m.Contender1 == byeContender);
                matchesToEvaluate.RemoveAll(m => m.Contender2 == byeContender);

                while (!(this.Matches.Count == (this.GamesPerWeek * week)))
                {
                    if (matchesToEvaluate.Count == 0)
                    {
                        matchesToEvaluate = new List<Match>(tempMatches);
                        matchesToEvaluate.RemoveAll(m => m.Contender1 == byeContender);
                        matchesToEvaluate.RemoveAll(m => m.Contender2 == byeContender);

                        matchesToEvaluate.RemoveAll(m => !remainingBag.Contains(m.Contender1));
                        matchesToEvaluate.RemoveAll(m => !remainingBag.Contains(m.Contender2));

                        if (matchesToEvaluate.Count == 0)
                        {
                            if (this.DoesNeedByeWeek) 
                            { 
                                //update bye contenders
                                this.ByeWeekContenders.RemoveAt(this.ByeWeekContenders.Count - 1);
                                byeWeekBag.Remove(byeContender);
                                choice = rand.Next(byeWeekBag.Count - 1);
                                var tempByContender = byeWeekBag[choice];
                                this.ByeWeekContenders.Add(new ByeWeek() { Contender = tempByContender, Week = week });
                                byeWeekBag.Add(byeContender);
                                byeWeekBag.Remove(tempByContender);
                                byeContender = tempByContender;
                                tempMatches.AddRange(this.Matches.Where(m => m.Week == week));
                                this.Matches.RemoveAll(m => m.Week == week);
                            
                                remainingBag = this.PlayerCalendar.Select(player => player.PlayerId).ToList();
                                remainingBag.Remove(byeContender);
                                //start again
                                matchesToEvaluate = new List<Match>(tempMatches);

                                matchesToEvaluate.RemoveAll(m => m.Contender1 == byeContender);
                                matchesToEvaluate.RemoveAll(m => m.Contender2 == byeContender);
                            }
                            else
                            {                                
                                matchesToEvaluate = new List<Match>(tempMatches);
                            }
                        }
                    }


                    var choiceMatch = rand.Next(matchesToEvaluate.Count - 1);
                    var match = matchesToEvaluate[choiceMatch];
                    matchesToEvaluate.Remove(match);
                    this.MatchCounter++;

                    match.Week = week;
                    this.SetLocalOrVisitor(match, week);
                    this.Matches.Add(match);
                    tempMatches.Remove(tempMatches.SingleOrDefault(m => m.Index == match.Index));



                    remainingBag.Remove(match.Contender1);
                    remainingBag.Remove(match.Contender2);                        

                    if (week < weeks)
                    {
                        matchesToEvaluate.RemoveAll(m => m.Contender1 == match.Contender1);
                        matchesToEvaluate.RemoveAll(m => m.Contender2 == match.Contender1);
                        matchesToEvaluate.RemoveAll(m => m.Contender1 == match.Contender2);
                        matchesToEvaluate.RemoveAll(m => m.Contender2 == match.Contender2);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                }                
            }

        }

        private bool CheckMatch(Match match, List<Match> tempMatches, int week, string byeContender)
        {
            var isValidMatch = false;

            if (this.Matches.Count > 0)
            {
                var compare =
                    this.Matches.Where(m => m.Week == week &&
                    (m.Contender1 == match.Contender1 ||
                    m.Contender2 == match.Contender2 ||
                    m.Contender1 == match.Contender2 ||
                    m.Contender2 == match.Contender1));


                if (compare.Count() != 0)
                {
                    isValidMatch = false;
                    if (this.MatchCounter > 5000)
                    {
                        isValidMatch = true;
                    }
                }
                else
                    isValidMatch = true;
            }
            else
                isValidMatch = true;



            return isValidMatch;
        }

        #endregion
    }
}
