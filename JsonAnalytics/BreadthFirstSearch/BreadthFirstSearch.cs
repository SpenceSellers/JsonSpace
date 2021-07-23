using System;
using System.Collections.Generic;
using System.Linq;
using JsonAnalytics.Parsing;

namespace JsonAnalytics
{
    public class BreadthFirstSearch
    {
        private readonly SearchConfig _config;

        public BreadthFirstSearch(SearchConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Performs a breadth-first search across the space of all possible JSON strings.
        /// </summary>
        public IEnumerable<BfsNode> Bfs()
        {
            var initialParser = new RootParser().ReadAll(_config.InitialState);
            var initialStates = initialParser
                .AcceptableStructuralChars()
                .Select(c => new BfsNode
            {
                Json = new[] {c},
                Parser = initialParser.Read(c)
            });
            var queue = new Queue<BfsNode>();
            QueueAll(queue, initialStates);

            var count = 0;

            while (queue.Any())
            {
                count++;
                var next = queue.Dequeue();

                // Is this one of the winning states we're looking for?
                if (_config.IsSuccessState(next))
                {
                    yield return next;
                }

                // Don't queue if the current node can never lead to a solution node
                if (_config.CanLeadToSuccessState(next))
                {
                    var nextStates = NextStates(next).ToList();
                    QueueAll(queue, nextStates);
                }
            }

            Console.Out.WriteLine($"Searched states: {count}");
        }

        public class BfsNode
        {
            public StructuralChar[] Json;
            public JsonParser Parser;
        }

        private IEnumerable<BfsNode> NextStates(BfsNode node)
        {
            return node.Parser.AcceptableStructuralChars().Select(c =>
            {
                var newJson = new StructuralChar[node.Json.Length + 1];
                node.Json.CopyTo(newJson, 0);
                newJson[node.Json.Length] = c;
                return new BfsNode
                {
                    Json = newJson,
                    Parser = node.Parser.Read(c)
                };
            });
        }

        private static void QueueAll<T>(Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

    }
}