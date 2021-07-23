using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using NUnit.Framework;

namespace JsonAnalytics.Tests
{
    public class BfsTests
    {
        [TestCase(1, 10)]
        [TestCase(2, 123)]
        public void ShouldCorrectlyCalculateSomeKnownCounts(long length, long expectedCount)
        {
            var config = new SearchConfig
            {
                IsSuccessState = node => node.Json.Length == length && node.Parser.CanBeTheEndOfInput,
                CanLeadToSuccessState = node => node.Json.Length < length
            };
            var results = new JsonHandler().Bfs(config);

            BigInteger combinations = 0;
            foreach (var solution in results)
            {
                combinations += Structure.Combinations(solution.Json);
            }

            combinations.Should().Be(expectedCount);
        }

        [Test, Timeout(1000)]
        public void ShouldReachNarrowStateQuickly()
        {
            var config = new SearchConfig
            {
                IsSuccessState = node => node.Parser.CanBeTheEndOfInput
                                         && node.Json.All(sc => sc is StructuralChar.ArrayBegin or StructuralChar.ArrayEnd),
                CanLeadToSuccessState = node => node.Json.All(sc => sc is StructuralChar.ArrayBegin or StructuralChar.ArrayEnd)
            };
            var hundredthResult = new JsonHandler().Bfs(config).Skip(99).First();

            // The 1st shortest JSON string of all array characters is [], length 1
            // 2nd is [[]], length 4
            // Therefore, the 100th result should look like:
            hundredthResult.Json.Should().HaveCount(200);
            hundredthResult.Json[..100].All(sc => sc == StructuralChar.ArrayBegin).Should().BeTrue();
            hundredthResult.Json[100..].All(sc => sc == StructuralChar.ArrayEnd).Should().BeTrue();
        }

        [Test]
        public void ShouldBeAbleToCompleteExistingState()
        {
            var config = new SearchConfig
            {
                InitialState = new List<StructuralChar>
                    {StructuralChar.ArrayBegin, StructuralChar.ArrayBegin, StructuralChar.LeadingNegative},
                IsSuccessState = node => node.Parser.CanBeTheEndOfInput
            };

            var shortestResult = new JsonHandler().Bfs(config).First();
            // The shortest way to complete this is to spam out a single digit and then close the arrays as fast as possible
            shortestResult.Json.Should().Equal(
                StructuralChar.LeadingIntegerDigit,
                StructuralChar.ArrayEnd,
                StructuralChar.ArrayEnd
            );
        }
    }
}