using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makeYourRankLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            var bagPlayers = new List<MatchAlgorithm.BagPlayers>();
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Mike"});
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Alex Nava" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Martinez" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Virgilio" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Tiki Tiki" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Balder" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Oscarito" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Julio" });
            bagPlayers.Add(new MatchAlgorithm.BagPlayers { PlayerId = "Gauna Jr" });
            makeYourRankLibrary.MatchAlgorithm.BasicAlgorithm basic =
                new MatchAlgorithm.BasicAlgorithm(bagPlayers, false);

            var weeks = basic.Matches.Count / basic.GamesPerWeek;

            Console.WriteLine("Calendario ASPL 5ta Edicion");
            Console.WriteLine("*********************************************************");
            Console.WriteLine("");
            Console.WriteLine("");
            for (int i = 1; i <= weeks; i++)
            {
                Console.WriteLine("Jornada {0}",i);
                Console.WriteLine("*********************************************************");
                Console.WriteLine("");
                var matches = basic.Matches.Where(m => m.Week == i);
                foreach (var match in matches)
                {
                    Console.WriteLine("{0} - {1} ", match.Contender1, match.Contender2);
                }
                if (false)
                {
                    Console.WriteLine("Descansa: {0}",
                        basic.ByeWeekContenders.First(b => b.Week == i).Contender);
                } 

                Console.WriteLine("");
                Console.WriteLine("*********************************************************");
                
            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("*********************************************************");
            Console.ReadLine();
        }
    }
}
