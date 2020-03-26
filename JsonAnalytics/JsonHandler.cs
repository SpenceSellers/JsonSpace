using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    public class JsonHandler
    {
        public bool IsValidJson(string json)
        {
            JsonParser parser = new ValueParser();
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

        private class BfsNode
        {
            public string Json;
            public JsonParser Parser;
        }

        private IEnumerable<BfsNode> NextStates(BfsNode node)
        {
            return node.Parser.AcceptableChars().Select(c => new BfsNode
            {
                Json = node.Json + c,
                Parser = node.Parser.Read(c)
            });
        }

        private static void QueueAll<T>(Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        public void Bfs()
        {
            var initialParser = new ValueParser();
            var initialStates = initialParser.AcceptableChars().Select(c => new BfsNode
            {
                Json = c.ToString(),
                Parser = initialParser.Read(c)
            });
            var queue = new Queue<BfsNode>();
            QueueAll(queue, initialStates);

            while (queue.Any())
            {
                var next = queue.Dequeue();
                if (next.Parser.CanBeTheEndOfInput)
                {
                    Console.Out.WriteLine(next.Json);                    
                }
                if (next.Json.Length > 40)
                {
                    return;
                }
                
                var nextStates = NextStates(next);
                QueueAll(queue, nextStates); // TODO: Filter nextStates for seen before (via hash?)
            }
        }
    }
}