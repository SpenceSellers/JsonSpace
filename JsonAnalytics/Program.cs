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
                // We're looking for JSON strings that are both of our target length and are COMPLETE valid JSON strings.
                IsSuccessState = node => node.Json.Length == targetLength && node.Parser.CanBeTheEndOfInput,

                // Unfortunately, basically any fragment of a JSON string could lead to our success state. Lists can be capped off,
                // strings can be terminated, etc. It'd be interesting to detect that we're "too deeply nested" and abandon the search though.
                // For example, "[[" will never be the beginning of a valid length-of-3 JSON string.
                CanLeadToSuccessState = node => node.Json.Length < targetLength
            };

            // Do a breadth first search across the space of all possible JSON strings.
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