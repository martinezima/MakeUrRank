using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Convert.ToBoolean(0))
            {
                for (int i = 0; i < 2; i++)
                {
                    ShowRandomNumbers();
                }
            }

            List<string> original = new List<string> { "1", "2", "3", "4" };
            List<string> copy = new List<string>(original);

            copy[0] = "cambio";

            for (int i = 0; i < original.Count; i++)
            {
                Console.WriteLine("Element {0}, value: {1}",i,original[i]);
                Console.WriteLine("Element {0}, value: {1}", i, copy[i]);
                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        private static void ShowRandomNumbers()
        {
            string[] values = new string[] { "brazil", "mexico", "croacia", "dinamarca", "españa", "francia", "grecia", "holanda" };
            
            var valuesList = values.ToList();
            var iterations = valuesList.Count;
            var rand = new System.Random(DateTime.Now.Millisecond);

            for (int i = 0; i < iterations; i++)
            {
                var choice = rand.Next(iterations -i);
                Console.WriteLine(valuesList[choice]);
                valuesList.RemoveAt(choice);
                valuesList.Reverse();
            }
        }
    }
}
