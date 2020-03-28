using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {

            var watch = new Stopwatch();
            BigInteger combinations = 0;
            var structrualSolutions = 0;
            
            watch.Start();
            foreach (var solution in new JsonHandler().Bfs())
            {
                structrualSolutions++;
                combinations += Structure.Combinations(solution.Json);
                // Console.Out.WriteLine(Structure.StringRepr(solution.Json));
            }
            watch.Stop();
            
            Console.Out.WriteLine($"Combinations: {combinations}");
            Console.Out.WriteLine($"Structural Solutions: {structrualSolutions}");
            var elapsed = watch.ElapsedMilliseconds;
            Console.Out.WriteLine($"Milliseconds: {elapsed}");
        }

    }
}