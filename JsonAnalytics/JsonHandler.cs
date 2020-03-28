using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    public class JsonHandler
    {
        public bool IsValidJson(string json)
        {
            JsonParser parser = new RootParser();
            foreach (var c in json)
            {
                if (!parser.AcceptableChars().Contains(c))
                {
                    return false;
                }
                
                parser = parser.Read(c);
            }

            return parser.CanBeTheEndOfInput;
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

        public IEnumerable<BfsNode> Bfs()
        {
            var initialParser = new RootParser();
            var initialStates = initialParser.AcceptableStructuralChars().Select(c => new BfsNode
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
                if (next.Json.Length == 3 && next.Parser.CanBeTheEndOfInput)
                {
                    yield return next;
                }
                if (next.Json.Length > 3)
                {
                    break;
                }
                
                var nextStates = NextStates(next);
                QueueAll(queue, nextStates); // TODO: Filter nextStates for seen before (via hash?)
            }
            
            Console.Out.WriteLine($"Searched states: {count}");
        }
    }
}