using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            // for (var n = 0; n < 20; n++)
            // {
            //     var s = "[";
            //     JsonParser parser = new RootParser().Read('[');
            //     for (var i = 0; i < 250; i++)
            //     {
            //         // var nextChar = RandomElement(parser.AcceptableChars());
            //         var nextChars = parser.AcceptableChars().ToList();
            //         if (!nextChars.Any())
            //         {
            //             break;
            //         }
            //
            //         var next = RandomElement(nextChars);
            //         s += next;
            //         parser = parser.Read(next);
            //     }
            //     
            //     Console.Out.WriteLine(s);
            // }

            BigInteger combinations = 0;
            var structrualSolutions = 0;

            foreach (var solution in new JsonHandler().Bfs())
            {
                structrualSolutions++;
                combinations += Structure.Combinations(solution.Json);
                Console.Out.WriteLine(Structure.StringRepr(solution.Json));
            }
            
            Console.Out.WriteLine($"Combinations: {combinations}");
            Console.Out.WriteLine($"Structural Solutions: {structrualSolutions}");
        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }
    }
}