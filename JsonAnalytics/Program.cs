using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace JsonAnalytics
{
    class Program
    {
        private const int PossibleCharacters = 95;

        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            BigInteger combinations = 0;
            var structuralSolutions = 0;

            watch.Start();

            var targetLength = 5;
            var config = new SearchConfig
            {
                IsSuccessState = node => node.Json.Length == targetLength && node.Parser.CanBeTheEndOfInput,
                CanLeadToSuccessState = node => node.Json.Length < targetLength
            };
            foreach (var solution in new JsonHandler().Bfs(config))
            {
                structuralSolutions++;
                combinations += Structure.Combinations(solution.Json);
                // Console.Out.WriteLine(Structure.StringRepr(solution.Json));
            }
            watch.Stop();

            Console.Out.WriteLine($"Combinations: {combinations}");
            var outOf = BigInteger.Pow(PossibleCharacters, targetLength);
            Console.Out.WriteLine($"Out of {outOf}");
            Console.Out.WriteLine($"{100.0  / (double) (outOf / combinations)}");
            Console.Out.WriteLine($"Structural Solutions: {structuralSolutions}");
            var elapsed = watch.ElapsedMilliseconds;
            Console.Out.WriteLine($"Milliseconds: {elapsed}");
        }
    }
}